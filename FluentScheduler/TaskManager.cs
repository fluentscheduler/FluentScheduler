using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using FluentScheduler.Model;

namespace FluentScheduler
{
	/// <summary>
	/// Controls the timer logic to execute all configured tasks.
	/// </summary>
	public static class TaskManager
	{
		public static event GenericEventHandler<Task, UnhandledExceptionEventArgs> UnobservedTaskException;
		public static event GenericEventHandler<TaskStartScheduleInformation, EventArgs> TaskStart;
		public static event GenericEventHandler<TaskEndScheduleInformation, EventArgs> TaskEnd;

		private static List<Schedule> _tasks;
		private static Timer _timer;
		private static readonly ConcurrentDictionary<List<Action>, bool> RunningNonReentrantTasks = new ConcurrentDictionary<List<Action>, bool>();
		private static readonly ConcurrentDictionary<Guid, Schedule> _runningSchedules = new ConcurrentDictionary<Guid, Schedule>();
		/// <summary>
		/// Gets a list of currently schedules currently executing.
		/// </summary>
		public static Schedule[] RunningSchedules
		{
			get
			{
				return _runningSchedules.Values.ToArray();
			}
		}

		/// <summary>
		/// The list of all schedules, whether or not they are currently running.
		/// Use <see cref="GetSchedule"/> to get concrete schedule by name.
		/// </summary>
		public static Schedule[] AllSchedules
		{
			get
			{
				return _tasks.ToArray();
			}
		}
		/// <summary>
		/// Get schedule by name.
		/// </summary>
		/// <param name="name">Schedule name</param>
		/// <returns>Schedule instance or null if the schedule does not exist</returns>
		public static Schedule GetSchedule(string name)
		{
			if (_tasks != null)
			{
				return _tasks.FirstOrDefault(x => x.Name == name);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Initializes the task manager with all schedules configured in the specified registry
		/// </summary>
		/// <param name="registry">Registry containing task schedules</param>
		public static void Initialize(Registry registry)
		{
			var immediateTasks = new List<Schedule>();
			lock (typeof(TaskManager))
			{
				var now = DateTime.Now;
				_tasks = new List<Schedule>();

				AddSchedules(registry.Schedules, immediateTasks, now);
			}

			RunAndInitializeSchedule(immediateTasks);
		}

		private static void RaiseUnobservedTaskException(Task t)
		{
			var handler = UnobservedTaskException;
			if (handler != null && t.Exception != null)
				handler(t, new UnhandledExceptionEventArgs(t.Exception.InnerException, true));
		}
		private static void RaiseTaskStart(Schedule schedule, DateTime startTime)
		{
			var handler = TaskStart;
			if (handler != null)
			{
				var info = new TaskStartScheduleInformation
					{
						Name = schedule.Name,
						StartTime = startTime
					};
				handler(info, new EventArgs());
			}
		}
		private static void RaiseTaskEnd(Schedule schedule, DateTime startTime, TimeSpan duration)
		{
			var handler = TaskEnd;
			if (handler != null)
			{
				var info = new TaskEndScheduleInformation
					{
						Name = schedule.Name,
						StartTime = startTime,
						Duration = duration
					};
				if (schedule.NextRunTime != default(DateTime))
					info.NextRunTime = schedule.NextRunTime;

				handler(info, new EventArgs());
			}
		}

		private static void AddSchedules(IEnumerable<Schedule> schedules, ICollection<Schedule> immediatelyInvokedSchedules, DateTime now)
		{
			foreach (var schedule in schedules)
			{
				if (schedule.CalculateNextRun == null)
				{
					immediatelyInvokedSchedules.Add(schedule);
					var hasAdded = false;
					foreach (var child in schedule.AdditionalSchedules.Where(x => x.CalculateNextRun != null))
					{
						var nextRun = child.CalculateNextRun(now.AddMilliseconds(1));
						if (!hasAdded || schedule.NextRunTime > nextRun)
						{
							schedule.NextRunTime = nextRun;
							hasAdded = true;
						}
					}
				}
				else
				{
					schedule.NextRunTime = schedule.CalculateNextRun(now);
					_tasks.Add(schedule);
				}

				foreach (var childSchedule in schedule.AdditionalSchedules)
				{
					if (childSchedule.CalculateNextRun == null)
					{
						immediatelyInvokedSchedules.Add(childSchedule);
						continue;
					}
					childSchedule.NextRunTime = childSchedule.CalculateNextRun(now);
					_tasks.Add(childSchedule);
				}
			}
		}

		private static void RunAndInitializeSchedule(IEnumerable<Schedule> immediatelyInvokedSchedules)
		{
			foreach (var task in immediatelyInvokedSchedules)
			{
				StartTask(task);
			}

			if (!_tasks.Any())
				return;

			if (_timer == null)
			{
				_timer = new Timer { AutoReset = false };
				_timer.Elapsed += Timer_Elapsed;
			}
			_tasks.Sort((x, y) => DateTime.Compare(x.NextRunTime, y.NextRunTime));
			Schedule();
		}

		internal static void StartTask(Schedule schedule)
		{
			if (!schedule.Reentrant)
			{
				if (!RunningNonReentrantTasks.TryAdd(schedule.Tasks, true))
					return;
			}

			var id = Guid.NewGuid();
			_runningSchedules.TryAdd(id, schedule);

			var start = DateTime.Now;
			RaiseTaskStart(schedule, start);
			var mainTask = Task.Factory.StartNew(() =>
			{
				var stopwatch = new Stopwatch();
				try
				{
					stopwatch.Start();
					foreach (var action in schedule.Tasks)
					{
						var subTask = Task.Factory.StartNew(action);
						subTask.Wait();
					}
				}
				finally
				{
					stopwatch.Stop();
					bool notUsed;
					RunningNonReentrantTasks.TryRemove(schedule.Tasks, out notUsed);
					Schedule notUsedSchedule;
					_runningSchedules.TryRemove(id, out notUsedSchedule);
					RaiseTaskEnd(schedule, start, stopwatch.Elapsed);
				}
			}, TaskCreationOptions.PreferFairness);
			mainTask.ContinueWith(RaiseUnobservedTaskException, TaskContinuationOptions.OnlyOnFaulted);
		}

		/// <summary>
		/// Adds a task to the task manager
		/// </summary>
		/// <typeparam name="T">Task to schedule</typeparam>
		/// <param name="taskSchedule">Schedule for the task</param>
		public static void AddTask<T>(Action<Schedule> taskSchedule) where T : ITask, new()
		{
			if (taskSchedule == null)
				throw new ArgumentNullException("taskSchedule", "Please specify the task schedule to add to the task manager.");

			var schedule = new Schedule(new T());
			AddTask(taskSchedule, schedule);
		}

		/// <summary>
		/// Adds a task to the task manager
		/// </summary>
		/// <param name="taskAction">Task to schedule</param>
		/// <param name="taskSchedule">Schedule for the task</param>
		public static void AddTask(Action taskAction, Action<Schedule> taskSchedule)
		{
			if (taskSchedule == null)
				throw new ArgumentNullException("taskSchedule", "Please specify the task schedule to add to the task manager.");

			var schedule = new Schedule(taskAction);
			AddTask(taskSchedule, schedule);
		}

		private static void AddTask(Action<Schedule> taskSchedule, Schedule schedule)
		{
			taskSchedule(schedule);

			var immediateTasks = new List<Schedule>();
			lock (typeof(TaskManager))
			{
				var now = DateTime.Now;
				if (_tasks == null)
					_tasks = new List<Schedule>();

				AddSchedules(new List<Schedule> { schedule }, immediateTasks, now);
			}

			RunAndInitializeSchedule(immediateTasks);
		}

		/// <summary>
		/// Stops the task manager from executing tasks.
		/// </summary>
		public static void Stop()
		{
			if (_timer != null)
				_timer.Stop();
		}

		/// <summary>
		/// Restarts the task manager if it had previously been stopped
		/// </summary>
		public static void Start()
		{
			if (_timer != null)
				Schedule();
		}

		static void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Schedule();
		}

		private static void Schedule()
		{
			_timer.Stop();

			var firstTask = _tasks.First();
			if (firstTask.NextRunTime <= DateTime.Now)
			{
				StartTask(firstTask);
				firstTask.NextRunTime = firstTask.CalculateNextRun(DateTime.Now.AddMilliseconds(1));
				if (firstTask.TaskExecutions > 0)
				{
					firstTask.TaskExecutions--;
				}
				if (firstTask.TaskExecutions == 0)
				{
					lock (typeof(TaskManager))
					{
						_tasks.Remove(firstTask);
					}
				}
				_tasks.Sort((x, y) => DateTime.Compare(x.NextRunTime, y.NextRunTime));
				Schedule();
				return;
			}

			var timerInterval = (firstTask.NextRunTime - DateTime.Now).TotalMilliseconds;
			if (timerInterval <= 0)
			{
				Schedule();
				return;
			}
			// If the interval is greater than what the _timer supports, just go for int.MaxValue. A new interval will be calculated the next time the timer runs.
			if (timerInterval > int.MaxValue)
				timerInterval = int.MaxValue;

			_timer.Interval = timerInterval;
			_timer.Start();
		}
	}
}

using System;
using System.Collections.Generic;

namespace FluentScheduler.Model
{
	public class Schedule
	{
		public DateTime NextRunTime { get; set; }
		public string Name { get; set; }

		public bool Disabled { get; private set; }

		internal List<Action> Tasks { get; private set; }

		internal Func<DateTime, DateTime> CalculateNextRun { get; set; }

		/// <summary>
		/// The first execution of the task can be delayed by the interval defined here.
		/// It will only delay the startup (first execution).
		/// </summary>
		internal TimeSpan DelayRunFor { get; set; }
		internal ICollection<Schedule> AdditionalSchedules { get; set; }
		internal Schedule Parent { get; set; }
		internal int TaskExecutions { get; set; }

		internal bool Reentrant { get; set; }

		/// <summary>
		/// Schedules the specified task to run
		/// </summary>
		/// <param name="task">Task to run</param>
		public Schedule(ITask task)
			: this(task.Execute)
		{
		}

		/// <summary>
		/// Schedules the specified task to run
		/// </summary>
		/// <param name="action">A parameterless method to run</param>
		public Schedule(Action action)
		{
			Disabled = false;
			Tasks = new List<Action> { action };
			AdditionalSchedules = new List<Schedule>();
			TaskExecutions = -1;
			Reentrant = true;
		}

		/// <summary>
		/// Schedules the specified task to run
		/// </summary>
		/// <param name="actions">A list of parameterless methods to run</param>
		public Schedule(List<Action> actions)
		{
			Disabled = false;
			Tasks = actions;
			AdditionalSchedules = new List<Schedule>();
			TaskExecutions = -1;
			Reentrant = true;
		}

		/// <summary>
		/// Start the task now, regardless of any scheduled start time.
		/// </summary>
		public void Execute()
		{
			TaskManager.StartTask(this);
		}

		/// <summary>
		/// Schedules another task to be run with this schedule
		/// </summary>
		/// <typeparam name="T">Type of task to run</typeparam>
		/// <returns></returns>
		public Schedule AndThen<T>() where T : ITask
		{
			//If no task factory has been added to the schedule, use the default.
			var factory = TaskManager.TaskFactory ?? new TaskFactory();

			Tasks.Add(() => factory.GetTaskInstance<T>().Execute());
			return this;
		}

		/// <summary>
		/// Schedules another task to be run with this schedule
		/// </summary>
		/// <param name="action">A parameterless method to run</param>
		public Schedule AndThen(Action action)
		{
			Tasks.Add(action);
			return this;
		}

		/// <summary>
		/// Schedules another task to be run with this schedule
		/// </summary>
		/// <param name="task">An instantiated ITask.</param>
		public Schedule AndThen(ITask task)
		{
			Tasks.Add(task.Execute);
			return this;
		}

		/// <summary>
		/// Schedules the specified tasks to run now
		/// </summary>
		/// <returns></returns>
		public SpecificRunTime ToRunNow()
		{
			return new SpecificRunTime(this);
		}

		/// <summary>
		/// Schedules the specified tasks to run for the specified interval
		/// </summary>
		/// <param name="interval"></param>
		/// <returns></returns>
		public TimeUnit ToRunEvery(int interval)
		{
			return new TimeUnit(this, interval);
		}

		/// <summary>
		/// Schedules the specified tasks to run once, delayed by a specific time interval. 
		/// </summary>
		/// <param name="interval"></param>
		/// <returns></returns>
		public TimeUnit ToRunOnceIn(int interval)
		{
			TaskExecutions = 1;
			return new TimeUnit(this, interval);
		}

		/// <summary>
		/// Schedules the specified tasks to run once at the hour and minute specified.  If the hour and minute have passed, the tasks will be executed immediately.
		/// </summary>
		/// <param name="hours">0-23: Represents the hour of today</param>
		/// <param name="minutes">0-59: Represents the minute to run the task</param>
		/// <returns></returns>
		public SpecificRunTime ToRunOnceAt(int hours, int minutes)
		{
			return ToRunOnceAt(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, hours, minutes, 0));
		}

		/// <summary>
		/// Schedules the specified tasks to run once at the time specified.  If the time has passed, the task will be executed immediately.
		/// </summary>
		/// <param name="time">Time to run the task</param>
		/// <returns></returns>
		public SpecificRunTime ToRunOnceAt(DateTime time)
		{
			CalculateNextRun = x => (DelayRunFor > TimeSpan.Zero ? time.Add(DelayRunFor) : time);
			TaskExecutions = 1;

			return new SpecificRunTime(this);
		}

		/// <summary>
		/// Provide a name for this schedule
		/// </summary>
		/// <param name="name">Name of this schedule</param>
		/// <returns></returns>
		public Schedule WithName(string name)
		{
			Name = name;
			return this;
		}

		/// <summary>
		/// Will not start a new instance of the scheduler if a previous schedule is still running
		/// </summary>
		/// <returns></returns>
		public Schedule NonReentrant()
		{
			Reentrant = false;
			return this;
		}

		public void Disable()
		{
			Disabled = true;
		}


		public void Enable()
		{
			Disabled = false;
		}
	}
}

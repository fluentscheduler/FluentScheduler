using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentScheduler.Model
{
	public class Schedule
	{
		public DateTime NextRunTime { get; set; }
		public string Name { get; set; }

        internal List<Action> Tasks { get; private set; }
        internal ITaskFactory TaskFactory {get; private set;}

		internal Func<DateTime, DateTime> CalculateNextRun { get; set; }

		internal ICollection<Schedule> AdditionalSchedules { get; set; }
		internal Schedule Parent { get; set; }
		internal int TaskExecutions { get; set; }

        internal bool Concurrent { get; set; }
        internal bool Reentrant { get; set; }

		/// <summary>
		/// Schedules the specified task to run
		/// </summary>
		/// <param name="task">Task to run</param>
		public Schedule(ITask task) : this(task.Execute)
		{
            TaskFactory = new ITaskFactory();
		}

        /// <summary>
        /// Creates a specific schedule for a group of tasks
        /// </summary>
        /// <param name="factory">An instantiated ITaskFactory.</param>
        /// <param name="action">A parameterless method to run.</param>
        public Schedule(ITaskFactory factory, Action action)
        {            
            Tasks = new List<Action>();
            TaskFactory = factory;
            Tasks.Add(action);
            AdditionalSchedules = new List<Schedule>();
            TaskExecutions = -1;
            Reentrant = true;
        }
        
        /// <summary>
        /// Schedules the specified task to run
        /// </summary>
        /// <param name="action">A parameterless method to run</param>
        public Schedule(Action action)
        {
            Tasks = new List<Action>();
            Tasks.Add(action);
            AdditionalSchedules = new List<Schedule>();
            TaskExecutions = -1;
            Reentrant = true;
        }

        /// <summary>
        /// Schedules the specified task to run
        /// </summary>
        /// <param name="action">A list of parameterless methods to run</param>
        public Schedule(List<Action> actions)
        {
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
        /// <param name="task">An ITask type.</param>
        public Schedule AndThen<T>() where T : ITask
        {
            //If no task factory has been added to the schedule, use the default.
            if (TaskFactory == null)
                TaskFactory = new ITaskFactory();

            Tasks.Add(() => TaskFactory.GetTaskInstance<T>().Execute());
            return this;
        }

        /// <summary>
        /// Schedules another task to be run with this schedule
        /// </summary>
        /// <param name="task">A parameterless function to be executed.</param>
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
            Tasks.Add(() => task.Execute());
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
			CalculateNextRun = x => time;
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

        /// <summary>
        /// Allows for running multiple tasks in a schedule concurrently.
        /// </summary>
        /// <returns></returns>
        public Schedule Concurrently()
        {
            Concurrent = true;
            return this;
        }
	}
}

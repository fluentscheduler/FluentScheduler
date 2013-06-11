using System;
using System.Collections.Generic;

namespace FluentScheduler.Model
{
	public class Schedule
	{
		public DateTime NextRunTime { get; set; }
		public string Name { get; set; }

		internal Action Task { get; private set; }

		internal Func<DateTime, DateTime> CalculateNextRun { get; set; }

		internal ICollection<Schedule> AdditionalSchedules { get; set; }
		internal bool Reentrant { get; set; }
		internal Schedule Parent { get; set; }
		internal int TaskExecutions { get; set; }

		/// <summary>
		/// Schedules the specified task to run
		/// </summary>
		/// <param name="task">Task to run</param>
		public Schedule(ITask task) : this(task.Execute)
		{
		}

		/// <summary>
		/// Schedules the specified task to run
		/// </summary>
		/// <param name="action">Task to run</param>
		public Schedule(Action action)
		{
			Task = action;
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
		/// Schedules the specified task to run now
		/// </summary>
		/// <returns></returns>
		public SpecificRunTime ToRunNow()
		{
			return new SpecificRunTime(this);
		}

		/// <summary>
		/// Schedules the specified task to run for the specified interval
		/// </summary>
		/// <param name="interval"></param>
		/// <returns></returns>
		public TimeUnit ToRunEvery(int interval)
		{
			return new TimeUnit(this, interval);
		}

        /// <summary>
        /// Schedules the specified task to run for the specified interval.
        /// The specified interval is randomized between a value of (interval - 10%) 
        /// and (interval + 10%) to allow for the start time of the task to be 
        /// different for each scheduled run.
        /// <para></para>
        /// <para>*** NOTE ***</para>
        /// <para>The randomization only applies when using Seconds() and Minutes().
        /// The other time ranges (hours, days, weeks, months, years) allow for an 
        /// overridden "At()" that contradicts the intended randomness therefore, 
        /// an InvalidOperationException will be thrown.
        /// </para>
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// If you use the <see cref="ToRunAboutEvery"/> method to schedule a 
        /// random start time of the task, but use any method other than Seconds() or
        /// Minutes() to specify the amount of time you want to use when scheduling
        /// the next run time of the task.
        /// </exception>
        public TimeUnit ToRunAboutEvery(int interval)
        {
            return new TimeUnit(this, interval, true);
        }

		/// <summary>
		/// Schedules the specified task to run once at the hour and minute specified.  If the hour and minute have passed, the task will be executed immediately.
		/// </summary>
		/// <param name="hours">0-23: Represents the hour of today</param>
		/// <param name="minutes">0-59: Represents the minute to run the task</param>
		/// <returns></returns>
		public SpecificRunTime ToRunOnceAt(int hours, int minutes)
		{
			return ToRunOnceAt(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, hours, minutes, 0));
		}

		/// <summary>
		/// Schedules the specified task to run once at the time specified.  If the time has passed, the task will be executed immediately.
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
		/// Will not start a new instance of the task if a previous schedule is still running
		/// </summary>
		/// <returns></returns>
		public Schedule NonReentrant()
		{
			Reentrant = false;
			return this;
		}
	}
}

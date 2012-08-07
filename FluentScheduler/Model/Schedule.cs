using System;
using System.Collections.Generic;

namespace FluentScheduler.Model
{
	public class Schedule
	{
		internal Action Task { get; private set; }

		public Func<DateTime, DateTime> CalculateNextRun { get; internal set; }
		public DateTime NextRunTime { get; set; }

		public ICollection<Schedule> AdditionalSchedules { get; internal set; }
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
	}
}

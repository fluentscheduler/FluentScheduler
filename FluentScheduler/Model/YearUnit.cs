using System;

namespace FluentScheduler.Model
{
	public class YearUnit
	{
		internal Schedule Schedule { get; private set; }
		internal int Duration { get; private set; }

		public YearUnit(Schedule schedule, int duration)
		{
			Schedule = schedule;
			Duration = duration;
			Schedule.CalculateNextRun = x => {
				var nextRun = x.Date.AddYears(Duration);
				return (x > nextRun) ? nextRun.AddYears(Duration) : nextRun;
			};
		}

		/// <summary>
		/// Schedules the specified task to run on the day specified.  If the day has passed, the task will execute the next scheduled year.
		/// </summary>
		/// <param name="day">1-365: Represents the day of the year</param>
		/// <returns></returns>
		public YearOnDayOfYearUnit On(int day)
		{
			return new YearOnDayOfYearUnit(Schedule, Duration, day);
		}

		/// <summary>
		/// Schedules the specified task to run on the last day of the year.
		/// </summary>
		/// <returns></returns>
		public YearOnLastDayOfYearUnit OnTheLastDay()
		{
			return new YearOnLastDayOfYearUnit(Schedule, Duration);
		}
	}
}

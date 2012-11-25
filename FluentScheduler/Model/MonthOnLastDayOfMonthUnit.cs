using System;
using FluentScheduler.Extensions;

namespace FluentScheduler.Model
{
	public class MonthOnLastDayOfMonthUnit
	{
		internal Schedule Schedule { get; private set; }
		internal int Duration { get; private set; }

		public MonthOnLastDayOfMonthUnit(Schedule schedule, int duration)
		{
			Schedule = schedule;
			Duration = duration;
			Schedule.CalculateNextRun = x => {
				var nextRun = x.Date.Last();
				return (x > nextRun) ? x.Date.First().AddMonths(Duration).Last() : x.Date.Last();
			};
		}

		/// <summary>
		/// Schedules the specified task to run at the hour and minute specified.  If the hour and minute have passed, the task will execute the next scheduled month.
		/// </summary>
		/// <param name="hours">0-23: Represents the hour of the day</param>
		/// <param name="minutes">0-59: Represents the minute of the day</param>
		/// <returns></returns>
		public void At(int hours, int minutes)
		{
			Schedule.CalculateNextRun = x => {
				var nextRun = x.Date.Last().AddHours(hours).AddMinutes(minutes);
				return (x > nextRun) ? x.Date.First().AddMonths(Duration).Last().AddHours(hours).AddMinutes(minutes) : nextRun;
			};
		}
	}
}

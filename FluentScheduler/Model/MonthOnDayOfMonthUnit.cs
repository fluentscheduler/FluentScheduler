using System;
using FluentScheduler.Extensions;

namespace FluentScheduler.Model
{
	public class MonthOnDayOfMonthUnit
	{
		internal Schedule Schedule { get; private set; }
		internal int Duration { get; private set; }
		internal int DayOfMonth { get; private set; }

		public MonthOnDayOfMonthUnit(Schedule schedule, int duration, int dayOfMonth)
		{
			Schedule = schedule;
			Duration = duration;
			DayOfMonth = dayOfMonth;
			Schedule.CalculateNextRun = x => {
				var nextRun = x.Date.First().AddDays(DayOfMonth - 1);
				return (x > nextRun) ? x.Date.First().AddMonths(Duration).AddDays(DayOfMonth - 1) : nextRun;
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
				var nextRun = x.Date.First().AddDays(DayOfMonth - 1).AddHours(hours).AddMinutes(minutes);
				return (x > nextRun) ? x.Date.First().AddMonths(Duration).AddDays(DayOfMonth - 1).AddHours(hours).AddMinutes(minutes) : nextRun;
			};
		}
	}
}

using System;
using FluentScheduler.Extensions;

namespace FluentScheduler.Model
{
	public class WeeklyDayOfWeekUnit
	{
		internal Schedule Schedule { get; private set; }
		internal int Duration { get; private set; }
		internal DayOfWeek Day { get; private set; }

		public WeeklyDayOfWeekUnit(Schedule schedule, int duration, DayOfWeek day)
		{
			Schedule = schedule;
			Duration = duration;
			Day = day;
			if (Duration > 0)
			{
				Schedule.CalculateNextRun = x => {
					var nextRun = x.Date.AddDays(Duration * 7).ThisOrNext(day);
					return (x > nextRun) ? nextRun.AddDays(Duration * 7) : nextRun;
				};
			}
			else
			{
				Schedule.CalculateNextRun = x => {
					var nextRun = x.Date.Next(day);
					return (x > nextRun) ? nextRun.AddDays(7) : nextRun;
				};
			}
		}

		/// <summary>
		/// Schedules the specified task to run at the hour and minute specified.  If the hour and minute have passed, the task will execute the next scheduled week.
		/// </summary>
		/// <param name="hours">0-23: Represents the hour of the day</param>
		/// <param name="minutes">0-59: Represents the minute of the day</param>
		/// <returns></returns>
		public void At(int hours, int minutes)
		{
			Schedule.CalculateNextRun = x => {
				var nextRun = x.Date.AddDays(Duration * 7).ThisOrNext(Day).AddHours(hours).AddMinutes(minutes);
				return (x > nextRun) ? nextRun.AddDays(Math.Max(Duration, 1) * 7) : nextRun;
			};
		}
	}
}

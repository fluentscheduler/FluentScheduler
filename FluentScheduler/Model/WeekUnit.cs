using System;

namespace FluentScheduler.Model
{
	public class WeekUnit
	{
		internal Schedule Schedule { get; private set; }
		internal int Duration { get; private set; }

		public WeekUnit(Schedule schedule, int duration)
		{
			Schedule = schedule;
			Duration = duration;
			if (Duration < 1)
				Duration = 1;
			Schedule.CalculateNextRun = x => x.Date.AddDays(Duration * 7);
		}

		/// <summary>
		/// Schedules the specified task to run at the hour and minute specified.  If the hour and minute have passed, the task will execute the next scheduled week.
		/// </summary>
		/// <param name="hours">0-23: Represents the hour of the day</param>
		/// <param name="minutes">0-59: Represents the minute of the day</param>
		/// <returns></returns>
		public void At(int hours, int minutes)
		{
//			Schedule.CalculateNextRun = x => x.Date.AddDays(Duration * 7).AddHours(hours).AddMinutes(minutes);
			Schedule.CalculateNextRun = x => (x > x.Date.AddHours(hours).AddMinutes(minutes)) ? x.Date.AddDays(Duration * 7).AddHours(hours).AddMinutes(minutes) : x.Date.AddHours(hours).AddMinutes(minutes);
		}

		/// <summary>
		/// Schedules the specified task to run on the day specified.  If the day has passed, the task will execute the next scheduled week.
		/// </summary>
		/// <param name="day">Day of week to run the task</param>
		/// <returns></returns>
		public WeeklyDayOfWeekUnit On(DayOfWeek day)
		{
			return new WeeklyDayOfWeekUnit(Schedule, Duration, day);
		}
	}
}

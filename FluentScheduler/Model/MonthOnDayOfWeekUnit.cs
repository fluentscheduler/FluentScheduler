using System;
using FluentScheduler.Extensions;

namespace FluentScheduler.Model
{
	public class MonthOnDayOfWeekUnit
	{
		internal Schedule Schedule { get; private set; }
		internal int Duration { get; private set; }
		internal Week Week { get; private set; }
		internal DayOfWeek DayOfWeek { get; private set; }

		public MonthOnDayOfWeekUnit(Schedule schedule, int duration, Week week, DayOfWeek dayOfWeek)
		{
			Schedule = schedule;
			Duration = duration;
			Week = week;
			DayOfWeek = dayOfWeek;
			if (Week == Week.Last)
				Schedule.CalculateNextRun = x => (x > x.Date.First().Last(DayOfWeek)) ? x.Date.First().AddMonths(Duration).Last(DayOfWeek) : x.Date.First().Last(DayOfWeek);
			else
				Schedule.CalculateNextRun = x => (x > x.Date.First().ToWeek(Week).ThisOrNext(DayOfWeek)) ? x.Date.First().AddMonths(Duration).ToWeek(Week).ThisOrNext(DayOfWeek) : x.Date.First().ToWeek(Week).ThisOrNext(DayOfWeek);
		}

		/// <summary>
		/// Schedules the specified task to run at the hour and minute specified.  If the hour and minute have passed, the task will execute the next scheduled month.
		/// </summary>
		/// <param name="hours">0-23: Represents the hour of the day</param>
		/// <param name="minutes">0-59: Represents the minute of the day</param>
		/// <returns></returns>
		public void At(int hours, int minutes)
		{
			if (Week == Week.Last)
				Schedule.CalculateNextRun = x => (x > x.Date.First().Last(DayOfWeek).AddHours(hours).AddMinutes(minutes)) ? x.Date.First().AddMonths(Duration).Last(DayOfWeek).AddHours(hours).AddMinutes(minutes) : x.Date.First().Last(DayOfWeek).AddHours(hours).AddMinutes(minutes);
			else
				Schedule.CalculateNextRun = x => (x > x.Date.First().ToWeek(Week).ThisOrNext(DayOfWeek).AddHours(hours).AddMinutes(minutes)) ? x.Date.First().AddMonths(Duration).ToWeek(Week).ThisOrNext(DayOfWeek).AddHours(hours).AddMinutes(minutes) : x.Date.First().ToWeek(Week).ThisOrNext(DayOfWeek).AddHours(hours).AddMinutes(minutes);
		}
	}
}

using System;

namespace FluentScheduler.Model
{
	public class MonthUnit
	{
		internal Schedule Schedule { get; private set; }
		internal int Duration { get; private set; }

		public MonthUnit(Schedule schedule, int duration)
		{
			Schedule = schedule;
			Duration = duration;
			Schedule.CalculateNextRun = x => x.Date.AddMonths(Duration);
		}

		/// <summary>
		/// Schedules the specified task to run on the day specified.  If the day has passed, the task will execute the next scheduled month.
		/// </summary>
		/// <param name="day">1-31: Represents the day of the month</param>
		/// <returns></returns>
		public MonthOnDayOfMonthUnit On(int day)
		{
			return new MonthOnDayOfMonthUnit(Schedule, Duration, day);
		}

		/// <summary>
		/// Schedules the specified task to run on the last day of the month.
		/// </summary>
		/// <returns></returns>
		public MonthOnLastDayOfMonthUnit OnTheLastDay()
		{
			return new MonthOnLastDayOfMonthUnit(Schedule, Duration);
		}

		/// <summary>
		/// Schedules the specified task to run on the first occurance of the specified day of the week.  If the day has passed, the task will execute the next scheduled month.
		/// </summary>
		/// <param name="day">Day of week to run the task</param>
		/// <returns></returns>
		public MonthOnDayOfWeekUnit OnTheFirst(DayOfWeek day)
		{
			return new MonthOnDayOfWeekUnit(Schedule, Duration, Week.First, day);
		}
		/// <summary>
		/// Schedules the specified task to run on the second occurance of the specified day of the week.  If the day has passed, the task will execute the next scheduled month.
		/// </summary>
		/// <param name="day">Day of week to run the task</param>
		/// <returns></returns>
		public MonthOnDayOfWeekUnit OnTheSecond(DayOfWeek day)
		{
			return new MonthOnDayOfWeekUnit(Schedule, Duration, Week.Second, day);
		}
		/// <summary>
		/// Schedules the specified task to run on the third occurance of the specified day of the week.  If the day has passed, the task will execute the next scheduled month.
		/// </summary>
		/// <param name="day">Day of week to run the task</param>
		/// <returns></returns>
		public MonthOnDayOfWeekUnit OnTheThird(DayOfWeek day)
		{
			return new MonthOnDayOfWeekUnit(Schedule, Duration, Week.Third, day);
		}
		/// <summary>
		/// Schedules the specified task to run on the fourth occurance of the specified day of the week.  If the day has passed, the task will execute the next scheduled month.
		/// </summary>
		/// <param name="day">Day of week to run the task</param>
		/// <returns></returns>
		public MonthOnDayOfWeekUnit OnTheFourth(DayOfWeek day)
		{
			return new MonthOnDayOfWeekUnit(Schedule, Duration, Week.Fourth, day);
		}
		/// <summary>
		/// Schedules the specified task to run on the last occurance of the specified day of the week.  Depending on the month, this might be the 4th week or the 5th week.  If the day has passed, the task will execute the next scheduled month.
		/// </summary>
		/// <param name="day">Day of week to run the task</param>
		/// <returns></returns>
		public MonthOnDayOfWeekUnit OnTheLast(DayOfWeek day)
		{
			return new MonthOnDayOfWeekUnit(Schedule, Duration, Week.Last, day);
		}
	}
}

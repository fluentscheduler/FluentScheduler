using System;
using FluentScheduler.Model;

namespace FluentScheduler.Extensions
{
	// Some of these from: http://datetimeextensions.codeplex.com/
	public static class DateTimeExtensions
	{
		/// <summary>
		/// Gets a DateTime representing the first day in the current month
		/// </summary>
		/// <param name="current">The current date</param>
		/// <returns></returns>
		public static DateTime First(this DateTime current)
		{
			var first = current.AddDays(1 - current.Day);
			return first;
		}

		/// <summary>
		/// Gets a DateTime representing the first day in the current year
		/// </summary>
		/// <param name="current">The current day</param>
		/// <returns></returns>
		public static DateTime FirstOfYear(this DateTime current)
		{
			return new DateTime(current.Year, 1, 1);
		}

		/// <summary>
		/// Gets a DateTime representing the last day in the current month
		/// </summary>
		/// <param name="current">The current date</param>
		/// <returns></returns>
		public static DateTime Last(this DateTime current)
		{
			var daysInMonth = DateTime.DaysInMonth(current.Year, current.Month);

			var last = current.First().AddDays(daysInMonth - 1);
			return last;
		}

		/// <summary>
		/// Gets a DateTime representing the last specified day in the current month
		/// </summary>
		/// <param name="current">The current date</param>
		/// <param name="dayOfWeek">The current day of week</param>
		/// <returns></returns>
		public static DateTime Last(this DateTime current, DayOfWeek dayOfWeek)
		{
			var last = current.Last();

			var diff = dayOfWeek - last.DayOfWeek;
			if (diff > 0) 
				diff -= 7;
			return last.AddDays(diff);
		}

		/// <summary>
		/// Gets a DateTime representing the first date following the current date which falls on the given day of the week
		/// </summary>
		/// <param name="current">The current date</param>
		/// <param name="dayOfWeek">The day of week for the next date to get</param>
		public static DateTime ThisOrNext(this DateTime current, DayOfWeek dayOfWeek)
		{
			var offsetDays = dayOfWeek - current.DayOfWeek;

			if (offsetDays < 0)
			{
				offsetDays += 7;
			}

			return current.AddDays(offsetDays);
		}

		/// <summary>
		/// Gets a DateTime representing the first date following the current date which falls on the given day of the week
		/// </summary>
		/// <param name="current">The current date</param>
		/// <param name="dayOfWeek">The day of week for the next date to get</param>
		public static DateTime Next(this DateTime current, DayOfWeek dayOfWeek)
		{
			var offsetDays = dayOfWeek - current.DayOfWeek;

			if (offsetDays <= 0)
			{
				offsetDays += 7;
			}

			return current.AddDays(offsetDays);
		}

		/// <summary>
		/// Zero's out the minutes and seconds for the specified datetime.
		/// </summary>
		/// <param name="current">The current date</param>
		public static DateTime ClearMinutesAndSeconds(this DateTime current)
		{
			return current.AddMinutes(-1 * current.Minute).AddSeconds(-1 * current.Second);
		}

		/// <summary>
		/// Gets a DateTime representing the first date following the current date which falls on the given day of the week
		/// </summary>
		/// <param name="current">The current date</param>
		/// <param name="week">The week for the next date to get</param>
		public static DateTime ToWeek(this DateTime current, Week week)
		{
			switch (week)
			{
				case Week.Second:
					return current.First().AddDays(7);
				case Week.Third:
					return current.First().AddDays(14);
				case Week.Fourth:
					return current.First().AddDays(21);
				case Week.Last:
					return current.Last().AddDays(-7);
			}
			return current.First();
		}
	}
}

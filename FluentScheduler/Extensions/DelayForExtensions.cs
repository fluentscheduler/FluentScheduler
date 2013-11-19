using System;
using FluentScheduler.Model;

namespace FluentScheduler
{
	/// <summary>
	/// Extensions for DelayFor() functionality
	/// </summary>
	public static class DelayForExtensions
	{
		private static DelayTimeUnit DelayFor(Schedule schedule, int interval)
		{
			return new DelayTimeUnit(schedule, interval);
		}
		/// <summary>
		/// Delay first execution of the task for the specified time interval.
		/// </summary>
		public static DelayTimeUnit DelayFor(this SpecificRunTime runTime, int interval)
		{
			return DelayFor(runTime.Schedule, interval);
		}

		/// <summary>
		/// Delay first execution of the task for the specified time interval.
		/// </summary>
		public static DelayTimeUnit DelayFor(this SecondUnit timeUnit, int interval)
		{
			return DelayFor(timeUnit.Schedule, interval);
		}
		/// <summary>
		/// Delay first execution of the task for the specified time interval.
		/// </summary>
		public static DelayTimeUnit DelayFor(this MinuteUnit timeUnit, int interval)
		{
			return DelayFor(timeUnit.Schedule, interval);
		}
		/// <summary>
		/// Delay first execution of the task for the specified time interval.
		/// </summary>
		public static DelayTimeUnit DelayFor(this HourUnit timeUnit, int interval)
		{
			return DelayFor(timeUnit.Schedule, interval);
		}
		/// <summary>
		/// Delay first execution of the task for the specified time interval.
		/// </summary>
		public static DelayTimeUnit DelayFor(this DayUnit timeUnit, int interval)
		{
			return DelayFor(timeUnit.Schedule, interval);
		}
		/// <summary>
		/// Delay first execution of the task for the specified time interval.
		/// </summary>
		public static DelayTimeUnit DelayFor(this WeekUnit timeUnit, int interval)
		{
			return DelayFor(timeUnit.Schedule, interval);
		}
		/// <summary>
		/// Delay first execution of the task for the specified time interval.
		/// </summary>
		public static DelayTimeUnit DelayFor(this MonthUnit timeUnit, int interval)
		{
			return DelayFor(timeUnit.Schedule, interval);
		}
		/// <summary>
		/// Delay first execution of the task for the specified time interval.
		/// </summary>
		public static DelayTimeUnit DelayFor(this YearUnit timeUnit, int interval)
		{
			return DelayFor(timeUnit.Schedule, interval);
		}
	}
}

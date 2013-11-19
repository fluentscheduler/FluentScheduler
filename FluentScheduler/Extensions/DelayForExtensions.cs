using System;
using FluentScheduler.Model;

namespace FluentScheduler
{
	/// <summary>
	/// TODO: comments
	/// </summary>
    public static class DelayForExtensions
	{
        private static DelayTimeUnit DelayFor(Schedule schedule, int interval)
        {
            return new DelayTimeUnit(schedule, interval);
        }

		public static DelayTimeUnit DelayFor(this SpecificRunTime runTime, int interval)
		{
            return DelayFor(runTime.Schedule, interval);
		}
		public static DelayTimeUnit DelayFor(this SecondUnit timeUnit, int interval)
		{
            return DelayFor(timeUnit.Schedule, interval);
		}
        public static DelayTimeUnit DelayFor(this MinuteUnit timeUnit, int interval)
        {
            return DelayFor(timeUnit.Schedule, interval);
        }
        public static DelayTimeUnit DelayFor(this HourUnit timeUnit, int interval)
        {
            return DelayFor(timeUnit.Schedule, interval);
        }
        public static DelayTimeUnit DelayFor(this DayUnit timeUnit, int interval)
        {
            return DelayFor(timeUnit.Schedule, interval);
        }
        public static DelayTimeUnit DelayFor(this WeekUnit timeUnit, int interval)
        {
            return DelayFor(timeUnit.Schedule, interval);
        }
        public static DelayTimeUnit DelayFor(this MonthUnit timeUnit, int interval)
        {
            return DelayFor(timeUnit.Schedule, interval);
        }
        public static DelayTimeUnit DelayFor(this YearUnit timeUnit, int interval)
        {
            return DelayFor(timeUnit.Schedule, interval);
        }
    }
}

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
        public static DelayTimeUnit DelayFor(this SpecificTime time, int interval)
        {
            if (time == null)
                throw new ArgumentNullException("time");

            return DelayFor(time.Schedule, interval);
        }

        /// <summary>
        /// Delay first execution of the task for the specified time interval.
        /// </summary>
        public static DelayTimeUnit DelayFor(this SecondUnit timeUnit, int interval)
        {
            if (timeUnit == null)
                throw new ArgumentNullException("timeUnit");

            return DelayFor(timeUnit.Schedule, interval);
        }
        /// <summary>
        /// Delay first execution of the task for the specified time interval.
        /// </summary>
        public static DelayTimeUnit DelayFor(this MinuteUnit timeUnit, int interval)
        {
            if (timeUnit == null)
                throw new ArgumentNullException("timeUnit");

            return DelayFor(timeUnit.Schedule, interval);
        }
        /// <summary>
        /// Delay first execution of the task for the specified time interval.
        /// </summary>
        public static DelayTimeUnit DelayFor(this HourUnit timeUnit, int interval)
        {
            if (timeUnit == null)
                throw new ArgumentNullException("timeUnit");

            return DelayFor(timeUnit.Schedule, interval);
        }
        /// <summary>
        /// Delay first execution of the task for the specified time interval.
        /// </summary>
        public static DelayTimeUnit DelayFor(this DayUnit timeUnit, int interval)
        {
            if (timeUnit == null)
                throw new ArgumentNullException("timeUnit");

            return DelayFor(timeUnit.Schedule, interval);
        }
        /// <summary>
        /// Delay first execution of the task for the specified time interval.
        /// </summary>
        public static DelayTimeUnit DelayFor(this WeekUnit timeUnit, int interval)
        {
            if (timeUnit == null)
                throw new ArgumentNullException("timeUnit");

            return DelayFor(timeUnit.Schedule, interval);
        }
        /// <summary>
        /// Delay first execution of the task for the specified time interval.
        /// </summary>
        public static DelayTimeUnit DelayFor(this MonthUnit timeUnit, int interval)
        {
            if (timeUnit == null)
                throw new ArgumentNullException("timeUnit");

            return DelayFor(timeUnit.Schedule, interval);
        }
        /// <summary>
        /// Delay first execution of the task for the specified time interval.
        /// </summary>
        public static DelayTimeUnit DelayFor(this YearUnit timeUnit, int interval)
        {
            if (timeUnit == null)
                throw new ArgumentNullException("timeUnit");

            return DelayFor(timeUnit.Schedule, interval);
        }
    }
}

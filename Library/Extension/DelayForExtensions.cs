namespace FluentScheduler
{
    using System;

    /// <summary>
    /// DelayFor extension methods.
    /// </summary>
    public static class DelayForExtensions
    {
        private static DelayTimeUnit DelayFor(Schedule schedule, int interval)
        {
            return new DelayTimeUnit(schedule, interval);
        }

        /// <summary>
        /// Delays the job for the given interval.
        /// </summary>
        /// <param name="unit">The schedule being affected.</param>
        /// <param name="interval">Interval to wait.</param>
        public static DelayTimeUnit DelayFor(this SpecificTimeUnit unit, int interval)
        {
            if (unit == null)
                throw new ArgumentNullException("unit");

            return DelayFor(unit.Schedule, interval);
        }

        /// <summary>
        /// Delays the job for the given interval.
        /// </summary>
        /// <param name="unit">The schedule being affected.</param>
        /// <param name="interval">Interval to wait.</param>
        public static DelayTimeUnit DelayFor(this MillisecondUnit unit, int interval)
        {
            if (unit == null)
                throw new ArgumentNullException("unit");

            return DelayFor(unit.Schedule, interval);
        }

        /// <summary>
        /// Delays the job for the given interval.
        /// </summary>
        /// <param name="unit">The schedule being affected.</param>
        /// <param name="interval">Interval to wait.</param>
        public static DelayTimeUnit DelayFor(this SecondUnit unit, int interval)
        {
            if (unit == null)
                throw new ArgumentNullException("unit");

            return DelayFor(unit.Schedule, interval);
        }

        /// <summary>
        /// Delays the job for the given interval.
        /// </summary>
        /// <param name="unit">The schedule being affected.</param>
        /// <param name="interval">Interval to wait.</param>
        public static DelayTimeUnit DelayFor(this MinuteUnit unit, int interval)
        {
            if (unit == null)
                throw new ArgumentNullException("unit");

            return DelayFor(unit.Schedule, interval);
        }

        /// <summary>
        /// Delays the job for the given interval.
        /// </summary>
        /// <param name="unit">The schedule being affected.</param>
        /// <param name="interval">Interval to wait.</param>
        public static DelayTimeUnit DelayFor(this HourUnit unit, int interval)
        {
            if (unit == null)
                throw new ArgumentNullException("unit");

            return DelayFor(unit.Schedule, interval);
        }

        /// <summary>
        /// Delays the job for the given interval.
        /// </summary>
        /// <param name="unit">The schedule being affected.</param>
        /// <param name="interval">Interval to wait.</param>
        public static DelayTimeUnit DelayFor(this DayUnit unit, int interval)
        {
            if (unit == null)
                throw new ArgumentNullException("unit");

            return DelayFor(unit.Schedule, interval);
        }

        /// <summary>
        /// Delays the job for the given interval.
        /// </summary>
        /// <param name="unit">The schedule being affected.</param>
        /// <param name="interval">Interval to wait.</param>
        public static DelayTimeUnit DelayFor(this WeekUnit unit, int interval)
        {
            if (unit == null)
                throw new ArgumentNullException("unit");

            return DelayFor(unit.Schedule, interval);
        }

        /// <summary>
        /// Delays the job for the given interval.
        /// </summary>
        /// <param name="unit">The schedule being affected.</param>
        /// <param name="interval">Interval to wait.</param>
        public static DelayTimeUnit DelayFor(this MonthUnit unit, int interval)
        {
            if (unit == null)
                throw new ArgumentNullException("unit");

            return DelayFor(unit.Schedule, interval);
        }

        /// <summary>
        /// Delays the job for the given interval.
        /// </summary>
        /// <param name="unit">The schedule being affected.</param>
        /// <param name="interval">Interval to wait.</param>
        public static DelayTimeUnit DelayFor(this YearUnit unit, int interval)
        {
            if (unit == null)
                throw new ArgumentNullException("unit");

            return DelayFor(unit.Schedule, interval);
        }
    }
}

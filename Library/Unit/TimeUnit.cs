namespace FluentScheduler
{
    /// <summary>
    /// Unit of time.
    /// </summary>
    public sealed class TimeUnit
    {
        private readonly Schedule _schedule;

        private readonly int _duration;

        internal TimeUnit(Schedule schedule, int duration)
        {
            _schedule = schedule;
            _duration = duration;
        }

        /// <summary>
        /// Sets the interval to milliseconds.
        /// The timing may not be accurated when used with very low intervals.
        /// </summary>
        public MillisecondUnit Milliseconds()
        {
            return new MillisecondUnit(_schedule, _duration);
        }

        /// <summary>
        /// Sets the interval to seconds.
        /// </summary>
        public SecondUnit Seconds()
        {
            return new SecondUnit(_schedule, _duration);
        }

        /// <summary>
        /// Sets the interval to minutes.
        /// </summary>
        public MinuteUnit Minutes()
        {
            return new MinuteUnit(_schedule, _duration);
        }

        /// <summary>
        /// Sets the interval to hours.
        /// </summary>
        public HourUnit Hours()
        {
            return new HourUnit(_schedule, _duration);
        }

        /// <summary>
        /// Sets the interval to days.
        /// </summary>
        public DayUnit Days()
        {
            return new DayUnit(_schedule, _duration);
        }

        /// <summary>
        /// Sets the interval to weekdays.
        /// </summary>
        public WeekdayUnit Weekdays()
        {
            return new WeekdayUnit(_schedule, _duration);
        }

        /// <summary>
        /// Sets the interval to weeks.
        /// </summary>
        public WeekUnit Weeks()
        {
            return new WeekUnit(_schedule, _duration);
        }

        /// <summary>
        /// Sets the interval to months.
        /// </summary>
        public MonthUnit Months()
        {
            return new MonthUnit(_schedule, _duration);
        }

        /// <summary>
        /// Sets the interval to years.
        /// </summary>
        public YearUnit Years()
        {
            return new YearUnit(_schedule, _duration);
        }
    }
}

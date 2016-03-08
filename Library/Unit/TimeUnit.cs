namespace FluentScheduler
{
    public sealed class TimeUnit
    {
        private readonly Schedule _schedule;

        private readonly int _duration;

        public TimeUnit(Schedule schedule, int duration)
        {
            _schedule = schedule;
            _duration = duration;
        }

        public SecondUnit Seconds()
        {
            return new SecondUnit(_schedule, _duration);
        }
        public MinuteUnit Minutes()
        {
            return new MinuteUnit(_schedule, _duration);
        }
        public HourUnit Hours()
        {
            return new HourUnit(_schedule, _duration);
        }
        public DayUnit Days()
        {
            return new DayUnit(_schedule, _duration);
        }
        public WeekdayUnit Weekdays()
        {
            return new WeekdayUnit(_schedule, _duration);
        }
        public WeekUnit Weeks()
        {
            return new WeekUnit(_schedule, _duration);
        }
        public MonthUnit Months()
        {
            return new MonthUnit(_schedule, _duration);
        }
        public YearUnit Years()
        {
            return new YearUnit(_schedule, _duration);
        }
    }
}

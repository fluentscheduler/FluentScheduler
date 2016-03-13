namespace FluentScheduler
{
    public sealed class YearUnit
    {
        private readonly int _duration;

        public YearUnit(Schedule schedule, int duration)
        {
            _duration = duration;
            Schedule = schedule;
            Schedule.CalculateNextRun = x =>
            {
                var nextRun = x.Date.AddYears(_duration);
                return x > nextRun ? nextRun.AddYears(_duration) : nextRun;
            };
        }

        internal Schedule Schedule { get; private set; }

        /// <summary>
        /// Schedules it to run on the day specified. If the day has passed it will execute the next scheduled year.
        /// </summary>
        /// <param name="day">1-365: Represents the day of the year</param>
        /// <returns></returns>
        public YearOnDayOfYearUnit On(int day)
        {
            return new YearOnDayOfYearUnit(Schedule, _duration, day);
        }

        /// <summary>
        /// Schedules it to run on the last day of the year.
        /// </summary>
        /// <returns></returns>
        public YearOnLastDayOfYearUnit OnTheLastDay()
        {
            return new YearOnLastDayOfYearUnit(Schedule, _duration);
        }
    }
}

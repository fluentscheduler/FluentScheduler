namespace FluentScheduler
{
    public sealed class YearOnLastDayOfYearUnit
    {
        private readonly int _duration;

        public YearOnLastDayOfYearUnit(Schedule schedule, int duration)
        {
            _duration = duration;
            Schedule = schedule;
            At(0, 0);
        }

        internal Schedule Schedule { get; private set; }

        /// <summary>
        /// Schedules it to run at the hour and minute specified on the last day of year. If the hour and minute have passed it will execute the next scheduled year.
        /// </summary>
        /// <param name="hours">0-23: Represents the hour of the day</param>
        /// <param name="minutes">0-59: Represents the minute of the day</param>
        /// <returns></returns>
        public void At(int hours, int minutes)
        {
            Schedule.CalculateNextRun = x =>
            {
                var nextRun = x.Date.FirstOfYear().AddMonths(11).Last().AddHours(hours).AddMinutes(minutes);
                return x > nextRun ? x.Date.FirstOfYear().AddYears(_duration).AddMonths(11).Last().AddHours(hours).AddMinutes(minutes) : nextRun;
            };
        }
    }
}

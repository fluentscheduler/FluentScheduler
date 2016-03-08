namespace FluentScheduler
{
    public sealed class MonthOnLastDayOfMonthUnit
    {
        private readonly int _duration;

        public MonthOnLastDayOfMonthUnit(Schedule schedule, int duration)
        {
            _duration = duration;
            Schedule = schedule;
            Schedule.CalculateNextRun = x =>
            {
                var nextRun = x.Date.Last();
                return x > nextRun ? x.Date.First().AddMonths(_duration).Last() : x.Date.Last();
            };
        }

        internal Schedule Schedule { get; private set; }

        /// <summary>
        /// Schedules the specified task to run at the hour and minute specified.  If the hour and minute have passed, the task will execute the next scheduled month.
        /// </summary>
        /// <param name="hours">0-23: Represents the hour of the day</param>
        /// <param name="minutes">0-59: Represents the minute of the day</param>
        /// <returns></returns>
        public void At(int hours, int minutes)
        {
            Schedule.CalculateNextRun = x =>
            {
                var nextRun = x.Date.Last().AddHours(hours).AddMinutes(minutes);
                return x > nextRun ? x.Date.First().AddMonths(_duration).Last().AddHours(hours).AddMinutes(minutes) : nextRun;
            };
        }
    }
}

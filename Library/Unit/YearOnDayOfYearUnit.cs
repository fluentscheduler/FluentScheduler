namespace FluentScheduler
{
    public sealed class YearOnDayOfYearUnit
    {
        private readonly int _duration;

        private readonly int _dayOfYear;

        public YearOnDayOfYearUnit(Schedule schedule, int duration, int dayOfYear)
        {
            _duration = duration;
            _dayOfYear = dayOfYear;
            Schedule = schedule;
            At(0, 0);
        }

        internal Schedule Schedule { get; private set; }

        /// <summary>
        /// Schedules the specified task to run at the hour and minute specified.  If the hour and minute have passed, the task will execute the next scheduled year.
        /// </summary>
        /// <param name="hours">0-23: Represents the hour of the day</param>
        /// <param name="minutes">0-59: Represents the minute of the day</param>
        /// <returns></returns>
        public void At(int hours, int minutes)
        {
            Schedule.CalculateNextRun = x =>
            {
                var nextRun = x.Date.FirstOfYear().AddDays(_dayOfYear - 1).AddHours(hours).AddMinutes(minutes);
                return x > nextRun ? x.Date.FirstOfYear().AddYears(_duration).AddDays(_dayOfYear - 1).AddHours(hours).AddMinutes(minutes) : nextRun;
            };
        }
    }
}

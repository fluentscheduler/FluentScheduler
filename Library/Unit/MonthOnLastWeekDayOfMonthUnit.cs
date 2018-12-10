namespace FluentScheduler
{
    using System;

    /// <summary>
    /// Unit of time that represents last day of the month.
    /// </summary>
    public sealed class MonthOnLastWeekDayOfMonthUnit
    {
        private readonly int _duration;

        internal MonthOnLastWeekDayOfMonthUnit(Schedule schedule, int duration)
        {
            _duration = duration;
            Schedule = schedule;
            At(0, 0);
        }

        internal Schedule Schedule { get; private set; }

        /// <summary>
        /// Runs the job at the given time of day.
        /// </summary>
        /// <param name="hours">The hours (0 through 23).</param>
        /// <param name="minutes">The minutes (0 through 59).</param>
        public void At(int hours, int minutes)
        {
            Schedule.CalculateNextRun = x =>
            {
                Func<DateTime, DateTime> calculate = y =>
                {
                    return y.LastWeekDay().AddHours(hours).AddMinutes(minutes);
                };

                var date              = x.Date.First();
                var runThisMonth      = calculate(date);
                var runAfterThisMonth = calculate(date.AddMonths(_duration));

                return x > runThisMonth ? runAfterThisMonth : runThisMonth;
            };
        }
    }
}

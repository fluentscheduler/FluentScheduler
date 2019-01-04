namespace FluentScheduler
{
    using System;

    /// <summary>
    /// Unit of time that represents a specific week day of the month.
    /// </summary>
    public sealed class MonthOnWeekDayOfMonthUnit
    {
        private readonly int _duration;

        private readonly int weekDayOfMonth;

        internal MonthOnWeekDayOfMonthUnit(Schedule schedule, int duration, int weekDayOfMonth)
        {
            _duration = duration;
            this.weekDayOfMonth = weekDayOfMonth;
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
                    var weekDay = this.weekDayOfMonth > 0
                                      ? y.AddDays(-1).NextNWeekday(this.weekDayOfMonth)
                                      : y.PreviousNWeekday(this.weekDayOfMonth);

                    var lastWeekDayOfMonth = y.LastWeekDay();
                    if (weekDay > lastWeekDayOfMonth)
                        weekDay = lastWeekDayOfMonth;

                    return weekDay.AddHours(hours).AddMinutes(minutes);
                };

                var date = x.Date.First();
                var runThisMonth = calculate(date);
                var runAfterThisMonth = calculate(date.AddMonths(_duration));

                return x > runThisMonth ? runAfterThisMonth : runThisMonth;
            };
        }
    }
}

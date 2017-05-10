namespace FluentScheduler
{
    using System;

    /// <summary>
    /// Unit of time that represents a specific day of the month.
    /// </summary>
    public sealed class MonthOnDayOfMonthUnit : IDayRestrictableUnit
    {
        private readonly int _duration;

        private readonly int _dayOfMonth;

        internal MonthOnDayOfMonthUnit(Schedule schedule, int duration, int dayOfMonth)
        {
            _duration = duration;
            _dayOfMonth = dayOfMonth;
            Schedule = schedule;
            At(0, 0);
        }

        internal Schedule Schedule { get; private set; }

        Schedule IDayRestrictableUnit.Schedule { get { return this.Schedule; } }

        DateTime IDayRestrictableUnit.DayIncrement(DateTime increment)
        {
            return increment.AddDays(_duration);
        }

        /// <summary>
        /// Runs the job at the given time of day.
        /// </summary>
        /// <param name="hours">The hours (0 through 23).</param>
        /// <param name="minutes">The minutes (0 through 59).</param>
        public IDayRestrictableUnit At(int hours, int minutes)
        {
            Schedule.CalculateNextRun = x =>
            {
                Func<DateTime, DateTime> calculate = y =>
                {
                    var day = Math.Min(_dayOfMonth, DateTime.DaysInMonth(y.Year, y.Month));
                    return y.AddDays(day - 1).AddHours(hours).AddMinutes(minutes);
                };

                var date = x.Date.First();
                var runThisMonth = calculate(date);
                var runAfterThisMonth = calculate(date.AddMonths(_duration));

                return x > runThisMonth ? runAfterThisMonth : runThisMonth;
            };

            return this;
        }
    }
}

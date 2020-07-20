namespace FluentScheduler
{
    using System;

    /// <summary>
    /// Unit of time in weeks.
    /// </summary>
    public sealed class WeekUnit
    {
        private readonly int _duration;

        internal WeekUnit(Schedule schedule, int duration)
        {
            _duration = duration < 0 ? 0 : duration;
            Schedule = schedule;
            Schedule.CalculateNextRun = x =>
            {
                var nextRun = x.Date.AddDays(_duration * 7);
                return x > nextRun ? nextRun.AddDays(Math.Max(_duration, 1) * 7) : nextRun;
            };
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
                var nextRun = x.Date.AddHours(hours).AddMinutes(minutes);
                return x > nextRun ? nextRun.AddDays(Math.Max(_duration, 1) * 7) : nextRun;
            };
        }

        /// <summary>
        /// Runs the job on the given day of the week.
        /// </summary>
        /// <param name="day">The day of the week.</param>
        public WeeklyDayOfWeekUnit On(DayOfWeek day)
        {
            return new WeeklyDayOfWeekUnit(Schedule, _duration, day);
        }
    }
}

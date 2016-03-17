namespace FluentScheduler
{
    using System;

    public sealed class WeekUnit
    {
        private readonly int _duration;

        public WeekUnit(Schedule schedule, int duration)
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
        /// Schedules it to run at the hour and minute specified. If the hour and minute have passed it will execute the next scheduled week.
        /// </summary>
        /// <param name="hours">0-23: Represents the hour of the day</param>
        /// <param name="minutes">0-59: Represents the minute of the day</param>
        /// <returns></returns>
        public void At(int hours, int minutes)
        {
            Schedule.CalculateNextRun = x =>
            {
                var nextRun = x.Date.AddHours(hours).AddMinutes(minutes);
                return x > nextRun ? nextRun.AddDays(Math.Max(_duration, 1) * 7) : nextRun;
            };
        }

        /// <summary>
        /// Schedules it to run on the day specified. If the day has passed it will execute the next scheduled week.
        /// </summary>
        /// <param name="day">Day of week to run</param>
        /// <returns></returns>
        public WeeklyDayOfWeekUnit On(DayOfWeek day)
        {
            return new WeeklyDayOfWeekUnit(Schedule, _duration, day);
        }
    }
}

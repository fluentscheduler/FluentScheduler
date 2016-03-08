using System;

namespace FluentScheduler
{
    public sealed class WeeklyDayOfWeekUnit
    {
        private readonly int _duration;

        private readonly DayOfWeek _day;

        public WeeklyDayOfWeekUnit(Schedule schedule, int duration, DayOfWeek day)
        {
            _duration = duration;
            _day = day;
            Schedule = schedule;

            if (_duration > 0)
            {
                Schedule.CalculateNextRun = x =>
                {
                    var nextRun = x.Date.AddDays(_duration * 7).ThisOrNext(day);
                    return x > nextRun ? nextRun.AddDays(_duration * 7) : nextRun;
                };
            }
            else
            {
                Schedule.CalculateNextRun = x =>
                {
                    var nextRun = x.Date.Next(day);
                    return x > nextRun ? nextRun.AddDays(7) : nextRun;
                };
            }
        }

        internal Schedule Schedule { get; private set; }

        /// <summary>
        /// Schedules the specified task to run at the hour and minute specified.  If the hour and minute have passed, the task will execute the next scheduled week.
        /// </summary>
        /// <param name="hours">0-23: Represents the hour of the day</param>
        /// <param name="minutes">0-59: Represents the minute of the day</param>
        /// <returns></returns>
        public void At(int hours, int minutes)
        {
            Schedule.CalculateNextRun = x =>
            {
                var nextRun = x.Date.AddDays(_duration * 7).ThisOrNext(_day).AddHours(hours).AddMinutes(minutes);
                return x > nextRun ? nextRun.AddDays(Math.Max(_duration, 1) * 7) : nextRun;
            };
        }
    }
}

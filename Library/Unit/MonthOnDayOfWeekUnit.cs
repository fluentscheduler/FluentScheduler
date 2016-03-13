using System;

namespace FluentScheduler
{
    public sealed class MonthOnDayOfWeekUnit
    {
        private readonly int _duration;

        private readonly Week _week;

        private readonly DayOfWeek _dayOfWeek;

        public MonthOnDayOfWeekUnit(Schedule schedule, int duration, Week week, DayOfWeek dayOfWeek)
        {
            _duration = duration;
            _week = week;
            _dayOfWeek = dayOfWeek;
            Schedule = schedule;
            At(0, 0);
        }

        internal Schedule Schedule { get; private set; }

        /// <summary>
        /// Schedules to run at the hour and minute specified. If the hour and minute have passed it will execute the next scheduled month.
        /// </summary>
        /// <param name="hours">0-23: Represents the hour of the day</param>
        /// <param name="minutes">0-59: Represents the minute of the day</param>
        /// <returns></returns>
        public void At(int hours, int minutes)
        {
            if (_week == Week.Last)
            {
                Schedule.CalculateNextRun = x =>
                {
                    var nextRun = x.Date.First().Last(_dayOfWeek).AddHours(hours).AddMinutes(minutes);
                    return x > nextRun ? x.Date.First().AddMonths(_duration).Last(_dayOfWeek).AddHours(hours).AddMinutes(minutes) : nextRun;
                };
            }
            else
            {
                Schedule.CalculateNextRun = x =>
                {
                    var nextRun = x.Date.First().ToWeek(_week).ThisOrNext(_dayOfWeek).AddHours(hours).AddMinutes(minutes);
                    return x > nextRun ? x.Date.First().AddMonths(_duration).ToWeek(_week).ThisOrNext(_dayOfWeek).AddHours(hours).AddMinutes(minutes) : nextRun;
                };
            }
        }
    }
}

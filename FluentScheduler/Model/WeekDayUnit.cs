using System;

namespace FluentScheduler.Model
{
    public sealed class WeekDayUnit : ITimeRestrictableUnit
    {
        internal Schedule Schedule { get; private set; }

        Schedule ITimeRestrictableUnit.Schedule { get { return this.Schedule; } }

        internal int Duration { get; private set; }

        public WeekDayUnit(Schedule schedule, int duration)
        {
            Schedule = schedule;
            Duration = duration;
            if (Duration < 1)
                Duration = 1;

            Schedule.CalculateNextRun = x =>
            {
                DateTime nextRun = x.Date.NextNWeekday(Duration);
                return (x > nextRun || !nextRun.Date.IsWeekDay()) ? nextRun.NextNWeekday(Duration) : nextRun;
            };
        }

        /// <summary>
        /// Schedules the specified task to run at the hour and minute specified.  If the hour and minute have passed, the task will execute the next scheduled day.
        /// </summary>
        /// <param name="hours">0-23: Represents the hour of the day</param>
        /// <param name="minutes">0-59: Represents the minute of the day</param>
        /// <returns></returns>
        public void At(int hours, int minutes)
        {
            Schedule.CalculateNextRun = x =>
            {
                DateTime nextRun = x.Date.AddHours(hours).AddMinutes(minutes);
                return (x > nextRun || !nextRun.Date.IsWeekDay()) ? nextRun.NextNWeekday(Duration) : nextRun;
            };
        }
    }
}
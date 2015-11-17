using System;

namespace FluentScheduler.Model
{
    public sealed class DayUnit : IDayRestrictableUnit
    {
        internal Schedule Schedule { get; private set; }

        Schedule IDayRestrictableUnit.Schedule { get { return this.Schedule; } }

        internal int Duration { get; private set; }

        public DayUnit(Schedule schedule, int duration)
        {
            Schedule = schedule;
            Duration = duration;
            if (Duration < 1)
                Duration = 1;

            Schedule.CalculateNextRun = x =>
            {
                var nextRun = x.Date.AddDays(Duration);
                return (x > nextRun) ? ((IDayRestrictableUnit)this).DayIncrement(nextRun) : nextRun;
            };
        }

        /// <summary>
        /// Schedules the specified task to run at the hour and minute specified.  If the hour and minute have passed, the task will execute the next scheduled day.
        /// </summary>
        /// <param name="hours">0-23: Represents the hour of the day</param>
        /// <param name="minutes">0-59: Represents the minute of the day</param>
        /// <returns></returns>
        public IDayRestrictableUnit At(int hours, int minutes)
        {
            Schedule.CalculateNextRun = x =>
            {
                var nextRun = x.Date.AddHours(hours).AddMinutes(minutes);
                return (x > nextRun) ? ((IDayRestrictableUnit)this).DayIncrement(nextRun) : nextRun;
            };
            return this;
        }

        DateTime IDayRestrictableUnit.DayIncrement(DateTime toIncrement)
        {
            return toIncrement.AddDays(Duration);
        }
    }
}

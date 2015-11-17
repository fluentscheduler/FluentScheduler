using System;

namespace FluentScheduler.Model
{
    public sealed class HourUnit : ITimeRestrictableUnit
    {
        internal Schedule Schedule { get; private set; }

    Schedule ITimeRestrictableUnit.Schedule { get { return this.Schedule; } }

        internal int Duration { get; private set; }

        public HourUnit(Schedule schedule, int duration)
        {
            Schedule = schedule;
            Duration = duration;
            if (Duration < 1)
                Duration = 1;
            Schedule.CalculateNextRun = x => {
                var nextRun = x.AddHours(Duration);
                return (x > nextRun) ? nextRun.AddHours(Duration) : nextRun;
            };
        }

        /// <summary>
        /// Schedules the specified task to run at the minute specified.  If the minute has passed, the task will execute the next hour.
        /// </summary>
        /// <param name="minutes">0-59: Represents the minute of the hour</param>
        /// <returns></returns>
        public ITimeRestrictableUnit At(int minutes)
        {
            Schedule.CalculateNextRun = x => {
                var nextRun = x.ClearMinutesAndSeconds().AddMinutes(minutes);
                return (Duration == 1 && x < nextRun) ? nextRun : nextRun.AddHours(Duration);
            };
      return this;
        }
    }
}

using System;

namespace FluentScheduler.Model
{
    public sealed class SecondUnit : ITimeRestrictableUnit
    {
        internal Schedule Schedule { get; private set; }

        Schedule ITimeRestrictableUnit.Schedule { get { return this.Schedule; } }

        internal int Duration { get; private set; }

        public SecondUnit(Schedule schedule, int duration)
        {
            Schedule = schedule;
            Duration = duration;

            Schedule.CalculateNextRun = x => x.AddSeconds(Duration);
        }
    }
}

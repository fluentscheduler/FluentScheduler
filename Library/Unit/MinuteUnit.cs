namespace FluentScheduler
{
    public sealed class MinuteUnit : ITimeRestrictableUnit
    {
        private readonly int _duration;

        public MinuteUnit(Schedule schedule, int duration)
        {
            _duration = duration;
            Schedule = schedule;
            Schedule.CalculateNextRun = x => x.AddMinutes(_duration);
        }

        internal Schedule Schedule { get; private set; }

        Schedule ITimeRestrictableUnit.Schedule { get { return this.Schedule; } }
    }
}

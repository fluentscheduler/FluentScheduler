namespace FluentScheduler
{
    /// <summary>
    /// Unit of time in milliseconds.
    /// </summary>
    public sealed class MillisecondUnit : ITimeRestrictableUnit
    {
        private readonly int _duration;

        internal MillisecondUnit(Schedule schedule, int duration)
        {
            _duration = duration;
            Schedule = schedule;
            Schedule.CalculateNextRun = x => x.AddMilliseconds(_duration);
        }

        internal Schedule Schedule { get; private set; }

        Schedule ITimeRestrictableUnit.Schedule { get { return this.Schedule; } }
    }
}

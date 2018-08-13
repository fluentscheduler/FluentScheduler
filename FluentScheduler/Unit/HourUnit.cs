namespace FluentScheduler
{
    /// <summary>
    /// Unit of time in hours.
    /// </summary>
    public sealed class HourUnit : ITimeRestrictableUnit
    {
        private readonly int _duration;

        internal HourUnit(Schedule schedule, int duration)
        {
            _duration = duration < 1 ? 1 : duration;
            Schedule = schedule;
            Schedule.CalculateNextRun = x =>
            {
                var nextRun = x.AddHours(_duration);
                return x > nextRun ? nextRun.AddHours(_duration) : nextRun;
            };
        }

        internal Schedule Schedule { get; private set; }

        Schedule ITimeRestrictableUnit.Schedule { get { return this.Schedule; } }

        /// <summary>
        /// Runs the job at the given minute of the hour.
        /// </summary>
        /// <param name="minutes">The minutes (0 through 59).</param>
        public ITimeRestrictableUnit At(int minutes)
        {
            Schedule.CalculateNextRun = x =>
            {
                var nextRun = x.ClearMinutesAndSeconds().AddMinutes(minutes);
                return _duration == 1 && x < nextRun ? nextRun : nextRun.AddHours(_duration);
            };
            return this;
        }
    }
}

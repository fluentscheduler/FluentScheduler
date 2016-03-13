namespace FluentScheduler
{
    public sealed class HourUnit : ITimeRestrictableUnit
    {
        private readonly int _duration;

        public HourUnit(Schedule schedule, int duration)
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
        /// Schedules to run at the minute specified. If the minute has passed it will execute the next hour.
        /// </summary>
        /// <param name="minutes">0-59: Represents the minute of the hour</param>
        /// <returns></returns>
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

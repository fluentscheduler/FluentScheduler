using System;

namespace FluentScheduler
{
    /// <summary>
    /// Delayed execution support - each method extends the startup time of the task for the specific interval.
    /// </summary>
    public sealed class DelayTimeUnit
    {
        private readonly int _interval;

        public DelayTimeUnit(Schedule schedule, int interval)
        {
            _interval = interval;
            Schedule = schedule;
        }

        internal Schedule Schedule { get; private set; }

        public void Seconds()
        {
            Schedule.DelayRunFor = TimeSpan.FromSeconds(_interval);
        }

        public void Minutes()
        {
            Schedule.DelayRunFor = TimeSpan.FromMinutes(_interval);
        }

        public void Hours()
        {
            Schedule.DelayRunFor = TimeSpan.FromHours(_interval);
        }

        public void Days()
        {
            Schedule.DelayRunFor = TimeSpan.FromDays(_interval);
        }

        public void Weeks()
        {
            Schedule.DelayRunFor = TimeSpan.FromDays(_interval * 7);
        }

        public void Months()
        {
            var today = DateTime.Today;
            Schedule.DelayRunFor = today.AddMonths(_interval).Subtract(today);
        }

        public void Years()
        {
            var today = DateTime.Today;
            Schedule.DelayRunFor = today.AddYears(_interval).Subtract(today);
        }
    }
}

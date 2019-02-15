namespace FluentScheduler
{
    using NCrontab;
    using System;

    internal class CronTimeCalculator : ITimeCalculator
    {
        private CrontabSchedule _calculator;

        public void UseUtc() => ((ITimeCalculator)this).Now = () => DateTime.UtcNow;

        Func<DateTime> ITimeCalculator.Now { get; set; } = () => DateTime.Now;

        internal CronTimeCalculator(string cronExpression) => _calculator = CrontabSchedule.Parse(cronExpression);

        DateTime? ITimeCalculator.Calculate(DateTime last) => _calculator.GetNextOccurrence(last);

        void ITimeCalculator.Reset() { }
    }
}
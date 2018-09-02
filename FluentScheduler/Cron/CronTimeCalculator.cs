namespace FluentScheduler
{
    using NCrontab;
    using System;

    internal class CronTimeCalculator : ITimeCalculator
    {
        private CrontabSchedule _calculator;

        internal CronTimeCalculator(string cronExpression) => _calculator = CrontabSchedule.Parse(cronExpression);

        DateTime? ITimeCalculator.Calculate(DateTime last) => _calculator.GetNextOccurrence(last);

        void ITimeCalculator.Reset() { }
    }
}
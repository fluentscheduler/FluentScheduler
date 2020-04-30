namespace FluentScheduler
{
    using NCrontab;
    using System;

    internal class CronTimeCalculator : ITimeCalculator
    {
        private CrontabSchedule _calculator;

        public void UseUtc() => ((ITimeCalculator)this).Now = () => DateTime.UtcNow;

        Func<DateTime> ITimeCalculator.Now { get; set; } = () => DateTime.Now;

        internal CronTimeCalculator(string cronExpression)
        {
           if (cronExpression == null) 
                throw new ArgumentNullException(nameof(cronExpression));

            var cronFields = cronExpression.Split(StringSeparatorStock.Space, StringSplitOptions.RemoveEmptyEntries).Length;
            var parseOptions = new CrontabSchedule.ParseOptions 
            {
                IncludingSeconds = cronFields == 6
            };

            _calculator = CrontabSchedule.Parse(cronExpression, parseOptions);
        }

        DateTime? ITimeCalculator.Calculate(DateTime last) => _calculator.GetNextOccurrence(last);

        void ITimeCalculator.Reset() { }
    }
}
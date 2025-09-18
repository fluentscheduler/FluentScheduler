namespace FluentScheduler;

using NCrontab;
using System;

internal class CronTimeCalculator : ITimeCalculator
{
    private readonly CrontabSchedule _calculator;

    internal CronTimeCalculator(string cronExpression)
    {
        var cronFields = cronExpression.Split(StringSeparatorStock.Space, StringSplitOptions.RemoveEmptyEntries).Length;
        var parseOptions = new CrontabSchedule.ParseOptions
        {
            IncludingSeconds = cronFields == 6
        };

        _calculator = CrontabSchedule.Parse(cronExpression, parseOptions);
    }
    
    public Func<DateTime> Now { get; set; } = () => DateTime.Now;
    
    public void UseUtc() => ((ITimeCalculator)this).Now = () => DateTime.UtcNow;

    public DateTime? Calculate(DateTime last) => _calculator.GetNextOccurrence(last);

    public void Reset() { }
}
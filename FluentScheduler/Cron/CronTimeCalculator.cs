using System;
using System.Collections.Generic;
using System.Linq;
using NCrontab;

namespace FluentScheduler;

internal class CronTimeCalculator : ITimeCalculator
{
    private readonly List<CrontabSchedule> _calculators;

    internal CronTimeCalculator(string cronExpression)
    {
        var cronFields = cronExpression.Split(StringSeparatorStock.Space, StringSplitOptions.RemoveEmptyEntries).Length;
        var parseOptions = new CrontabSchedule.ParseOptions
        {
            IncludingSeconds = cronFields == 6
        };

        _calculators = [CrontabSchedule.Parse(cronExpression, parseOptions)];
    }

    internal CronTimeCalculator(IEnumerable<string> cronExpressions)
    {
        List<CrontabSchedule> schedules = new();

        foreach (string expression in cronExpressions)
        {
            var cronFields = expression.Split(StringSeparatorStock.Space, StringSplitOptions.RemoveEmptyEntries).Length;
            var parseOptions = new CrontabSchedule.ParseOptions
            {
                IncludingSeconds = cronFields == 6
            };

            var schedule = CrontabSchedule.Parse(expression, parseOptions);

            schedules.Add(schedule);
        }

        _calculators = schedules;
    }

    public Func<DateTime> Now { get; set; } = () => DateTime.Now;

    public void UseUtc() => ((ITimeCalculator)this).Now = () => DateTime.UtcNow;

    public DateTime? Calculate(DateTime last) => _calculators.GetNextOccurrences(last, DateTime.MaxValue).First();

    public void Reset()
    {
    }
}
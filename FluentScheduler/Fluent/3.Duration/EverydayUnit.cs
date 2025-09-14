using System;

namespace FluentScheduler;

/// <summary>
/// The "period" run has been set, but not its unit.
/// </summary>
public class EverydayUnit
{
    private readonly FluentTimeCalculator _calculator;

    internal EverydayUnit(FluentTimeCalculator calculator) => _calculator = calculator;

    /// <summary>
    /// Runs the job at the given time of day (military format).
    /// </summary>
    /// <param name="hour">The hour (0 through 23).</param>
    /// <param name="minute">The minute (0 through 59).</param>
    public void At(int hour, int minute)
    {
        ThrowHelper.ThrowIfOutOfMilitaryTimeRange(hour, minute);

        _calculator.PeriodCalculations.Add(last =>
        {
            ITimeCalculator timeCalculator = _calculator;
            var now = timeCalculator.Now();

            var next = new DateTime(last.Year, last.Month, last.Day, hour, minute, 0);

            return now > next ? next.AddDays(1) : next;
        });
    }

    /// <summary>
    /// Runs the job at the given time of day.
    /// </summary>
    /// <param name="timeCollection">Time of day.</param>
    public void At(params TimeSpan[] timeCollection)
    {
        ThrowHelper.ThrowIfEmpty(timeCollection);
        ThrowHelper.ThrowIfOutOfOrder(timeCollection);
        ThrowHelper.ThrowIfOutOfMilitaryTimeRange(timeCollection);

        _calculator.PeriodCalculations.Add(_ =>
        {
            ITimeCalculator timeCalculator = _calculator;
            var now = timeCalculator.Now();

            var nextTime = timeCollection[0];

            foreach (var time in timeCollection)
            {
                if (now.TimeOfDay < time)
                {
                    nextTime = time;
                    break;
                }
            }

            var next = new DateTime(now.Year, now.Month, now.Day).Add(nextTime);

            return now > next ? next.AddDays(1) : next;
        });
    }

    /// <summary>
    /// Runs the job at the given period of time.
    /// </summary>
    /// <param name="hourFrom">Hour to delimitate period beginning.</param>
    /// <param name="minuteFrom">Minute to delimitade period beginning.</param>
    /// <param name="hourTo">Hour to delimitate period end.</param>
    /// <param name="minuteTo">Minute to delimitade period end.</param>
    public void Between(int hourFrom, int minuteFrom, int hourTo, int minuteTo) =>
        Between(new TimeSpan(hourFrom, minuteFrom, 0), new TimeSpan(hourTo, minuteTo, 0));

    /// <summary>
    /// Runs the job at the given period of time.
    /// </summary>
    /// <param name="from">Time of the day to delimitate the period beginnig.</param>
    /// <param name="to">Time of the day to delimitade the period end.</param>
    public void Between(TimeSpan from, TimeSpan to)
    {
        if (from == to)
            throw new ArgumentException($"The parameters '{nameof(from)}' and '{nameof(to)}' must not be equal.");

        _calculator.PeriodCalculations.Add(
            last =>
            {
                var fromDate = new DateTime(last.Year, last.Month, last.Day).Add(from);
                var toDate = new DateTime(last.Year, last.Month, last.Day).Add(to);
                var now = ((ITimeCalculator)_calculator).Now();

                var next = new DateTime(last.Year, last.Month, last.Day).Add(from);

                if (now >= fromDate && now <= toDate)
                {
                    next = now;
                }

                return next;
            }
        );
    }
}
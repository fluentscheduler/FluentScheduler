using System.Globalization;

namespace FluentScheduler;

using System;

/// <summary>
/// The "period" run has been set, but not its unit.
/// </summary>
public class PeriodOnceSet
{
    private readonly FluentTimeCalculator _calculator;

    internal PeriodOnceSet(FluentTimeCalculator calculator) => _calculator = calculator;

    /// <summary>
    /// Runs the job at the given time of day (military format).
    /// </summary>
    /// <param name="hour">The hour (0 through 23).</param>
    /// <param name="minute">The minute (0 through 59).</param>
    public void At(int hour, int minute)
    {
        ThrowHelper.ThrowIfOutOfMilitaryTimeRange(hour, minute);

        _calculator.PeriodCalculations.Add(last => new DateTime(last.Year, last.Month, last.Day, hour, minute, 0));
    }

    /// <summary>
    /// Runs the job at the given time of day.
    /// </summary>
    /// <param name="timeCollection">Time of day.</param>
    public void At(params TimeSpan[] timeCollection)
    {
        ThrowHelper.ThrowIfEmpty(timeCollection);
        ThrowHelper.ThrowIfOutOfMilitaryTimeRange(timeCollection);

        foreach (var time in timeCollection)
        {
            if (time >= ((ITimeCalculator)_calculator).Now().TimeOfDay)
            {
                _calculator.PeriodCalculations.Add(last =>
                    new DateTime(last.Year, last.Month, last.Day, time.Hours, time.Minutes, 0)
                );

                break;
            }
        }
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
                var next = new DateTime(last.Year, last.Month, last.Day).Add(from);

                return next;
            }
        );
    }
}

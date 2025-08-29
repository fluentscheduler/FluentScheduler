namespace FluentScheduler;

using System;

/// <summary>
/// Allows you to fluently specify when the job should run.
/// </summary>
public class RunSpecifier
{
    private readonly FluentTimeCalculator _calculator;

    internal RunSpecifier(FluentTimeCalculator calculator) => _calculator = calculator;

    /// <summary>
    /// Runs the job according to the given interval.
    /// </summary>
    /// <param name="interval">Interval (without unit) to wait</param>
    public PeriodDurationSet Every(int interval)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(interval);

        return new PeriodDurationSet(interval, _calculator);
    }

    /// <summary>
    /// Runs the job everyday. Tries to run in the same day if the given time was not reached.
    /// </summary>
    public EverydayUnit Everyday()
    {
        ITimeCalculator timeCalculator = _calculator;

        var now = timeCalculator.Now();
        var calculatedFirstExecution = false;

        _calculator.PeriodCalculations.Add(last =>
        {
            var next = new DateTime(last.Year, last.Month, last.Day, now.Hour, now.Minute, 0);

            if (calculatedFirstExecution)
                next = next.AddDays(1);

            calculatedFirstExecution = true;

            return next;
        });

        return new EverydayUnit(_calculator);
    }

    /// <summary>
    /// Runs the job according to the given interval.
    /// </summary>
    /// <param name="day">Day to run the job</param>
    public RestrictionUnit Every(DayOfWeek day)
    {
        ThrowHelper.ThrowIfNotDefinedInEnum(day);

        _calculator.PeriodCalculations.Add(last =>
        {
            var daysToNext = day - last.DayOfWeek;

            if (day <= last.DayOfWeek)
                daysToNext = 7 - Math.Abs(daysToNext);

            last = last.AddDays(daysToNext);

            return last;
        });

        return new RestrictionUnit(_calculator);
    }

    /// <summary>
    /// Runs the job according to the given interval.
    /// </summary>
    /// <param name="time">Time to run the job</param>
    public void Every(TimeSpan time)
    {
        ThrowHelper.ThrowIfNegative(time);

        var timeOfDay = new TimeSpan(time.Hours, time.Minutes, time.Seconds);
        _calculator.PeriodCalculations.Add(last => last.Add(timeOfDay));
    }

    /// <summary>
    /// Runs the job every weekday
    /// </summary>
    public RestrictionUnit EveryWeekday() => new DayUnit(_calculator).Weekday();

    /// <summary>
    /// Runs the job every weekend
    /// </summary>
    public RestrictionUnit EveryWeekend() => new DayUnit(_calculator).Weekend();

    /// <summary>
    /// Runs the job now.
    /// </summary>
    public OnceSet Now()
    {
        _calculator.OnceCalculation = last => last;
        return new OnceSet(_calculator);
    }

    /// <summary>
    /// Runs the job once at the given time time of day.
    /// </summary>
    /// <param name="hour">The hour (0 to 23)</param>
    /// <param name="minute">The minute (0 to 59)</param>
    public OnceSet OnceAt(int hour, int minute)
    {
        ThrowHelper.ThrowIfOutOfMilitaryTimeRange(hour, minute);

        OnceAt(new TimeSpan(hour, minute, 0));
        return new OnceSet(_calculator);
    }

    /// <summary>
    /// Runs the job once at the given time of day.
    /// </summary>
    /// <param name="timeOfDay">Time of the day to run</param>
    public OnceSet OnceAt(TimeSpan timeOfDay)
    {
        ThrowHelper.ThrowIfOutOfMilitaryTimeRange(timeOfDay);

        timeOfDay = new TimeSpan(timeOfDay.Hours, timeOfDay.Minutes, timeOfDay.Seconds);

        _calculator.OnceCalculation = last =>
        {
            var result = last.Date.Add(timeOfDay);
            return result > last ? result : result.AddDays(1);
        };

        return new OnceSet(_calculator);
    }

    /// <summary>
    /// Runs the job once at the given date and time.
    /// </summary>
    /// <param name="dateTime">Date and time to run</param>
    public OnceSet OnceAt(DateTime dateTime)
    {
        _calculator.OnceCalculation = _ => dateTime;
        return new OnceSet(_calculator);
    }

    /// <summary>
    /// Runs the job once after the given delay.
    /// </summary>
    /// <param name="delay">Delay (without unit) to wait</param>
    public OnceDurationSet OnceIn(int delay)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(delay);

        _calculator.OnceCalculation = last => last;
        return new OnceDurationSet(delay, _calculator);
    }

    /// <summary>
    /// Runs the job once after the given delay.
    /// </summary>
    /// <param name="delay">Delay to wait</param>
    public OnceSet OnceIn(TimeSpan delay)
    {
        ThrowHelper.ThrowIfNegative(delay);

        _calculator.OnceCalculation = last => last.Add(delay);
        return new OnceSet(_calculator);
    }
}
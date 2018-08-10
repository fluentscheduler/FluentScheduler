using System;
using FluentScheduler.Enum;
using FluentScheduler.Extension;

namespace FluentScheduler.Unit
{
  /// <summary>
  /// Unit of time that represents week and day of week of the month (wow that's confusing).
  /// </summary>
  public sealed class MonthOnDayOfWeekUnit
  {
    private readonly int _duration;

    private readonly Week _week;

    private readonly DayOfWeek _dayOfWeek;

    internal MonthOnDayOfWeekUnit(Schedule schedule, int duration, Week week, DayOfWeek dayOfWeek)
    {
      _duration = duration;
      _week = week;
      _dayOfWeek = dayOfWeek;
      this.Schedule = schedule;
      this.At(0, 0);
    }

    internal Schedule Schedule { get; }

    /// <summary>
    /// Runs the job at the given time of day.
    /// </summary>
    /// <param name="hours">The hours (0 through 23).</param>
    /// <param name="minutes">The minutes (0 through 59).</param>

    public void At(int hours, int minutes)
    {
      if (_week == Week.Last)
      {
        this.Schedule.CalculateNextRun = x =>
        {
          var nextRun = x.Date.First().Last(_dayOfWeek).AddHours(hours).AddMinutes(minutes);
          return x > nextRun ? x.Date.First().AddMonths(_duration).Last(_dayOfWeek).AddHours(hours).AddMinutes(minutes) : nextRun;
        };
      }
      else
      {
        this.Schedule.CalculateNextRun = x =>
        {
          var nextRun = x.Date.First().ToWeek(_week).ThisOrNext(_dayOfWeek).AddHours(hours).AddMinutes(minutes);
          return x > nextRun ? x.Date.First().AddMonths(_duration).ToWeek(_week).ThisOrNext(_dayOfWeek).AddHours(hours).AddMinutes(minutes) : nextRun;
        };
      }
    }
  }
}

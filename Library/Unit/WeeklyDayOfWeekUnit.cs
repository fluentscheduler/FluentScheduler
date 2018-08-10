using System;
using Moong.FluentScheduler.Extension;

namespace Moong.FluentScheduler.Unit
{
  /// <summary>
  /// Unit of time that represents day of the week.
  /// </summary>
  public sealed class WeeklyDayOfWeekUnit
  {
    private readonly int _duration;

    private readonly DayOfWeek _day;

    internal WeeklyDayOfWeekUnit(Schedule schedule, int duration, DayOfWeek day)
    {
      _duration = duration;
      _day = day;
      this.Schedule = schedule;

      if (_duration > 0)
      {
        this.Schedule.CalculateNextRun = x =>
        {
          var nextRun = x.Date.AddDays(_duration * 7).ThisOrNext(day);
          return x > nextRun ? nextRun.AddDays(_duration * 7) : nextRun;
        };
      }
      else
      {
        this.Schedule.CalculateNextRun = x =>
        {
          var nextRun = x.Date.Next(day);
          return x > nextRun ? nextRun.AddDays(7) : nextRun;
        };
      }
    }

    internal Schedule Schedule { get; }

    /// <summary>
    /// Runs the job at the given time of day.
    /// </summary>
    /// <param name="hours">The hours (0 through 23).</param>
    /// <param name="minutes">The minutes (0 through 59).</param>
    public void At(int hours, int minutes)
    {
      this.Schedule.CalculateNextRun = x =>
      {
        var nextRun = x.Date.AddDays(_duration * 7).ThisOrNext(_day).AddHours(hours).AddMinutes(minutes);
        return x > nextRun ? nextRun.AddDays(Math.Max(_duration, 1) * 7) : nextRun;
      };
    }
  }
}

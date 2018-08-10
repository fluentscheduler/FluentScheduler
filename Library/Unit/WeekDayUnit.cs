using Moong.FluentScheduler.Extension;

namespace Moong.FluentScheduler.Unit
{
  /// <summary>
  /// Unit of time in weekdays.
  /// </summary>
  public sealed class WeekdayUnit : ITimeRestrictableUnit
  {
    private readonly int _duration;

    internal WeekdayUnit(Schedule schedule, int duration)
    {
      _duration = duration < 1 ? 1 : duration;
      this.Schedule = schedule;
      this.Schedule.CalculateNextRun = x =>
      {
        var nextRun = x.Date.NextNWeekday(_duration);
        return x > nextRun || !nextRun.Date.IsWeekday() ? nextRun.NextNWeekday(_duration) : nextRun;
      };
    }

    internal Schedule Schedule { get; }

    Schedule ITimeRestrictableUnit.Schedule => this.Schedule;

    /// <summary>
    /// Runs the job at the given time of day.
    /// </summary>
    /// <param name="hours">The hours (0 through 23).</param>
    /// <param name="minutes">The minutes (0 through 59).</param>
    public void At(int hours, int minutes)
    {
      this.Schedule.CalculateNextRun = x =>
      {
        var nextRun = x.Date.AddHours(hours).AddMinutes(minutes);
        return x > nextRun || !nextRun.Date.IsWeekday() ? nextRun.NextNWeekday(_duration) : nextRun;
      };
    }
  }
}
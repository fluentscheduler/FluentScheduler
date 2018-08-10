using Moong.FluentScheduler.Extension;

namespace Moong.FluentScheduler.Unit
{
  /// <summary>
  /// Unit of time that represents last day of the month.
  /// </summary>
  public sealed class MonthOnLastDayOfMonthUnit
  {
    private readonly int _duration;

    internal MonthOnLastDayOfMonthUnit(Schedule schedule, int duration)
    {
      _duration = duration;
      this.Schedule = schedule;
      this.Schedule.CalculateNextRun = x =>
      {
        var nextRun = x.Date.Last();
        return x > nextRun ? x.Date.First().AddMonths(_duration).Last() : x.Date.Last();
      };
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
        var nextRun = x.Date.Last().AddHours(hours).AddMinutes(minutes);
        return x > nextRun ? x.Date.First().AddMonths(_duration).Last().AddHours(hours).AddMinutes(minutes) : nextRun;
      };
    }
  }
}

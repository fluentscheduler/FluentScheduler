using Moong.FluentScheduler.Extension;

namespace Moong.FluentScheduler.Unit
{
  /// <summary>
  /// Unit of time that represents last day of the year.
  /// </summary>
  public sealed class YearOnLastDayOfYearUnit
  {
    private readonly int _duration;

    internal YearOnLastDayOfYearUnit(Schedule schedule, int duration)
    {
      _duration = duration;
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
      this.Schedule.CalculateNextRun = x =>
      {
        var nextRun = x.Date.FirstOfYear().AddMonths(11).Last().AddHours(hours).AddMinutes(minutes);
        return x > nextRun ? x.Date.FirstOfYear().AddYears(_duration).AddMonths(11).Last().AddHours(hours).AddMinutes(minutes) : nextRun;
      };
    }
  }
}

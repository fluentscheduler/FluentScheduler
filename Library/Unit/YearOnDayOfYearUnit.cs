using Moong.FluentScheduler.Extension;

namespace Moong.FluentScheduler.Unit
{
  /// <summary>
  /// Unit of time that represents a specific day of the year.
  /// </summary>
  public sealed class YearOnDayOfYearUnit
  {
    private readonly int _duration;

    private readonly int _dayOfYear;

    internal YearOnDayOfYearUnit(Schedule schedule, int duration, int dayOfYear)
    {
      _duration = duration;
      _dayOfYear = dayOfYear;
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
        var nextRun = x.Date.FirstOfYear().AddDays(_dayOfYear - 1).AddHours(hours).AddMinutes(minutes);
        return x > nextRun ? x.Date.FirstOfYear().AddYears(_duration).AddDays(_dayOfYear - 1).AddHours(hours).AddMinutes(minutes) : nextRun;
      };
    }
  }
}

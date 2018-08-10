namespace FluentScheduler.Unit
{
  /// <summary>
  /// Unit of time in years.
  /// </summary>
  public sealed class YearUnit
  {
    private readonly int _duration;

    internal YearUnit(Schedule schedule, int duration)
    {
      _duration = duration;
      this.Schedule = schedule;
      this.Schedule.CalculateNextRun = x =>
      {
        var nextRun = x.Date.AddYears(_duration);
        return x > nextRun ? nextRun.AddYears(_duration) : nextRun;
      };
    }

    internal Schedule Schedule { get; }

    /// <summary>
    /// Runs the job on the given day of the year.
    /// If the day has passed it schedules to the next year.
    /// </summary>
    /// <param name="day">Day of the year to run the job.</param>
    public YearOnDayOfYearUnit On(int day)
    {
      return new YearOnDayOfYearUnit(this.Schedule, _duration, day);
    }

    /// <summary>
    /// Runs the job on the last day of the year.
    /// </summary>
    public YearOnLastDayOfYearUnit OnTheLastDay()
    {
      return new YearOnLastDayOfYearUnit(this.Schedule, _duration);
    }
  }
}

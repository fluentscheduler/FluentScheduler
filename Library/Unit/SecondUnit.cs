namespace FluentScheduler.Unit
{
  /// <summary>
  /// Unit of time in seconds.
  /// </summary>
  public sealed class SecondUnit : ITimeRestrictableUnit
  {
    internal SecondUnit(Schedule schedule, int duration)
    {
      this.Schedule = schedule;
      this.Schedule.CalculateNextRun = x => x.AddSeconds(duration);
    }

    internal Schedule Schedule { get; }

    Schedule ITimeRestrictableUnit.Schedule => this.Schedule;
  }
}

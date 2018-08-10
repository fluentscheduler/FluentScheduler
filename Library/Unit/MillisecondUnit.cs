namespace FluentScheduler.Unit
{
  /// <summary>
  /// Unit of time in milliseconds.
  /// </summary>
  public sealed class MillisecondUnit : ITimeRestrictableUnit
  {
    internal MillisecondUnit(Schedule schedule, int duration)
    {
      this.Schedule = schedule;
      this.Schedule.CalculateNextRun = x => x.AddMilliseconds(duration);
    }

    internal Schedule Schedule { get; }

    Schedule ITimeRestrictableUnit.Schedule => this.Schedule;
  }
}

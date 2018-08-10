namespace FluentScheduler.Unit
{
  /// <summary>
  /// Unit of time in minutes.
  /// </summary>
  public sealed class MinuteUnit : ITimeRestrictableUnit
  {
    internal MinuteUnit(Schedule schedule, int duration)
    {
      this.Schedule = schedule;
      this.Schedule.CalculateNextRun = x => x.AddMinutes(duration);
    }

    internal Schedule Schedule { get; }

    Schedule ITimeRestrictableUnit.Schedule => this.Schedule;
  }
}

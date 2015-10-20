namespace FluentScheduler.Model
{
  public class MinuteUnit : ITimeRestrictableUnit
  {
    internal Schedule Schedule { get; private set; }
    Schedule ITimeRestrictableUnit.Schedule { get { return this.Schedule; } }
    internal int Duration { get; private set; }

    public MinuteUnit(Schedule schedule, int duration)
    {
      this.Schedule = schedule;
      this.Duration = duration;

      this.Schedule.CalculateNextRun = x => x.AddMinutes(Duration);
    }
  }
}

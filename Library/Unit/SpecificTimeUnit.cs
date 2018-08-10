using System;

namespace Moong.FluentScheduler.Unit
{
  /// <summary>
  /// Unit of specific time of the day.
  /// </summary>
  public sealed class SpecificTimeUnit
  {
    internal SpecificTimeUnit(Schedule schedule)
    {
      this.Schedule = schedule;
    }

    internal Schedule Schedule { get; }

    /// <summary>
    /// Also runs the job according to the given interval.
    /// </summary>
    /// <param name="interval">Interval to wait.</param>
    public TimeUnit AndEvery(int interval)
    {
      var parent = this.Schedule.Parent ?? this.Schedule;

      var child =
          new Schedule(this.Schedule.Jobs)
          {
            Parent = parent,
            Reentrant = parent.Reentrant,
            Name = parent.Name,
          };

      if (parent.CalculateNextRun != null)
      {
        var now = JobManager.Now;
        var delay = parent.CalculateNextRun(now) - now;

        if (delay > TimeSpan.Zero)
          child.DelayRunFor = delay;
      }

      child.Parent.AdditionalSchedules.Add(child);
      return child.ToRunEvery(interval);
    }
  }
}

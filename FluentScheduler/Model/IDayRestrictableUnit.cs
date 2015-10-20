namespace FluentScheduler.Model
{
  using System;

  public interface IDayRestrictableUnit
  {
    Schedule Schedule { get; }

    DateTime DayIncrement(DateTime x);
  }
}
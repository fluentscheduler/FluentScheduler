using System;

namespace FluentScheduler.Model
{
    public interface IDayRestrictableUnit
    {
        Schedule Schedule { get; }

        DateTime DayIncrement(DateTime toIncrement);
    }
}
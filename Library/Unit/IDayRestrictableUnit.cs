using System;

namespace FluentScheduler
{
    public interface IDayRestrictableUnit
    {
        Schedule Schedule { get; }

        DateTime DayIncrement(DateTime increment);
    }
}
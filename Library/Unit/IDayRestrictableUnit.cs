namespace FluentScheduler
{
    using System;

    public interface IDayRestrictableUnit
    {
        Schedule Schedule { get; }

        DateTime DayIncrement(DateTime increment);
    }
}
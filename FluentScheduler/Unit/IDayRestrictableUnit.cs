namespace FluentScheduler
{
    using System;

    /// <summary>
    /// Common interface of units that can be restricted by day.
    /// </summary>
    public interface IDayRestrictableUnit
    {
        /// <summary>
        /// The schedule being affected.
        /// </summary>
        Schedule Schedule { get; }

        /// <summary>
        /// Increment the given days.
        /// </summary>
        /// <param name="increment">Days to increment</param>
        /// <returns>The resulting date</returns>
        DateTime DayIncrement(DateTime increment);
    }
}
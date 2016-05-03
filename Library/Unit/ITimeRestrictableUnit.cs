namespace FluentScheduler
{
    /// <summary>
    /// Common interface of units that can be restricted by time.
    /// </summary>
    public interface ITimeRestrictableUnit
    {
        /// <summary>
        /// The schedule being affected.
        /// </summary>
        Schedule Schedule { get; }
    }
}
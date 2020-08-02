namespace FluentScheduler
{
    /// <summary>
    /// Some work to be done.
    /// Make sure there's a parameterless constructor (avoid System.MissingMethodException).
    /// </summary>
    public interface IJob
    {
        /// <summary>
        /// Executes the job.
        /// </summary>
        void Execute();
    }
}

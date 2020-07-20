namespace FluentScheduler
{
    /// <summary>
    /// Some work to be done.
    /// If you are relying on the library to instantiate the job, make sure you implement a parameterless constructor
    /// (else you will be getting a System.MissingMethodException).
    /// </summary>
    public interface IJob
    {
        /// <summary>
        /// Executes the job.
        /// </summary>
        void Execute();
    }
}

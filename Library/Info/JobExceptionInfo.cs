namespace FluentScheduler
{
    using System.Threading.Tasks;

    /// <summary>
    /// Information of an exception occurred in a job.
    /// </summary>
    public class JobExceptionInfo
    {
        /// <summary>
        /// Name of the job.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Job's task.
        /// </summary>
        public Task Task { get; set; }
    }
}

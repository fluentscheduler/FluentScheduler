namespace FluentScheduler
{
    using System;

    /// <summary>
    /// Information of a job end.
    /// </summary>
    public class JobEndInfo : JobStartInfo
    {
        /// <summary>
        /// The elapsed time of the job.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Date and time of next run.
        /// </summary>
        public DateTime? NextRun { get; set; }
    }
}

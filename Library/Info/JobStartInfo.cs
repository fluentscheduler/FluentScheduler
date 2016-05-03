namespace FluentScheduler
{
    using System;

    /// <summary>
    /// Information of a job start.
    /// </summary>
    public class JobStartInfo
    {
        /// <summary>
        /// Name of the job.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Date and time of the start.
        /// </summary>
        public DateTime StartTime { get; set; }
    }
}

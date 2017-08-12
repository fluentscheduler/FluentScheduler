namespace FluentScheduler
{
    using System;

    /// <summary>
    /// A job schedule.
    /// </summary>
    public class Schedule
    {
        private readonly Action _job;

        /// <summary>
        /// Creates a new schedule for the given job.
        /// </summary>
        /// <param name="job">Job to be scheduled</param>
        /// <returns>A schedule for the given job</returns>
        public Schedule(Action job)
        {
            _job = job;
        }
    }
}

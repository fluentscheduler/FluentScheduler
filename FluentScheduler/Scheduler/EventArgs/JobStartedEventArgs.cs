namespace FluentScheduler
{
    using System;

    /// <summary>
    /// Information about a job start.
    /// </summary>
    public class JobStartedEventArgs : EventArgs
    {
        internal JobStartedEventArgs(DateTime startTime) => StartTime = startTime;

        /// <summary>
        /// Date and time of the job start.
        /// </summary>
        public DateTime StartTime { get; }
    }
}

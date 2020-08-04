namespace FluentScheduler
{
    using System;

    /// <summary>
    /// Information about a job end.
    /// </summary>
    public class JobEndedEventArgs : EventArgs
    {
        internal JobEndedEventArgs(Exception exception, DateTime startTime, DateTime endTime, DateTime? nextRun)
        {
            Exception = exception;
            StartTime = startTime;
            EndTime = endTime;
            NextRun = nextRun;
        }

        /// <summary>
        /// Null if the job did not throw an exception, the exception object otherwise.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Duration of the job.
        /// </summary>
        public TimeSpan Duration => EndTime - StartTime;

        /// <summary>
        /// Date and time of the job start.
        /// </summary>
        public DateTime StartTime { get; }

        /// <summary>
        /// Date and time of the job end.
        /// </summary>
        public DateTime EndTime { get; }

        /// <summary>
        /// Date and time of the next job run.
        /// </summary>
        public DateTime? NextRun { get; }
    }
}

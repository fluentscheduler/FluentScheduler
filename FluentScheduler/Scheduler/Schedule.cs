namespace FluentScheduler
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A job schedule.
    /// </summary>
    public class Schedule
    {
        internal InternalSchedule Internal { get; private set; }


        /// <summary>
        /// Creates a new schedule for the given job.
        /// </summary>
        /// <param name="job">Job to be scheduled</param>
        /// <param name="cronExpression">The scheduling as a cron expression</param>
        public Schedule(Action job, string cronExpression) =>
            Internal = new InternalSchedule(job, new CronTimeCalculator(cronExpression));

        /// <summary>
        /// Creates a new schedule for the given job.
        /// </summary>
        /// <param name="job">Job to be scheduled</param>
        /// <param name="specifier">The scheduling as a fluent call</param>
        public Schedule(Action job, Action<RunSpecifier> specifier) =>
            Internal = new InternalSchedule(job, new FluentTimeCalculator(specifier));

        /// <summary>
        /// True if the schedule is running, false otherwise.
        /// </summary>
        public bool Running
        {
            get
            {
                lock (Internal.RunningLock)
                {
                    return Internal.Running();
                }
            }
        }

        /// <summary>
        /// Date and time of the next job run.
        /// </summary>
        public DateTime? NextRun { get => Internal.NextRun; }

        /// <summary>
        /// Event raised when the job starts.
        /// </summary>
        public event EventHandler<JobStartedEventArgs> JobStarted
        {
            add => Internal.JobStarted += value;
            remove => Internal.JobStarted -= value;
        }

        /// <summary>
        /// Event raised when the job ends.
        /// </summary>
        public event EventHandler<JobEndedEventArgs> JobEnded
        {
            add => Internal.JobEnded += value;
            remove => Internal.JobEnded -= value;
        }

        /// <summary>
        /// Sets the scheduling to use UTC time system.
        /// You must not call this method if the schedule is running.
        /// </summary>
        public void UseUtc()
        {
            lock (Internal.RunningLock)
            {
                Internal.UseUtc();
            }
        }

        /// <summary>
        /// Resets the scheduling of this schedule.
        /// You must not call this method if the schedule is running.
        /// </summary>
        public void ResetScheduling()
        {
            lock (Internal.RunningLock)
            {
                Internal.ShouldNotBeRunning();
                Internal.ResetScheduling();
            }
        }

        /// <summary>
        /// Changes the scheduling of this schedule.
        /// You must not call this method if the schedule is running.
        /// </summary>
        /// <param name="specifier">Scheduling of this schedule</param>
        public void SetScheduling(Action<RunSpecifier> specifier)
        {
            lock (Internal.RunningLock)
            {
                if (specifier == null)
                    throw new ArgumentNullException(nameof(specifier));

                Internal.SetScheduling(new FluentTimeCalculator(specifier));
            }
        }

        /// <summary>
        /// Starts the schedule or does nothing if it's already running.
        /// </summary>
        public void Start()
        {
            lock (Internal.RunningLock)
            {
                Internal.Start();
            }
        }

        /// <summary>
        /// Stops the schedule or does nothing if it's not running.
        /// This call does not block.
        /// </summary>
        public void Stop()
        {
            lock (Internal.RunningLock)
            {
                Internal.Stop(false);
            }
        }

        /// <summary>
        /// Stops the schedule or does nothing if it's not running.
        /// This call blocks (it waits for the running job to end its execution).
        /// </summary>
        public void StopAndBlock()
        {
            lock (Internal.RunningLock)
            {
                Internal.Stop(true);
            }
        }

        /// <summary>
        /// Stops the schedule or does nothing if it's not running.
        /// This call blocks (it waits for the running job to end its execution).
        /// </summary>
        /// <param name="timeout">Milliseconds to wait</param>
        public void StopAndBlock(int timeout)
        {
            lock (Internal.RunningLock)
            {
                Internal.Stop(true, timeout);
            }
        }

        /// <summary>
        /// Stops the schedule or does nothing if it's not running.
        /// This call blocks (it waits for the running job to end its execution).
        /// </summary>
        /// <param name="timeout">Time to wait</param>
        public void StopAndBlock(TimeSpan timeout)
        {
            lock (Internal.RunningLock)
            {
                Internal.Stop(true, timeout.Milliseconds);
            }
        }
    }
}

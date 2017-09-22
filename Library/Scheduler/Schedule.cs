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
        private readonly Action _job;

        private readonly TimeCalculator _calculator;

        private readonly object _lock;

        private Task _task;

        private CancellationTokenSource _tokenSource;

        /// <summary>
        /// Creates a new schedule for the given job.
        /// </summary>
        /// <param name="job">Job to be scheduled</param>
        /// <param name="specifier">Fluent specifier that determines when the job should run</param>
        /// <returns>A schedule for the given job</returns>
        public Schedule(Action job, Action<RunSpecifier> specifier)
        {
            _job = job ?? throw new ArgumentNullException(nameof(job));

            if (specifier == null)
                throw new ArgumentNullException(nameof(specifier));

            _calculator = new TimeCalculator();
            _lock = new object();
            _task = null;
            _tokenSource = null;

            specifier(new RunSpecifier(_calculator));
        }

        /// <summary>
        /// True if the schedule is started, false otherwise.
        /// </summary>
        public bool Running
        {
            get
            {
                lock (_lock)
                {
                    return _Running();
                }
            }
        }

        /// <summary>
        /// Date and time of the next job run.
        /// </summary>
        public DateTime? NextRun { get; private set; }

        /// <summary>
        /// Event raised when the job starts.
        /// </summary>
        public event EventHandler<JobStartedEventArgs> JobStarted;

        /// <summary>
        /// Evemt raised when the job ends.
        /// </summary>
        public event EventHandler<JobEndedEventArgs> JobEnded;

        /// <summary>
        /// Starts the schedule or does nothing if it's already running.
        /// </summary>
        public void Start()
        {
            lock (_lock)
            {
                if (_Running())
                    return;

                CalculateNextRun(DateTime.Now);

                _tokenSource = new CancellationTokenSource();
                _task = Run(_tokenSource.Token);
            }
        }

        /// <summary>
        /// Stops the schedule or does nothing if it's not running.
        /// This calls doesn't block (it doesn't wait for the running job to end its execution).
        /// </summary>
        public void Stop() => _Stop(false, null);

        /// <summary>
        /// Stops the schedule or does nothing if it's not running.
        /// This calls blocks (it waits for the running job to end its execution).
        /// </summary>
        public void StopAndBlock() => _Stop(false, null);

        /// <summary>
        /// Stops the schedule or does nothing if it's not running.
        /// This calls blocks (it waits for the running job to end its execution).
        /// </summary>
        /// <param name="timeout">Milliseconds to wait</param>
        public void StopAndBlock(int timeout)
        {
            if (timeout < 0)
                throw new ArgumentOutOfRangeException($"\"{nameof(timeout)}\" should be positive.");

            _Stop(false, timeout);
        }

        /// <summary>
        /// Stops the schedule or does nothing if it's not running.
        /// This calls blocks (it waits for the running job to end its execution).
        /// </summary>
        /// <param name="timeout">Time to wait</param>
        public void StopAndBlock(TimeSpan timeout)
        {
            if (timeout < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException($"\"{nameof(timeout)}\" should be positive.");

            _Stop(false, timeout.Milliseconds);
        }

        private void CalculateNextRun(DateTime last) => NextRun = _calculator.Calculate(last);

        private async Task Run(CancellationToken token)
        {
            // checking if it's supposed to run
            // it assumes that CalculateNextRun has been called previously from somewhere else
            if (!NextRun.HasValue)
                return;

            // calculating delay
            var delay = NextRun.Value - DateTime.Now;

            // delaying until it's time to run or a cancellation was requested
            await Task.Delay(delay < TimeSpan.Zero ? TimeSpan.Zero : delay, token);

            // checking if a cancellation was requested
            if (token.IsCancellationRequested)
                return;

            // used on both JobStarted and JobEnded events
            var startTime = DateTime.Now;

            // raising JobStarted event
            JobStarted?.Invoke(this, new JobStartedEventArgs(startTime));

            // used on JobEnded event
            Exception exception = null;

            try
            {
                // running the job
                _job();
            }
            catch (Exception e)
            {
                // catching the exception if any
                exception = e;
            }

            // used on JobEnded event
            var endTime = DateTime.Now;

            // calculating the next run
            // used on both JobEnded event and for the next run of this method
            CalculateNextRun(startTime);

            // raising JobEnded event
            JobEnded?.Invoke(this, new JobEndedEventArgs(exception, startTime, endTime, NextRun));

            // recursive call
            // note that the NextRun was already calculated in this run
            _task = Run(token);
        }

        private bool _Running()
        {
            // task and token source should be both null or both not null
            Debug.Assert(
                (_task == null && _tokenSource == null) ||
                (_task != null && _tokenSource != null)
            );

            return _task != null;
        }

        private void _Stop(bool block, int? timeout)
        {
            lock (_lock)
            {
                if (!_Running())
                    return;

                _tokenSource.Cancel();
                _tokenSource.Dispose();

                if (block && timeout.HasValue)
                    _task.Wait(timeout.Value);

                if (block && !timeout.HasValue)
                    _task.Wait();

                _task = null;
                _tokenSource = null;
            }
        }
    }
}

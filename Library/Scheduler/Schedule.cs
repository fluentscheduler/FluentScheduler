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
            _job = job;
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
        /// Starts the schedule.
        /// </summary>
        /// <returns>
        /// True if the schedule is started, false if the scheduled was already started and the call did nothing
        /// </returns>
        public bool Start()
        {
            lock (_lock)
            {
                if (_Running())
                    return false;

                _tokenSource = new CancellationTokenSource();
                _task = Run(_tokenSource.Token);

                return true;
            }
        }

        /// <summary>
        /// Stops the schedule.
        /// This calls doesn't block (it doesn't wait for the running job to end its execution).
        /// </summary>
        /// <returns>
        /// True if the schedule is stopped, false if the scheduled wasn't started and the call did nothing
        /// </returns>
        public bool Stop()
        {
            return _Stop(false, null);
        }

        /// <summary>
        /// Stops the schedule.
        /// This calls blocks (it waits for the running job to end its execution).
        /// </summary>
        /// <returns>
        /// True if the schedule is stopped, false if the scheduled wasn't started and the call did nothing
        /// </returns>
        public bool StopAndBlock()
        {
            return _Stop(false, null);
        }

        /// <summary>
        /// Stops the schedule.
        /// This calls blocks (it waits for the running job to end its execution).
        /// </summary>
        /// <param name="millisecondsTimeout">Milliseconds to wait</param>
        /// <returns>
        /// True if the schedule is stopped, false if the scheduled wasn't started and the call did nothing
        /// </returns>
        public bool StopAndBlock(int millisecondsTimeout)
        {
            return _Stop(false, millisecondsTimeout);
        }

        /// <summary>
        /// Stops the schedule.
        /// This calls blocks (it waits for the running job to end its execution).
        /// </summary>
        /// <param name="timeout">Time to wait</param>
        /// <returns>
        /// True if the schedule stopped, false if the scheduled wasn't started and the call did nothing
        /// </returns>
        public bool StopAndBlock(TimeSpan timeout)
        {
            return _Stop(false, timeout.Milliseconds);
        }

        private TimeSpan? CalculateDelay()
        {
            var now = DateTime.Now;
            var next = _calculator.Calculate(now);

            return next.HasValue ? next.Value - now : null as TimeSpan?;
        }

        private async Task Run(CancellationToken token)
        {
            var delay = CalculateDelay();

            if (!delay.HasValue)
                return;

            await Task.Delay(delay.Value, token);

            _job();

            _task = Run(token);
        }

        private bool _Running()
        {
            Debug.Assert(
                (_task == null && _tokenSource == null) ||
                (_task != null && _tokenSource != null)
            );

            return _task != null;
        }

        private bool _Stop(bool block, int? timeout)
        {
            lock (_lock)
            {
                if (!_Running())
                    return false;

                _tokenSource.Cancel();
                _tokenSource.Dispose();

                if (block && timeout.HasValue)
                    _task.Wait(timeout.Value);

                if (block && !timeout.HasValue)
                    _task.Wait();

                _task = null;
                _tokenSource = null;

                return true;
            }
        }
    }
}

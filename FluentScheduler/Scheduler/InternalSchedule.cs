namespace FluentScheduler
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    internal class InternalSchedule
    {
        internal ITimeCalculator Calculator;

        private readonly Func<CancellationToken, Task> _job;

        private Task _task;

        private CancellationTokenSource _tokenSource;

        internal InternalSchedule(Func<CancellationToken, Task> job, ITimeCalculator calculator)
        {
            _job = job ?? throw new ArgumentNullException(nameof(job));

            SetScheduling(calculator);
        }

        internal DateTime? NextRun { get; private set; }

        internal object RunningLock { get; } = new object();

        internal event EventHandler<JobStartedEventArgs> JobStarted;

        internal event EventHandler<JobEndedEventArgs> JobEnded;

        internal void ResetScheduling()
        {
            NextRun = null;
            Calculator.Reset();
        }

        internal void SetScheduling(ITimeCalculator calculator)
        {
            NextRun = null;
            Calculator = calculator;
        }

        internal void ShouldNotBeRunning()
        {
            if (Running())
                throw new InvalidOperationException("You cannot change the scheduling of a running schedule.");
        }

        internal bool Running()
        {
            Debug.Assert(
                (_task == null && _tokenSource == null) ||
                (_task != null && _tokenSource != null)
            );

            return _task != null;
        }

        internal void Start()
        {
            if (Running())
                return;

            CalculateNextRun(Calculator.Now());

            _tokenSource = new CancellationTokenSource();
            _task = Run(_tokenSource.Token);
        }

        internal void Stop(bool block, int? timeout = null)
        {
            if (timeout.HasValue && timeout < 0)
                throw new ArgumentOutOfRangeException($"\"{nameof(timeout)}\" should be positive.");

            if (!Running())
                return;

            _tokenSource.Cancel();
            try
            {
                if (block && timeout.HasValue)
                    _task.Wait(timeout.Value);

                if (block && !timeout.HasValue)
                    _task.Wait();
            }
            finally
            {
                _tokenSource.Dispose();
                _task = null;
                _tokenSource = null;
            }
        }

        internal void UseUtc()
        {
            if (Running())
                return;

            Calculator.Now = () => DateTime.UtcNow;
        }

        private void CalculateNextRun(DateTime last) => NextRun = Calculator.Calculate(last);

        private async Task Run(CancellationToken token)
        {
            // checking if it's supposed to run
            // it assumes that CalculateNextRun has been called previously from somewhere else
            if (!NextRun.HasValue)
                return;

            // calculating delay
            var delay = NextRun.Value - Calculator.Now();

            // delaying until it's time to run or a cancellation was requested
            await Task.Delay(delay < TimeSpan.Zero ? TimeSpan.Zero : delay, token).ContinueWith(_ => {});

            // checking if a cancellation was requested
            if (token.IsCancellationRequested)
                return;

            // used on both JobStarted and JobEnded events
            var startTime = Calculator.Now();

            // raising JobStarted event
            JobStarted?.Invoke(this, new JobStartedEventArgs(startTime));

            // used on JobEnded event
            Exception exception = null;

            try
            {
                // running the job
                await _job(token);
            }
            catch (Exception e)
            {
                // catching the exception if any
                exception = e;
            }

            // used on JobEnded event
            var endTime = Calculator.Now();

            // calculating the next run
            // used on both JobEnded event and for the next run of this method
            CalculateNextRun(startTime);

            // raising JobEnded event
            JobEnded?.Invoke(this, new JobEndedEventArgs(exception, startTime, endTime, NextRun));

            // recursive call
            // note that the NextRun was already calculated in this run
            _task = Run(token);
        }
    }
}
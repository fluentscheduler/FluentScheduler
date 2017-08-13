namespace FluentScheduler
{
    using System;
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

            specifier(new RunSpecifier(_calculator));
        }

        /// <summary>
        /// Starts the schedule.
        /// If the schedule already started it does nothing.
        /// </summary>
        public void Start()
        {
            lock (_lock)
            {
                _tokenSource = new CancellationTokenSource();

                #pragma warning disable CS4014
                Run(_tokenSource.Token);
                #pragma warning restore CS4014
            }
        }

        private async Task Run(CancellationToken token)
        {
            var delay = CalculateDelay();

            if (!delay.HasValue)
                return;

            await Task.Delay(delay.Value, token);

            _job();

            #pragma warning disable CS4014
            Run(token);
            #pragma warning restore CS4014
        }

        private TimeSpan? CalculateDelay()
        {
            var now = DateTime.Now;
            var next = _calculator.Calculate(now);

            return next.HasValue ? next.Value - now : null as TimeSpan?;
        }
    }
}

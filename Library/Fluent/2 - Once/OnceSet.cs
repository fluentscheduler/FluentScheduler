namespace FluentScheduler
{
    using System;

    /// <summary>
    /// The "once" run has been set.
    /// </summary>
    public class OnceSet
    {
        private readonly TimeCalculator _calculator;

        internal OnceSet(TimeCalculator calculator) => _calculator = calculator;

        /// <summary>
        /// Runs the job according to the given interval.
        /// </summary>
        /// <param name="interval">Interval (without unit) to wait</param>
        public PeriodDurationSet AndEvery(int interval)
        {
            if (interval < 0)
                throw new ArgumentOutOfRangeException($"\"{nameof(interval)}\" should be positive.");

            return new PeriodDurationSet(interval, _calculator);
        }
    }
}

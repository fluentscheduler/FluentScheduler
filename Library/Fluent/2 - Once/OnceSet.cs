namespace FluentScheduler
{
    using System;

    public class OnceSet
    {
        private readonly TimeCalculator _calculator;

        internal OnceSet(TimeCalculator calculator)
        {
            _calculator = calculator;
        }

        public PeriodDurationSet AndEvery(int duration)
        {
            if (duration < 0)
                throw new ArgumentOutOfRangeException($"\"{nameof(duration)}\" should be positive.");

            return new PeriodDurationSet(duration, _calculator);
        }
    }
}

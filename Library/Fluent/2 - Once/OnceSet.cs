namespace FluentScheduler
{
    public class OnceSet
    {
        private readonly TimeCalculator _calculator;

        internal OnceSet(TimeCalculator calculator)
        {
            _calculator = calculator;
        }

        public PeriodDurationSet AndEvery(int duration)
        {
            return new PeriodDurationSet(duration, _calculator);
        }
    }
}

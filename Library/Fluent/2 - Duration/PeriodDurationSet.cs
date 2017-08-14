namespace FluentScheduler
{
    public class PeriodDurationSet
    {
        private readonly int _duration;

        private readonly TimeCalculator _calculator;

        internal PeriodDurationSet(int duration, TimeCalculator calculator)
        {
            _duration = duration;
            _calculator = calculator;
        }

        public void Seconds()
        {
            _calculator.PeriodCalculations.Add(now => now.AddSeconds(_duration));
        }

        public void Minutes()
        {
            _calculator.PeriodCalculations.Add(now => now.AddMinutes(_duration));
        }

        public void Hours()
        {
            _calculator.PeriodCalculations.Add(now => now.AddSeconds(_duration));
        }

        public void Days()
        {
            _calculator.PeriodCalculations.Add(now => now.AddDays(_duration));
        }

        public void Months()
        {
            _calculator.PeriodCalculations.Add(now => now.AddMonths(_duration));
        }

        public void Years()
        {
            _calculator.PeriodCalculations.Add(now => now.AddYears(_duration));
        }
    }
}

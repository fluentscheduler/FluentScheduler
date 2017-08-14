namespace FluentScheduler
{
    public class OnceDurationSet
    {
        private readonly int _duration;

        private readonly TimeCalculator _calculator;

        internal OnceDurationSet(int duration, TimeCalculator calculator)
        {
            _duration = duration;
            _calculator = calculator;
        }

        public void Seconds()
        {
            _calculator.OnceCalculation = now => now.AddSeconds(_duration);
        }

        public void Minutes()
        {
            _calculator.OnceCalculation = now => now.AddMinutes(_duration);
        }

        public void Hours()
        {
            _calculator.OnceCalculation = now => now.AddHours(_duration);
        }

        public void Days()
        {
            _calculator.OnceCalculation = now => now.AddDays(_duration);
        }

        public void Months()
        {
            _calculator.OnceCalculation = now => now.AddMonths(_duration);
        }

        public void Years()
        {
            _calculator.OnceCalculation = now => now.AddYears(_duration);
        }
    }
}

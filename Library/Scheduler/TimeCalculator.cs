namespace FluentScheduler
{
    using System;

    internal class TimeCalculator
    {
        private bool _firstCalculation = true;

        internal Func<DateTime, DateTime> OnceCalculation { get; set; }

        public DateTime? Calculate(DateTime now)
        {
            return CalculateOnce(now);
        }

        private DateTime? CalculateOnce(DateTime now)
        {
            if (!_firstCalculation)
                return null;

            _firstCalculation = false;
            return OnceCalculation?.Invoke(now);
        }
    }
}

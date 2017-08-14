namespace FluentScheduler
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal class TimeCalculator
    {
        private bool _firstCalculation = true;

        internal Func<DateTime, DateTime> OnceCalculation { get; set; }

        internal IList<Func<DateTime, DateTime>> PeriodCalculations { get; private set; } =
            new List<Func<DateTime, DateTime>>();

        public DateTime? Calculate(DateTime now)
        {
            return CalculateOnce(now) ?? CalculatePeriod(now);
        }

        private DateTime? CalculateOnce(DateTime now)
        {
            if (!_firstCalculation)
                return null;

            _firstCalculation = false;
            return OnceCalculation?.Invoke(now);
        }

        private DateTime? CalculatePeriod(DateTime now)
        {
            if (!PeriodCalculations.Any())
                return null;

            var next = now;

            foreach (var calc in PeriodCalculations)
                next = calc(next);

            return next;
        }
    }
}

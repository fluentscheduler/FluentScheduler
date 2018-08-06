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

        internal void Reset() => _firstCalculation = true;

        internal DateTime? Calculate(DateTime last) => CalculateOnce(last) ?? CalculatePeriod(last);

        private DateTime? CalculateOnce(DateTime last)
        {
            if (!_firstCalculation)
                return null;

            _firstCalculation = false;
            return OnceCalculation?.Invoke(last);
        }

        private DateTime? CalculatePeriod(DateTime last)
        {
            if (!PeriodCalculations.Any())
                return null;

            var next = last;

            foreach (var calc in PeriodCalculations)
                next = calc(next);

            return next;
        }
    }
}

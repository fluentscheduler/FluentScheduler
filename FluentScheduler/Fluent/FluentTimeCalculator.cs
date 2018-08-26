namespace FluentScheduler
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal class TimeCalculator : ITimeCalculator
    {
        private bool _firstCalculation = true;

        internal Func<DateTime, DateTime> OnceCalculation { get; set; }

        internal IList<Func<DateTime, DateTime>> PeriodCalculations { get; private set; } =
            new List<Func<DateTime, DateTime>>();

        internal TimeCalculator() { }

        internal TimeCalculator(Action<RunSpecifier> specifier) => specifier(new RunSpecifier(this));

        public void Reset() => _firstCalculation = true;

        public DateTime? Calculate(DateTime last) => CalculateOnce(last) ?? CalculatePeriod(last);

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

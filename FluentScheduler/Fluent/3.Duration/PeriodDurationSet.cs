namespace FluentScheduler
{
    /// <summary>
    /// Duration of "period" run has been set, but not its unit.
    /// </summary>
    public class PeriodDurationSet
    {
        private readonly int _duration;

        private readonly FluentTimeCalculator _calculator;

        internal PeriodDurationSet(int duration, FluentTimeCalculator calculator)
        {
            _duration = duration;
            _calculator = calculator;
        }

        /// <summary>
        /// Sets the unit as seconds.
        /// </summary>
        public void Seconds() => _calculator.PeriodCalculations.Add(last => last.AddSeconds(_duration));

        /// <summary>
        /// Sets the unit as minutes.
        /// </summary>
        public void Minutes() => _calculator.PeriodCalculations.Add(last => last.AddMinutes(_duration));

        /// <summary>
        /// Sets the unit as hours.
        /// </summary>
        public void Hours() => _calculator.PeriodCalculations.Add(last => last.AddHours(_duration));

        /// <summary>
        /// Sets the unit as days.
        /// </summary>
        public RestrictionUnit Days()
        {
            _calculator.PeriodCalculations.Add(last => last.AddDays(_duration));

            return new RestrictionUnit(_calculator);
        }

        /// <summary>
        /// Sets the unit as weeks.
        /// </summary>
        public RestrictionUnit Weeks()
        {
            _calculator.PeriodCalculations.Add(last => last.AddDays(7 * _duration));
            return new RestrictionUnit(_calculator);
        }

        /// <summary>
        /// Sets the unit as months.
        /// </summary>
        public MonthUnit Months()
        {
            _calculator.PeriodCalculations.Add(last => last.AddMonths(_duration));
            return new MonthUnit(_calculator);
        }
    }
}



namespace FluentScheduler
{
    using System;

    /// <summary>
    /// The days the job should run
    /// </summary>
    public class DayUnit
    {
        private readonly FluentTimeCalculator _calculator;

        internal DayUnit(FluentTimeCalculator calculator) => _calculator = calculator;

        /// <summary>
        /// Runs the job only on weekdays.
        /// </summary>
        public RestrictionUnit Weekday()
        {
            _calculator.PeriodCalculations.Add(last =>
            {
                if ((last.DayOfWeek == DayOfWeek.Saturday) || (last.DayOfWeek == DayOfWeek.Sunday))
                    last = last.AddDays(((int)last.DayOfWeek / 6) + 1);

                return last;
            });

            return new RestrictionUnit(_calculator);
        }

        /// <summary>
        /// Runs the job only on the weekends.
        /// </summary>
        public RestrictionUnit Weekend()
        {
            _calculator.PeriodCalculations.Add(last =>
            {
                if ((last.DayOfWeek != DayOfWeek.Saturday) && (last.DayOfWeek != DayOfWeek.Sunday))
                        last = last.AddDays(DayOfWeek.Saturday - last.DayOfWeek);

                return last;
            });

            return new RestrictionUnit(_calculator);
        }
    }
}

namespace FluentScheduler
{
    using System;

    /// <summary>
    /// The months the job should run
    /// </summary>
    public class MonthUnit
    {
        private readonly FluentTimeCalculator _calculator;

        internal MonthUnit(FluentTimeCalculator calculator) => _calculator = calculator;

        /// <summary>
        /// Runs the job on the given day of the month.
        /// </summary>
        /// <param name="day">The day (1 through the number of days in month)</param>
        public RestrictionUnit On(int day)
        {
            _calculator.PeriodCalculations.Add(
                last => new DateTime(last.Year, last.Month, day, last.Hour, last.Minute, last.Second));

            return new RestrictionUnit(_calculator);
        }
    }
}

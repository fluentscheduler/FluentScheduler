namespace FluentScheduler
{
    using System;
    using System.Linq;

    public class MonthUnit
    {
        private readonly TimeCalculator _calculator;

        internal MonthUnit(TimeCalculator calculator) => _calculator = calculator;

        /// <summary>
        /// Runs the job on the given day of the month.
        /// </summary>
        /// <param name="day">The day (1 through the number of days in month)</param>
        public PeriodOnceSet On(int day)
        {
            _calculator.PeriodCalculations.Add(
                last => new DateTime(last.Year, last.Month, day, last.Hour, last.Minute, last.Second));

            return new PeriodOnceSet(_calculator);
        }

        /// <summary>
        /// Runs the job on the given day of week on the first week of the month.
        /// </summary>
        /// <param name="day">The day of the week</param>
        public PeriodOnceSet OnTheFirstDay(DayOfWeek day)
        {
            _calculator.PeriodCalculations.Add(last => Next(last.Date.Year, last.Date.Month, day, 1));
            return new PeriodOnceSet(_calculator);
        }

        /// <summary>
        /// Runs the job on the given day of week on the second week of the month.
        /// </summary>
        /// <param name="day">The day of the week</param>
        public PeriodOnceSet OnTheSecondDay(DayOfWeek day)
        {
            _calculator.PeriodCalculations.Add(last => Next(last.Date.Year, last.Date.Month, day, 2));
            return new PeriodOnceSet(_calculator);
        }

        /// <summary>
        /// Runs the job on the given day of week on the third week of the month.
        /// </summary>
        /// <param name="day">The day of the week</param>
        public PeriodOnceSet OnTheThirdDay(DayOfWeek day)
        {
            _calculator.PeriodCalculations.Add(last => Next(last.Date.Year, last.Date.Month, day, 3));
            return new PeriodOnceSet(_calculator);
        }

        /// <summary>
        /// Runs the job on the given day of week on the fourth week of the month.
        /// </summary>
        /// <param name="day">The day of the week</param>
        public PeriodOnceSet OnTheFourthDay(DayOfWeek day)
        {
            _calculator.PeriodCalculations.Add(last => Next(last.Date.Year, last.Date.Month, day, 4));
            return new PeriodOnceSet(_calculator);
        }

        private static DateTime Next(int year, int month, DayOfWeek dayOfWeek, int occurrence) =>
            Enumerable.Range(1, 7)
                .Select(day => new DateTime(year, month, day))
                .First(dateTime => dateTime.DayOfWeek == dayOfWeek)
                .AddDays(7 * (occurrence - 1));
    }
}

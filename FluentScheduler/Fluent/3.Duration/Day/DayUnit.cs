namespace FluentScheduler
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using static System.DayOfWeek;

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
                var daysToNextWeekday = 1;

                if (last.DayOfWeek == Friday)
                    daysToNextWeekday = 3;
                
                if (last.DayOfWeek == Saturday)
                    daysToNextWeekday = 2;

                last = last.AddDays(daysToNextWeekday);

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
                var daysToNextWeekend = 6 - (int)last.DayOfWeek;
                
                if (last.DayOfWeek == Saturday)
                    daysToNextWeekend = 1;

                if (last.DayOfWeek == Sunday)
                    daysToNextWeekend = 6;

                last = last.AddDays(daysToNextWeekend);

                return last;
            });

            return new RestrictionUnit(_calculator);
        }
    }
}

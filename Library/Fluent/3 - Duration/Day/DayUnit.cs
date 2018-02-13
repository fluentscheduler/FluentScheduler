using System;

namespace FluentScheduler
{
    public class DayUnit
    {
        private readonly TimeCalculator _calculator;

        public DayUnit(TimeCalculator calculator) => _calculator = calculator;


        /// <summary>
        /// Runs the job only on weekdays.
        /// </summary>
       public TimeSet Weekday()
       {
            _calculator.PeriodCalculations.Add(last =>
            {
                if ((last.DayOfWeek == DayOfWeek.Saturday) || (last.DayOfWeek == DayOfWeek.Sunday))
                    last.AddDays(last.DayOfWeek - DayOfWeek.Monday);

                return last;
            });

            return new TimeSet(_calculator);
        }

        /// <summary>
        /// Runs the job only on the weekends.
        /// </summary>
        public TimeSet Weekend()
        {
            _calculator.PeriodCalculations.Add(last =>
            {
                if ((last.DayOfWeek != DayOfWeek.Saturday) && (last.DayOfWeek != DayOfWeek.Sunday))
                    last.AddDays(last.DayOfWeek - DayOfWeek.Saturday);

                return last;
            });

            return new TimeSet(_calculator);
        }
    }
}

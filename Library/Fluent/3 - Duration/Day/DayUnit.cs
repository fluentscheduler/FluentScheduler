namespace FluentScheduler
{
	using System;

	public class DayUnit
    {
       private readonly TimeCalculator _calculator;

       internal DayUnit(TimeCalculator calculator) => _calculator = calculator;

        /// <summary>
        /// Runs the job only on weekdays.
        /// </summary>
       public PeriodOnceSet Weekday()
       {
            _calculator.PeriodCalculations.Add(last =>
            {
                if ((last.DayOfWeek == DayOfWeek.Saturday) || (last.DayOfWeek == DayOfWeek.Sunday))
                    last = last.AddDays(((int)last.DayOfWeek / 6) + 1);

                return last;
            });

            return new PeriodOnceSet(_calculator);
        }

        /// <summary>
        /// Runs the job only on the weekends.
        /// </summary>
        public PeriodOnceSet Weekend()
        {
            _calculator.PeriodCalculations.Add(last =>
            {
                if ((last.DayOfWeek != DayOfWeek.Saturday) && (last.DayOfWeek != DayOfWeek.Sunday))
                     last = last.AddDays(DayOfWeek.Saturday - last.DayOfWeek);

                return last;
            });

            return new PeriodOnceSet(_calculator);
        }
    }
}

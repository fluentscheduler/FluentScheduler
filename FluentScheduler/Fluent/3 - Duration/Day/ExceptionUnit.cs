namespace FluentScheduler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ExceptionUnit
    {
        private readonly FluentTimeCalculator _calculator;

        internal ExceptionUnit(FluentTimeCalculator calculator) => _calculator = calculator;

        /// <summary>
        /// Excludes given days from job scheduling.
        /// </summary>
        /// <param name="exceptionalDays">Days to exclude</param>
        public void Except(params DayOfWeek[] exceptionalDays)
        {
            var allDays = (IEnumerable<DayOfWeek>)Enum.GetValues(typeof(DayOfWeek));

            if (allDays.All(day => exceptionalDays.Contains(day)))
                throw new ArgumentException($"\"{nameof(exceptionalDays)}\" cannot contain all days of week.");

            _calculator.PeriodCalculations.Add(last => 
            {
                while (exceptionalDays.Contains(last.DayOfWeek))
                    last = last.AddDays(1);

                return last;
            });
        }
    }
}
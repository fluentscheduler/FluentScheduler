namespace FluentScheduler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RestrictionUnit
    {
        private readonly FluentTimeCalculator _calculator;

        internal RestrictionUnit(FluentTimeCalculator calculator) => _calculator = calculator;

        /// <summary>
        /// Runs the job at the given time of day (military format).
        /// </summary>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        public void At(int hour, int minute) => new PeriodOnceSet(_calculator).At(hour, minute);

        public void Between(TimeSpan from, TimeSpan to) => new PeriodOnceSet(_calculator).Between(from, to);

        /// <summary>
        /// Runs the job at the given time of day.
        /// </summary>
        /// <param name="timeCollection">Time of day</param>
        public void At(params TimeSpan[] timeCollection) => new PeriodOnceSet(_calculator).At(timeCollection);

        /// <summary>
        /// Excludes given days from job scheduling.
        /// </summary>
        /// <param name="exceptionalDays">Days to exclude</param>
        public PeriodOnceSet Except(params DayOfWeek[] exceptionalDays)
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

            return new PeriodOnceSet(_calculator);
        }
    }
}
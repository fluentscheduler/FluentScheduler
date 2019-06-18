namespace FluentScheduler
{
    using System;
    using System.Collections.Generic;

    public class PeriodOnceSet
    {
        private readonly FluentTimeCalculator _calculator;

        internal PeriodOnceSet(FluentTimeCalculator calculator) => _calculator = calculator;

        /// <summary>
        /// Runs the job at the given time of day (military format).
        /// </summary>
        /// <param name="hour">The hours (0 through 23).</param>
        /// <param name="minute">The minutes (0 through 59).</param>
        public void At(int hour, int minute)
        {
            if (hour < 0 || hour > 23)
                throw new ArgumentOutOfRangeException($"\"{nameof(hour)}\" should be in the 0 to 23 range.");

            if (minute < 0 || minute > 59)
                throw new ArgumentOutOfRangeException($"\"{nameof(minute)}\" should be in the 0 to 59 range.");

            _calculator.PeriodCalculations.Add(last => EarlierDate(last, new TimeSpan(hour, minute, 0)));
        }

        /// <summary>
        /// Runs the job at the given time of day.
        /// </summary>
        /// <param name="timeCollection">Time of day</param>
        public void At(params TimeSpan[] timeCollection) =>
            _calculator.PeriodCalculations.Add(last => EarlierDate(last, timeCollection));

        internal DateTime EarlierDate(DateTime last, params TimeSpan[] timeCollection)
        {
            var now = ((ITimeCalculator)_calculator).Now();
            var calculatedDate = new DateTime();

            foreach (var time in timeCollection)
            {
                var current = new DateTime(now.Year, now.Month, now.Day).Add(time);
                var next = new DateTime(last.Year, last.Month, last.Day).Add(time);

                if (current.Date < last.Date)
                {
                    calculatedDate = next;
                    break;
                }

                if (current.TimeOfDay > now.TimeOfDay)
                {
                    calculatedDate = current;
                    break;
                }
                else
                    calculatedDate = next;
            }

            return calculatedDate;
        }
    }
}

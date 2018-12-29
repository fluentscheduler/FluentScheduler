namespace FluentScheduler
{
    using System;

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

            _calculator.PeriodCalculations.Add(last =>
            {
                var now = DateTime.Now;
                var next = new DateTime(last.Year, last.Month, last.Day, hour, minute, 0);

                return now.TimeOfDay < next.TimeOfDay ?
                    new DateTime(now.Year, now.Month, now.Day, hour, minute, 0) :
                    next;
            });
        }

        /// <summary>
        /// Runs the job at the given time of day.
        /// </summary>
        /// <param name="time">Time of day</param>
        public void At(TimeSpan time) =>
            _calculator.PeriodCalculations.Add(last =>
                    new DateTime(last.Year, last.Month, last.Day, time.Hours, time.Minutes, time.Seconds));
    }
}

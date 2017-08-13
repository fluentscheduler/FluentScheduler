namespace FluentScheduler
{
    using System;

    /// <summary>
    /// Allows you to fluently specify when the job should run.
    /// </summary>
    public class RunSpecifier
    {
        private readonly TimeCalculator _calculator;

        internal RunSpecifier(TimeCalculator calculator)
        {
            _calculator = calculator;
        }

        /// <summary>
        /// Runs the job now.
        /// </summary>
        public void Now()
        {
            _calculator.OnceCalculation = now => now;
        }

        /// <summary>
        /// Runs the job once at the given time time of day.
        /// </summary>
        /// <param name="hours">The hours (0 to 23).</param>
        /// <param name="minutes">The minutes (0 to 59).</param>
        public void OnceAt(int hours, int minutes)
        {
            OnceAt(new TimeSpan(hours, minutes, 0));
        }

        /// <summary>
        /// Runs the job once at the given time of day.
        /// </summary>
        /// <param name="timeOfDay">Time of the day to run</param>
        public void OnceAt(TimeSpan timeOfDay)
        {
            timeOfDay = new TimeSpan(timeOfDay.Hours, timeOfDay.Minutes, timeOfDay.Seconds);

            _calculator.OnceCalculation = now =>
            {
                var result = now.Date.Add(timeOfDay);
                return result > now ? result : result.AddDays(1);
            };
        }

        /// <summary>
        /// Runs the job once at the given date and time.
        /// </summary>
        /// <param name="dateTime">Date and time to run</param>
        public void OnceAt(DateTime dateTime)
        {
            _calculator.OnceCalculation = now => dateTime;
        }

        /// <summary>
        /// Runs the job once after the given delay.
        /// </summary>
        /// <param name="delay">Delay to wait</param>
        public void OnceIn(TimeSpan delay)
        {
            _calculator.OnceCalculation = now =>
            {
                var next = now.Add(delay);
                return next > now ? next : now;
            };
        }
    }
}

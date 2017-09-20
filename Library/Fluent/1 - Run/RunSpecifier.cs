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

        public PeriodDurationSet Every(int duration)
        {
            return new PeriodDurationSet(duration, _calculator);
        }

        /// <summary>
        /// Runs the job now.
        /// </summary>
        public OnceSet Now()
        {
            _calculator.OnceCalculation = now => now;
            return new OnceSet(_calculator);
        }

        /// <summary>
        /// Runs the job once at the given time time of day.
        /// </summary>
        /// <param name="hours">The hours (0 to 23).</param>
        /// <param name="minutes">The minutes (0 to 59).</param>
        public OnceSet OnceAt(int hours, int minutes)
        {
            OnceAt(new TimeSpan(hours, minutes, 0));
            return new OnceSet(_calculator);
        }

        /// <summary>
        /// Runs the job once at the given time of day.
        /// </summary>
        /// <param name="timeOfDay">Time of the day to run</param>
        public OnceSet OnceAt(TimeSpan timeOfDay)
        {
            timeOfDay = new TimeSpan(timeOfDay.Hours, timeOfDay.Minutes, timeOfDay.Seconds);

            _calculator.OnceCalculation = now =>
            {
                var result = now.Date.Add(timeOfDay);
                return result > now ? result : result.AddDays(1);
            };

            return new OnceSet(_calculator);
        }

        /// <summary>
        /// Runs the job once at the given date and time.
        /// </summary>
        /// <param name="dateTime">Date and time to run</param>
        public OnceSet OnceAt(DateTime dateTime)
        {
            _calculator.OnceCalculation = now => dateTime;
            return new OnceSet(_calculator);
        }

        public OnceDurationSet OnceIn(int duration)
        {
            _calculator.OnceCalculation = now => now;
            return new OnceDurationSet(duration, _calculator);
        }

        /// <summary>
        /// Runs the job once after the given delay.
        /// </summary>
        /// <param name="delay">Delay to wait</param>
        public OnceSet OnceIn(TimeSpan delay)
        {
            _calculator.OnceCalculation = now => now.Add(delay);
            return new OnceSet(_calculator);
        }
    }
}

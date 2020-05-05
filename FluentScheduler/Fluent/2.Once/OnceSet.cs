namespace FluentScheduler
{
    using System;

    /// <summary>
    /// The "once" run has been set.
    /// </summary>
    public class OnceSet                                    
    {
        private readonly FluentTimeCalculator _calculator;

        internal OnceSet(FluentTimeCalculator calculator) => _calculator = calculator;

        /// <summary>
        /// Runs the job according to the given interval.
        /// </summary>
        /// <param name="interval">Interval (without unit) to wait</param>
        public PeriodDurationSet AndEvery(int interval)
        {
            if (interval <= 0)
                throw new ArgumentOutOfRangeException($"\"{nameof(interval)}\" should be positive.");

            return new PeriodDurationSet(interval, _calculator);
        }

        /// <summary>
        /// Runs the job according to the given interval.
        /// </summary>
        /// <param name="day">Day to run the job</param>
        public RestrictionUnit AndEvery(DayOfWeek day)
        {
            _calculator.PeriodCalculations.Add(last => 
            {
                if (last.DayOfWeek != day)
                    last = last.AddDays(7 - (int)last.DayOfWeek);

                return last;
            });

            return new RestrictionUnit(_calculator);
        }

        /// <summary>
        /// Runs the job every weekday
        /// </summary>
        public RestrictionUnit AndEveryWeekday() => new DayUnit(_calculator).Weekday();

        /// <summary>
        /// Runs the job every weekend
        /// </summary>
        public RestrictionUnit AndEveryWeekend() => new DayUnit(_calculator).Weekend();
    }
}

namespace FluentScheduler
{
    using System;

    /// <summary>
    /// The "period" run has been set, but not its unit.
    /// </summary>
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

            _calculator.PeriodCalculations.Add(last => new DateTime(last.Year, last.Month, last.Day, hour, minute, 0));
        }

        /// <summary>
        /// Runs the job at the given time of day.
        /// </summary>
        /// <param name="timeCollection">Time of day.</param>
        public void At(params TimeSpan[] timeCollection) 
        {
            foreach (var time in timeCollection)
            {
                if (time >= ((ITimeCalculator)_calculator).Now().TimeOfDay)
                {
                    _calculator.PeriodCalculations.Add(last => 
                        new DateTime(last.Year, last.Month, last.Day, time.Hours, time.Minutes, 0)
                    );
                    
                    break;
                }
            }
        }

        /// <summary>
        /// Runs the job at the given period of time.
        /// </summary>
        /// <param name="hourFrom">Hour to delimitate period beginning.</param>
        /// <param name="minuteFrom">Minute to delimitade period beginning.</param>
        /// <param name="hourTo">Hour to delimitate period end.</param>
        /// <param name="minuteTo">Minute to delimitade period end.</param>
        public void Between(int hourFrom, int minuteFrom, int hourTo, int minuteTo) =>
            Between(new TimeSpan(hourFrom, minuteFrom, 0), new TimeSpan(hourTo, minuteTo, 0));

        /// <summary>
        /// Runs the job at the given period of time.
        /// </summary>
        /// <param name="from">Time of the day to delimitate the period beginnig.</param>
        /// <param name="to">Time of the day to delimitade the period end.</param>
        public void Between(TimeSpan from, TimeSpan to)
        {
            _calculator.PeriodCalculations.Add(last =>
                {
                    var now = ((ITimeCalculator)_calculator).Now();
                    var lastTime = last.TimeOfDay;

                    var next = new DateTime(last.Year, last.Month, last.Day).Add(from);

                    if (from >= to || lastTime < from)
                        return  next;

                    if (lastTime > to)
                        return next.AddDays(1);

                    return last;
                }
            );
        }
    }
}

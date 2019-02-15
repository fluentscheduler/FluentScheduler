namespace FluentScheduler
{
    using System;

    public class UtcSet
    {
        private readonly FluentTimeCalculator _calculator;

        internal UtcSet(FluentTimeCalculator calculator) => _calculator = calculator;

        /// <summary>
        /// Sets the scheduling to use UTC time system.
        /// </summary>
        public void UseUtc() => ((ITimeCalculator)_calculator).Now = () => DateTime.UtcNow;
    }
}
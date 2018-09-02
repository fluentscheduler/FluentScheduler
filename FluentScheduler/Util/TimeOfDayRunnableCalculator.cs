namespace FluentScheduler
{
    using System;

    internal class TimeOfDayRunnableCalculator
    {
        private readonly int _startHour;

        private readonly int _startMinute;

        private readonly int _endHour;

        private readonly int _endMinute;

        internal TimeOfDayRunnableCalculator(int startHour, int startMinute, int endHour, int endMinute)
        {
            _startHour = startHour;
            _startMinute = startMinute;
            _endHour = endHour;
            _endMinute = endMinute;
        }

        internal TimeOfDayRunnable Calculate(DateTime nextRun)
        {
            if (nextRun.Hour < _startHour || (nextRun.Hour == _startHour && nextRun.Minute < _startMinute))
                return TimeOfDayRunnable.TooEarly;

            if (nextRun.Hour < _endHour || (nextRun.Hour == _endHour && nextRun.Minute < _endMinute))
                return TimeOfDayRunnable.CanRun;

            return TimeOfDayRunnable.TooLate;
        }
    }
}
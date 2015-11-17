using System;

namespace FluentScheduler.Model
{
    public class TimeOfDayRunnableCalculator
    {
        private readonly int startHour;

        private readonly int startMinute;

        private readonly int endHour;

        private readonly int endMinute;

        public TimeOfDayRunnableCalculator(int startHour, int startMinute, int endHour, int endMinute)
        {
            this.startHour = startHour;
            this.startMinute = startMinute;
            this.endHour = endHour;
            this.endMinute = endMinute;
        }

        public TimeOfDayRunnable Calculate(DateTime nextRun)
        {
            if (nextRun.Hour < this.startHour || (nextRun.Hour == this.startHour && nextRun.Minute < this.startMinute))
            {
                return Model.TimeOfDayRunnable.TooEarly;
            }
            if (nextRun.Hour < this.endHour || (nextRun.Hour == this.endHour && nextRun.Minute < this.endMinute))
            {
                return Model.TimeOfDayRunnable.CanRun;
            }
            return Model.TimeOfDayRunnable.TooLate;
        }
    }
}
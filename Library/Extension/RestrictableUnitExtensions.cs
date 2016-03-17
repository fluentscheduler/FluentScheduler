namespace FluentScheduler
{
    using System;

    public static class RestrictableUnitExtensions
    {
        public static ITimeRestrictableUnit Between(this ITimeRestrictableUnit restrictableUnit, int startHour,
            int startMinute, int endHour, int endMinute)
        {
            if (restrictableUnit == null)
                throw new ArgumentNullException("restrictableUnit");

            var timeOfDayRunnableCalculator = new TimeOfDayRunnableCalculator(startHour, startMinute, endHour, endMinute);

            var unboundCalculateNextRun = restrictableUnit.Schedule.CalculateNextRun;
            restrictableUnit.Schedule.CalculateNextRun = x =>
            {
                var nextRun = unboundCalculateNextRun(x);

                if (timeOfDayRunnableCalculator.Calculate(nextRun) == TimeOfDayRunnable.TooEarly)
                    nextRun = nextRun.Date.AddHours(startHour).AddMinutes(startMinute);

                while (timeOfDayRunnableCalculator.Calculate(nextRun) != TimeOfDayRunnable.CanRun)
                    nextRun = unboundCalculateNextRun(nextRun);

                return nextRun;
            };

            return restrictableUnit;
        }

        public static IDayRestrictableUnit WeekdaysOnly(this IDayRestrictableUnit restrictableUnit)
        {
            if (restrictableUnit == null)
                throw new ArgumentNullException("restrictableUnit");

            var unboundCalculateNextRun = restrictableUnit.Schedule.CalculateNextRun;
            restrictableUnit.Schedule.CalculateNextRun = x =>
            {
                var nextRun = unboundCalculateNextRun(x);

                while (!nextRun.IsWeekday())
                    nextRun = restrictableUnit.DayIncrement(nextRun);

                return nextRun;
            };

            return restrictableUnit;
        }
    }
}
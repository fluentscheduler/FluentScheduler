using System;
using FluentScheduler.Model;

namespace FluentScheduler
{
  public static class RestrictableUnitExtensions
  {
    public static ITimeRestrictableUnit Between(this ITimeRestrictableUnit restrictableUnit, int startHour, int startMinute, int endHour, int endMinute)
      {
          if (restrictableUnit == null)
              throw new ArgumentNullException("restrictableUnit");

      TimeOfDayRunnableCalculator timeOfDayRunnableCalculator = new TimeOfDayRunnableCalculator(startHour, startMinute, endHour, endMinute);

      Func<DateTime, DateTime> unboundCalculateNextRun = restrictableUnit.Schedule.CalculateNextRun;
      restrictableUnit.Schedule.CalculateNextRun = x =>
      {
        var nextRun = unboundCalculateNextRun(x);
        if (timeOfDayRunnableCalculator.Calculate(nextRun) == Model.TimeOfDayRunnable.TooEarly)
        {
          nextRun = nextRun.Date.AddHours(startHour).AddMinutes(startMinute);
        }
        while (timeOfDayRunnableCalculator.Calculate(nextRun) != Model.TimeOfDayRunnable.CanRun)
        {
          nextRun = unboundCalculateNextRun(nextRun);
        }
        return nextRun;
      };
      return restrictableUnit;
    }

    public static IDayRestrictableUnit WeekDaysOnly(this IDayRestrictableUnit restrictableUnit)
    {
        if (restrictableUnit == null)
            throw new ArgumentNullException("restrictableUnit");

      Func<DateTime, DateTime> unboundCalculateNextRun = restrictableUnit.Schedule.CalculateNextRun;
      restrictableUnit.Schedule.CalculateNextRun = x =>
      {
        var nextRun = unboundCalculateNextRun(x);
        while (!nextRun.IsWeekDay())
        {
          nextRun = restrictableUnit.DayIncrement(nextRun);
        }
        return nextRun;
      };
      return restrictableUnit;
    }
  }
}
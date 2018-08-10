using System;
using Moong.FluentScheduler.Enum;

namespace Moong.FluentScheduler.Unit
{
  /// <summary>
  /// Unit of time in months.
  /// </summary>
  public sealed class MonthUnit
  {
    private readonly int _duration;

    internal MonthUnit(Schedule schedule, int duration)
    {
      _duration = duration;
      this.Schedule = schedule;
      this.Schedule.CalculateNextRun = x => x.Date.AddMonths(_duration);
    }

    internal Schedule Schedule { get; }

    /// <summary>
    /// Runs the job on the given day of the month.
    /// </summary>
    /// <param name="day">The day (1 through the number of days in month).</param>
    public MonthOnDayOfMonthUnit On(int day)
    {
      return new MonthOnDayOfMonthUnit(this.Schedule, _duration, day);
    }

    /// <summary>
    /// Runs the job on the last day of the month.
    /// </summary>
    public MonthOnLastDayOfMonthUnit OnTheLastDay()
    {
      return new MonthOnLastDayOfMonthUnit(this.Schedule, _duration);
    }

    /// <summary>
    /// Runs the job on the given day of week on the first week of the month.
    /// </summary>
    /// <param name="day">The day of the week.</param>
    public MonthOnDayOfWeekUnit OnTheFirst(DayOfWeek day)
    {
      return new MonthOnDayOfWeekUnit(this.Schedule, _duration, Week.First, day);
    }

    /// <summary>
    /// Runs the job on the given day of week on the second week of the month.
    /// </summary>
    /// <param name="day">The day of the week.</param>
    public MonthOnDayOfWeekUnit OnTheSecond(DayOfWeek day)
    {
      return new MonthOnDayOfWeekUnit(this.Schedule, _duration, Week.Second, day);
    }

    /// <summary>
    /// Runs the job on the given day of week on the third week of the month.
    /// </summary>
    /// <param name="day">The day of the week.</param>
    public MonthOnDayOfWeekUnit OnTheThird(DayOfWeek day)
    {
      return new MonthOnDayOfWeekUnit(this.Schedule, _duration, Week.Third, day);
    }

    /// <summary>
    /// Runs the job on the given day of week on the fourth week of the month.
    /// </summary>
    /// <param name="day">The day of the week.</param>
    public MonthOnDayOfWeekUnit OnTheFourth(DayOfWeek day)
    {
      return new MonthOnDayOfWeekUnit(this.Schedule, _duration, Week.Fourth, day);
    }

    /// <summary>
    /// Runs the job on the given day of week on the last week of the month.
    /// </summary>
    /// <param name="day">The day of the week.</param>
    public MonthOnDayOfWeekUnit OnTheLast(DayOfWeek day)
    {
      return new MonthOnDayOfWeekUnit(this.Schedule, _duration, Week.Last, day);
    }
  }
}

namespace FluentScheduler.Model
{
  using System;

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

    public TimeOfDayRunnable Calculate(DateTime x)
    {
      if (x.Hour < this.startHour || (x.Hour == this.startHour && x.Minute < this.startMinute))
      {
        return Model.TimeOfDayRunnable.TooEarly;
      }
      if (x.Hour < this.endHour || (x.Hour == this.endHour && x.Minute < this.endMinute))
      {
        return Model.TimeOfDayRunnable.CanRun;
      }
      return Model.TimeOfDayRunnable.TooLate;
    }
  }
}
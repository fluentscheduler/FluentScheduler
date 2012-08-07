using System;

namespace FluentScheduler.Model
{
	public class TimeUnit
	{
		internal Schedule Schedule { get; private set; }
		internal int Duration { get; private set; }

		public TimeUnit(Schedule schedule, int duration)
		{
			Schedule = schedule;
			Duration = duration;
		}

		public void Seconds()
		{
			Schedule.CalculateNextRun = x => x.AddSeconds(Duration);
		}
		public void Minutes()
		{
			Schedule.CalculateNextRun = x => x.AddMinutes(Duration);
		}
		public HourUnit Hours()
		{
			return new HourUnit(Schedule, Duration);
		}
		public DayUnit Days()
		{
			return new DayUnit(Schedule, Duration);
		}
		public WeekUnit Weeks()
		{
			return new WeekUnit(Schedule, Duration);
		}
		public MonthUnit Months()
		{
			return new MonthUnit(Schedule, Duration);
		}
		public YearUnit Years()
		{
			return new YearUnit(Schedule, Duration);
		}
	}
}

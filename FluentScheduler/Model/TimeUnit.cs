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

		public SecondUnit Seconds()
		{
			return new SecondUnit(Schedule, Duration);
		}
		public MinuteUnit Minutes()
		{
			return new MinuteUnit(Schedule, Duration);
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

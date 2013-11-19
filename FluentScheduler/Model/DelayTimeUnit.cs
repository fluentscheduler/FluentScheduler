using System;

namespace FluentScheduler.Model
{

	/// <summary>
	/// Delayed execution support - each method extends the startup time of the task for the specific interval.
	/// </summary>
	public class DelayTimeUnit
	{
		internal Schedule Schedule { get; private set; }
		internal int Interval { get; private set; }

		public DelayTimeUnit(Schedule schedule, int interval)
		{
			Schedule = schedule;
			Interval = interval;
		}
		public void Seconds()
		{
			Schedule.DelayRunFor = new TimeSpan(0, 0, 0, Interval, 0);
		}

		public void Minutes()
		{
			Schedule.DelayRunFor = new TimeSpan(0, 0, Interval, 0, 0);
		}
		public void Hours()
		{
			Schedule.DelayRunFor = new TimeSpan(0, Interval, 0, 0, 0);
		}
		public void Days()
		{
			Schedule.DelayRunFor = new TimeSpan(Interval, 0, 0, 0, 0);
		}
		public void Weeks()
		{
			Schedule.DelayRunFor = new TimeSpan(Interval * 7, 0, 0, 0, 0);
		}
		public void Months()
		{
			var now = DateTime.Now;
			Schedule.DelayRunFor = now.AddMonths(Interval).Subtract(now);
		}
		public void Years()
		{
			var now = DateTime.Now;
			Schedule.DelayRunFor = now.AddYears(Interval).Subtract(now);
		}
	}
}

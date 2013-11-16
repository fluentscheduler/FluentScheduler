using System;

namespace FluentScheduler.Model
{
	public class SecondUnit
	{
		internal Schedule Schedule { get; private set; }
		internal int Duration { get; private set; }

		public SecondUnit(Schedule schedule, int duration)
		{
			Schedule = schedule;
			Duration = duration;

			Schedule.CalculateNextRun = x => x.AddSeconds(Duration);
		}
	}
}

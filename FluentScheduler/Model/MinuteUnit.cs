using System;

namespace FluentScheduler.Model
{
	public class MinuteUnit
	{
		internal Schedule Schedule { get; private set; }
		internal int Duration { get; private set; }

		public MinuteUnit(Schedule schedule, int duration)
		{
			Schedule = schedule;
			Duration = duration;

			Schedule.CalculateNextRun = x => x.AddMinutes(Duration);
		}
	}
}

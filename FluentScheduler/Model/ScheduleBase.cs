using System;

namespace FluentScheduler.Model
{
	public abstract class ScheduleBase
	{
		public DateTime NextRunTime { get; set; }
		public string Name { get; set; }
	}
}

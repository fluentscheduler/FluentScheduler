using System;

namespace FluentScheduler.Model
{

	/// <summary>
	/// TODO: comments
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
            Schedule.DelayRunFor = new TimeSpan(0, 0, Interval);
        }

        public void Minutes()
        {
            Schedule.DelayRunFor = new TimeSpan(0, Interval, 0);
        }

    }
}

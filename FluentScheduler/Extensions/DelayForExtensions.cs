using System;
using FluentScheduler.Model;

namespace FluentScheduler
{
	/// <summary>
	/// TODO: comments
	/// </summary>
    public static class DelayForExtensions
	{
		public static DelayTimeUnit DelayFor(this SpecificRunTime runTime, int interval)
		{
			return new DelayTimeUnit(runTime.Schedule, interval);
		}
		public static DelayTimeUnit DelayFor(this SecondUnit timeUnit, int interval)
		{
			return new DelayTimeUnit(timeUnit.Schedule, interval);
		}
		public static DelayTimeUnit DelayFor(this MinuteUnit timeUnit, int interval)
		{
			return new DelayTimeUnit(timeUnit.Schedule, interval);
		}
    }
}

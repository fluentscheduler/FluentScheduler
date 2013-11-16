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
    }
}

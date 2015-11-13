using System;

namespace FluentScheduler.Tests.UnitTests.Utilities
{
    public static class DateTimeExtensions
    {
        public static DateTime WithoutMilliseconds(this DateTime dateTime)
        {
            return dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerSecond));
        }
    }
}

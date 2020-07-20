namespace FluentScheduler.UnitTests.Utilities
{
    using System;

    public static class DateTimeExtensions
    {
        public static DateTime WithoutMilliseconds(this DateTime dateTime)
        {
            return dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerSecond));
        }
    }
}

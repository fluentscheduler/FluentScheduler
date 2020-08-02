namespace FluentScheduler.UnitTests.Utilities
{
    using System;

    public static class DateTimeExtensions
    {
        public static DateTime WithoutMilliseconds(this DateTime dateTime) =>
            dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerSecond));
    }
}

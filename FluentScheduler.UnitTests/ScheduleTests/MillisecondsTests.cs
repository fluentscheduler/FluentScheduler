namespace FluentScheduler.UnitTests.ScheduleTests
{
    using Xunit;
    using System;

    public class MillisecondsTests
    {
        [Fact]
        public void Should_Add_Specified_Milliseconds_To_Next_Run_Date()
        {
            // Assert
            var input = new DateTime(2000, 1, 1);
            var expected = new DateTime(2000, 1, 1, 0, 0, 0, 500);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(500).Milliseconds();
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}

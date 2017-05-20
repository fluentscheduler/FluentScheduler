namespace FluentScheduler.Tests.UnitTests.ScheduleTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;

    [TestClass]
    public class MillisecondsTests
    {
        [TestMethod]
        public void Should_Add_Specified_Milliseconds_To_Next_Run_Date()
        {
            // Assert
            var input = new DateTime(2000, 1, 1, 0, 0, 0, millisecond: 0);
            var expected = new DateTime(2000, 1, 1, 0, 0, 0, millisecond: 250);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(250).Milliseconds();
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}

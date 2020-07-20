namespace FluentScheduler.UnitTests.ScheduleTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class MinutesTests
    {
        [TestMethod]
        public void Should_Add_Specified_Minutes_To_Next_Run_Date()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1);
            var expected = new DateTime(2000, 1, 1, 0, 30, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(30).Minutes();
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Add_Specified_Minutes_To_Next_Run_Date_When_Is_Between_Specified_Bounds()
        {
            // Assert
            var input = new DateTime(2000, 1, 6, 12, 23, 25);
            var expected = new DateTime(2000, 1, 6, 12, 53, 25);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(30).Minutes().Between(10, 00, 16, 00);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Delay_Next_Run_Date_Until_Specified_Start()
        {
            // Assert
            var input = new DateTime(2000, 1, 6, 1, 23, 25);
            var expected = new DateTime(2000, 1, 6, 10, 0, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Minutes().Between(10, 00, 11, 00);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}

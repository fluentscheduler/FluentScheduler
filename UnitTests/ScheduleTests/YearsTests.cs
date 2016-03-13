using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace FluentScheduler.Tests.UnitTests.ScheduleTests
{
    [TestClass]
    public class YearsTests
    {
        [TestMethod]
        public void Should_Add_Specified_Years_To_Next_Run_Date()
        {
            // Arrange
            var job = new Mock<IJob>();
            var input = new DateTime(2000, 1, 1);
            var expected = new DateTime(2002, 1, 1);

            // Act
            var schedule = new Schedule(job.Object);
            schedule.ToRunEvery(2).Years();
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Default_To_00_00_If_At_Is_Not_Defined()
        {
            // Arrange
            var job = new Mock<IJob>();
            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var expected = new DateTime(2002, 1, 1);

            // Act
            var schedule = new Schedule(job.Object);
            schedule.ToRunEvery(2).Years();
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}

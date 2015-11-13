using FluentScheduler.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;

namespace FluentScheduler.Tests.UnitTests.ScheduleTests
{
    [TestClass]
    public class SpecificRunTimeTests
    {
        [TestMethod]
        public void Should_Add_Chained_Tasks_To_AdditionalSchedules_Property()
        {
            // Arrange
            var task = new Mock<ITask>();

            // Act
            var schedule = new Schedule(task.Object);
            schedule.ToRunNow().AndEvery(1).Months();

            // Assert
            Assert.AreEqual(1, schedule.AdditionalSchedules.Count);
        }

        [TestMethod]
        public void Should_Set_Chained_Task_Schedule_As_Expected()
        {
            // Arrange
            var task = new Mock<ITask>();
            var input = new DateTime(2000, 1, 1);
            var expected = new DateTime(2000, 3, 1);

            // Act
            var schedule = new Schedule(task.Object);
            schedule.ToRunNow().AndEvery(2).Months();
            var actual = schedule.AdditionalSchedules.ElementAt(0).CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Not_Alter_Original_Runtime_If_Chained_Task_Exists()
        {
            // Arrange
            var task = new Mock<ITask>();

            // Act
            var schedule = new Schedule(task.Object);
            schedule.ToRunNow().AndEvery(1).Months();

            // Assert
            Assert.IsNull(schedule.CalculateNextRun);
        }
    }
}

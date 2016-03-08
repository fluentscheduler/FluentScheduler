using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FluentScheduler.Tests.UnitTests.ScheduleTests
{
    [TestClass]
    public class NonReentrantTests
    {
        [TestMethod]
        public void Should_Be_True_By_Default()
        {
            // Arrange
            var task = new Mock<ITask>();

            // Act
            var schedule = new Schedule(task.Object);
            schedule.ToRunNow();

            // Assert
            Assert.IsTrue(schedule.Reentrant);
        }

        [TestMethod]
        public void Should_Default_Reentrent_Parameter_For_Child_Schedules()
        {
            // Arrange
            var task = new Mock<ITask>();

            // Act
            var schedule = new Schedule(task.Object);
            schedule.ToRunNow().AndEvery(1).Minutes();

            // Assert
            Assert.IsTrue(schedule.Reentrant);
            foreach (var child in schedule.AdditionalSchedules)
                Assert.IsTrue(child.Reentrant);
        }

        [TestMethod]
        public void Should_Set_Reentrent_Parameter_For_Child_Schedules()
        {
            // Arrange
            var task = new Mock<ITask>();

            // Act
            var schedule = new Schedule(task.Object);
            schedule.NonReentrant().ToRunNow().AndEvery(1).Minutes();

            // Assert
            Assert.IsFalse(schedule.Reentrant);
            foreach (var child in schedule.AdditionalSchedules)
                Assert.IsFalse(child.Reentrant);
        }
    }
}

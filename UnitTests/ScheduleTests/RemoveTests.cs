using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading;

namespace FluentScheduler.Tests.UnitTests.ScheduleTests
{
    [TestClass]
    public class RemoveTests
    {
        [TestMethod]
        public void Should_Remove_Named_Task()
        {
            // Arrange
            var task = new Mock<ITask>();

            // Act
            var schedule = new Schedule(task.Object).WithName("remove named task");
            schedule.ToRunNow().AndEvery(1).Seconds();
            TaskManager.RemoveTask("remove named task");

            // Assert
            Assert.IsNull(TaskManager.GetSchedule("remove named task"));
        }

        [TestMethod]
        public void Should_Remove_LongRunning_Task_But_Keep_Running()
        {
            // Act
            var schedule = new Schedule(() => Thread.Sleep(100));
            schedule.WithName("remove long running task").ToRunNow().AndEvery(2).Seconds();
            schedule.Execute();

            // Assert
            Assert.IsTrue(TaskManager.RunningSchedules.Any(s => s.Name == "remove long running task"));

            // Act
            TaskManager.RemoveTask("remove long running task");

            // Assert
            Assert.IsNull(TaskManager.GetSchedule("remove long running task"));
            Assert.IsTrue(TaskManager.RunningSchedules.Any(s => s.Name == "remove long running task"));
            Thread.Sleep(2000);
            Assert.IsFalse(TaskManager.RunningSchedules.Any(s => s.Name == "remove long running task"));
        }
    }
}

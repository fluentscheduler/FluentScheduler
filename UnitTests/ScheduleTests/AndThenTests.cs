using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading;

namespace FluentScheduler.Tests.UnitTests.ScheduleTests
{
    [TestClass]
    public class AndThenTests
    {
        [TestMethod]
        public void Should_Be_Able_To_Schedule_Multiple_ITasks()
        {
            // Arrange
            var task1 = new Mock<ITask>();
            var task2 = new Mock<ITask>();
            task1.Setup(m => m.Execute());
            task2.Setup(m => m.Execute());

            // Act
            var schedule = new Schedule(task1.Object).AndThen(task2.Object);
            schedule.Execute();
            while (TaskManager.RunningSchedules.Any())
                Thread.Sleep(1);

            // Assert
            task1.Verify(m => m.Execute(), Times.Once());
            task2.Verify(m => m.Execute(), Times.Once());
        }

        [TestMethod]
        public void Should_Be_Able_To_Schedule_Multiple_Simple_Methods()
        {
            // Arrange
            var task1 = new Mock<ITask>();
            var task2 = new Mock<ITask>();
            task1.Setup(m => m.Execute());
            task2.Setup(m => m.Execute());

            // Act
            var schedule = new Schedule(() => task1.Object.Execute()).AndThen(() => task2.Object.Execute());
            schedule.Execute();
            while (TaskManager.RunningSchedules.Any())
                Thread.Sleep(1);

            // Assert
            task1.Verify(m => m.Execute(), Times.Once());
            task2.Verify(m => m.Execute(), Times.Once());
        }

        [TestMethod]
        public void Should_Execute_Tasks_In_Order()
        {
            // Arrange
            var task1 = new Mock<ITask>();
            var task2 = new Mock<ITask>();
            var task1Runtime = DateTime.MinValue;
            var task2Runtime = DateTime.MinValue;

            // Act
            task1.Setup(m => m.Execute()).Callback(() =>
            {
                task1Runtime = DateTime.Now;
                Thread.Sleep(1);
            });
            task2.Setup(m => m.Execute()).Callback(() => task2Runtime = DateTime.Now);
            var schedule = new Schedule(() => task1.Object.Execute()).AndThen(() => task2.Object.Execute());
            schedule.Execute();
            while (TaskManager.RunningSchedules.Any())
                Thread.Sleep(1);

            // Assert
            Assert.IsTrue(task1Runtime.Ticks < task2Runtime.Ticks);
        }
    }
}
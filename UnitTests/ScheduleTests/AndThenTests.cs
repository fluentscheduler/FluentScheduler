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
        public void Should_Be_Able_To_Schedule_Multiple_IJobs()
        {
            // Arrange
            var job1 = new Mock<IJob>();
            var job2 = new Mock<IJob>();
            job1.Setup(m => m.Execute());
            job2.Setup(m => m.Execute());

            // Act
            var schedule = new Schedule(job1.Object).AndThen(job2.Object);
            schedule.Execute();
            while (JobManager.RunningSchedules.Any())
                Thread.Sleep(1);

            // Assert
            job1.Verify(m => m.Execute(), Times.Once());
            job2.Verify(m => m.Execute(), Times.Once());
        }

        [TestMethod]
        public void Should_Be_Able_To_Schedule_Multiple_Simple_Methods()
        {
            // Arrange
            var job1 = new Mock<IJob>();
            var job2 = new Mock<IJob>();
            job1.Setup(m => m.Execute());
            job2.Setup(m => m.Execute());

            // Act
            var schedule = new Schedule(() => job1.Object.Execute()).AndThen(() => job2.Object.Execute());
            schedule.Execute();
            while (JobManager.RunningSchedules.Any())
                Thread.Sleep(1);

            // Assert
            job1.Verify(m => m.Execute(), Times.Once());
            job2.Verify(m => m.Execute(), Times.Once());
        }

        [TestMethod]
        public void Should_Execute_Jobs_In_Order()
        {
            // Arrange
            var job1 = new Mock<IJob>();
            var job2 = new Mock<IJob>();
            var job1Runtime = DateTime.MinValue;
            var job2Runtime = DateTime.MinValue;

            // Act
            job1.Setup(m => m.Execute()).Callback(() =>
            {
                job1Runtime = DateTime.Now;
                Thread.Sleep(1);
            });
            job2.Setup(m => m.Execute()).Callback(() => job2Runtime = DateTime.Now);
            var schedule = new Schedule(() => job1.Object.Execute()).AndThen(() => job2.Object.Execute());
            schedule.Execute();
            while (JobManager.RunningSchedules.Any())
                Thread.Sleep(1);

            // Assert
            Assert.IsTrue(job1Runtime.Ticks < job2Runtime.Ticks);
        }
    }
}
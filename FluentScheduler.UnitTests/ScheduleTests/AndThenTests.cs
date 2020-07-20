namespace FluentScheduler.UnitTests.ScheduleTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
    using System.Threading;

    [TestClass]
    public class AndThenTests
    {
        [TestMethod]
        public void Should_Be_Able_To_Schedule_Multiple_Jobs()
        {
            // Arrange
            var job1 = false;
            var job2 = false;

            // Act
            var schedule = new Schedule(() => job1 = true).AndThen(() => job2 = true);
            schedule.Execute();
            while (JobManager.RunningSchedules.Any())
                Thread.Sleep(1);

            // Assert
            Assert.IsTrue(job1);
            Assert.IsTrue(job2);
        }

        [TestMethod]
        public void Should_Be_Able_To_Schedule_Multiple_Simple_Methods()
        {
            // Arrange
            var job1 = false;
            var job2 = false;

            // Act
            var schedule = new Schedule(() => job1 = true).AndThen(() => job2 = true);
            schedule.Execute();
            while (JobManager.RunningSchedules.Any())
                Thread.Sleep(1);

            // Assert
            Assert.IsTrue(job1);
            Assert.IsTrue(job2);
        }

        [TestMethod]
        public void Should_Execute_Jobs_In_Order()
        {
            // Arrange
            var job1 = DateTime.MinValue;
            var job2 = DateTime.MinValue;

            // Act
            var schedule = new Schedule(() =>
            {
                job1 = DateTime.Now;
                Thread.Sleep(1);
            }).AndThen(() => job2 = DateTime.Now);
            schedule.Execute();
            while (JobManager.RunningSchedules.Any())
                Thread.Sleep(1);

            // Assert
            Assert.IsTrue(job1.Ticks < job2.Ticks);
        }
    }
}
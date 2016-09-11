namespace FluentScheduler.Tests.ScheduleTests
{
    using System;
    using System.Linq;
    using System.Threading;
    using Xunit;

    public class AndThenTests
    {
        [Fact]
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
            Assert.True(job1);
            Assert.True(job2);
        }

        [Fact]
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
            Assert.True(job1);
            Assert.True(job2);
        }

        [Fact]
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
            Assert.True(job1.Ticks < job2.Ticks);
        }
    }
}
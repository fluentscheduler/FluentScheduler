namespace FluentScheduler.Tests.ScheduleTests
{
    using System.Linq;
    using System.Threading;
    using Xunit;

    public class RemoveTests
    {
        [Fact]
        public void Should_Remove_Named_Job()
        {
            // Act
            var schedule = new Schedule(() => { }).WithName("remove named job");
            schedule.ToRunNow().AndEvery(1).Seconds();
            JobManager.RemoveJob("remove named job");

            // Assert
            Assert.Null(JobManager.GetSchedule("remove named job"));
        }

        [Fact]
        public void Should_Remove_LongRunning_Job_But_Keep_Running()
        {
            // Act
            var schedule = new Schedule(() => Thread.Sleep(100));
            schedule.WithName("remove long running job").ToRunNow().AndEvery(2).Seconds();
            schedule.Execute();

            // Assert
            Assert.True(JobManager.RunningSchedules.Any(s => s.Name == "remove long running job"));

            // Act
            JobManager.RemoveJob("remove long running job");

            // Assert
            Assert.Null(JobManager.GetSchedule("remove long running job"));
            Assert.True(JobManager.RunningSchedules.Any(s => s.Name == "remove long running job"));
            Thread.Sleep(2000);
            Assert.False(JobManager.RunningSchedules.Any(s => s.Name == "remove long running job"));
        }
    }
}

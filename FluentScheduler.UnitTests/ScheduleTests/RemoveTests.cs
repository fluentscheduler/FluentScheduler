namespace FluentScheduler.UnitTests.ScheduleTests
{
    using Xunit;
    using System.Linq;
    using System.Threading;

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
        public void Should_Remove_All_Jobs()
        {
            // Act
            JobManager.AddJob(() => { }, s => s.ToRunNow());
            JobManager.AddJob(() => { }, s => s.ToRunNow());
            JobManager.AddJob(() => { }, s => s.ToRunNow());
            JobManager.AddJob(() => { }, s => s.ToRunNow());
            JobManager.AddJob(() => { }, s => s.ToRunNow());
            JobManager.AddJob(() => { }, s => s.ToRunNow());

            JobManager.RemoveAllJobs();

            // Assert
            Assert.True(JobManager.AllSchedules.Count() == 0);
        }

        [Fact]
        public void Should_Remove_LongRunning_Job_But_Keep_Running()
        {
            // Act
            var schedule = new Schedule(() => Thread.Sleep(100));
            schedule.WithName("remove long running job").ToRunNow().AndEvery(2).Seconds();
            schedule.Execute();

            // Assert
            Assert.Contains(JobManager.RunningSchedules, s => s.Name == "remove long running job");

            // Act
            JobManager.RemoveJob("remove long running job");

            // Assert
            Assert.Null(JobManager.GetSchedule("remove long running job"));
            Assert.Contains(JobManager.RunningSchedules, s => s.Name == "remove long running job");
            Thread.Sleep(2000);
            Assert.DoesNotContain(JobManager.RunningSchedules, s => s.Name == "remove long running job");
        }
    }
}

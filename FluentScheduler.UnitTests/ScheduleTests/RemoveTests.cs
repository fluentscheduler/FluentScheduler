namespace FluentScheduler.UnitTests.ScheduleTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using System.Threading;

    [TestClass]
    public class RemoveTests
    {
        [TestMethod]
        public void Should_Remove_Named_Job()
        {
            // Act
            var schedule = new Schedule(() => { }).WithName("remove named job");
            schedule.ToRunNow().AndEvery(1).Seconds();
            JobManager.RemoveJob("remove named job");

            // Assert
            Assert.IsNull(JobManager.GetSchedule("remove named job"));
        }

        [TestMethod]
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
            Assert.IsTrue(JobManager.AllSchedules.Count() == 0);
        }

        [TestMethod]
        public void Should_Remove_LongRunning_Job_But_Keep_Running()
        {
            // Act
            var schedule = new Schedule(() => Thread.Sleep(100));
            schedule.WithName("remove long running job").ToRunNow().AndEvery(2).Seconds();
            schedule.Execute();

            // Assert
            Assert.IsTrue(JobManager.RunningSchedules.Any(s => s.Name == "remove long running job"));

            // Act
            JobManager.RemoveJob("remove long running job");

            // Assert
            Assert.IsNull(JobManager.GetSchedule("remove long running job"));
            Assert.IsTrue(JobManager.RunningSchedules.Any(s => s.Name == "remove long running job"));
            Thread.Sleep(2000);
            Assert.IsFalse(JobManager.RunningSchedules.Any(s => s.Name == "remove long running job"));
        }
    }
}

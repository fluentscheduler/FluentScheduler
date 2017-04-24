namespace FluentScheduler.Tests.UnitTests.ScheduleTests
{
    using System;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NonReentrantTests
    {
        [TestMethod]
        public void Should_Be_Null_By_Default()
        {
            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunNow();

            // Assert
            Assert.IsNull(schedule.Reentrant);
        }

        [TestMethod]
        public void Should_Default_Reentrant_Parameter_For_Child_Schedules()
        {
            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunNow().AndEvery(1).Minutes();

            // Assert
            Assert.IsNull(schedule.Reentrant);

            foreach (var child in schedule.AdditionalSchedules)
                Assert.IsNull(child.Reentrant);
        }

        [TestMethod]
        public void Should_Set_Reentrent_Parameter_For_Child_Schedules()
        {
            // Act
            var schedule = new Schedule(() => { });
            schedule.NonReentrant().ToRunNow().AndEvery(1).Minutes();

            // Assert
            Assert.IsNotNull(schedule.Reentrant);
            foreach (var child in schedule.AdditionalSchedules)
                Assert.AreEqual(schedule.Reentrant, child.Reentrant);
        }

        [TestMethod]
        public void Should_Only_One_Job_Be_Executing()
        {
            JobManager.AddJob(new InfinityJob(), schedule => schedule.NonReentrant().ToRunNow().AndEvery(1).Seconds());

            JobManager.Start();
            Thread.Sleep(3000);
            JobManager.Stop();

            Console.WriteLine("InfinityJob.Count: " + InfinityJob.Count);
            try
            {
                // the job must be run once at the same time
                // for reentrant mode
                Assert.IsTrue(InfinityJob.Count == 1);
            }
            finally
            {
                InfinityJob.StopJob = true;
            }
        }

        private class InfinityJob : IJob
        {
            public static int Count;

            public static volatile bool StopJob;

            public void Execute()
            {
                Count++;
                while (!StopJob)
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}

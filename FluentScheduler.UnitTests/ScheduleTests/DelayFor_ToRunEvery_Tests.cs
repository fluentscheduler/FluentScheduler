namespace FluentScheduler.UnitTests.ScheduleTests
{
    using FluentScheduler.UnitTests.Utilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class DelayFor_ToRunEvery_Tests
    {
        [TestMethod]
        public void Should_Delay_ToRunEvery_For_500_Milliseconds()
        {
            // Arrange
            var expected = DateTime.Now.AddMilliseconds(600);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run every 500 milliseconds and delay for 100 milliseconds")
                    .ToRunEvery(500).Milliseconds()
                    .DelayFor(100).Milliseconds()
            );
            var actual = JobManager.GetSchedule("run every 500 milliseconds and delay for 100 milliseconds").NextRun;

            // Assert
            Assert.IsTrue((expected - actual).TotalMilliseconds < 150);
        }

        [TestMethod]
        public void Should_Delay_ToRunEvery_For_2_Seconds()
        {
            // Arrange
            var expected = DateTime.Now.AddSeconds(12);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run every 10 seconds and delay for 2 seconds")
                    .ToRunEvery(10).Seconds()
                    .DelayFor(2).Seconds()
            );
            var actual = JobManager.GetSchedule("run every 10 seconds and delay for 2 seconds").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

        [TestMethod]
        public void Should_Delay_ToRunEvery_For_2_Minutes()
        {
            // Arrange
            var expected = DateTime.Now.AddSeconds(10).AddMinutes(2);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run every 10 seconds and delay for 2 minutes")
                    .ToRunEvery(10).Seconds()
                    .DelayFor(2).Minutes()
            );
            var actual = JobManager.GetSchedule("run every 10 seconds and delay for 2 minutes").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

        [TestMethod]
        public void Should_Delay_ToRunEvery_For_2_Hours()
        {
            // Arrange
            var expected = DateTime.Now.AddSeconds(10).AddHours(2);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run every 10 seconds and delay for 2 hours")
                    .ToRunEvery(10).Seconds()
                    .DelayFor(2).Hours()
            );
            var actual = JobManager.GetSchedule("run every 10 seconds and delay for 2 hours").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

        [TestMethod]
        public void Should_Delay_ToRunEvery_For_2_Days()
        {
            // Arrange
            var expected = DateTime.Now.AddSeconds(10).AddDays(2);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run every 10 seconds and delay for 2 days")
                    .ToRunEvery(10).Seconds()
                    .DelayFor(2).Days()
            );
            var actual = JobManager.GetSchedule("run every 10 seconds and delay for 2 days").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

        [TestMethod]
        public void Should_Delay_ToRunEvery_For_2_Weeks()
        {
            // Arrange
            var expected = DateTime.Now.AddSeconds(10).AddDays(14);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run every 10 seconds and delay for 2 weeks")
                    .ToRunEvery(10).Seconds()
                    .DelayFor(2).Weeks()
            );
            var actual = JobManager.GetSchedule("run every 10 seconds and delay for 2 weeks").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

        [TestMethod]
        public void Should_Delay_ToRunEvery_For_2_Months()
        {
            // Arrange
            var expected = DateTime.Now.AddSeconds(10).AddMonths(2);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run every 10 seconds and delay for 2 months")
                    .ToRunEvery(10).Seconds()
                    .DelayFor(2).Months()
            );
            var actual = JobManager.GetSchedule("run every 10 seconds and delay for 2 months").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

        [TestMethod]
        public void Should_Delay_ToRunEvery_For_2_Years()
        {
            // Arrange
            var expected = DateTime.Now.AddSeconds(10).AddYears(2);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run every 10 seconds and delay for 2 years")
                    .ToRunEvery(10).Seconds()
                    .DelayFor(2).Years()
            );
            var actual = JobManager.GetSchedule("run every 10 seconds and delay for 2 years").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

    }
}

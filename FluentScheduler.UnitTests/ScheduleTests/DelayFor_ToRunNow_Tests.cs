namespace FluentScheduler.UnitTests.ScheduleTests
{
    using FluentScheduler.UnitTests.Utilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class DelayFor_ToRunOnceAt_Tests
    {
        [TestMethod]
        public void Should_Delay_ToRunOnceAt_For_500_Milliseconds()
        {
            // Arrange
            var now = DateTime.Now;
            var expected = now.AddMilliseconds(500);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run once at x and delay for 500 milliseconds")
                    .ToRunOnceAt(now)
                    .DelayFor(500).Milliseconds()
            );
            var actual = JobManager.GetSchedule("run once at x and delay for 500 milliseconds").NextRun;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Delay_ToRunOnceAt_For_2_Seconds()
        {
            // Arrange
            var now = DateTime.Now;
            var expected = now.AddSeconds(2);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run once at x and delay for 2 seconds")
                    .ToRunOnceAt(now)
                    .DelayFor(2).Seconds()
            );
            var actual = JobManager.GetSchedule("run once at x and delay for 2 seconds").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

        [TestMethod]
        public void Should_Delay_ToRunOnceAt_For_2_Minutes()
        {
            // Arrange
            var now = DateTime.Now;
            var expected = now.AddMinutes(2);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run once at x and delay for 2 minutes")
                    .ToRunOnceAt(now)
                    .DelayFor(2).Minutes()
            );
            var actual = JobManager.GetSchedule("run once at x and delay for 2 minutes").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

        [TestMethod]
        public void Should_Delay_ToRunOnceAt_For_2_Hours()
        {
            // Act
            var now = DateTime.Now;
            var expected = now.AddHours(2);

            // Arrange
            JobManager.AddJob(
                () => { },
                s => s.WithName("run once at x and delay for 2 hours")
                    .ToRunOnceAt(now)
                    .DelayFor(2).Hours()
            );
            var actual = JobManager.GetSchedule("run once at x and delay for 2 hours").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

        [TestMethod]
        public void Should_Delay_ToRunOnceAt_For_2_Days()
        {
            // Assert
            var expected = DateTime.Now.AddDays(2);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run once at x and delay for 2 days")
                    .ToRunOnceAt(DateTime.Now)
                    .DelayFor(2).Days()
            );
            var actual = JobManager.GetSchedule("run once at x and delay for 2 days").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

        [TestMethod]
        public void Should_Delay_ToRunOnceAt_For_2_Weeks()
        {
            // Assert
            var now = DateTime.Now;
            var expected = now.AddDays(14);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run once at x and delay for 2 weeks")
                    .ToRunOnceAt(now)
                    .DelayFor(2).Weeks());
            var actual = JobManager.GetSchedule("run once at x and delay for 2 weeks").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

        [TestMethod]
        public void Should_Delay_ToRunOnceAt_For_2_Months()
        {
            // Arrange
            var now = DateTime.Now;
            var expected = DateTime.Now.AddMonths(2);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run once at x and delay for 2 months")
                    .ToRunOnceAt(now)
                    .DelayFor(2).Months()
            );
            var actual = JobManager.GetSchedule("run once at x and delay for 2 months").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

        [TestMethod]
        public void Should_Delay_ToRunOnceAt_For_2_Years()
        {
            // Arrange
            var now = DateTime.Now;
            var expected = DateTime.Now.AddYears(2);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run once at x and delay for 2 years")
                    .ToRunOnceAt(now)
                    .DelayFor(2).Years()
            );
            var actual = JobManager.GetSchedule("run once at x and delay for 2 years").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

    }
}

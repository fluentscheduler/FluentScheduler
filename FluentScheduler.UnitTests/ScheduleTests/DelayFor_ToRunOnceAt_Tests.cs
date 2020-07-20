namespace FluentScheduler.UnitTests.ScheduleTests
{
    using FluentScheduler.UnitTests.Utilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class DelayFor_ToRunNow_Tests
    {
        [TestMethod]
        public void Should_Delay_ToRunNow_For_2_Seconds()
        {
            // Arrange
            var expected = DateTime.Now.AddSeconds(2);

            // Act
            JobManager.AddJob(
                () => { },
                x => x.WithName("run now and delay for 2 seconds")
                    .ToRunNow().DelayFor(2).Seconds()
            );
            var actual = JobManager.GetSchedule("run now and delay for 2 seconds").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

        [TestMethod]
        public void Should_Delay_ToRunNow_For_2_Minutes()
        {
            // Arrange
            var expected = DateTime.Now.AddMinutes(2);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run now and delay for 2 minutes")
                    .ToRunNow().DelayFor(2).Minutes()
            );
            var actual = JobManager.GetSchedule("run now and delay for 2 minutes").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

        [TestMethod]
        public void Should_Delay_ToRunNow_For_2_Hours()
        {
            // Arrange
            var expected = DateTime.Now.AddHours(2);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run now and delay for 2 hours")
                    .ToRunNow().DelayFor(2).Hours()
            );
            var actual = JobManager.GetSchedule("run now and delay for 2 hours").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

        [TestMethod]
        public void Should_Delay_ToRunNow_For_2_Days()
        {
            // Arrange
            var expected = DateTime.Now.AddDays(2);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run now and delay for 2 days")
                    .ToRunNow().DelayFor(2).Days()
            );
            var actual = JobManager.GetSchedule("run now and delay for 2 days").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

        [TestMethod]
        public void Should_Delay_ToRunNow_For_2_Weeks()
        {
            // Assert
            var expected = DateTime.Now.AddDays(14);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run now and delay for 2 weeks")
                .ToRunNow().DelayFor(2).Weeks()
            );

            var actual = JobManager.GetSchedule("run now and delay for 2 weeks").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

        [TestMethod]
        public void Should_Delay_ToRunNow_For_2_Months()
        {
            // Assert
            var expected = DateTime.Now.AddMonths(2);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run now and delay for 2 months")
                    .ToRunNow().DelayFor(2).Months()
            );
            var actual = JobManager.GetSchedule("run now and delay for 2 months").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

        [TestMethod]
        public void Should_Delay_ToRunNow_For_2_Years()
        {
            // Assert
            var expected = DateTime.Now.AddYears(2);

            // Act
            JobManager.AddJob(
                () => { },
                s => s.WithName("run now and delay for 2 years")
                    .ToRunNow().DelayFor(2).Years()
            );
            var actual = JobManager.GetSchedule("run now and delay for 2 years").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

    }
}

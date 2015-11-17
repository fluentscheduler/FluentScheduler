using FluentScheduler.Tests.UnitTests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FluentScheduler.Tests.UnitTests.ScheduleTests
{
    [TestClass]
    public class DelayFor_ToRunOnceAt_Tests
    {
        [TestMethod]
        public void Should_Delay_ToRunOnceAt_For_2_Seconds()
        {
            // Arrange
            var now = DateTime.Now;
            var expected = now.AddSeconds(2);

            // Act
            TaskManager.AddTask(
                () => { },
                s => s.WithName("run once at x and delay for 2 seconds")
                    .ToRunOnceAt(now)
                    .DelayFor(2).Seconds()
            );
            var actual = TaskManager.GetSchedule("run once at x and delay for 2 seconds").NextRun;

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
            TaskManager.AddTask(
                () => { },
                s => s.WithName("run once at x and delay for 2 minutes")
                    .ToRunOnceAt(now)
                    .DelayFor(2).Minutes()
            );
            var actual = TaskManager.GetSchedule("run once at x and delay for 2 minutes").NextRun;

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
            TaskManager.AddTask(
                () => { },
                s => s.WithName("run once at x and delay for 2 hours")
                    .ToRunOnceAt(now)
                    .DelayFor(2).Hours()
            );
            var actual = TaskManager.GetSchedule("run once at x and delay for 2 hours").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

        [TestMethod]
        public void Should_Delay_ToRunOnceAt_For_2_Days()
        {
            // Assert
            var expected = DateTime.Now.AddDays(2);

            // Act
            TaskManager.AddTask(
                () => { },
                s => s.WithName("run once at x and delay for 2 days")
                    .ToRunOnceAt(DateTime.Now)
                    .DelayFor(2).Days()
            );
            var actual = TaskManager.GetSchedule("run once at x and delay for 2 days").NextRun;

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
            TaskManager.AddTask(
                () => { },
                s => s.WithName("run once at x and delay for 2 weeks")
                    .ToRunOnceAt(now)
                    .DelayFor(2).Weeks());
            var actual = TaskManager.GetSchedule("run once at x and delay for 2 weeks").NextRun;

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
            TaskManager.AddTask(
                () => { },
                s => s.WithName("run once at x and delay for 2 months")
                    .ToRunOnceAt(now)
                    .DelayFor(2).Months()
            );
            var actual = TaskManager.GetSchedule("run once at x and delay for 2 months").NextRun;

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
            TaskManager.AddTask(
                () => { },
                s => s.WithName("run once at x and delay for 2 years")
                    .ToRunOnceAt(now)
                    .DelayFor(2).Years()
            );
            var actual = TaskManager.GetSchedule("run once at x and delay for 2 years").NextRun;

            // Assert
            Assert.AreEqual(expected.WithoutMilliseconds(), actual.WithoutMilliseconds());
        }

    }
}

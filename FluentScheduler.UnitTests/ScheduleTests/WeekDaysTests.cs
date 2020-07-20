namespace FluentScheduler.UnitTests.ScheduleTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class WeekDaysTests
    {
        [TestMethod]
        public void Should_Pick_Same_Day_If_Now_Is_In_Time_On_Friday()
        {
            // Arrange
            var input = new DateTime(1999, 12, 31, 1, 23, 25);
            var expected = new DateTime(1999, 12, 31, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(1).Weekdays().At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(DayOfWeek.Friday, input.DayOfWeek);
            Assert.AreEqual(DayOfWeek.Friday, actual.DayOfWeek);
        }

        [TestMethod]
        public void Should_Pick_Monday_If_Now_Is_Too_Late_On_Friday()
        {
            // Arrange
            var input = new DateTime(1999, 12, 31, 12, 23, 25);
            var expected = new DateTime(2000, 1, 3, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(1).Weekdays().At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(DayOfWeek.Friday, input.DayOfWeek);
            Assert.AreEqual(DayOfWeek.Monday, actual.DayOfWeek);
        }

        [TestMethod]
        public void Should_Pick_Monday_If_Now_Is_Saturday()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var expected = new DateTime(2000, 1, 3, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(1).Weekdays().At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(DayOfWeek.Saturday, input.DayOfWeek);
            Assert.AreEqual(DayOfWeek.Monday, actual.DayOfWeek);
        }

        [TestMethod]
        public void Should_Pick_Monday_If_Now_Is_Sunday()
        {
            // Arrange
            var input = new DateTime(2000, 1, 2, 1, 23, 25);
            var expected = new DateTime(2000, 1, 3, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(1).Weekdays().At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(DayOfWeek.Sunday, input.DayOfWeek);
            Assert.AreEqual(DayOfWeek.Monday, actual.DayOfWeek);
        }

        [TestMethod]
        public void Should_Pick_Tuesday_If_Now_Is_Too_Late_Monday()
        {
            // Assert
            var input = new DateTime(2000, 1, 3, 12, 23, 25);
            var expected = new DateTime(2000, 1, 4, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(1).Weekdays().At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(DayOfWeek.Monday, input.DayOfWeek);
            Assert.AreEqual(DayOfWeek.Tuesday, actual.DayOfWeek);
        }

        [TestMethod]
        public void Should_Skip_Monday_If_Now_Is_Saturday()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var expected = new DateTime(2000, 1, 4, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Weekdays().At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(DayOfWeek.Saturday, input.DayOfWeek);
            Assert.AreEqual(DayOfWeek.Tuesday, actual.DayOfWeek);
        }

        [TestMethod]
        public void Should_Pick_Same_Day_If_Now_Is_In_Time_Monday()
        {
            // Arrange
            var input = new DateTime(2000, 1, 3, 1, 23, 25);
            var expected = new DateTime(2000, 1, 3, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Weekdays().At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(DayOfWeek.Monday, input.DayOfWeek);
            Assert.AreEqual(DayOfWeek.Monday, actual.DayOfWeek);
        }

        [TestMethod]
        public void Should_Pick_Wednesday_If_Now_Is_Too_Late_Monday()
        {
            // Arrange
            var input = new DateTime(2000, 1, 3, 12, 23, 25);
            var expected = new DateTime(2000, 1, 5, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Weekdays().At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(DayOfWeek.Monday, input.DayOfWeek);
            Assert.AreEqual(DayOfWeek.Wednesday, actual.DayOfWeek);
        }

        [TestMethod]
        public void Should_Pick_Monday_If_Now_Is_Too_Late_Thursday()
        {
            // Arrange
            var input = new DateTime(2000, 1, 6, 12, 23, 25);
            var expected = new DateTime(2000, 1, 10, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Weekdays().At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(DayOfWeek.Thursday, input.DayOfWeek);
            Assert.AreEqual(DayOfWeek.Monday, actual.DayOfWeek);
        }
    }
}

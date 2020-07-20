namespace FluentScheduler.UnitTests.ScheduleTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class DaysTests
    {
        [TestMethod]
        public void Should_Add_Specified_Days_To_Next_Run_Date()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var expected = new DateTime(2000, 1, 2);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(1).Days();
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Default_To_00_00_If_At_Is_Not_Defined()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var expected = new DateTime(2000, 1, 2);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(1).Days();
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Set_Specific_Hour_And_Minute_If_At_Method_Is_Called()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1);
            var expected = new DateTime(2000, 1, 1, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(1).Days().At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Pick_Next_Date_If_Now_Is_After_At_Time()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 3, 15, 0).AddMilliseconds(1);
            var expected = new DateTime(2000, 1, 4, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(3).Days().At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Pick_Today_If_Now_Is_Before_At_Time()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var expected = new DateTime(2000, 1, 1, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(3).Days().At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Set_To_Next_Interval_If_Inputted_Time_Is_After_Run_Time_By_A_Millisecond()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 3, 15, 0).AddMilliseconds(1);
            var expected = new DateTime(2000, 1, 2, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(1).Days().At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Override_Existing_Minutes_And_Seconds_If_At_Method_Is_Called()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var expected = new DateTime(2000, 1, 1, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(1).Days().At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}

namespace FluentScheduler.UnitTests.ScheduleTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class MonthsOnTests
    {
        [TestMethod]
        public void Should_Add_Specified_Months_To_Next_Run_Date_And_Select_Specified_Day()
        {
            // Assert
            var input = new DateTime(2000, 1, 6);
            var expected = new DateTime(2000, 3, 5);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().On(5);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Default_To_00_00_If_At_Is_Not_Defined()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var expected = new DateTime(2000, 3, 1);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().On(1);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Pick_Last_Day_If_Specified_Day_Does_Not_Exist_In_Month()
        {
            // Arrange
            var input = new DateTime(2000, 2, 1, 1, 23, 25);
            var expected = new DateTime(2000, 2, 29);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(1).Months().On(31);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Override_Existing_Minutes_And_Seconds_If_At_Method_Is_Called()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 5, 23, 25);
            var expected = new DateTime(2000, 3, 1, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().On(1).At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Handle_Negative_Numbers()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var expected = new DateTime(2000, 2, 28);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().On(-1);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Pick_Next_Date_If_Now_Is_After_At_Time()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 3, 15, 0).AddMilliseconds(1);
            var expected = new DateTime(2000, 4, 1, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(3).Months().On(1).At(3, 15);
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
            schedule.ToRunEvery(3).Months().On(1).At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Set_To_Next_Interval_If_Inputted_Time_Is_After_Run_Time_By_A_Millisecond()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 3, 15, 0).AddMilliseconds(1);
            var expected = new DateTime(2000, 2, 1, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(1).Months().On(1).At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Pick_This_Month_If_Now_Is_Before_At_Time()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var expected = new DateTime(2000, 1, 2, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(3).Months().On(2).At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}

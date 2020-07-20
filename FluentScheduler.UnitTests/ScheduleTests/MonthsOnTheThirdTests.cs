namespace FluentScheduler.UnitTests.ScheduleTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class MonthsOnTheThirdTests
    {
        [TestMethod]
        public void Should_Default_To_00_00_If_At_Is_Not_Defined()
        {
            // Arrange
            var input = new DateTime(2000, 1, 31, 1, 23, 25);
            var expected = new DateTime(2000, 3, 20, 0, 0, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheThird(DayOfWeek.Monday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(DayOfWeek.Monday, actual.DayOfWeek);
        }

        [TestMethod]
        public void Should_Set_Specific_Hour_And_Minute_If_At_Method_Is_Called()
        {
            // Arrange
            var input = new DateTime(2000, 1, 31);
            var expected = new DateTime(2000, 3, 20, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheThird(DayOfWeek.Monday).At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(DayOfWeek.Monday, actual.DayOfWeek);
        }

        [TestMethod]
        public void Should_Override_Existing_Minutes_And_Seconds_If_At_Method_Is_Called()
        {
            // Arrange
            var input = new DateTime(2000, 1, 31, 1, 23, 25);
            var expected = new DateTime(2000, 3, 20, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheThird(DayOfWeek.Monday).At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(DayOfWeek.Monday, actual.DayOfWeek);
        }

        [TestMethod]
        public void Should_Select_The_Date_If_The_Next_Runtime_Falls_On_The_Specified_Day()
        {
            // Arrange
            var input = new DateTime(2000, 1, 31);
            var expected = new DateTime(2000, 3, 15);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheThird(DayOfWeek.Wednesday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(DayOfWeek.Wednesday, actual.DayOfWeek);
        }

        [TestMethod]
        public void Should_Ignore_The_Specified_Day()
        {
            // Arrange
            var input = new DateTime(2000, 1, 25);
            var expected = new DateTime(2000, 3, 16);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheThird(DayOfWeek.Thursday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(DayOfWeek.Thursday, actual.DayOfWeek);
        }

        [TestMethod]
        public void Should_Pick_The_Day_Of_Week_Specified()
        {
            // Arrange
            var input = new DateTime(2000, 1, 31);
            var expected = new DateTime(2000, 3, 17);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheThird(DayOfWeek.Friday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(DayOfWeek.Friday, actual.DayOfWeek);
        }

        [TestMethod]
        public void Should_Pick_The_Next_Week_If_The_Day_Of_Week_Has_Passed()
        {
            // Arrange
            var input = new DateTime(2000, 1, 31);
            var expected = new DateTime(2000, 3, 21);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheThird(DayOfWeek.Tuesday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(DayOfWeek.Tuesday, actual.DayOfWeek);
        }

        [TestMethod]
        public void Should_Pick_The_Next_Week_If_The_Day_Of_Week_Has_Passed_For_New_Weeks()
        {
            // Arrange
            var input = new DateTime(2000, 1, 31);
            var expected = new DateTime(2000, 10, 21);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(9).Months().OnTheThird(DayOfWeek.Saturday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(DayOfWeek.Saturday, actual.DayOfWeek);
        }

        [TestMethod]
        public void Should_Pick_The_Next_Week_If_The_Day_Of_Week_Has_Passed_For_End_Of_Week()
        {
            // Arrange
            var input = new DateTime(2000, 1, 31);
            var expected = new DateTime(2000, 4, 16);

            // ACt
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(3).Months().OnTheThird(DayOfWeek.Sunday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(DayOfWeek.Sunday, actual.DayOfWeek);
        }
    }
}

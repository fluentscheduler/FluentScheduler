namespace FluentScheduler.UnitTests.ScheduleTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class YearsOnTheLastDayTests
    {
        [TestMethod]
        public void Should_Add_Specified_Years_To_Next_Run_Date_And_Select_Last_Day_In_That_Year()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1);
            var expected = new DateTime(2000, 12, 31);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Years().OnTheLastDay();
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Default_To_00_00_If_At_Is_Not_Defined()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var expected = new DateTime(2000, 12, 31);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Years().OnTheLastDay();
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Override_Existing_Minutes_And_Seconds_If_At_Method_Is_Called()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var expected = new DateTime(2000, 12, 31, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Years().OnTheLastDay().At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}

namespace FluentScheduler.UnitTests.ScheduleTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class YearsOnTests
    {
        [TestMethod]
        public void Should_Add_Specified_Years_To_Next_Run_Date_And_Select_Specified_Day()
        {
            // Arrange
            var input = new DateTime(2000, 2, 15);
            var expected = new DateTime(2002, 1, 5);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Years().On(5);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Default_To_00_00_If_At_Is_Not_Defined()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var expected = new DateTime(2002, 1, 1);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Years().On(1);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Not_Fail_If_Specified_Day_Does_Not_Exist_In_Year()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1);
            var expected = new DateTime(2001, 2, 3);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(1).Years().On(400);
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
            schedule.ToRunEvery(2).Years().On(1).At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Handle_Negative_Numbers()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1);
            var expected = new DateTime(2001, 12, 30);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Years().On(-1);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}

namespace FluentScheduler.UnitTests.ScheduleTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class HoursTests
    {
        [TestMethod]
        public void Should_Add_Specified_Hours_To_Next_Run_Date()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1);
            var expected = new DateTime(2000, 1, 1, 2, 0, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Hours();
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Roll_To_StartHour_On_Next_Run_Date_If_After_Bound()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 16, 0, 0);
            var expected = new DateTime(2000, 1, 2, 10, 0, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Hours().Between(10, 00, 15, 00);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Set_To_Next_Interval_If_Inputted_Time_Is_After_Run_Time_By_A_Millisecond()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 5, 30, 0).AddMilliseconds(1);
            var expected = new DateTime(2000, 1, 1, 6, 30, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(1).Hours().At(30);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Set_Specific_Minute_If_At_Method_Is_Called()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1);
            var expected = new DateTime(2000, 1, 1, 2, 30, 0);

            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Hours().At(30);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Override_Existing_Minutes_And_Seconds_If_At_Method_Is_Called()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 5, 23, 25);
            var expected = new DateTime(2000, 1, 1, 7, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Hours().At(15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Pick_Next_Interval_If_Specified_Time_Is_After_Specified_At_Minutes()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 5, 23, 25);
            var expected = new DateTime(2000, 1, 1, 6, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(1).Hours().At(15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Pick_Next_Interval_Within_Bound_If_Specified_Time_Is_After_Specified_At_Minutes()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 5, 23, 25);
            var expected = new DateTime(2000, 1, 1, 6, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(1).Hours().At(15).Between(5, 00, 15, 00);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Pick_Next_Interval_If_Specified_Time_Is_After_Specified_At_Minutes2()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 5, 23, 25);
            var expected = new DateTime(2000, 1, 1, 8, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(3).Hours().At(15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Pick_Current_Hour_If_Specified_Time_Is_Before_Specified_At_Minutes()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 5, 14, 25);
            var expected = new DateTime(2000, 1, 1, 5, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(1).Hours().At(15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Pick_Current_Hour_If_Specified_Time_Is_Before_Specified_At_Minutes2()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 5, 14, 25);
            var expected = new DateTime(2000, 1, 1, 8, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(3).Hours().At(15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}

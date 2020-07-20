namespace FluentScheduler.UnitTests.ScheduleTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class WeeksTests
    {
        [TestMethod]
        public void Should_Add_Specified_Weeks_To_Next_Run_Date()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1);
            var expected = new DateTime(2000, 1, 15);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Weeks();
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Default_To_00_00_If_At_Is_Not_Defined()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var expected = new DateTime(2000, 1, 15);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Weeks();
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
            schedule.ToRunEvery(2).Weeks().At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Override_Existing_Minutes_And_Seconds_If_At_Method_Is_Called()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 5, 23, 25);
            var expected = new DateTime(2000, 1, 15, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Weeks().At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Default_To_00_00_If_On_Is_Specified_And_At_Is_Not_Defined()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var expected = new DateTime(2000, 1, 15);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Saturday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Set_Specific_Hour_And_Minute_If_On_Is_Specified_And_At_Method_Is_Called()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1);
            var expected = new DateTime(2000, 1, 15, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Saturday).At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Override_Existing_Minutes_And_Seconds_If_On_Is_Specified_And_At_Method_Is_Called()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var expected = new DateTime(2000, 1, 15, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Saturday).At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Select_The_Date_If_The_Next_Runtime_Falls_On_The_Specified_Day()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1);
            var actual = new DateTime(2000, 1, 15);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Saturday);
            var expected = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Pick_The_Day_Of_Week_Specified()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1);
            var expected = new DateTime(2000, 1, 16);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Sunday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Pick_The_Next_Week_If_The_Day_Of_Week_Has_Passed()
        {
            // Arrange
            var input = new DateTime(2000, 1, 5);
            var actual = new DateTime(2000, 1, 24);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Monday);
            var expected = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Pick_The_Next_Week_If_The_Day_Of_Week_Has_Passed_For_New_Weeks()
        {
            // Arrange
            var input = new DateTime(2000, 1, 2);
            var actual = new DateTime(2000, 1, 22);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Saturday);
            var expected = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Pick_The_Next_Week_If_The_Day_Of_Week_Has_Passed_For_End_Of_Week()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1);
            var expected = new DateTime(2000, 1, 16);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Sunday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Pick_The_Next_Day_Of_Week_If_0_Weeks_Specified()
        {
            // Arrange
            var input = new DateTime(2000, 1, 4);
            var actual = new DateTime(2000, 1, 5);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(0).Weeks().On(DayOfWeek.Wednesday);
            var expected = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Pick_The_Next_Week_On_The_Specified_Day_Of_Week_If_0_Weeks_Specified()
        {
            // Arrange
            var input = new DateTime(2000, 1, 4, 1, 23, 25);
            var expected = new DateTime(2000, 1, 11);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(0).Weeks().On(DayOfWeek.Tuesday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Pick_The_Same_Day_Of_Week_If_0_Weeks_Specified_And_Before_Specified_Run_Time()
        {
            // Arrange
            var input = new DateTime(2000, 1, 4, 3, 15, 0);
            var expected = new DateTime(2000, 1, 4, 4, 20, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(0).Weeks().On(DayOfWeek.Tuesday).At(4, 20);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Schedule_Today_If_Input_Time_Is_Before_Run_Time()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1);
            var expected = new DateTime(2000, 1, 1, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Weeks().At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_Not_Schedule_Today_If_Input_Time_Is_After_Run_Time()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 5, 23, 25);
            var expected = new DateTime(2000, 1, 15, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Weeks().At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}

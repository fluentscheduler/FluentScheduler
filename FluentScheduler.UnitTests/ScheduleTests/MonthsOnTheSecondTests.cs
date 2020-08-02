namespace FluentScheduler.UnitTests.ScheduleTests
{
    using Xunit;
    using System;

    public class MonthsOnTheSecondTests
    {
        [Fact]
        public void Should_Default_To_00_00_If_At_Is_Not_Defined()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var expected = new DateTime(2000, 1, 10);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheSecond(DayOfWeek.Monday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(DayOfWeek.Monday, actual.DayOfWeek);
        }

        [Fact]
        public void Should_Set_Specific_Hour_And_Minute_If_At_Method_Is_Called()
        {
            // Arrange
            var input = new DateTime(2000, 1, 16);
            var expected = new DateTime(2000, 3, 13, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheSecond(DayOfWeek.Monday).At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(DayOfWeek.Monday, actual.DayOfWeek);
        }

        [Fact]
        public void Should_Override_Existing_Minutes_And_Seconds_If_At_Method_Is_Called()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var expected = new DateTime(2000, 1, 10, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheSecond(DayOfWeek.Monday).At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(DayOfWeek.Monday, actual.DayOfWeek);
        }

        [Fact]
        public void Should_Select_The_Date_If_The_Next_Runtime_Falls_On_The_Specified_Day()
        {
            // Arrange
            var input = new DateTime(2000, 1, 16);
            var expected = new DateTime(2000, 3, 8);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheSecond(DayOfWeek.Wednesday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(DayOfWeek.Wednesday, actual.DayOfWeek);
        }

        [Fact]
        public void Should_Ignore_The_Specified_Day()
        {
            // Arrange
            var input = new DateTime(2000, 1, 25);
            var expected = new DateTime(2000, 3, 9);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheSecond(DayOfWeek.Thursday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(DayOfWeek.Thursday, actual.DayOfWeek);
        }

        [Fact]
        public void Should_Pick_The_Day_Of_Week_Specified()
        {
            // Arrange
            var input = new DateTime(2000, 1, 15);
            var expected = new DateTime(2000, 3, 10);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheSecond(DayOfWeek.Friday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(DayOfWeek.Friday, actual.DayOfWeek);
        }

        [Fact]
        public void Should_Pick_The_Next_Week_If_The_Day_Of_Week_Has_Passed()
        {
            // Arrange
            var input = new DateTime(2000, 1, 15);
            var expected = new DateTime(2000, 3, 14);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheSecond(DayOfWeek.Tuesday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(DayOfWeek.Tuesday, actual.DayOfWeek);
        }

        [Fact]
        public void Should_Pick_The_Next_Week_If_The_Day_Of_Week_Has_Passed_For_New_Weeks()
        {
            // Arrange
            var input = new DateTime(2000, 1, 16);
            var expected = new DateTime(2000, 10, 14);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(9).Months().OnTheSecond(DayOfWeek.Saturday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(DayOfWeek.Saturday, actual.DayOfWeek);
        }

        [Fact]
        public void Should_Pick_The_Next_Week_If_The_Day_Of_Week_Has_Passed_For_End_Of_Week()
        {
            // Arrange
            var input = new DateTime(2000, 1, 16);
            var expected = new DateTime(2000, 4, 9);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(3).Months().OnTheSecond(DayOfWeek.Sunday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(DayOfWeek.Sunday, actual.DayOfWeek);
        }
    }
}

namespace FluentScheduler.UnitTests.ScheduleTests
{
    using Xunit;
    using System;

    public class MonthsOnTheLastTests
    {
        [Fact]
        public void Should_Default_To_00_00_If_At_Is_Not_Defined()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var expected = new DateTime(2000, 1, 31, 0, 0, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheLast(DayOfWeek.Monday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(DayOfWeek.Monday, actual.DayOfWeek);
        }

        [Fact]
        public void Should_Set_Specific_Hour_And_Minute_If_At_Method_Is_Called()
        {
            // Arrange
            var input = new DateTime(2000, 1, 1);
            var expected = new DateTime(2000, 1, 31, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheLast(DayOfWeek.Monday).At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(DayOfWeek.Monday, actual.DayOfWeek);
        }

        [Fact]
        public void Should_Override_Existing_Minutes_And_Seconds_If_At_Method_Is_Called()
        {
            // Arrange
            var input = new DateTime(2000, 1, 31, 5, 23, 25);
            var expected = new DateTime(2000, 3, 27, 3, 15, 0);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheLast(DayOfWeek.Monday).At(3, 15);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(DayOfWeek.Monday, actual.DayOfWeek);
        }

        [Fact]
        public void Should_Select_The_Date_If_The_Next_Runtime_Falls_On_The_Specified_Day()
        {
            // Arrange
            var input = new DateTime(2000, 1, 31);
            var expected = new DateTime(2000, 3, 31);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheLast(DayOfWeek.Friday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(DayOfWeek.Friday, actual.DayOfWeek);
        }

        [Fact]
        public void Should_Select_The_Date_If_The_Next_Runtime_Falls_Before_The_Specified_Day()
        {
            // Arrange
            var input = new DateTime(2000, 1, 31);
            var expected = new DateTime(2000, 3, 30);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheLast(DayOfWeek.Thursday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(DayOfWeek.Thursday, actual.DayOfWeek);
        }

        [Fact]
        public void Should_Pick_The_Previous_Week_If_The_Day_Of_Week_Has_Passed()
        {
            // Arrange
            var input = new DateTime(2000, 1, 31);
            var expected = new DateTime(2000, 3, 25);

            // Act
            var schedule = new Schedule(() => { });
            schedule.ToRunEvery(2).Months().OnTheLast(DayOfWeek.Saturday);
            var actual = schedule.CalculateNextRun(input);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(DayOfWeek.Saturday, actual.DayOfWeek);
        }
    }
}

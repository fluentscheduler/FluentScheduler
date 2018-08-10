using System;
using Xunit;

namespace FluentScheduler.Tests.UnitTests.ScheduleTests
{
  public class WeekDaysTests
  {
    [Fact]
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
      Assert.Equal(expected, actual);
      Assert.Equal(DayOfWeek.Friday, input.DayOfWeek);
      Assert.Equal(DayOfWeek.Friday, actual.DayOfWeek);
    }

    [Fact]
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
      Assert.Equal(expected, actual);
      Assert.Equal(DayOfWeek.Friday, input.DayOfWeek);
      Assert.Equal(DayOfWeek.Monday, actual.DayOfWeek);
    }

    [Fact]
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
      Assert.Equal(expected, actual);
      Assert.Equal(DayOfWeek.Saturday, input.DayOfWeek);
      Assert.Equal(DayOfWeek.Monday, actual.DayOfWeek);
    }

    [Fact]
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
      Assert.Equal(expected, actual);
      Assert.Equal(DayOfWeek.Sunday, input.DayOfWeek);
      Assert.Equal(DayOfWeek.Monday, actual.DayOfWeek);
    }

    [Fact]
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
      Assert.Equal(expected, actual);
      Assert.Equal(DayOfWeek.Monday, input.DayOfWeek);
      Assert.Equal(DayOfWeek.Tuesday, actual.DayOfWeek);
    }

    [Fact]
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
      Assert.Equal(expected, actual);
      Assert.Equal(DayOfWeek.Saturday, input.DayOfWeek);
      Assert.Equal(DayOfWeek.Tuesday, actual.DayOfWeek);
    }

    [Fact]
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
      Assert.Equal(expected, actual);
      Assert.Equal(DayOfWeek.Monday, input.DayOfWeek);
      Assert.Equal(DayOfWeek.Monday, actual.DayOfWeek);
    }

    [Fact]
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
      Assert.Equal(expected, actual);
      Assert.Equal(DayOfWeek.Monday, input.DayOfWeek);
      Assert.Equal(DayOfWeek.Wednesday, actual.DayOfWeek);
    }

    [Fact]
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
      Assert.Equal(expected, actual);
      Assert.Equal(DayOfWeek.Thursday, input.DayOfWeek);
      Assert.Equal(DayOfWeek.Monday, actual.DayOfWeek);
    }
  }
}

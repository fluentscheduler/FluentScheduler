using System;
using FluentScheduler.Extension;
using Xunit;

namespace FluentScheduler.Tests.UnitTests.ScheduleTests
{
  public class MonthsWeekDaysOnlyTests
  {
    [Fact]
    public void Should_Pick_Monday_If_Now_Is_Saturday()
    {
      // Arrange
      var input = new DateTime(2016, 10, 1, 2, 0, 0);
      var expected = new DateTime(2016, 10, 3, 3, 15, 0);

      // Act
      var schedule = new Schedule(() => { });
      schedule.ToRunEvery(1).Months().On(1).At(3, 15).WeekdaysOnly();
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
      var input = new DateTime(2016, 10, 2, 2, 0, 0);
      var expected = new DateTime(2016, 10, 3, 3, 15, 0);

      // Act
      var schedule = new Schedule(() => { });
      schedule.ToRunEvery(1).Months().On(2).At(3, 15).WeekdaysOnly();
      var actual = schedule.CalculateNextRun(input);

      // Assert
      Assert.Equal(expected, actual);
      Assert.Equal(DayOfWeek.Sunday, input.DayOfWeek);
      Assert.Equal(DayOfWeek.Monday, actual.DayOfWeek);
    }

    [Fact]
    public void Should_Pick_Today_If_Now_Is_Monday()
    {
      // Arrange
      var input = new DateTime(2016, 8, 1, 2, 0, 0);
      var expected = new DateTime(2016, 8, 1, 3, 15, 0);

      // Act
      var schedule = new Schedule(() => { });
      schedule.ToRunEvery(1).Months().On(1).At(3, 15).WeekdaysOnly();
      var actual = schedule.CalculateNextRun(input);

      // Assert
      Assert.Equal(expected, actual);
      Assert.Equal(DayOfWeek.Monday, input.DayOfWeek);
      Assert.Equal(DayOfWeek.Monday, actual.DayOfWeek);
      Assert.Equal(8, actual.Month);
    }

    [Fact]
    public void Should_Pick_Next_Month_If_Now_Is_Too_Late_On_Provided_Day()
    {
      // Arrange
      var runHour = 3;
      var input = new DateTime(2016, 8, 4, runHour + 1, 15, 0);
      var expected = new DateTime(2016, 9, 5, 3, 15, 0);

      // Act
      var schedule = new Schedule(() => { });
      schedule.ToRunEvery(1).Months().On(4).At(runHour, 15).WeekdaysOnly();
      var actual = schedule.CalculateNextRun(input);

      // Assert
      Assert.Equal(expected, actual);
      Assert.Equal(DayOfWeek.Thursday, input.DayOfWeek);
      Assert.Equal(DayOfWeek.Monday, actual.DayOfWeek);
      Assert.Equal(9, actual.Month);
    }
  }
}

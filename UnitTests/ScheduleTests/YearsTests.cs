using System;
using Xunit;

namespace Moong.FluentScheduler.Tests.UnitTests.ScheduleTests
{
  public class YearsTests
  {
    [Fact]
    public void Should_Add_Specified_Years_To_Next_Run_Date()
    {
      // Arrange
      var input = new DateTime(2000, 1, 1);
      var expected = new DateTime(2002, 1, 1);

      // Act
      var schedule = new Schedule(() => { });
      schedule.ToRunEvery(2).Years();
      var actual = schedule.CalculateNextRun(input);

      // Assert
      Assert.Equal(expected, actual);
    }

    [Fact]
    public void Should_Default_To_00_00_If_At_Is_Not_Defined()
    {
      // Arrange
      var input = new DateTime(2000, 1, 1, 1, 23, 25);
      var expected = new DateTime(2002, 1, 1);

      // Act
      var schedule = new Schedule(() => { });
      schedule.ToRunEvery(2).Years();
      var actual = schedule.CalculateNextRun(input);

      // Assert
      Assert.Equal(expected, actual);
    }
  }
}

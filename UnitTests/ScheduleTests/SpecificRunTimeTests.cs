using System;
using System.Linq;
using Xunit;

namespace FluentScheduler.Tests.UnitTests.ScheduleTests
{
  public class SpecificRunTimeTests
  {
    [Fact]
    public void Should_Add_Chained_Jobs_To_AdditionalSchedules_Property()
    {
      // Act
      var schedule = new Schedule(() => { });
      schedule.ToRunNow().AndEvery(1).Months();

      // Assert
      Assert.Equal(1, schedule.AdditionalSchedules.Count);
    }

    [Fact]
    public void Should_Set_Chained_Job_Schedule_As_Expected()
    {
      // Arrange
      var input = new DateTime(2000, 1, 1);
      var expected = new DateTime(2000, 3, 1);

      // Act
      var schedule = new Schedule(() => { });
      schedule.ToRunNow().AndEvery(2).Months();
      var actual = schedule.AdditionalSchedules.ElementAt(0).CalculateNextRun(input);

      // Assert
      Assert.Equal(expected, actual);
    }

    [Fact]
    public void Should_Not_Alter_Original_Runtime_If_Chained_Job_Exists()
    {
      // Act
      var schedule = new Schedule(() => { });
      schedule.ToRunNow().AndEvery(1).Months();

      // Assert
      Assert.Null(schedule.CalculateNextRun);
    }
  }
}

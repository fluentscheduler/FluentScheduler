using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;
using FluentAssertions;

namespace FluentScheduler.Tests.UnitTests.ScheduleTests
{
  [TestFixture]
  public class SecondsTests
  {
    [Test]
    public void Should_Add_Specified_Seconds_To_Next_Run_Date()
    {
      var task = new Mock<ITask>();
      var schedule = new Schedule(task.Object);
      schedule.ToRunEvery(30).Seconds();

      var input = new DateTime(2000, 1, 1);
      var scheduledTime = schedule.CalculateNextRun(input);
      Assert.AreEqual(scheduledTime.Date, input.Date);
      Assert.AreEqual(scheduledTime.Hour, input.Hour);
      Assert.AreEqual(scheduledTime.Minute, input.Minute);
      Assert.AreEqual(scheduledTime.Second, 30);
    }

    [Test]
    public void Should_Add_Specified_Seconds_To_Same_Date_Within_Bounds()
    {
      var task = new Mock<ITask>();
      var schedule = new Schedule(task.Object);
      schedule.ToRunEvery(30).Seconds().Between(10, 0, 11, 0);

      var input = new DateTime(2000, 1, 1, 10, 15, 0);
      var scheduledTime = schedule.CalculateNextRun(input);
      Assert.AreEqual(scheduledTime.Date, input.Date);
      Assert.AreEqual(scheduledTime.Hour, input.Hour);
      Assert.AreEqual(scheduledTime.Minute, input.Minute);
      Assert.AreEqual(scheduledTime.Second, 30);
    }

    [Test]
    public void Should_Roll_To_Next_Run_Date_Bound_Start_As_After_Bounds()
    {
      var task = new Mock<ITask>();
      var schedule = new Schedule(task.Object);
      schedule.ToRunEvery(30).Seconds().Between(10, 0, 11, 0);

      var input = new DateTime(2000, 1, 1, 12, 0, 0);
      var scheduledTime = schedule.CalculateNextRun(input);

      scheduledTime.Date.Should().Be(input.Date.AddDays(1));
      scheduledTime.Hour.Should().Be(10);
      scheduledTime.Minute.Should().Be(0);
      scheduledTime.Second.Should().Be(0);
    }
  }
}

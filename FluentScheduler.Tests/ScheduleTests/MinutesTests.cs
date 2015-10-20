namespace FluentScheduler.Tests.ScheduleTests
{
  using System;
  using FluentScheduler.Model;
  using Moq;
  using NUnit.Framework;
  using FluentAssertions;
  using FluentScheduler.Extensions;

  [TestFixture]
  public class MinutesTests
  {
    [Test]
    public void Should_Add_Specified_Minutes_To_Next_Run_Date()
    {
      var task = new Mock<ITask>();
      var schedule = new Schedule(task.Object);
      schedule.ToRunEvery(30).Minutes();

      var input = new DateTime(2000, 1, 1);
      var scheduledTime = schedule.CalculateNextRun(input);
      Assert.AreEqual(scheduledTime.Date, input.Date);

      Assert.AreEqual(scheduledTime.Hour, input.Hour);
      Assert.AreEqual(scheduledTime.Minute, 30);
      Assert.AreEqual(scheduledTime.Second, input.Second);
    }

    [Test]
    public void Should_Add_Specified_Minutes_To_Next_Run_Date_When_Is_Between_Specified_Bounds()
    {
      var task = new Mock<ITask>();
      var schedule = new Schedule(task.Object);
      schedule.ToRunEvery(30).Minutes().Between(10, 00, 16, 00);

      var input = new DateTime(2000, 1, 6, 12, 23, 25);
      var scheduledTime = schedule.CalculateNextRun(input);
      Assert.AreEqual(scheduledTime.Date, input.Date);

      scheduledTime.Hour.Should().Be(input.Hour);
      scheduledTime.Minute.Should().Be(input.Minute + 30);
      scheduledTime.Second.Should().Be(input.Second);
    }

    [Test]
    public void Should_Delay_Next_Run_Date_Until_Specified_Start()
    {
      var task = new Mock<ITask>();
      var schedule = new Schedule(task.Object);
      schedule.ToRunEvery(2).Minutes().Between(10, 00, 11, 00);

      var input = new DateTime(2000, 1, 6, 1, 23, 25);
      var scheduledTime = schedule.CalculateNextRun(input);
      scheduledTime.Date.Should().Be(input.Date);

      scheduledTime.Hour.Should().Be(10);
      scheduledTime.Minute.Should().Be(0);
      scheduledTime.Second.Should().Be(0);
    }
  }
}

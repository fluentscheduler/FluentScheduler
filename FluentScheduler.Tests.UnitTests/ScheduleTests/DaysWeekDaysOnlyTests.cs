using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;
using FluentAssertions;

namespace FluentScheduler.Tests.UnitTests.ScheduleTests
{
  [TestFixture]
  public class DaysWeekDaysOnlyTests
  {
    [Test]
    public void Should_Pick_Same_Day_If_Now_Is_In_Time_On_Friday()
    {
      var task = new Mock<ITask>();
      var schedule = new Schedule(task.Object);
      schedule.ToRunEvery(1).Days().At(3, 15).WeekDaysOnly();

      var input = new DateTime(1999, 12, 31, 1, 23, 25);
      input.DayOfWeek.Should().Be(DayOfWeek.Friday);
      var scheduledTime = schedule.CalculateNextRun(input);
      scheduledTime.Date.Should().Be(input.Date);

      scheduledTime.Hour.Should().Be(3);
      scheduledTime.Minute.Should().Be(15);
      scheduledTime.Second.Should().Be(0);
    }

    [Test]
    public void Should_Pick_Monday_If_Now_Is_Too_Late_On_Friday()
    {
      var task = new Mock<ITask>();
      var schedule = new Schedule(task.Object);
      schedule.ToRunEvery(1).Days().At(3, 15).WeekDaysOnly();

      var input = new DateTime(1999, 12, 31, 12, 23, 25);
      input.DayOfWeek.Should().Be(DayOfWeek.Friday);
      var scheduledTime = schedule.CalculateNextRun(input);
      scheduledTime.Date.Should().Be(input.AddDays(3).Date);

      scheduledTime.Hour.Should().Be(3);
      scheduledTime.Minute.Should().Be(15);
      scheduledTime.Second.Should().Be(0);
    }

    [Test]
    public void Should_Pick_Monday_If_Now_Is_Saturday()
    {
      var task = new Mock<ITask>();
      var schedule = new Schedule(task.Object);
      schedule.ToRunEvery(1).Days().At(3, 15).WeekDaysOnly();

      var input = new DateTime(2000, 1, 1, 1, 23, 25);
      input.DayOfWeek.Should().Be(DayOfWeek.Saturday);
      var scheduledTime = schedule.CalculateNextRun(input);
      scheduledTime.Date.Should().Be(input.AddDays(2).Date);

      scheduledTime.Hour.Should().Be(3);
      scheduledTime.Minute.Should().Be(15);
      scheduledTime.Second.Should().Be(0);
    }

    [Test]
    public void Should_Pick_Monday_If_Now_Is_Sunday()
    {
      var task = new Mock<ITask>();
      var schedule = new Schedule(task.Object);
      schedule.ToRunEvery(1).Days().At(3, 15).WeekDaysOnly();

      var input = new DateTime(2000, 1, 2, 1, 23, 25);
      input.DayOfWeek.Should().Be(DayOfWeek.Sunday);
      var scheduledTime = schedule.CalculateNextRun(input);
      scheduledTime.Date.Should().Be(input.AddDays(1).Date);

      scheduledTime.Hour.Should().Be(3);
      scheduledTime.Minute.Should().Be(15);
      scheduledTime.Second.Should().Be(0);
    }

    [Test]
    public void Should_Pick_Tuesday_If_Now_Is_Too_Late_Monday()
    {
      var task = new Mock<ITask>();
      var schedule = new Schedule(task.Object);
      schedule.ToRunEvery(1).Days().At(3, 15).WeekDaysOnly();

      var input = new DateTime(2000, 1, 3, 12, 23, 25);
      input.DayOfWeek.Should().Be(DayOfWeek.Monday);
      var scheduledTime = schedule.CalculateNextRun(input);
      scheduledTime.Date.Should().Be(input.AddDays(1).Date);

      scheduledTime.Hour.Should().Be(3);
      scheduledTime.Minute.Should().Be(15);
      scheduledTime.Second.Should().Be(0);
    }
  }
}

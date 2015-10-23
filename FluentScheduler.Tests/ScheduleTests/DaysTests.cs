using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;

namespace FluentScheduler.Tests.ScheduleTests
{
    [TestFixture]
    public class DaysTests
    {
        [Test]
        public void Should_Add_Specified_Days_To_Next_Run_Date()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(1).Days();

            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var scheduledTime = schedule.CalculateNextRun(input);
            Assert.AreEqual(scheduledTime, input.Date.AddDays(1));
        }

        [Test]
        public void Should_Default_To_00_00_If_At_Is_Not_Defined()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(1).Days();

            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var scheduledTime = schedule.CalculateNextRun(input);

            Assert.AreEqual(scheduledTime.Hour, 0);
            Assert.AreEqual(scheduledTime.Minute, 0);
            Assert.AreEqual(scheduledTime.Second, 0);
        }

        [Test]
        public void Should_Set_Specific_Hour_And_Minute_If_At_Method_Is_Called()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(1).Days().At(3, 15);

            var input = new DateTime(2000, 1, 1);
            var scheduledTime = schedule.CalculateNextRun(input);
            Assert.AreEqual(scheduledTime.Date, input.Date);

            Assert.AreEqual(scheduledTime.Hour, 3);
            Assert.AreEqual(scheduledTime.Minute, 15);
            Assert.AreEqual(scheduledTime.Second, 0);
        }

        [Test]
        public void Should_Pick_Next_Date_If_Now_Is_After_At_Time()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(3).Days().At(3, 15);

            var input = new DateTime(2000, 1, 1, 3, 15, 0).AddMilliseconds(1);
            var scheduledTime = schedule.CalculateNextRun(input);
            Assert.AreEqual(scheduledTime.Date, new DateTime(2000, 1, 4));

            Assert.AreEqual(scheduledTime.Hour, 3);
            Assert.AreEqual(scheduledTime.Minute, 15);
            Assert.AreEqual(scheduledTime.Second, 0);
        }

        [Test]
        public void Should_Pick_Today_If_Now_Is_Before_At_Time()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(3).Days().At(3, 15);

            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var scheduledTime = schedule.CalculateNextRun(input);
            Assert.AreEqual(scheduledTime.Date, input.Date);

            Assert.AreEqual(scheduledTime.Hour, 3);
            Assert.AreEqual(scheduledTime.Minute, 15);
            Assert.AreEqual(scheduledTime.Second, 0);
        }

        [Test]
        public void Should_Set_To_Next_Interval_If_Inputted_Time_Is_After_Run_Time_By_A_Millisecond()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(1).Days().At(3, 15);

            var input = new DateTime(2000, 1, 1, 3, 15, 0).AddMilliseconds(1);
            var scheduledTime = schedule.CalculateNextRun(input);
            Assert.AreEqual(scheduledTime.Date, new DateTime(2000, 1, 2));

            Assert.AreEqual(scheduledTime.Hour, 3);
            Assert.AreEqual(scheduledTime.Minute, 15);
            Assert.AreEqual(scheduledTime.Second, 0);
        }

        [Test]
        public void Should_Override_Existing_Minutes_And_Seconds_If_At_Method_Is_Called()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(1).Days().At(3, 15);

            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var scheduledTime = schedule.CalculateNextRun(input);
            Assert.AreEqual(scheduledTime.Date, input.Date);

            Assert.AreEqual(scheduledTime.Hour, 3);
            Assert.AreEqual(scheduledTime.Minute, 15);
            Assert.AreEqual(scheduledTime.Second, 0);
        }
    }
}

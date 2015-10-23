using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;

namespace FluentScheduler.Tests.ScheduleTests
{
    [TestFixture]
    public class MonthsTests
    {
        [Test]
        public void Should_Add_Specified_Months_To_Next_Run_Date()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(2).Months();

            var input = new DateTime(2000, 1, 1);
            var scheduledTime = schedule.CalculateNextRun(input);
            var expectedTime = new DateTime(2000, 3, 1);
            Assert.AreEqual(scheduledTime, expectedTime);
        }

        [Test]
        public void Should_Default_To_00_00_If_At_Is_Not_Defined()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(2).Months();

            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var scheduledTime = schedule.CalculateNextRun(input);

            Assert.AreEqual(scheduledTime.Hour, 0);
            Assert.AreEqual(scheduledTime.Minute, 0);
            Assert.AreEqual(scheduledTime.Second, 0);
        }
    }
}

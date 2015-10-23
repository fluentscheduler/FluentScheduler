using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;

namespace FluentScheduler.Tests.ScheduleTests
{
    [TestFixture]
    public class YearsOnTests
    {
        [Test]
        public void Should_Add_Specified_Years_To_Next_Run_Date_And_Select_Specified_Day()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(2).Years().On(5);

            var input = new DateTime(2000, 2, 15);
            var scheduledTime = schedule.CalculateNextRun(input);
            var expectedTime = new DateTime(2002, 1, 5);
            Assert.AreEqual(scheduledTime, expectedTime);
        }

        [Test]
        public void Should_Default_To_00_00_If_At_Is_Not_Defined()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(2).Years().On(1);

            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var scheduledTime = schedule.CalculateNextRun(input);

            Assert.AreEqual(scheduledTime.Hour, 0);
            Assert.AreEqual(scheduledTime.Minute, 0);
            Assert.AreEqual(scheduledTime.Second, 0);
        }

        [Test]
        public void Should_Not_Fail_If_Specified_Day_Does_Not_Exist_In_Year()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(1).Years().On(400);

            var input = new DateTime(2000, 1, 1);
            var scheduledTime = schedule.CalculateNextRun(input);
            var expectedTime = new DateTime(2001, 2, 3);
            Assert.AreEqual(scheduledTime, expectedTime);
        }

        [Test]
        public void Should_Override_Existing_Minutes_And_Seconds_If_At_Method_Is_Called()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(2).Years().On(1).At(3, 15);

            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var scheduledTime = schedule.CalculateNextRun(input);
            var expectedTime = new DateTime(2000, 1, 1);
            Assert.AreEqual(scheduledTime.Date, expectedTime);

            Assert.AreEqual(scheduledTime.Hour, 3);
            Assert.AreEqual(scheduledTime.Minute, 15);
            Assert.AreEqual(scheduledTime.Second, 0);
        }        
        
        [Test]
        public void Should_Handle_Negative_Numbers()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(2).Years().On(-1);

            var input = new DateTime(2000, 1, 1);
            var scheduledTime = schedule.CalculateNextRun(input);
            var expectedTime = new DateTime(2001, 12, 30);
            Assert.AreEqual(scheduledTime.Date, expectedTime);
        }
    }
}

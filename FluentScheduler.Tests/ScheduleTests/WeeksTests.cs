using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;

namespace FluentScheduler.Tests.ScheduleTests
{
    [TestFixture]
    public class WeeksTests
    {
        [Test]
        public void Should_Add_Specified_Weeks_To_Next_Run_Date()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(2).Weeks();

            var input = new DateTime(2000, 1, 1);
            var scheduledTime = schedule.CalculateNextRun(input);
            var expectedTime = new DateTime(2000, 1, 15);
            Assert.AreEqual(scheduledTime, expectedTime);
        }

        [Test]
        public void Should_Default_To_00_00_If_At_Is_Not_Defined()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(2).Weeks();

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
            schedule.ToRunEvery(2).Weeks().At(3, 15);

            var input = new DateTime(2000, 1, 1);
            var scheduledTime = schedule.CalculateNextRun(input);
            Assert.AreEqual(scheduledTime.Date, input.Date);

            Assert.AreEqual(scheduledTime.Hour, 3);
            Assert.AreEqual(scheduledTime.Minute, 15);
            Assert.AreEqual(scheduledTime.Second, 0);
        }

        [Test]
        public void Should_Override_Existing_Minutes_And_Seconds_If_At_Method_Is_Called()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(2).Weeks().At(3, 15);

            var input = new DateTime(2000, 1, 1, 5, 23, 25);
            var scheduledTime = schedule.CalculateNextRun(input);
            Assert.AreEqual(scheduledTime.Date, input.Date.AddDays(14));

            Assert.AreEqual(scheduledTime.Hour, 3);
            Assert.AreEqual(scheduledTime.Minute, 15);
            Assert.AreEqual(scheduledTime.Second, 0);
        }

        [Test]
        public void Should_Default_To_00_00_If_On_Is_Specified_And_At_Is_Not_Defined()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Saturday);

            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var scheduledTime = schedule.CalculateNextRun(input);

            Assert.AreEqual(scheduledTime.Hour, 0);
            Assert.AreEqual(scheduledTime.Minute, 0);
            Assert.AreEqual(scheduledTime.Second, 0);
        }

        [Test]
        public void Should_Set_Specific_Hour_And_Minute_If_On_Is_Specified_And_At_Method_Is_Called()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Saturday).At(3, 15);

            var input = new DateTime(2000, 1, 1);
            var scheduledTime = schedule.CalculateNextRun(input);
            Assert.AreEqual(scheduledTime.Date, input.Date.AddDays(14));

            Assert.AreEqual(scheduledTime.Hour, 3);
            Assert.AreEqual(scheduledTime.Minute, 15);
            Assert.AreEqual(scheduledTime.Second, 0);
        }

        [Test]
        public void Should_Override_Existing_Minutes_And_Seconds_If_On_Is_Specified_And_At_Method_Is_Called()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Saturday).At(3, 15);

            var input = new DateTime(2000, 1, 1, 1, 23, 25);
            var scheduledTime = schedule.CalculateNextRun(input);
            Assert.AreEqual(scheduledTime.Date, input.Date.AddDays(14));

            Assert.AreEqual(scheduledTime.Hour, 3);
            Assert.AreEqual(scheduledTime.Minute, 15);
            Assert.AreEqual(scheduledTime.Second, 0);
        }

        [Test]
        public void Should_Select_The_Date_If_The_Next_Runtime_Falls_On_The_Specified_Day()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Saturday);

            var input = new DateTime(2000, 1, 1); // Saturday
            var scheduledTime = schedule.CalculateNextRun(input);

            var expectedTime = new DateTime(2000, 1, 15);
            Assert.AreEqual(scheduledTime, expectedTime);
        }

        [Test]
        public void Should_Pick_The_Day_Of_Week_Specified()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Sunday);

            var input = new DateTime(2000, 1, 1);
            var scheduledTime = schedule.CalculateNextRun(input);

            var expectedTime = new DateTime(2000, 1, 16);
            Assert.AreEqual(scheduledTime, expectedTime);
        }

        [Test]
        public void Should_Pick_The_Next_Week_If_The_Day_Of_Week_Has_Passed()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Monday);

            var input = new DateTime(2000, 1, 5); // Wednesday
            var scheduledTime = schedule.CalculateNextRun(input);

            var expectedTime = new DateTime(2000, 1, 24);
            Assert.AreEqual(scheduledTime, expectedTime);
        }

        [Test]
        public void Should_Pick_The_Next_Week_If_The_Day_Of_Week_Has_Passed_For_New_Weeks()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Saturday);

            var input = new DateTime(2000, 1, 2); // Sunday
            var scheduledTime = schedule.CalculateNextRun(input);

            var expectedTime = new DateTime(2000, 1, 22);
            Assert.AreEqual(scheduledTime, expectedTime);
        }

        [Test]
        public void Should_Pick_The_Next_Week_If_The_Day_Of_Week_Has_Passed_For_End_Of_Week()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Sunday);

            var input = new DateTime(2000, 1, 1); // Saturday
            var scheduledTime = schedule.CalculateNextRun(input);

            var expectedTime = new DateTime(2000, 1, 16);
            Assert.AreEqual(scheduledTime, expectedTime);
        }

        [Test]
        public void Should_Pick_The_Next_Day_Of_Week_If_0_Weeks_Specified()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(0).Weeks().On(DayOfWeek.Wednesday);

            var input = new DateTime(2000, 1, 4); // Tuesday
            var scheduledTime = schedule.CalculateNextRun(input);

            var expectedTime = new DateTime(2000, 1, 5);
            Assert.AreEqual(scheduledTime, expectedTime);
        }

        [Test]
        public void Should_Pick_The_Next_Week_On_The_Specified_Day_Of_Week_If_0_Weeks_Specified()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(0).Weeks().On(DayOfWeek.Tuesday);

            var input = new DateTime(2000, 1, 4, 1, 23, 25); // Tuesday
            var scheduledTime = schedule.CalculateNextRun(input);

            var expectedTime = new DateTime(2000, 1, 11);
            Assert.AreEqual(scheduledTime, expectedTime);
        }

        [Test]
        public void Should_Pick_The_Same_Day_Of_Week_If_0_Weeks_Specified_And_Before_Specified_Run_Time()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(0).Weeks().On(DayOfWeek.Tuesday).At(4, 20);

            var input = new DateTime(2000, 1, 4, 3, 15, 0); // Tuesday
            var scheduledTime = schedule.CalculateNextRun(input);

            var expectedTime = new DateTime(2000, 1, 4, 4, 20, 0);
            Assert.AreEqual(scheduledTime, expectedTime);
        }

        [Test]
        public void Should_Schedule_Today_If_Input_Time_Is_Before_Run_Time()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(2).Weeks().At(3, 15);

            var input = new DateTime(2000, 1, 1);
            var scheduledTime = schedule.CalculateNextRun(input);
            Assert.AreEqual(scheduledTime.Date, input.Date);

            Assert.AreEqual(scheduledTime.Hour, 3);
            Assert.AreEqual(scheduledTime.Minute, 15);
            Assert.AreEqual(scheduledTime.Second, 0);
        }

        [Test]
        public void Should_Not_Schedule_Today_If_Input_Time_Is_After_Run_Time()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunEvery(2).Weeks().At(3, 15);

            var input = new DateTime(2000, 1, 1, 5, 23, 25);
            var scheduledTime = schedule.CalculateNextRun(input);
            Assert.AreEqual(scheduledTime.Date, input.Date.AddDays(14));

            Assert.AreEqual(scheduledTime.Hour, 3);
            Assert.AreEqual(scheduledTime.Minute, 15);
            Assert.AreEqual(scheduledTime.Second, 0);
        }
    }
}

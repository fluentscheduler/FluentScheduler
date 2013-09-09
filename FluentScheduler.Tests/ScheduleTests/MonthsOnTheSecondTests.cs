using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;

namespace FluentScheduler.Tests.ScheduleTests
{
	[TestFixture]
	public class MonthsOnTheSecondTests
	{
		[Test]
		public void Should_Default_To_00_00_If_At_Is_Not_Defined()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheSecond(DayOfWeek.Monday);

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
			schedule.ToRunEvery(2).Months().OnTheSecond(DayOfWeek.Monday).At(3, 15);

			var input = new DateTime(2000, 1, 16);
			var scheduledTime = schedule.CalculateNextRun(input);
			var expectedTime = new DateTime(2000, 3, 13);
			Assert.AreEqual(scheduledTime.Date, expectedTime.Date);

			Assert.AreEqual(scheduledTime.Hour, 3);
			Assert.AreEqual(scheduledTime.Minute, 15);
			Assert.AreEqual(scheduledTime.Second, 0);
		}

		[Test]
		public void Should_Override_Existing_Minutes_And_Seconds_If_At_Method_Is_Called()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheSecond(DayOfWeek.Monday).At(3, 15);

			var input = new DateTime(2000, 1, 1, 1, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);
			var expectedTime = new DateTime(2000, 1, 10);
			Assert.AreEqual(scheduledTime.Date, expectedTime.Date);

			Assert.AreEqual(scheduledTime.Hour, 3);
			Assert.AreEqual(scheduledTime.Minute, 15);
			Assert.AreEqual(scheduledTime.Second, 0);
		}

		[Test]
		public void Should_Select_The_Date_If_The_Next_Runtime_Falls_On_The_Specified_Day()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheSecond(DayOfWeek.Wednesday);

			var input = new DateTime(2000, 1, 16);
			var scheduledTime = schedule.CalculateNextRun(input);

			var expectedTime = new DateTime(2000, 3, 8);
			Assert.AreEqual(scheduledTime, expectedTime);
		}

		[Test]
		public void Should_Ignore_The_Specified_Day()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheSecond(DayOfWeek.Thursday);

			var input = new DateTime(2000, 1, 25);
			var scheduledTime = schedule.CalculateNextRun(input);

			var expectedTime = new DateTime(2000, 3, 9);
			Assert.AreEqual(scheduledTime, expectedTime);
		}

		[Test]
		public void Should_Pick_The_Day_Of_Week_Specified()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheSecond(DayOfWeek.Friday);

			var input = new DateTime(2000, 1, 15);
			var scheduledTime = schedule.CalculateNextRun(input);

			var expectedTime = new DateTime(2000, 3, 10);
			Assert.AreEqual(scheduledTime, expectedTime);
		}

		[Test]
		public void Should_Pick_The_Next_Week_If_The_Day_Of_Week_Has_Passed()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheSecond(DayOfWeek.Tuesday);

			var input = new DateTime(2000, 1, 15); 
			var scheduledTime = schedule.CalculateNextRun(input);

			var expectedTime = new DateTime(2000, 3, 14);
			Assert.AreEqual(scheduledTime, expectedTime);
		}

		[Test]
		public void Should_Pick_The_Next_Week_If_The_Day_Of_Week_Has_Passed_For_New_Weeks()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(9).Months().OnTheSecond(DayOfWeek.Saturday);

			var input = new DateTime(2000, 1, 16);
			var scheduledTime = schedule.CalculateNextRun(input);

			var expectedTime = new DateTime(2000, 10, 14);
			Assert.AreEqual(scheduledTime, expectedTime);
		}

		[Test]
		public void Should_Pick_The_Next_Week_If_The_Day_Of_Week_Has_Passed_For_End_Of_Week()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(3).Months().OnTheSecond(DayOfWeek.Sunday);

			var input = new DateTime(2000, 1, 16); 
			var scheduledTime = schedule.CalculateNextRun(input);

			var expectedTime = new DateTime(2000, 4, 9);
			Assert.AreEqual(scheduledTime, expectedTime);
		}
	}
}

using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;

namespace FluentScheduler.Tests.ScheduleTests
{
	[TestFixture]
	public class MonthsOnTests
	{
		[Test]
		public void Should_Add_Specified_Months_To_Next_Run_Date_And_Select_Specified_Day()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().On(5);

			var input = new DateTime(2000, 1, 6);
			var scheduledTime = schedule.CalculateNextRun(input);
			var expectedTime = new DateTime(2000, 3, 5);
			Assert.AreEqual(scheduledTime, expectedTime);
		}

		[Test]
		public void Should_Default_To_00_00_If_At_Is_Not_Defined()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().On(1);

			var input = new DateTime(2000, 1, 1, 1, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);

			Assert.AreEqual(scheduledTime.Hour, 0);
			Assert.AreEqual(scheduledTime.Minute, 0);
			Assert.AreEqual(scheduledTime.Second, 0);
		}

		[Test]
		public void Should_Not_Fail_If_Specified_Day_Does_Not_Exist_In_Month()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(1).Months().On(31);

			var input = new DateTime(2000, 2, 1, 1, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);
			var expectedTime = new DateTime(2000, 3, 2);
			Assert.AreEqual(scheduledTime, expectedTime);
		}

		[Test]
		public void Should_Override_Existing_Minutes_And_Seconds_If_At_Method_Is_Called()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().On(1).At(3, 15);

			var input = new DateTime(2000, 1, 1, 5, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);
			var expectedTime = new DateTime(2000, 3, 1);
			Assert.AreEqual(expectedTime, scheduledTime.Date);

			Assert.AreEqual(scheduledTime.Hour, 3);
			Assert.AreEqual(scheduledTime.Minute, 15);
			Assert.AreEqual(scheduledTime.Second, 0);
		}		
		
		[Test]
		public void Should_Handle_Negative_Numbers()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().On(-1);

			var input = new DateTime(2000, 1, 1, 1, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);
			var expectedTime = new DateTime(2000, 2, 28);
			Assert.AreEqual(expectedTime, scheduledTime.Date);
		}

		public void Should_Pick_Next_Date_If_Now_Is_After_At_Time()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(3).Months().On(1).At(3, 15);

			var input = new DateTime(2000, 1, 1, 3, 15, 0).AddMilliseconds(1);
			var scheduledTime = schedule.CalculateNextRun(input);
			Assert.AreEqual(scheduledTime.Date, new DateTime(2000, 4, 1));

			Assert.AreEqual(scheduledTime.Hour, 3);
			Assert.AreEqual(scheduledTime.Minute, 15);
			Assert.AreEqual(scheduledTime.Second, 0);
		}

		[Test]
		public void Should_Pick_Today_If_Now_Is_Before_At_Time()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(3).Months().On(1).At(3, 15);

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
			schedule.ToRunEvery(1).Months().On(1).At(3, 15);

			var input = new DateTime(2000, 1, 1, 3, 15, 0).AddMilliseconds(1);
			var scheduledTime = schedule.CalculateNextRun(input);
			Assert.AreEqual(scheduledTime.Date, new DateTime(2000, 2, 1));

			Assert.AreEqual(scheduledTime.Hour, 3);
			Assert.AreEqual(scheduledTime.Minute, 15);
			Assert.AreEqual(scheduledTime.Second, 0);
		}

		[Test]
		public void Should_Pick_This_Month_If_Now_Is_Before_At_Time()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(3).Months().On(2).At(3, 15);

			var input = new DateTime(2000, 1, 1, 1, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);
			Assert.AreEqual(scheduledTime.Date, new DateTime(2000, 1, 2));

			Assert.AreEqual(scheduledTime.Hour, 3);
			Assert.AreEqual(scheduledTime.Minute, 15);
			Assert.AreEqual(scheduledTime.Second, 0);
		}
	}
}

using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;
using Should.Fluent;

namespace FluentScheduler.Tests.ScheduleTests
{
	[TestFixture]
	public class MonthsOnTheLastDayTests
	{
		[Test]
		public void Should_Add_Specified_Months_To_Next_Run_Date_And_Select_Last_Day_In_That_Month()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheLastDay();

			var input = new DateTime(2000, 1, 1);
			var scheduledTime = schedule.CalculateNextRun(input);
			var expectedTime = new DateTime(2000, 1, 31);
			scheduledTime.Should().Equal(expectedTime);
		}

		[Test]
		public void Should_Default_To_00_00_If_At_Is_Not_Defined()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheLastDay();

			var input = new DateTime(2000, 1, 1, 1, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);

			scheduledTime.Hour.Should().Equal(0);
			scheduledTime.Minute.Should().Equal(0);
			scheduledTime.Second.Should().Equal(0);
		}

		[Test]
		public void Should_Override_Existing_Minutes_And_Seconds_If_At_Method_Is_Called()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheLastDay().At(3, 15);

			var input = new DateTime(2000, 1, 1, 1, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);
			var expectedTime = new DateTime(2000, 1, 31);
			expectedTime.Should().Equal(scheduledTime.Date);

			scheduledTime.Hour.Should().Equal(3);
			scheduledTime.Minute.Should().Equal(15);
			scheduledTime.Second.Should().Equal(0);
		}

		public void Should_Pick_Next_Date_If_Now_Is_After_At_Time()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheLastDay().At(3, 15);

			var input = new DateTime(2000, 1, 31, 3, 15, 0).AddMilliseconds(1);
			var scheduledTime = schedule.CalculateNextRun(input);
			scheduledTime.Date.Should().Equal(new DateTime(2000, 3, 31));

			scheduledTime.Hour.Should().Equal(3);
			scheduledTime.Minute.Should().Equal(15);
			scheduledTime.Second.Should().Equal(0);
		}

		[Test]
		public void Should_Pick_Today_If_Now_Is_Before_At_Time()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheLastDay().At(3, 15);

			var input = new DateTime(2000, 1, 31, 1, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);
			scheduledTime.Date.Should().Equal(input.Date);

			scheduledTime.Hour.Should().Equal(3);
			scheduledTime.Minute.Should().Equal(15);
			scheduledTime.Second.Should().Equal(0);
		}

		[Test]
		public void Should_Set_To_Next_Interval_If_Inputted_Time_Is_After_Run_Time_By_A_Millisecond()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheLastDay().At(3, 15);

			var input = new DateTime(2000, 1, 31, 3, 15, 0).AddMilliseconds(1);
			var scheduledTime = schedule.CalculateNextRun(input);
			scheduledTime.Date.Should().Equal(new DateTime(2000, 3, 31));

			scheduledTime.Hour.Should().Equal(3);
			scheduledTime.Minute.Should().Equal(15);
			scheduledTime.Second.Should().Equal(0);
		}

		[Test]
		public void Should_Pick_This_Month_If_Now_Is_Before_At_Time()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheLastDay().At(3, 15);

			var input = new DateTime(2000, 1, 31, 1, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);
			scheduledTime.Date.Should().Equal(new DateTime(2000, 1, 31));

			scheduledTime.Hour.Should().Equal(3);
			scheduledTime.Minute.Should().Equal(15);
			scheduledTime.Second.Should().Equal(0);
		}
	}
}

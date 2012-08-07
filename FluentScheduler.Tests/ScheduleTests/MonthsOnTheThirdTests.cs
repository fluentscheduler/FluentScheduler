using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;
using Should.Fluent;

namespace FluentScheduler.Tests.ScheduleTests
{
	[TestFixture]
	public class MonthsOnTheThirdTests
	{
		[Test]
		public void Should_Default_To_00_00_If_At_Is_Not_Defined()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheThird(DayOfWeek.Monday);

			var input = new DateTime(2000, 1, 31, 1, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);

			scheduledTime.Hour.Should().Equal(0);
			scheduledTime.Minute.Should().Equal(0);
			scheduledTime.Second.Should().Equal(0);
		}

		[Test]
		public void Should_Set_Specific_Hour_And_Minute_If_At_Method_Is_Called()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheThird(DayOfWeek.Monday).At(3, 15);

			var input = new DateTime(2000, 1, 31);
			var scheduledTime = schedule.CalculateNextRun(input);
			var expectedTime = new DateTime(2000, 3, 20);
			scheduledTime.Date.Should().Equal(expectedTime.Date);

			scheduledTime.Hour.Should().Equal(3);
			scheduledTime.Minute.Should().Equal(15);
			scheduledTime.Second.Should().Equal(0);
		}

		[Test]
		public void Should_Override_Existing_Minutes_And_Seconds_If_At_Method_Is_Called()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheThird(DayOfWeek.Monday).At(3, 15);

			var input = new DateTime(2000, 1, 31, 1, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);
			var expectedTime = new DateTime(2000, 3, 20);
			scheduledTime.Date.Should().Equal(expectedTime.Date);

			scheduledTime.Hour.Should().Equal(3);
			scheduledTime.Minute.Should().Equal(15);
			scheduledTime.Second.Should().Equal(0);
		}

		[Test]
		public void Should_Select_The_Date_If_The_Next_Runtime_Falls_On_The_Specified_Day()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheThird(DayOfWeek.Wednesday);

			var input = new DateTime(2000, 1, 31);
			var scheduledTime = schedule.CalculateNextRun(input);

			var expectedTime = new DateTime(2000, 3, 15);
			scheduledTime.Should().Equal(expectedTime);
		}

		[Test]
		public void Should_Ignore_The_Specified_Day()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheThird(DayOfWeek.Thursday);

			var input = new DateTime(2000, 1, 25);
			var scheduledTime = schedule.CalculateNextRun(input);

			var expectedTime = new DateTime(2000, 3, 16);
			scheduledTime.Should().Equal(expectedTime);
		}

		[Test]
		public void Should_Pick_The_Day_Of_Week_Specified()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheThird(DayOfWeek.Friday);

			var input = new DateTime(2000, 1, 31);
			var scheduledTime = schedule.CalculateNextRun(input);

			var expectedTime = new DateTime(2000, 3, 17);
			scheduledTime.Should().Equal(expectedTime);
		}

		[Test]
		public void Should_Pick_The_Next_Week_If_The_Day_Of_Week_Has_Passed()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Months().OnTheThird(DayOfWeek.Tuesday);

			var input = new DateTime(2000, 1, 31); 
			var scheduledTime = schedule.CalculateNextRun(input);

			var expectedTime = new DateTime(2000, 3, 21);
			scheduledTime.Should().Equal(expectedTime);
		}

		[Test]
		public void Should_Pick_The_Next_Week_If_The_Day_Of_Week_Has_Passed_For_New_Weeks()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(9).Months().OnTheThird(DayOfWeek.Saturday);

			var input = new DateTime(2000, 1, 31); 
			var scheduledTime = schedule.CalculateNextRun(input);

			var expectedTime = new DateTime(2000, 10, 21);
			scheduledTime.Should().Equal(expectedTime);
		}

		[Test]
		public void Should_Pick_The_Next_Week_If_The_Day_Of_Week_Has_Passed_For_End_Of_Week()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(3).Months().OnTheThird(DayOfWeek.Sunday);

			var input = new DateTime(2000, 1, 31); 
			var scheduledTime = schedule.CalculateNextRun(input);

			var expectedTime = new DateTime(2000, 4, 16);
			scheduledTime.Should().Equal(expectedTime);
		}
	}
}

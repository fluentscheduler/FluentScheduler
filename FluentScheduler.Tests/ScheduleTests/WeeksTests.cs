using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;
using Should.Fluent;

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
			scheduledTime.Should().Equal(expectedTime);
		}

		[Test]
		public void Should_Default_To_00_00_If_At_Is_Not_Defined()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Weeks();

			var input = new DateTime(2000, 1, 1, 1, 23, 25);
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
			schedule.ToRunEvery(2).Weeks().At(3, 15);

			var input = new DateTime(2000, 1, 1);
			var scheduledTime = schedule.CalculateNextRun(input);
			scheduledTime.Date.Should().Equal(input.Date);

			scheduledTime.Hour.Should().Equal(3);
			scheduledTime.Minute.Should().Equal(15);
			scheduledTime.Second.Should().Equal(0);
		}

		[Test]
		public void Should_Override_Existing_Minutes_And_Seconds_If_At_Method_Is_Called()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Weeks().At(3, 15);

			var input = new DateTime(2000, 1, 1, 5, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);
			scheduledTime.Date.Should().Equal(input.Date.AddDays(14));

			scheduledTime.Hour.Should().Equal(3);
			scheduledTime.Minute.Should().Equal(15);
			scheduledTime.Second.Should().Equal(0);
		}

		[Test]
		public void Should_Default_To_00_00_If_On_Is_Specified_And_At_Is_Not_Defined()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Saturday);

			var input = new DateTime(2000, 1, 1, 1, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);

			scheduledTime.Hour.Should().Equal(0);
			scheduledTime.Minute.Should().Equal(0);
			scheduledTime.Second.Should().Equal(0);
		}

		[Test]
		public void Should_Set_Specific_Hour_And_Minute_If_On_Is_Specified_And_At_Method_Is_Called()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Saturday).At(3, 15);

			var input = new DateTime(2000, 1, 1);
			var scheduledTime = schedule.CalculateNextRun(input);
			scheduledTime.Date.Should().Equal(input.Date.AddDays(14));

			scheduledTime.Hour.Should().Equal(3);
			scheduledTime.Minute.Should().Equal(15);
			scheduledTime.Second.Should().Equal(0);
		}

		[Test]
		public void Should_Override_Existing_Minutes_And_Seconds_If_On_Is_Specified_And_At_Method_Is_Called()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Saturday).At(3, 15);

			var input = new DateTime(2000, 1, 1, 1, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);
			scheduledTime.Date.Should().Equal(input.Date.AddDays(14));

			scheduledTime.Hour.Should().Equal(3);
			scheduledTime.Minute.Should().Equal(15);
			scheduledTime.Second.Should().Equal(0);
		}

		[Test]
		public void Should_Select_The_Date_If_The_Next_Runtime_Falls_On_The_Specified_Day()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Weeks().On(DayOfWeek.Saturday);

			var input = new DateTime(2000, 1, 1);
			var scheduledTime = schedule.CalculateNextRun(input);

			var expectedTime = new DateTime(2000, 1, 15);
			scheduledTime.Should().Equal(expectedTime);
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
			scheduledTime.Should().Equal(expectedTime);
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
			scheduledTime.Should().Equal(expectedTime);
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
			scheduledTime.Should().Equal(expectedTime);
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
			scheduledTime.Should().Equal(expectedTime);
		}

		[Test]
		public void Should_Schedule_Today_If_Input_Time_Is_Before_Run_Time()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Weeks().At(3, 15);

			var input = new DateTime(2000, 1, 1);
			var scheduledTime = schedule.CalculateNextRun(input);
			scheduledTime.Date.Should().Equal(input.Date);

			scheduledTime.Hour.Should().Equal(3);
			scheduledTime.Minute.Should().Equal(15);
			scheduledTime.Second.Should().Equal(0);
		}

		[Test]
		public void Should_Not_Schedule_Today_If_Input_Time_Is_After_Run_Time()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(2).Weeks().At(3, 15);

			var input = new DateTime(2000, 1, 1, 5, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);
			scheduledTime.Date.Should().Equal(input.Date.AddDays(14));

			scheduledTime.Hour.Should().Equal(3);
			scheduledTime.Minute.Should().Equal(15);
			scheduledTime.Second.Should().Equal(0);			
		}
	}
}

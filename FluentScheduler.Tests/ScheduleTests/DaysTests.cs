using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;
using Should.Fluent;

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
			scheduledTime.Should().Equal(input.Date.AddDays(1));
		}

		[Test]
		public void Should_Default_To_00_00_If_At_Is_Not_Defined()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(1).Days();

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
			schedule.ToRunEvery(1).Days().At(3, 15);

			var input = new DateTime(2000, 1, 1);
			var scheduledTime = schedule.CalculateNextRun(input);
			scheduledTime.Date.Should().Equal(input.Date);

			scheduledTime.Hour.Should().Equal(3);
			scheduledTime.Minute.Should().Equal(15);
			scheduledTime.Second.Should().Equal(0);
		}

		[Test]
		public void Should_Pick_Next_Date_If_Now_Is_After_At_Time()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(3).Days().At(3, 15);

			var input = new DateTime(2000, 1, 1, 3, 15, 0).AddMilliseconds(1);
			var scheduledTime = schedule.CalculateNextRun(input);
			scheduledTime.Date.Should().Equal(new DateTime(2000, 1, 4));

			scheduledTime.Hour.Should().Equal(3);
			scheduledTime.Minute.Should().Equal(15);
			scheduledTime.Second.Should().Equal(0);
		}

		[Test]
		public void Should_Pick_Today_If_Now_Is_Before_At_Time()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(3).Days().At(3, 15);

			var input = new DateTime(2000, 1, 1, 1, 23, 25);
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
			schedule.ToRunEvery(1).Days().At(3, 15);

			var input = new DateTime(2000, 1, 1, 3, 15, 0).AddMilliseconds(1);
			var scheduledTime = schedule.CalculateNextRun(input);
			scheduledTime.Date.Should().Equal(new DateTime(2000, 1, 2));

			scheduledTime.Hour.Should().Equal(3);
			scheduledTime.Minute.Should().Equal(15);
			scheduledTime.Second.Should().Equal(0);
		}

		[Test]
		public void Should_Override_Existing_Minutes_And_Seconds_If_At_Method_Is_Called()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(1).Days().At(3, 15);

			var input = new DateTime(2000, 1, 1, 1, 23, 25);
			var scheduledTime = schedule.CalculateNextRun(input);
			scheduledTime.Date.Should().Equal(input.Date);

			scheduledTime.Hour.Should().Equal(3);
			scheduledTime.Minute.Should().Equal(15);
			scheduledTime.Second.Should().Equal(0);
		}

        [Test]
        public void SHould_Throw_Exception_When_Using_Randomized_Day_For_Start_Time()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            Assert.Throws(typeof(InvalidOperationException), delegate { schedule.ToRunAboutEvery(3).Days(); });
        }
	}
}

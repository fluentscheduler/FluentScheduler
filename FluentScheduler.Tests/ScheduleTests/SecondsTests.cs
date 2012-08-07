using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;
using Should.Fluent;

namespace FluentScheduler.Tests.ScheduleTests
{
	[TestFixture]
	public class SecondsTests
	{
		[Test]
		public void Should_Add_Specified_Seconds_To_Next_Run_Date()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(30).Seconds();

			var input = new DateTime(2000, 1, 1);
			var scheduledTime = schedule.CalculateNextRun(input);
			scheduledTime.Date.Should().Equal(input.Date);

			scheduledTime.Hour.Should().Equal(input.Hour);
			scheduledTime.Minute.Should().Equal(input.Minute);
			scheduledTime.Second.Should().Equal(30);
		}
	}
}

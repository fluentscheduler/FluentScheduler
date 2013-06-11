using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;
using Should.Fluent;

namespace FluentScheduler.Tests.ScheduleTests
{
	[TestFixture]
	public class MinutesTests
	{
		[Test]
		public void Should_Add_Specified_Minutes_To_Next_Run_Date()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunEvery(30).Minutes();

			var input = new DateTime(2000, 1, 1);
			var scheduledTime = schedule.CalculateNextRun(input);
			scheduledTime.Date.Should().Equal(input.Date);

			scheduledTime.Hour.Should().Equal(input.Hour);
			scheduledTime.Minute.Should().Equal(30);
			scheduledTime.Second.Should().Equal(input.Second);
		}

        [Test]
        public void Should_Add_Specified_Minutes_To_Next_Run_Date_Random()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunAboutEvery(30).Minutes();

            var input = new DateTime(2000,1,1);
            var scheduledTime = schedule.CalculateNextRun(input);
            scheduledTime.Date.Should().Equal(input.Date);

            scheduledTime.Hour.Should().Equal(input.Hour);
            scheduledTime.Minute.Should().Be.InRange(27, 30);
        }
	}
}

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

        [Test]
        public void Should_Add_Specified_Seconds_To_Next_Run_Date_Random()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunAboutEvery(300).Seconds();

            var input = new DateTime(2000, 1, 1);
            var scheduledTime = schedule.CalculateNextRun(input);
            scheduledTime.Date.Should().Equal(input.Date);

            scheduledTime.Hour.Should().Equal(input.Hour);
            //we can only really test the minutes here because the seconds
            // could end up at any value due to the randomness.
            scheduledTime.Minute.Should().Be.InRange(4, 5);
        }
	}
}

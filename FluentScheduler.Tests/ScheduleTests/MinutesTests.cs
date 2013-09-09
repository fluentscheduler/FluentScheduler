using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;

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
			Assert.AreEqual(scheduledTime.Date, input.Date);

			Assert.AreEqual(scheduledTime.Hour, input.Hour);
			Assert.AreEqual(scheduledTime.Minute, 30);
			Assert.AreEqual(scheduledTime.Second, input.Second);
		}
	}
}

using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;

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
			Assert.AreEqual(scheduledTime.Date, input.Date);
			Assert.AreEqual(scheduledTime.Hour, input.Hour);
			Assert.AreEqual(scheduledTime.Minute, input.Minute);
			Assert.AreEqual(scheduledTime.Second, 30);
		}
	}
}

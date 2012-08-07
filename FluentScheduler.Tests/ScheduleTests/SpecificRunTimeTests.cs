using System;
using System.Linq;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;
using Should.Fluent;

namespace FluentScheduler.Tests.ScheduleTests
{
	[TestFixture]
	public class SpecificRunTimeTests
	{
		[Test]
		public void Should_Add_Chained_Tasks_To_AdditionalSchedules_Property()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunNow().AndEvery(1).Months();

			schedule.AdditionalSchedules.Should().Count.Exactly(1);
		}

		[Test]
		public void Should_Set_Chained_Task_Schedule_As_Expected()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunNow().AndEvery(2).Months();

			var input = new DateTime(2000, 1, 1);
			var scheduledTime = schedule.AdditionalSchedules.ElementAt(0).CalculateNextRun(input);
			var expectedTime = new DateTime(2000, 3, 1);
			scheduledTime.Should().Equal(expectedTime);
		}

		[Test]
		public void Should_Not_Alter_Original_Runtime_If_Chained_Task_Exists()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunNow().AndEvery(1).Months();

			schedule.CalculateNextRun.Should().Be.Null();
		}
	}
}

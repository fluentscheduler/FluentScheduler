using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;
using Should.Fluent;

namespace FluentScheduler.Tests.ScheduleTests
{
	[TestFixture]
	public class NonReentrantTests
	{
		[Test]
		public void Should_Be_True_By_Default()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunNow();

			schedule.Reentrant.Should().Be.True();
		}

		[Test]
		public void Should_Default_Reentrent_Parameter_For_Child_Schedules()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.ToRunNow().AndEvery(1).Minutes();

			schedule.Reentrant.Should().Be.True();
			foreach (var child in schedule.AdditionalSchedules)
			{
				child.Reentrant.Should().Be.True();
			}
		}

		[Test]
		public void Should_Set_Reentrent_Parameter_For_Child_Schedules()
		{
			var task = new Mock<ITask>();
			var schedule = new Schedule(task.Object);
			schedule.NonReentrant().ToRunNow().AndEvery(1).Minutes();

			schedule.Reentrant.Should().Be.False();
			foreach (var child in schedule.AdditionalSchedules)
			{
				child.Reentrant.Should().Be.False();
			}
		}
	}
}

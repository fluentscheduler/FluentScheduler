using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;
using Should.Fluent;

namespace FluentScheduler.Tests.ScheduleTests {
	[TestFixture]
	public class RemoveTests {
		[Test]
		public void ShouldRemoveNamedTask() {
			var task = new Mock<ITask>();
			var name = "ShouldRemoveTask";
			var schedule = new Schedule(task.Object).WithName(name);
			schedule.ToRunNow().AndEvery(1).Seconds();
			TaskManager.RemoveTask(name);

			var s = TaskManager.GetSchedule(name);
			s.Should().Be.Null();
		}
	}
}

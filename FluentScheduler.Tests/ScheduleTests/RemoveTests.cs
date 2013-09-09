using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;
using Should.Fluent;

namespace FluentScheduler.Tests.ScheduleTests
{
	[TestFixture]
	public class RemoveTests
	{
		[Test]
		public void Should_Remove_Named_Task()
		{
			var task = new Mock<ITask>();
			var name = "ShouldRemoveTask";
			var schedule = new Schedule(task.Object).WithName(name);
			schedule.ToRunNow().AndEvery(1).Seconds();
			TaskManager.RemoveTask(name);

			var taskFromManager = TaskManager.GetSchedule(name);
			taskFromManager.Should().Be.Null();
		}

		[Test]
		public void Should_Remove_LongRunning_Task_But_Keep_Running()
		{
			var name = "longrunning";
			var schedule = new Schedule(() => { Thread.Sleep(100); });
			schedule.WithName(name).ToRunNow().AndEvery(2).Seconds();
			schedule.Execute();

			TaskManager.RunningSchedules.Any(task => task.Name == name).Should().Be.True();
			TaskManager.RemoveTask(name);
			TaskManager.GetSchedule(name).Should().Be.Null();
			TaskManager.RunningSchedules.Any(task => task.Name == name).Should().Be.True();

			Thread.Sleep(2042); // wait until a second run would normally be executed
			TaskManager.RunningSchedules.Any(task => task.Name == name).Should().Be.False();
		}
	}
}

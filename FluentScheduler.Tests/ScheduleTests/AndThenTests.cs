using System;
using System.Linq;
using System.Threading;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;

namespace FluentScheduler.Tests.ScheduleTests
{
	[TestFixture]
	public class AndThenTests
	{
		[Test]
		public void Should_Be_Able_To_Schedule_Multiple_ITasks()
		{
			var task1 = new Mock<ITask>();
			var task2 = new Mock<ITask>();
			task1.Setup(m => m.Execute());
			task2.Setup(m => m.Execute());
			var schedule = new Schedule(task1.Object).AndThen(task2.Object);
			schedule.Execute();

			while (TaskManager.RunningSchedules.Any())
			{
				Thread.Sleep(1);
			}
			task1.Verify(m => m.Execute(), Times.Once());
			task2.Verify(m => m.Execute(), Times.Once());
		}

		[Test]
		public void Should_Be_Able_To_Schedule_Multiple_Simple_Methods()
		{
			var task1 = new Mock<ITask>();
			var task2 = new Mock<ITask>();
			task1.Setup(m => m.Execute());
			task2.Setup(m => m.Execute());
			var schedule = new Schedule(() => task1.Object.Execute()).AndThen(() => task2.Object.Execute());
			schedule.Execute();

			while (TaskManager.RunningSchedules.Any())
			{
				Thread.Sleep(1);
			}

			task1.Verify(m => m.Execute(), Times.Once());
			task2.Verify(m => m.Execute(), Times.Once());
		}

		[Test]
		public void Should_Execute_Tasks_In_Order()
		{
			var task1 = new Mock<ITask>();
			var task2 = new Mock<ITask>();
			var task1Runtime = DateTime.MinValue;
			var task2Runtime = DateTime.MinValue;
			task1.Setup(m => m.Execute()).Callback(() =>
				{
					task1Runtime = DateTime.Now;
					Thread.Sleep(1);
				});
			task2.Setup(m => m.Execute()).Callback(() => task2Runtime = DateTime.Now);
			var schedule = new Schedule(() => task1.Object.Execute()).AndThen(() => task2.Object.Execute());
			schedule.Execute();

			while (TaskManager.RunningSchedules.Any())
			{
				Thread.Sleep(1);
			}
			Assert.Less(task1Runtime.Ticks, task2Runtime.Ticks);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;

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
            Assert.IsNull(taskFromManager);
        }

        [Test]
        public void Should_Remove_LongRunning_Task_But_Keep_Running()
        {
            var name = "longrunning";
            var schedule = new Schedule(() => { Thread.Sleep(100); });
            schedule.WithName(name).ToRunNow().AndEvery(2).Seconds();
            schedule.Execute();

            Assert.IsTrue(TaskManager.RunningSchedules.Any(task => task.Name == name));
            TaskManager.RemoveTask(name);
            Assert.IsNull(TaskManager.GetSchedule(name));
            Assert.IsTrue(TaskManager.RunningSchedules.Any(task => task.Name == name));

            Thread.Sleep(2042); // wait until a second run would normally be executed
            Assert.IsFalse(TaskManager.RunningSchedules.Any(task => task.Name == name));
        }
    }
}

using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;
using Should.Fluent;

namespace FluentScheduler.Tests.ScheduleTests
{
    [TestFixture]
    public class AndThenTests
    {

        [SetUp]
        public void Setup()
        {
            TaskManager.RunInTestingMode();
        }

        [Test]
        public void Should_Be_Able_To_Schedule_Multiple_ITasks()
        {
            var task_1 = new Mock<ITask>();
            var task_2 = new Mock<ITask>();
            task_1.Setup(m => m.Execute());
            task_2.Setup(m => m.Execute());
            Schedule schedule = new Schedule(task_1.Object).AndThen(task_2.Object);
            schedule.Execute();

            task_1.Verify(m => m.Execute(), Times.Once());
            task_2.Verify(m => m.Execute(), Times.Once());
        }

        [Test]
        public void Should_Be_Able_To_Schedule_Multiple_Simple_Methods()
        {
            var task_1 = new Mock<ITask>();
            var task_2 = new Mock<ITask>();
            task_1.Setup(m => m.Execute());
            task_2.Setup(m => m.Execute());
            Schedule schedule = new Schedule(() => task_1.Object.Execute()).AndThen(() => task_2.Object.Execute());
            schedule.Execute();

            task_1.Verify(m => m.Execute(), Times.Once());
            task_2.Verify(m => m.Execute(), Times.Once());
        }
    }
}
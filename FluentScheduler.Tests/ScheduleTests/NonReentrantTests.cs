using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;

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

            Assert.IsTrue(schedule.Reentrant);
        }

        [Test]
        public void Should_Default_Reentrent_Parameter_For_Child_Schedules()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.ToRunNow().AndEvery(1).Minutes();

            Assert.IsTrue(schedule.Reentrant);
            foreach (var child in schedule.AdditionalSchedules)
            {
                Assert.IsTrue(child.Reentrant);
            }
        }

        [Test]
        public void Should_Set_Reentrent_Parameter_For_Child_Schedules()
        {
            var task = new Mock<ITask>();
            var schedule = new Schedule(task.Object);
            schedule.NonReentrant().ToRunNow().AndEvery(1).Minutes();

            Assert.IsFalse(schedule.Reentrant);
            foreach (var child in schedule.AdditionalSchedules)
            {
                Assert.IsFalse(child.Reentrant);
            }
        }
    }
}

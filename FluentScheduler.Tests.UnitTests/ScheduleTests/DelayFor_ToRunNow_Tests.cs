using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;

namespace FluentScheduler.Tests.UnitTests.ScheduleTests
{
    [TestFixture]
    public class DelayFor_ToRunOnceAt_Tests
    {
        [Test]
        public void Should_Delay_ToRunOnceAt_For_2_Seconds()
        {
            TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunOnceAt_For_2_Seconds").ToRunOnceAt(DateTime.Now).DelayFor(2).Seconds());
            DateTime expectedTime = DateTime.Now.AddSeconds(2);

            DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunOnceAt_For_2_Seconds").NextRun;

            Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
        }
        [Test]
        public void Should_Delay_ToRunOnceAt_For_2_Minutes()
        {
            TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunOnceAt_For_2_Minutes").ToRunOnceAt(DateTime.Now).DelayFor(2).Minutes());
            DateTime expectedTime = DateTime.Now.AddMinutes(2);

            DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunOnceAt_For_2_Minutes").NextRun;

            Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
        }
        [Test]
        public void Should_Delay_ToRunOnceAt_For_2_Hours()
        {
            TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunOnceAt_For_2_Hours").ToRunOnceAt(DateTime.Now).DelayFor(2).Hours());
            DateTime expectedTime = DateTime.Now.AddHours(2);

            DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunOnceAt_For_2_Hours").NextRun;

            Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
        }
        [Test]
        public void Should_Delay_ToRunOnceAt_For_2_Days()
        {
            TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunOnceAt_For_2_Days").ToRunOnceAt(DateTime.Now).DelayFor(2).Days());
            DateTime expectedTime = DateTime.Now.AddDays(2);

            DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunOnceAt_For_2_Days").NextRun;

            Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
        }
        [Test]
        public void Should_Delay_ToRunOnceAt_For_2_Weeks()
        {
            TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunOnceAt_For_2_Weeks").ToRunOnceAt(DateTime.Now).DelayFor(2).Weeks());
            DateTime expectedTime = DateTime.Now.AddDays(14);

            DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunOnceAt_For_2_Weeks").NextRun;

            Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
        }
        [Test]
        public void Should_Delay_ToRunOnceAt_For_2_Months()
        {
            TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunOnceAt_For_2_Months").ToRunOnceAt(DateTime.Now).DelayFor(2).Months());
            DateTime expectedTime = DateTime.Now.AddMonths(2);

            DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunOnceAt_For_2_Months").NextRun;

            Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
        }
        [Test]
        public void Should_Delay_ToRunOnceAt_For_2_Years()
        {
            TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunOnceAt_For_2_Years").ToRunOnceAt(DateTime.Now).DelayFor(2).Years());
            DateTime expectedTime = DateTime.Now.AddYears(2);

            DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunOnceAt_For_2_Years").NextRun;

            Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
        }

    }
}

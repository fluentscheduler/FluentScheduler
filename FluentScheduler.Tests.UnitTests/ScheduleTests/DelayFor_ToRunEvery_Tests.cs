using System;
using FluentScheduler.Model;
using Moq;
using NUnit.Framework;

namespace FluentScheduler.Tests.UnitTests.ScheduleTests
{
    [TestFixture]
    public class DelayFor_ToRunEvery_Tests
    {
        [Test]
        public void Should_Delay_ToRunEvery_For_2_Seconds()
        {
            TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunEvery_For_2_Seconds").ToRunEvery(10).Seconds().DelayFor(2).Seconds());
            DateTime expectedTime = DateTime.Now.AddSeconds(12);

            DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunEvery_For_2_Seconds").NextRunTime;

            Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
        }
        [Test]
        public void Should_Delay_ToRunEvery_For_2_Minutes()
        {
            TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunEvery_For_2_Minutes").ToRunEvery(10).Seconds().DelayFor(2).Minutes());
            DateTime expectedTime = DateTime.Now.AddSeconds(10).AddMinutes(2);

            DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunEvery_For_2_Minutes").NextRunTime;

            Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
        }
        [Test]
        public void Should_Delay_ToRunEvery_For_2_Hours()
        {
            TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunEvery_For_2_Hours").ToRunEvery(10).Seconds().DelayFor(2).Hours());
            DateTime expectedTime = DateTime.Now.AddSeconds(10).AddHours(2);

            DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunEvery_For_2_Hours").NextRunTime;

            Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
        }
        [Test]
        public void Should_Delay_ToRunEvery_For_2_Days()
        {
            TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunEvery_For_2_Days").ToRunEvery(10).Seconds().DelayFor(2).Days());
            DateTime expectedTime = DateTime.Now.AddSeconds(10).AddDays(2);

            DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunEvery_For_2_Days").NextRunTime;

            Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
        }
        [Test]
        public void Should_Delay_ToRunEvery_For_2_Weeks()
        {
            TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunEvery_For_2_Weeks").ToRunEvery(10).Seconds().DelayFor(2).Weeks());
            DateTime expectedTime = DateTime.Now.AddSeconds(10).AddDays(14);

            DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunEvery_For_2_Weeks").NextRunTime;

            Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
        }
        [Test]
        public void Should_Delay_ToRunEvery_For_2_Months()
        {
            TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunEvery_For_2_Months").ToRunEvery(10).Seconds().DelayFor(2).Months());
            DateTime expectedTime = DateTime.Now.AddSeconds(10).AddMonths(2);

            DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunEvery_For_2_Months").NextRunTime;

            Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
        }
        [Test]
        public void Should_Delay_ToRunEvery_For_2_Years()
        {
            TaskManager.AddTask(() => { }, x => x.WithName("Should_Delay_ToRunEvery_For_2_Years").ToRunEvery(10).Seconds().DelayFor(2).Years());
            DateTime expectedTime = DateTime.Now.AddSeconds(10).AddYears(2);

            DateTime actualTime = TaskManager.GetSchedule("Should_Delay_ToRunEvery_For_2_Years").NextRunTime;

            Assert.AreEqual(Math.Floor(expectedTime.TimeOfDay.TotalSeconds), Math.Floor(actualTime.TimeOfDay.TotalSeconds));
        }

    }
}

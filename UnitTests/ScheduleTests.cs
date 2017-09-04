namespace FluentScheduler.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading;

    [TestClass]
    public class ScheduleTests
    {
        [TestMethod]
        public void SimpleSchedule()
        {
            var expected = 2;
            var actual = 0;

            var schedule = new Schedule(() => ++actual, run => run.Every(1).Seconds());

            schedule.Start();

            Thread.Sleep(2100);
            Assert.AreEqual(expected, actual);
        }
    }
}

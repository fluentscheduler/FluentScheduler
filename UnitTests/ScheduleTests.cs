namespace FluentScheduler.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading;

    [TestClass]
    public class ScheduleTests
    {
        [TestMethod]
        public void Start()
        {
            // Arrange
            var calls = 0;
            var schedule = new Schedule(() => ++calls, run => run.Now().AndEvery(1).Seconds());

            // Act
            var returned = schedule.Start();
            var running = schedule.Running;

            // Assert
            Assert.AreEqual(1, calls);
            Assert.AreEqual(true, returned);
            Assert.AreEqual(true, running);

            // Act
            returned = schedule.Start();
            running = schedule.Running;

            // Assert
            Assert.AreEqual(false, returned);
            Assert.AreEqual(true, running);

            // Act
            Thread.Sleep(1000);

            // Assert
            Assert.AreEqual(2, calls);
        }

        [TestMethod]
        public void Stop()
        {
            // Arrange
            var calls = 0;
            var schedule = new Schedule(() => ++calls, run => run.Now().AndEvery(1).Seconds());

            // Act
            var returned = schedule.Start();
            var running = schedule.Running;

            Thread.Sleep(200);

            // Assert
            Assert.AreEqual(1, calls);
            Assert.AreEqual(true, returned);
            Assert.AreEqual(true, running);

            // Act
            returned = schedule.StopAndBlock();
            running = schedule.Running;

            // Assert
            Assert.AreEqual(1, calls);
            Assert.AreEqual(true, returned);
            Assert.AreEqual(false, running);
        }
    }
}

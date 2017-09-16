namespace FluentScheduler.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
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

            Thread.Sleep(100);

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

            Thread.Sleep(100);

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

        [TestMethod]
        public void Events()
        {
            // Arrange
            var startedCalls = 0;
            var endedCalls = 0;
            var schedule = new Schedule(() => { }, run => run.Now().AndEvery(1).Seconds());

            schedule.JobStarted += (sender, e) => ++startedCalls;
            schedule.JobEnded += (sender, e) => ++endedCalls;

            // Act
            schedule.Start();

            Thread.Sleep(100);

            // Assert
            Assert.AreEqual(1, startedCalls);
            Assert.AreEqual(1, endedCalls);

            // Act
            Thread.Sleep(1000);

            // Assert
            Assert.AreEqual(2, startedCalls);
            Assert.AreEqual(2, endedCalls);
        }

        [TestMethod]
        public void Exception()
        {
            // Arrange
            var schedule = new Schedule(() => throw new Exception("Some exception."), run => run.Now());

            Exception exception = null;
            schedule.JobEnded += (sender, e) => exception = e.Exception;

            // Act
            schedule.Start();

            Thread.Sleep(100);

            // Assert
            Assert.AreEqual("Some exception.", exception.Message);
        }
    }
}

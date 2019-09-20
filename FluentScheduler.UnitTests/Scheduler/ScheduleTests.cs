namespace FluentScheduler.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

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
            schedule.Start();
            Thread.Sleep(100);

            // Assert
            Assert.AreEqual(1, calls);
            Assert.IsTrue(schedule.Running);

            // Act
            Thread.Sleep(1000);

            // Assert
            Assert.AreEqual(2, calls);
        }

        [TestMethod]
        public void StartCron()
        {
            // Arrange
            var now = DateTime.Now.AddMinutes(1);
            var schedule = new Schedule(() => { }, "* * * * *");

            // Act
            schedule.Start();
            Thread.Sleep(100);

            // Assert
            Assert.AreEqual(now.Hour, schedule.NextRun.Value.Hour);
            Assert.AreEqual(now.Minute, schedule.NextRun.Value.Minute);
        }

        [TestMethod]
        public void StartAsync()
        {
            // Arrange
            var calls = 0;

            #pragma warning disable 1998
            var schedule = new Schedule(async () => ++calls, run => run.Now().AndEvery(1).Seconds());
            #pragma warning restore 1998

            // Act
            schedule.Start();
            Thread.Sleep(100);

            // Assert
            Assert.AreEqual(1, calls);
            Assert.IsTrue(schedule.Running);

            // Act
            Thread.Sleep(1000);

            // Assert
            Assert.AreEqual(2, calls);
        }

        [TestMethod]
        public void StartCronASync()
        {
            // Arrange
            var now = DateTime.Now.AddMinutes(1);
            #pragma warning disable 1998
            var schedule = new Schedule(async () => { }, "* * * * *");
            #pragma warning restore 1998

            // Act
            schedule.Start();
            Thread.Sleep(100);

            // Assert
            Assert.AreEqual(now.Hour, schedule.NextRun.Value.Hour);
            Assert.AreEqual(now.Minute, schedule.NextRun.Value.Minute);
        }

        [TestMethod]
        public void Stop()
        {
            // Arrange
            var schedule = new Schedule(() => { }, run => run.Now().AndEvery(1).Seconds());

            // Act
            schedule.Start();

            Thread.Sleep(100);

            schedule.Stop();

            // Assert
            Assert.IsFalse(schedule.Running);
        }

        [TestMethod]
        public void StopAndBlock()
        {
            // Arrange
            var calls = 0;
            var schedule = new Schedule(() => ++calls, run => run.Now().AndEvery(1).Seconds());

            // Act
            schedule.Start();
            Thread.Sleep(100);

            // Assert
            Assert.AreEqual(1, calls);
            Assert.IsTrue(schedule.Running);

            // Act
            schedule.StopAndBlock();

            // Assert
            Assert.AreEqual(1, calls);
            Assert.IsFalse(schedule.Running);
        }

        [TestMethod]
        public void SetScheduling()
        {
            // Arrange
            var calls = 0;
            var expectedCalls = 2;
            var schedule = new Schedule(() => calls++, run => run.Now().AndEvery(1).Days());

            // Act
            schedule.Start();

            Thread.Sleep(100);

            schedule.StopAndBlock();
            schedule.SetScheduling(run => run.Now());
            schedule.Start();

            Thread.Sleep(100);

            // Assert
            Assert.AreEqual(expectedCalls, calls);
        }

        [TestMethod]
        public void ResetScheduling()
        {
            // Arrange
            var calls = 0;
            var expectedCalls = 2;
            var schedule = new Schedule(() => calls++, run => run.Now());

            // Act
            schedule.Start();

            Thread.Sleep(100);

            schedule.StopAndBlock();
            schedule.ResetScheduling();
            schedule.Start();

            Thread.Sleep(100);

            // Assert
            Assert.AreEqual(expectedCalls, calls);
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

        [TestMethod]
        public void UseUtc()
        {
            // Arrange
            var expectedNow = DateTime.UtcNow;
            var schedule = new Schedule(() => { }, run => run.Now());

            // Act
            schedule.UseUtc();
            var resultedNow = schedule.Internal.Calculator.Now();

            // Assert
            Assert.AreEqual(expectedNow.Hour, resultedNow.Hour);
            Assert.AreEqual(expectedNow.Minute, resultedNow.Minute);
        }

        [TestMethod]
        public void DoNotUseUtc()
        {
            // Arrange
            var expectedNow = DateTime.Now;
            var schedule = new Schedule(() => { }, run => run.Now());

            // Act
            var resultedNow = schedule.Internal.Calculator.Now();

            // Assert
            Assert.AreEqual(expectedNow.Hour, resultedNow.Hour);
            Assert.AreEqual(expectedNow.Minute, resultedNow.Minute);
        }

        [TestMethod]
        public void DoNotUseUtcAfterStart()
        {
            // Arrange
            var expectedNow = DateTime.Now;
            var schedule = new Schedule(() => { }, run => run.Now());

            // Act
            schedule.Start();

            // Assert
            Assert.ThrowsException<InvalidOperationException>(() => schedule.UseUtc());
        }


        [TestMethod]
        public void UseUtcBeforeStart()
        {
            // Arrange
            var expectedNow = DateTime.UtcNow;
            var schedule = new Schedule(() => { }, run => run.Now());

            // Act
            schedule.UseUtc();
            schedule.Start();

            var resultedNow = schedule.Internal.Calculator.Now();

            // Assert
            Assert.AreEqual(expectedNow.Hour, resultedNow.Hour);
            Assert.AreEqual(expectedNow.Minute, resultedNow.Minute);
        }

        [TestMethod]
        public void UseUtcAfterStop()
        {
            // Arrange
            var expectedNow = DateTime.UtcNow;
            var schedule = new Schedule(() => { }, run => run.Now());

            // Act
            schedule.Start();
            schedule.StopAndBlock();
            schedule.UseUtc();
            schedule.Start();

            var resultedNow = schedule.Internal.Calculator.Now();

            // Assert
            Assert.AreEqual(expectedNow.Hour, resultedNow.Hour);
            Assert.AreEqual(expectedNow.Minute, resultedNow.Minute);
        }

        [TestMethod]
        public void WaitForCancellation()
        {
            // Arrange
            var cancelled = false;
            var schedule = new Schedule(async (cancellationToken) =>
            {
                cancelled = cancellationToken.WaitHandle.WaitOne(1000);
            }, run => run.Now());

            // Act
            schedule.Start();
            Thread.Sleep(100);
            schedule.StopAndBlock();

            // Assert
            Assert.IsTrue(cancelled);
        }
    }
}
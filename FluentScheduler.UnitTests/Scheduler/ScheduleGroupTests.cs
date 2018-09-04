namespace FluentScheduler.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    [TestClass]
    public class ScheduleGroupTests
    {
        [TestMethod]
        public void Start()
        {
             // Arrange
            var scheduleCollection = new List<Schedule>
            {
                new Schedule(() => { } , run => run.Now().AndEvery(1).Seconds()),
                new Schedule(() => { } , run => run.Now().AndEvery(1).Seconds()),
            };

            // Act
            scheduleCollection.Start();

            // Assert
            Assert.IsTrue(scheduleCollection.AllRunning());
        }

         [TestMethod]
        public void AllRunning()
        {
            // Arrange
            var scheduleGroup = new List<Schedule>
            {
                new Schedule(() => { } , run => run.Now().AndEvery(1).Seconds()),
                new Schedule(() => { } , run => run.Now().AndEvery(1).Seconds()),
                new Schedule(() => { } , run => run.Now().AndEvery(1).Seconds()),
            };

            // Act
            scheduleGroup.Start();
            Thread.Sleep(100);

            // Assert
            Assert.IsTrue(scheduleGroup.AllRunning());

            // Act
            scheduleGroup.Stop();

            // Assert
            Assert.IsFalse(scheduleGroup.AllRunning());
        }

        [TestMethod]
        public void AnyRunning()
        {
            // Arrange
            var scheduleGroup = new List<Schedule>
            {
                new Schedule(() => { } , run => run.Now().AndEvery(1).Seconds()),
                new Schedule(() => { } , run => run.Now().AndEvery(1).Seconds()),
                new Schedule(() => { } , run => run.Now()),
            };

            // Act
            scheduleGroup.Start();

            // Assert
            Assert.IsTrue(scheduleGroup.AnyRunning());
        }

        [TestMethod]
        public void AllStopped()
        {
            // Arrange
            var scheduleGroup = new List<Schedule>
            {
                new Schedule(() => { } , run => run.Now().AndEvery(1).Seconds()),
                new Schedule(() => { } , run => run.Now().AndEvery(1).Seconds()),
                new Schedule(() => { } , run => run.Now().AndEvery(1).Seconds()),
            };

            // Act
            scheduleGroup.Start();
            
            // Assert
            Assert.IsFalse(scheduleGroup.AllStopped());

            // Act
            scheduleGroup.StopAndBlock();

            // Assert
            Assert.IsTrue(scheduleGroup.AllStopped());
        }

        [TestMethod]
        public void AnyStopped()
        {
            // Arrange
            var scheduleGroup = new List<Schedule>
            {
                new Schedule(() => { } , run => run.Now().AndEvery(1).Seconds()),
                new Schedule(() => { } , run => run.Now().AndEvery(1).Seconds()),
                new Schedule(() => { } , run => run.Now().AndEvery(1).Seconds()),
            };

            // Act
            scheduleGroup.Start();

            // Assert
            Assert.IsFalse(scheduleGroup.AnyStopped());

            // Act
            scheduleGroup[0].StopAndBlock();

            // Assert
            Assert.IsTrue(scheduleGroup.AnyStopped());
        }
       
        [TestMethod]
        public void SetScheduling()
        {
            // Arrange
            var now = DateTime.Now;

            var scheduleGroup = new List<Schedule>
            {
                new Schedule(() => { } , run => run.Now().AndEvery(1).Seconds()),
                new Schedule(() => { } , run => run.Now().AndEvery(1).Seconds()),
            };

            // Act
            scheduleGroup.SetScheduling(newRun => newRun.Every(3).Minutes());
            scheduleGroup.Start();

            scheduleGroup.StopAndBlock();

            // Assert
            Assert.AreEqual(now.AddMinutes(3).Minute, scheduleGroup[0].NextRun.Value.Minute);
        }

        [TestMethod]
        public void ResetScheduling()
        {
            // Arrange
            var now = DateTime.Now;

            var scheduleGroup = new List<Schedule>
            {
                new Schedule(() => { } , run => run.Now().AndEvery(1).Months()),
                new Schedule(() => { } , run => run.Now().AndEvery(1).Months())
            };

            // Act
            
            var stopped = scheduleGroup.AllStopped();

            Thread.Sleep(100);

            scheduleGroup.Stop();
            scheduleGroup.ResetScheduling();

            scheduleGroup.Start();
            var nextRun = scheduleGroup.NextRun();

            // Assert
            Assert.AreEqual(now.Day, nextRun.Value.Item2.Day);
        }

        [TestMethod]
        public void ListenJobStarted()
        {
            // Arrange
            var calls = 0;
            var expectedCalls = 2;

            var scheduleGroup = new List<Schedule>
            {
                new Schedule(() => { } , run => run.Now()),
                new Schedule(() => { } , run => run.Now()),
            };

            // Act
            scheduleGroup.ListenJobStarted((_, e) => calls++);

            scheduleGroup.Start();
            Thread.Sleep(100);
            scheduleGroup.StopAndBlock();

            // Assert
            Assert.AreEqual(expectedCalls, calls);
        }

        [TestMethod]
        public void UnlistenJobStarted()
        {
            // Arrange
            var calls = 0;
            var expectedCalls = 2;

            EventHandler<JobStartedEventArgs> jobStartedEvent = (_, e) => calls++; 

            var scheduleGroup = new List<Schedule>
            {
                new Schedule(() => { } , run => run.Now()),
                new Schedule(() => { } , run => run.Now()),
            };

            // Act
            scheduleGroup.ListenJobStarted(jobStartedEvent);

            scheduleGroup.Start();
            Thread.Sleep(100);
            scheduleGroup.StopAndBlock();

            scheduleGroup.ResetScheduling();
            scheduleGroup.UnlistenJobStarted(jobStartedEvent);

            scheduleGroup.Start();
            Thread.Sleep(100);
            scheduleGroup.StopAndBlock();

            // Assert
            Assert.AreEqual(expectedCalls, calls);
        }

        [TestMethod]
        public void ListenJobEnded()
        {
            // Arrange
            var calls = 0;
            var expectedCalls = 2;

            var scheduleGroup = new List<Schedule>
            {
                new Schedule(() => { } , run => run.Now()),
                new Schedule(() => { } , run => run.Now()),
            };

            // Act
            scheduleGroup.ListenJobEnded((_, e) => calls++);

            scheduleGroup.Start();
            Thread.Sleep(100);
            scheduleGroup.StopAndBlock();

            // Assert
            Assert.AreEqual(expectedCalls, calls);
        }

        [TestMethod]
        public void UnlistenJobEnded()
        {
            // Arrange
            var calls = 0;
            var expectedCalls = 2;

            EventHandler<JobEndedEventArgs> jobEndedEvent = (_, e) => calls++; 

            var scheduleGroup = new List<Schedule>
            {
                new Schedule(() => { } , run => run.Now()),
                new Schedule(() => { } , run => run.Now()),
            };

            // Act
            scheduleGroup.ListenJobEnded(jobEndedEvent);

            scheduleGroup.Start();
            Thread.Sleep(100);
            scheduleGroup.StopAndBlock();

            scheduleGroup.ResetScheduling();
            scheduleGroup.UnlistenJobEnded(jobEndedEvent);

            scheduleGroup.Start();
            Thread.Sleep(100);
            scheduleGroup.StopAndBlock();

            // Assert
            Assert.AreEqual(expectedCalls, calls);
        }

        [TestMethod]
        public void NextRun()
        {
            // Arrange
            var scheduleGroup = new List<Schedule>
            {
                new Schedule(() => { } , run => run.Every(10).Minutes()),
                new Schedule(() => { } , run => run.Every(20).Minutes()),
            };

            // Act
            scheduleGroup.Start();
            var expectedNextRun = DateTime.Now;

            Thread.Sleep(100);

            var nextRun = scheduleGroup.NextRun();

            // Assert
            Assert.AreEqual(expectedNextRun.AddMinutes(10).Minute, nextRun.Value.Item2.Minute);
        }
    }
}
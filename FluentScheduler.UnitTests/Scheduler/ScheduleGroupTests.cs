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
            ScheduleGroup.Start(scheduleCollection);

            // Assert
            Assert.IsTrue(scheduleCollection.AllRunning());

            ScheduleGroup.Stop(scheduleCollection);
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
            var scheduleCollection = new List<Schedule>
            {
                new Schedule(() => { } , run => run.Now().AndEvery(1).Seconds()),
                new Schedule(() => { } , run => run.Now().AndEvery(1).Seconds()),
                new Schedule(() => { } , run => run.Now()),
            };

            // Act
            ScheduleGroup.Start(scheduleCollection);

            // Assert
            Assert.IsTrue(scheduleCollection.AnyRunning());
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
            // scheduleGroup.Stop();
            
            // Change this assert to IsTrue and uncomment last line to simulate this cute bug 
            // Assert
            Assert.IsFalse(scheduleGroup.AllStopped());

            // Act
            scheduleGroup.Stop();

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
            //scheduleGroup[0].Stop();

            // Change this assert to IsTrue and uncomment last line to simulate this cute bug 
            // Assert
            Assert.IsFalse(scheduleGroup.AnyStopped());

            // Act
            scheduleGroup[0].Stop();

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

            // Assert
            Assert.AreEqual(now.AddMinutes(3).Minute, scheduleGroup[0].NextRun.Value.Minute);
        }
    }
}
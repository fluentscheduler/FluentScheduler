namespace FluentScheduler.UnitTests
{
    using System.Collections.Generic;
    using System.Threading;
    using System;
    using Xunit;
    using static Xunit.Assert;

    public class ScheduleGroupTests
    {
        [Fact]
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
            True(scheduleCollection.AllRunning());
        }

         [Fact]
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
            True(scheduleGroup.AllRunning());

            // Act
            scheduleGroup.Stop();

            // Assert
            False(scheduleGroup.AllRunning());
        }

        [Fact]
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
            True(scheduleGroup.AnyRunning());
        }

        [Fact]
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
            False(scheduleGroup.AllStopped());

            // Act
            scheduleGroup.StopAndBlock();

            // Assert
            True(scheduleGroup.AllStopped());
        }

        [Fact]
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
            False(scheduleGroup.AnyStopped());

            // Act
            scheduleGroup[0].StopAndBlock();

            // Assert
            True(scheduleGroup.AnyStopped());
        }
       
        [Fact]
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
            Equal(now.AddMinutes(3).Minute, scheduleGroup[0].NextRun.Value.Minute);
        }

        [Fact]
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
            Equal(now.Day, nextRun.Value.Item2.Day);
        }

        [Fact]
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
            Equal(expectedCalls, calls);
        }

        [Fact]
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
            Equal(expectedCalls, calls);
        }

        [Fact]
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
            Equal(expectedCalls, calls);
        }

        [Fact]
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
            Equal(expectedCalls, calls);
        }

        [Fact]
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
            Equal(expectedNextRun.AddMinutes(10).Minute, nextRun.Value.Item2.Minute);
        }
    }
}
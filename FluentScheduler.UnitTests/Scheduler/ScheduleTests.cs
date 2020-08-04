namespace FluentScheduler.UnitTests
{
    using System.Threading.Tasks;
    using System.Threading;
    using System;
    using Xunit;
    using static Xunit.Assert;
    
    public class ScheduleTests
    {
        [Fact]
        public void Start()
        {
            // Arrange
            var calls = 0;
            var schedule = new Schedule(() => ++calls, run => run.Now().AndEvery(1).Seconds());

            // Act
            schedule.Start();
            Thread.Sleep(100);

            // Assert
            Equal(1, calls);
            True(schedule.Running);

            // Act
            Thread.Sleep(1000);

            // Assert
            Equal(2, calls);
        }

        [Fact]
        public void StartCron()
        {
            // Arrange
            var now = DateTime.Now.AddMinutes(1);
            var schedule = new Schedule(() => { }, "* * * * *");

            // Act
            schedule.Start();
            Thread.Sleep(100);

            // Assert
            Equal(now.Hour, schedule.NextRun.Value.Hour);
            Equal(now.Minute, schedule.NextRun.Value.Minute);
        }

        [Fact]
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
            Equal(1, calls);
            True(schedule.Running);

            // Act
            Thread.Sleep(1000);

            // Assert
            Equal(2, calls);
        }

        [Fact]
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
            Equal(now.Hour, schedule.NextRun.Value.Hour);
            Equal(now.Minute, schedule.NextRun.Value.Minute);
        }

        [Fact]
        public void Stop()
        {
            // Arrange
            var schedule = new Schedule(() => { }, run => run.Now().AndEvery(1).Seconds());

            // Act
            schedule.Start();

            Thread.Sleep(100);

            schedule.Stop();

            // Assert
            False(schedule.Running);
        }

        [Fact]
        public void StopAndBlock()
        {
            // Arrange
            var calls = 0;
            var schedule = new Schedule(() => ++calls, run => run.Now().AndEvery(1).Seconds());

            // Act
            schedule.Start();
            Thread.Sleep(100);

            // Assert
            Equal(1, calls);
            True(schedule.Running);

            // Act
            schedule.StopAndBlock();

            // Assert
            Equal(1, calls);
            False(schedule.Running);
        }

        [Fact]
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
            Equal(expectedCalls, calls);
        }

        [Fact]
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
            Equal(expectedCalls, calls);
        }

        [Fact]
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
            Equal(1, startedCalls);
            Equal(1, endedCalls);

            // Act
            Thread.Sleep(1000);

            // Assert
            Equal(2, startedCalls);
            Equal(2, endedCalls);
        }

        [Fact]
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
            Equal("Some exception.", exception.Message);
        }

        [Fact]
        public void UseUtc()
        {
            // Arrange
            var expectedNow = DateTime.UtcNow;
            var schedule = new Schedule(() => { }, run => run.Now());

            // Act
            schedule.UseUtc();
            var resultedNow = schedule.Internal.Calculator.Now();

            // Assert
            Equal(expectedNow.Hour, resultedNow.Hour);
            Equal(expectedNow.Minute, resultedNow.Minute);
        }

        [Fact]
        public void DoNotUseUtc()
        {
            // Arrange
            var expectedNow = DateTime.Now;
            var schedule = new Schedule(() => { }, run => run.Now());

            // Act
            var resultedNow = schedule.Internal.Calculator.Now();

            // Assert
            Equal(expectedNow.Hour, resultedNow.Hour);
            Equal(expectedNow.Minute, resultedNow.Minute);
        }

        [Fact]
        public void DoNotUseUtcAfterStart()
        {
            // Arrange
            var expectedNow = DateTime.Now;
            var schedule = new Schedule(() => { }, run => run.Now());

            // Act
            schedule.Start();

            // Assert
            Throws<InvalidOperationException>(() => schedule.UseUtc());
        }


        [Fact]
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
            Equal(expectedNow.Hour, resultedNow.Hour);
            Equal(expectedNow.Minute, resultedNow.Minute);
        }

        [Fact]
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
            Equal(expectedNow.Hour, resultedNow.Hour);
            Equal(expectedNow.Minute, resultedNow.Minute);
        }

        [Fact]
        public void WaitForCancellation()
        {
            // Arrange
            var cancelled = false;
            var schedule = new Schedule(async (token) =>
                await Task.Delay(1000, token).ContinueWith(_ => cancelled = token.IsCancellationRequested),
                run => run.Now()
            );

            // Act
            schedule.Start();
            Thread.Sleep(100);
            schedule.StopAndBlock();

            // Assert
            True(cancelled);
        }
    }
}
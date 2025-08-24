namespace FluentScheduler.UnitTests;

using System;
using System.Threading.Tasks;
using Xunit;
using static Xunit.Assert;

public class ScheduleTests
{
    [Fact]
    public async Task Start()
    {
        // Arrange
        var calls = 0;
        var schedule = new Schedule(() => ++calls, run => run.Now().AndEvery(1).Seconds());

        // Act
        schedule.Start();
        await Task.Delay(100);

        // Assert
        Equal(1, calls);
        True(schedule.Running);
        True(schedule.NextRun.HasValue);

        // Act
        await Task.Delay(1000);

        // Assert
        Equal(2, calls);
    }

    [Fact]
    public async Task StartCron()
    {
        // Arrange
        var now = DateTime.Now.AddMinutes(1);
        var schedule = new Schedule(() => { }, "* * * * *");

        // Act
        schedule.Start();
        await Task.Delay(100);

        // Assert
        Equal(now.Hour, schedule.NextRun.Value.Hour);
        Equal(now.Minute, schedule.NextRun.Value.Minute);
    }

    [Fact]
    public async Task StartAsync()
    {
        // Arrange
        var calls = 0;

        var schedule = new Schedule(
            async () =>
            {
                ++calls;
                await Task.Yield();
            },
            run => run.Now().AndEvery(1).Seconds()
        );

        // Act
        schedule.Start();
        await Task.Delay(100);

        // Assert
        Equal(1, calls);
        True(schedule.Running);
        True(schedule.NextRun.HasValue);

        // Act
        await Task.Delay(1000);

        // Assert
        Equal(2, calls);
    }

    [Fact]
    public async Task StartCronASync()
    {
        // Arrange
        var now = DateTime.Now.AddMinutes(1);

        var schedule = new Schedule(async () => await Task.Yield(), "* * * * *");

        // Act
        schedule.Start();
        await Task.Delay(100);

        // Assert
        Equal(now.Hour, schedule.NextRun.Value.Hour);
        Equal(now.Minute, schedule.NextRun.Value.Minute);
    }

    [Fact]
    public async Task Stop()
    {
        // Arrange
        var schedule = new Schedule(() => { }, run => run.Now().AndEvery(1).Seconds());

        // Act
        schedule.Start();

        await Task.Delay(100);

        schedule.Stop();

        // Assert
        False(schedule.Running);
        False(schedule.NextRun.HasValue);
    }

    [Fact]
    public async Task StopAndBlock()
    {
        // Arrange
        var calls = 0;
        var schedule = new Schedule(() => ++calls, run => run.Now().AndEvery(1).Seconds());

        // Act
        schedule.Start();
        await Task.Delay(100);

        // Assert
        Equal(1, calls);
        True(schedule.Running);

        // Act
        schedule.StopAndBlock();

        // Assert
        Equal(1, calls);
        False(schedule.Running);
        False(schedule.NextRun.HasValue);
    }

    [Fact]
    public async Task SetScheduling()
    {
        // Arrange
        var calls = 0;
        var expectedCalls = 2;
        var schedule = new Schedule(() => ++calls, run => run.Everyday());

        // Act
        schedule.Start();

        await Task.Delay(100);

        schedule.StopAndBlock();
        schedule.SetScheduling(run => run.Now());
        schedule.Start();

        await Task.Delay(100);

        // Assert
        Equal(expectedCalls, calls);
    }

    [Fact]
    public async Task ResetScheduling()
    {
        // Arrange
        var calls = 0;
        var expectedCalls = 2;
        var schedule = new Schedule(() => ++calls, run => run.Now());

        // Act
        schedule.Start();

        await Task.Delay(100);

        schedule.StopAndBlock();
        schedule.ResetScheduling();
        schedule.Start();

        await Task.Delay(100);

        // Assert
        Equal(expectedCalls, calls);
    }

    [Fact]
    public async Task Events()
    {
        // Arrange
        var startedCalls = 0;
        var endedCalls = 0;
        var schedule = new Schedule(() => { }, run => run.Now().AndEvery(1).Seconds());

        schedule.JobStarted += (sender, e) => ++startedCalls;
        schedule.JobEnded += (sender, e) => ++endedCalls;

        // Act
        schedule.Start();
        await Task.Delay(100);

        // Assert
        Equal(1, startedCalls);
        Equal(1, endedCalls);

        // Act
        await Task.Delay(1000);

        // Assert
        Equal(2, startedCalls);
        Equal(2, endedCalls);
    }

    [Fact]
    public async Task Exception()
    {
        // Arrange
        var schedule = new Schedule(() => throw new InvalidOperationException("Some exception."), run => run.Now());

        Exception exception = null;
        schedule.JobEnded += (sender, e) => exception = e.Exception;

        // Act
        schedule.Start();
        await Task.Delay(100);

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
        var resultedNow = schedule.Internal._calculator.Now();

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
        var resultedNow = schedule.Internal._calculator.Now();

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
        Throws<InvalidOperationException>(schedule.UseUtc);
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

        var resultedNow = schedule.Internal._calculator.Now();

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

        var resultedNow = schedule.Internal._calculator.Now();

        // Assert
        Equal(expectedNow.Hour, resultedNow.Hour);
        Equal(expectedNow.Minute, resultedNow.Minute);
    }

    [Fact]
    public async Task WaitForCancellation()
    {
        // Arrange
        var cancelled = false;
        var schedule = new Schedule(async (token) =>
            await Task.Delay(1000, token).ContinueWith(_ => cancelled = token.IsCancellationRequested).ConfigureAwait(false),
            run => run.Now()
        );

        // Act
        schedule.Start();
        await Task.Delay(100);
        schedule.StopAndBlock();

        // Assert
        True(cancelled);
    }
}
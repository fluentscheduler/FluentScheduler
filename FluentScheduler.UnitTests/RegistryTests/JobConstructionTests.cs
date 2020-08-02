namespace FluentScheduler.UnitTests.RegistryTests
{
    using FluentScheduler.UnitTests.RegistryTests.Mocks;
    using Xunit;
    using System.Threading;

    public class JobConstructionTests
    {
        [Fact]
        public void Should_Call_Ctor()
        {
            JobManager.AddJob<CtorJob>(s => s.ToRunNow());
            Thread.Sleep(200);
            Assert.Equal(1, CtorJob.Calls);

            JobManager.AddJob<CtorJob>(s => s.ToRunNow());
            Thread.Sleep(200);
            Assert.Equal(2, CtorJob.Calls);
        }

        [Fact]
        public void Should_Call_Dispose()
        {
            JobManager.AddJob<DisposableJob>(s => s.ToRunNow());
            Thread.Sleep(200);
            Assert.True(DisposableJob.Disposed);
        }
    }
}

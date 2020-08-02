namespace FluentScheduler.UnitTests
{
    using Mocks;
    using Xunit;
    using static JobManager;
    using static System.Threading.Thread;
    using static Xunit.Assert;

    public class JobConstructionTests
    {
        [Fact]
        public void Should_Call_Ctor()
        {
            AddJob<CtorJob>(s => s.ToRunNow());
            Sleep(200);
            Equal(1, CtorJob.Calls);

            AddJob<CtorJob>(s => s.ToRunNow());
            Sleep(200);
            Equal(2, CtorJob.Calls);
        }

        [Fact]
        public void Should_Call_Dispose()
        {
            AddJob<DisposableJob>(s => s.ToRunNow());
            Sleep(200);
            True(DisposableJob.Disposed);
        }
    }
}

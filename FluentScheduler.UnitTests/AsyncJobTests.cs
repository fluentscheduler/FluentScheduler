namespace FluentScheduler.UnitTests
{
    using Mocks;
    using Xunit;
    using static JobManager;
    using static System.Threading.Thread;
    using static Xunit.Assert;

    public class AsyncJobTests
    {
        [Fact]
        public void Should_Run_AsyncJob()
        {
            AddJob<AsyncJob>(s => s.ToRunNow());
            Sleep(200);
            Equal(1, AsyncJob.Calls);

            AddJob<AsyncJob>(s => s.ToRunNow());
            Sleep(200);
            Equal(2, AsyncJob.Calls);
        }
    }
}

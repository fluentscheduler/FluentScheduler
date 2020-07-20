namespace FluentScheduler.UnitTests.RegistryTests
{
    using FluentScheduler.UnitTests.RegistryTests.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading;

    [TestClass]
    public class JobConstructionTests
    {
        [TestMethod]
        public void Should_Call_Ctor()
        {
            JobManager.AddJob<CtorJob>(s => s.ToRunNow());
            Thread.Sleep(50);
            Assert.AreEqual(1, CtorJob.Calls);

            JobManager.AddJob<CtorJob>(s => s.ToRunNow());
            Thread.Sleep(50);
            Assert.AreEqual(2, CtorJob.Calls);

            JobManager.AddJob<CtorJob>(s => s.ToRunNow());
            Thread.Sleep(50);
            Assert.AreEqual(3, CtorJob.Calls);
        }

        [TestMethod]
        public void Should_Call_Dispose()
        {
            JobManager.AddJob<DisposableJob>(s => s.ToRunNow());
            Thread.Sleep(50);
            Assert.IsTrue(DisposableJob.Disposed);
        }
    }
}

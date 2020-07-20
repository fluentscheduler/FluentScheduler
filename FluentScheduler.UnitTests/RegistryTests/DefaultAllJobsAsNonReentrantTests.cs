namespace FluentScheduler.UnitTests.RegistryTests
{
    using FluentScheduler.UnitTests.RegistryTests.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DefaultAllJobsAsNonReentrantTests
    {
        [TestMethod]
        public void Should_Set_NonReentrant_For_Any_Previously_Configured_Job_In_The_Registry()
        {
            var registry = new RegistryWithPreviousJobsConfigured();
            foreach (var schedule in registry.Schedules)
                Assert.IsNotNull(schedule.Reentrant);
        }

        [TestMethod]
        public void Should_Set_Future_Configured_Jobs_In_The_Registry()
        {
            var registry = new RegistryWithFutureJobsConfigured();
            foreach (var schedule in registry.Schedules)
                Assert.IsNotNull(schedule.Reentrant);
        }
    }
}

namespace FluentScheduler.UnitTests.RegistryTests
{
    using FluentScheduler.UnitTests.RegistryTests.Mocks;
    using Xunit;

    public class DefaultAllJobsAsNonReentrantTests
    {
        [Fact]
        public void Should_Set_NonReentrant_For_Any_Previously_Configured_Job_In_The_Registry()
        {
            var registry = new RegistryWithPreviousJobsConfigured();
            foreach (var schedule in registry.Schedules)
                Assert.NotNull(schedule.Reentrant);
        }

        [Fact]
        public void Should_Set_Future_Configured_Jobs_In_The_Registry()
        {
            var registry = new RegistryWithFutureJobsConfigured();
            foreach (var schedule in registry.Schedules)
                Assert.NotNull(schedule.Reentrant);
        }
    }
}

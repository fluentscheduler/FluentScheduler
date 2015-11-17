using FluentScheduler.Tests.UnitTests.RegistryTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentScheduler.Tests.UnitTests.RegistryTests
{
    [TestClass]
    public class DefaultAllTasksAsNonReentrantTests
    {
        [TestMethod]
        public void Should_Set_NonReentrant_For_Any_Previously_Configured_Task_In_The_Registry()
        {
            var registry = new RegistryWithPreviousTasksConfigured();
            foreach (var schedule in registry.Schedules)
                Assert.IsFalse(schedule.Reentrant);
        }

        [TestMethod]
        public void Should_Set_Future_Configured_Tasks_In_The_Registry()
        {
            var registry = new RegistryWithFutureTasksConfigured();
            foreach (var schedule in registry.Schedules)
                Assert.IsFalse(schedule.Reentrant);
        }
    }
}

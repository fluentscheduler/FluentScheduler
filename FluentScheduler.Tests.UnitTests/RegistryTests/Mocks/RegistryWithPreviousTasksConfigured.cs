using System;

namespace FluentScheduler.Tests.UnitTests.RegistryTests.Mocks
{
    public class RegistryWithPreviousTasksConfigured : Registry
    {
        public RegistryWithPreviousTasksConfigured()
        {
            Schedule(() => Console.WriteLine("Hi"));
            Schedule<StronglyTypedTestTask>();
            DefaultAllTasksAsNonReentrant();
        }
    }
}

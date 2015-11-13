using System;

namespace FluentScheduler.Tests.UnitTests.RegistryTests.Mocks
{
    public class RegistryWithFutureTasksConfigured : Registry
    {
        public RegistryWithFutureTasksConfigured()
        {
            DefaultAllTasksAsNonReentrant();
            Schedule(() => Console.WriteLine("Hi"));
            Schedule<StronglyTypedTestTask>();
        }
    }
}

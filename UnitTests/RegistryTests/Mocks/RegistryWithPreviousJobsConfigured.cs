using System;

namespace FluentScheduler.Tests.UnitTests.RegistryTests.Mocks
{
    public class RegistryWithPreviousJobsConfigured : Registry
    {
        public RegistryWithPreviousJobsConfigured()
        {
            Schedule(() => Console.WriteLine("Hi"));
            Schedule<StronglyTypedTestJob>();
            NonReentrantAsDefault();
        }
    }
}

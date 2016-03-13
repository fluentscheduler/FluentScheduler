using System;

namespace FluentScheduler.Tests.UnitTests.RegistryTests.Mocks
{
    public class RegistryWithFutureJobsConfigured : Registry
    {
        public RegistryWithFutureJobsConfigured()
        {
            NonReentrantAsDefault();
            Schedule(() => Console.WriteLine("Hi"));
            Schedule<StronglyTypedTestJob>();
        }
    }
}

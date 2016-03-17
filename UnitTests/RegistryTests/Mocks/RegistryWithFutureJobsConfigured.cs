namespace FluentScheduler.Tests.UnitTests.RegistryTests.Mocks
{
    using System;

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

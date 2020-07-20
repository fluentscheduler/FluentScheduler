namespace FluentScheduler.UnitTests.RegistryTests.Mocks
{
    using System;

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

namespace FluentScheduler.Tests.UnitTests.RegistryTests.Mocks
{
    using System;

    public abstract class StronglyTypedTestJob : IJob
    {
        public void Execute()
        {
            Console.WriteLine("Hi");
        }
    }
}
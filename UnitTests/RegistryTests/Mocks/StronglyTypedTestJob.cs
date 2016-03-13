using System;

namespace FluentScheduler.Tests.UnitTests.RegistryTests.Mocks
{
    public abstract class StronglyTypedTestJob : IJob
    {
        public void Execute()
        {
            Console.WriteLine("Hi");
        }
    }
}

using System;

namespace FluentScheduler.Tests.UnitTests.RegistryTests.Mocks
{
    public abstract class StronglyTypedTestTask : ITask
    {
        public void Execute()
        {
            Console.WriteLine("Hi");
        }
    }
}

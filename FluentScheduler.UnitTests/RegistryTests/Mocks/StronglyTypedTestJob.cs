namespace FluentScheduler.UnitTests.RegistryTests.Mocks
{
    using System;

    public class StronglyTypedTestJob : IJob
    {
        public void Execute()
        {
            Console.WriteLine("Hi");
        }
    }
}
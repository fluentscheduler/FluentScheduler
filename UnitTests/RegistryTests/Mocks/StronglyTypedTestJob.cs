using System;

namespace Moong.FluentScheduler.Tests.UnitTests.RegistryTests.Mocks
{
  public class StronglyTypedTestJob : IJob
  {
    public void Execute()
    {
      Console.WriteLine("Hi");
    }
  }
}
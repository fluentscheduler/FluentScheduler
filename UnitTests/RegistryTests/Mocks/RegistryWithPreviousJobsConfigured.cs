using System;

namespace Moong.FluentScheduler.Tests.UnitTests.RegistryTests.Mocks
{
  public class RegistryWithPreviousJobsConfigured : Registry
  {
    public RegistryWithPreviousJobsConfigured()
    {
      this.Schedule(() => Console.WriteLine("Hi"));
      this.Schedule<StronglyTypedTestJob>();
      this.NonReentrantAsDefault();
    }
  }
}

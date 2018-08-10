using System;

namespace Moong.FluentScheduler.Tests.UnitTests.RegistryTests.Mocks
{
  public class RegistryWithFutureJobsConfigured : Registry
  {
    public RegistryWithFutureJobsConfigured()
    {
      this.NonReentrantAsDefault();
      this.Schedule(() => Console.WriteLine("Hi"));
      this.Schedule<StronglyTypedTestJob>();
    }
  }
}

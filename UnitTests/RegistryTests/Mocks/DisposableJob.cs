using System;

namespace Moong.FluentScheduler.Tests.UnitTests.RegistryTests.Mocks
{
  public class DisposableJob : IJob, IDisposable
  {
    public DisposableJob()
    {
      Disposed = false;
    }

    public static bool Disposed { get; private set; }

    public void Execute() { }

    public void Dispose()
    {
      Disposed = true;
    }
  }
}

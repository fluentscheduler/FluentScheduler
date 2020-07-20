namespace FluentScheduler.UnitTests.RegistryTests.Mocks
{
    using System;

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

namespace FluentScheduler.Tests.TestApplication
{
    using LLibrary;
    using System;

    class DisposableJob : IJob, IDisposable
    {
        public DisposableJob()
        {
            L.Register("[disposable]");
        }

        public void Execute()
        {
            L.Log("[disposable]", "Just executed.");
        }

        public void Dispose()
        {
            L.Log("[disposable]", "Disposed properly.");
        }
    }
}

namespace FluentScheduler.TestApplication
{
    using LLibrary;
    using System;

    public class DisposableJob : IJob, IDisposable
    {
        static DisposableJob()
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

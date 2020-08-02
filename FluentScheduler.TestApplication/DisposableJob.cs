namespace FluentScheduler.TestApplication
{
    using System;
    using static Serilog.Log;

    public class DisposableJob : IJob, IDisposable
    {
        public void Execute() => Logger.Information("Disposable: executing");

        public void Dispose() => Logger.Information("Disposable: disposed");
    }
}

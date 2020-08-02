namespace FluentScheduler.TestApplication
{
    using Serilog;
    using System;
    using System.Threading;
    using static Serilog.Log;
    using static Serilog.RollingInterval;

    public static class Program
    {
        static void Main(string[] args)
        {
            InitializeLogger();
            ListenForStart();
            ListenForEnd();
            ListenForException();
            Initialize();
            Sleep();
        }

        private static void InitializeLogger()
        {
            var outputTemplate = "[{Timestamp:HH:mm:ss}] {Message}{NewLine}";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: outputTemplate)
                .WriteTo.File("logs/.txt", outputTemplate: outputTemplate, rollingInterval: Day)
                .CreateLogger();
        }

        private static void ListenForStart()
        {
            JobManager.JobStart += (info) => Logger.Information($"{info.Name}: started");
        }

        private static void ListenForEnd()
        {
            JobManager.JobEnd += (info) => Logger.Information(
                info.Duration > TimeSpan.FromSeconds(1) ?
                $"{info.Name}: ended ({info.Duration})" :
                $"{info.Name}: ended"
            );
        }

        private static void ListenForException()
        {
            JobManager.JobException += info =>
                Logger.Information($"{info.Name}: {Environment.NewLine}{info.Exception}");
        }

        private static void Initialize()
        {
            JobManager.Initialize(new MyRegistry());
            JobManager.RemoveJob("Removed");

            JobManager.AddJob(
                () => Logger.Information("Late: added after the initialize"),
                s => s.WithName("Late").ToRunNow()
            );
        }

        private static void Sleep()
        {
            Thread.Sleep(Timeout.Infinite);
        }
    }
}

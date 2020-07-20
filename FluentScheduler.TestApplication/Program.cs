namespace FluentScheduler.TestApplication
{
    using LLibrary;
    using System;
    using System.Threading;

    public class Program
    {
        static void Main(string[] args)
        {
            ListenForStart();
            ListenForEnd();
            ListenForException();
            Initialize();
            Sleep();
        }

        private static void ListenForStart()
        {
            L.Register("[job start]", "{0} has started.");
            JobManager.JobStart += (info) => L.Log("[job start]", info.Name);
        }

        private static void ListenForEnd()
        {
            L.Register("[job end]", "{0} has ended{1}.");

            JobManager.JobEnd += (info) =>
                L.Log("[job end]", info.Name,
                    info.Duration > TimeSpan.FromSeconds(1) ? " with duration of " + info.Duration : string.Empty);
        }

        private static void ListenForException()
        {
            L.Register("[job exception]", "An error just happened:" + Environment.NewLine + "{0}");
            JobManager.JobException += (info) => L.Log("[job exception]", info.Exception);
        }

        private static void Initialize()
        {
            JobManager.Initialize(new MyRegistry());
            JobManager.RemoveJob("[removed]");

            L.Register("[late]");
            JobManager.AddJob(() => L.Log("[late]", "This was added after the initialize call."),
                s => s.WithName("[late]").ToRunNow());
        }

        private static void Sleep()
        {
            Thread.Sleep(Timeout.Infinite);
        }
    }
}

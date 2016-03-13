using LogThis;
using System;
using System.Threading;

namespace FluentScheduler.Tests.TestApplication
{
    class Program
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
            Log.Register("[job start]", "\"{0}\" has started.");
            JobManager.JobStart += (schedule, e) => Log.This("[job start]", schedule.Name);
        }

        private static void ListenForEnd()
        {
            Log.Register("[job end]", "\"{0}\" has ended{1}.");

            JobManager.JobEnd += (schedule, e) =>
                Log.This("[job end]", schedule.Name,
                    schedule.Duration > TimeSpan.FromSeconds(1) ?
                    " with duration of " + schedule.Duration : string.Empty);
        }

        private static void ListenForException()
        {
            Log.Register("[job exception]", "An error just happened:" + Environment.NewLine + "{0}");
            JobManager.JobException += (sender, e) => Log.This("[job exception]", e.ExceptionObject);
        }

        private static void Initialize()
        {
            JobManager.Initialize(new MyRegistry());
        }

        private static void Sleep()
        {
            Thread.Sleep(Timeout.Infinite);
        }
    }
}

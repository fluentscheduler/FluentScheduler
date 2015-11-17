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
            Log.Register("[task start]", "\"{0}\" has started.");
            TaskManager.TaskStart += (schedule, e) => Log.This("[task start]", schedule.Name);
        }

        private static void ListenForEnd()
        {
            Log.Register("[task end]", "\"{0}\" has ended{1}.");

            TaskManager.TaskEnd += (schedule, e) =>
                Log.This("[task end]", schedule.Name,
                    schedule.Duration > TimeSpan.FromSeconds(1) ?
                    " with duration of " + schedule.Duration : string.Empty);
        }

        private static void ListenForException()
        {
            Log.Register("[task exception]", "An error just happened:" + Environment.NewLine + "{0}");
            TaskManager.UnobservedTaskException += (sender, e) => Log.This("[task exception]", e.ExceptionObject);
        }

        private static void Initialize()
        {
            TaskManager.Initialize(new MyRegistry());
        }

        private static void Sleep()
        {
            Thread.Sleep(Timeout.Infinite);
        }
    }
}

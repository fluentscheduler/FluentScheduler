using LogThis;
using System;
using System.Threading;

namespace FluentScheduler.Tests.TestApplication
{
    class MyRegistry : Registry
    {
        public MyRegistry()
        {
            NonReentrantAsDefault();

            Welcome();

            NonReentrant();
            Reentrant();

            OnceIn();
            OnceAt();

            TenMinutes();
            Sleepy();
            Faulty();

            Hour();
            Day();
            Weekday();
            Week();
        }

        private void Welcome()
        {
            Schedule(() => Console.Write("3, "))
                .WithName("[welcome]")
                .AndThen(() => Console.Write("2, "))
                .AndThen(() => Console.Write("1, "))
                .AndThen(() => Console.WriteLine("Live!"))
                .AndThen(() => Console.WriteLine("{0}You can check what's happening in the log file at \"{1}\"",
                    Environment.NewLine, Log.Directory));
        }
        private void NonReentrant()
        {
            Log.Register("[non reentrant]");

            Schedule(() =>
            {
                Log.This("[non reentrant]", "Sleeping a minute...");
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }).NonReentrant().WithName("[non reentrant]").ToRunEvery(1).Seconds();
        }

        private void Reentrant()
        {
            Log.Register("[reentrant]");

            Schedule(() =>
            {
                Log.This("[reentrant]", "Sleeping a minute...");
                Thread.Sleep(TimeSpan.FromMinutes(3));
            }).WithName("[reentrant]").ToRunNow().AndEvery(1).Minutes();
        }

        private void OnceIn()
        {
            Log.Register("[once in]");

            Schedule(() =>
            {
                JobManager.RemoveJob("[reentrant]");
                JobManager.RemoveJob("[non reentrant]");
                Log.This("[once in]", "Disabled the reentrant and non reentrant jobs.");
            }).WithName("[once in]").ToRunOnceIn(3).Minutes();
        }

        private void OnceAt()
        {
            Log.Register("[once at]");

            Schedule(() => Log.This("[once at]", "It's almost midnight."))
                .WithName("[once at]").ToRunOnceAt(23, 50);
        }

        private void TenMinutes()
        {
            Log.Register("[ten minutes]");

            Schedule(() => Log.This("[ten minutes]", "Ten minutes has passed."))
                .WithName("[ten minutes]").ToRunEvery(10).Minutes();
        }

        private void Sleepy()
        {
            Log.Register("[sleepy]");

            Schedule(() =>
            {
                Log.This("[sleepy]", "Sleeping...");
                Thread.Sleep(new TimeSpan(0, 7, 30));
            }).WithName("[sleepy]").ToRunEvery(15).Minutes();
        }

        private void Faulty()
        {
            Log.Register("[faulty]");

            Schedule(() =>
            {
                Log.This("[faulty]", "I'm going to raise an exception!");
                throw new Exception("I warned you.");
            }).WithName("[faulty]").ToRunEvery(20).Minutes();
        }

        private void Hour()
        {
            Log.Register("[hour]");

            Schedule(() => Log.This("[hour]", "A hour has passed."))
                .WithName("[hour]").ToRunEvery(1).Hours();
        }

        private void Day()
        {
            Log.Register("[day]");

            Schedule(() => Log.This("[day]", "A day has passed."))
                .WithName("[day]").ToRunEvery(1).Days();
        }

        private void Weekday()
        {
            Log.Register("[weekday]");

            Schedule(() => Log.This("[weekday]", "A new weekday has started."))
                .WithName("[weekday]").ToRunEvery(1).Weekdays();
        }

        private void Week()
        {
            Log.Register("[week]");

            Schedule(() => Log.This("[week]", "A new week has started."))
                .WithName("[week]").ToRunEvery(1).Weeks();
        }
    }
}

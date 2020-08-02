namespace FluentScheduler.TestApplication
{
    using System;
    using static Serilog.Log;
    using static System.Threading.Thread;

    public class MyRegistry : Registry
    {
        public MyRegistry()
        {
            Welcome();

            NonReentrant();
            Reentrant();
            Disable();

            Faulty();
            Removed();
            Parameter();
            Disposable();

            FiveHundredMilliseconds();
            FiveMinutes();
            TenMinutes();
            Hour();
            Day();
            Weekday();
            Week();
        }

        private void Welcome()
        {
            Schedule(() => Logger.Information("CountDown: 3"))
                .WithName("Countdown")
                .AndThen(() => Logger.Information("Countdown: 2"))
                .AndThen(() => Logger.Information("Countdown: 1"))
                .AndThen(() => Logger.Information("Countdown: live!"));
        }

        private void NonReentrant()
        {
            Schedule(() =>
            {
                Logger.Information("NonReentrant: sleeping a minute");
                Sleep(TimeSpan.FromMinutes(1));
            }).NonReentrant().WithName("NonReentrant").ToRunEvery(1).Seconds();
        }

        private void Reentrant()
        {
            Schedule(() =>
            {
                Logger.Information("Reentrant: sleeping a minute");
                Sleep(TimeSpan.FromMinutes(3));
            }).WithName("Reentrant").ToRunNow().AndEvery(1).Minutes();
        }

        private void Disable()
        {
            Schedule(() =>
            {
                JobManager.RemoveJob("Reentrant");
                JobManager.RemoveJob("NonReentrant");

                Logger.Information("Disable: disabled Reentrant and NonReentrant jobs");
            }).WithName("Disable").ToRunOnceIn(200).Seconds();
        }

        private void Faulty()
        {
            Schedule(() =>
            {
                Logger.Information("Faulty: throwing an exception");
                throw new Exception("Exception from Faulty job");
            }).WithName("Faulty").ToRunNow().AndEvery(20).Minutes();
        }

        private void Removed()
        {
            Schedule(() =>
            {
                Logger.Information("SOMETHING WENT WRONG");
            }).WithName("Removed").ToRunOnceIn(2).Minutes();
        }

        private void Parameter()
        {
            Schedule(new ParameterJob { Parameter = "Foo" }).WithName("Parameter").ToRunOnceIn(10).Seconds();
        }

        private void Disposable()
        {
            Schedule<DisposableJob>().WithName("Disposable").ToRunOnceIn(10).Seconds();
        }

        private void FiveHundredMilliseconds()
        {
            Schedule(() => Logger.Information("HalfSecond: half a second has passed"))
                .WithName("HalfSecond").ToRunOnceIn(500).Milliseconds();
        }

        private void FiveMinutes()
        {
            Schedule(() => Logger.Information("FiveMinutes: five minutes has passed"))
                .WithName("FiveMinutes").ToRunOnceAt(DateTime.Now.AddMinutes(5)).AndEvery(5).Minutes();
        }

        private void TenMinutes()
        {
            Schedule(() => Logger.Information("TenMinutes: ten minutes has passed"))
                .WithName("TenMinutes").ToRunEvery(10).Minutes();
        }

        private void Hour()
        {
            Schedule(() => Logger.Information("Hour: a hour has passed"))
                .WithName("Hour").ToRunEvery(1).Hours();
        }

        private void Day()
        {
            Schedule(() => Logger.Information("Day: a day has passed"))
                .WithName("Day").ToRunEvery(1).Days();
        }

        private void Weekday()
        {
            Schedule(() => Logger.Information("Weekday: a new weekday has started"))
                .WithName("Weekday").ToRunEvery(1).Weekdays();
        }

        private void Week()
        {
            Schedule(() => Logger.Information("Week: a new week has started"))
                .WithName("Week").ToRunEvery(1).Weeks();
        }
    }
}

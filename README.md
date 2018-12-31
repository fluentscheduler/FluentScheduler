<p align="center">
    <a href="#fluentscheduler">
        <img alt="logo" src="Logo/logo-200x200.png">
    </a>
</p>

# FluentScheduler

[![][build-img]][build]
[![][nuget-img]][nuget]

Automated job scheduler with fluent interface.

* [Usage](#usage)
* [Using it with ASP.NET](#using-it-with-aspnet)
* [Stopping the scheduler](#stopping-the-scheduler)
* [Unexpected exceptions](#unexpected-exceptions)
* [Concurrent jobs](#concurrent-jobs)
* [Daylight Saving Time](#daylight-saving-time)
* [Milliseconds Accuracy](#milliseconds-accuracy)
* [Weekly jobs](#weekly-jobs)
* [Dependency Injection](#dependency-injection)
* [Contributing](#contributing)

[build]:     https://ci.appveyor.com/project/TallesL/fluentscheduler
[build-img]: https://ci.appveyor.com/api/projects/status/github/fluentscheduler/fluentscheduler?svg=true
[nuget]:     https://www.nuget.org/packages/FluentScheduler
[nuget-img]: https://badge.fury.io/nu/fluentscheduler.svg

## Usage

The job configuration is handled in a [Registry] class. A job is either an [Action] or a class that inherits [IJob]:

```cs
using FluentScheduler;

public class MyRegistry : Registry
{
    public MyRegistry()
    {
        // Schedule an IJob to run at an interval
        Schedule<MyJob>().ToRunNow().AndEvery(2).Seconds();

        // Schedule an IJob to run once, delayed by a specific time interval
        Schedule<MyJob>().ToRunOnceIn(5).Seconds();

        // Schedule a simple job to run at a specific time
        Schedule(() => Console.WriteLine("It's 9:15 PM now.")).ToRunEvery(1).Days().At(21, 15);

        // Schedule a more complex action to run immediately and on an monthly interval
        Schedule<MyComplexJob>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);

        // Schedule a job using a factory method and pass parameters to the constructor.
        Schedule(() => new MyComplexJob("Foo", DateTime.Now)).ToRunNow().AndEvery(2).Seconds();

        // Schedule multiple jobs to be run in a single schedule
        Schedule<MyJob>().AndThen<MyOtherJob>().ToRunNow().AndEvery(5).Minutes();
    }
}
```

You can also use the [Registry] class directly (instead of inheriting it):

```cs
var registry = new Registry();
registry.Schedule<MyJob>().ToRunNow().AndEvery(2).Seconds();
registry.Schedule<MyJob>().ToRunOnceIn(5).Seconds();
registry.Schedule(() => Console.WriteLine("It's 9:15 PM now.")).ToRunEvery(1).Days().At(21, 15);
registry.Schedule<MyComplexJob>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);
registry.Schedule<MyJob>().AndThen<MyOtherJob>().ToRunNow().AndEvery(5).Minutes();
```

With the registry ready you then need to initialize the [JobManager]. This is usually done as soon as your application is loaded (in the [Application_Start] method of a web application for example):

```cs
protected void Application_Start()
{
    JobManager.Initialize(new MyRegistry());
} 
```

It's also possible to schedule jobs after initialization:

```cs
JobManager.AddJob(() => Console.WriteLine("Late job!"), (s) => s.ToRunEvery(5).Seconds());
```

[JobManager]: Library/JobManager.cs
[Registry]:          Library/Registry.cs
[IJob]:              Library/IJob.cs
[Action]:            https://msdn.microsoft.com/library/System.Action
[Application_Start]: https://msdn.microsoft.com/library/ms178473

## Using it with ASP.NET

When using it with ASP.NET consider implementing [IRegisteredObject] and registering it on [HostingEnvironment]&nbsp;([here's why](http://haacked.com/archive/2011/10/16/the-dangers-of-implementing-recurring-background-tasks-in-asp-net.aspx)), like:

```cs
public class SampleJob : IJob, IRegisteredObject
{
    private readonly object _lock = new object();

    private bool _shuttingDown;

    public SampleJob()
    {
        // Register this job with the hosting environment.
        // Allows for a more graceful stop of the job, in the case of IIS shutting down.
        HostingEnvironment.RegisterObject(this);
    }

    public void Execute()
    {
        try
        {
            lock (_lock)
            {
                if (_shuttingDown)
                    return;

                // Do work, son!
            }
        }
        finally
        {
            // Always unregister the job when done.
            HostingEnvironment.UnregisterObject(this);
        }
    }

    public void Stop(bool immediate)
    {
        // Locking here will wait for the lock in Execute to be released until this code can continue.
        lock (_lock)
        {
            _shuttingDown = true;
        }

        HostingEnvironment.UnregisterObject(this);
    }
}
```

[IRegisteredObject]: https://msdn.microsoft.com/library/System.Web.Hosting.IRegisteredObject
[HostingEnvironment]: https://msdn.microsoft.com/library/System.Web.Hosting.HostingEnvironment

## Stopping the Scheduler

To stop the scheduler:

```cs
JobManager.Stop();
```

To both stop and wait for the running jobs to finish:

```cs
JobManager.StopAndBlock();
```

## Unexpected exceptions

To observe unhandled exceptions from your scheduled jobs listen for the JobException event on [JobManager]:

```cs
JobManager.JobException += info => Log.Fatal("An error just happened with a scheduled job: " + info.Exception);
```

## Concurrent jobs

By default, the library allows a schedule to run in parallel with a previously triggered execution of the
same schedule.

If you don't want such behaviour you can set a specific schedule as non-reentrant:

```cs
public class MyRegistry : Registry
{
    Schedule<MyJob>().NonReentrant().ToRunEvery(2).Seconds();
}
```

Or you can set it to all schedules of the registry at once:

```cs
public class MyRegistry : Registry
{
    NonReentrantAsDefault();
    Schedule<MyJob>().ToRunEvery(2).Seconds();
}
```

## Daylight Saving Time

Unfortunately, not unlike many schedulers, there is no daylight saving time support yet.

If you are worried about your jobs not running or running twice due to that, the suggestion is to either avoid troublesome time ranges or to force the use of UTC:

```cs
JobManager.UseUtcTime();
```

## Milliseconds Accuracy

The aim of the library is ease of use and flexibility, and not millisecond precision.  While the library has a millisecond unit, you should avoid relying on really small intervals (less than 100ms).

## Weekly jobs

Let's suppose it's 10:00 of a Monday morning and you want to start a job that runs every Monday at 14:00.
Should the first run of your job be today or only on the next week Monday?

If you want the former (not waiting for a whole week):

```cs
// Every "zero" weeks
Schedule<MyJob>().ToRunEvery(0).Weeks().On(DayOfWeek.Monday).At(14, 0);
```

Or if you want the latter (making sure that at least one week has passed):

```cs
// Every "one" weeks
Schedule<MyJob>().ToRunEvery(1).Weeks().On(DayOfWeek.Monday).At(14, 0);
```

## Dependency Injection

Currently, the library supports dependency injection of jobs (via IJobFactory). However, you shouldn't use it, it's bad idea on its way to be deprecated.

## Contributing

Feel free to [open an issue] or to [submit a pull request].

To make sure your pull request doesn't get rejected, discuss it in an issue beforehand. Also, remember to [Run All Tests (Ctrl + R, A)], [Run Code Analysis on Solution (Alt + F11)] and to be consistent with the existing code.

You can also help others in need for support, check the ["help wanted"] label.

[open an issue]: https://github.com/fluentscheduler/FluentScheduler/issues
[submit a pull request]: https://github.com/fluentscheduler/FluentScheduler/pulls
[Run All Tests (Ctrl + R, A)]: https://msdn.microsoft.com/library/ms182470
[Run Code Analysis on Solution (Alt + F11)]: https://msdn.microsoft.com/library/bb907198
["help wanted"]: https://github.com/fluentscheduler/FluentScheduler/issues?q=label:"help%20wanted"

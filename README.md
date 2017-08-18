<p align="center">
    <a href="#fluentscheduler">
        <img alt="logo" src="Assets/logo-200x200.png">
    </a>
</p>

# FluentScheduler

[![][build-img]][build]
[![][nuget-img]][nuget]

Automated job scheduler with fluent interface.

* [Usage](#usage)
* [Using it with ASP.NET](#using-it-with-aspnet)
* [Using it with .NET Core](#using-it-with-net-core)
* [Stopping the scheduler](#stopping-the-scheduler)
* [Dependency Injection](#dependency-injection)
* [Unexpected exceptions](#unexpected-exceptions)
* [Milliseconds Accuracy](#milliseconds-accuracy)
* [Daylight Saving Time](#daylight-saving-time)
* [Weekly jobs](#weekly-jobs)
* [Concurrent jobs](#concurrent-jobs)
* [Contributing](#contributing)

[build]:     https://ci.appveyor.com/project/TallesL/fluentscheduler
[build-img]: https://ci.appveyor.com/api/projects/status/github/fluentscheduler/fluentscheduler?svg=true
[nuget]:     https://www.nuget.org/packages/FluentScheduler
[nuget-img]: https://badge.fury.io/nu/fluentscheduler.svg

## Usage

The job configuration is handled in a [Registry] class.
A job is either an [Action] or a class that inherits [IJob]:

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

With the registry ready you then need to initialize the [JobManager].
This is usually done as soon as your application is loaded (in the [Application_Start] method of a web application for example):

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

When using it with ASP.NET consider implementing [IRegisteredObject] in your job and registering it itself on
[HostingEnvironment]&nbsp;([here's a great explanation on it]), like:

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
        lock (_lock)
        {
            if (_shuttingDown)
                return;

            // Do work, son!
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

[IRegisteredObject]:                https://msdn.microsoft.com/library/System.Web.Hosting.IRegisteredObject
[HostingEnvironment]:               https://msdn.microsoft.com/library/System.Web.Hosting.HostingEnvironment
[here's a great explanation on it]: http://haacked.com/archive/2011/10/16/the-dangers-of-implementing-recurring-background-tasks-in-asp-net.aspx

## Using it with .NET Core

FluentScheduler supports .NET Core, just add the dependency to `project.json` and run `dotnet restore`.

```json
  "dependencies": {
    "FluentScheduler": "<desired version>"
  }
```

## Stopping the Scheduler

If you just want to stop the scheduler, call `JobManager.Stop()`.
If you want to both stop the scheduler and wait the currently running jobs to finish, call `JobManager.StopAndBlock()`.

## Dependency Injection

FluentScheduler makes it easy to use your IoC tool of choice to create job instances.
Simply implement [IJobFactory].

An example using [StructureMap]:

```cs
using FluentScheduler;
using StructureMap;

public class StructureMapJobFactory : IJobFactory
{
    public IJob GetJobInstance<T>() where T : IJob
    {
        return ObjectFactory.Container.GetInstance<T>();
    }
}

public class MyRegistry : Registry
{
    public MyRegistry()
    {
        // Schedule an IJob to run at an interval
        Schedule<MyJob>().ToRunNow().AndEvery(2).Seconds();
    }
} 
```

Register the new job factory with the [JobManager]:

```cs
protected void Application_Start()
{
    JobManager.JobFactory = new StructureMapJobFactory();
    JobManager.Initialize(new MyRegistry()); 
}
```

[IJobFactory]:  Library/JobFactory.cs
[StructureMap]: http://structuremap.github.io

## Unexpected exceptions

To observe unhandled exceptions from your scheduled jobs, you will need to hook the JobException event on
[JobManager].
That event will give you access to the underlying [System.Threading.Tasks.Task] and thrown exception details.

```cs
JobManager.JobException += (info) => Log.Fatal("An error just happened with a scheduled job: " + info.Exception);
```

[System.Threading.Tasks.Task]: https://msdn.microsoft.com/library/System.Threading.Tasks.Task

## Milliseconds Accuracy

The aim of the library is ease of use and flexibility, and not millisecond precision.

While we make *best efforts* to ensure the library's efficiency, we don't test it for millisecond accuracy.
Guaranteeing such precision can be tricky considering the time the library has to spend 'with itself'.
As such, we provide a millisecond time unit; however, timing may not be accurate when using low intervals.

## Daylight Saving Time

Unfortunately, not unlike many schedulers, there is no Daylight Saving Time support yet.

If you are worried about your jobs not running or running twice due to that, the suggestion is to avoid troublesome time
ranges or just call `JobManager.UseUtcTime()` before using the library.

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

Or you can set it on all schedules:

```cs
public class MyRegistry : Registry
{
    NonReentrantAsDefault();
    Schedule<MyJob>().ToRunEvery(2).Seconds();
}
```

## Contributing

Feel free to [open an issue] or [submit a pull request].
To make sure your pull request doesn't go in vain (gets declined), open an issue first discussing it (before actually implementing it).

When sending a patch remember to [Run All Tests (Ctrl + R, A)] and [Run Code Analysis on Solution (Alt + F11)] if
possible.
And, of course, be consistent with the existing code.

You can also help others in need for support, there's a ["help wanted"] label to sign those issues.                                                                           
Right now most of them are about problems on different environments (other than vanilla .NET in Windows):

* Azure ([#80])
* .NET Core/Standard ([#118], [#159])
* Xamarin ([#140])

[open an issue]:                             https://github.com/fluentscheduler/FluentScheduler/issues
[submit a pull request]:                     https://github.com/fluentscheduler/FluentScheduler/pulls
[Run All Tests (Ctrl + R, A)]:               https://msdn.microsoft.com/library/ms182470
[Run Code Analysis on Solution (Alt + F11)]: https://msdn.microsoft.com/library/bb907198
["help wanted"]:                             https://github.com/fluentscheduler/FluentScheduler/issues?q=label:"help%20wanted"
[#80]:                                       https://github.com/fluentscheduler/FluentScheduler/issues/80
[#118]:                                      https://github.com/fluentscheduler/FluentScheduler/issues/118
[#140]:                                      https://github.com/fluentscheduler/FluentScheduler/issues/140
[#159]:                                      https://github.com/fluentscheduler/FluentScheduler/issues/159

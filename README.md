<p align="center">
    <a href="#fluentscheduler">
        <img alt="logo" src="Assets/logo-200x200.png">
    </a>
</p>

# FluentScheduler

[![][build-img]][build]
[![][nuget-img]][nuget]

Task scheduler with fluent interface that runs automated jobs from your application.

[build]:     https://ci.appveyor.com/project/TallesL/fluentscheduler
[build-img]: https://ci.appveyor.com/api/projects/status/github/fluentscheduler/fluentscheduler?svg=true
[nuget]:     https://www.nuget.org/packages/FluentScheduler
[nuget-img]: https://badge.fury.io/nu/fluentscheduler.svg

## Usage

All task configuration is handled in a [Registry] class.
You can use classes that implement [ITask] or express your task as an [Action].

```cs
using FluentScheduler;

public class MyRegistry : Registry
{
    public MyRegistry()
    {
        // Schedule an ITask to run at an interval
        Schedule<MyTask>().ToRunNow().AndEvery(2).Seconds();

        // Schedule an ITask to run once, delayed by a specific time interval
        Schedule<MyTask>().ToRunOnceIn(5).Seconds();

        // Schedule a simple task to run at a specific time
        Schedule(() => Console.WriteLine("Timed Task - Will run every day at 9:15pm: " + DateTime.Now))
            .ToRunEvery(1).Days().At(21, 15);

        // Schedule a more complex action to run immediately and on an monthly interval
        Schedule(() =>
        {
            Console.WriteLine("Complex Action Task Starts: " + DateTime.Now);
            Thread.Sleep(1000);
            Console.WriteLine("Complex Action Task Ends: " + DateTime.Now);
        }).ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);
        
        //Schedule multiple tasks to be run in a single schedule
        Schedule<MyTask>().AndThen<MyOtherTask>().ToRunNow().AndEvery(5).Minutes();
    }
} 
```

You then need to initialize the [TaskManager].
This is usually done in the [Application_Start] method of a web application or when your application is being loaded:

```cs
protected void Application_Start()
{
    TaskManager.Initialize(new MyRegistry()); 
} 
```

[Registry]:          Library/Registry.cs
[ITask]:             Library/ITask.cs
[Action]:            https://msdn.microsoft.com/library/System.Action
[TaskManager]:       Library/TaskManager.cs
[Application_Start]: https://msdn.microsoft.com/library/ms178473

## Using it with ASP.NET

When using it with ASP.NET consider implementing [IRegisteredObject] in your task and registering it itself on [HostingEnvironment]&nbsp;([here's a great explanation on it]), like:

```cs
public class SampleTask : ITask, IRegisteredObject
{
    private readonly object _lock = new object();

    private bool _shuttingDown;

    public SampleTask()
    {
        // Register this task with the hosting environment.
        // Allows for a more graceful stop of the task, in the case of IIS shutting down.
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

## Dependency Injection

FluentScheduler makes it easy to use your IoC tool of choice to create task instances.
Simply implement [ITaskFactory].

An example incorporating [StructureMap]:

```cs
using FluentScheduler;
using StructureMap;

public class StructureMapTaskFactory : ITaskFactory
{
    public ITask GetTaskInstance<T>() where T : ITask
    {
        return ObjectFactory.Container.GetInstance<T>();
    }
}

public class MyRegistry : Registry
{
    public MyRegistry()
    {
        // Schedule an ITask to run at an interval
        Schedule<MyTask>().ToRunNow().AndEvery(2).Seconds();
    }
} 
```

Register the new task factory with the [TaskManager]:

```cs
protected void Application_Start()
{
    TaskManager.TaskFactory = new StructureMapTaskFactory();
    TaskManager.Initialize(new MyRegistry()); 
}
```

[ITaskFactory]: Library/TaskFactory.cs
[StructureMap]: http://structuremap.github.io

## Unexpected exceptions

To observe unhandled exceptions from your scheduled tasks, you will need to hook the [UnobservedTaskException] event on [TaskManager].
That event will give you access to the underlying [System.Threading.Tasks.Task] and thrown exception details.

```cs
protected void Application_Start()
{
    TaskManager.UnobservedTaskException += TaskManager_UnobservedTaskException;
    TaskManager.Initialize(new TaskRegistry());
}

static void TaskManager_UnobservedTaskException(Task sender, UnhandledExceptionEventArgs e)
{
    Log.Fatal("An error happened with a scheduled task: " + e.ExceptionObject);
}
```

[UnobservedTaskException]:     Library/TaskManager.cs#L32
[System.Threading.Tasks.Task]: https://msdn.microsoft.com/library/System.Threading.Tasks.Task

## Contributing

Feel free to [open an issue] or [submit a pull request].

When sending a patch remember to [Run All Tests (Ctrl + R, A)] and [Run Code Analysis on Solution (Alt + F11)] if
possible.
And, of course, be consistent with the existing code!

[open an issue]:                             https://github.com/fluentscheduler/FluentScheduler/issues
[submit a pull request]:                     https://github.com/fluentscheduler/FluentScheduler/pulls
[Run All Tests (Ctrl + R, A)]:               https://msdn.microsoft.com/library/ms182470
[Run Code Analysis on Solution (Alt + F11)]: https://msdn.microsoft.com/library/bb907198
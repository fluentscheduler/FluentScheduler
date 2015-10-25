# FluentScheduler

[![][build-img]][build]
[![][nuget-img]][nuget]

Task scheduler with fluent interface that runs automated tasks (cron jobs) from your application.

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

## Dependency Injection / Inversion of Control

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

[build]:     https://ci.appveyor.com/project/TallesL/fluentscheduler
[build-img]: https://ci.appveyor.com/api/projects/status/rvgyhrs904qsxlho

[nuget]:     http://badge.fury.io/nu/fluentscheduler
[nuget-img]: https://badge.fury.io/nu/fluentscheduler.png

[Registry]:                    FluentScheduler/Registry.cs
[ITask]:                       FluentScheduler/ITask.cs
[Action]:                      https://msdn.microsoft.com/library/System.Action
[TaskManager]:                 FluentScheduler/TaskManager.cs
[ITaskFactory]:                FluentScheduler/TaskFactory.cs
[StructureMap]:                http://structuremap.github.io
[Application_Start]:           https://msdn.microsoft.com/library/ms178473
[UnobservedTaskException]:     FluentScheduler/TaskManager.cs#L18
[System.Threading.Tasks.Task]: https://msdn.microsoft.com/library/System.Threading.Tasks.Task

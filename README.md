FluentScheduler
===============

A task scheduler that uses fluent interface to configure schedules. Useful for running cron jobs/automated tasks from your application.


Getting Started
---------------

All task configuration is handled in a registry class. You can use classes that implement FluentScheduler.ITask or express your task as an Action . An example registration class would be:

```csharp
using FluentScheduler;

public class MyRegistry : Registry
{
	public MyRegistry()
	{
		// Schedule an ITask to run at an interval
		Schedule<MyTask>().ToRunNow().AndEvery(2).Seconds();

		// Schedule an ITask to run once, delayed by a specific time interval. 
		Schedule<MyTask>().ToRunOnceIn(5).Seconds();

		// Schedule a simple task to run at a specific time
		Schedule(() => Console.WriteLine("Timed Task - Will run every day at 9:15pm: " + DateTime.Now)).ToRunEvery(1).Days().At(21, 15);

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

You then need to initialize the task manager. This is usually done in the Application_Start method of a web application or when your application is being loaded:

```csharp
protected void Application_Start()
{
	TaskManager.Initialize(new MyRegistry()); 
} 
```

Using your Dependency Injection / Inversion of Control tool of choice
---------------------------------------------------------------------

FluentScheduler makes it easy to use your IOC tool to create task instances. Simply implement the ITaskFactory interface and the GetTaskInstance<T>() method. An example incorporating StructureMap:

```csharp
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

Register the new task factory with the TaskManager:

```csharp
protected void Application_Start()
{
	TaskManager.TaskFactory = new StructureMapTaskFactory();
	TaskManager.Initialize(new MyRegistry()); 
}
```

Handling unexpected exceptions thrown from tasks
------------------------------------------------

To observe unhandled exceptions from your scheduled tasks, you will need to hook the UnobservedTaskException event on TaskManager. That event will give you access to the underlying System.Threading.Tasks.Task and thrown exception details. Example code that handles unexpected exceptions:

```csharp
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
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
		
		//Schedule multiple tasks to be run in a single schedule concurrently
		Schedule<MyTask>().AndThen<MyOtherTask>().Concurrently().ToRunNow();
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

FluentScheduler makes it easy to use your IOC tool to create task instances. Simply extend the ITaskFactory class and override the GetTaskInstance<T>() method. An example incorporating StructureMap:

```csharp
using FluentScheduler;
using StructureMap;

public class StructureMapTaskFacotry : ITaskFactory
{
	public StructureMapTaskFactory () { }
	
	public override ITask GetTaskInstance<T>()
	{
		return ObjectFactory.Container.GetInstance<T>();
	}
}

public class MyRegistry : Registry
{
	public MyRegistry()
	{
		TaskFactory = new StructureMapTaskFactory();
	
		// Schedule an ITask to run at an interval
		Schedule<MyTask>().ToRunNow().AndEvery(2).Seconds();
	}
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


LICENSE
=======
New BSD License (BSD)
---------------------
Copyright (c) Bia Creations
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

* Neither the name of Bia Creations nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
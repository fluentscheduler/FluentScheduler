using System;
using System.Threading;
using System.Threading.Tasks;
using FluentScheduler;
using FluentScheduler.Model;

namespace ConsoleTester
{
	class Program
	{
		static void Main(string[] args)
		{
//			Console.WriteLine("Starting everything...");
//			TaskManager.Initialize(new MyRegistry());
//			Console.WriteLine("Done initializing...");
			//Thread.Sleep(10000);
			//TaskManager.Stop();

/*			TaskManager.AddTask(() => Console.WriteLine("Inline task: " + DateTime.Now), x => x.ToRunEvery(15).Seconds());
			TaskManager.AddTask(() => Console.WriteLine("Inline task (once): " + DateTime.Now), x => x.ToRunOnceAt(DateTime.Now.AddSeconds(5)));

			TaskManager.AddTask<MyInlineTask>(x => x.ToRunNow());
*/
			TaskManager.UnobservedTaskException += TaskManager_UnobservedTaskException;
			TaskManager.AddTask(() => {
			                          	Console.WriteLine("Inline task: " + DateTime.Now); 
				throw new Exception("Hi"); }, x => x.ToRunNow());

			Console.ReadKey();
		}

		static void TaskManager_UnobservedTaskException(Task sender, UnhandledExceptionEventArgs e)
		{
			Console.WriteLine("Something went wrong: " + e.ExceptionObject);
		}
	}

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
		}
	}

	public class MyTask : ITask
	{
		public void Execute()
		{
			Console.WriteLine("ITask Task: " + DateTime.Now);
		}
	}
	public class MyInlineTask : ITask
	{
		public void Execute()
		{
			Console.WriteLine("ITask Inline Task: " + DateTime.Now);
		}
	}
}

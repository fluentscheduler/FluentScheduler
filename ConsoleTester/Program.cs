using System;
using System.Threading;
using System.Threading.Tasks;
using FluentScheduler;

namespace ConsoleTester
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Starting everything...");

			TaskManager.TaskStart += (schedule, e) => Console.WriteLine(schedule.Name + " Started: " + schedule.StartTime);
			TaskManager.TaskEnd += (schedule, e) => Console.WriteLine(schedule.Name + " Ended.\n\tStarted: " + schedule.StartTime + "\n\tDuration: " + schedule.Duration + "\n\tNext run: " + schedule.NextRunTime);

			TaskManager.Initialize(new MyRegistry());
			Console.WriteLine("Done initializing...");

			// try to get the named schedule registered inside MyRegistry
			FluentScheduler.Model.Schedule named = TaskManager.GetSchedule("named task");
			if (named != null)
			{
				// success, execute it manually
				named.Execute();
			}

			//Thread.Sleep(10000);
			//TaskManager.Stop();

/*			TaskManager.AddTask(() => Console.WriteLine("Inline task: " + DateTime.Now), x => x.ToRunEvery(15).Seconds());
			TaskManager.AddTask(() => Console.WriteLine("Inline task (once): " + DateTime.Now), x => x.ToRunOnceAt(DateTime.Now.AddSeconds(5)));

			TaskManager.AddTask<MyInlineTask>(x => x.ToRunNow());
*/
			TaskManager.UnobservedTaskException += TaskManager_UnobservedTaskException;
/*			TaskManager.AddTask(() => {
			                          	Console.WriteLine("Inline task: " + DateTime.Now); 
				throw new Exception("Hi"); }, x => x.ToRunNow());
*/
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
			DefaultAllTasksAsNonReentrant();

			Schedule(() =>
			{
				Console.WriteLine();
				Console.WriteLine("... named task output ...");
				Console.WriteLine();
			}).WithName("named task").ToRunEvery(1).Years();

			Schedule(() =>
			{
				Console.WriteLine("Before sleep - " + DateTime.Now);
				Console.WriteLine("Running Tasks: " + TaskManager.RunningSchedules.Count);
				Thread.Sleep(4000);
				Console.WriteLine("After sleep - " + DateTime.Now);

			}).WithName("Sleepy Task").ToRunEvery(1).Months().On(10).At(5, 0);
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

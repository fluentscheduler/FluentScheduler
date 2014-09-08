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
			Console.WriteLine("Which the test you'd like to run (enter the test number):");
			Console.WriteLine("1. DelayFor");
            Console.WriteLine("2. MiscTests (everything else)");
            Console.WriteLine("3. Pause/Resume ");

			byte testNum;
			if (byte.TryParse(Console.ReadLine(), out testNum))
			{
				// which test to run?
				switch (testNum)
				{
					case 1: // DelayFor
						DelayForTest();
						break;
                            case 2: // MiscTests
                                    MiscTests();
                            break;
                        case 3: // Test of Pausing/Resuming
                                PauseResumeTest();
                        break;
					default:
						Console.WriteLine(string.Format("There's not test #{0}", testNum));
						return;
				}
			}
			else
			{
				MiscTests();
			}

			Console.ReadKey();
		}

	    static void PauseResumeTest()
	    {

            Console.WriteLine("Testing Pause/Resume...");
            TaskManager.AddTask(() => Console.WriteLine("Runner 1 " + DateTime.Now), x => x.WithName("Runner1").ToRunEvery(1).Seconds());
            //TaskManager.AddTask(() => Console.WriteLine("Runner 2 " + DateTime.Now), x => x.WithName("Runner2").ToRunEvery(1).Seconds());  //Test that pause/resume did not affect other tasks
            TaskManager.AddTask(() =>
            {
                Console.WriteLine("Pause: " + DateTime.Now);
                TaskManager.GetSchedule("Runner1").Pause();

            }, x => x.WithName("Pauser").ToRunOnceIn(10).Seconds());


            TaskManager.AddTask(() =>
            {
                Console.WriteLine("Resume: " + DateTime.Now);
                TaskManager.GetSchedule("Runner1").Resume();

            }, x => x.WithName("Resumer").ToRunOnceIn(20).Seconds());
	    }

	    static void DelayForTest()
		{
			Console.WriteLine("Testing DelayFor...");

			TaskManager.AddTask(() => Console.WriteLine("ToRunNow() - not delayed: " + DateTime.Now), x => x.ToRunNow());
			TaskManager.AddTask(() => Console.WriteLine("ToRunNow() - delayed 2 sec: " + DateTime.Now), x => x.ToRunNow().DelayFor(2).Seconds());
			TaskManager.AddTask(() => Console.WriteLine("ToRunOnceAt() - not delayed: " + DateTime.Now), x => x.ToRunOnceAt(DateTime.Now));
			TaskManager.AddTask(() => Console.WriteLine("ToRunOnceAt() - delayed 2 sec: " + DateTime.Now), x => x.ToRunOnceAt(DateTime.Now).DelayFor(2).Seconds());
			TaskManager.AddTask(() => Console.WriteLine("ToRunEvery() - not delayed: " + DateTime.Now), x => x.ToRunEvery(2).Seconds());
			TaskManager.AddTask(() => Console.WriteLine("ToRunEvery() - delayed 2 sec: " + DateTime.Now), x => x.ToRunEvery(2).Seconds().DelayFor(2).Seconds());


			//TaskManager.AddTask(() => Console.WriteLine("recurring, not delayed: " + DateTime.Now), x => x.ToRunNow().DelayFor(3).Seconds());
			//TaskManager.AddTask(() => Console.WriteLine("Inline task (delayed 5 sec): " + DateTime.Now), x => x.ToRunOnceAt(DateTime.Now).DelayFor(5).Seconds());
		}

		static void MiscTests()
		{
			TaskManager.TaskFactory = new MyTaskFactory();
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

			FluentScheduler.Model.Schedule removable = TaskManager.GetSchedule("removable task");
			if (removable != null)
			{
				Console.WriteLine("before remove");
				TaskManager.RemoveTask(removable.Name);
				Console.WriteLine("after remove");
			}

			FluentScheduler.Model.Schedule longRemovable = TaskManager.GetSchedule("long removable task");
			if (longRemovable != null)
			{
				Console.WriteLine("before remove long running");
				TaskManager.RemoveTask(longRemovable.Name);
				Console.WriteLine("after remove long running");
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
		}

		static void TaskManager_UnobservedTaskException(TaskExceptionInformation sender, UnhandledExceptionEventArgs e)
		{
			Console.WriteLine("Something went wrong with task: " + sender.Name + "\n" + e.ExceptionObject);
		}
	}

    public class MyRegistry : Registry
	{
		public MyRegistry()
		{
			DefaultAllTasksAsNonReentrant();

			//Schedule(() =>
			//{
			//    if (TaskManager.RunningSchedules.Any(x => x.Name == "Sleepy Task"))
			//    {
			//        Console.WriteLine("Skipped named task because sleepy task is running");
			//        return;
			//    }
			//    Console.WriteLine();
			//    Console.WriteLine("... named task output ...");
			//    Console.WriteLine();


			//}).WithName("named task").ToRunEvery(1).Years();

			//Schedule(() =>
			//{
			//    Console.WriteLine("Before sleep - " + DateTime.Now);
			//    Console.WriteLine("Running Tasks: " + TaskManager.RunningSchedules.Length);
			//    Thread.Sleep(4000);
			//    Console.WriteLine("After sleep - " + DateTime.Now);

			//}).WithName("Sleepy Task").ToRunEvery(1).Months().On(10).At(5, 0);

			// Immediately invoked task example
			Schedule<MyTask>().ToRunNow();

			// Delayed invoked task example
			Schedule<MyTask>().ToRunOnceIn(5).Seconds();

            // Chaining tasks example
			Schedule(() =>
			{
				Console.WriteLine("First task will fire first!");
				Console.WriteLine("Waiting four seconds...");
				Thread.Sleep(4000);
			}).AndThen(() =>
			{
				Console.WriteLine("Then the second task fires!");
			}).WithName("Multitask").ToRunNow();

			Schedule(() =>
			{
				Console.WriteLine();
				Console.WriteLine("... removable task output ...");
				Console.WriteLine();
			}).WithName("removable task").ToRunNow().AndEvery(2).Seconds();

			Schedule(() =>
			{
				Console.WriteLine();
				Console.WriteLine("Before sleep - " + DateTime.Now);
				Console.WriteLine("... long removable task output ...");
				Thread.Sleep(4000);
				Console.WriteLine("After sleep - " + DateTime.Now);
				Console.WriteLine();
			}).WithName("long removable task").ToRunNow().AndEvery(10).Seconds();
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

	public class MyTaskFactory : ITaskFactory
	{
		public ITask GetTaskInstance<T>() where T : ITask
		{
			// If you're creating your own ITaskFactory, typically your DI framework of choice would take care of this
			Console.WriteLine("Creating task: " + typeof(T));
			return Activator.CreateInstance<T>();
		}
	}
}

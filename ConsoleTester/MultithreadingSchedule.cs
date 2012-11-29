using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTester
{
    class MultithreadingSchedule : Registry
    {
        public MultithreadingSchedule()
        {
            // default (reentrant = true), should execute only once
            //Schedule<LongRunningTask>().ToRunNow();

            // setup for every 2 seconds, not reentrant
            // we should see execution of this task every 4 seconds!
            Schedule<LongRunningTask>().Reentrant(false).ToRunEvery(2).Seconds();

            // reentrant task, should see status at regular 2sec intervals
            Schedule<LongRunningTaskReentrant>().Reentrant(true).ToRunEvery(2).Seconds();
        }
    }

    class LongRunningTask : ITask
    {
        public void Execute()
        {
            // sleep for 4 sec - must be more than the schedule interval!
            System.Threading.Thread.Sleep(4000);
            Console.WriteLine(DateTime.Now + " LongRunningTask Execute");
        }
    }
    class LongRunningTaskReentrant : ITask
    {
        public void Execute()
        {
            // this task should execute regulary at every 2 sec regardless of this sleep interval
            System.Threading.Thread.Sleep(4000);
            Console.WriteLine(DateTime.Now + " LongRunningTaskReentrant Execute");
        }
    }
}

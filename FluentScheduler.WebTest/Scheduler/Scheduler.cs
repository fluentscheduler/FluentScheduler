using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FluentScheduler.WebTest.Scheduler
{
    public class Scheduler : Registry
    {
        public Scheduler()
        {
            // start the task
            Schedule<MyTask>().ToRunNow().AndEvery(10).Minutes();
        }
    }
}
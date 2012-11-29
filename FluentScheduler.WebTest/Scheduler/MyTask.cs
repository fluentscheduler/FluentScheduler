using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;

namespace FluentScheduler.WebTest.Scheduler
{
    public class MyTask : ITask, IRegisteredObject
    {
        private bool _shouldStop = false;
        public void Execute()
        {
            try
            {
                // we can call RegisterObject as many times as we want, because the same instance is always used for the same schedule
                HostingEnvironment.RegisterObject(this);

                // simulate some long-running activity
                for (int i = 0; i < 1000; i++)
                {
                    if (_shouldStop)
                    {
                        // asp.net is shutting down!
                        // save the state in DB and exit
                        return;
                    }
                    // sleep for a second
                    Thread.Sleep(1000);
                }
            }
            finally
            {
                // unregister
                HostingEnvironment.UnregisterObject(this);
            }
        }

        public void Stop(bool immediate)
        {
            // Stop() is called from asp.net hosting environment when app is shutting down
            _shouldStop = true;

            // remove our task from HostingEnvironment (if the task is not registered, nothing happens)
            HostingEnvironment.UnregisterObject(this);
        }
    }
}
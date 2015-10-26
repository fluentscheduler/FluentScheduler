using System;

namespace FluentScheduler.Tests.Web.Infrastructure.Tasks
{
    public class TaskRegistry : Registry
    {
        public TaskRegistry()
        {
            Schedule<SampleTask>()
                .ToRunNow()
                .AndEvery(1).Minutes();
        }
    }
}
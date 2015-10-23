using System;

namespace FluentScheduler.WebTester.Infrastructure.Tasks
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
using System;

namespace FluentScheduler
{
    public class TaskEndScheduleInformation : TaskStartScheduleInformation
    {
        public TimeSpan Duration { get; set; }

        public DateTime? NextRun { get; set; }
    }
}

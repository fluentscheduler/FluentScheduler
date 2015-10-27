using System;

namespace FluentScheduler.Model
{
    public class TaskEndScheduleInformation : TaskStartScheduleInformation
    {
        public TimeSpan Duration { get; set; }
        public DateTime? NextRun { get; set; }
    }
}

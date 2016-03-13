using System;

namespace FluentScheduler
{
    public class JobEndInfo : JobStartInfo
    {
        public TimeSpan Duration { get; set; }

        public DateTime? NextRun { get; set; }
    }
}

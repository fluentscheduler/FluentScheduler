namespace FluentScheduler
{
    using System;

    public class JobEndInfo : JobStartInfo
    {
        public TimeSpan Duration { get; set; }

        public DateTime? NextRun { get; set; }
    }
}

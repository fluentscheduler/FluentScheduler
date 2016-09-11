namespace FluentScheduler.Tests.TestApplication
{
    using FluentScheduler;
    using LLibrary;

    public class ParameterJob : IJob
    {
        public string Parameter { get; set; }

        static ParameterJob()
        {
            L.Register("[parameter]", "Just executed with parameter \"{0}\".");
        }

        public void Execute()
        {
            L.Log("[parameter]", Parameter);
        }
    }
}

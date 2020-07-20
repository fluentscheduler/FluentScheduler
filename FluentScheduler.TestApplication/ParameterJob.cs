namespace FluentScheduler.TestApplication
{
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

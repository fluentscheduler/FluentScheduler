namespace FluentScheduler
{
    using System.Threading.Tasks;

    public class JobExceptionInfo
    {
        public string Name { get; set; }

        public Task Task { get; set; }
    }
}

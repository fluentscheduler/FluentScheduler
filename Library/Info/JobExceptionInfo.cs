using System.Threading.Tasks;

namespace FluentScheduler
{
    public class JobExceptionInfo
    {
        public string Name { get; set; }

        public Task Task { get; set; }
    }
}

using System.Threading.Tasks;

namespace FluentScheduler
{
  public interface IAsyncJob : IFluentJob
  {
    /// <summary>
    /// Executes the job.
    /// </summary>
    Task ExecuteAsync();
  }
}
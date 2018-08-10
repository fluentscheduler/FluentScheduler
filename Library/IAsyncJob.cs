using System.Threading.Tasks;

namespace Moong.FluentScheduler
{
  public interface IAsyncJob : IFluentJob
  {
    /// <summary>
    /// Executes the job.
    /// </summary>
    Task ExecuteAsync();
  }
}
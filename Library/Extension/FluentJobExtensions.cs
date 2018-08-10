using System;
using System.Threading.Tasks;
using FluentScheduler.Helpers;

namespace FluentScheduler.Extension
{
  internal static class FluentJobExtensions
  {
    public static Task ExecuteAsync(this IFluentJob fluentJob)
    {
      switch (fluentJob)
      {
        case IJob job:
          return TaskHelpers.ExecuteSynchronously(job.Execute);
        case IAsyncJob asuncJob:
          return asuncJob.ExecuteAsync();
        default:
          throw new NotSupportedException($"The job type {fluentJob.GetType().FullName} is not supported.");
      }
    }
  }
}
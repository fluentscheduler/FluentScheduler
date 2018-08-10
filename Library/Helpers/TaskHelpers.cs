using System;
using System.Threading.Tasks;

namespace Moong.FluentScheduler.Helpers
{
  public static class TaskHelpers
  {
    /// <summary>
    /// Execute action and wrap to task.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <returns>The action within wrapped task. </returns>
    public static Task ExecuteSynchronously(Action action)
    {
      var completionSource = new TaskCompletionSource<object>();
      try
      {
        action();
        completionSource.SetResult(null);
      }
      catch (Exception ex)
      {
        completionSource.SetException(ex);
      }

      return completionSource.Task;
    }
  }
}
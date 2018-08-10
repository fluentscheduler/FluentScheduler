using System;

namespace Moong.FluentScheduler
{
  /// <summary>
  /// A job factory.
  /// </summary>
  public interface IJobFactory
  {
    /// <summary>
    /// Instantiate a job of the given type.
    /// </summary>
    /// <typeparam name="T">Type of the job to instantiate</typeparam>
    /// <returns>The instantiated job</returns>
    T GetJobInstance<T>() where T : IFluentJob;
  }

  internal class JobFactory : IJobFactory
  {
    T IJobFactory.GetJobInstance<T>()
    {
      return Activator.CreateInstance<T>();
    }
  }
}

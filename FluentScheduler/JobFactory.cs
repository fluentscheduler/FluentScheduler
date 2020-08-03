namespace FluentScheduler
{
    using System;

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
        IJob GetJobInstance<T>() where T : IJob;
    }

    internal class JobFactory : IJobFactory
    {
        IJob IJobFactory.GetJobInstance<T>()
        {
            return Activator.CreateInstance<T>();
        }
    }
}

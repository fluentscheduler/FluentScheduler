namespace FluentScheduler
{
    using System;
    using System.Diagnostics.CodeAnalysis;

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
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "The 'T' requirement is on purpose.")]
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

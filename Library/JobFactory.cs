namespace FluentScheduler
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public interface IJobFactory
    {
        /// <summary>
        /// Retrieves the job instance for the specified type
        /// </summary>
        /// <typeparam name="T">Type of job to create</typeparam>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "The 'T' requirement is on purpose.")]
        IJob GetJobInstance<T>() where T : IJob;
    }

    public class JobFactory : IJobFactory
    {
        /// <summary>
        /// Retrieves the job instance for the specified type
        /// </summary>
        /// <typeparam name="T">Type of job to create</typeparam>
        public virtual IJob GetJobInstance<T>() where T : IJob
        {
            return Activator.CreateInstance<T>();
        }
    }
}

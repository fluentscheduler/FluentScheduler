using System;
using System.Diagnostics.CodeAnalysis;

namespace FluentScheduler
{
    public interface ITaskFactory
    {
        /// <summary>
        /// Retrieves the task instance for the specified type
        /// </summary>
        /// <typeparam name="T">Type of task to create</typeparam>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "The 'T' requirement is on purpose.")]
        ITask GetTaskInstance<T>() where T : ITask;
    }

    public class TaskFactory : ITaskFactory
    {
        /// <summary>
        /// Retrieves the task instance for the specified type
        /// </summary>
        /// <typeparam name="T">Type of task to create</typeparam>
        public virtual ITask GetTaskInstance<T>() where T : ITask
        {
            return Activator.CreateInstance<T>();
        }
    }
}

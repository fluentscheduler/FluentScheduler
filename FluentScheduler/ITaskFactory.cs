using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentScheduler
{
    public class ITaskFactory
    {
        public ITaskFactory(){ }

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

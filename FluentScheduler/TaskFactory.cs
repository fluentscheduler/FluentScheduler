using System;

namespace FluentScheduler
{
	public interface ITaskFactory
	{
		/// <summary>
		/// Retrieves the task instance for the specified type
		/// </summary>
		/// <typeparam name="T">Type of task to create</typeparam>
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

using System;
using System.Web.Hosting;

namespace FluentScheduler.WebTester.Infrastructure.Tasks
{
	public class SampleTask : ITask, IRegisteredObject
	{
		private readonly object _lock = new object();
		private bool _shuttingDown;

		public SampleTask()
		{
			// Register this task with the hosting environment. Allows for a more graceful stop of the task, in the case of IIS shutting down.
			HostingEnvironment.RegisterObject(this);
		}

		public void Execute()
		{
			lock (_lock)
			{
				if (_shuttingDown)
					return;

				// TODO: Do work, son!
			}
		}

		public void Stop(bool immediate)
		{
			// Locking here will wait for the lock in Execute to be released until this code can continue.
			lock (_lock)
			{
				_shuttingDown = true;
			}
			HostingEnvironment.UnregisterObject(this);
		}
	}
}
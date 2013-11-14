using System;
using System.Threading.Tasks;

namespace FluentScheduler.Model
{
	public class TaskExceptionInformation
	{
		public string Name { get; set; }
		public Task Task { get; set; }
	}
}

namespace FluentScheduler.Tests.UnitTests.RegistryTests
{
	using System;
	using FluentScheduler.Tests.UnitTests.RegistryTests.Mocks;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class JobFactoryTests
	{

		public class JobFactory : FluentScheduler.IJobFactory
		{
			public IJob GetJobInstance<T>() where T : IJob {
				Called++;
				return Activator.CreateInstance<T>();
			}
		}

		public class TestJob : IJob, IDisposable
		{
			public TestJob() {
				Called++;
			}

			public void Dispose() {
				Called++;
			}

			public void Execute() {
				Called++;
			}
		}


		public static int Called { get; set; }

		[TestMethod]
		public void RegisterWithoutConstructingAndDisposing() {
			// Arrange
			JobManager.JobFactory = new JobFactory();
			JobManager.AddJob<TestJob>((s) => s.NonReentrant().ToRunOnceIn(1).Seconds());
			// Act

			// Assert
			Assert.AreEqual(0, JobFactoryTests.Called);

			System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
			Assert.AreEqual(4, JobFactoryTests.Called);
		}

	}
}

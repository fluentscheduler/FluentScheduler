using System.Threading.Tasks;
using Moong.FluentScheduler.Tests.UnitTests.RegistryTests.Mocks;
using Xunit;

namespace Moong.FluentScheduler.Tests.UnitTests.RegistryTests
{
 public class JobConstructionTests
  {
    [Fact]
    public async Task Should_Call_Ctor()
    {
      JobManager.AddJob<CtorJob>(s => s.ToRunNow());
      await Task.Delay(50);
      Assert.Equal(1, CtorJob.Calls);

      JobManager.AddJob<CtorJob>(s => s.ToRunNow());
      await Task.Delay(50);
      Assert.Equal(2, CtorJob.Calls);

      JobManager.AddJob<CtorJob>(s => s.ToRunNow());
      await Task.Delay(50);
      Assert.Equal(3, CtorJob.Calls);
    }

    [Fact]
    public async Task Should_Call_Dispose()
    {
      JobManager.AddJob<DisposableJob>(s => s.ToRunNow());
      await Task.Delay(50);
      Assert.True(DisposableJob.Disposed);
    }
  }
}

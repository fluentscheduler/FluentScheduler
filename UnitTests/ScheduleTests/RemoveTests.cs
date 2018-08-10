using System.Threading.Tasks;
using Xunit;

namespace Moong.FluentScheduler.Tests.UnitTests.ScheduleTests
{
  public class RemoveTests
  {
    [Fact]
    public void Should_Remove_Named_Job()
    {
      // Act
      var schedule = new Schedule(() => { }).WithName("remove named job");
      schedule.ToRunNow().AndEvery(1).Seconds();
      JobManager.Instance.RemoveJob("remove named job");

      // Assert
      Assert.Null(JobManager.Instance.GetSchedule("remove named job"));
    }

    [Fact]
    public void Should_Remove_All_Jobs()
    {
      // Act
      JobManager.Instance.AddJob(() => { }, s => s.ToRunNow());
      JobManager.Instance.AddJob(() => { }, s => s.ToRunNow());
      JobManager.Instance.AddJob(() => { }, s => s.ToRunNow());
      JobManager.Instance.AddJob(() => { }, s => s.ToRunNow());
      JobManager.Instance.AddJob(() => { }, s => s.ToRunNow());
      JobManager.Instance.AddJob(() => { }, s => s.ToRunNow());

      JobManager.Instance.RemoveAllJobs();

      // Assert
      Assert.Empty(JobManager.Instance.AllSchedules);
    }

    [Fact]
    public async Task Should_Remove_LongRunning_Job_But_Keep_Running()
    {
      // Act
      var schedule = new Schedule(() => Task.Delay(100));
      schedule.WithName("remove long running job").ToRunNow().AndEvery(2).Seconds();
      schedule.Execute();

      // Assert
      Assert.Contains(JobManager.Instance.RunningSchedules, s => s.Name == "remove long running job");

      // Act
      JobManager.Instance.RemoveJob("remove long running job");

      // Assert
      Assert.Null(JobManager.Instance.GetSchedule("remove long running job"));
      Assert.Contains(JobManager.Instance.RunningSchedules, s => s.Name == "remove long running job");
      await Task.Delay(2000);
      Assert.DoesNotContain(JobManager.Instance.RunningSchedules, s => s.Name == "remove long running job");
    }
  }
}

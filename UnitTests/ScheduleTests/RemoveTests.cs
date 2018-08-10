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
      JobManager.RemoveJob("remove named job");

      // Assert
      Assert.Null(JobManager.GetSchedule("remove named job"));
    }

    [Fact]
    public void Should_Remove_All_Jobs()
    {
      // Act
      JobManager.AddJob(() => { }, s => s.ToRunNow());
      JobManager.AddJob(() => { }, s => s.ToRunNow());
      JobManager.AddJob(() => { }, s => s.ToRunNow());
      JobManager.AddJob(() => { }, s => s.ToRunNow());
      JobManager.AddJob(() => { }, s => s.ToRunNow());
      JobManager.AddJob(() => { }, s => s.ToRunNow());

      JobManager.RemoveAllJobs();

      // Assert
      Assert.Empty(JobManager.AllSchedules);
    }

    [Fact]
    public async Task Should_Remove_LongRunning_Job_But_Keep_Running()
    {
      // Act
      var schedule = new Schedule(async () => await Task.Delay(100));
      schedule.WithName("remove long running job").ToRunNow().AndEvery(2).Seconds();
      schedule.Execute();

      // Assert
      Assert.Contains(JobManager.RunningSchedules, s => s.Name == "remove long running job");

      // Act
      JobManager.RemoveJob("remove long running job");

      // Assert
      Assert.Null(JobManager.GetSchedule("remove long running job"));
      Assert.Contains(JobManager.RunningSchedules, s => s.Name == "remove long running job");
      await Task.Delay(2000);
      Assert.DoesNotContain(JobManager.RunningSchedules, s => s.Name == "remove long running job");
    }
  }
}

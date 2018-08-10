using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Moong.FluentScheduler.Helpers;
using Moong.FluentScheduler.Unit;

namespace Moong.FluentScheduler
{
  /// <summary>
  /// A job schedule.
  /// </summary>
  public class Schedule
  {
    /// <summary>
    /// Date and time of the next run of this job schedule.
    /// </summary>
    public DateTime NextRun { get; internal set; }

    /// <summary>
    /// Name of this job schedule.
    /// </summary>
    public string Name { get; internal set; }

    /// <summary>
    /// Flag indicating if this job schedule is disabled.
    /// </summary>
    public bool Disabled { get; private set; }

    internal List<Func<Task>> Jobs { get; }

    internal Func<DateTime, DateTime> CalculateNextRun { get; set; }

    internal TimeSpan DelayRunFor { get; set; }

    internal ICollection<Schedule> AdditionalSchedules { get; set; }

    internal Schedule Parent { get; set; }

    internal bool PendingRunOnce { get; set; }

    internal object Reentrant { get; set; }

    /// <summary>
    /// Schedules a new job in the registry.
    /// </summary>
    /// <param name="func">Job to schedule.</param>
    public Schedule(Func<Task> func) 
      : this(new[] { func })
    { }

    public Schedule(Action action) 
      : this(() => TaskHelpers.ExecuteSynchronously(action))
    { }

    /// <summary>
    /// Schedules a new job in the registry.
    /// </summary>
    /// <param name="actions">Jobs to schedule</param>
    public Schedule(IEnumerable<Func<Task>> actions)
    {
      this.Disabled = false;
      this.Jobs = actions.ToList();
      this.AdditionalSchedules = new List<Schedule>();
      this.PendingRunOnce = false;
      this.Reentrant = null;
    }

    /// <summary>
    /// Executes the job regardless its schedule.
    /// </summary>
    public void Execute()
    {
      JobManager.Instance.RunJob(this);
    }

    /// <summary>
    /// Schedules another job to be run with this schedule.
    /// </summary>
    /// <param name="job">Job to run.</param>
    public Schedule AndThen(Func<Task> job)
    {
      if (job == null)
        throw new ArgumentNullException(nameof(job));

      this.Jobs.Add(job);
      return this;
    }

    public Schedule AndThen(Action job)
    {
      if (job == null)
        throw new ArgumentNullException(nameof(job));

      this.Jobs.Add(() => TaskHelpers.ExecuteSynchronously(job));
      return this;
    }

    /// <summary>
    /// Schedules another job to be run with this schedule.
    /// </summary>
    /// <param name="job">Job to run.</param>
    public Schedule AndThen(IFluentJob job)
    {
      if (job == null)
        throw new ArgumentNullException(nameof(job));

      this.Jobs.Add(JobManager.Instance.GetJobFunction(job));
      return this;
    }

    /// <summary>
    /// Schedules another job to be run with this schedule.
    /// </summary>
    /// <param name="job">Job to run.</param>
    public Schedule AndThen(Func<IFluentJob> job)
    {
      if (job == null)
        throw new ArgumentNullException(nameof(job));

      this.Jobs.Add(JobManager.Instance.GetJobFunction(job));
      return this;
    }


    /// <summary>
    /// Schedules another job to be run with this schedule.
    /// </summary>
    /// <typeparam name="T">Job to run.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
        Justification = "The 'T' requirement is on purpose.")]
    public Schedule AndThen<T>() where T : IFluentJob
    {
      this.Jobs.Add(JobManager.Instance.GetJobFunction<T>());
      return this;
    }

    /// <summary>
    /// Runs the job now.
    /// </summary>
    public SpecificTimeUnit ToRunNow()
    {
      return new SpecificTimeUnit(this);
    }

    /// <summary>
    /// Runs the job according to the given interval.
    /// </summary>
    /// <param name="interval">Interval to wait.</param>
    public TimeUnit ToRunEvery(int interval)
    {
      return new TimeUnit(this, interval);
    }

    /// <summary>
    /// Runs the job once after the given interval.
    /// </summary>
    /// <param name="interval">Interval to wait.</param>
    public TimeUnit ToRunOnceIn(int interval)
    {
      this.PendingRunOnce = true;
      return new TimeUnit(this, interval);
    }

    /// <summary>
    /// Runs the job once at the given time.
    /// </summary>
    /// <param name="hours">The hours (0 through 23).</param>
    /// <param name="minutes">The minutes (0 through 59).</param>
    public SpecificTimeUnit ToRunOnceAt(int hours, int minutes)
    {
      var dateTime =
          new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, hours, minutes, 0);

      return this.ToRunOnceAt(dateTime < JobManager.Instance.Now ? dateTime.AddDays(1) : dateTime);
    }

    /// <summary>
    /// Runs the job once at the given time.
    /// </summary>
    /// <param name="time">The time to run.</param>
    public SpecificTimeUnit ToRunOnceAt(DateTime time)
    {
      this.CalculateNextRun = x => (this.DelayRunFor > TimeSpan.Zero ? time.Add(this.DelayRunFor) : time);
      this.PendingRunOnce = true;

      return new SpecificTimeUnit(this);
    }

    /// <summary>
    /// Assigns a name to this job schedule.
    /// </summary>
    /// <param name="name">Name to assign</param>
    public Schedule WithName(string name)
    {
      this.Name = name;
      return this;
    }

    /// <summary>
    /// Sets this job schedule as non reentrant.
    /// </summary>
    public Schedule NonReentrant()
    {
      this.Reentrant = this.Reentrant ?? new object();
      return this;
    }

    /// <summary>
    /// Disables this job schedule.
    /// </summary>
    public void Disable()
    {
      this.Disabled = true;
    }

    /// <summary>
    /// Enables this job schedule.
    /// </summary>
    public void Enable()
    {
      this.Disabled = false;
    }
  }
}

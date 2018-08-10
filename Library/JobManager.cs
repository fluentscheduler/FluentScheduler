using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moong.FluentScheduler.Event;
using Moong.FluentScheduler.Extension;
using Moong.FluentScheduler.Helpers;
using Moong.FluentScheduler.Util;

namespace Moong.FluentScheduler
{
  /// <summary>
  /// Job manager that handles jobs execution.
  /// </summary>
  public class JobManager
  {
    #region Internal fields

    private const uint _maxTimerInterval = (uint) 0xfffffffe;

    private bool _useUtc;
    private readonly Timer _timer;
    private readonly ScheduleCollection _schedules = new ScheduleCollection();
    private readonly ConcurrentSet<Tuple<Schedule, Task>> _running = new ConcurrentSet<Tuple<Schedule, Task>>();

    internal DateTime Now => _useUtc ? DateTime.UtcNow : DateTime.Now;

    private static Lazy<JobManager> _lazy = new Lazy<JobManager>(() => new JobManager(), true);
    #endregion


    private JobManager()
    {
      _timer = new Timer(state => this.ScheduleJobs(), null, Timeout.Infinite, Timeout.Infinite);
    }

    public static JobManager Instance => _lazy.Value;

    #region UTC

    /// <summary>
    /// Use UTC time rather than local time.
    /// It's recommended to call this method before any other library interaction to avoid mixed dates.
    /// </summary>
    public void UseUtcTime()
    {
      _useUtc = true;
    }

    #endregion

    #region Job factory

    private IJobFactory _jobFactory;

    /// <summary>
    /// Job factory used by the job manager.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly",
        Justification = "Doing that way to not break compatibility with older versions.")]
    public IJobFactory JobFactory
    {
      private get
      {
        return _jobFactory = _jobFactory ?? new JobFactory();
      }

      set
      {
        _jobFactory = value;
      }
    }

    internal Func<Task> GetJobFunction<T>() where T : IFluentJob
    {
      return async () =>
      {
        var job = this.JobFactory.GetJobInstance<T>();
        try
        {
          await job.ExecuteAsync();
        }
        finally
        {
          this.DisposeIfNeeded(job);
        }
      };
    }

    internal Func<Task> GetJobFunction(IFluentJob job)
    {
      return async () =>
      {
        try
        {
          await job.ExecuteAsync();
        }
        finally
        {
          this.DisposeIfNeeded(job);
        }
      };
    }

    internal Func<Task> GetJobFunction(Func<IFluentJob> jobFactory)
    {
      return async () =>
      {
        var job = jobFactory();

        if (job == null)
        {
          throw new InvalidOperationException("The given Func<IFluentJob> returned null.");
        }

        try
        {
          await job.ExecuteAsync();
        }
        finally
        {
          this.DisposeIfNeeded(job);
        }
      };
    }

    private void DisposeIfNeeded(IFluentJob job)
    {
      var disposable = job as IDisposable;

      disposable?.Dispose();
    }

    #endregion

    #region Event handling

    /// <summary>
    /// Event raised when an exception occurs in a job.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly",
        Justification = "Using strong-typed GenericEventHandler<TSender, TEventArgs> event handler pattern.")]
    public event Action<JobExceptionInfo> JobException;

    /// <summary>
    /// Event raised when a job starts.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly",
        Justification = "Using strong-typed GenericEventHandler<TSender, TEventArgs> event handler pattern.")]
    public event Action<JobStartInfo> JobStart;

    /// <summary>
    /// Evemt raised when a job ends.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly",
        Justification = "Using strong-typed GenericEventHandler<TSender, TEventArgs> event handler pattern.")]
    public event Action<JobEndInfo> JobEnd;

    #endregion

    #region Start, stop & initialize

    /// <summary>
    /// Initializes the job manager with the jobs to run and starts it.
    /// </summary>
    /// <param name="registries">Registries of jobs to run</param>
    public void Initialize(params Registry[] registries)
    {
      this.InitializeWithoutStarting(registries);
      this.Start();
    }

    /// <summary>
    /// Initializes the job manager with the jobs without starting it.
    /// </summary>
    /// <param name="registries">Registries of jobs to run</param>
    public void InitializeWithoutStarting(params Registry[] registries)
    {
      if (registries == null)
        throw new ArgumentNullException(nameof(registries));

      this.CalculateNextRun(registries.SelectMany(r => r.Schedules)).ToList()
        .ForEach(this.RunJob);
    }

    /// <summary>
    /// Starts the job manager.
    /// </summary>
    public void Start()
    {
      this.ScheduleJobs();
    }

    /// <summary>
    /// Stops the job manager.
    /// </summary>
    public void Stop()
    {
      _timer.Change(Timeout.Infinite, Timeout.Infinite);
    }

    /// <summary>
    /// Stops the job manager and blocks until all running schedules finishes.
    /// </summary>
    public async Task StopAndBlock()
    {
      this.Stop();
      // Even though Stop() was just called, a scheduling may be happening right now, that's why the loop.
      // Simply waiting for the tasks inside the lock causes a deadlock (a task may try to remove itself from
      // running, but it can't access the collection, it's blocked by the wait).
      do
      {
        await Task.WhenAll(_running.Select(t => t.Item2));
      }
      while (_running.Any());
    }

    #endregion

    #region Exposing schedules

    /// <summary>
    /// Returns the schedule of the given name.
    /// </summary>
    /// <param name="name">Name of the schedule.</param>
    /// <returns>The schedule of the given name, if any.</returns>
    public Schedule GetSchedule(string name)
    {
      return _schedules.Get(name);
    }

    /// <summary>
    /// Collection of the currently running schedules.
    /// </summary>
    public IEnumerable<Schedule> RunningSchedules => _running.Select(t => t.Item1).ToList();

    /// <summary>
    /// Collection of all schedules.
    /// </summary>
    public IEnumerable<Schedule> AllSchedules => _schedules.All().ToList();

    #endregion

    #region Exposing adding & removing jobs (without the registry)

    /// <summary>
    /// Adds a job schedule to the job manager.
    /// </summary>
    /// <param name="job">Job to run.</param>
    /// <param name="schedule">Job schedule to add.</param>
    public void AddJob(Func<Task> job, Action<Schedule> schedule)
    {
      if (job == null)
      {
        throw new ArgumentNullException(nameof(job));
      }

      if (schedule == null)
      {
        throw new ArgumentNullException(nameof(schedule));
      }

      this.AddJob(schedule, new Schedule(job));
    }

    /// <summary>
    /// Adds a job schedule to the job manager.
    /// </summary>
    /// <param name="job">Job to run.</param>
    /// <param name="schedule">Job schedule to add.</param>
    public void AddJob(Action job, Action<Schedule> schedule)
    {
      if (job == null)
        throw new ArgumentNullException(nameof(job));

      if (schedule == null)
        throw new ArgumentNullException(nameof(schedule));

      this.AddJob(schedule, new Schedule(() => TaskHelpers.ExecuteSynchronously(job)));
    }

    /// <summary>
    /// Adds a job schedule to the job manager.
    /// </summary>
    /// <param name="job">Job to run.</param>
    /// <param name="schedule">Job schedule to add.</param>
    public void AddJob(IFluentJob job, Action<Schedule> schedule)
    {
      if (job == null)
        throw new ArgumentNullException(nameof(job));

      if (schedule == null)
        throw new ArgumentNullException(nameof(schedule));

      this.AddJob(schedule, new Schedule(this.GetJobFunction(job)));
    }

    /// <summary>
    /// Adds a job schedule to the job manager.
    /// </summary>
    /// <typeparam name="T">Job to run.</typeparam>
    /// <param name="schedule">Job schedule to add.</param>
    public void AddJob<T>(Action<Schedule> schedule) where T : IJob
    {
      if (schedule == null)
        throw new ArgumentNullException(nameof(schedule));

      this.AddJob(schedule, new Schedule(this.GetJobFunction<T>()) { Name = typeof(T).Name });
    }

    private void AddJob(Action<Schedule> jobSchedule, Schedule schedule)
    {
      jobSchedule(schedule);
      this.CalculateNextRun(new[] { schedule }).ToList().ForEach(this.RunJob);
      this.ScheduleJobs();
    }

    /// <summary>
    /// Removes the schedule of the given name.
    /// </summary>
    /// <param name="name">Name of the schedule.</param>
    public void RemoveJob(string name)
    {
      _schedules.Remove(name);
    }

    /// <summary>
    /// Removes all schedules.
    /// </summary>
    public void RemoveAllJobs()
    {
      _schedules.RemoveAll();
    }

    #endregion

    #region Calculating, scheduling & running

    private IEnumerable<Schedule> CalculateNextRun(IEnumerable<Schedule> schedules)
    {
      foreach (var schedule in schedules)
      {
        if (schedule.CalculateNextRun == null)
        {
          if (schedule.DelayRunFor > TimeSpan.Zero)
          {
            // delayed job
            schedule.NextRun = this.Now.Add(schedule.DelayRunFor);
            _schedules.Add(schedule);
          }
          else
          {
            // run immediately
            yield return schedule;
          }

          var hasAdded = false;
          foreach (var child in schedule.AdditionalSchedules.Where(x => x.CalculateNextRun != null))
          {
            var nextRun = child.CalculateNextRun(this.Now.Add(child.DelayRunFor).AddMilliseconds(1));
            if (!hasAdded || schedule.NextRun > nextRun)
            {
              schedule.NextRun = nextRun;
              hasAdded = true;
            }
          }
        }
        else
        {
          schedule.NextRun = schedule.CalculateNextRun(this.Now.Add(schedule.DelayRunFor));
          _schedules.Add(schedule);
        }

        foreach (var childSchedule in schedule.AdditionalSchedules)
        {
          if (childSchedule.CalculateNextRun == null)
          {
            if (childSchedule.DelayRunFor > TimeSpan.Zero)
            {
              // delayed job
              childSchedule.NextRun = this.Now.Add(childSchedule.DelayRunFor);
              _schedules.Add(childSchedule);
            }
            else
            {
              // run immediately
              yield return childSchedule;
            }
          }
          else
          {
            childSchedule.NextRun = childSchedule.CalculateNextRun(this.Now.Add(childSchedule.DelayRunFor));
            _schedules.Add(childSchedule);
          }
        }
      }
    }

    private void ScheduleJobs()
    {
      _timer.Change(Timeout.Infinite, Timeout.Infinite);
      _schedules.Sort();

      if (!_schedules.Any())
        return;

      var firstJob = _schedules.First();
      if (firstJob.NextRun <= this.Now)
      {
        this.RunJob(firstJob);
        if (firstJob.CalculateNextRun == null)
        {
          // probably a ToRunNow().DelayFor() job, there's no CalculateNextRun
        }
        else
        {
          firstJob.NextRun = firstJob.CalculateNextRun(this.Now.AddMilliseconds(1));
        }

        if (firstJob.NextRun <= this.Now || firstJob.PendingRunOnce)
        {
          _schedules.Remove(firstJob);
        }

        firstJob.PendingRunOnce = false;
        this.ScheduleJobs();
        return;
      }

      var interval = firstJob.NextRun - this.Now;

      if (interval <= TimeSpan.Zero)
      {
        this.ScheduleJobs();
      }
      else
      {
        if (interval.TotalMilliseconds > _maxTimerInterval)
          interval = TimeSpan.FromMilliseconds(_maxTimerInterval);

        _timer.Change(interval, interval);
      }
    }

    internal void RunJob(Schedule schedule)
    {
      if (schedule.Disabled)
        return;

      if (schedule.Reentrant != null && _running.Any(t => ReferenceEquals(t.Item1.Reentrant, schedule.Reentrant)))
      {
        return;
      }

      Tuple<Schedule, Task> tuple = null;

      var task = Task.Run(async () =>
      {
        var start = this.Now;

        this.JobStart?.Invoke(
          new JobStartInfo
          {
            Name = schedule.Name,
            StartTime = start,
          });

        var stopwatch = new Stopwatch();

        try
        {
          stopwatch.Start();
          foreach (var scheduleJob in schedule.Jobs)
          {
            await scheduleJob();
          }
        }
        catch (Exception e)
        {
          if (this.JobException != null)
          {
            if (e is AggregateException aggregate && aggregate.InnerExceptions.Count == 1)
              e = aggregate.InnerExceptions.Single();

            this.JobException(
                     new JobExceptionInfo
                     {
                       Name = schedule.Name,
                       Exception = e,
                     }
                 );
          }
        }
        finally
        {
          if (tuple != null)
          {
            _running.Remove(tuple);
          }

          this.JobEnd?.Invoke(
            new JobEndInfo
            {
              Name = schedule.Name,
              StartTime = start,
              Duration = stopwatch.Elapsed,
              NextRun = schedule.NextRun,
            });
        }
      });

      tuple = new Tuple<Schedule, Task>(schedule, task);
      _running.Add(tuple);

    }

    #endregion
  }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FluentScheduler.Tests.UnitTests")]

namespace FluentScheduler
{
  /// <summary>
  /// A registry of job schedules.
  /// </summary>
  public class Registry
  {
    private bool _allJobsConfiguredAsNonReentrant;

    internal List<Schedule> Schedules { get; }

    /// <summary>
    /// Default ctor.
    /// </summary>
    public Registry()
    {
      _allJobsConfiguredAsNonReentrant = false;
      this.Schedules = new List<Schedule>();
    }

    /// <summary>
    /// Sets all jobs in this schedule as non reentrant.
    /// </summary>
    public void NonReentrantAsDefault()
    {
      _allJobsConfiguredAsNonReentrant = true;
      lock (((ICollection)this.Schedules).SyncRoot)
      {
        foreach (var schedule in this.Schedules)
          schedule.NonReentrant();
      }
    }

    /// <summary>
    /// Schedules a new job in the registry.
    /// </summary>
    /// <param name="job">Job to run.</param>
    public Schedule Schedule(Action job)
    {
      if (job == null)
        throw new ArgumentNullException(nameof(job));

      return this.Schedule(job, null);
    }

    /// <summary>
    /// Schedules a new job in the registry.
    /// </summary>
    /// <param name="job">Job to run.</param>
    public Schedule Schedule(IJob job)
    {
      if (job == null)
        throw new ArgumentNullException(nameof(job));

      return this.Schedule(JobManager.GetJobAction(job), null);
    }

    /// <summary>
    /// Schedules a new job in the registry.
    /// </summary>
    /// <typeparam name="T">Job to schedule.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
        Justification = "The 'T' requirement is on purpose.")]
    public Schedule Schedule<T>() where T : IJob
    {
      return this.Schedule(JobManager.GetJobAction<T>(), typeof(T).Name);
    }

    /// <summary>
    /// Schedules a new job in the registry.
    /// </summary>
    /// <param name="job">Factory method creating a IJob instance to run.</param>
    public Schedule Schedule(Func<IJob> job)
    {
      if (job == null)
        throw new ArgumentNullException(nameof(job));

      return this.Schedule(JobManager.GetJobAction(job), null);
    }

    private Schedule Schedule(Action action, string name)
    {
      var schedule = new Schedule(action);

      if (_allJobsConfiguredAsNonReentrant)
        schedule.NonReentrant();

      lock (((ICollection)this.Schedules).SyncRoot)
      {
        this.Schedules.Add(schedule);
      }

      schedule.Name = name;

      return schedule;
    }
  }
}

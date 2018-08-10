using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Moong.FluentScheduler.Helpers;

[assembly: InternalsVisibleTo("Moong.FluentScheduler.Tests.UnitTests")]
namespace Moong.FluentScheduler
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
      lock (((ICollection) this.Schedules).SyncRoot)
      {
        foreach (var schedule in this.Schedules)
          schedule.NonReentrant();
      }
    }

    /// <summary>
    /// Schedules a new job in the registry.
    /// </summary>
    /// <param name="job">Job to run.</param>
    public Schedule Schedule(Func<Task> job)
    {
      if (job == null)
        throw new ArgumentNullException(nameof(job));

      return this.Schedule(job, null);
    }

    public Schedule Schedule(Action job)
    {
      if (job == null)
        throw new ArgumentNullException(nameof(job));

      return this.Schedule(() => TaskHelpers.ExecuteSynchronously(job), null);
    }

    /// <summary>
    /// Schedules a new job in the registry.
    /// </summary>
    /// <param name="job">Job to run.</param>
    public Schedule Schedule(IFluentJob job)
    {
      if (job == null)
        throw new ArgumentNullException(nameof(job));

      return this.Schedule(JobManager.GetJobFunction(job), null);
    }

    /// <summary>
    /// Schedules a new job in the registry.
    /// </summary>
    /// <typeparam name="T">Job to schedule.</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
        Justification = "The 'T' requirement is on purpose.")]
    public Schedule Schedule<T>() where T : IFluentJob
    {
      return this.Schedule(JobManager.GetJobFunction<T>(), typeof(T).Name);
    }

    /// <summary>
    /// Schedules a new job in the registry.
    /// </summary>
    /// <param name="job">Factory method creating a IJob instance to run.</param>
    public Schedule Schedule(Func<IFluentJob> job)
    {
      if (job == null)
        throw new ArgumentNullException(nameof(job));

      return this.Schedule(JobManager.GetJobFunction(job), null);
    }

    private Schedule Schedule(Func<Task> func, string name)
    {
      var schedule = new Schedule(func);

      if (_allJobsConfiguredAsNonReentrant)
        schedule.NonReentrant();

      lock (((ICollection) this.Schedules).SyncRoot)
      {
        this.Schedules.Add(schedule);
      }

      schedule.Name = name;

      return schedule;
    }
  }
}

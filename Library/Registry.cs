namespace FluentScheduler
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// A registry of job schedules.
    /// </summary>
    public class Registry
    {
        private bool _allJobsConfiguredAsNonReentrant;

        internal bool UtcTime { get; private set; }

        internal List<Schedule> Schedules { get; private set; }

        /// <summary>
        /// Default ctor.
        /// </summary>
        public Registry()
        {
            _allJobsConfiguredAsNonReentrant = false;
            Schedules = new List<Schedule>();
        }

        /// <summary>
        /// Sets all jobs in this schedule as non reentrant.
        /// </summary>
        public void NonReentrantAsDefault()
        {
            _allJobsConfiguredAsNonReentrant = true;
            lock (((ICollection)Schedules).SyncRoot)
            {
                foreach (var schedule in Schedules)
                    schedule.NonReentrant();
            }
        }

        /// <summary>
        /// Use UTC time rather than local time.
        /// </summary>
        public void UseUtcTime()
        {
            UtcTime = true;
        }

        /// <summary>
        /// Schedules a new job in the registry.
        /// </summary>
        /// <typeparam name="T">Job to schedule.</typeparam>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "The 'T' requirement is on purpose.")]
        public Schedule Schedule<T>() where T : IJob
        {
            var schedule = new Schedule(() => JobManager.JobFactory.GetJobInstance<T>().Execute());

            if (_allJobsConfiguredAsNonReentrant)
                schedule.NonReentrant();

            lock (((ICollection)Schedules).SyncRoot)
            {
                Schedules.Add(schedule);
            }

            schedule.Name = typeof(T).Name;
            return schedule;
        }

        /// <summary>
        /// Schedules a new job in the registry.
        /// </summary>
        /// <param name="action">Job to run.</param>
        public Schedule Schedule(Action action)
        {
            var schedule = new Schedule(action);

            if (_allJobsConfiguredAsNonReentrant)
                schedule.NonReentrant();

            lock (((ICollection)Schedules).SyncRoot)
            {
                Schedules.Add(schedule);
            }

            return schedule;
        }
    }
}

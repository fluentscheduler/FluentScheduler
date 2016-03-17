namespace FluentScheduler
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Used to register all job schedules
    /// </summary>
    public class Registry
    {
        // Schedule(x => // Do something).ToRun...
        // Schedule<MyJob>().ToRunNow()
        // Schedule<MyJob>().ToRunNow().And().ToRunEvery()...
        // Schedule<MyJob>().ToRunEvery(30).Seconds()
        // Schedule<MyJob>().ToRunEvery(15).Minutes()
        // Schedule<MyJob>().ToRunEvery(1).Hours().At(15)
        // Schedule<MyJob>().ToRunEvery(2).Days().At(0, 15)
        // Schedule<MyJob>().ToRunEvery(1).Months().On(1).OfMonth().At(0, 15)
        // Schedule<MyJob>().ToRunEvery(1).Months().On(1).Monday().At(0, 15)

        private bool _allJobsConfiguredAsNonReentrant;

        internal bool UtcTime { get; private set; }

        internal List<Schedule> Schedules { get; private set; }

        public Registry()
        {
            _allJobsConfiguredAsNonReentrant = false;
            Schedules = new List<Schedule>();
        }

        public void NonReentrantAsDefault()
        {
            _allJobsConfiguredAsNonReentrant = true;
            lock (((ICollection)Schedules).SyncRoot)
            {
                foreach (var schedule in Schedules)
                    schedule.NonReentrant();
            }
        }

        public void UseUtcTime()
        {
            UtcTime = true;
        }

        /// <summary>
        /// Schedules a job to run
        /// </summary>
        /// <typeparam name="T">Job to schedule</typeparam>
        /// <returns></returns>
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
        /// Schedules a job to run
        /// </summary>
        /// <param name="action">Job to schedule</param>
        /// <returns></returns>
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

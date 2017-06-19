namespace FluentScheduler
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Job manager that handles jobs execution.
    /// </summary>
    public static class JobManager
    {
        #region Internal fields

        private const uint _maxTimerInterval = (uint)0xfffffffe;

        private static bool _useUtc = false;

        private static Timer _timer = new Timer(state => ScheduleJobs(), null, Timeout.Infinite, Timeout.Infinite);

        private static ScheduleCollection _schedules = new ScheduleCollection();

        private static readonly ISet<Tuple<Schedule, Task>> _running = new HashSet<Tuple<Schedule, Task>>();

        internal static DateTime Now
        {
            get
            {
                return _useUtc ? DateTime.UtcNow : DateTime.Now;
            }
        }

        #endregion

        #region UTC

        /// <summary>
        /// Use UTC time rather than local time.
        /// It's recommended to call this method before any other library interaction to avoid mixed dates.
        /// </summary>
        public static void UseUtcTime()
        {
            _useUtc = true;
        }

        #endregion

        #region Job factory

        private static IJobFactory _jobFactory;

        /// <summary>
        /// Job factory used by the job manager.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly",
            Justification = "Doing that way to not break compatibility with older versions.")]
        public static IJobFactory JobFactory
        {
            private get
            {
                return (_jobFactory = _jobFactory ?? new JobFactory());
            }
            set
            {
                _jobFactory = value;
            }
        }

        internal static Action GetJobAction<T>() where T : IJob
        {
            return () =>
            {
                var job = JobFactory.GetJobInstance<T>();
                try
                {
                    job.Execute();
                }
                finally
                {
                    DisposeIfNeeded(job);
                }
            };
        }

        internal static Action GetJobAction(IJob job)
        {
            return () =>
            {
                try
                {
                    job.Execute();
                }
                finally
                {
                    DisposeIfNeeded(job);
                }
            };
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
            Justification = "It is spelled correctly.")]
        internal static Action GetJobAction(Func<IJob> jobFactory)
        {
            return () =>
            {
                var job = jobFactory();

                if (job == null)
                    throw new InvalidOperationException("The given Func<IJob> returned null.");

                try
                {
                    job.Execute();
                }
                finally
                {
                    DisposeIfNeeded(job);
                }
            };
        }

        private static void DisposeIfNeeded(IJob job)
        {
            var disposable = job as IDisposable;

            if (disposable != null)
                disposable.Dispose();
        }

        #endregion

        #region Event handling

        /// <summary>
        /// Event raised when an exception occurs in a job.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly",
            Justification = "Using strong-typed GenericEventHandler<TSender, TEventArgs> event handler pattern.")]
        public static event Action<JobExceptionInfo> JobException;

        /// <summary>
        /// Event raised when a job starts.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly",
            Justification = "Using strong-typed GenericEventHandler<TSender, TEventArgs> event handler pattern.")]
        public static event Action<JobStartInfo> JobStart;

        /// <summary>
        /// Evemt raised when a job ends.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly",
            Justification = "Using strong-typed GenericEventHandler<TSender, TEventArgs> event handler pattern.")]
        public static event Action<JobEndInfo> JobEnd;

        #endregion

        #region Start, stop & initialize

        /// <summary>
        /// Initializes the job manager with the jobs to run and starts it.
        /// </summary>
        /// <param name="registries">Registries of jobs to run</param>
        public static void Initialize(params Registry[] registries)
        {
            InitializeWithoutStarting(registries);
            Start();
        }

        /// <summary>
        /// Initializes the job manager with the jobs without starting it.
        /// </summary>
        /// <param name="registries">Registries of jobs to run</param>
        public static void InitializeWithoutStarting(params Registry[] registries)
        {
            if (registries == null)
                throw new ArgumentNullException("registries");

            CalculateNextRun(registries.SelectMany(r => r.Schedules)).ToList().ForEach(RunJob);
        }

        /// <summary>
        /// Starts the job manager.
        /// </summary>
        public static void Start()
        {
            ScheduleJobs();
        }

        /// <summary>
        /// Stops the job manager.
        /// </summary>
        public static void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Stops the job manager and blocks until all running schedules finishes.
        /// </summary>
        public static void StopAndBlock()
        {
            Stop();

            var tasks = new Task[0];

            // Even though Stop() was just called, a scheduling may be happening right now, that's why the loop.
            // Simply waiting for the tasks inside the lock causes a deadlock (a task may try to remove itself from
            // running, but it can't access the collection, it's blocked by the wait).
            do
            {
                lock (_running)
                {
                    tasks = _running.Select(t => t.Item2).ToArray();
                }

                Task.WaitAll(tasks);
            } while (tasks.Any());
        }

        #endregion

        #region Exposing schedules

        /// <summary>
        /// Returns the schedule of the given name.
        /// </summary>
        /// <param name="name">Name of the schedule.</param>
        /// <returns>The schedule of the given name, if any.</returns>
        public static Schedule GetSchedule(string name)
        {
            return _schedules.Get(name);
        }

        /// <summary>
        /// Collection of the currently running schedules.
        /// </summary>
        public static IEnumerable<Schedule> RunningSchedules
        {
            get
            {
                lock (_running)
                {
                    return _running.Select(t => t.Item1).ToList();
                }
            }
        }

        /// <summary>
        /// Collection of all schedules.
        /// </summary>
        public static IEnumerable<Schedule> AllSchedules
        {
            get
            {
                // returning a shallow copy
                return _schedules.All().ToList();
            }
        }

        #endregion

        #region Exposing adding & removing jobs (without the registry)

        /// <summary>
        /// Adds a job schedule to the job manager.
        /// </summary>
        /// <param name="job">Job to run.</param>
        /// <param name="schedule">Job schedule to add.</param>
        public static void AddJob(Action job, Action<Schedule> schedule)
        {
            if (job == null)
                throw new ArgumentNullException("job");

            if (schedule == null)
                throw new ArgumentNullException("schedule");

            AddJob(schedule, new Schedule(job));
        }

        /// <summary>
        /// Adds a job schedule to the job manager.
        /// </summary>
        /// <param name="job">Job to run.</param>
        /// <param name="schedule">Job schedule to add.</param>
        public static void AddJob(IJob job, Action<Schedule> schedule)
        {
            if (job == null)
                throw new ArgumentNullException("job");

            if (schedule == null)
                throw new ArgumentNullException("schedule");

            AddJob(schedule, new Schedule(JobManager.GetJobAction(job)));
        }

        /// <summary>
        /// Adds a job schedule to the job manager.
        /// </summary>
        /// <typeparam name="T">Job to run.</typeparam>
        /// <param name="schedule">Job schedule to add.</param>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "The 'T' requirement is on purpose.")]
        public static void AddJob<T>(Action<Schedule> schedule) where T : IJob
        {
            if (schedule == null)
                throw new ArgumentNullException("schedule");

            AddJob(schedule, new Schedule(JobManager.GetJobAction<T>()) { Name = typeof(T).Name });
        }

        private static void AddJob(Action<Schedule> jobSchedule, Schedule schedule)
        {
            jobSchedule(schedule);
            CalculateNextRun(new Schedule[] { schedule }).ToList().ForEach(RunJob);
            ScheduleJobs();
        }

        /// <summary>
        /// Removes the schedule of the given name.
        /// </summary>
        /// <param name="name">Name of the schedule.</param>
        public static void RemoveJob(string name)
        {
            _schedules.Remove(name);
        }

        /// <summary>
        /// Removes all schedules.
        /// </summary>
        public static void RemoveAllJobs()
        {
            _schedules.RemoveAll();
        }

        #endregion

        #region Calculating, scheduling & running

        private static IEnumerable<Schedule> CalculateNextRun(IEnumerable<Schedule> schedules)
        {
            foreach (var schedule in schedules)
            {
                if (schedule.CalculateNextRun == null)
                {
                    if (schedule.DelayRunFor > TimeSpan.Zero)
                    {
                        // delayed job
                        schedule.NextRun = Now.Add(schedule.DelayRunFor);
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
                        var nextRun = child.CalculateNextRun(Now.Add(child.DelayRunFor).AddMilliseconds(1));
                        if (!hasAdded || schedule.NextRun > nextRun)
                        {
                            schedule.NextRun = nextRun;
                            hasAdded = true;
                        }
                    }
                }
                else
                {
                    schedule.NextRun = schedule.CalculateNextRun(Now.Add(schedule.DelayRunFor));
                    _schedules.Add(schedule);
                }

                foreach (var childSchedule in schedule.AdditionalSchedules)
                {
                    if (childSchedule.CalculateNextRun == null)
                    {
                        if (childSchedule.DelayRunFor > TimeSpan.Zero)
                        {
                            // delayed job
                            childSchedule.NextRun = Now.Add(childSchedule.DelayRunFor);
                            _schedules.Add(childSchedule);
                        }
                        else
                        {
                            // run immediately
                            yield return childSchedule;
                            continue;
                        }
                    }
                    else
                    {
                        childSchedule.NextRun = childSchedule.CalculateNextRun(Now.Add(childSchedule.DelayRunFor));
                        _schedules.Add(childSchedule);
                    }
                }
            }
        }

        private static void ScheduleJobs()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            _schedules.Sort();

            if (!_schedules.Any())
                return;

            var firstJob = _schedules.First();
            if (firstJob.NextRun <= Now)
            {
                RunJob(firstJob);
                if (firstJob.CalculateNextRun == null)
                {
                    // probably a ToRunNow().DelayFor() job, there's no CalculateNextRun
                }
                else
                {
                    firstJob.NextRun = firstJob.CalculateNextRun(Now.AddMilliseconds(1));
                }

                if (firstJob.NextRun <= Now || firstJob.PendingRunOnce)
                {
                    _schedules.Remove(firstJob);
                }

                firstJob.PendingRunOnce = false;
                ScheduleJobs();
                return;
            }

            var interval = firstJob.NextRun - Now;

            if (interval <= TimeSpan.Zero)
            {
                ScheduleJobs();
                return;
            }
            else
            {
                if (interval.TotalMilliseconds > _maxTimerInterval)
                    interval = TimeSpan.FromMilliseconds(_maxTimerInterval);

                _timer.Change(interval, interval);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "That's OK since we're raising the JobException event with it."),
        SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "https://blogs.msdn.microsoft.com/pfxteam/2012/03/25/do-i-need-to-dispose-of-tasks/")]
        internal static void RunJob(Schedule schedule)
        {
            if (schedule.Disabled)
                return;

            lock (_running)
            {
                if (schedule.Reentrant != null &&
                    _running.Any(t => ReferenceEquals(t.Item1.Reentrant, schedule.Reentrant)))
                    return;
            }

            Tuple<Schedule, Task> tuple = null;

            var task = new Task(() =>
            {
                var start = Now;

                if (JobStart != null)
                {
                    JobStart(
                        new JobStartInfo
                        {
                            Name = schedule.Name,
                            StartTime = start,
                        }
                    );
                }

                var stopwatch = new Stopwatch();

                try
                {
                    stopwatch.Start();
                    schedule.Jobs.ForEach(action => Task.Factory.StartNew(action).Wait());
                }
                catch (Exception e)
                {
                    if (JobException != null)
                    {
                        var aggregate = e as AggregateException;

                        if (aggregate != null && aggregate.InnerExceptions.Count == 1)
                            e = aggregate.InnerExceptions.Single();

                        JobException(
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
                    lock (_running)
                    {
                        _running.Remove(tuple);
                    }

                    if (JobEnd != null)
                    {
                        JobEnd(
                            new JobEndInfo
                            {
                                Name = schedule.Name,
                                StartTime = start,
                                Duration = stopwatch.Elapsed,
                                NextRun = schedule.NextRun,
                            }
                        );
                    }
                }
            }, TaskCreationOptions.PreferFairness);

            tuple = new Tuple<Schedule, Task>(schedule, task);

            lock (_running)
            {
                _running.Add(tuple);
            }

            task.Start();
        }

        #endregion
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FluentScheduler
{
    /// <summary>
    /// Controls the timer logic to execute all configured jobs.
    /// </summary>
    public static class JobManager
    {
        private static IJobFactory _jobFactory;

        public static IJobFactory JobFactory
        {
            get
            {
                return (_jobFactory = _jobFactory ?? new JobFactory());
            }
            set
            {
                _jobFactory = value;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly",
            Justification = "Using strong-typed GenericEventHandler<TSender, TEventArgs> event handler pattern.")]
        public static event GenericEventHandler<JobExceptionInfo, FluentScheduler.UnhandledExceptionEventArgs> JobException;

        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly",
            Justification = "Using strong-typed GenericEventHandler<TSender, TEventArgs> event handler pattern.")]
        public static event GenericEventHandler<JobStartInfo, EventArgs> JobStart;

        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly",
            Justification = "Using strong-typed GenericEventHandler<TSender, TEventArgs> event handler pattern.")]
        public static event GenericEventHandler<JobEndInfo, EventArgs> JobEnd;

        private static ScheduleCollection _schedules = new ScheduleCollection();

        private static System.Threading.Timer _timer;

        private static readonly ConcurrentDictionary<List<Action>, bool> RunningNonReentrantJobs = new ConcurrentDictionary<List<Action>, bool>();

        private static readonly ConcurrentDictionary<Guid, Schedule> _runningSchedules = new ConcurrentDictionary<Guid, Schedule>();

        private static bool _useUtc = false;

        private static DateTime Now { get { return _useUtc ? DateTime.UtcNow : DateTime.Now; } }

        /// <summary>
        /// Gets a list of currently schedules currently executing.
        /// </summary>
        public static IEnumerable<Schedule> RunningSchedules
        {
            get
            {
                return _runningSchedules.Values.ToList();
            }
        }

        /// <summary>
        /// The list of all schedules, whether or not they are currently running.
        /// Use <see cref="GetSchedule"/> to get concrete schedule by name.
        /// </summary>
        public static IEnumerable<Schedule> AllSchedules
        {
            get
            {
                return _schedules.All();
            }
        }
        /// <summary>
        /// Get schedule by name.
        /// </summary>
        /// <param name="name">Schedule name</param>
        /// <returns>Schedule instance or null if the schedule does not exist</returns>
        public static Schedule GetSchedule(string name)
        {
            return _schedules.Get(name);
        }

        /// <summary>
        /// Initializes the job manager with all schedules configured in the specified registry
        /// </summary>
        /// <param name="registry">Registry containing job schedules</param>
        public static void Initialize(Registry registry)
        {
            if (registry == null)
                throw new ArgumentNullException("registry");

            _useUtc = registry.UtcTime;

            var immediateJobs = new List<Schedule>();
            AddSchedules(registry.Schedules, immediateJobs, Now);
            RunAndInitializeSchedule(immediateJobs);
        }

        private static void RaiseJobException(Schedule schedule, Task t)
        {
            var handler = JobException;
            if (handler != null && t.Exception != null)
            {
                var info = new JobExceptionInfo
                {
                    Name = schedule.Name,
                    Task = t
                };
                handler(info, new FluentScheduler.UnhandledExceptionEventArgs(t.Exception.InnerException, true));
            }
        }
        private static void RaiseJobStart(Schedule schedule, DateTime startTime)
        {
            var handler = JobStart;
            if (handler != null)
            {
                var info = new JobStartInfo
                {
                    Name = schedule.Name,
                    StartTime = startTime
                };
                handler(info, new EventArgs());
            }
        }
        private static void RaiseJobEnd(Schedule schedule, DateTime startTime, TimeSpan duration)
        {
            var handler = JobEnd;
            if (handler != null)
            {
                var info = new JobEndInfo
                {
                    Name = schedule.Name,
                    StartTime = startTime,
                    Duration = duration
                };
                if (schedule.NextRun != default(DateTime))
                    info.NextRun = schedule.NextRun;

                handler(info, new EventArgs());
            }
        }

        private static void AddSchedules(IEnumerable<Schedule> schedules, ICollection<Schedule> immediatelyInvokedSchedules, DateTime now)
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
                        // only non-delayed jobs are started right away
                        immediatelyInvokedSchedules.Add(schedule);
                    }
                    var hasAdded = false;
                    foreach (var child in schedule.AdditionalSchedules.Where(x => x.CalculateNextRun != null))
                    {
                        var nextRun = child.CalculateNextRun(now.Add(child.DelayRunFor).AddMilliseconds(1));
                        if (!hasAdded || schedule.NextRun > nextRun)
                        {
                            schedule.NextRun = nextRun;
                            hasAdded = true;
                        }
                    }
                }
                else
                {
                    schedule.NextRun = schedule.CalculateNextRun(now.Add(schedule.DelayRunFor));
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
                            immediatelyInvokedSchedules.Add(childSchedule);
                            continue;
                        }
                    }
                    else
                    {
                        childSchedule.NextRun = childSchedule.CalculateNextRun(now.Add(schedule.DelayRunFor));
                        _schedules.Add(childSchedule);
                    }
                }
            }
        }

        private static void RunAndInitializeSchedule(IEnumerable<Schedule> immediatelyInvokedSchedules)
        {
            foreach (var job in immediatelyInvokedSchedules)
            {
                StartJob(job);
            }

            if (!_schedules.Any())
                return;

            if (_timer == null)
            {
                _timer = new System.Threading.Timer(Timer_Callback, null, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            }
            _schedules.Sort();
            Schedule();
        }

        internal static void StartJob(Schedule schedule)
        {
            if (schedule.Disabled)
                return;

            if (!schedule.Reentrant)
            {
                if (!RunningNonReentrantJobs.TryAdd(schedule.Jobs, true))
                    return;
            }

            var id = Guid.NewGuid();
            _runningSchedules.TryAdd(id, schedule);

            var start = Now;
            RaiseJobStart(schedule, start);
            var task = Task.Factory.StartNew(() =>
            {
                var stopwatch = new Stopwatch();
                try
                {
                    stopwatch.Start();
                    foreach (var action in schedule.Jobs)
                    {
                        var subTask = Task.Factory.StartNew(action);
                        subTask.Wait();
                    }
                }
                finally
                {
                    stopwatch.Stop();
                    bool notUsed;
                    RunningNonReentrantJobs.TryRemove(schedule.Jobs, out notUsed);
                    Schedule notUsedSchedule;
                    _runningSchedules.TryRemove(id, out notUsedSchedule);
                    RaiseJobEnd(schedule, start, stopwatch.Elapsed);
                }
            }, TaskCreationOptions.PreferFairness);
            task.ContinueWith(t => RaiseJobException(schedule, t), TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// Adds a job to the job manager
        /// </summary>
        /// <typeparam name="T">Job to schedule</typeparam>
        /// <param name="jobSchedule">Schedule for the job</param>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "The 'T' requirement is on purpose.")]
        public static void AddJob<T>(Action<Schedule> jobSchedule) where T : IJob
        {
            if (jobSchedule == null)
                throw new ArgumentNullException("jobSchedule", "Please specify the job schedule to add to the job manager.");

            var schedule = new Schedule(JobFactory.GetJobInstance<T>())
            {
                Name = typeof(T).Name
            };
            AddJob(jobSchedule, schedule);
        }

        /// <summary>
        /// Adds a job to the job manager
        /// </summary>
        /// <param name="jobAction">Job to schedule</param>
        /// <param name="jobSchedule">Schedule for the job</param>
        public static void AddJob(Action jobAction, Action<Schedule> jobSchedule)
        {
            if (jobSchedule == null)
                throw new ArgumentNullException("jobSchedule", "Please specify the job schedule to add to the job manager.");

            var schedule = new Schedule(jobAction);
            AddJob(jobSchedule, schedule);
        }

        private static void AddJob(Action<Schedule> jobSchedule, Schedule schedule)
        {
            jobSchedule(schedule);

            var immediateJobs = new List<Schedule>();
            AddSchedules(new List<Schedule> { schedule }, immediateJobs, Now);
            RunAndInitializeSchedule(immediateJobs);
        }

        public static void RemoveJob(string name)
        {
            _schedules.Remove(name);
        }

        /// <summary>
        /// Stops the job manager from executing jobs.
        /// </summary>
        public static void Stop()
        {
            if (_timer != null)
                _timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
        }

        /// <summary>
        /// Restarts the job manager if it had previously been stopped
        /// </summary>
        public static void Start()
        {
            if (_timer != null)
                Schedule();
        }

        static void Timer_Callback(object state)
        {
            Schedule();
        }

        private static void Schedule()
        {
            _timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

            var firstJob = _schedules.First();
            if (firstJob == null)
            {
                return;
            }
            if (firstJob.NextRun <= Now)
            {
                StartJob(firstJob);
                if (firstJob.CalculateNextRun == null)
                {
                    // probably a ToRunNow().DelayFor() job, there's no CalculateNextRun
                }
                else
                {
                    firstJob.NextRun = firstJob.CalculateNextRun(Now.AddMilliseconds(1));
                }
                if (firstJob.JobExecutions > 0)
                {
                    firstJob.JobExecutions--;
                }
                if (firstJob.NextRun <= Now || firstJob.JobExecutions == 0)
                {
                    _schedules.Remove(firstJob);
                }
                _schedules.Sort();
                Schedule();
                return;
            }

            var timerInterval = firstJob.NextRun - Now;
            if (timerInterval <= TimeSpan.Zero)
            {
                Schedule();
                return;
            }

            _timer.Change(timerInterval, timerInterval);
        }
    }
}

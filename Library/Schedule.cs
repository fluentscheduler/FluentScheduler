namespace FluentScheduler
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

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

        internal List<Action> Jobs { get; private set; }

        internal Func<DateTime, DateTime> CalculateNextRun { get; set; }

        internal TimeSpan DelayRunFor { get; set; }

        internal ICollection<Schedule> AdditionalSchedules { get; set; }

        internal Schedule Parent { get; set; }

        internal bool PendingRunOnce { get; set; }

        internal object Reentrant { get; set; }

        /// <summary>
        /// Schedules a new job in the registry.
        /// </summary>
        /// <param name="action">Job to schedule.</param>
        public Schedule(Action action) : this(new[] { action }) { }

        /// <summary>
        /// Schedules a new job in the registry.
        /// </summary>
        /// <param name="actions">Jobs to schedule</param>
        public Schedule(IEnumerable<Action> actions)
        {
            Disabled = false;
            Jobs = actions.ToList();
            AdditionalSchedules = new List<Schedule>();
            PendingRunOnce = false;
            Reentrant = null;
        }

        /// <summary>
        /// Executes the job regardless its schedule.
        /// </summary>
        public void Execute()
        {
            JobManager.RunJob(this);
        }

        /// <summary>
        /// Schedules another job to be run with this schedule.
        /// </summary>
        /// <param name="job">Job to run.</param>
        public Schedule AndThen(Action job)
        {
            if (job == null)
                throw new ArgumentNullException("job");

            Jobs.Add(job);
            return this;
        }

        /// <summary>
        /// Schedules another job to be run with this schedule.
        /// </summary>
        /// <param name="job">Job to run.</param>
        public Schedule AndThen(IJob job)
        {
            if (job == null)
                throw new ArgumentNullException("job");

            Jobs.Add(JobManager.GetJobAction(job));
            return this;
        }

        /// <summary>
        /// Schedules another job to be run with this schedule.
        /// </summary>
        /// <param name="job">Job to run.</param>
        public Schedule AndThen(Func<IJob> job)
        {
            if (job == null)
                throw new ArgumentNullException("job");

            Jobs.Add(JobManager.GetJobAction(job));
            return this;
        }


        /// <summary>
        /// Schedules another job to be run with this schedule.
        /// </summary>
        /// <typeparam name="T">Job to run.</typeparam>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "The 'T' requirement is on purpose.")]
        public Schedule AndThen<T>() where T : IJob
        {
            Jobs.Add(JobManager.GetJobAction<T>());
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
            PendingRunOnce = true;
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

            return ToRunOnceAt(dateTime < JobManager.Now ? dateTime.AddDays(1) : dateTime);
        }

        /// <summary>
        /// Runs the job once at the given time.
        /// </summary>
        /// <param name="time">The time to run.</param>
        public SpecificTimeUnit ToRunOnceAt(DateTime time)
        {
            CalculateNextRun = x => (DelayRunFor > TimeSpan.Zero ? time.Add(DelayRunFor) : time);
            PendingRunOnce = true;

            return new SpecificTimeUnit(this);
        }

        /// <summary>
        /// Assigns a name to this job schedule.
        /// </summary>
        /// <param name="name">Name to assign</param>
        public Schedule WithName(string name)
        {
            Name = name;
            return this;
        }

        /// <summary>
        /// Sets this job schedule as non reentrant.
        /// </summary>
        public Schedule NonReentrant()
        {
            Reentrant = Reentrant ?? new object();
            return this;
        }

        /// <summary>
        /// Disables this job schedule.
        /// </summary>
        public void Disable()
        {
            Disabled = true;
        }

        /// <summary>
        /// Enables this job schedule.
        /// </summary>
        public void Enable()
        {
            Disabled = false;
        }
    }
}

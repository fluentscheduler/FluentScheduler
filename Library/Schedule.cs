using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FluentScheduler
{
    public class Schedule
    {
        public DateTime NextRun { get; set; }

        public string Name { get; set; }

        public bool Disabled { get; private set; }

        internal List<Action> Jobs { get; private set; }

        internal Func<DateTime, DateTime> CalculateNextRun { get; set; }

        /// <summary>
        /// The first execution can be delayed by the interval defined here.
        /// It will only delay the startup (first execution).
        /// </summary>
        internal TimeSpan DelayRunFor { get; set; }

        internal ICollection<Schedule> AdditionalSchedules { get; set; }

        internal Schedule Parent { get; set; }

        internal bool PendingRunOnce { get; set; }

        internal bool Reentrant { get; set; }

        /// <summary>
        /// Schedules the specified job to run
        /// </summary>
        /// <param name="job">Job to run</param>
        public Schedule(IJob job) : this(job.Execute) { }

        /// <summary>
        /// Schedules the specified job to run
        /// </summary>
        /// <param name="action">A parameterless method to run</param>
        public Schedule(Action action) : this(new Action[] { action }) { }

        /// <summary>
        /// Schedules the specified job to run
        /// </summary>
        /// <param name="actions">A list of parameterless methods to run</param>
        public Schedule(IEnumerable<Action> actions)
        {
            Disabled = false;
            Jobs = actions.ToList();
            AdditionalSchedules = new List<Schedule>();
            PendingRunOnce = false;
            Reentrant = true;
        }

        /// <summary>
        /// Start the job now, regardless of any scheduled start time.
        /// </summary>
        public void Execute()
        {
            JobManager.RunJob(this);
        }

        /// <summary>
        /// Schedules another job to be run with this schedule
        /// </summary>
        /// <typeparam name="T">Type of job to run</typeparam>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "The 'T' requirement is on purpose.")]
        public Schedule AndThen<T>() where T : IJob
        {
            //If no job factory has been added to the schedule, use the default.
            var factory = JobManager.JobFactory ?? new JobFactory();

            Jobs.Add(() => factory.GetJobInstance<T>().Execute());
            return this;
        }

        /// <summary>
        /// Schedules another job to be run with this schedule
        /// </summary>
        /// <param name="action">A parameterless method to run</param>
        public Schedule AndThen(Action action)
        {
            Jobs.Add(action);
            return this;
        }

        /// <summary>
        /// Schedules another job to be run with this schedule
        /// </summary>
        /// <param name="job">An instantiated IJob.</param>
        public Schedule AndThen(IJob job)
        {
            Jobs.Add(job.Execute);
            return this;
        }

        /// <summary>
        /// Schedules the specified jobs to run now
        /// </summary>
        /// <returns></returns>
        public SpecificTimeUnit ToRunNow()
        {
            return new SpecificTimeUnit(this);
        }

        /// <summary>
        /// Schedules the specified jobs to run for the specified interval
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public TimeUnit ToRunEvery(int interval)
        {
            return new TimeUnit(this, interval);
        }

        /// <summary>
        /// Schedules the specified jobs to run once, delayed by a specific time interval. 
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public TimeUnit ToRunOnceIn(int interval)
        {
            PendingRunOnce = true;
            return new TimeUnit(this, interval);
        }

        /// <summary>
        /// Schedules the specified jobs to run once at the hour and minute specified.  If the hour and minute have passed, the jobs will be executed immediately.
        /// </summary>
        /// <param name="hours">0-23: Represents the hour of today</param>
        /// <param name="minutes">0-59: Represents the minute to run the job</param>
        /// <returns></returns>
        public SpecificTimeUnit ToRunOnceAt(int hours, int minutes)
        {
            return ToRunOnceAt(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, hours, minutes, 0));
        }

        /// <summary>
        /// Schedules the specified jobs to run once at the time specified.  If the time has passed, the job will be executed immediately.
        /// </summary>
        /// <param name="time">Time to run the job</param>
        /// <returns></returns>
        public SpecificTimeUnit ToRunOnceAt(DateTime time)
        {
            CalculateNextRun = x => (DelayRunFor > TimeSpan.Zero ? time.Add(DelayRunFor) : time);
            PendingRunOnce = true;

            return new SpecificTimeUnit(this);
        }

        /// <summary>
        /// Provide a name for this schedule
        /// </summary>
        /// <param name="name">Name of this schedule</param>
        /// <returns></returns>
        public Schedule WithName(string name)
        {
            Name = name;
            return this;
        }

        /// <summary>
        /// Will not start a new instance of the scheduler if a previous schedule is still running
        /// </summary>
        /// <returns></returns>
        public Schedule NonReentrant()
        {
            Reentrant = false;
            return this;
        }

        public void Disable()
        {
            Disabled = true;
        }


        public void Enable()
        {
            Disabled = false;
        }
    }
}

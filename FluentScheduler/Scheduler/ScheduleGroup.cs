namespace FluentScheduler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Operations on multiple schedules at once.
    /// </summary>
    public static class ScheduleGroup
    {
        /// <summary>
        /// Listens for the event raised when the job starts.
        /// </summary>
        /// <param name="schedules">Schedules to operate on</param>
        /// <param name="handler">Event handler for the job start</param>
        public static void ListenJobStarted(
            this IEnumerable<Schedule> schedules, EventHandler<JobStartedEventArgs> handler) =>
            ForEach(schedules, false, i => i.JobStarted += handler);

        /// <summary>
        /// Listens for the event raised when the job ends.
        /// </summary>
        /// <param name="schedules">Schedules to operate on</param>
        /// <param name="handler">Event handler for the job end</param>
        public static void ListenJobEnded(
            this IEnumerable<Schedule> schedules, EventHandler<JobEndedEventArgs> handler) =>
            ForEach(schedules, false, i => i.JobEnded += handler);

        /// <summary>
        /// 'Unlistens' for the event raised when the job starts.
        /// </summary>
        /// <param name="schedules">Schedules to operate on</param>
        /// <param name="handler">Event handler for the job start</param>
        public static void UnlistenJobStarted(
            this IEnumerable<Schedule> schedules, EventHandler<JobStartedEventArgs> handler) =>
            ForEach(schedules, false, i => i.JobStarted -= handler);

        /// <summary>
        /// 'Unlistens' for the event raised when the job ends.
        /// </summary>
        /// <param name="schedules">Schedules to operate on</param>
        /// <param name="handler">Event handler for the job end</param>
        public static void UnlistenJobEnded(
            this IEnumerable<Schedule> schedules, EventHandler<JobEndedEventArgs> handler) =>
            ForEach(schedules, false, i => i.JobEnded -= handler);


        /// <summary>
        /// Resets the scheduling of the schedules.
        /// You must not call this method if any of the schedules is running.
        /// </summary>
        /// <param name="schedules">Schedules to operate on</param>
        public static void ResetScheduling(this IEnumerable<Schedule> schedules) =>
            ForEach(schedules, false, i => i.ShouldNotBeRunning(), i => i.ResetScheduling());

        /// <summary>
        /// Changes the scheduling of the schedules.
        /// You must not call this method if any of the schedules is running.
        /// </summary>
        /// <param name="schedules">Schedules to operate on</param>
        /// <param name="specifier">Scheduling of this schedule</param>
        public static void SetScheduling(this IEnumerable<Schedule> schedules, Action<RunSpecifier> specifier) =>
            ForEach(schedules, false, i => i.ShouldNotBeRunning(), i => i.SetScheduling(new FluentTimeCalculator(specifier)));

        /// <summary>
        /// Starts the schedules that are not already running.
        /// </summary>
        /// <param name="schedules">Schedules to operate on</param>
        public static void Start(this IEnumerable<Schedule> schedules) =>
            ForEach(schedules, false, i => i.Start());

        /// <summary>
        /// Stops the schedules that are running.
        /// This call does not block.
        /// </summary>
        /// <param name="schedules">Schedules to operate on</param>
        public static void Stop(this IEnumerable<Schedule> schedules) =>
            ForEach(schedules, false, i => i.Stop(false));

        /// <summary>
        /// Stops the schedules that are running.
        /// This call blocks (it waits for the running job to end its execution).
        /// </summary>
        /// <param name="schedules">Schedules to operate on</param>
        public static void StopAndBlock(this IEnumerable<Schedule> schedules) =>
            ForEach(schedules, true, i => i.Stop(true));

        /// <summary>
        /// Stops the schedules that are running.
        /// This call blocks (it waits for the running job to end its execution).
        /// </summary>
        /// <param name="schedules">Schedules to operate on</param>
        /// <param name="timeout">Milliseconds to wait</param>
        public static void StopAndBlock(this IEnumerable<Schedule> schedules, int timeout) =>
            ForEach(schedules, true, i => i.Stop(true, timeout));

        /// <summary>
        /// Stops the schedules that are running.
        /// This call blocks (it waits for the running job to end its execution).
        /// </summary>
        /// <param name="schedules">Schedules to operate on</param>
        /// <param name="timeout">Time to wait</param>
        public static void StopAndBlock(this IEnumerable<Schedule> schedules, TimeSpan timeout) =>
            ForEach(schedules, true, i => i.Stop(true, timeout.Milliseconds));

        /// <summary>
        /// True if all of the schedules are running, false otherwise.
        /// </summary>
        /// <param name="schedules">Schedules to operate on</param>
        public static bool AllRunning(this IEnumerable<Schedule> schedules) =>
            Select(schedules, i => i.Running()).All(r => r);

        /// <summary>
        /// True if all of the schedules are stopped, false otherwise.
        /// </summary>
        /// <param name="schedules">Schedules to operate on</param>
        public static bool AllStopped(this IEnumerable<Schedule> schedules) =>
            Select(schedules, i => i.Running()).All(r => !r);

        /// <summary>
        /// True if any of the schedules are running, false otherwise.
        /// </summary>
        /// <param name="schedules">Schedules to operate on</param>
        public static bool AnyRunning(this IEnumerable<Schedule> schedules) =>
            Select(schedules, i => i.Running()).Any(r => r);

        /// <summary>
        /// True if any of the schedules are stopped, false otherwise.
        /// </summary>
        /// <param name="schedules">Schedules to operate on</param>
        public static bool AnyStopped(this IEnumerable<Schedule> schedules) =>
            Select(schedules, i => i.Running()).Any(r => !r);

        /// <summary>
        /// The schedule that is the next to run and the date and time of its next job run.
        /// </summary>
        /// <param name="schedules">Schedules to operate on</param>
        /// <returns>The schedule and its next run date and time</returns>
        public static (Schedule, DateTime)? NextRun(this IEnumerable<Schedule> schedules)
        {
            var _schedules = schedules.ToList();

            if (!_schedules.Any())
                return null;

            var next = 0;
            var times = Select(_schedules, i => i.NextRun).ToList();

            foreach (var i in Enumerable.Range(0, times.Count))
            {
                if (times[i] < times[next])
                    next = i;
            }

            if (!times[next].HasValue)
                return null;

            return (_schedules[next], times[next].Value);
        }

        private static void ForEach(
            IEnumerable<Schedule> schedules, bool parallel, params Action<InternalSchedule>[] toRun)
        {
            var internals = Internal(schedules);

            EnterLock(internals);

            try
            {
                foreach (var _toRun in toRun)
                {
                    if (parallel)
                    {
                        Parallel.ForEach(internals, _toRun);
                    }
                    else
                    {
                        foreach (var i in internals)
                            _toRun(i);
                    }
                }
            }
            finally
            {
                ExitLock(internals);
            }
        }

        private static IEnumerable<T> Select<T>(IEnumerable<Schedule> schedules, Func<InternalSchedule, T> toRun)
        {
            var internals = Internal(schedules);

            EnterLock(internals);

            try
            {
                return internals.Select(toRun).ToList();
            }
            finally
            {
                ExitLock(internals);
            }
        }

        private static IEnumerable<InternalSchedule> Internal(IEnumerable<Schedule> schedules) =>
            schedules.Select(s => s.Internal);

        private static void EnterLock(IEnumerable<InternalSchedule> internals)
        {
            foreach (var i in internals)
                Monitor.Enter(i.RunningLock);
        }

        private static void ExitLock(IEnumerable<InternalSchedule> internals)
        {
            foreach (var i in internals)
                Monitor.Exit(i.RunningLock);
        }
    }
}
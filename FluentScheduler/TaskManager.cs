﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using FluentScheduler.Model;

namespace FluentScheduler
{
    /// <summary>
    /// Controls the timer logic to execute all configured tasks.
    /// </summary>
    public static class TaskManager
    {
        public static event GenericEventHandler<Task, UnhandledExceptionEventArgs> UnobservedTaskException;
        private static List<Schedule> _tasks;
        private static List<Action> _runningTasks;
        private static Timer _timer;

        /// <summary>
        /// Initializes the task manager with all schedules configured in the specified registry
        /// </summary>
        /// <param name="registry">Registry containing task schedules</param>
        public static void Initialize(Registry registry)
        {
            var immediateTasks = new List<Schedule>();
            lock (typeof(TaskManager))
            {
                var now = DateTime.Now;
                _tasks = new List<Schedule>();
                _runningTasks = new List<Action>();

                AddSchedules(registry.Schedules, immediateTasks, now);
            }

            RunAndInitializeSchedule(immediateTasks);
        }

        private static void ThrowUnobservedTaskException(Task t)
        {
            var handler = UnobservedTaskException;
            if (handler != null)
                handler(t, new UnhandledExceptionEventArgs(t.Exception.InnerException, true));
        }

        private static void AddSchedules(IEnumerable<Schedule> schedules, ICollection<Schedule> immediateTasks, DateTime now)
        {
            foreach (var schedule in schedules)
            {
                if (schedule.CalculateNextRun == null)
                {
                    immediateTasks.Add(schedule);
                }
                else
                {
                    schedule.NextRunTime = schedule.CalculateNextRun(now);
                    _tasks.Add(schedule);
                }

                foreach (var childSchedule in schedule.AdditionalSchedules)
                {
                    if (childSchedule.CalculateNextRun == null)
                    {
                        immediateTasks.Add(childSchedule);
                        continue;
                    }
                    childSchedule.NextRunTime = childSchedule.CalculateNextRun(now);
                    _tasks.Add(childSchedule);
                }
            }
        }

        private static void RunAndInitializeSchedule(IEnumerable<Schedule> immediateTasks)
        {
            foreach (var task in immediateTasks)
            {
                StartTask(task);
            }

            if (!_tasks.Any())
                return;

            if (_timer == null)
            {
                _timer = new Timer { AutoReset = false };
                _timer.Elapsed += Timer_Elapsed;
            }
            _tasks.Sort((x, y) => DateTime.Compare(x.NextRunTime, y.NextRunTime));
            Schedule();
        }

        /// <summary>
        /// Adds a task to the task manager
        /// </summary>
        /// <typeparam name="T">Task to schedule</typeparam>
        /// <param name="taskSchedule">Schedule for the task</param>
        public static void AddTask<T>(Action<Schedule> taskSchedule) where T : ITask, new()
        {
            if (taskSchedule == null)
                throw new ArgumentNullException("taskSchedule", "Please specify the task schedule to add to the task manager.");

            var schedule = new Schedule(new T());
            AddTask(taskSchedule, schedule);
        }

        /// <summary>
        /// Adds a task to the task manager
        /// </summary>
        /// <param name="taskAction">Task to schedule</param>
        /// <param name="taskSchedule">Schedule for the task</param>
        public static void AddTask(Action taskAction, Action<Schedule> taskSchedule)
        {
            if (taskSchedule == null)
                throw new ArgumentNullException("taskSchedule", "Please specify the task schedule to add to the task manager.");

            var schedule = new Schedule(taskAction);
            AddTask(taskSchedule, schedule);
        }

        private static void AddTask(Action<Schedule> taskSchedule, Schedule schedule)
        {
            taskSchedule(schedule);

            var immediateTasks = new List<Schedule>();
            lock (typeof(TaskManager))
            {
                var now = DateTime.Now;
                if (_tasks == null)
                    _tasks = new List<Schedule>();

                AddSchedules(new List<Schedule> { schedule }, immediateTasks, now);
            }

            RunAndInitializeSchedule(immediateTasks);
        }

        /// <summary>
        /// Stops the task manager from executing tasks.
        /// </summary>
        public static void Stop()
        {
            if (_timer != null)
                _timer.Stop();
        }

        /// <summary>
        /// Restarts the task manager if it had previously been stopped
        /// </summary>
        public static void Start()
        {
            if (_timer != null)
                Schedule();
        }

        static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Schedule();
        }

        private static void Schedule()
        {
            _timer.Stop();

            var firstTask = _tasks.First();
            if (firstTask.NextRunTime <= DateTime.Now)
            {
                StartTask(firstTask);

                firstTask.NextRunTime = firstTask.CalculateNextRun(DateTime.Now.AddMilliseconds(1));
                if (firstTask.TaskExecutions > 0)
                {
                    firstTask.TaskExecutions--;
                }
                if (firstTask.TaskExecutions == 0)
                {
                    lock (typeof(TaskManager))
                    {
                        _tasks.Remove(firstTask);
                    }
                }
                _tasks.Sort((x, y) => DateTime.Compare(x.NextRunTime, y.NextRunTime));
                Schedule();
                return;
            }

            var timerInterval = (firstTask.NextRunTime - DateTime.Now).TotalMilliseconds;
            if (timerInterval <= 0)
            {
                Schedule();
                return;
            }

            _timer.Interval = timerInterval;
            _timer.Start();
        }

        /// <summary>
        /// Start new task.
        /// Only reentrant tasks (<see cref="Schedule.IsReentrant"/>) can be started if they are already running.
        /// If task is already running and IsReentrant is false, than we are not doing anything.
        /// </summary>
        /// <param name="task"></param>
        private static void StartTask(Schedule task)
        {
            if (!task.IsReentrant)
            {
                // if task does not support reentry, we need to check that another instance is not already executing
                lock (typeof(TaskManager))
                {
                    if (_runningTasks.Contains(task.Task))
                    {
                        // task is already running, nothing to do here :)
                        return;
                    }
                }
            }

            Task.Factory.StartNew(() =>
            {
                try
                {
                    lock (typeof(TaskManager))
                    {
                        // add to running tasks
                        _runningTasks.Add(task.Task);
                    }
                    // execute task
                    task.Task();
                }
                finally
                {
                    // remove from running tasks
                    lock (typeof(TaskManager))
                    {
                        _runningTasks.Remove(task.Task);
                    }
                }
            }
                , TaskCreationOptions.PreferFairness)
                    .ContinueWith(ThrowUnobservedTaskException, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}

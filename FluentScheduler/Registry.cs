using System;
using System.Collections;
using System.Collections.Generic;
using FluentScheduler.Model;

namespace FluentScheduler
{
	/// <summary>
	/// Used to register all task schedules
	/// </summary>
	public class Registry
	{
		// Schedule(x => // Do something).ToRun...
		// Schedule<MyTask>().ToRunNow()
		// Schedule<MyTask>().ToRunNow().And().ToRunEvery()...
		// Schedule<MyTask>().ToRunEvery(30).Seconds()
		// Schedule<MyTask>().ToRunEvery(15).Minutes()
		// Schedule<MyTask>().ToRunEvery(1).Hours().At(15)
		// Schedule<MyTask>().ToRunEvery(2).Days().At(0, 15)
		// Schedule<MyTask>().ToRunEvery(1).Months().On(1).OfMonth().At(0, 15)
		// Schedule<MyTask>().ToRunEvery(1).Months().On(1).Monday().At(0, 15)

		internal List<Schedule> Schedules { get; private set; }
		internal bool AllTasksConfiguredAsNonReentrant { get; set; }

		public Registry()
		{
			Schedules = new List<Schedule>();
		}

		public void DefaultAllTasksAsNonReentrant()
		{
			AllTasksConfiguredAsNonReentrant = true;
			lock (((ICollection)Schedules).SyncRoot)
			{
				foreach (var schedule in Schedules)
				{
					schedule.Reentrant = true;
				}
			}
		}

		/// <summary>
		/// Schedules a task to run
		/// </summary>
		/// <typeparam name="T">Task to schedule</typeparam>
		/// <returns></returns>
		public Schedule Schedule<T>() where T : ITask
		{
			var schedule = new Schedule(() => GetTaskInstance<T>().Execute());
			if (AllTasksConfiguredAsNonReentrant)
			{
				schedule.Reentrant = true;
			}
			lock (((ICollection)Schedules).SyncRoot)
			{
				Schedules.Add(schedule);
			}
			return schedule;
		}

		/// <summary>
		/// Schedules a task to run
		/// </summary>
		/// <param name="action">Task to schedule</param>
		/// <returns></returns>
		public Schedule Schedule(Action action)
		{
			var schedule = new Schedule(action);
			if (AllTasksConfiguredAsNonReentrant)
			{
				schedule.Reentrant = true;
			}
			lock (((ICollection)Schedules).SyncRoot)
			{
				Schedules.Add(schedule);
			}
			return schedule;
		}

		/// <summary>
		/// Retrieves the task instance for the specified type
		/// </summary>
		/// <typeparam name="T">Type of task to create</typeparam>
		/// <returns></returns>
		public virtual ITask GetTaskInstance<T>() where T : ITask
		{
			return Activator.CreateInstance<T>();
		}
	}
}

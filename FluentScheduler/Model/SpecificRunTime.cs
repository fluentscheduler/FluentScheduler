namespace FluentScheduler.Model
{
	public class SpecificRunTime
	{
		internal Schedule Schedule { get; private set; }

		public SpecificRunTime(Schedule schedule)
		{
			Schedule = schedule;
		}

		/// <summary>
		/// Schedules the specified task to run for the specified interval
		/// </summary>
		/// <param name="interval"></param>
		/// <returns></returns>
		public TimeUnit AndEvery(int interval)
		{
			return SetupChildSchedule().ToRunEvery(interval);
		}

        /// <summary>
        /// Schedules the specified task to run for the specified interval.
        /// The specified interval is randomized between a value of (interval - 10%) 
        /// and (interval + 10%) to allow for the start time of the task to be 
        /// different for each scheduled run.
        /// <para></para>
        /// <para>*** NOTE ***</para>
        /// <para>The randomization only applies when using Seconds() and Minutes().
        /// The other time ranges (hours, days, weeks, months, years) allow for an 
        /// overridden "At()" that contradicts the intended randomness therefore, 
        /// an InvalidOperationException will be thrown.
        /// </para>
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// If you use the <see cref="ToRunAboutEvery"/> method to schedule a 
        /// random start time of the task, but use any method other than Seconds() or
        /// Minutes() to specify the amount of time you want to use when scheduling
        /// the next run time of the task.
        /// </exception>
        public TimeUnit AndAboutEvery(int interval)
        {
            return SetupChildSchedule().ToRunAboutEvery(interval);
        }

        private Schedule SetupChildSchedule()
        {
            var parent = Schedule.Parent ?? Schedule;
            var child = new Schedule(Schedule.Task)
            {
                Parent = parent,
                Reentrant = parent.Reentrant,
                Name = parent.Name
            };

            child.Parent.AdditionalSchedules.Add(child);

            return child;
        }
	}
}

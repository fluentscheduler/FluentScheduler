namespace FluentScheduler
{
    public class SpecificTimeUnit
    {
        public SpecificTimeUnit(Schedule schedule)
        {
            Schedule = schedule;
        }

        internal Schedule Schedule { get; private set; }

        /// <summary>
        /// Schedules the specified task to run for the specified interval
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public TimeUnit AndEvery(int interval)
        {
            var parent = Schedule.Parent ?? Schedule;

            var child =
                new Schedule(Schedule.Tasks)
                {
                    Parent = parent,
                    Reentrant = parent.Reentrant,
                    Name = parent.Name
                };

            child.Parent.AdditionalSchedules.Add(child);
            return child.ToRunEvery(interval);
        }
    }
}

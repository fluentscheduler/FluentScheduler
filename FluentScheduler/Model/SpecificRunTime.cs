using System;

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
            var child = new Schedule(Schedule.Task)
                            {
                                Parent = Schedule.Parent ?? Schedule,
                                IsReentrant = (Schedule.Parent ?? Schedule).IsReentrant
                            };

            child.Parent.AdditionalSchedules.Add(child);

            return child.ToRunEvery(interval);
        }
    }
}

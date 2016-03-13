using System;

namespace FluentScheduler
{
    public sealed class MonthUnit
    {
        private readonly int _duration;

        public MonthUnit(Schedule schedule, int duration)
        {
            _duration = duration;
            Schedule = schedule;
            Schedule.CalculateNextRun = x => x.Date.AddMonths(_duration);
        }

        internal Schedule Schedule { get; private set; }

        /// <summary>
        /// Schedules it to run on the day specified. If the day has passed it will execute the next scheduled month.
        /// </summary>
        /// <param name="day">1-31: Represents the day of the month</param>
        /// <returns></returns>
        public MonthOnDayOfMonthUnit On(int day)
        {
            return new MonthOnDayOfMonthUnit(Schedule, _duration, day);
        }

        /// <summary>
        /// Schedules it to run on the last day of the month.
        /// </summary>
        /// <returns></returns>
        public MonthOnLastDayOfMonthUnit OnTheLastDay()
        {
            return new MonthOnLastDayOfMonthUnit(Schedule, _duration);
        }

        /// <summary>
        /// Schedules it to run on the first occurance of the specified day of the week. If the day has passed it will execute the next scheduled month.
        /// </summary>
        /// <param name="day">Day of week to run</param>
        /// <returns></returns>
        public MonthOnDayOfWeekUnit OnTheFirst(DayOfWeek day)
        {
            return new MonthOnDayOfWeekUnit(Schedule, _duration, Week.First, day);
        }

        /// <summary>
        /// Schedules it to run on the second occurance of the specified day of the week. If the day has passed it will execute the next scheduled month.
        /// </summary>
        /// <param name="day">Day of week to run</param>
        /// <returns></returns>
        public MonthOnDayOfWeekUnit OnTheSecond(DayOfWeek day)
        {
            return new MonthOnDayOfWeekUnit(Schedule, _duration, Week.Second, day);
        }

        /// <summary>
        /// Schedules it to run on the third occurance of the specified day of the week. If the day has passed it will execute the next scheduled month.
        /// </summary>
        /// <param name="day">Day of week to run</param>
        /// <returns></returns>
        public MonthOnDayOfWeekUnit OnTheThird(DayOfWeek day)
        {
            return new MonthOnDayOfWeekUnit(Schedule, _duration, Week.Third, day);
        }

        /// <summary>
        /// Schedules it to run on the fourth occurance of the specified day of the week. If the day has passed it will execute the next scheduled month.
        /// </summary>
        /// <param name="day">Day of week to run</param>
        /// <returns></returns>
        public MonthOnDayOfWeekUnit OnTheFourth(DayOfWeek day)
        {
            return new MonthOnDayOfWeekUnit(Schedule, _duration, Week.Fourth, day);
        }

        /// <summary>
        /// Schedules it to run on the last occurance of the specified day of the week.  Depending on the month, this might be the 4th week or the 5th week. If the day has passed it will execute the next scheduled month.
        /// </summary>
        /// <param name="day">Day of week to run</param>
        /// <returns></returns>
        public MonthOnDayOfWeekUnit OnTheLast(DayOfWeek day)
        {
            return new MonthOnDayOfWeekUnit(Schedule, _duration, Week.Last, day);
        }
    }
}

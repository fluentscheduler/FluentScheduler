﻿namespace FluentScheduler
{
    using System;
    using System.Linq;

    public class MonthUnit
    {
        private readonly FluentTimeCalculator _calculator;

        internal MonthUnit(FluentTimeCalculator calculator) => _calculator = calculator;

        /// <summary>
        /// Runs the job on the given day of the month.
        /// </summary>
        /// <param name="day">The day (1 through the number of days in month)</param>
        public PeriodOnceSet On(int day)
        {
            _calculator.PeriodCalculations.Add(
                last => new DateTime(last.Year, last.Month, day, last.Hour, last.Minute, last.Second));

            return new PeriodOnceSet(_calculator);
        }

        /// <summary>
        /// Runs the job on the given day of week on the first week of the month.
        /// </summary>
        /// <param name="day">The day of the week</param>
        public PeriodOnceSet OnTheFirstDay(DayOfWeek day)
        {
            _calculator.PeriodCalculations.Add(last => SelectNthDay(last.Date.Year, last.Date.Month, day, 1));
            return new PeriodOnceSet(_calculator);
        }

        /// <summary>
        /// Runs the job on the given day of week on the second week of the month.
        /// </summary>
        /// <param name="day">The day of the week</param>
        public PeriodOnceSet OnTheSecondDay(DayOfWeek day)
        {
            _calculator.PeriodCalculations.Add(last => SelectNthDay(last.Date.Year, last.Date.Month, day, 2));
            return new PeriodOnceSet(_calculator);
        }

        /// <summary>
        /// Runs the job on the given day of week on the third week of the month.
        /// </summary>
        /// <param name="day">The day of the week</param>
        public PeriodOnceSet OnTheThirdDay(DayOfWeek day)
        {
            _calculator.PeriodCalculations.Add(last => SelectNthDay(last.Date.Year, last.Date.Month, day, 3));
            return new PeriodOnceSet(_calculator);
        }

        /// <summary>
        /// Runs the job on the given day of week on the fourth week of the month.
        /// </summary>
        /// <param name="day">The day of the week</param>
        public PeriodOnceSet OnTheFourthDay(DayOfWeek day)
        {
            _calculator.PeriodCalculations.Add(last => SelectNthDay(last.Date.Year, last.Date.Month, day, 4));
            return new PeriodOnceSet(_calculator);
        }

        /// <summary>
        /// Runs the job on the last given week of day before the given day of month.
        /// </summary>
        /// <param name="dayOfWeek">The day of the week</param>
        /// <param name="dayOfMonth">The day of the month</param>
        public PeriodOnceSet LastWeekdayBefore(DayOfWeek dayOfWeek, int dayOfMonth)
        {
            _calculator.PeriodCalculations.Add(last => 
            {
                var next = new DateTime(last.Year, last.Month, dayOfMonth);
                
                while (next.DayOfWeek != dayOfWeek)
                {
                    next = next.AddDays(-1);
                }

                return next;
            });

            return new PeriodOnceSet(_calculator);
        }

        /// <summary>
        /// Runs the job on the first given week of day after the given day of month.
        /// </summary>
        /// <param name="dayOfWeek">The day of the week</param>
        /// <param name="dayOfMonth">The day of the month</param>
        public PeriodOnceSet FirstWeekdayAfter(DayOfWeek dayOfWeek, int dayOfMonth)
        {
            _calculator.PeriodCalculations.Add(last => 
            {
                var next = new DateTime(last.Year, last.Month, dayOfMonth);
                
                while (next.DayOfWeek != dayOfWeek)
                {
                    next = next.AddDays(+1);
                }

                return next;
            });

            return new PeriodOnceSet(_calculator);
        }

        private static DateTime SelectNthDay(int year, int month, DayOfWeek dayOfWeek, int occurrence) =>
            Enumerable.Range(1, 7)
                .Select(day => new DateTime(year, month, day))
                .First(dateTime => dateTime.DayOfWeek == dayOfWeek)
                .AddDays(7 * (occurrence - 1));
    }
}

using System;

namespace FluentScheduler.Model
{
    public class TimeUnit
    {
        private readonly Schedule _schedule;
        private readonly int _duration;
        private readonly bool _randomizeStartTime;

        public TimeUnit(Schedule schedule, int duration)
        {
            _schedule = schedule;
            _duration = duration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeUnit"/> class.
        /// </summary>
        /// <param name="schedule">The task scheduler</param>
        /// <param name="duration">The duration between task runs</param>
        /// <param name="randomizeStartTime">Whether to schedule the start time of the task at a 
        /// random time based on the duration (+/- 10% of the duration)</param>
        public TimeUnit(Schedule schedule, int duration, bool randomizeStartTime)
        {
            _schedule = schedule;
            _duration = duration;
            _randomizeStartTime = randomizeStartTime;
        }

        public void Seconds()
        {
            _schedule.CalculateNextRun = x => x.AddSeconds(_randomizeStartTime ? GetRandomDuration(_duration) : _duration);
        }

        public void Minutes()
        {
            _schedule.CalculateNextRun = x => x.AddMinutes(_randomizeStartTime ? GetRandomDuration(_duration) : _duration);
        }

        public HourUnit Hours()
        {
            if (_randomizeStartTime)
            {
                throw new InvalidOperationException("Using a randomized start time to calculate the next task run time is not supported for the Hours duration.");
            }
            return new HourUnit(_schedule, _duration);
        }

        public DayUnit Days()
        {
            if (_randomizeStartTime)
            {
                throw new InvalidOperationException("Using a randomized start time to calculate the next task run time is not supported for the Days duration.");
            }
            return new DayUnit(_schedule, _duration);
        }

        public WeekUnit Weeks()
        {
            if (_randomizeStartTime)
            {
                throw new InvalidOperationException("Using a randomized start time to calculate the next task run time is not supported for the Weeks duration.");
            }

            return new WeekUnit(_schedule, _duration);
        }

        public MonthUnit Months()
        {
            if (_randomizeStartTime)
            {
                throw new InvalidOperationException("Using a randomized start time to calculate the next task run time is not supported for the Months duration.");
            }

            return new MonthUnit(_schedule, _duration);
        }

        public YearUnit Years()
        {
            if (_randomizeStartTime)
            {
                throw new InvalidOperationException("Using a randomized start time to calculate the next task run time is not supported for the Years duration.");
            }

            return new YearUnit(_schedule, _duration);
        }

        private double GetRandomDuration(int duration)
        {
            return RandomNumberGenerator.Get(duration - (duration * .1), duration + (duration * .1));
        }

        /// <summary>
        /// Generate a random number between the minimum and maximum values provided
        /// </summary>
        private class RandomNumberGenerator
        {
            private static readonly Random rnd = new Random();

            public static double Get(double min, double max)
            {
                return (rnd.NextDouble() * (max - min) + min);
            }
        }
    }
}
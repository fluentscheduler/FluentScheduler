namespace FluentScheduler
{
    using System;
    using System.Linq;

    public static class ScheduleExtensions
    {
        public static DateTime? FindNextRun(this Schedule schedule, DateTime following)
        {
            var next = schedule.CalculateNextRun(following);

            if (next > following)
            {
                return next;
            }
            else
            {
                var subNext = schedule.AdditionalSchedules
                                      .Select(x => x.FindNextRun(following))
                                      .Where(x => x.HasValue)
                                      .OrderBy(x => x.Value)
                                      .Where(x => x.Value > following)
                                      .ToArray();

                if (subNext.Length > 0)
                {
                    return subNext.First();
                }
            }

            return null;
        }
    }
}

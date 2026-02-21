#region License and Terms
//
// NCrontab - Crontab for .NET
// Copyright (c) 2008 Atif Aziz. All rights reserved.
// Portions Copyright (c) 2023 Microsoft Corp. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using System.Collections.Generic;

namespace NCrontab;

public static class CrontabScheduleExtensions
{
    /// <summary>
    /// Generates a sequence of unique next occurrences (in order) between two dates based on one or
    /// more schedules.
    /// </summary>
    /// <remarks>
    /// The <paramref name="baseTime"/> and <paramref name="endTime"/> arguments are exclusive.
    /// </remarks>

    public static IEnumerable<DateTime>
        GetNextOccurrences(this IEnumerable<CrontabSchedule> schedules,
                           DateTime baseTime, DateTime endTime)
    {
        if (schedules == null) throw new ArgumentNullException(nameof(schedules));

        return GetNextOccurrences(schedules, baseTime, endTime, static (_, dt) => dt).DistinctUntilChanged();
    }

    /// <summary>
    /// Generates a sequence of next occurrences (in order) between two dates and based on one or
    /// more schedules. An additional parameter specifies a function that projects the items of the
    /// resulting sequence where each invocation of the function is given the schedule that produced
    /// the occurrence and the occurrence itself.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <paramref name="baseTime"/> and <paramref name="endTime"/> arguments are
    /// exclusive.</para>
    /// <para>
    /// The resulting sequence can contain duplicate occurrences if multiple schedules produce the
    /// same occurrence.</para>
    /// </remarks>

    public static IEnumerable<T>
        GetNextOccurrences<T>(this IEnumerable<CrontabSchedule> schedules,
                              DateTime baseTime, DateTime endTime,
                              Func<CrontabSchedule, DateTime, T> resultSelector)
    {
        if (schedules == null) throw new ArgumentNullException(nameof(schedules));
        if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

        return Schedule.GetOccurrences(schedules,
                                       s => s.GetNextOccurrences(baseTime, endTime),
                                       resultSelector);
    }
}

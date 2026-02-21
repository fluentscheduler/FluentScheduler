#region License and Terms
//
// NCrontab - Crontab for .NET
// Copyright (c) 2008 Atif Aziz. All rights reserved.
// Portions Copyright (c) 2001 The OpenSymphony Group. All rights reserved.
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

using System.Collections.Generic;
using System.Linq;
using System;

namespace NCrontab;

static class Schedule
{
    /// <summary>
    /// Generates a sequence of occurrences based on one or more schedules.
    /// </summary>

    public static IEnumerable<TResult>
        GetOccurrences<T, TResult>(IEnumerable<T> source,
                                   Func<T, IEnumerable<DateTime>> occurrencesSelector,
                                   Func<T, DateTime, TResult> resultSelector)
    {
        return from e in source.Aggregate(Enumerable.Empty<KeyValuePair<T, DateTime>>(),
                                           (a, s) => a.Merge(from e in occurrencesSelector(s)
                                                             select Pair(s, e)))
               select resultSelector(e.Key, e.Value);

        static KeyValuePair<TKey, TValue> Pair<TKey, TValue>(TKey key, TValue value) => new(key, value);
    }

    enum Sides { None, First, Second, Both }

    /// <remarks>
    /// The schedules, <paramref name="schedule1"/> and <paramref name="schedule2"/>, are expected
    /// to produce a timeline of unique occurrences in order of earliest to latest time. The
    /// behaviour is otherwise undefined.
    /// </remarks>

    static IEnumerable<KeyValuePair<T, DateTime>>
        Merge<T>(this IEnumerable<KeyValuePair<T, DateTime>> schedule1,
                 IEnumerable<KeyValuePair<T, DateTime>> schedule2)
    {
        // Initialize two enumerators for each input sequence.

        using var enumerator1 = schedule1.GetEnumerator();
        using var enumerator2 = schedule2.GetEnumerator();

        // Initialize the flags to determine if each enumerator has a value.

        var have1 = enumerator1.MoveNext();
        var have2 = enumerator2.MoveNext();

        // Enumerate and yield the items in order of earliest to latest time.

        for (;;)
        {
            // Determine which sequence contains the next smallest due time.

            var sides = have1 && have2 ? Sides.Both
                      : have1 ? Sides.First
                      : have2 ? Sides.Second
                      : Sides.None;

#pragma warning disable IDE0010 // Add missing cases (false negative)
            switch (sides)
#pragma warning restore IDE0010 // Add missing cases
            {
                case Sides.First: // Only first sequence has a value.
                {
                    yield return enumerator1.Current;
                    have1 = enumerator1.MoveNext();
                    break;
                }
                case Sides.Second: // Only second sequence has a value.
                {
                    yield return enumerator2.Current;
                    have2 = enumerator2.MoveNext();
                    break;
                }
                case Sides.Both: // Both sequences have a value.
                {
                    // Determine which of the two has the next earliest time.

                    var occurrence1 = enumerator1.Current;
                    var occurrence2 = enumerator2.Current;

                    if (occurrence1.Value.CompareTo(occurrence2.Value) > 0)
                    {
                        // Second has the earlier time, yield it and progress to the next value in
                        // the second sequence.

                        yield return occurrence2;
                        have2 = enumerator2.MoveNext();
                    }
                    else
                    {
                        // First has the earlier time, yield it and progress to the next value in
                        // the first sequence.

                        yield return occurrence1;
                        have1 = enumerator1.MoveNext();
                    }

                    break;
                }
                case Sides.None:
                {
                    // No value left in either sequence, so end enumerating.

                    yield break;
                }
            }
        }
    }
}

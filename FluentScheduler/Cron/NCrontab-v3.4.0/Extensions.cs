#region License and Terms
//
// NCrontab - Crontab for .NET
// Copyright (c) 2024 Atif Aziz. All rights reserved.
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

static class Extensions
{
    /// <summary>
    /// Iterates over the given sequence and yields the first occurrence of each consecutively
    /// repeating element, e.g. <c>[1, 2, 2, 3, 3, 3, 2, 4]</c> &#x2192; <c>[1, 2, 3, 2, 4]</c>.
    /// </summary>

    public static IEnumerable<T> DistinctUntilChanged<T>(this IEnumerable<T> source,
                                                         IEqualityComparer<T>? comparer = null)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return Iterator(source, comparer ?? EqualityComparer<T>.Default);

        static IEnumerable<T> Iterator(IEnumerable<T> source, IEqualityComparer<T> comparer)
        {
            using var enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext())
                yield break;

            var running = enumerator.Current;
            yield return running;

            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;

                if (comparer.Equals(current, running))
                    continue;

                running = current;
                yield return current;
            }
        }
    }
}

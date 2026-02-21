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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Debug = System.Diagnostics.Debug;

namespace NCrontab;

delegate T CrontabFieldAccumulator<T>(int start, int end, int interval, T success, Func<ExceptionProvider, T> onError);

// ReSharper disable once PartialTypeWithSinglePart

sealed partial class CrontabFieldImpl
{
    public static readonly CrontabFieldImpl Second    = new(CrontabFieldKind.Second, 0, 59, null);
    public static readonly CrontabFieldImpl Minute    = new(CrontabFieldKind.Minute, 0, 59, null);
    public static readonly CrontabFieldImpl Hour      = new(CrontabFieldKind.Hour, 0, 23, null);
    public static readonly CrontabFieldImpl Day       = new(CrontabFieldKind.Day, 1, 31, null);
    public static readonly CrontabFieldImpl Month     = new(CrontabFieldKind.Month, 1, 12, ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]);
    public static readonly CrontabFieldImpl DayOfWeek = new(CrontabFieldKind.DayOfWeek, 0, 6, ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]);

    static readonly CrontabFieldImpl[] FieldByKind = [Second, Minute, Hour, Day, Month, DayOfWeek];

    static readonly CompareInfo Comparer = CultureInfo.InvariantCulture.CompareInfo;

    readonly string[]? names; // TODO reconsider empty array == unnamed

    public static CrontabFieldImpl FromKind(CrontabFieldKind kind)
    {
        if (!Enum.IsDefined(typeof(CrontabFieldKind), kind))
        {
            var kinds = string.Join(", ", Enum.GetNames(typeof(CrontabFieldKind)));
            throw new ArgumentException($"Invalid crontab field kind. Valid values are {kinds}.", nameof(kind));
        }

        return FieldByKind[(int)kind];
    }

    CrontabFieldImpl(CrontabFieldKind kind, int minValue, int maxValue, string[]? names)
    {
        Debug.Assert(Enum.IsDefined(typeof(CrontabFieldKind), kind));
        Debug.Assert(minValue >= 0);
        Debug.Assert(maxValue >= minValue);
        Debug.Assert(names == null || names.Length == (maxValue - minValue + 1));

        Kind = kind;
        MinValue = minValue;
        MaxValue = maxValue;
        this.names = names;
    }

    public CrontabFieldKind Kind { get; }
    public int MinValue { get; }
    public int MaxValue { get; }

    public int ValueCount => MaxValue - MinValue + 1;

    public void Format(ICrontabField field, TextWriter writer) =>
        Format(field, writer, false);

    public void Format(ICrontabField field, TextWriter writer, bool noNames)
    {
        if (field == null) throw new ArgumentNullException(nameof(field));
        if (writer == null) throw new ArgumentNullException(nameof(writer));

        var next = field.GetFirst();
        var count = 0;

        while (next != -1)
        {
            var first = next;
            int last;

            do
            {
                last = next;
                next = field.Next(last + 1);
            }
            while (next - last == 1);

            if (count == 0
                && first == MinValue && last == MaxValue)
            {
                writer.Write('*');
                return;
            }

            if (count > 0)
                writer.Write(',');

            if (first == last)
            {
                FormatValue(first, writer, noNames);
            }
            else
            {
                FormatValue(first, writer, noNames);
                writer.Write('-');
                FormatValue(last, writer, noNames);
            }

            count++;
        }
    }

    void FormatValue(int value, TextWriter writer, bool noNames)
    {
        if (noNames || this.names == null)
        {
            if (value is >= 0 and < 100)
            {
                FastFormatNumericValue(value, writer);
            }
            else
            {
                writer.Write(value.ToString(CultureInfo.InvariantCulture));
            }
        }
        else
        {
            var index = value - MinValue;
            writer.Write(this.names[index]);
        }
    }

    static void FastFormatNumericValue(int value, TextWriter writer)
    {
        Debug.Assert(value is >= 0 and < 100);

        if (value >= 10)
        {
            writer.Write((char)('0' + (value / 10)));
            writer.Write((char)('0' + (value % 10)));
        }
        else
        {
            writer.Write((char)('0' + value));
        }
    }

    public void Parse(string str, CrontabFieldAccumulator<ExceptionProvider?> acc) =>
        _ = TryParse(str, acc, null, static ep => throw ep());

    public T TryParse<T>(string str, CrontabFieldAccumulator<T> acc, T success,
                         Func<ExceptionProvider, T> errorSelector)
    {
        if (acc == null) throw new ArgumentNullException(nameof(acc));

        if (string.IsNullOrEmpty(str))
            return success;

        try
        {
            return InternalParse(str, acc, success, errorSelector);
        }
        catch (Exception e) when (e is FormatException or CrontabException)
        {
            return errorSelector(() => new CrontabException($"'{str}' is not a valid [{Kind}] crontab field expression.", e));
        }
    }

    T InternalParse<T>(string str, CrontabFieldAccumulator<T> acc, T success, Func<ExceptionProvider, T> errorSelector)
    {
        if (str.Length == 0)
            return errorSelector(() => new CrontabException("A crontab field value cannot be empty."));

        //
        // Next, look for a list of values (e.g. 1,2,3).
        //

        if (str.IndexOf(',') > 0)
        {
            var result = success;
            using var token = ((IEnumerable<string>)str.Split(StringSeparatorStock.Comma)).GetEnumerator();
            while (token.MoveNext() && result == null)
                result = InternalParse(token.Current, acc, success, errorSelector);
            return result;
        }

        int? every = null;

        //
        // Look for stepping first (e.g. */2 = every 2nd).
        //

        if (str.IndexOf('/') is var slashIndex and > 0)
        {
            every = int.Parse(str.Substring(slashIndex + 1), CultureInfo.InvariantCulture);
            str = str.Substring(0, slashIndex);
        }

        //
        // Next, look for wildcard (*).
        //

        if (str.Length == 1 && str[0] == '*')
        {
            return acc(-1, -1, every ?? 1, success, errorSelector);
        }

        //
        // Next, look for a range of values (e.g. 2-10).
        //

        if (str.IndexOf('-') is var dashIndex and > 0)
        {
            var first = ParseValue(str.Substring(0, dashIndex));
            var last = ParseValue(str.Substring(dashIndex + 1));

            return acc(first, last, every ?? 1, success, errorSelector);
        }

        //
        // Finally, handle the case where there is only one number.
        //

        var value = ParseValue(str);

        return every is { } someEvery
             ? acc(value, MaxValue, someEvery, success, errorSelector)
             : acc(value, value, 1, success, errorSelector);

        int ParseValue(string str)
        {
            if (str.Length == 0)
                throw new CrontabException("A crontab field value cannot be empty.");

            if (str[0] is >= '0' and <= '9')
                return int.Parse(str, CultureInfo.InvariantCulture);

            if (this.names == null)
            {
                throw new CrontabException($"'{str}' is not a valid [{Kind}] crontab field value. It must be a numeric value between {MinValue} and {MaxValue} (all inclusive).");
            }

            for (var i = 0; i < this.names.Length; i++)
            {
                if (Comparer.IsPrefix(this.names[i], str, CompareOptions.IgnoreCase))
                    return i + MinValue;
            }

            var names = string.Join(", ", this.names);
            throw new CrontabException($"'{str}' is not a known value name. Use one of the following: {names}.");
        }
    }

}

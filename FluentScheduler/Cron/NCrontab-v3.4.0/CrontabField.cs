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
using System.Collections;
using System.Globalization;
using System.IO;

namespace NCrontab;

/// <summary>
/// Represents a single crontab field.
/// </summary>

// ReSharper disable once PartialTypeWithSinglePart

public sealed partial class CrontabField : ICrontabField
{
    readonly BitArray bits;
    /* readonly */ int minValueSet;
    /* readonly */ int maxValueSet;
    readonly CrontabFieldImpl impl;

    /// <summary>
    /// Parses a crontab field expression given its kind.
    /// </summary>

    public static CrontabField Parse(CrontabFieldKind kind, string expression) =>
        TryParse(kind, expression, v => v, static e => throw e());

    public static CrontabField? TryParse(CrontabFieldKind kind, string expression) =>
        TryParse(kind, expression, static v => (CrontabField?)v, _ => null);

    public static T TryParse<T>(CrontabFieldKind kind, string expression,
                                Func<CrontabField, T> valueSelector,
                                Func<ExceptionProvider, T> errorSelector)
    {
        if (valueSelector == null) throw new ArgumentNullException(nameof(valueSelector));
        if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));

        var field = new CrontabField(CrontabFieldImpl.FromKind(kind));
        var error = field.impl.TryParse(expression, field.Accumulate, null, static e => e);
        return error == null ? valueSelector(field) : errorSelector(error);
    }

    /// <summary>
    /// Parses a crontab field expression representing seconds.
    /// </summary>

    public static CrontabField Seconds(string expression) =>
        Parse(CrontabFieldKind.Second, expression);

    /// <summary>
    /// Parses a crontab field expression representing minutes.
    /// </summary>

    public static CrontabField Minutes(string expression) =>
        Parse(CrontabFieldKind.Minute, expression);

    /// <summary>
    /// Parses a crontab field expression representing hours.
    /// </summary>

    public static CrontabField Hours(string expression) =>
        Parse(CrontabFieldKind.Hour, expression);

    /// <summary>
    /// Parses a crontab field expression representing days in any given month.
    /// </summary>

    public static CrontabField Days(string expression) =>
        Parse(CrontabFieldKind.Day, expression);

    /// <summary>
    /// Parses a crontab field expression representing months.
    /// </summary>

    public static CrontabField Months(string expression) =>
        Parse(CrontabFieldKind.Month, expression);

    /// <summary>
    /// Parses a crontab field expression representing days of a week.
    /// </summary>

    public static CrontabField DaysOfWeek(string expression) =>
        Parse(CrontabFieldKind.DayOfWeek, expression);

    CrontabField(CrontabFieldImpl impl)
    {
        this.impl = impl ?? throw new ArgumentNullException(nameof(impl));
        this.bits = new BitArray(impl.ValueCount);
        this.minValueSet = int.MaxValue;
        this.maxValueSet = -1;
    }

    /// <summary>
    /// Gets the first value of the field or -1.
    /// </summary>

    public int GetFirst() => this.minValueSet < int.MaxValue ? this.minValueSet : -1;

    /// <summary>
    /// Gets the next value of the field that occurs after the given start value or -1 if there is
    /// no next value available.
    /// </summary>

    public int Next(int start)
    {
        if (start < this.minValueSet)
            return this.minValueSet;

        var startIndex = ValueToIndex(start);
        var lastIndex = ValueToIndex(this.maxValueSet);

        for (var i = startIndex; i <= lastIndex; i++)
        {
            if (this.bits[i])
                return IndexToValue(i);
        }

        return -1;
    }

    int IndexToValue(int index) => index + this.impl.MinValue;
    int ValueToIndex(int value) => value - this.impl.MinValue;

    /// <summary>
    /// Determines if the given value occurs in the field.
    /// </summary>

    public bool Contains(int value) => this.bits[ValueToIndex(value)];

    /// <summary>
    /// Accumulates the given range (start to end) and interval of values into the current set of
    /// the field.
    /// </summary>
    /// <remarks>
    /// To set the entire range of values representable by the field, set <param name="start" /> and
    /// <param name="end" /> to -1 and <param name="interval" /> to 1.
    /// </remarks>

    T Accumulate<T>(int start, int end, int interval, T success, Func<ExceptionProvider, T> errorSelector)
    {
        var minValue = this.impl.MinValue;
        var maxValue = this.impl.MaxValue;

        if (start == end)
        {
            if (start < 0)
            {
                //
                // We're setting the entire range of values.
                //

                if (interval <= 1)
                {
                    this.minValueSet = minValue;
                    this.maxValueSet = maxValue;
                    this.bits.SetAll(true);
                    return success;
                }

                start = minValue;
                end = maxValue;
            }
            else
            {
                //
                // We're only setting a single value - check that it is in range.
                //

                if (start < minValue)
                    return OnValueBelowMinError(start);

                if (start > maxValue)
                    return OnValueAboveMaxError(start);
            }
        }
        else
        {
            //
            // For ranges, if the start is bigger than the end value then
            // swap them over.
            //

            if (start > end)
            {
                end ^= start;
                start ^= end;
                end ^= start;
            }

            if (start < 0)
                start = minValue;
            else if (start < minValue)
                return OnValueBelowMinError(start);

            if (end < 0)
                end = maxValue;
            else if (end > maxValue)
                return OnValueAboveMaxError(end);
        }

        if (interval < 1)
            interval = 1;

        int i;

        //
        // Populate the _bits table by setting all the bits corresponding to the valid field values.
        //

        for (i = start - minValue; i <= (end - minValue); i += interval)
            this.bits[i] = true;

        //
        // Make sure we remember the minimum value set so far Keep track of the highest and lowest
        // values that have been added to this field so far.
        //

        if (this.minValueSet > start)
            this.minValueSet = start;

        i += minValue - interval;

        if (this.maxValueSet < i)
            this.maxValueSet = i;

        return success;

        T OnValueAboveMaxError(int value) =>
            errorSelector(
                () => new CrontabException(
                    $"{value} is higher than the maximum allowable value for the [{this.impl.Kind}] field. " +
                    $"Value must be between {this.impl.MinValue} and {this.impl.MaxValue} (all inclusive)."));

        T OnValueBelowMinError(int value) =>
            errorSelector(
                () => new CrontabException(
                    $"{value} is lower than the minimum allowable value for the [{this.impl.Kind}] field. " +
                    $"Value must be between {this.impl.MinValue} and {this.impl.MaxValue} (all inclusive)."));
    }

    public override string ToString() => ToString(null);

    public string ToString(string? format)
    {
        using var writer = new StringWriter(CultureInfo.InvariantCulture);

        switch (format)
        {
            case "G":
            case null:
                Format(writer, true);
                break;
            case "N":
                Format(writer);
                break;
            default:
                throw new FormatException();
        }

        return writer.ToString();
    }

    public void Format(TextWriter writer) => Format(writer, false);

    public void Format(TextWriter writer, bool noNames) =>
        this.impl.Format(this, writer, noNames);
}

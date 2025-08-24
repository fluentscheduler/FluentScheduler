using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FluentScheduler;

// helper methods for performing simple validations and that should throw an exception
internal class ThrowHelper
{
    // throws if the given value is not present in the given enum T
    internal static void ThrowIfNotDefinedInEnum<T>(
        T value, [CallerArgumentExpression(nameof(value))] string paramName = null) where T : struct, Enum
    {
        if (!Enum.IsDefined(value))
            throw new ArgumentOutOfRangeException(paramName, value, "Enumeration value out of range.");
    }

    // a version of the existing helper that operates on an array instead
    internal static void ThrowIfNotDefinedInEnum<T>(
        T[] values, [CallerArgumentExpression(nameof(values))] string paramName = null) where T : struct, Enum
    {
        ArgumentNullException.ThrowIfNull(values);

        Array.ForEach(values, v => ThrowIfNotDefinedInEnum(v, paramName));
    }

    // throws if the given time is negative
    internal static void ThrowIfNegative(
        TimeSpan value, [CallerArgumentExpression(nameof(value))] string paramName = null)
    {
        if (value < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(paramName, value, "The given time must be a non-negative value.");
    }

    // throws if the given hours and minutes are outside 00:00 to 23:59
    internal static void ThrowIfOutOfMilitaryTimeRange(
        int hour,
        int minute,
        [CallerArgumentExpression(nameof(hour))]
        string hourParamName = null,
        [CallerArgumentExpression(nameof(minute))]
        string minuteParamName = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(hour, hourParamName);
        ArgumentOutOfRangeException.ThrowIfNegative(minute, minuteParamName);

        ArgumentOutOfRangeException.ThrowIfGreaterThan(hour, 23, hourParamName);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minute, 59, minuteParamName);
    }

    // throws if the hours and minutes component of the timespan are outside 00:00 to 23:59
    internal static void ThrowIfOutOfMilitaryTimeRange(
        TimeSpan value, [CallerArgumentExpression(nameof(value))] string paramName = null)
    {
        ThrowIfNegative(value);

        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Hours, 23, $"{paramName}.Hours");
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Minutes, 59, $"{paramName}.Minutes");
    }

    // a version of the existing helper that operates on an array instead
    internal static void ThrowIfOutOfMilitaryTimeRange(
        TimeSpan[] values, [CallerArgumentExpression(nameof(values))] string paramName = null)
    {
        ArgumentNullException.ThrowIfNull(values);

        Array.ForEach(values, v => ThrowIfOutOfMilitaryTimeRange(v, paramName));
    }

    internal static void ThrowIfEmpty(
        TimeSpan[] values, [CallerArgumentExpression(nameof(values))] string paramName = null)
    {
        ArgumentNullException.ThrowIfNull(values);

        if (values.Length == 0)
            throw new ArgumentException($"\"{paramName}\" can't be empty.");
    }

    public static void ThrowIfOutOfOrder(
        TimeSpan[] values, [CallerArgumentExpression(nameof(values))] string paramName = null)
    {
        var collection = values.ToList();
        var orderedCollection = collection.OrderBy(t => t).ToList();

        for (var i = 0; i < collection.Count; i++)
        {
            if (collection[i] != orderedCollection[i])
                throw new ArgumentException($"\"{paramName}\" is out of order.");
        }
    }
}
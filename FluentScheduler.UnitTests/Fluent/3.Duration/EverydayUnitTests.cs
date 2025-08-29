namespace FluentScheduler.UnitTests;

using System;
using Xunit;
using static Xunit.Assert;

public class EverydayUnitTests
{
    [Fact]
    public void At()
    {
        // Arrange
        var now = new DateTime(2025, 8, 20, 10, 0, 0);
        var expected = new DateTime(2025, 8, 20, 15, 0, 0);

        var fluentCalculator = new FluentTimeCalculator();
        var calculator = (ITimeCalculator)fluentCalculator;
        var run = new EverydayUnit(fluentCalculator);

        calculator.Now = () => now;

        // Act
        run.At(15, 0);
        var calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expected, calculated.Value);

        // Arrange
        now = now.AddHours(6);
        expected = expected.AddDays(1);

        // Act
        calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expected, calculated.Value);
    }

    [Fact]
    public void AtTimeSpan()
    {
        // Arrange
        var now = new DateTime(2025, 8, 20, 10, 0, 0);
        var expected = new DateTime(2025, 8, 20, 15, 0, 0);

        var timeSpan = new TimeSpan(15, 0, 0);

        var fluentCalculator = new FluentTimeCalculator();
        var calculator = (ITimeCalculator)fluentCalculator;
        var run = new EverydayUnit(fluentCalculator);

        calculator.Now = () => now;

        // Act
        run.At(timeSpan);
        var calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expected, calculated.Value);

        // Arrange
        now = now.AddHours(6);
        expected = expected.AddDays(1);

        // Act
        calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expected, calculated.Value);
    }

    [Fact]
    public void AtMultipleTimeSpan()
    {
        // Arrange
        var now = new DateTime(2025, 8, 20, 10, 0, 0);
        var expected = new DateTime(2025, 8, 20, 15, 0, 0);

        var timeSpans = new TimeSpan[] { new(15, 0, 0), new(16, 0, 0), new(17, 0, 0) };

        var fluentCalculator = new FluentTimeCalculator();
        var calculator = (ITimeCalculator)fluentCalculator;
        var run = new EverydayUnit(fluentCalculator);

        calculator.Now = () => now;

        // Act
        run.At(timeSpans);
        var calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expected, calculated.Value);

        // Arrange
        now = now.AddHours(5.1);
        expected = expected.AddHours(1);

        // Act
        calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expected, calculated.Value);

        // Arrange
        now = now.AddHours(1.1);
        expected = expected.AddHours(1);

        // Act
        calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expected, calculated.Value);

        // Arrange
        now = now.AddHours(1.1);
        expected = new DateTime(2025, 8, 21, 15, 0, 0);

        // Act
        calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expected, calculated.Value);
    }

    [Fact]
    public void AtMultipleOutOfOrderTimeSpan()
    {
        // Arrange
        var timeSpans = new TimeSpan[] { new(16, 0, 0), new(15, 0, 0), new(17, 0, 0) };

        var fluentCalculator = new FluentTimeCalculator();
        var run = new EverydayUnit(fluentCalculator);

        // Act Assert
        Throws<ArgumentException>(() => run.At(timeSpans));
    }

    [Fact]
    public void AtMultipleEmptyTimeSpan()
    {
        var fluentCalculator = new FluentTimeCalculator();
        var run = new EverydayUnit(fluentCalculator);

        // Act Assert
        Throws<ArgumentException>(() => run.At());
    }

    [Fact]
    public void AtMultipleNullTimeSpan()
    {
        // Arrange
        var fluentCalculator = new FluentTimeCalculator();
        var run = new EverydayUnit(fluentCalculator);

        // Act Assert
        Throws<ArgumentNullException>(() => run.At(null));
    }

    [Fact]
    public void Between17and19TimeSpan()
    {
        // Arrange
        var now = new DateTime(2025, 08, 24, 16, 30, 0);
        var expectedDate = new DateTime(2025, 08, 24, 17, 30, 0);

        var from = new TimeSpan(17, 30, 0);
        var to = new TimeSpan(19, 30, 0);

        var fluentCalculator = new FluentTimeCalculator();
        var calculator = (ITimeCalculator)fluentCalculator;
        var run = new RunSpecifier(fluentCalculator);

        calculator.Now = () => now;

        // Act
        run.Everyday().Between(from, to);
        var calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);
    }

    [Fact]
    public void Between21and23TimeSpan()
    {
        // Arrange
        var now = new DateTime(2025, 8, 11, 23, 40, 0);
        var expectedDate = new DateTime(2025, 08, 12, 21, 30, 0);

        var from = new TimeSpan(21, 30, 0);
        var to = new TimeSpan(23, 30, 0);

        var fluentCalculator = new FluentTimeCalculator();
        var calculator = (ITimeCalculator)fluentCalculator;
        var run = new RunSpecifier(fluentCalculator);

        calculator.Now = () => now;

        // Act
        run.Everyday().Between(from, to);

        var calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);
    }

    [Fact]
    public void Between19and22TimeSpan()
    {
        // Arrange
        var now = new DateTime(2025, 08, 19, 23, 50, 0);
        var expectedDate = new DateTime(2025, 08, 20, 19, 30, 0);

        var from = new TimeSpan(19, 30, 0);
        var to = new TimeSpan(22, 30, 0);

        var fluentCalculator = new FluentTimeCalculator();
        var calculator = (ITimeCalculator)fluentCalculator;
        var run = new RunSpecifier(fluentCalculator);

        calculator.Now = () => now;

        // Act
        run.Everyday().Between(from, to);
        var calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);

        now = expectedDate.AddMinutes(5);
        expectedDate = now;

        calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);

        now = new DateTime(2025, 08, 20).Add(to).AddMinutes(1);
        expectedDate = new DateTime(2025, 08, 21).Add(from);

        calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);
    }

    [Fact]
    public void Between23and2TimeSpan()
    {
        // Arrange
        var now = new DateTime(2025, 08, 24, 22, 30, 0);
        var expectedDate = new DateTime(2025, 08, 24, 23, 0, 0);

        var from = new TimeSpan(23, 0, 0);
        var to = new TimeSpan(2, 0, 0);

        var fluentCalculator = new FluentTimeCalculator();
        var calculator = (ITimeCalculator)fluentCalculator;
        var run = new RunSpecifier(fluentCalculator);

        calculator.Now = () => now;

        // Act
        run.Everyday().Between(from, to);
        var calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);

        // Arrange
        now = now.AddHours(1);
        expectedDate = now;

        // Act
        calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);

        // Arrange
        now = now.AddHours(2);
        expectedDate = now;

        // Act
        calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);

        // Arrange
        now = now.AddHours(6);
        expectedDate = new DateTime(2025, 08, 25, 23, 0, 0);

        // Act
        calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);
    }

    [Fact]
    public void Between23and2()
    {
        // Arrange
        var now = new DateTime(2025, 08, 24, 22, 30, 0);
        var expectedDate = new DateTime(2025, 08, 24, 23, 0, 0);

        var fluentCalculator = new FluentTimeCalculator();
        var calculator = (ITimeCalculator)fluentCalculator;
        var run = new RunSpecifier(fluentCalculator);

        calculator.Now = () => now;

        // Act
        run.Everyday().Between(23, 0, 2, 0);
        var calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);

        // Arrange
        now = now.AddHours(1);
        expectedDate = now;

        // Act
        calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);

        // Arrange
        now = now.AddHours(2);
        expectedDate = now;

        // Act
        calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);

        // Arrange
        now = now.AddHours(6);
        expectedDate = new DateTime(2025, 08, 25, 23, 0, 0);

        // Act
        calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);
    }

    [Fact]
    public void Between19and22()
    {
        // Arrange
        var now = new DateTime(2025, 08, 19, 23, 50, 0);
        var expectedDate = new DateTime(2025, 08, 20, 19, 30, 0);

        var fluentCalculator = new FluentTimeCalculator();
        var calculator = (ITimeCalculator)fluentCalculator;
        var run = new RunSpecifier(fluentCalculator);

        calculator.Now = () => now;

        // Act
        run.Everyday().Between(19, 30, 22, 30);
        var calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);

        now = expectedDate.AddMinutes(5);
        expectedDate = now;

        calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);

        now = new DateTime(2025, 08, 20).Add(new TimeSpan(22, 30, 0)).AddMinutes(1);
        expectedDate = new DateTime(2025, 08, 21).Add(new TimeSpan(19, 30, 0));

        calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);
    }
}
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
        var timeSpans = new TimeSpan[] { new(15, 0, 0), new(16, 0, 0), new(17, 0, 0) };
        var now = new DateTime(2025, 8, 20, 10, 0, 0);
        var expected = new DateTime(2025, 8, 20).Add(timeSpans[0]);

        var fluentCalculator = new FluentTimeCalculator();
        var calculator = (ITimeCalculator)fluentCalculator;
        var run = new RunSpecifier(fluentCalculator);

        calculator.Now = () => now;

        // Act
        run.Everyday().At(timeSpans);
        var calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expected, calculated.Value);

        // Arrange
        now = now.AddHours(5.1);
        expected = expected.Date.Add(timeSpans[1]);

        // Act
        calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expected, calculated.Value);

        // Arrange
        now = now.AddHours(1.1);
        expected = expected.Date.Add(timeSpans[2]);

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
    public void AtMultipleTimeSpan2()
    {
        // Arrange
        var timeSpans = new TimeSpan[] { new(15, 0, 0), new(16, 0, 0), new(17, 0, 0) };
        var now = new DateTime(2025, 8, 20, 15, 1, 0);
        var expected = new DateTime(2025, 8, 20).Add(timeSpans[1]);

        var fluentCalculator = new FluentTimeCalculator();
        var calculator = (ITimeCalculator)fluentCalculator;
        var run = new RunSpecifier(fluentCalculator);

        calculator.Now = () => now;

        // Act
        run.Everyday().At(timeSpans);
        var calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expected, calculated.Value);

        // Arrange
        now = now.AddHours(1.1);
        expected = expected.Date.Add(timeSpans[2]);

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
    public void EverydayAt15()
    {
        // Arrange
        var now = new DateTime(2025, 08, 24, 14, 30, 0);
        var expectedDate = new DateTime(2025, 08, 24, 15, 0, 0);

        var fluentCalculator = new FluentTimeCalculator();
        var calculator = (ITimeCalculator)fluentCalculator;
        var run = new RunSpecifier(fluentCalculator);

        calculator.Now = () => now;

        // Act
        run.Everyday().At(15, 0);
        var calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);

        now = expectedDate;

        calculated = calculator.Calculate(expectedDate)!;

        expectedDate = expectedDate.AddDays(1);

        // Assert
        Equal(expectedDate, calculated.Value);
    }

    [Fact]
    public void EverydayAt16()
    {
        // Arrange
        var now = new DateTime(2025, 08, 24, 16, 0, 0);
        var expectedDate = new DateTime(2025, 08, 24, 16, 0, 0);

        var fluentCalculator = new FluentTimeCalculator();
        var calculator = (ITimeCalculator)fluentCalculator;
        var run = new RunSpecifier(fluentCalculator);

        calculator.Now = () => now;

        // Act
        run.Everyday().At(16, 0);
        var calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);

        now = expectedDate;

        calculated = calculator.Calculate(expectedDate)!;

        expectedDate = expectedDate.AddDays(1);

        // Assert
        Equal(expectedDate, calculated.Value);
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
        var now = new DateTime(2025, 09, 10, 23, 40, 0);
        var expectedDate = new DateTime(2025, 09, 10, 21, 30, 0);

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

        expectedDate = expectedDate.AddDays(1);

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

        expectedDate = expectedDate.AddDays(1);

        calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);
    }

    [Fact]
    public void Between19and22()
    {
        // Arrange
        var now = new DateTime(2025, 08, 24, 20, 0, 0);
        var expectedDate = new DateTime(2025, 08, 24, 20, 0, 0);

        var fluentCalculator = new FluentTimeCalculator();
        var calculator = (ITimeCalculator)fluentCalculator;
        var run = new RunSpecifier(fluentCalculator);

        calculator.Now = () => now;

        // Act
        run.Everyday().Between(19, 0, 22, 0);
        var calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);

        calculated = calculator.Calculate(expectedDate)!;

        expectedDate = expectedDate.Date.AddDays(1).Add(new TimeSpan(19, 0, 0));

        // Assert
        Equal(expectedDate, calculated.Value);

        calculated = calculator.Calculate(expectedDate)!;

        expectedDate = expectedDate.AddDays(1);

        // Assert
        Equal(expectedDate, calculated.Value);
    }

    [Fact]
    public void ThrowIfAtOutOfOrder()
    {
        // Arrange
        var fluentCalculator = new FluentTimeCalculator();
        var run = new EverydayUnit(fluentCalculator);

        // Act
        Throws<ArgumentException>(() => run.At(new TimeSpan(10, 0, 0), new TimeSpan(9, 0, 0)));
    }
    
    [Fact]
    public void ThrowIfAtEmpty()
    {
        // Arrange
        var fluentCalculator = new FluentTimeCalculator();
        var run = new EverydayUnit(fluentCalculator);

        // Act
        Throws<ArgumentException>(() => run.At());
    }
}
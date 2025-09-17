namespace FluentScheduler.UnitTests;

using System;
using Xunit;
using static Xunit.Assert;

public class PeriodOnceTests
{
    [Fact]
    public void At()
    {
        // Arrange
        var now = new DateTime(2018, 3, 3, 6, 53, 0);
        var expected = new DateTime(2018, 3, 3, 8, 40, 0);

        var fluentCalculator = new FluentTimeCalculator();
        var calculator = (ITimeCalculator)fluentCalculator;
        var run = new PeriodOnceSet(fluentCalculator);

        calculator.Now = () => now;

        // Act
        run.At(8, 40);
        var calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expected, calculated.Value);
    }

    [Fact]
    public void AtTimeSpan()
    {
        // Arrange
        var now = new DateTime(2018, 3, 3, 10, 0, 0);
        var expected = new DateTime(2018, 3, 3, 12, 30, 0);

        var timeSpan = new TimeSpan(12, 30, 0);

        var fluentCalculator = new FluentTimeCalculator();
        var calculator = (ITimeCalculator)fluentCalculator;
        var run = new PeriodOnceSet(fluentCalculator);

        calculator.Now = () => now;

        // Act
        run.At(timeSpan);
        var calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expected, calculated.Value);
    }

    [Fact]
    public void ThrowIfAtEmpty()
    {
        // Arrange
        var fluentCalculator = new FluentTimeCalculator();
        var run = new PeriodOnceSet(fluentCalculator);

        // Act
        Throws<ArgumentException>(() => run.At());
    }

    [Fact]
    public void ThrowIfAtOutOfOrder()
    {
        // Arrange
        var fluentCalculator = new FluentTimeCalculator();
        var run = new PeriodOnceSet(fluentCalculator);

        // Act
        Throws<ArgumentException>(() => run.At(new TimeSpan(10, 0, 0), new TimeSpan(9, 0, 0)));
    }

    [Fact]
    public void InTheNextMonth()
    {
        // Arrange
        var now = new DateTime(2019, 02, 11, 0, 30, 0);
        var expectedDate = new DateTime(2019, 03, 10, 11, 30, 0);

        var fluentCalculator = new FluentTimeCalculator();
        var calculator = (ITimeCalculator)fluentCalculator;
        var run = new RunSpecifier(fluentCalculator);

        calculator.Now = () => now;

        // Act
        run.Every(1).Months().On(10).At(11, 30);
        var calculated = calculator.Calculate(now)!;

        // Assert
        Equal(expectedDate, calculated.Value);
    }
}
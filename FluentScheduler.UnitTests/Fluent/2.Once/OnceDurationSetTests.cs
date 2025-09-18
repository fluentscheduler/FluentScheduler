namespace FluentScheduler.UnitTests;

using System;
using Xunit;
using static Xunit.Assert;

public class OnceDurationSetTests
{

    [Fact]
    public void Minutes()
    {
        // Arrange
        var date = new DateTime(2018, 2, 16);
        var expected = new DateTime(2018, 2, 16, 0, 3, 0);

        var calculator = new FluentTimeCalculator();
        var run = new OnceDurationSet(3, calculator);

        // Act
        run.Minutes();
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void Hours()
    {
        // Arrange
        var date = new DateTime(2018, 2, 16);
        var expected = new DateTime(2018, 2, 16, 3, 0, 0);

        var calculator = new FluentTimeCalculator();
        var run = new OnceDurationSet(3, calculator);

        // Act
        run.Hours();
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void Days()
    {
        // Arrange
        var date = new DateTime(2018, 2, 16);
        var expected = new DateTime(2018, 2, 19);

        var calculator = new FluentTimeCalculator();
        var run = new OnceDurationSet(3, calculator);

        // Act
        run.Days();
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void Weeks()
    {
        // Arrange
        var date = new DateTime(2018, 2, 16);
        var expected = new DateTime(2018, 3, 9);

        var calculator = new FluentTimeCalculator();
        var run = new OnceDurationSet(3, calculator);

        // Act
        run.Weeks();
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);
    }

    [Fact]
    public void Months()
    {
        // Arrange
        var date = new DateTime(2018, 3, 16);
        var expected = new DateTime(2018, 6, 16);

        var calculator = new FluentTimeCalculator();
        var run = new OnceDurationSet(3, calculator);

        // Act
        run.Months();
        var calculated = calculator.Calculate(date);

        // Assert
        Equal(expected, calculated);
    }
}
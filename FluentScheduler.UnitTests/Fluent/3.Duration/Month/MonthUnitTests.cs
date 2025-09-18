namespace FluentScheduler.UnitTests;

using System;
using Xunit;
using static Xunit.Assert;

public class MonthUnitTests
{
    [Fact]
    public void On()
    {
        // Arrange
        var now = new DateTime(2020, 9, 2);
        var expected = new DateTime(2020, 9, 2);

        var calculator = new FluentTimeCalculator();
        var monthUnit = new MonthUnit(calculator);

        // Act
        monthUnit.On(2);
        var calculated = calculator.Calculate(now);

        // Assert
        Equal(expected, calculated);
    }
}

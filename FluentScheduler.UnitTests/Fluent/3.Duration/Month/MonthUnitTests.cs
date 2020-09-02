namespace FluentScheduler.UnitTests
{
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

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var monthUnit = new MonthUnit(fluentCalculator);

            // Act
            monthUnit.On(2);
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(expected, calculated);
        }
    }
}

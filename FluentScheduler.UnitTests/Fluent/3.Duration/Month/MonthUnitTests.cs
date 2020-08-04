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
            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            var now = DateTime.Now;
            var expectedDay = 3;

            // Act
            run.Every(1).Months().On(expectedDay);
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(expectedDay, calculated.Value.Day);
        }
    }
}

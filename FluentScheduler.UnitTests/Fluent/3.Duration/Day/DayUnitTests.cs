namespace FluentScheduler.UnitTests
{
    using System;
    using Xunit;
    using static Xunit.Assert;

    public class DayUnitTests
    {
        [Fact]
        public void EveryWeekday()
        {
            // Arrange
            var sunday = new DateTime(2018, 02, 18);
            var saturday = new DateTime(2018, 02, 17);

            var monday = new DateTime(2018, 02, 19);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new DayUnit(fluentCalculator);

            // Act
            run.Weekday();
            var calculated = calculator.Calculate(sunday);

            // Assert
            var expected = new DateTime(2018, 02, 19);
            Equal(expected, calculated);

            // Act
            calculated = calculator.Calculate(saturday);

            // Assert
            Equal(expected, calculated);

            // Arrange
            expected = new DateTime(2018, 02, 20);

            // Act
            calculated = calculator.Calculate(monday);

            // Assert
            Equal(expected, calculated);
        }

        [Fact]
        public void EveryWeekend()
        {
            // Arrange
            var tuesday = new DateTime(2018, 02, 13);
            var saturday = new DateTime(2018, 02, 17);
            var sunday = new DateTime(2018, 02, 18);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new DayUnit(fluentCalculator);

            // Act
            run.Weekend();
            var calculated = calculator.Calculate(tuesday);

            // Assert
            var expected = new DateTime(2018, 02, 17);
            Equal(expected, calculated);

            // Act
            calculated = calculator.Calculate(saturday);

            // Assert
            Equal(expected, calculated);

            // Act
            calculated = calculator.Calculate(sunday);

            // Assert
            expected = new DateTime(2018, 02, 18);
            Equal(expected, calculated);
        }
    }
}
namespace FluentScheduler.UnitTests
{
    using System;
    using Xunit;
    using static Xunit.Assert;

    public class OnceSetTests
    {

        [Fact]
        public void AndEveryDayOfWeek()
        {
            // Arrange
            var friday = new DateTime(2018, 2, 16);
            var monday = new DateTime(2018, 2, 19);
            var sunday = new DateTime(2018, 2, 25);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new OnceSet(fluentCalculator);

            // Act
            run.AndEvery(DayOfWeek.Sunday);
            var calculated = calculator.Calculate(friday);

            // Assert
            var expected = new DateTime(2018, 2, 18);
            Equal(expected, calculated);

            //Act
            calculated = calculator.Calculate(monday);

            // Assert
            expected = new DateTime(2018, 2, 25);
            Equal(expected, calculated);

            // Act
            calculated = calculator.Calculate(sunday);

            // Assert
            Equal(sunday, calculated);

        }

        [Fact]
        public void EveryInterval()
        {
            // Arrange
            var date = new DateTime(2018, 12, 23);
            var expected = date.AddSeconds(10);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new OnceSet(fluentCalculator);

            // Act
            run.AndEvery(10).Seconds();
            var calculated = calculator.Calculate(date);

            // Assert
            Equal(expected, calculated);
        }

        [Fact]
        public void AndEveryWeekday()
        {
            // Arrange
            var date = new DateTime(2018, 12, 22);
            var expected = new DateTime(2018, 12, 24);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new OnceSet(fluentCalculator);

            // Act
            run.AndEveryWeekday();
            var calculated = calculator.Calculate(date);

            // Assert
            Equal(expected, calculated);
        }

        [Fact]
        public void AndEveryWeekend()
        {
            // Arrange
            var date = new DateTime(2018, 12, 24);
            var expected = new DateTime(2018, 12, 29);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new OnceSet(fluentCalculator);

            // Act
            run.AndEveryWeekend();
            var calculated = calculator.Calculate(date);

            // Assert
            Equal(expected, calculated);
        }
    }
}
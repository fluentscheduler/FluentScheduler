namespace FluentScheduler.UnitTests
{
    using System;
    using Xunit;
    using static Xunit.Assert;

    public class PeriodDurationSetTests
    {
        [Fact]
        public void Seconds()
        {
            // Arrange
            var now = DateTime.Now;

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(5).Seconds();
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(now.AddSeconds(5).Second, calculated.Value.Second);
        }

        [Fact]
        public void Minutes()
        {
            // Arrange
            var now = DateTime.Now;

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(5).Minutes();
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(now.AddMinutes(5).Minute, calculated.Value.Minute);
        }

        [Fact]
        public void Hours()
        {
            // Arrange
            var now = DateTime.Now;

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(5).Hours();
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(now.AddHours(5).Hour, calculated.Value.Hour);
        }

        [Fact]
        public void Days()
        {
            // Arrange
            var now = DateTime.Now;

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(5).Days();
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(now.AddDays(5).Day, calculated.Value.Day);
        }

        [Fact]
        public void Weeks()
        {
            // Arrange
            var now = DateTime.Now;

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(5).Weeks();
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(now.AddDays(35), calculated.Value);
        }

        [Fact]
        public void Months()
        {
            // Arrange
            var now = DateTime.Now;

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(5).Months();
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(now.AddMonths(5).Month, calculated.Value.Month);
        }
    }
}
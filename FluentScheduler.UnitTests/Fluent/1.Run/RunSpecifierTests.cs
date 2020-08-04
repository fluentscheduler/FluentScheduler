namespace FluentScheduler.UnitTests
{
    using System;
    using Xunit;
    using static Xunit.Assert;

    public class RunSpecifierTests
    {
        [Fact]
        public void NowAndEvery()
        {
            // Arrange
            var now = DateTime.Now;

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Now().AndEvery(10).Seconds();
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(now, calculated);

            // Act
            calculated = calculator.Calculate(now);

            // Assert
            Equal(now.AddSeconds(10), calculated);
        }

        [Fact]
        public void EveryTimeSpan()
        {
            // Arrange
            var now = DateTime.Now;
            var time = new TimeSpan(1, 3, 0);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(time);
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(now.Add(time), calculated);
        }

        [Fact]
        public void EveryDayOfWeek()
        {
            // Arrange
            var date = new DateTime(2018, 12, 23);
            var expected = new DateTime(2018, 12, 28);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(DayOfWeek.Friday);
            var calculated = calculator.Calculate(date);

            // Assert
            Equal(expected, calculated);
        }

        [Fact]
        public void OnceAtAndEvery()
        {
            // Arrange
            var now = DateTime.Now;
            var at = new DateTime(2000, 10, 10, 10, 10, 10);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.OnceAt(at).AndEvery(10).Seconds();
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(at, calculated);

            // Act
            calculated = calculator.Calculate(now);

            // Assert
            Equal(now.AddSeconds(10), calculated);
        }

        [Fact]
        public void Now()
        {
            // Arrange
            var now = DateTime.Now;

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Now();
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(now, calculated);

            // Act
            calculated = calculator.Calculate(now);

            // Assert
            Null(calculated);
        }

        [Fact]
        public void OnceAtHoursMinutes()
        {
            // Arrange
            var now = DateTime.Today;
            var hours = 13;
            var minutes = 50;

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.OnceAt(hours, minutes);
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(now.Date.AddHours(hours).AddMinutes(minutes), calculated);

            // Act
            calculated = calculator.Calculate(now);

            // Assert
            Null(calculated);
        }

        [Fact]
        public void OnceAtTimeSpan()
        {
            // Arrange
            var now = DateTime.Today;
            var at = new TimeSpan(13, 50, 0);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.OnceAt(at);
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(now.Add(at), calculated);

            // Act
            calculated = calculator.Calculate(now);

            // Assert
            Null(calculated);
        }

        [Fact]
        public void OnceAtDateTime()
        {
            // Arrange
            var now = DateTime.Now;
            var at = new DateTime(2000, 10, 10, 10, 10, 10);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.OnceAt(at);
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(at, calculated);

            // Act
            calculated = calculator.Calculate(now);

            // Assert
            Null(calculated);
        }

        [Fact]
        public void OnceIn()
        {
            // Arrange
            var now = DateTime.Today;

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.OnceIn(10).Seconds();
            var actual = calculator.Calculate(now);

            // Assert
            Equal(now.AddSeconds(10), actual);

            // Act
            actual = calculator.Calculate(now);

            // Assert
            Null(actual);
        }

        [Fact]
        public void OnceInTimeSpan()
        {
            // Arrange
            var now = DateTime.Today;
            var time = TimeSpan.FromSeconds(10);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.OnceIn(time);
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(now.Add(time), calculated);

            // Act
            calculated = calculator.Calculate(now);

            // Assert
            Null(calculated);
        }

        [Fact]
        public void EveryWeekday()
        {
            // Arrange
            var now = new DateTime(2018, 08, 31);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.EveryWeekday();
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(now, calculated);

            // Act
            calculated = calculator.Calculate(now.AddDays(1));

            // Assert
            Equal(now.AddDays(3), calculated);
        }

         [Fact]
        public void EveryWeekend()
        {
             // Arrange
            var now = new DateTime(2018, 08, 31);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.EveryWeekend();
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(now.AddDays(1), calculated);

            // Act
            calculated = calculator.Calculate(calculated.Value.AddDays(1));

            // Assert
            Equal(now.AddDays(2), calculated);

            // Act
            calculated = calculator.Calculate(calculated.Value.AddDays(1));

            // Assert
            Equal(now.AddDays(8), calculated);
        }
    }
}
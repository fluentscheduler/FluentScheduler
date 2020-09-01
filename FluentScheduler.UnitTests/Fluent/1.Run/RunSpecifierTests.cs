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
        public void ExecuteOnlyOnceEverySunday()
        {
            // Arrange
            var date = new DateTime(2020, 9, 1);
            var expected = new DateTime(2020, 9, 6);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(DayOfWeek.Sunday);
            var calculated = calculator.Calculate(date);

            // Assert
            Equal(expected, calculated);

            // Arrange
            expected = new DateTime(2020, 9, 13);

            // Act
            calculated = calculator.Calculate((DateTime)calculated);

            // Assert
            Equal(expected, calculated);
        }

        [Fact]
        public void ExecuteOnlyOnceEveryMonday()
        {
            // Arrange
            var date = new DateTime(2020, 9, 1);
            var expected = new DateTime(2020, 9, 7);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(DayOfWeek.Monday);
            var calculated = calculator.Calculate(date);

            // Assert
            Equal(expected, calculated);

            // Arrange
            expected = new DateTime(2020, 9, 14);

            // Act
            calculated = calculator.Calculate((DateTime)calculated);

            // Assert
            Equal(expected, calculated);
        }

        [Fact]
        public void ExecuteOnlyOnceEveryTuesday()

        {
            // Arrange
            var date = new DateTime(2020, 9, 1);
            var expected = new DateTime(2020, 9, 8);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(DayOfWeek.Tuesday);
            var calculated = calculator.Calculate(date);

            // Assert
            Equal(expected, calculated);

            // Arrange
            expected =  new DateTime(2020, 9, 15);

            // Act
            calculated = calculator.Calculate((DateTime)calculated);

            // Assert
            Equal(expected, calculated);
        }

        [Fact]
        public void ExecuteOnlyOnceEveryThursday()
        {
            // Arrange
            var date = new DateTime(2020, 9, 1);
            var expected = new DateTime(2020, 9, 3);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(DayOfWeek.Thursday);
            var calculated = calculator.Calculate(date);

            // Assert
            Equal(expected, calculated);

            // Arrange
            expected =  new DateTime(2020, 9, 10);

            // Act
            calculated = calculator.Calculate((DateTime)calculated);

            // Assert
            Equal(expected, calculated);
        }

        [Fact]
        public void ExecuteOnlyOnceEveryFriday()
        {
            // Arrange
            var date = new DateTime(2020, 9, 1);
            var expected = new DateTime(2020, 9, 4);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(DayOfWeek.Friday);
            var calculated = calculator.Calculate(date);

            // Assert
            Equal(expected, calculated);

            // Arrange
            expected =  new DateTime(2020, 9, 11);

            // Act
            calculated = calculator.Calculate((DateTime)calculated);

            // Assert
            Equal(expected, calculated);
        }

        [Fact]
        public void EverySaturday()
        {
            // Arrange
            var date = new DateTime(2020, 9, 1);
            var expected = new DateTime(2020, 9, 5);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(DayOfWeek.Saturday);
            var calculated = calculator.Calculate(date);

            // Assert
            Equal(expected, calculated);

            // Arrange
            expected =  new DateTime(2020, 9, 12);

            // Act
            calculated = calculator.Calculate((DateTime)calculated);

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
            var expected = new DateTime(2018, 09, 3);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.EveryWeekday();
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(expected, calculated);

            // Act
            calculated = calculator.Calculate(expected);

            // Assert
            Equal(expected.AddDays(1), calculated);
        }

         [Fact]
        public void EveryWeekend()
        {
            // Arrange
            var now = new DateTime(2018, 08, 31);
            var expected = new DateTime(2018, 09, 01);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.EveryWeekend();
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(expected, calculated);

            // Arrange
            now = new DateTime(2018, 09, 01);
            expected = new DateTime(2018, 09, 02);

            // Act
            calculated = calculator.Calculate(now);

            // Assert
            Equal(expected, calculated);

            // Arrange
            now = new DateTime(2018, 09, 02);
            expected = new DateTime(2018, 09, 08);

            // Act
            calculated = calculator.Calculate(now);

            // Assert
            Equal(expected, calculated);
        }
    }
}

namespace FluentScheduler.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class RunSpecifierTests
    {
        [TestMethod]
        public void NowAndEvery()
        {
            // Arrange
            var now = DateTime.Now;

            var calculator = new FluentTimeCalculator();
            var run = new RunSpecifier(calculator);

            // Act
            run.Now().AndEvery(10).Seconds();
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now, calculated);

            // Act
            calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now.AddSeconds(10), calculated);
        }

        [TestMethod]
        public void OnceAtAndEvery()
        {
            // Arrange
            var now = DateTime.Now;
            var at = new DateTime(2000, 10, 10, 10, 10, 10);

            var calculator = new FluentTimeCalculator();
            var run = new RunSpecifier(calculator);

            // Act
            run.OnceAt(at).AndEvery(10).Seconds();
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(at, calculated);

            // Act
            calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now.AddSeconds(10), calculated);
        }

        [TestMethod]
        public void Now()
        {
            // Arrange
            var now = DateTime.Now;

            var calculator = new FluentTimeCalculator();
            var run = new RunSpecifier(calculator);

            // Act
            run.Now();
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now, calculated);

            // Act
            calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(null, calculated);
        }

        [TestMethod]
        public void OnceAtHoursMinutes()
        {
            // Arrange
            var now = DateTime.Today;
            var hours = 13;
            var minutes = 50;

            var calculator = new FluentTimeCalculator();
            var run = new RunSpecifier(calculator);

            // Act
            run.OnceAt(hours, minutes);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now.Date.AddHours(hours).AddMinutes(minutes), calculated);

            // Act
            calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(null, calculated);
        }

        [TestMethod]
        public void OnceAtTimeSpan()
        {
            // Arrange
            var now = DateTime.Today;
            var at = new TimeSpan(13, 50, 0);

            var calculator = new FluentTimeCalculator();
            var run = new RunSpecifier(calculator);

            // Act
            run.OnceAt(at);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now.Add(at), calculated);

            // Act
            calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(null, calculated);
        }

        [TestMethod]
        public void OnceAtDateTime()
        {
            // Arrange
            var now = DateTime.Now;
            var at = new DateTime(2000, 10, 10, 10, 10, 10);

            var calculator = new FluentTimeCalculator();
            var run = new RunSpecifier(calculator);

            // Act
            run.OnceAt(at);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(at, calculated);

            // Act
            calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(null, calculated);
        }

        [TestMethod]
        public void OnceIn()
        {
            // Arrange
            var now = DateTime.Today;

            var calculator = new FluentTimeCalculator();
            var run = new RunSpecifier(calculator);

            // Act
            run.OnceIn(10).Seconds();
            var actual = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now.AddSeconds(10), actual);

            // Act
            actual = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void OnceInTimeSpan()
        {
            // Arrange
            var now = DateTime.Today;
            var time = TimeSpan.FromSeconds(10);

            var calculator = new FluentTimeCalculator();
            var run = new RunSpecifier(calculator);

            // Act
            run.OnceIn(time);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now.Add(time), calculated);

            // Act
            calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(null, calculated);
        }

        [TestMethod]
        public void EveryWeekday()
        {
            // Arrange
            var now = new DateTime(2018, 08, 31);

            var calculator = new FluentTimeCalculator();
            var run = new RunSpecifier(calculator);

            // Act
            run.EveryWeekday();
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now, calculated);

            // Act
            calculated = calculator.Calculate(now.AddDays(1));

            // Assert
            Assert.AreEqual(now.AddDays(3), calculated);
        }

         [TestMethod]
        public void EveryWeekend()
        {
             // Arrange
            var now = new DateTime(2018, 08, 31);

            var calculator = new FluentTimeCalculator();
            var run = new RunSpecifier(calculator);

            // Act
            run.EveryWeekend();
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now.AddDays(1), calculated);

            // Act
            calculated = calculator.Calculate(calculated.Value.AddDays(1));

            // Assert
            Assert.AreEqual(now.AddDays(2), calculated);

            // Act
            calculated = calculator.Calculate(calculated.Value.AddDays(1));

            // Assert
            Assert.AreEqual(now.AddDays(8), calculated);
        }
    }
}
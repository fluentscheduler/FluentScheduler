namespace FluentScheduler.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class OnceSetTests
    {
        [TestMethod]
        public void Now()
        {
            // Arrange
            var now = DateTime.Now;

            var calculator = new TimeCalculator();
            var run = new RunSpecifier(calculator);

            // Act
            run.Now();

            // Assert
            DateTime? expected = now;
            var actual = calculator.Calculate(now);

            Assert.AreEqual(expected, actual);

            actual = calculator.Calculate(now);
            expected = null;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OnceAtHoursMinutes()
        {
            // Arrange
            var hours = 13;
            var minutes = 50;
            var now = new DateTime(2000, 10, 10, 10, 10, 10);

            var calculator = new TimeCalculator();
            var run = new RunSpecifier(calculator);

            // Act
            run.OnceAt(hours, minutes);

            // Assert
            DateTime? expected = now.Date.AddHours(hours).AddMinutes(minutes);
            var actual = calculator.Calculate(now);

            Assert.AreEqual(expected, actual);

            expected = null;
            actual = calculator.Calculate(now);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OnceAtTimeSpan()
        {
            // Arrange
            var time = new TimeSpan(13, 50, 0);
            var now = new DateTime(2000, 10, 10, 10, 10, 10);

            var calculator = new TimeCalculator();
            var run = new RunSpecifier(calculator);

            // Act
            run.OnceAt(time);

            // Assert
            DateTime? expected = now.Date.Add(time);
            var actual = calculator.Calculate(now);

            Assert.AreEqual(expected, actual);

            expected = null;
            actual = calculator.Calculate(now);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OnceAtDateTime()
        {
            // Arrange
            var dateTime = new DateTime(2000, 10, 10, 10, 10, 10);
            var calculator = new TimeCalculator();
            var run = new RunSpecifier(calculator);

            // Act
            run.OnceAt(dateTime);

            // Assert
            DateTime? expected = dateTime;
            var actual = calculator.Calculate(DateTime.Now);

            Assert.AreEqual(expected, actual);

            expected = null;
            actual = calculator.Calculate(DateTime.Now);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OnceIn()
        {
            // Arrange
            var now = new DateTime(2000, 10, 10, 10, 10, 10);

            var calculator = new TimeCalculator();
            var run = new RunSpecifier(calculator);

            // Act
            run.OnceIn(10).Seconds();

            // Assert
            DateTime? expected = now.AddSeconds(10);
            var actual = calculator.Calculate(now);

            Assert.AreEqual(expected, actual);

            expected = null;
            actual = calculator.Calculate(now);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OnceInTimeSpan()
        {
            // Arrange
            var time = TimeSpan.FromSeconds(10);
            var now = new DateTime(2000, 10, 10, 10, 10, 10);

            var calculator = new TimeCalculator();
            var run = new RunSpecifier(calculator);

            // Act
            run.OnceIn(time);

            // Assert
            DateTime? expected = now.Add(time);
            var actual = calculator.Calculate(now);

            Assert.AreEqual(expected, actual);

            expected = null;
            actual = calculator.Calculate(now);

            Assert.AreEqual(expected, actual);
        }
    }
}

namespace FluentScheduler.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class PeriodDurationSetTests
    {
        [TestMethod]
        public void NowAndEvery()
        {
            // Arrange
            var now = DateTime.Now;

            var calculator = new TimeCalculator();
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

            var calculator = new TimeCalculator();
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
    }
}

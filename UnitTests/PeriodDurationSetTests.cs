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

            // Assert
            DateTime? expected = now;
            var actual = calculator.Calculate(now);

            Assert.AreEqual(expected, actual);

            actual = calculator.Calculate(now);
            expected = expected.Value.AddSeconds(10);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OnceAtAndEvery()
        {
            // Arrange
            var now = new DateTime(2000, 10, 10, 10, 10, 10);
            var calculator = new TimeCalculator();
            var run = new RunSpecifier(calculator);

            // Act
            run.OnceAt(now).AndEvery(10).Seconds();

            // Assert
            DateTime? expected = now;
            var actual = calculator.Calculate(DateTime.Now);

            Assert.AreEqual(expected, actual);

            expected = expected.Value.AddSeconds(10);
            actual = calculator.Calculate(now);

            Assert.AreEqual(expected, actual);
        }
    }
}

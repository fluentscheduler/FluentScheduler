namespace FluentScheduler.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class UtcSetTests
    {
        [TestMethod]
        public void UseUtcFluent()
        {
            // Arrange
            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new PeriodOnceSet(fluentCalculator);

            var utc = DateTime.UtcNow;

            // Act
            run.At(utc.Hour, utc.Minute).UseUtc();
            var calculated = calculator.Calculate(utc);

            // Assert
            Assert.AreEqual(utc.Hour, calculated.Value.Hour);
            Assert.AreEqual(utc.Minute, calculated.Value.Minute);
        }

        [TestMethod]
        public void NotUsingUtcFluent()
        {
            // Arrange
            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new PeriodOnceSet(fluentCalculator);

            var now = DateTime.Now;

            // Act
            run.At(now.Hour, now.Minute);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now.Hour, calculator.Now().Hour);
            Assert.AreEqual(now.Minute, calculator.Now().Minute);
        }

        [TestMethod]
        public void UseUtcCron()
        {
            // Arrange
            var cronTimeCalculator = new CronTimeCalculator("* * * * *");
            var calculator = (ITimeCalculator)cronTimeCalculator;

            var utc = DateTime.UtcNow;

            // Act
            cronTimeCalculator.UseUtc();

            // Assert
            Assert.AreEqual(utc.Hour, calculator.Now().Hour);
            Assert.AreEqual(utc.Minute, calculator.Now().Minute);
        }

        [TestMethod]
        public void NotUsingUtcCron()
        {
            // Arrange
            var cronTimeCalculator = new CronTimeCalculator("* * * * *");
            var calculator = (ITimeCalculator)cronTimeCalculator;

            var now = DateTime.Now;

            // Assert
            Assert.AreEqual(now.Hour, calculator.Now().Hour);
            Assert.AreEqual(now.Minute, calculator.Now().Minute);
        }
    }
}
        
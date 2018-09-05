namespace FluentScheduler.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class PeriodOnceTests
    {
        [TestMethod]
        public void At()
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
            Assert.AreEqual(now.Hour, calculated.Value.Hour);
            Assert.AreEqual(now.Minute, calculated.Value.Minute);
        }

        [TestMethod]
        public void AtTimeSpan()
        {
            // Arrange
            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new PeriodOnceSet(fluentCalculator);

            var now = DateTime.Now;
            var timeSpan = new TimeSpan(12, 30, 0);

            // Act
            run.At(timeSpan);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(timeSpan.Hours, calculated.Value.Hour);
            Assert.AreEqual(timeSpan.Minutes, calculated.Value.Minute);
        }
    }
}
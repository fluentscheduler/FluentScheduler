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
            var calculator = new TimeCalculator();
            var run = new PeriodOnceSet(calculator);

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
            var calculator = new TimeCalculator();
            var run = new PeriodOnceSet(calculator);

            var now = DateTime.Now;
            var timeSpan = new TimeSpan(12, 30, 0);

            // Act
            run.At(timeSpan);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now.Hour, calculated.Value.Hour);
            Assert.AreEqual(now.Minute, calculated.Value.Minute);
        }
    }
}

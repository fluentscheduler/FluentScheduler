namespace FluentScheduler.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class MonthOnDayofWeekUnitTestscs
    {
        [TestMethod]
        public void At()
        {
            // Arrange
            var calculator = new TimeCalculator();
            var run = new TimeSet(calculator);

            var now = DateTime.Now;

            // Act
            run.At(now.Hour, now.Minute);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now.AddHours(now.Hour).Hour, calculated.Value.Hour);
            Assert.AreEqual(now.AddMinutes(now.Minute).Minute, calculated.Value.Minute);
        }

        [TestMethod]
        public void AtTimeSpan()
        {
            // Arrange
            var calculator = new TimeCalculator();
            var run = new TimeSet(calculator);

            var now = DateTime.Now;
            var timeSpan = new TimeSpan(12, 30, 0);

            // Act
            run.At(timeSpan);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now.AddHours(timeSpan.Hours).Hour, calculated.Value.Hour);
            Assert.AreEqual(now.AddMinutes(timeSpan.Minutes).Minute, calculated.Value.Minute);
        }
    }
}

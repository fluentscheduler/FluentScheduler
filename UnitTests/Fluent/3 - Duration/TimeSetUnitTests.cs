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
            var run = new RunSpecifier(calculator);

            var now = DateTime.Now;

            // Act
            run.Every(1).Months().OnTheFirstDay(DayOfWeek.Friday).At(now.Hour, now.Minute);
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
            var run = new RunSpecifier(calculator);

            var now = DateTime.Now;
            var timeSpan = new TimeSpan(12, 30, 0);

            // Act
            run.Every(1).Months().OnTheFirstDay(DayOfWeek.Friday).At(timeSpan);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(timeSpan.Hours, calculated.Value.Hour);
            Assert.AreEqual(timeSpan.Minutes, calculated.Value.Minute);
        }
    }
}

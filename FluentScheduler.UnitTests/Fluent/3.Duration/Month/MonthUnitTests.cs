namespace FluentScheduler.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class MonthUnitTests
    {
        [TestMethod]
        public void On()
        {
            // Arrange
            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            var now = DateTime.Now;
            var expectedDay = 3;

            // Act
            run.Every(1).Months().On(expectedDay);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(expectedDay, calculated.Value.Day);
        }
    }
}

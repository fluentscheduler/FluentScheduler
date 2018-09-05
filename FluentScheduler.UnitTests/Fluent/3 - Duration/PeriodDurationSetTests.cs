namespace FluentScheduler.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class PeriodDurationSetTests
    {

        [TestMethod]
        public void Seconds()
        {
            // Arrange
            var now = DateTime.Now;

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(5).Seconds();
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now.AddSeconds(5).Second, calculated.Value.Second);
        }

        [TestMethod]
        public void Minutes()
        {
            // Arrange
            var now = DateTime.Now;

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(5).Minutes();
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now.AddMinutes(5).Minute, calculated.Value.Minute);
        }

        [TestMethod]
        public void Hours()
        {
            // Arrange
            var now = DateTime.Now;

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(5).Hours();
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now.AddHours(5).Hour, calculated.Value.Hour);
        }

        [TestMethod]
        public void Days()
        {
            // Arrange
            var now = DateTime.Now;

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(5).Days();
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now.AddDays(5).Day, calculated.Value.Day);
        }

        [TestMethod]
        public void Weeks()
        {
            // Arrange
            var now = DateTime.Now;

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(5).Weeks();
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now.AddDays(35), calculated.Value);
        }

        [TestMethod]
        public void Months()
        {
            // Arrange
            var now = DateTime.Now;

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            // Act
            run.Every(5).Months();
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(now.AddMonths(5).Month, calculated.Value.Month);
        }
    }
}
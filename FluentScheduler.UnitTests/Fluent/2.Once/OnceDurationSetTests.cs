namespace FluentScheduler.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class OnceDurationSetTests
    {

        [TestMethod]
        public void Minutes()
        {
            // Arrange
            var date = new DateTime(2018, 2, 16);
            var expected = new DateTime(2018, 2, 16, 0, 3, 0);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new OnceDurationSet(3, fluentCalculator);

            // Act
            run.Minutes();
            var calculated = calculator.Calculate(date);

            // Assert
            Assert.AreEqual(expected, calculated);
        }

        [TestMethod]
        public void Hours()
        {
            // Arrange
            var date = new DateTime(2018, 2, 16);
            var expected = new DateTime(2018, 2, 16, 3, 0, 0);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new OnceDurationSet(3, fluentCalculator);

            // Act
            run.Hours();
            var calculated = calculator.Calculate(date);

            // Assert
            Assert.AreEqual(expected, calculated);
        }

        [TestMethod]
        public void Days()
        {
            // Arrange
            var date = new DateTime(2018, 2, 16);
            var expected = new DateTime(2018, 2, 19);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new OnceDurationSet(3, fluentCalculator);

            // Act
            run.Days();
            var calculated = calculator.Calculate(date);

            // Assert
            Assert.AreEqual(expected, calculated);
        }

        [TestMethod]
        public void Weeks()
        {
            // Arrange
            var date = new DateTime(2018, 2, 16);
            var expected = new DateTime(2018, 3, 9);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new OnceDurationSet(3, fluentCalculator);

            // Act
            run.Weeks();
            var calculated = calculator.Calculate(date);

            // Assert
            Assert.AreEqual(expected, calculated);
        }

        [TestMethod]
        public void Months()
        {
            // Arrange
            var date = new DateTime(2018, 3, 16);
            var expected = new DateTime(2018, 6, 16);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new OnceDurationSet(3, fluentCalculator);

            // Act
            run.Months();
            var calculated = calculator.Calculate(date);

            // Assert
            Assert.AreEqual(expected, calculated);
        }
    }
}
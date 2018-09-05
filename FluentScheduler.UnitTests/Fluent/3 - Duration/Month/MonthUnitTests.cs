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

        [TestMethod]
        public void OnTheFirstDay()
        {
            // Arrange
            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            var now = new DateTime(2017, 10, 27);
            var expectedDate = new DateTime(2017, 11, 3);

            // Act
            run.Every(1).Months().OnTheFirstDay(DayOfWeek.Friday);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(DayOfWeek.Friday, calculated.Value.DayOfWeek);
            Assert.AreEqual(expectedDate, calculated.Value.Date);
        }

        [TestMethod]
        public void OnTheSecondDay()
        {
            // Arrange
            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            var now = new DateTime(2017, 10, 27);
            var expectedDate = new DateTime(2017, 11, 10);

            // Act
            run.Every(1).Months().OnTheSecondDay(DayOfWeek.Friday);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(DayOfWeek.Friday, calculated.Value.DayOfWeek);
            Assert.AreEqual(expectedDate, calculated.Value.Date);
        }

        [TestMethod]
        public void OnTheThirdDay()
        {
            // Arrange
            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            var now = new DateTime(2017, 10, 27);
            var expectedDate = new DateTime(2017, 11, 17);

            // Act
            run.Every(1).Months().OnTheThirdDay(DayOfWeek.Friday);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(DayOfWeek.Friday, calculated.Value.DayOfWeek);
            Assert.AreEqual(expectedDate, calculated.Value.Date);
        }

        [TestMethod]
        public void OnTheFourthDay()
        {
            // Arrange
            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            var now = new DateTime(2017, 10, 27);
            var expectedDate = new DateTime(2017, 11, 24);

            // Act
            run.Every(1).Months().OnTheFourthDay(DayOfWeek.Friday);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(DayOfWeek.Friday, calculated.Value.DayOfWeek);
            Assert.AreEqual(expectedDate, calculated.Value.Date);
        }
    }
}
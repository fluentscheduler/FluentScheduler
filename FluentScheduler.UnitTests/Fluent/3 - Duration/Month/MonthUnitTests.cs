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

        [TestMethod]
        public void LastWeekdayBefore()
        {
            // Arrange
            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            var now = new DateTime(2018, 10, 20);
            var expectedDate = new DateTime(2018, 10, 18);
            var dayOfWeek = DayOfWeek.Thursday;

            // Act
            run.Every(0).Months().LastWeekdayBefore(dayOfWeek, 20);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(dayOfWeek, calculated.Value.DayOfWeek);
            Assert.AreEqual(expectedDate, calculated.Value.Date);
        }

        [TestMethod]
        public void LastWeekdayBeforeAt()
        {
            // Arrange
            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            var now = new DateTime(2018, 10, 20);
            var expectedDate = new DateTime(2018, 10, 18, 9, 15, 0);
            var dayOfWeek = DayOfWeek.Thursday;

            // Act
            run.Every(0).Months().LastWeekdayBefore(dayOfWeek, 20).At(9, 15);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(dayOfWeek, calculated.Value.DayOfWeek);
            Assert.AreEqual(expectedDate, calculated.Value);
        }

        [TestMethod]
        public void FirstWeekdayAfter()
        {
            // Arrange
            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            var now = new DateTime(2018, 10, 20);
            var expectedDate = new DateTime(2018, 10, 25);
            var dayOfWeek = DayOfWeek.Thursday;

            // Act
            run.Every(0).Months().FirstWeekdayAfter(dayOfWeek, 20);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(dayOfWeek, calculated.Value.DayOfWeek);
            Assert.AreEqual(expectedDate, calculated.Value.Date);
        }

        [TestMethod]
        public void FirstWeekdayAfterAt()
        {
            // Arrange
            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            var now = new DateTime(2018, 10, 20);
            var expectedDate = new DateTime(2018, 10, 25, 9, 15, 0);
            var dayOfWeek = DayOfWeek.Thursday;

            // Act
            run.Every(0).Months().FirstWeekdayAfter(dayOfWeek, 20).At(9, 15);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(dayOfWeek, calculated.Value.DayOfWeek);
            Assert.AreEqual(expectedDate, calculated.Value);
        }
    }
}
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

            var now = new DateTime(2018, 3, 3, 6, 53, 0);
            var expected = new DateTime(2018, 3, 3, 8, 40, 0);

            calculator.Now = () => now;

            // Act
            run.At(8, 40);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(expected, calculated.Value);
        }

        [TestMethod]
        public void AtTimeSpan()
        {
            // Arrange
            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new PeriodOnceSet(fluentCalculator);

            var now = new DateTime(2018, 3, 3, 10, 0 ,0);
            var timeSpan = new TimeSpan(12, 30, 0);
            var expected = new DateTime(2018, 3, 3, 12, 30, 0);

            calculator.Now = () => now;

            // Act
            run.At(timeSpan);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(expected, calculated.Value);
        }

        [TestMethod]
        public void EarlierDateParamsTimeSpan()
        {
            // Arrange
            var now = new DateTime(2019, 2, 8, 3, 0, 0);
            var last = new DateTime(2019, 2, 8, 1, 0 ,0);

            var time1 = new TimeSpan(5, 0, 0);
            var time2 = new TimeSpan(8, 0, 0);
            var time3 = new TimeSpan(10, 0, 0);

            var expectedFirstRun = new TimeSpan(5, 0, 0);
            var expectedSecondRun = new TimeSpan(8, 0, 0);
            var expectedThirdRun = new TimeSpan(10, 0, 0);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;

            calculator.Now = () => now;

            var run = new PeriodOnceSet(fluentCalculator);

            // Act
            var firstRun = run.EarlierDate(last, time1, time2, time3);

            // Assert
            Assert.AreEqual(expectedFirstRun, firstRun.TimeOfDay);

            // Arrange
            calculator.Now = () => firstRun;

            // Act
            var secondRun = run.EarlierDate(firstRun, time1, time2, time3);

            // Assert
            Assert.AreEqual(expectedSecondRun, secondRun.TimeOfDay);

            // Arrange
            calculator.Now = () => secondRun;

            // Act
            var thirdRun = run.EarlierDate(secondRun, time1, time2, time3);

            // Assert
            Assert.AreEqual(expectedThirdRun, thirdRun.TimeOfDay);
        }

        [TestMethod]
        public void InTheNextMonth()
        {
            // Arrange
            var now = new DateTime(2019, 02, 11, 0, 30, 0);

            var expectedDate = new DateTime(2019, 03, 10, 11, 30, 0);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            calculator.Now = () => now;

            // Act
            run.Every(1).Months().On(10).At(11, 30);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(expectedDate, calculated.Value);
        }

         [TestMethod]
        public void InTheSameMonth()
        {
            // Arrange
            var now = new DateTime(2019, 02, 11, 0, 30, 0);

            var expectedDate = new DateTime(2019, 02, 20, 11, 30, 0);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            calculator.Now = () => now;

            // Act
            run.Every(1).Months().On(20).At(11, 30);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(expectedDate, calculated.Value);
        }
    }
}
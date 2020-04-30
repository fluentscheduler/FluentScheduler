namespace FluentScheduler.UnitTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CronTimeCalculatorTests
    {

        [TestMethod]
        public void At0405()
        {
            // Arrange
            var cronCalculator = new CronTimeCalculator("5 4 * * *");
            var calculator = (ITimeCalculator)cronCalculator;

            var date = new DateTime(2018, 12, 22);
            var expected =  new DateTime(2018, 12, 22, 4, 5, 0);

            // Act
            var calculated = calculator.Calculate(date);
            
            // Assert
            Assert.AreEqual(expected, calculated);
        }

        [TestMethod]
        public void At0005InAugust()
        {
            // Arrange
            var cronCalculator = new CronTimeCalculator("5 0 * 8 *");
            var calculator = (ITimeCalculator)cronCalculator;

            var date = new DateTime(2018, 12, 22);
            var expected =  new DateTime(2019, 8, 1, 0, 5, 0);

            // Act
            var calculated = calculator.Calculate(date);
            
            // Assert
            Assert.AreEqual(expected, calculated);
        }

        [TestMethod]
        public void At0405OnSunday()
        {
            // Arrange
            var cronCalculator = new CronTimeCalculator("5 4 * * sun");
            var calculator = (ITimeCalculator)cronCalculator;

            var date = new DateTime(2018, 12, 22);
            var expected =  new DateTime(2018, 12, 23, 4, 5, 0);

            // Act
            var calculated = calculator.Calculate(date);
            
            // Assert
            Assert.AreEqual(expected, calculated);
        }

        [TestMethod]
        public void At0400OnEveryDayOfMonthFrom8Through14()
        {
            // Arrange
            var cronCalculator = new CronTimeCalculator("0 4 8-14 * *");
            var calculator = (ITimeCalculator)cronCalculator;

            var date = new DateTime(2018, 12, 22);
            var expected =  new DateTime(2019, 01, 08, 4, 0, 0);

            // Act
            var calculated = calculator.Calculate(date);

            // Assert
            Assert.AreEqual(expected, calculated);
        }

        [TestMethod]
        public void At1415OnJanuaryFirst()
        {
            // Arrange
            var cronCalculator = new CronTimeCalculator("15 14 1 * *");
            var calculator = (ITimeCalculator)cronCalculator;

            var date = new DateTime(2018, 12, 22);
            var expected =  new DateTime(2019, 1, 1, 14, 15, 0);

            // Act
            var calculated = calculator.Calculate(date);

            // Assert
            Assert.AreEqual(expected, calculated);
        }

        [TestMethod]
        public void At2200OnEveryDayOfWeekFromMondayThroughFriday()
        {
            // Arrange
            var cronCalculator = new CronTimeCalculator("0 22 * * 1-5");
            var calculator = (ITimeCalculator)cronCalculator;

            var date = new DateTime(2018, 12, 22);
            var expected =  new DateTime(2018, 12, 24, 22, 0, 0);

            // Act
            var calculated = calculator.Calculate(date);

            // Assert
            Assert.AreEqual(expected, calculated);
        }

        [TestMethod]
        public void EveryDayAt0500And1700Hours()
        {
            // Arrange
            var cronCalculator = new CronTimeCalculator("0 5,17 * * *");
            var calculator = (ITimeCalculator)cronCalculator;

            var date = new DateTime(2018, 12, 22);
            var expected =  new DateTime(2018, 12, 22, 5, 0, 0);

            // Act
            var calculated = calculator.Calculate(date);

            // Assert
            Assert.AreEqual(expected, calculated);
        }

        [TestMethod]
        public void AtEveryMinute()
        {
            // Arrange
            var cronCalculator = new CronTimeCalculator("* * * * *");
            var calculator = (ITimeCalculator)cronCalculator;

            var date = new DateTime(2018, 12, 22);
            var expected =  new DateTime(2018, 12, 22, 0, 1, 0);

            // Act
            var calculated = calculator.Calculate(date);

            // Assert
            Assert.AreEqual(expected, calculated);
        }

        [TestMethod]
        public void AtEvery10Minutes()
        {
            // Arrange
            var cronCalculator = new CronTimeCalculator("*/10 * * * *");
            var calculator = (ITimeCalculator)cronCalculator;

            var date = new DateTime(2018, 12, 22);
            var expected =  new DateTime(2018, 12, 22, 0, 10, 0);

            // Act
            var calculated = calculator.Calculate(date);

            // Assert
            Assert.AreEqual(expected, calculated);
        }

        [TestMethod]
        public void At1700OnSundayAndFriday()
        {
            // Arrange
            var cronCalculator = new CronTimeCalculator("0 17 * * sun,fri");
            var calculator = (ITimeCalculator)cronCalculator;

            var date = new DateTime(2018, 12, 22);
            var expected =  new DateTime(2018, 12, 23, 17, 0, 0);

            // Act
            var calculated = calculator.Calculate(date);

            // Assert
            Assert.AreEqual(expected, calculated);
        }

        [TestMethod]
        public void EverySecond()
        {
            // Arrange
            var cronCalculator = new CronTimeCalculator("* * * * * *");
            var calculator = (ITimeCalculator)cronCalculator;

            var date = new DateTime(2018, 12, 23, 17, 0 , 0);
            var expected =  new DateTime(2018, 12, 23, 17, 0, 1);

            // Act
            var calculated = calculator.Calculate(date);

            // Assert
            Assert.AreEqual(expected, calculated);
        }
    }
}
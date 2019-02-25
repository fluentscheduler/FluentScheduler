namespace FluentScheduler.UnitTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExceptionUnitTests
    {
        [TestMethod]
        public void Except()
        {
            // Arrange
            var now = new DateTime(2019, 2, 25);
            var expected = new DateTime(2019, 2, 27);  
            var exceptionalDays = new DayOfWeek[2]
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
            };

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new ExceptionUnit(fluentCalculator); 

            // Act
            run.Except(exceptionalDays);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(expected, calculated.Value);
        }

        [TestMethod]
        public void ExceptEmpty()
        {
            // Arrange
            var now = new DateTime(2019, 2, 25);
            var expected = new DateTime(2019, 2, 25);  
            var exceptionalDays = new DayOfWeek[2];

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new ExceptionUnit(fluentCalculator); 

            // Act
            run.Except(exceptionalDays);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(expected, calculated.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExceptEveryDay()
        {
            // Arrange
            var now = new DateTime(2019, 2, 25);
            var allDays = new DayOfWeek[7]
            {
                DayOfWeek.Sunday,
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday,
            };

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new ExceptionUnit(fluentCalculator); 

            // Act / Assert
            run.Except(allDays);
            calculator.Calculate(now);
        }
    }   
}
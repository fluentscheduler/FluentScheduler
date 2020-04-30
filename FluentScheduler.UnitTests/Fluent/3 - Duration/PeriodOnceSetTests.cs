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
            var now = new DateTime(2018, 3, 3, 6, 53, 0);
            var expected = new DateTime(2018, 3, 3, 8, 40, 0);
            
            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new PeriodOnceSet(fluentCalculator);

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
            var now = new DateTime(2018, 3, 3, 10, 0 ,0);
            var expected = new DateTime(2018, 3, 3, 12, 30, 0);
            
            var timeSpan = new TimeSpan(12, 30, 0);
            
            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new PeriodOnceSet(fluentCalculator);

            calculator.Now = () => now;

            // Act
            run.At(timeSpan);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(expected, calculated.Value);
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
        public void Between23and2()
        {

            // Arrange
            var now = new DateTime(2019, 08, 19, 23, 30, 0);
            var expectedDate = new DateTime(2019, 08, 20, 23, 0, 0);
            
            var from = new TimeSpan(23, 0, 0);
            var to = new TimeSpan(2, 30, 0);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            calculator.Now = () => now;

            // Act
            run.Every(1).Days().Between(from, to);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(expectedDate, calculated.Value); 
        }
        

        [TestMethod]
        public void Between17and19()
        {
            // Arrange
            var now = new DateTime(2019, 08, 19, 16, 30, 0);
            var expectedDate = new DateTime(2019, 08, 20, 17, 30, 0);
            
            var from = new TimeSpan(17, 30, 0);
            var to = new TimeSpan(19, 30, 0);

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            calculator.Now = () => now;

            // Act
            run.Every(1).Days().Between(from, to);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(expectedDate, calculated.Value); 
        }

        [TestMethod]
        public void Between21and23()
        {
            // Arrange
            var now = new DateTime(2019, 08, 19, 21, 40, 0);
            var expectedDate = new DateTime(2019, 08, 20, 21, 40, 0);
            
            var from = new TimeSpan(21, 30, 0);
            var to = new TimeSpan(23, 30, 0);
            
            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            calculator.Now = () => now;

            // Act
            run.Every(1).Days().Between(from, to);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(expectedDate, calculated.Value);
        }

        [TestMethod]
        public void Between19and22()
        {
            // Arrange
            var now = new DateTime(2019, 08, 19, 23, 50, 0);
            var expectedDate = new DateTime(2019, 08, 21, 19, 30, 0);
            
            var from = new TimeSpan(19, 30, 0);
            var to = new TimeSpan(22, 30, 0);
            
            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RunSpecifier(fluentCalculator);

            calculator.Now = () => now;

            // Act
            run.Every(1).Days().Between(from, to);
            var calculated = calculator.Calculate(now);

            // Assert
            Assert.AreEqual(expectedDate, calculated.Value);
        }
    }
}

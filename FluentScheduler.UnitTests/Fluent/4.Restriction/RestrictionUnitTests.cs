namespace FluentScheduler.UnitTests
{
    using System;
    using Xunit;
    using static Xunit.Assert;

    public class RestrictionUnitTests
    {
        [Fact]
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
            var run = new RestrictionUnit(fluentCalculator); 

            // Act
            run.Except(exceptionalDays);
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(expected, calculated.Value);
        }

        [Fact]
        public void ExceptEmpty()
        {
            // Arrange
            var now = new DateTime(2019, 2, 25);
            var expected = new DateTime(2019, 2, 25);  
            var exceptionalDays = new DayOfWeek[2];

            var fluentCalculator = new FluentTimeCalculator();
            var calculator = (ITimeCalculator)fluentCalculator;
            var run = new RestrictionUnit(fluentCalculator); 

            // Act
            run.Except(exceptionalDays);
            var calculated = calculator.Calculate(now);

            // Assert
            Equal(expected, calculated.Value);
        }

        [Fact]
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
            var run = new RestrictionUnit(fluentCalculator); 

            // Act / Assert
            Throws<ArgumentException>(() => run.Except(allDays));
        }
    }
}
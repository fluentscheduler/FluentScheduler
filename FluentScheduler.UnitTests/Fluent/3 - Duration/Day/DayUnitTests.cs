namespace FluentScheduler
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using System;

	[TestClass]
	public class DayUnitTests
	{
		public void EveryWeekday()
		{
			// Arrange
			var sunday = new DateTime(2018, 02, 18);
			var saturday = new DateTime(2018, 02, 17);

			var monday = new DateTime(2018, 02, 19);

			var calculator = new FluentTimeCalculator();
			var run = new DayUnit(calculator);

			// Act
			run.Weekday();
			var calculated = calculator.Calculate(sunday);

			// Assert
			var expected = new DateTime(2018, 02, 19);
			Assert.AreEqual(expected, calculated);

			// Act
			calculated = calculator.Calculate(saturday);

			// Assert
			Assert.AreEqual(expected, calculated);

			// Act
			calculated = calculator.Calculate(monday);

			// Assert
			Assert.AreEqual(expected, calculated);
		}

		[TestMethod]
		public void EveryWeekend()
		{
			// Arrange
			var tuesday = new DateTime(2018, 02, 13);
			var saturday = new DateTime(2018, 02, 17);
			var sunday = new DateTime(2018, 02, 18);

			var calculator = new FluentTimeCalculator();
			var run = new DayUnit(calculator);

			// Act
			run.Weekend();
			var calculated = calculator.Calculate(tuesday);

			// Assert
			var expected = new DateTime(2018, 02, 17);
			Assert.AreEqual(expected, calculated);

			// Act
			calculated = calculator.Calculate(saturday);

			// Assert
			Assert.AreEqual(expected, calculated);

			// Act
			calculated = calculator.Calculate(sunday);

			// Assert
			expected = new DateTime(2018, 02, 18);
			Assert.AreEqual(expected, calculated);
		}
	}
}
namespace FluentScheduler.UnitTests
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using System;

	[TestClass]
	public class OnceSetTests
	{

		[TestMethod]
		public void AndEveryDayOfWeek()
		{
			// Arrange
			var friday = new DateTime(2018, 2, 16);
			var monday = new DateTime(2018, 2, 19);
			var sunday = new DateTime(2018, 2, 25);

			var calculator = new FluentTimeCalculator();
			var run = new OnceSet(calculator);

			// Act
			run.AndEvery(DayOfWeek.Sunday);
			var calculated = calculator.Calculate(friday);

			// Assert
			var expected = new DateTime(2018, 2, 18);
			Assert.AreEqual(expected, calculated);

			//Act
			calculated = calculator.Calculate(monday);

			// Assert
			expected = new DateTime(2018, 2, 25);
			Assert.AreEqual(expected, calculated);

			// Act
			calculated = calculator.Calculate(sunday);

			// Assert
			Assert.AreEqual(sunday, calculated);

		}

		[TestMethod]
		public void EveryInterval()
		{
			var now = DateTime.Today;

			var calculator = new FluentTimeCalculator();
			var run = new OnceSet(calculator);

			// Act
			run.AndEvery(10).Seconds();
			var calculated = calculator.Calculate(now);

			// Assert
			Assert.AreEqual(now.AddSeconds(10), calculated);
		}
	}
}
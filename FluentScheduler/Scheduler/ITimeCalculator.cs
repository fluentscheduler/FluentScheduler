namespace FluentScheduler
{
	using System;

	internal interface ITimeCalculator
	{
		Func<DateTime> Now { get; set; }

		void Reset();

		DateTime? Calculate(DateTime last);
	}
}
namespace FluentScheduler
{
    using System;

   internal interface ITimeCalculator
   {
        void Reset();

        DateTime? Calculate(DateTime last);
   }
}
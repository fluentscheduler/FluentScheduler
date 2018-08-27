namespace FluentScheduler
{
    using System;

   internal interface IFluentTimeCalculator
   {
        void Reset();

        DateTime? Calculate(DateTime last);
   }
}
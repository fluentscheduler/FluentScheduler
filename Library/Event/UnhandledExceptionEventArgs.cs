namespace FluentScheduler
{
    using System;

    public class UnhandledExceptionEventArgs : EventArgs
    {
        public Exception ExceptionObject { get; private set; }

        public bool IsTerminating { get; private set; }

        public UnhandledExceptionEventArgs(Exception exceptionObject, bool isTerminating)
        {
            ExceptionObject = exceptionObject;
            IsTerminating = isTerminating;
        }
    }
}

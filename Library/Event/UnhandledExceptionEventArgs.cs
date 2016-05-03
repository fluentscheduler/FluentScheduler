namespace FluentScheduler
{
    using System;

    /// <summary>
    /// Same as System.UnhandledExceptionEventArgs except that ExceptionObject is an actual System.Exception reference.
    /// </summary>
    public class UnhandledExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the unhandled exception object.
        /// </summary>
        public Exception ExceptionObject { get; private set; }

        /// <summary>
        /// Indicates whether the common language runtime is terminating.
        /// </summary>
        public bool IsTerminating { get; private set; }

        internal UnhandledExceptionEventArgs(Exception exceptionObject, bool isTerminating)
        {
            ExceptionObject = exceptionObject;
            IsTerminating = isTerminating;
        }
    }
}

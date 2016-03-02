using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentScheduler
{
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

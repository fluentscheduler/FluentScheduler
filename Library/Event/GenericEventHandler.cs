namespace FluentScheduler
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Generic event handler to provide strongly typed event arguments.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly",
        Justification = "Using strong-typed GenericEventHandler<TSender, TEventArgs> event handler pattern.")]
    public delegate void GenericEventHandler<in TSender, in TEventArgs>(TSender sender, TEventArgs e) where TEventArgs : EventArgs;
}

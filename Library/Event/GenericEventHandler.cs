namespace FluentScheduler
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Event handler with typed sender.
    /// </summary>
    /// <typeparam name="TSender">Sender type.</typeparam>
    /// <typeparam name="TEventArgs">EventArgs type.</typeparam>
    /// <param name="sender">The object that initiated the event.</param>
    /// <param name="e">Data related to the event.</param>
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly",
        Justification = "Using strong-typed GenericEventHandler<TSender, TEventArgs> event handler pattern.")]
    public delegate void GenericEventHandler<in TSender, in TEventArgs>(TSender sender, TEventArgs e) where TEventArgs : EventArgs;
}

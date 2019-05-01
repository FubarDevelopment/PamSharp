// <copyright file="IPamConversationHandler.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;

namespace FubarDev.PamSharp
{
    /// <summary>
    /// Handler that converts PAM messages and sends them to a <see cref="IPamMessageHandler"/>.
    /// </summary>
    public interface IPamConversationHandler
    {
        /// <summary>
        /// Converts PAM messages, and passes them to a <see cref="IPamMessageHandler"/>.
        /// </summary>
        /// <param name="messageCount">The number of messages pointed to by <paramref name="messages"/>.</param>
        /// <param name="messages">The messages passed to the callback.</param>
        /// <param name="responseArrayPtr">The responses for all messages.</param>
        /// <param name="appDataPtr">An opaque pointer from the initialization (<see cref="Interop.PamConv.AppData"/>).</param>
        /// <returns>The status for this operation.</returns>
        PamStatus Handle(int messageCount, IntPtr messages, out IntPtr responseArrayPtr, IntPtr appDataPtr);
    }
}

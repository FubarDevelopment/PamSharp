// <copyright file="PamConv.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace FubarDev.PamSharp.Interop
{
    /// <summary>
    /// A callback function to be called for conversation messages.
    /// </summary>
    /// <param name="messageCount">The number of messages pointed to by <paramref name="messages"/>.</param>
    /// <param name="messages">The messages passed to the callback.</param>
    /// <param name="responseArrayPtr">The responses for all messages.</param>
    /// <param name="appDataPtr">An opaque pointer from the initialization (<see cref="PamConv.AppData"/>).</param>
    /// <returns>the status for the current operation.</returns>
    public delegate PamStatus ConvCallback(int messageCount, IntPtr messages, out IntPtr responseArrayPtr, IntPtr appDataPtr);

    /// <summary>
    /// Information about the conversation callback to use.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class PamConv
    {
#pragma warning disable SA1401 // Fields should be private
        /// <summary>
        /// The conversation callback to use.
        /// </summary>
        public ConvCallback ConversationCallback;

        /// <summary>
        /// A pointer passed to the callback function.
        /// </summary>
        public IntPtr AppData;
#pragma warning restore SA1401 // Fields should be private

        /// <summary>
        /// Initializes a new instance of the <see cref="PamConv"/> class.
        /// </summary>
        /// <param name="conversationCallback">The conversation callback to use.</param>
        public PamConv(ConvCallback conversationCallback)
            : this(conversationCallback, IntPtr.Zero)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PamConv"/> class.
        /// </summary>
        /// <param name="conversationCallback">The conversation callback to use.</param>
        /// <param name="appData">Some opaque pointer passed to the callback.</param>
        public PamConv(ConvCallback conversationCallback, IntPtr appData)
        {
            ConversationCallback = conversationCallback;
            AppData = appData;
        }
    }
}

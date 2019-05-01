// <copyright file="PamMessage.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace FubarDev.PamSharp.Interop
{
    /// <summary>
    /// The structure of a PAM message.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct PamMessage
    {
        /// <summary>
        /// The style of the PAM message.
        /// </summary>
        public readonly PamMessageStyle MsgStyle;

        /// <summary>
        /// A pointer to the message itself.
        /// </summary>
        public readonly IntPtr Message;
    }
}

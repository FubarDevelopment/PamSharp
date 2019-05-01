// <copyright file="PamBinaryMessageData.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;

namespace FubarDev.PamSharp.Interop
{
    /// <summary>
    /// Structure for a binary PAM message data.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal readonly struct PamBinaryMessageData
    {
        /// <summary>
        /// The length of the message.
        /// </summary>
        public readonly uint Length;

        /// <summary>
        /// The type of the message.
        /// </summary>
        public readonly byte Type;

        /// <summary>
        /// Placeholder for the data of the message.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public readonly byte Data;
    }
}

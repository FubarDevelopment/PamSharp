// <copyright file="PamBinaryData.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;

namespace FubarDev.PamSharp.Messages
{
    /// <summary>
    /// Binary data for a <see cref="PamBinaryPromptMessage"/>.
    /// </summary>
    public sealed class PamBinaryData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PamBinaryData"/> class.
        /// </summary>
        /// <param name="type">The type of the binary data.</param>
        /// <param name="data">The binary data iteself.</param>
        internal PamBinaryData(byte type, byte[] data)
        {
            Type = type;
            Data = data;
        }

        /// <summary>
        /// Gets the type of the binary data.
        /// </summary>
        public byte Type { get; }

        /// <summary>
        /// Gets the binary data.
        /// </summary>
        public ReadOnlyMemory<byte> Data { get; }
    }
}

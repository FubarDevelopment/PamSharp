// <copyright file="TextResponseContainer.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;

namespace FubarDev.PamSharp.Interop
{
    /// <summary>
    /// Used to return the user's response to the PAM library.
    /// </summary>
    /// <remarks>
    /// This structure is allocated by the application program, and free()'d by
    /// the Linux-PAM library (or calling module). This means that this structure
    /// can only be passed to a P/Invoke function when it was converted manually.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct TextResponseContainer
    {
        [MarshalAs(PamConstants.STRING_TYPE)]
        public readonly string? Response;

        /// <summary>
        /// Currently unused, zero expected.
        /// </summary>
        public readonly int ReturnCode;

        public TextResponseContainer(string? response)
        {
            Response = response;
            ReturnCode = 0;
        }
    }
}

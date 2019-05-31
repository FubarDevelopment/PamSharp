// <copyright file="PamException.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;

using FubarDev.PamSharp.Interop;

namespace FubarDev.PamSharp
{
    /// <summary>
    /// The PAM exception.
    /// </summary>
    public class PamException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PamException"/> class.
        /// </summary>
        /// <param name="interop">The object implementing the PAM API.</param>
        /// <param name="handle">The PAM handle.</param>
        /// <param name="status">The error status.</param>
        internal PamException(IPamInterop interop, IntPtr handle, PamStatus status)
            : base(GetPamError(interop, handle, (int)status))
        {
            Status = status;
        }

        /// <summary>
        /// Gets the PAM status.
        /// </summary>
        public PamStatus Status { get; }

        /// <summary>
        /// Gets the PAM error message.
        /// </summary>
        /// <param name="interop">The object implementing the PAM interface.</param>
        /// <param name="handle">The PAM handle.</param>
        /// <param name="status">The error status.</param>
        /// <returns>The error message.</returns>
        private static string GetPamError(IPamInterop interop, IntPtr handle, int status)
        {
            var result = interop.pam_strerror(handle, status);
            if (result == IntPtr.Zero)
            {
                return $"Error ({status})";
            }

            return Marshal.PtrToStringUTF8(result);
        }
    }
}

// <copyright file="PamException.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Runtime.CompilerServices;
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
        /// <param name="caller">The PAM method (or library method which uses PAM method internally) that causes exception.</param>
        internal PamException(IPamInterop interop, IntPtr handle, PamStatus status, [CallerMemberName] string? caller = null)
            : base(GetPamError(interop, handle, status, caller))
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
        /// <param name="caller">The PAM method (or library method which uses PAM method internally) that causes exception.</param>
        /// <returns>The error message.</returns>
        private static string? GetPamError(IPamInterop interop, IntPtr handle, PamStatus status, string? caller)
        {
            var result = interop.pam_strerror(handle, (int)status);
            if (result == IntPtr.Zero)
            {
                caller ??= "unknown method";
                var errorMessage = PamStatusHelper.TransformStatusToString(status, caller);
                return $"{caller} fails with {errorMessage}";
            }

            return Marshal.PtrToStringUTF8(result);
        }
    }
}

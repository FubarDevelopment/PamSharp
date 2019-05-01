// <copyright file="DelayCallback.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;

namespace FubarDev.PamSharp.Interop
{
    /// <summary>
    /// Callback for delays between authentication failures.
    /// </summary>
    /// <param name="status">Return code of the module stack.</param>
    /// <param name="usec">The delay in microseconds.</param>
    /// <param name="appDataPtr">The pointer to application data associated with the PAM handle.</param>
    internal delegate void DelayCallback(PamStatus status, uint usec, IntPtr appDataPtr);
}

// <copyright file="PamConstants.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;

namespace FubarDev.PamSharp.Interop
{
    internal static class PamConstants
    {
#pragma warning disable SA1310 // Field names should not contain underscore
        /// <summary>
        /// We always use UTF-8.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        internal const UnmanagedType STRING_TYPE = UnmanagedType.LPUTF8Str;
#pragma warning restore SA1310 // Field names should not contain underscore

        /// <summary>
        /// The default library to use.
        /// </summary>
        internal const string DllName = "pam";
    }
}

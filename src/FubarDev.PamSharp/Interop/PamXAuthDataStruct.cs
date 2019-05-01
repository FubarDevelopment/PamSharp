// <copyright file="PamXAuthDataStruct.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace FubarDev.PamSharp.Interop
{
    /// <summary>
    /// The data structure for XAuth.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct PamXAuthDataStruct
    {
        /// <summary>
        /// The length of the name.
        /// </summary>
        public int NameLen;

        /// <summary>
        /// Pointer to the name.
        /// </summary>
        public IntPtr Name;

        /// <summary>
        /// The length of the data.
        /// </summary>
        public int DataLen;

        /// <summary>
        /// The pointer to the data.
        /// </summary>
        public IntPtr Data;
    }
}

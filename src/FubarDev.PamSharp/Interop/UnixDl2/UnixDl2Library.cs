// <copyright file="UnixDl2Library.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;

using Platform.Invoke;

namespace FubarDev.PamSharp.Interop.UnixDl2
{
    /// <summary>
    /// Implements library function loading using libdl.
    /// </summary>
    [DebuggerDisplay("so({" + nameof(Name) + "})")]
    public sealed class UnixDl2Library : LibraryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnixDl2Library"/> class.
        /// </summary>
        /// <param name="moduleHandle">Handle to the loaded module returned by dlopen(3).</param>
        /// <param name="libraryName">Name of the loaded library.</param>
        public UnixDl2Library(IntPtr moduleHandle, string libraryName)
            : base(moduleHandle, new UnixDl2LibraryProcProvider(), libraryName)
        {
        }
    }
}

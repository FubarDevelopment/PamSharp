// <copyright file="UnixDl2LibraryProcProvider.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

using Platform.Invoke;

namespace FubarDev.PamSharp.Interop.UnixDl2
{
    /// <summary>
    /// Implements function loading and library handling for Unix operating systems.
    /// </summary>
    [ImmutableObject(true)]
    public sealed class UnixDl2LibraryProcProvider : ILibraryProcProvider
    {
        /// <summary>
        /// Free this library.
        /// </summary>
        /// <param name="module">Module to free.</param>
        /// <returns><see langword="true"/> when freeing the library was successful.</returns>
        public bool Free(IntPtr module)
        {
            return dlclose(module) == 0;
        }

        /// <summary>
        /// Retrieves a pointer to the specified function.
        /// </summary>
        /// <param name="module">Operating system provided library handle.</param>
        /// <param name="procName">Library function name.</param>
        /// <returns>Pointer to the loaded function. Null if procedure could not be located.</returns>
        [Pure]
        public IntPtr GetProc(IntPtr module, string procName)
        {
            return dlsym(module, procName);
        }

        /// <summary>
        /// Gets a delegate for the specified function pointer.
        /// </summary>
        /// <param name="functionPointer">Function pointer retrieved using <see cref="GetProc"/>.</param>
        /// <param name="delegateType">Type of delegate to return.</param>
        /// <returns><see cref="Delegate"/> fro the specified function pointer.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <see paramref="functionPointer"/> or <see paramref="delegateType"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <see paramref="delegateType"/> is not null.</exception>
        [Pure]
        public Delegate GetDelegateFromFunctionPointer(IntPtr functionPointer, Type delegateType)
        {
            return Marshal.GetDelegateForFunctionPointer(functionPointer, delegateType);
        }

        /// <summary>
        /// Gets a symbol pointer from the specified module.
        /// </summary>
        /// <param name="handle">Operating system handle to a library.</param>
        /// <param name="symbolName">Name of symbol to locate.</param>
        /// <returns>an error code.</returns>
        [DllImport("libdl.so.2")]
#pragma warning disable SA1300 // Element should begin with upper-case letter
        private static extern IntPtr dlsym(IntPtr handle, [In]string symbolName);
#pragma warning restore SA1300 // Element should begin with upper-case letter

        /// <summary>
        /// Closes a library.
        /// </summary>
        /// <param name="handle">The library handle.</param>
        /// <returns>the error code.</returns>
        [DllImport("libdl.so.2")]
#pragma warning disable SA1300 // Element should begin with upper-case letter
        private static extern int dlclose(IntPtr handle);
#pragma warning restore SA1300 // Element should begin with upper-case letter
    }
}

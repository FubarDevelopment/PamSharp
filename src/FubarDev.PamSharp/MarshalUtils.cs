// <copyright file="MarshalUtils.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FubarDev.PamSharp
{
    /// <summary>
    /// Utilities for marshalling.
    /// </summary>
    internal static class MarshalUtils
    {
        /// <summary>
        /// Gets a string from a pointer.
        /// </summary>
        /// <param name="ptr">The pointer to create the string from.</param>
        /// <returns>The string or null.</returns>
        public static string? PtrToStringUTF8(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;

            return Marshal.PtrToStringUTF8(ptr);
        }

        /// <summary>
        /// Convert a string to a UTF-8 byte sequence and returning the pointer.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <param name="length">The length of the created byte array without the terminating NUL.</param>
        /// <returns>The address of the created UTF-8 byte sequence.</returns>
        public static IntPtr StringToHGlobalUTF8(string? s, out int length)
        {
            if (s == null)
            {
                length = 0;
                return IntPtr.Zero;
            }

            var bytes = Encoding.UTF8.GetBytes(s);
            var ptr = Marshal.AllocHGlobal(bytes.Length + 1);
            Marshal.Copy(bytes, 0, ptr, bytes.Length);
            Marshal.WriteByte(ptr, bytes.Length, 0);
            length = bytes.Length;

            return ptr;
        }

        /// <summary>
        /// Convert a string to a UTF-8 byte sequence and returning the pointer.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The address of the created UTF-8 byte sequence.</returns>
        public static IntPtr StringToHGlobalUTF8(string? s)
        {
            return StringToHGlobalUTF8(s, out _);
        }
    }
}

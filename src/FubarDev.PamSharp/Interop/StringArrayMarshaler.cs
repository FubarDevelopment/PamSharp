// <copyright file="StringArrayMarshaler.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace FubarDev.PamSharp.Interop
{
    /// <summary>
    /// A marshaler for string arrays.
    /// </summary>
    internal class StringArrayMarshaler : ICustomMarshaler
    {
        private static readonly ConcurrentDictionary<string, ICustomMarshaler> _customMarshalers =
            new ConcurrentDictionary<string, ICustomMarshaler>();

        /// <summary>
        /// Get an instance of the custom marshaler by cookie.
        /// </summary>
        /// <param name="cookie">The cookie to get or create the marshaler for.</param>
        /// <returns>The marshaler for the cookie.</returns>
        [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "This function is required by convention.")]
        public static ICustomMarshaler GetInstance(string cookie)
        {
            return _customMarshalers.GetOrAdd(cookie, c => new StringArrayMarshaler());
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Same name as in the interface.")]
        public void CleanUpManagedData(object ManagedObj)
        {
        }

        /// <inheritdoc />
        public unsafe void CleanUpNativeData(IntPtr pNativeData)
        {
            if (pNativeData == IntPtr.Zero)
            {
                return;
            }

            var arrayPtr = (byte**)pNativeData;

            while (*arrayPtr != null)
            {
                Marshal.FreeHGlobal(new IntPtr(*arrayPtr));
                arrayPtr += 1;
            }

            Marshal.FreeHGlobal(pNativeData);
        }

        /// <inheritdoc />
        public int GetNativeDataSize()
        {
            return IntPtr.Size;
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Same name as in the interface.")]
        public unsafe IntPtr MarshalManagedToNative(object ManagedObj)
        {
            if (ManagedObj == null)
            {
                return IntPtr.Zero;
            }

            var data = (string[])ManagedObj;
            var nativeData = Marshal.AllocHGlobal(IntPtr.Size * (data.Length + 1));
            var arrayPtr = (byte**)nativeData;
            Debug.Assert(arrayPtr != null, nameof(arrayPtr) + " != null");

            for (var i = 0; i < data.Length; i++)
            {
                var itemPtr = MarshalUtils.StringToHGlobalUTF8(data[i]);
                arrayPtr[i] = (byte*)itemPtr;
            }

            arrayPtr[data.Length] = null;
            return IntPtr.Zero;
        }

        /// <inheritdoc />
#nullable disable
        public unsafe object MarshalNativeToManaged(IntPtr pNativeData)
        {
            if (pNativeData == IntPtr.Zero)
            {
                return null;
            }

            var result = new List<string>();
            var arrayPtr = (byte**)pNativeData;

            while (*arrayPtr != null)
            {
                var s = MarshalUtils.PtrToStringUTF8(new IntPtr(*arrayPtr));
                result.Add(s!);

                arrayPtr += 1;
            }

            return result.ToArray();
        }
#nullable restore
    }
}

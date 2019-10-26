// <copyright file="ConstStringMarshaler.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace FubarDev.PamSharp.Interop
{
    /// <summary>
    /// A marshaler for strings that must not be freed.
    /// </summary>
    internal class ConstStringMarshaler : ICustomMarshaler
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
            return _customMarshalers.GetOrAdd(cookie, c => new ConstStringMarshaler());
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Same name as in the interface.")]
        public void CleanUpManagedData(object ManagedObj)
        {
        }

        /// <inheritdoc />
        public void CleanUpNativeData(IntPtr pNativeData)
        {
        }

        /// <inheritdoc />
        public int GetNativeDataSize()
        {
            return IntPtr.Size;
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Same name as in the interface.")]
        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            return MarshalUtils.StringToHGlobalUTF8((string)ManagedObj);
        }

        /// <inheritdoc />
#nullable disable
        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            return MarshalUtils.PtrToStringUTF8(pNativeData);
        }
#nullable restore
    }
}

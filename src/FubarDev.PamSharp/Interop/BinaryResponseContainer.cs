// <copyright file="BinaryResponseContainer.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace FubarDev.PamSharp.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct BinaryResponseContainer
    {
        public readonly IntPtr Response;
        public readonly int ReturnCode;

        public BinaryResponseContainer(IntPtr responsePtr)
        {
            Response = responsePtr;
            ReturnCode = 0;
        }
    }
}

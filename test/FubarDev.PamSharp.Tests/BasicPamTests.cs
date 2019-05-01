// <copyright file="BasicPamTests.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

using FubarDev.PamSharp.Interop;

using Xunit;

namespace FubarDev.PamSharp.Tests
{
    public class BasicPamTests
    {
        [Fact]
        public void CanStartPamTransaction()
        {
            var conv = new PamConv(NoOpConvCallback);
            PamInterop.pam_start("passwd", null, conv, out var handle);
            PamInterop.pam_end(handle, PamStatus.PAM_SUCCESS);
        }

        private static PamStatus NoOpConvCallback(int messageCount, IntPtr messages, out IntPtr responseArrayPtr, IntPtr appDataPtr)
        {
            responseArrayPtr = IntPtr.Zero;
            return PamStatus.PAM_CONV_ERR;
        }
    }
}

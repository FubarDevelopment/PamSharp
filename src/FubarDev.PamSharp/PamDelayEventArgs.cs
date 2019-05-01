// <copyright file="PamDelayEventArgs.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;

namespace FubarDev.PamSharp
{
    /// <summary>
    /// Event arguments for a PAM delay.
    /// </summary>
    public class PamDelayEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PamDelayEventArgs"/> class.
        /// </summary>
        /// <param name="status">The status of the action that caused the delay.</param>
        /// <param name="delay">The time span to wait for.</param>
        public PamDelayEventArgs(PamStatus status, TimeSpan delay)
        {
            Status = status;
            Delay = delay;
        }

        /// <summary>
        /// Gets the status of the action that caused the delay.
        /// </summary>
        public PamStatus Status { get; }

        /// <summary>
        /// Gets the time span to wait for.
        /// </summary>
        public TimeSpan Delay { get; }
    }
}

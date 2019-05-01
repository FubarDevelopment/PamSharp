// <copyright file="IPamMessage.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

namespace FubarDev.PamSharp
{
    /// <summary>
    /// Base interface for PAM messages.
    /// </summary>
    public interface IPamMessage
    {
        /// <summary>
        /// Gets the message style.
        /// </summary>
        PamMessageStyle MessageStyle { get; }
    }
}

// <copyright file="IPamService.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

namespace FubarDev.PamSharp
{
    /// <summary>
    /// The PAM service.
    /// </summary>
    public interface IPamService
    {
        /// <summary>
        /// Initializes a PAM transaction.
        /// </summary>
        /// <param name="messageHandler">The message handler.</param>
        /// <param name="user">The user name.</param>
        /// <returns>The new PAM transaction.</returns>
        IPamTransaction Start(
            IPamMessageHandler messageHandler,
            string? user = null);
    }
}

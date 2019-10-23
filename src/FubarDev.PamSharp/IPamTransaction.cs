// <copyright file="IPamTransaction.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using FubarDev.PamSharp.Interop;

namespace FubarDev.PamSharp
{
    /// <summary>
    /// The PAM transaction.
    /// </summary>
    public interface IPamTransaction : IDisposable
    {
        /// <summary>
        /// Event that is called when a delay is required in response
        /// to a failed authentication attempt.
        /// </summary>
        /// <remarks>
        /// The use of this event must be enabled with <see cref="EnableFailDelayEvent"/>.
        /// This feature is only available on Linux.
        /// </remarks>
        event EventHandler<PamDelayEventArgs>? Delay;

        /// <summary>
        /// Gets the PAM transaction handle.
        /// </summary>
        IntPtr Handle { get; }

        /// <summary>
        /// Gets or sets the service name.
        /// </summary>
        /// <seealso cref="PamItemTypes.PAM_SERVICE"/>
        string? ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        /// <seealso cref="PamItemTypes.PAM_USER"/>
        string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the terminal name.
        /// </summary>
        /// <seealso cref="PamItemTypes.PAM_TTY"/>
        string? TTYName { get; set; }

        /// <summary>
        /// Gets or sets the requesting (client) hostname.
        /// </summary>
        /// <seealso cref="PamItemTypes.PAM_RHOST"/>
        string? RemoteHostName { get; set; }

        /// <summary>
        /// Gets or sets the requesting (client) user name.
        /// </summary>
        /// <seealso cref="PamItemTypes.PAM_RUSER"/>
        string? RemoteUserName { get; set; }

        /// <summary>
        /// Gets or sets the user prompt.
        /// </summary>
        /// <seealso cref="PamItemTypes.PAM_USER_PROMPT"/>
        [DefaultValue("login: ")]
        string UserPrompt { get; set; }

        /// <summary>
        /// Gets or sets the type of authentication token.
        /// </summary>
        /// <seealso cref="PamItemTypes.PAM_AUTHTOK_TYPE"/>
        string? AuthenticationTokenType { get; set; }

        /// <summary>
        /// Gets or sets the X display name.
        /// </summary>
        /// <seealso cref="PamItemTypes.PAM_XDISPLAY"/>
        string? XDisplay { get; set; }

        /// <summary>
        /// Gets or sets the X authentication data.
        /// </summary>
        /// <seealso cref="PamItemTypes.PAM_XAUTHDATA"/>
        PamXAuthData? XAuthData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Delay"/> event is enabled.
        /// </summary>
        bool EnableFailDelayEvent { get; set; }

        /// <summary>
        /// Gets or sets the low-level PAM conversation handler.
        /// </summary>
        PamConv Conversation { get; set; }

        /// <summary>
        /// Account authentication.
        /// </summary>
        /// <param name="flags">Some flags.</param>
        void Authenticate(PamFlags flags = 0);

        /// <summary>
        /// Establish, maintain, or delete user credentials.
        /// </summary>
        /// <param name="flags">Some flags.</param>
        void SetCredentials(PamFlags flags = 0);

        /// <summary>
        /// Updating authentication tokens.
        /// </summary>
        /// <param name="flags">Some flags.</param>
        void ChangeAuthenticationToken(PamFlags flags = 0);

        /// <summary>
        /// PAM account validation management.
        /// </summary>
        /// <param name="flags">Some flags.</param>
        void AccountManagement(PamFlags flags = 0);

        /// <summary>
        /// Start PAM session management.
        /// </summary>
        /// <param name="flags">Some flags.</param>
        void OpenSession(PamFlags flags = 0);

        /// <summary>
        /// Terminate PAM session management.
        /// </summary>
        /// <param name="flags">Some flags.</param>
        void CloseSession(PamFlags flags = 0);

        /// <summary>
        /// Getting the PAM environment.
        /// </summary>
        /// <returns>A list of environment variables.</returns>
        string[] GetEnvironment();

        /// <summary>
        /// Get a PAM environment variable.
        /// </summary>
        /// <param name="name">The name of the environment variable (case-sensitive).</param>
        /// <returns>The value of the environment variable or NULL.</returns>
        string GetEnvironment(string name);

        /// <summary>
        /// Set or change PAM environment variable.
        /// </summary>
        /// <param name="nameValue">A string in the form NAME(=(value)?)?.</param>
        void PutEnvironment(string nameValue);

        /// <summary>
        /// Request a delay on failure.
        /// </summary>
        /// <param name="timeSpan">The time span to wait.</param>
        void FailDelay(TimeSpan timeSpan);

        /// <summary>
        /// Get PAM information.
        /// </summary>
        /// <typeparam name="T">The type of the object to return.</typeparam>
        /// <param name="itemType">The type of information to get.</param>
        /// <param name="unmarshalFunc">The function that marshals the value at the pointer into the target type.</param>
        /// <returns>The requested information.</returns>
        T GetItem<T>(PamItemTypes itemType, Func<IntPtr, T> unmarshalFunc);

        /// <summary>
        /// Set and update PAM information.
        /// </summary>
        /// <remarks>
        /// The data returned by <paramref name="createItemDataFunc"/> is freed with <see cref="Marshal.FreeHGlobal"/>.
        /// </remarks>
        /// <param name="itemType">The type of information to set.</param>
        /// <param name="createItemDataFunc">The function that returns a pointer to the data to be set.</param>
        void SetItem(PamItemTypes itemType, Func<IntPtr> createItemDataFunc);

        /// <summary>
        /// Set and update PAM information.
        /// </summary>
        /// <remarks>
        /// PAM always(?) copies the data, so we have to remove the data created
        /// by <paramref name="createItemDataFunc"/> to avoid memory leaks.
        /// </remarks>
        /// <param name="itemType">The type of information to set.</param>
        /// <param name="createItemDataFunc">The function that returns a pointer to the data to be set.</param>
        /// <param name="cleanupAction">The action to be called to cleanup the data after the data has been set.</param>
        void SetItem(PamItemTypes itemType, Func<IntPtr> createItemDataFunc, Action<IntPtr> cleanupAction);
    }
}

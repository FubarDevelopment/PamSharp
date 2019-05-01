// <copyright file="PamFlags.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;

namespace FubarDev.PamSharp
{
    /// <summary>
    /// The PAM flags.
    /// </summary>
    [Flags]
    public enum PamFlags
    {
        /// <summary>
        /// Authentication service should not generate any messages
        /// </summary>
        PAM_SILENT = 0x8000,

        /// <summary>
        /// The authentication service should return <see cref="PamStatus.PAM_AUTH_ERR"/> if the user has a null authentication token
        /// </summary>
        PAM_DISALLOW_NULL_AUTHTOK = 0x0001,

        /// <summary>
        /// Initialize the credentials for the user
        /// </summary>
        PAM_ESTABLISH_CRED = 0x0002,

        /// <summary>
        /// Delete the user's credentials
        /// </summary>
        PAM_DELETE_CRED = 0x0004,

        /// <summary>
        /// Fully reinitialize the user's credentials
        /// </summary>
        PAM_REINITIALIZE_CRED = 0x0008,

        /// <summary>
        /// Extend the lifetime of the existing credentials
        /// </summary>
        PAM_REFRESH_CRED = 0x0010,

        /// <summary>
        /// The users authentication token (password) should only be changed if it has expired
        /// </summary>
        PAM_CHANGE_EXPIRED_AUTHTOK = 0x0020,
    }
}

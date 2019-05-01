// <copyright file="PamItemTypes.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

namespace FubarDev.PamSharp
{
    /// <summary>
    /// The PAM item types.
    /// </summary>
    public enum PamItemTypes
    {
        /// <summary>
        /// The service name (which identifies that PAM stack that the PAM functions will use to authenticate the program)
        /// </summary>
        PAM_SERVICE = 1,

        /// <summary>
        /// The username of the entity under whose identity service will be given
        /// </summary>
        PAM_USER = 2,

        /// <summary>
        /// The terminal name
        /// </summary>
        PAM_TTY = 3,

        /// <summary>
        /// The requesting hostname
        /// </summary>
        PAM_RHOST = 4,

        /// <summary>
        /// The pam_conv structure
        /// </summary>
        PAM_CONV = 5,

        /// <summary>
        /// The authentication token (often a password)
        /// </summary>
        PAM_AUTHTOK = 6,

        /// <summary>
        /// The old authentication token
        /// </summary>
        PAM_OLDAUTHTOK = 7,

        /// <summary>
        /// The requesting user name
        /// </summary>
        PAM_RUSER = 8,

        /// <summary>
        /// The string used when prompting for a user's name
        /// </summary>
        PAM_USER_PROMPT = 9,

        /// <summary>
        /// A function pointer to redirect centrally managed failure delays
        /// </summary>
        PAM_FAIL_DELAY = 10,

        /// <summary>
        /// The name of the X display
        /// </summary>
        PAM_XDISPLAY = 11,

        /// <summary>
        /// A pointer to a structure containing the X authentication data
        /// required to make a connection to the display specified by PAM_XDISPLAY
        /// </summary>
        PAM_XAUTHDATA = 12,

        /// <summary>
        /// Authentication token type
        /// </summary>
        /// <remarks>
        /// The default action is for the module to use the following prompts when requesting
        /// passwords: &quot;New UNIX password: &quot; and &quot;Retype UNIX password: &quot;.
        /// The example word UNIX can be replaced with this item, by default it is empty.
        /// </remarks>
        PAM_AUTHTOK_TYPE = 13,
    }
}

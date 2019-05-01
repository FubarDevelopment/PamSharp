// <copyright file="PamMessageStyle.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

namespace FubarDev.PamSharp
{
    /// <summary>
    /// The PAM Message Styles.
    /// </summary>
    public enum PamMessageStyle
    {
        /// <summary>
        /// Get user input without echoing the input.
        /// </summary>
        PAM_PROMPT_ECHO_OFF = 1,

        /// <summary>
        /// Get user input while echoing the input.
        /// </summary>
        PAM_PROMPT_ECHO_ON = 2,

        /// <summary>
        /// Show an error message.
        /// </summary>
        PAM_ERROR_MSG = 3,

        /// <summary>
        /// Show an informational text.
        /// </summary>
        PAM_TEXT_INFO = 4,

        /* Linux-PAM specific types */

        /// <summary>
        /// Yes/No/Maybe conditionals.
        /// </summary>
        PAM_RADIO_TYPE = 5,

        /// <summary>
        /// This is for server client non-human interaction.. these are NOT
        /// part of the X/Open PAM specification.
        /// </summary>
        PAM_BINARY_PROMPT = 7,
    }
}

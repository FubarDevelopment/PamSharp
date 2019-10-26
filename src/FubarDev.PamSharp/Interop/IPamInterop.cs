// <copyright file="IPamInterop.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;

using Platform.Invoke.Attributes;

namespace FubarDev.PamSharp.Interop
{
    /// <summary>
    /// Interface to be implemented by the PAM bindings.
    /// </summary>
    /// <remarks>
    /// All functions are mapped to the corresponding native library functions.
    /// </remarks>
#pragma warning disable IDE1006 // Benennungsstile
#pragma warning disable SA1300 // Element should begin with upper-case letter
    [Library(PamConstants.DllName)]
    public interface IPamInterop
    {
        /// <summary>
        /// Initialization of a PAM transaction.
        /// </summary>
        /// <param name="serviceName">The name of the service to apply.</param>
        /// <param name="user">The user name.</param>
        /// <param name="conversation">Conversation information to use.</param>
        /// <param name="pamHandle">The new PAM handle.</param>
        /// <returns>A status code.</returns>
        PamStatus pam_start(
            [MarshalAs(PamConstants.STRING_TYPE)] string serviceName,
            [MarshalAs(PamConstants.STRING_TYPE)] string? user,
            PamConv conversation,
            out IntPtr pamHandle);

        /// <summary>
        /// Termination of a PAM transaction.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="pamStatus">The status of the last called PAM function.</param>
        /// <returns>A status code.</returns>
        PamStatus pam_end(IntPtr pamHandle, PamStatus pamStatus);

        /// <summary>
        /// Account authentication.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="flags">Some flags.</param>
        /// <returns>A status code.</returns>
        PamStatus pam_authenticate(IntPtr pamHandle, PamFlags flags);

        /// <summary>
        /// Establish, maintain, or delete user credentials.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="flags">Some flags.</param>
        /// <returns>A status code.</returns>
        PamStatus pam_setcred(IntPtr pamHandle, PamFlags flags);

        /// <summary>
        /// PAM account validation management.
        /// </summary>
        /// <remarks>
        /// The <see cref="pam_acct_mgmt"/> function is used to determine if the users account is valid.
        /// It checks for authentication token and account expiration and verifies access restrictions.
        /// It is typically called after the user has been authenticated.
        /// </remarks>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="flags">Some flags.</param>
        /// <returns>A status code.</returns>
        PamStatus pam_acct_mgmt(IntPtr pamHandle, PamFlags flags);

        /// <summary>
        /// Start PAM session management.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="flags">Some flags.</param>
        /// <returns>A status code.</returns>
        PamStatus pam_open_session(IntPtr pamHandle, PamFlags flags);

        /// <summary>
        /// Terminate PAM session management.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="flags">Some flags.</param>
        /// <returns>A status code.</returns>
        PamStatus pam_close_session(IntPtr pamHandle, PamFlags flags);

        /// <summary>
        /// Updating authentication tokens.
        /// </summary>
        /// <remarks>The <see cref="pam_chauthtok"/> function is used to change the authentication
        /// token for a given user (as indicated by the state associated with the handle <paramref name="pamHandle"/>).
        /// </remarks>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="flags">Some flags.</param>
        /// <returns>A status code.</returns>
        PamStatus pam_chauthtok(IntPtr pamHandle, PamFlags flags);

        /// <summary>
        /// Set and update PAM information.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="itemType">The item type to set and update.</param>
        /// <param name="item">The pointer to the item type data.</param>
        /// <returns>A status code.</returns>
        PamStatus pam_set_item(IntPtr pamHandle, PamItemTypes itemType, IntPtr item);

        /// <summary>
        /// Get PAM information.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="itemType">The item type to get.</param>
        /// <param name="item">A pointer to the item type data. It must never be freed or altered.</param>
        /// <returns>A status code.</returns>
        PamStatus pam_get_item(IntPtr pamHandle, PamItemTypes itemType, out IntPtr item);

        /// <summary>
        /// Returns a string describing the PAM error code.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="errnum">The PAM error code.</param>
        /// <returns>A pointer to a string.</returns>
        IntPtr pam_strerror(IntPtr pamHandle, int errnum);

        /// <summary>
        /// Set or change PAM environment variable.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="nameValue">A string in the form NAME(=(value)?)?.</param>
        /// <returns>A status code.</returns>
        PamStatus pam_putenv(IntPtr pamHandle, [MarshalAs(PamConstants.STRING_TYPE)] string nameValue);

        /// <summary>
        /// Get a PAM environment variable.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="name">The name of the environment variable (case-sensitive).</param>
        /// <returns>The value of the environment variable or NULL.</returns>
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
        string pam_getenv(IntPtr pamHandle, [MarshalAs(PamConstants.STRING_TYPE)] string name);

        /// <summary>
        /// Getting the PAM environment.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <returns>A list of environment variables.</returns>
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(StringArrayMarshaler))]
        string[] pam_getenvlist(IntPtr pamHandle);

        /// <summary>
        /// Request a delay on failure.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="microSeconds">The delay in microseconds.</param>
        /// <returns>A status code.</returns>
        PamStatus pam_fail_delay(IntPtr pamHandle, uint microSeconds);
    }
#pragma warning restore SA1300 // Element should begin with upper-case letter
#pragma warning restore IDE1006 // Benennungsstile
}

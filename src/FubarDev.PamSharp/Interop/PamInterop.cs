// <copyright file="PamInterop.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Reflection;
using System.Runtime.InteropServices;

#pragma warning disable SA1124 // Do not use regions
#pragma warning disable SA1300 // Element should begin with upper-case letter
#pragma warning disable IDE1006 // Benennungsstile
namespace FubarDev.PamSharp.Interop
{
    /// <summary>
    /// P/Invoke function declarations for PAM.
    /// </summary>
    public static class PamInterop
    {
#pragma warning disable SA1310 // Field names should not contain underscore
        /// <summary>
        /// We always use UTF-8.
        /// </summary>
        private const UnmanagedType STRING_TYPE = UnmanagedType.LPUTF8Str;
#pragma warning restore SA1310 // Field names should not contain underscore

        /// <summary>
        /// The default library to use.
        /// </summary>
        private const string PamDll = "pam";

        static PamInterop()
        {
            // Ensure that we find the native library by using our DLL map in the app config file
            // for this assembly.
            NativeLibrary.SetDllImportResolver(typeof(PamInterop).Assembly, MapAndLoad);
        }

        #region Transaction

        /// <summary>
        /// Initialization of a PAM transaction.
        /// </summary>
        /// <param name="serviceName">The name of the service to apply.</param>
        /// <param name="user">The user name.</param>
        /// <param name="conversation">Conversation information to use.</param>
        /// <param name="pamHandle">The new PAM handle.</param>
        /// <returns>A status code.</returns>
        [DllImport(PamDll)]
        public static extern PamStatus pam_start(
            [MarshalAs(STRING_TYPE)]
            string serviceName,
            [MarshalAs(STRING_TYPE)] string? user,
            PamConv conversation,
            out IntPtr pamHandle);

        /// <summary>
        /// Termination of a PAM transaction.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="pamStatus">The status of the last called PAM function.</param>
        /// <returns>A status code.</returns>
        [DllImport(PamDll)]
        public static extern PamStatus pam_end(IntPtr pamHandle, PamStatus pamStatus);

        #endregion

        #region Authentication

        /// <summary>
        /// Account authentication.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="flags">Some flags.</param>
        /// <returns>A status code.</returns>
        [DllImport(PamDll)]
        public static extern PamStatus pam_authenticate(IntPtr pamHandle, PamFlags flags);

        /// <summary>
        /// Establish, maintain, or delete user credentials.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="flags">Some flags.</param>
        /// <returns>A status code.</returns>
        [DllImport(PamDll)]
        public static extern PamStatus pam_setcred(IntPtr pamHandle, PamFlags flags);

        #endregion

        #region Account Management

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
        [DllImport(PamDll)]
        public static extern PamStatus pam_acct_mgmt(IntPtr pamHandle, PamFlags flags);

        #endregion

        #region Session Management

        /// <summary>
        /// Start PAM session management.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="flags">Some flags.</param>
        /// <returns>A status code.</returns>
        [DllImport(PamDll)]
        public static extern PamStatus pam_open_session(IntPtr pamHandle, PamFlags flags);

        /// <summary>
        /// Terminate PAM session management.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="flags">Some flags.</param>
        /// <returns>A status code.</returns>
        [DllImport(PamDll)]
        public static extern PamStatus pam_close_session(IntPtr pamHandle, PamFlags flags);

        #endregion

        #region Password Management

        /// <summary>
        /// Updating authentication tokens.
        /// </summary>
        /// <remarks>The <see cref="pam_chauthtok"/> function is used to change the authentication
        /// token for a given user (as indicated by the state associated with the handle <paramref name="pamHandle"/>).
        /// </remarks>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="flags">Some flags.</param>
        /// <returns>A status code.</returns>
        [DllImport(PamDll)]
        public static extern PamStatus pam_chauthtok(IntPtr pamHandle, PamFlags flags);

        #endregion

        #region Common Linux-PAM application/module PI

        /// <summary>
        /// Set and update PAM information.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="itemType">The item type to set and update.</param>
        /// <param name="item">The pointer to the item type data.</param>
        /// <returns>A status code.</returns>
        [DllImport(PamDll)]
        public static extern PamStatus pam_set_item(IntPtr pamHandle, PamItemTypes itemType, IntPtr item);

        /// <summary>
        /// Get PAM information.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="itemType">The item type to get.</param>
        /// <param name="item">A pointer to the item type data. It must never be freed or altered.</param>
        /// <returns>A status code.</returns>
        [DllImport(PamDll)]
        public static extern PamStatus pam_get_item(IntPtr pamHandle, PamItemTypes itemType, out IntPtr item);

        /// <summary>
        /// Returns a string describing the PAM error code.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="errnum">The PAM error code.</param>
        /// <returns>A pointer to a string.</returns>
        [DllImport(PamDll)]
        public static extern IntPtr pam_strerror(IntPtr pamHandle, int errnum);

        /// <summary>
        /// Set or change PAM environment variable.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="nameValue">A string in the form NAME(=(value)?)?.</param>
        /// <returns>A status code.</returns>
        [DllImport(PamDll)]
        public static extern PamStatus pam_putenv(IntPtr pamHandle, [MarshalAs(STRING_TYPE)] string nameValue);

        /// <summary>
        /// Get a PAM environment variable.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="name">The name of the environment variable (case-sensitive).</param>
        /// <returns>The value of the environment variable or NULL.</returns>
        [DllImport(PamDll)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstStringMarshaler))]
        public static extern string pam_getenv(IntPtr pamHandle, [MarshalAs(STRING_TYPE)] string name);

        /// <summary>
        /// Getting the PAM environment.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <returns>A list of environment variables.</returns>
        [DllImport(PamDll)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(StringArrayMarshaler))]
        public static extern string[] pam_getenvlist(IntPtr pamHandle);

        /// <summary>
        /// Request a delay on failure.
        /// </summary>
        /// <param name="pamHandle">The PAM handle.</param>
        /// <param name="microSeconds">The delay in microseconds.</param>
        /// <returns>A status code.</returns>
        [DllImport(PamDll)]
        public static extern PamStatus pam_fail_delay(IntPtr pamHandle, uint microSeconds);

        #endregion

        /// <summary>
        /// The callback which loads the mapped library in place of the original.
        /// </summary>
        /// <param name="libraryName">The name of the library to search for.</param>
        /// <param name="assembly">The assembly which initiated the load request.</param>
        /// <param name="dllImportSearchPath">The DLL import search path.</param>
        /// <returns>The handle for the loaded library.</returns>
        private static IntPtr MapAndLoad(string libraryName, Assembly assembly, DllImportSearchPath? dllImportSearchPath)
        {
            (bool success, IntPtr result) TryLoadLibrary(string testLibraryName)
            {
                if (NativeLibrary.TryLoad(testLibraryName, assembly, dllImportSearchPath, out var handle))
                {
                    return (true, handle);
                }

                return (false, IntPtr.Zero);
            }

            var result = NaiveDllMap.MapAndLoad(libraryName, assembly.Location + ".config", TryLoadLibrary);

            if (result.success)
            {
                return result.result;
            }

            return NativeLibrary.Load(libraryName, assembly, dllImportSearchPath);
        }

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

        /// <summary>
        /// Used to return the user's response to the PAM library.
        /// </summary>
        /// <remarks>
        /// This structure is allocated by the application program, and free()'d by
        /// the Linux-PAM library (or calling module). This means that this structure
        /// can only be passed to a P/Invoke function when it was converted manually.
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        internal readonly struct TextResponseContainer
        {
            [MarshalAs(STRING_TYPE)]
            public readonly string? Response;

            /// <summary>
            /// Currently unused, zero expected.
            /// </summary>
            public readonly int ReturnCode;

            public TextResponseContainer(string? response)
            {
                Response = response;
                ReturnCode = 0;
            }
        }
    }
}

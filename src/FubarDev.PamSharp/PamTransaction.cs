// <copyright file="PamTransaction.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

using FubarDev.PamSharp.Interop;

using Microsoft.Extensions.Logging;

namespace FubarDev.PamSharp
{
    /// <summary>
    /// The default implementation of <see cref="IPamTransaction"/>.
    /// </summary>
    internal class PamTransaction : IPamTransaction
    {
        private readonly IPamInterop _interop;
        private readonly ILogger<PamService>? _logger;

        private readonly IntPtr _handle;

        private readonly PamConv _defaultConversation;

        /// <summary>
        /// Stores the delay callback delegate.
        /// </summary>
        /// <remarks>
        /// Required to avoid garbage collection.
        /// </remarks>
        private readonly DelayCallback _delayCallback;

        private PamStatus _lastStatus = PamStatus.PAM_SUCCESS;

        private bool _failDelayEnabled;

        private bool _disposed;

        private PamConv _conversation;

        /// <summary>
        /// Initializes a new instance of the <see cref="PamTransaction"/> class.
        /// </summary>
        /// <param name="interop">The object implementing the PAM functions.</param>
        /// <param name="handle">The handle of the PAM transaction.</param>
        /// <param name="conversation">The conversation handler information.</param>
        /// <param name="logger">The logger.</param>
        public PamTransaction(
            IPamInterop interop,
            IntPtr handle,
            PamConv conversation,
            ILogger<PamService>? logger = null)
        {
            _conversation = _defaultConversation = conversation;
            _delayCallback = DelayCallback;
            _interop = interop;
            _logger = logger;
            _handle = handle;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="PamTransaction"/> class.
        /// </summary>
        ~PamTransaction()
        {
            Dispose(false);
        }

        /// <inheritdoc />
        public event EventHandler<PamDelayEventArgs>? Delay;

        /// <inheritdoc />
        public IntPtr Handle
            => !_disposed
                ? _handle
                : throw new ObjectDisposedException("The PAM transaction has already been disposed.");

        /// <inheritdoc />
        public string? ServiceName
        {
            get => GetStringItem(PamItemTypes.PAM_SERVICE);
            set => SetStringItem(PamItemTypes.PAM_SERVICE, value);
        }

        /// <inheritdoc />
        public string? UserName
        {
            get => GetStringItem(PamItemTypes.PAM_USER);
            set => SetStringItem(PamItemTypes.PAM_USER, value);
        }

        /// <inheritdoc />
        public string? TTYName
        {
            get => GetStringItem(PamItemTypes.PAM_TTY);
            set => SetStringItem(PamItemTypes.PAM_TTY, value);
        }

        /// <inheritdoc />
        public string? RemoteHostName
        {
            get => GetStringItem(PamItemTypes.PAM_RHOST);
            set => SetStringItem(PamItemTypes.PAM_RHOST, value);
        }

        /// <inheritdoc />
        public string? RemoteUserName
        {
            get => GetStringItem(PamItemTypes.PAM_RUSER);
            set => SetStringItem(PamItemTypes.PAM_RUSER, value);
        }

        /// <inheritdoc />
        [DefaultValue("login: ")]
        public string UserPrompt
        {
            get => GetStringItem(PamItemTypes.PAM_USER_PROMPT) ?? "login: ";
            set => SetStringItem(PamItemTypes.PAM_USER_PROMPT, value);
        }

        /// <inheritdoc />
        public string? AuthenticationTokenType
        {
            get => GetStringItem(PamItemTypes.PAM_AUTHTOK_TYPE);
            set => SetStringItem(PamItemTypes.PAM_AUTHTOK_TYPE, value);
        }

        /// <inheritdoc />
        public string? XDisplay
        {
            get => GetStringItem(PamItemTypes.PAM_XDISPLAY);
            set => SetStringItem(PamItemTypes.PAM_XDISPLAY, value);
        }

        /// <inheritdoc />
        public PamXAuthData? XAuthData
        {
            get => GetItem(PamItemTypes.PAM_XAUTHDATA, UnmarshalXAuthData);
            set => SetItem(PamItemTypes.PAM_XAUTHDATA, () => MarshalXAuthData(value), CleanupXAuthData);
        }

        /// <inheritdoc />
        public bool EnableFailDelayEvent
        {
            get => _failDelayEnabled;
            set
            {
                if (value)
                {
                    SetItem(PamItemTypes.PAM_FAIL_DELAY, () => Marshal.GetFunctionPointerForDelegate(_delayCallback));
                }
                else
                {
                    SetItem(PamItemTypes.PAM_FAIL_DELAY, () => IntPtr.Zero);
                }

                _failDelayEnabled = value;
            }
        }

        /// <inheritdoc />
        public PamConv Conversation
        {
            get => _conversation;
            set
            {
                var newValue = value ?? _defaultConversation;
                SetConv(newValue);
                _conversation = newValue;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public void Authenticate(PamFlags flags = 0)
        {
            CheckStatus(_interop.pam_authenticate(Handle, flags));
        }

        /// <inheritdoc />
        public void SetCredentials(PamFlags flags = 0)
        {
            CheckStatus(_interop.pam_setcred(Handle, flags));
        }

        /// <inheritdoc />
        public void ChangeAuthenticationToken(PamFlags flags = 0)
        {
            CheckStatus(_interop.pam_chauthtok(Handle, flags));
        }

        /// <inheritdoc />
        public void AccountManagement(PamFlags flags = 0)
        {
            CheckStatus(_interop.pam_acct_mgmt(Handle, flags));
        }

        /// <inheritdoc />
        public void OpenSession(PamFlags flags = 0)
        {
            CheckStatus(_interop.pam_open_session(Handle, flags));
        }

        /// <inheritdoc />
        public void CloseSession(PamFlags flags = 0)
        {
            CheckStatus(_interop.pam_close_session(Handle, flags));
        }

        /// <inheritdoc />
        public string[] GetEnvironment()
        {
            return _interop.pam_getenvlist(Handle);
        }

        /// <inheritdoc />
        public string GetEnvironment(string name)
        {
            return _interop.pam_getenv(Handle, name);
        }

        /// <inheritdoc />
        public void PutEnvironment(string nameValue)
        {
            _interop.pam_putenv(Handle, nameValue);
        }

        /// <inheritdoc />
        public void FailDelay(TimeSpan timeSpan)
        {
            var microSeconds = (uint)(timeSpan.Ticks / 10);
            CheckStatus(_interop.pam_fail_delay(Handle, microSeconds));
        }

        /// <inheritdoc />
        public T GetItem<T>(PamItemTypes itemType, Func<IntPtr, T> unmarshalFunc)
        {
            CheckStatus(_interop.pam_get_item(Handle, itemType, out var ptr));
            return unmarshalFunc(ptr);
        }

        /// <inheritdoc />
        public void SetItem(PamItemTypes itemType, Func<IntPtr> createItemDataFunc)
        {
            SetItem(itemType, createItemDataFunc, Marshal.FreeHGlobal);
        }

        /// <inheritdoc />
        public void SetItem(PamItemTypes itemType, Func<IntPtr> createItemDataFunc, Action<IntPtr> cleanupAction)
        {
            var ptr = createItemDataFunc();
            try
            {
                CheckStatus(_interop.pam_set_item(Handle, itemType, ptr));
            }
            finally
            {
                cleanupAction(ptr);
            }
        }

        private static void CleanupXAuthData(IntPtr authPtr)
        {
            if (authPtr == IntPtr.Zero)
            {
                return;
            }

            var authData = Marshal.PtrToStructure<PamXAuthDataStruct>(authPtr);
            Marshal.FreeHGlobal(authData.Data);
            Marshal.FreeHGlobal(authData.Name);
            Marshal.FreeHGlobal(authPtr);
        }

        private static IntPtr MarshalXAuthData(PamXAuthData? data)
        {
            if (data == null)
            {
                return IntPtr.Zero;
            }

            var namePtr = MarshalUtils.StringToHGlobalUTF8(data.Name, out var nameLength);
            var dataPtr = Marshal.AllocHGlobal(data.Data.Length);
            Marshal.Copy(data.Data, 0, dataPtr, data.Data.Length);
            var authData = new PamXAuthDataStruct
            {
                NameLen = nameLength,
                Name = namePtr,
                DataLen = data.Data.Length,
                Data = dataPtr,
            };

            var authPtr = Marshal.AllocHGlobal(Marshal.SizeOf<PamXAuthDataStruct>());
            Marshal.StructureToPtr(authData, authPtr, false);

            return authPtr;
        }

        private static PamXAuthData? UnmarshalXAuthData(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }

            var authData = Marshal.PtrToStructure<PamXAuthDataStruct>(ptr);
            var name = Marshal.PtrToStringUTF8(authData.Name, authData.NameLen);
            var data = new byte[authData.DataLen];
            Marshal.Copy(authData.Data, data, 0, authData.DataLen);
            return new PamXAuthData(name, data);
        }

        private string? GetStringItem(PamItemTypes itemType)
            => GetItem(itemType, MarshalUtils.PtrToStringUTF8);

        private void SetStringItem(PamItemTypes itemType, string? value)
            => SetItem(itemType, () => MarshalUtils.StringToHGlobalUTF8(value));

        private void SetConv(PamConv conv)
        {
            SetItem(
                PamItemTypes.PAM_CONV,
                () =>
                {
                    var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<PamConv>());
                    Marshal.StructureToPtr(conv, ptr, false);
                    return ptr;
                });
        }

        private void CheckStatus(PamStatus result, [CallerMemberName] string? caller = null)
        {
            if (result != PamStatus.PAM_SUCCESS)
            {
                _logger?.LogError("Action {0} failed with status {1}", caller, result);
                _lastStatus = result;
                var handle = _disposed ? IntPtr.Zero : Handle;
                throw new PamException(_interop, handle, result, caller);
            }
        }

        private void DelayCallback(PamStatus status, uint usec, IntPtr appDataPtr)
        {
            var delay = TimeSpan.FromTicks(usec * 10L);
            if (Delay != null)
            {
                Delay(this, new PamDelayEventArgs(status, delay));
            }
            else
            {
                _logger?.LogTrace("Delaying {0}µs for status {1}", usec, status);
                Thread.Sleep(delay);
            }
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // No managed objects to dispose.
            }

            _interop.pam_end(_handle, _lastStatus);
            _disposed = true;
        }
    }
}

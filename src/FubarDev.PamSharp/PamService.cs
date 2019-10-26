// <copyright file="PamService.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Runtime.CompilerServices;

using FubarDev.PamSharp.Interop;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FubarDev.PamSharp
{
    /// <summary>
    /// This service is used to create PAM transactions.
    /// </summary>
    public class PamService : IPamService
    {
        private readonly ILogger<PamService>? _logger;

        private readonly PamSessionConfiguration _configuration;

        private readonly IPamInterop _interop = PamInteropFactory.Create();

        /// <summary>
        /// Initializes a new instance of the <see cref="PamService"/> class.
        /// </summary>
        /// <param name="options">The options for the PAM service.</param>
        /// <param name="logger">The logger.</param>
        public PamService(
            IOptions<PamSessionConfiguration> options,
            ILogger<PamService>? logger = null)
        {
            _logger = logger;
            _configuration = options.Value;
        }

        /// <inheritdoc/>
        public IPamTransaction Start(
            IPamMessageHandler messageHandler,
            string? user = null)
        {
            var conversationHandler = _configuration.CreateMessaging?.Invoke(messageHandler)
                ?? new PamConversationHandler(messageHandler);

            PamStatus ConversationCallback(int messageCount, IntPtr messages, out IntPtr responseArrayPtr, IntPtr appDataPtr)
            {
                try
                {
                    return conversationHandler.Handle(messageCount, messages, out responseArrayPtr, appDataPtr);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Conversation failed with error message {0}", ex.Message);
                    responseArrayPtr = IntPtr.Zero;
                    return PamStatus.PAM_CONV_ERR;
                }
            }

            var conversation = new PamConv(ConversationCallback);
            CheckStatus(_interop.pam_start(_configuration.ServiceName, user, conversation, out var pamHandle));
            return new PamTransaction(_interop, pamHandle, conversation, _logger);
        }

        private void CheckStatus(PamStatus result, [CallerMemberName] string? caller = null)
        {
            if (result != PamStatus.PAM_SUCCESS)
            {
                _logger?.LogError("Action {0} failed with status {1}", caller, result);
                throw new PamException(_interop, IntPtr.Zero, result, caller);
            }
        }
    }
}

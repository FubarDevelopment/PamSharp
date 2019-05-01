// <copyright file="NullMessageHandler.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using FubarDev.PamSharp.Messages;

using Microsoft.Extensions.Logging;

namespace FubarDev.PamSharp.MessageHandlers
{
    /// <summary>
    /// A default message handler that does nothing except logging the messages.
    /// </summary>
    /// <remarks>
    /// This message handler returns <see cref="PamStatus.PAM_CONV_ERR"/> except
    /// for <see cref="TextInfo"/> and <see cref="ErrorMsg"/> where it returns
    /// <see cref="PamStatus.PAM_SUCCESS"/>.
    /// </remarks>
    public class NullMessageHandler : IPamMessageHandler
    {
        private readonly ILogger<NullMessageHandler>? _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullMessageHandler"/> class.
        /// </summary>
        /// <param name="logger">The optional logger.</param>
        public NullMessageHandler(ILogger<NullMessageHandler>? logger = null)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public PamStatus TextInfo(string text)
        {
            _logger?.LogInformation(text);
            return PamStatus.PAM_SUCCESS;
        }

        /// <inheritdoc />
        public PamStatus ErrorMsg(string text)
        {
            _logger?.LogError(text);
            return PamStatus.PAM_SUCCESS;
        }

        /// <inheritdoc />
        public PamResponse<string> PromptEchoOff(string text)
        {
            _logger?.LogWarning("PromptEchoOff not supported, text={0}", text);
            return PamResponse<string>.Error(PamStatus.PAM_CONV_ERR);
        }

        /// <inheritdoc />
        public PamResponse<string> PromptEchoOn(string text)
        {
            _logger?.LogWarning("PromptEchoOn not supported, text={0}", text);
            return PamResponse<string>.Error(PamStatus.PAM_CONV_ERR);
        }

        /// <inheritdoc />
        public PamResponse<string> RadioType(string text)
        {
            _logger?.LogWarning("RadioType not supported, text={0}", text);
            return PamResponse<string>.Error(PamStatus.PAM_CONV_ERR);
        }

        /// <inheritdoc />
        public PamResponse<PamBinaryData> BinaryPrompt(PamBinaryData data)
        {
            _logger?.LogWarning("BinaryPrompt not supported");
            return PamResponse<PamBinaryData>.Error(PamStatus.PAM_CONV_ERR);
        }
    }
}

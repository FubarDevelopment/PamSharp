// <copyright file="CredentialMessageHandler.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Net;

using FubarDev.PamSharp.Messages;

namespace FubarDev.PamSharp.MessageHandlers
{
    /// <summary>
    /// A <see cref="IPamMessageHandler"/> implementation that uses <see cref="NetworkCredential"/> under the hood.
    /// </summary>
    /// <remarks>
    /// The user name is returned in response to the <see cref="PromptEchoOn"/> message and
    /// the password is returned in response to the <see cref="PromptEchoOff"/> message.
    /// The messages <see cref="BinaryPrompt"/> and <see cref="RadioType"/> aren't supported.
    /// </remarks>
    public class CredentialMessageHandler : IPamMessageHandler
    {
        private readonly Func<NetworkCredential> _credentialFunc;

        private NetworkCredential? _credential;

        /// <summary>
        /// Initializes a new instance of the <see cref="CredentialMessageHandler"/> class.
        /// </summary>
        /// <param name="credential">The function to request the credentials if needed.</param>
        public CredentialMessageHandler(Func<NetworkCredential> credential)
        {
            _credentialFunc = credential;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CredentialMessageHandler"/> class.
        /// </summary>
        /// <param name="credential">The credentials to use.</param>
        public CredentialMessageHandler(NetworkCredential credential)
            : this(() => credential)
        {
        }

        private NetworkCredential Credential => _credential ??= _credentialFunc();

        /// <inheritdoc />
        public PamStatus TextInfo(string text)
        {
            return PamStatus.PAM_SUCCESS;
        }

        /// <inheritdoc />
        public PamStatus ErrorMsg(string text)
        {
            return PamStatus.PAM_SUCCESS;
        }

        /// <inheritdoc />
        public PamResponse<string> PromptEchoOff(string text)
        {
            return PamResponse<string>.Success(Credential.Password);
        }

        /// <inheritdoc />
        public PamResponse<string> PromptEchoOn(string text)
        {
            return PamResponse<string>.Success(Credential.UserName);
        }

        /// <inheritdoc />
        public PamResponse<string> RadioType(string text)
        {
            return PamResponse<string>.Error(PamStatus.PAM_CONV_ERR);
        }

        /// <inheritdoc />
        public PamResponse<PamBinaryData> BinaryPrompt(PamBinaryData data)
        {
            return PamResponse<PamBinaryData>.Error(PamStatus.PAM_CONV_ERR);
        }
    }
}

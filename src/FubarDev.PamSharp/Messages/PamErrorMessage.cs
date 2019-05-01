// <copyright file="PamErrorMessage.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

namespace FubarDev.PamSharp.Messages
{
    /// <summary>
    /// A PAM error message.
    /// </summary>
    public class PamErrorMessage : PamMessage<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PamErrorMessage"/> class.
        /// </summary>
        /// <param name="payload">The error message.</param>
        public PamErrorMessage(string payload)
            : base(PamMessageStyle.PAM_ERROR_MSG, payload)
        {
        }
    }
}

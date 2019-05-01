// <copyright file="PamTextInfoMessage.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

namespace FubarDev.PamSharp.Messages
{
    /// <summary>
    /// An informational PAM message.
    /// </summary>
    public class PamTextInfoMessage : PamMessage<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PamTextInfoMessage"/> class.
        /// </summary>
        /// <param name="payload">The payload of this message.</param>
        public PamTextInfoMessage(string payload)
            : base(PamMessageStyle.PAM_TEXT_INFO, payload)
        {
        }
    }
}

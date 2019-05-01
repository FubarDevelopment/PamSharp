// <copyright file="PamBinaryPromptMessage.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

namespace FubarDev.PamSharp.Messages
{
    /// <summary>
    /// A binary prompt message.
    /// </summary>
    public class PamBinaryPromptMessage : PamMessage<PamBinaryData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PamBinaryPromptMessage"/> class.
        /// </summary>
        /// <param name="type">The type of the binary data.</param>
        /// <param name="data">The binary data.</param>
        public PamBinaryPromptMessage(byte type, byte[] data)
            : base(PamMessageStyle.PAM_BINARY_PROMPT, new PamBinaryData(type, data))
        {
        }
    }
}

// <copyright file="PamRadioTypeMessage.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

namespace FubarDev.PamSharp.Messages
{
    /// <summary>
    /// A yes/no/maybe prompt.
    /// </summary>
    public class PamRadioTypeMessage : PamMessage<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PamRadioTypeMessage"/> class.
        /// </summary>
        /// <param name="payload">The prompt text.</param>
        public PamRadioTypeMessage(string payload)
            : base(PamMessageStyle.PAM_RADIO_TYPE, payload)
        {
        }
    }
}

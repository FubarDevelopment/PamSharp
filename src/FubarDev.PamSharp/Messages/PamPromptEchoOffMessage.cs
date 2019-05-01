// <copyright file="PamPromptEchoOffMessage.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

namespace FubarDev.PamSharp.Messages
{
    /// <summary>
    /// A user input prompt without echoing the user input.
    /// </summary>
    public class PamPromptEchoOffMessage : PamMessage<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PamPromptEchoOffMessage"/> class.
        /// </summary>
        /// <param name="payload">The prompt text.</param>
        public PamPromptEchoOffMessage(string payload)
            : base(PamMessageStyle.PAM_PROMPT_ECHO_OFF, payload)
        {
        }
    }
}

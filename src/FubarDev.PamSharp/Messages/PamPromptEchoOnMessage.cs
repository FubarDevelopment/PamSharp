// <copyright file="PamPromptEchoOnMessage.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

namespace FubarDev.PamSharp.Messages
{
    /// <summary>
    /// A user input prompt while echoing the user input.
    /// </summary>
    public class PamPromptEchoOnMessage : PamMessage<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PamPromptEchoOnMessage"/> class.
        /// </summary>
        /// <param name="payload">The prompt text.</param>
        public PamPromptEchoOnMessage(string payload)
            : base(PamMessageStyle.PAM_PROMPT_ECHO_ON, payload)
        {
        }
    }
}

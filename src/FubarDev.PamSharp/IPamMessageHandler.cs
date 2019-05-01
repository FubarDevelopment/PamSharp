// <copyright file="IPamMessageHandler.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using FubarDev.PamSharp.Messages;

namespace FubarDev.PamSharp
{
    /// <summary>
    /// Interface for a PAM message handler.
    /// </summary>
    public interface IPamMessageHandler
    {
        /// <summary>
        /// Write informational text.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <returns>The status.</returns>
        PamStatus TextInfo(string text);

        /// <summary>
        /// Write an error message.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <returns>The status.</returns>
        PamStatus ErrorMsg(string text);

        /// <summary>
        /// Request information from a user without echoing the input.
        /// </summary>
        /// <remarks>
        /// This is usually used to get the users password.
        /// </remarks>
        /// <param name="text">The text to write before turning off the input echo.</param>
        /// <returns>The user input.</returns>
        PamResponse<string> PromptEchoOff(string text);

        /// <summary>
        /// Request information from a user while echoing the input.
        /// </summary>
        /// <remarks>
        /// This is usually used to get the users login name.
        /// </remarks>
        /// <param name="text">The text to write before accepting the user input.</param>
        /// <returns>The user input.</returns>
        PamResponse<string> PromptEchoOn(string text);

        /// <summary>
        /// yes/no/maybe conditionals.
        /// </summary>
        /// <param name="text">The text to write before accepting the user input.</param>
        /// <returns>The user input.</returns>
        PamResponse<string> RadioType(string text);

        /// <summary>
        /// Server/client non-human interaction.
        /// </summary>
        /// <param name="data">The input data.</param>
        /// <returns>The response data.</returns>
        PamResponse<PamBinaryData> BinaryPrompt(PamBinaryData data);
    }
}

// <copyright file="ConsoleMessageHandler.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Text;

using FubarDev.PamSharp.Messages;

using Microsoft.Extensions.Logging;

namespace FubarDev.PamSharp.MessageHandlers
{
    /// <summary>
    /// A PAM message handler that supports password entry using the console.
    /// </summary>
    /// <remarks>The <see cref="RadioType"/> and <see cref="BinaryPrompt"/> messages
    /// aren't supported.</remarks>
    public class ConsoleMessageHandler : IPamMessageHandler
    {
        private readonly ILogger<ConsoleMessageHandler>? _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMessageHandler"/> class.
        /// </summary>
        /// <param name="logger">The logger for this message handler.</param>
        public ConsoleMessageHandler(ILogger<ConsoleMessageHandler>? logger = null)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public PamStatus TextInfo(string text)
        {
            Console.Out.WriteLine(text);
            return PamStatus.PAM_SUCCESS;
        }

        /// <inheritdoc />
        public PamStatus ErrorMsg(string text)
        {
            Console.Error.WriteLine(text);
            return PamStatus.PAM_SUCCESS;
        }

        /// <inheritdoc />
        public PamResponse<string> PromptEchoOff(string text)
        {
            Console.Out.Write(text);
            Console.Out.Flush();

            var conInput = new StringBuilder();
            var finished = false;
            while (!finished)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Backspace:
                        if (conInput.Length != 0)
                            conInput.Remove(conInput.Length - 1, 1);
                        break;
                    case ConsoleKey.Escape:
                        conInput.Clear();
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Execute:
                        finished = true;
                        break;
                    default:
                        if (key.KeyChar >= 32)
                            conInput.Append(key.KeyChar);
                        break;
                }
            }

            Console.Out.WriteLine();
            var input = conInput.ToString();

            return PamResponse<string>.Success(input);
        }

        /// <inheritdoc />
        public PamResponse<string> PromptEchoOn(string text)
        {
            Console.Out.Write(text);
            Console.Out.Flush();

            try
            {
                var input = Console.In.ReadLine();
                return PamResponse<string>.Success(input);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, ex.Message);
                return PamResponse<string>.Error(PamStatus.PAM_AUTH_ERR);
            }
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

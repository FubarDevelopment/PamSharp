// <copyright file="PamConversationHandler.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;

using FubarDev.PamSharp.Interop;
using FubarDev.PamSharp.Messages;

namespace FubarDev.PamSharp
{
    /// <summary>
    /// The default implementation of a <see cref="IPamMessageHandler"/>.
    /// </summary>
    public class PamConversationHandler : IPamConversationHandler
    {
        private readonly IPamMessageHandler _messageHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="PamConversationHandler"/> class.
        /// </summary>
        /// <param name="messageHandler">The message handler that gets the messages.</param>
        public PamConversationHandler(IPamMessageHandler messageHandler)
        {
            _messageHandler = messageHandler ?? throw new ArgumentNullException();
        }

        private interface IPamResponse
        {
            PamStatus Status { get; }
            void WriteTo(IntPtr dest);
        }

        /// <inheritdoc />
        public PamStatus Handle(int messageCount, IntPtr messages, out IntPtr responseArrayPtr, IntPtr appDataPtr)
        {
            var internalMessages = ReadMessages(messageCount, messages);
            var responseItems = new List<IPamResponse>();
            var responses = ProcessMessages(internalMessages, _messageHandler);
            foreach (var response in responses)
            {
                if (response.Status != PamStatus.PAM_SUCCESS)
                {
                    responseArrayPtr = IntPtr.Zero;
                    return response.Status;
                }

                responseItems.Add(response);
            }

            responseArrayPtr = WriteResponses(responseItems);
            return PamStatus.PAM_SUCCESS;
        }

        private static IntPtr WriteResponses(IReadOnlyList<IPamResponse> responses)
        {
            var responseSize = Marshal.SizeOf<PamInterop.BinaryResponseContainer>();
            var responseArraySize = responseSize * responses.Count;
            var responseArray = Marshal.AllocHGlobal(responseArraySize);
            for (var i = 0; i != responses.Count; ++i)
            {
                var response = responses[i];
                response.WriteTo(responseArray + (responseSize * i));
            }

            return responseArray;
        }

        /// <summary>
        /// Sends the messages to the <see cref="IPamMessageHandler"/>.
        /// </summary>
        /// <param name="messages">The messages to process.</param>
        /// <param name="messageHandler">The message handler that gets the messages.</param>
        /// <returns>The responses for the messages.</returns>
        private static IEnumerable<IPamResponse> ProcessMessages(IEnumerable<IPamMessage> messages, IPamMessageHandler messageHandler)
        {
            foreach (var message in messages)
            {
                switch (message)
                {
                    case PamMessage<string> msg when msg.MessageStyle == PamMessageStyle.PAM_TEXT_INFO:
                        yield return new PamNoResponse(messageHandler.TextInfo(msg.Payload));
                        break;
                    case PamMessage<string> msg when msg.MessageStyle == PamMessageStyle.PAM_ERROR_MSG:
                        yield return new PamNoResponse(messageHandler.ErrorMsg(msg.Payload));
                        break;
                    case PamMessage<string> msg when msg.MessageStyle == PamMessageStyle.PAM_PROMPT_ECHO_OFF:
                        yield return new PamTextResponse(messageHandler.PromptEchoOff(msg.Payload));
                        break;
                    case PamMessage<string> msg when msg.MessageStyle == PamMessageStyle.PAM_PROMPT_ECHO_ON:
                        yield return new PamTextResponse(messageHandler.PromptEchoOn(msg.Payload));
                        break;
                    case PamMessage<string> msg when msg.MessageStyle == PamMessageStyle.PAM_RADIO_TYPE:
                        yield return new PamTextResponse(messageHandler.RadioType(msg.Payload));
                        break;
                    case PamMessage<PamBinaryData> msg when msg.MessageStyle == PamMessageStyle.PAM_BINARY_PROMPT:
                        yield return new PamBinaryResponse(messageHandler.BinaryPrompt(msg.Payload));
                        break;
                    default:
                        throw new NotSupportedException($"Message style {message.MessageStyle} not supported");
                }
            }
        }

        /// <summary>
        /// Deserializes all messages.
        /// </summary>
        /// <param name="messageCount">The number of messages to deserialize.</param>
        /// <param name="messages">The pointer to the messages.</param>
        /// <returns>A list of messages read from <paramref name="messages"/>.</returns>
        private static unsafe IReadOnlyList<IPamMessage> ReadMessages(int messageCount, IntPtr messages)
        {
            var result = new List<IPamMessage>();
            for (var i = 0; i != messageCount; ++i)
            {
                var messagePtrPtr = messages + (i * IntPtr.Size);
                var message = *(PamMessage**)messagePtrPtr;

                var messageStyle = message->MsgStyle;
                switch (messageStyle)
                {
                    case PamMessageStyle.PAM_TEXT_INFO:
                        result.Add(ReadTextInfo(message, payload => new PamTextInfoMessage(payload)));
                        break;
                    case PamMessageStyle.PAM_ERROR_MSG:
                        result.Add(ReadTextInfo(message, payload => new PamErrorMessage(payload)));
                        break;
                    case PamMessageStyle.PAM_PROMPT_ECHO_OFF:
                        result.Add(ReadTextInfo(message, payload => new PamPromptEchoOffMessage(payload)));
                        break;
                    case PamMessageStyle.PAM_PROMPT_ECHO_ON:
                        result.Add(ReadTextInfo(message, payload => new PamPromptEchoOnMessage(payload)));
                        break;
                    case PamMessageStyle.PAM_RADIO_TYPE:
                        result.Add(ReadTextInfo(message, payload => new PamRadioTypeMessage(payload)));
                        break;
                    case PamMessageStyle.PAM_BINARY_PROMPT:
                        result.Add(ReadBinaryInfo(message));
                        break;
                    default:
                        throw new NotSupportedException($"Message style {messageStyle} not supported");
                }
            }

            return result;
        }

        private static unsafe IPamMessage ReadBinaryInfo(PamMessage* messagePtr)
        {
            var binaryPtr = messagePtr->Message;
            var binary = (PamBinaryMessageData*)binaryPtr;
            var length = IPAddress.NetworkToHostOrder((int)binary->Length);
            var control = binary->Type;
            var data = new byte[length - 5];
            Marshal.Copy(binaryPtr + 5, data, 0, length - 5);
            return new PamBinaryPromptMessage(control, data);
        }

        private static unsafe IPamMessage ReadTextInfo(PamMessage* messagePtr, Func<string, IPamMessage> createMessageFunc)
        {
            var text = Marshal.PtrToStringUTF8(messagePtr->Message);
            return createMessageFunc(text);
        }

        private static void WriteTextResponse(IntPtr dest, string? message)
        {
            var response = new PamInterop.TextResponseContainer(message);
            Marshal.StructureToPtr(response, dest, false);
        }

        private static unsafe void WriteBinaryResponse(IntPtr dest, byte control, ReadOnlyMemory<byte> data)
        {
            var totalLength = data.Length + 5;
            var binaryBuffer = Marshal.AllocHGlobal(totalLength);
            var bufferSpan = new Span<byte>(binaryBuffer.ToPointer(), totalLength);
            var lengthBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(totalLength));

            lengthBytes.AsSpan().CopyTo(bufferSpan);
            bufferSpan[4] = control;
            data.Span.CopyTo(bufferSpan.Slice(5));

            var response = new PamInterop.BinaryResponseContainer(binaryBuffer);
            Marshal.StructureToPtr(response, dest, false);
        }

        private class PamNoResponse : IPamResponse
        {
            public PamNoResponse(PamStatus status)
            {
                Status = status;
            }

            public PamStatus Status { get; }

            public void WriteTo(IntPtr dest)
            {
                WriteTextResponse(dest, null);
            }
        }

        private class PamTextResponse : IPamResponse
        {
            private readonly PamResponse<string> _response;

            public PamTextResponse(PamResponse<string> response)
            {
                _response = response;
            }

            public PamStatus Status => _response.Status;

            public void WriteTo(IntPtr dest)
            {
                WriteTextResponse(dest, _response.Payload);
            }
        }

        private class PamBinaryResponse : IPamResponse
        {
            private readonly PamResponse<PamBinaryData> _response;

            public PamBinaryResponse(PamResponse<PamBinaryData> response)
            {
                _response = response;
            }

            public PamStatus Status => _response.Status;

            public void WriteTo(IntPtr dest)
            {
                WriteBinaryResponse(dest, _response.Payload.Type, _response.Payload.Data);
            }
        }
    }
}

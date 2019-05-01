// <copyright file="PamMessage{T}.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

namespace FubarDev.PamSharp.Messages
{
    /// <summary>
    /// A default implementation of a PAM message.
    /// </summary>
    /// <typeparam name="T">The type of the payload.</typeparam>
    public abstract class PamMessage<T> : IPamMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PamMessage{T}"/> class.
        /// </summary>
        /// <param name="messageStyle">The message style.</param>
        /// <param name="payload">The message payload.</param>
        protected PamMessage(PamMessageStyle messageStyle, T payload)
        {
            MessageStyle = messageStyle;
            Payload = payload;
        }

        /// <inheritdoc />
        public PamMessageStyle MessageStyle { get; }

        /// <summary>
        /// Gets the message payload.
        /// </summary>
        public T Payload { get; }
    }
}

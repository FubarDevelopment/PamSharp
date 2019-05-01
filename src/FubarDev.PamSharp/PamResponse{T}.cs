// <copyright file="PamResponse{T}.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;

namespace FubarDev.PamSharp
{
    /// <summary>
    /// A PAM message response type.
    /// </summary>
    /// <typeparam name="T">The type of the payload.</typeparam>
    public class PamResponse<T>
    {
        private readonly Func<T> _payloadFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="PamResponse{T}"/> class.
        /// </summary>
        /// <param name="status">The PAM status.</param>
        /// <param name="payloadFunc">A function to get the payload.</param>
        private PamResponse(PamStatus status, Func<T> payloadFunc)
        {
            _payloadFunc = payloadFunc;
            Status = status;
        }

        /// <summary>
        /// Gets the PAM status.
        /// </summary>
        public PamStatus Status { get; }

        /// <summary>
        /// Gets the payload if the <see cref="Status"/> is <see cref="PamStatus.PAM_SUCCESS"/>.
        /// </summary>
        public T Payload => _payloadFunc();

        /// <summary>
        /// Creates a PAM response with payload (successful).
        /// </summary>
        /// <param name="payload">The payload of the response.</param>
        /// <returns>The created PAM response.</returns>
        public static PamResponse<T> Success(T payload)
        {
            return new PamResponse<T>(PamStatus.PAM_SUCCESS, () => payload);
        }

        /// <summary>
        /// Creates a PAM response with an error status.
        /// </summary>
        /// <param name="status">The PAM error status.</param>
        /// <returns>The created PAM response.</returns>
        public static PamResponse<T> Error(PamStatus status)
        {
            if (status == PamStatus.PAM_SUCCESS)
                throw new ArgumentOutOfRangeException(nameof(status), "Status must not indicate success");
            return new PamResponse<T>(status, () => throw new InvalidOperationException("No payload available."));
        }
    }
}

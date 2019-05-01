// <copyright file="PamXAuthData.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

namespace FubarDev.PamSharp
{
    /// <summary>
    /// PAM X authentication data.
    /// </summary>
    public class PamXAuthData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PamXAuthData"/> class.
        /// </summary>
        /// <param name="name">The authentication method.</param>
        /// <param name="data">The authentication method-specific data.</param>
        public PamXAuthData(string name, byte[] data)
        {
            Name = name;
            Data = data;
        }

        /// <summary>
        /// Gets the authentication method.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the authentication method-specific data.
        /// </summary>
        public byte[] Data { get; }
    }
}

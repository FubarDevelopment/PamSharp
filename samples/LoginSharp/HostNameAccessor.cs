// <copyright file="HostNameAccessor.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;

namespace LoginSharp
{
    /// <summary>
    /// Accessor for the host name.
    /// </summary>
    public class HostNameAccessor
    {
        /// <summary>
        /// Gets the host name.
        /// </summary>
        /// <returns>the host name.</returns>
        public string GetHostName()
        {
            return Environment.MachineName;
        }
    }
}

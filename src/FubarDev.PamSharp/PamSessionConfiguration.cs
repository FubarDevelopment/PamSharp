// <copyright file="PamSessionConfiguration.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;

namespace FubarDev.PamSharp
{
    /// <summary>
    /// Default PAM session configuration.
    /// </summary>
    public class PamSessionConfiguration
    {
        /// <summary>
        /// Gets or sets the service name.
        /// </summary>
        [DefaultValue("passwd")]
        public string ServiceName { get; set; } = "passwd";

        /// <summary>
        /// Gets or sets a custom function to create a PAM conversation handler.
        /// </summary>
        public Func<IPamMessageHandler, IPamConversationHandler>? CreateMessaging { get; set; }
    }
}

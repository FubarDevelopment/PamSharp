// <copyright file="PamServiceBuilder{TMessageHandler}.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using Microsoft.Extensions.DependencyInjection;

namespace FubarDev.PamSharp.Tests
{
    public class PamServiceBuilder<TMessageHandler> : PamServiceBuilder
        where TMessageHandler : class, IPamMessageHandler
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IPamMessageHandler, TMessageHandler>();
        }
    }
}

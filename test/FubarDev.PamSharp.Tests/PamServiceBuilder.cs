// <copyright file="PamServiceBuilder.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;

using Microsoft.Extensions.DependencyInjection;

namespace FubarDev.PamSharp.Tests
{
    public abstract class PamServiceBuilder : IDisposable
    {
        private readonly Lazy<ServiceProvider> _serviceProvider;

        public PamServiceBuilder()
        {
            _serviceProvider = new Lazy<ServiceProvider>(CreateServiceProvider);
        }

        public ServiceProvider ServiceProvider => _serviceProvider.Value;

        public void Dispose()
        {
            if (_serviceProvider.IsValueCreated)
            {
                _serviceProvider.Value.Dispose();
            }
        }

        protected abstract void ConfigureServices(IServiceCollection services);

        private ServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();
            services
                .AddOptions()
                .AddScoped<IPamService, PamService>();
            ConfigureServices(services);
            return services.BuildServiceProvider();
        }
    }
}

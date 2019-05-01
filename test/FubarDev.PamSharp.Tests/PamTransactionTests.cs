// <copyright file="PamTransactionTests.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;

using FubarDev.PamSharp.MessageHandlers;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace FubarDev.PamSharp.Tests
{
    public class PamTransactionTests : IClassFixture<PamServiceBuilder<NullMessageHandler>>
    {
        private readonly IServiceProvider _serviceProvider;

        public PamTransactionTests(PamServiceBuilder<NullMessageHandler> pamServiceBuilder)
        {
            _serviceProvider = pamServiceBuilder.ServiceProvider;
        }

        [Fact]
        public void CanRequestTransaction()
        {
            IPamTransaction pamTransaction;
            using (var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IPamService>();
                pamTransaction = service.Start();
            }

            Assert.NotEqual(IntPtr.Zero, pamTransaction.Handle);

            pamTransaction.Dispose();

            Assert.Throws<ObjectDisposedException>(() => pamTransaction.Handle);
        }

        [Fact]
        public void CanStartAndStopTransaction()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IPamService>();
                using (service.Start())
                {
                    // Do nothing
                }
            }
        }
    }
}

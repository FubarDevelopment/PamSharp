// <copyright file="Program.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

using FubarDev.PamSharp;
using FubarDev.PamSharp.MessageHandlers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Mono.Unix;

using Syslog.Framework.Logging;
using Syslog.Framework.Logging.TransportProtocols;

using Process = System.Diagnostics.Process;

namespace LoginSharp
{
    internal class Program
    {
        private static Task<int> Main(string[] args)
        {
            var root = new RootCommand
            {
                Description = "Example login shell written in C#",
                Handler = CommandHandler.Create(ExecuteLogin),
            };

            return root.InvokeAsync(args);
        }

        private static void ExecuteLogin()
        {
            using var services = CreateServices();

            // Enable logging
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddSyslog(new SyslogLoggerSettings()
            {
                MessageTransportProtocol = TransportProtocol.UnixSocket,
                UnixSocketPath = "/dev/log",
            });

            var hostName = services.GetRequiredService<HostNameAccessor>().GetHostName();

            var rootInfo = new RootUserInfo();

            Console.Clear();

            while (true)
            {
                var service = services.GetRequiredService<IPamService>();
                var messageHandler = services.GetRequiredService<IPamMessageHandler>();
                using var transaction = service.Start(messageHandler);
                transaction.UserPrompt = $"{hostName} login: ";
                transaction.UserName = null;

                try
                {
                    transaction.Authenticate();
                    transaction.AccountManagement();

                    var userInfo = new UnixUserInfo(transaction.UserName);
                    if (string.IsNullOrEmpty(userInfo.ShellProgram))
                    {
                        Console.Error.WriteLine("No shell specified. Login not possible.");
                    }

                    transaction.OpenSession();
                    try
                    {
                        // Start the shell
                        ProcessStartInfo startInfo;
                        if (rootInfo.IsRoot || rootInfo.IsSudo)
                        {
                            startInfo = new ProcessStartInfo("sudo", $"-i -u #{userInfo.UserId}")
                            {
                                WorkingDirectory = userInfo.HomeDirectory,
                            };
                        }
                        else
                        {
                            if (userInfo.UserId != rootInfo.Info.UserId)
                            {
                                throw new InvalidOperationException(
                                    "A non-root user can only \"login\" with its own credentials.");
                            }
                            else
                            {
                                startInfo = new ProcessStartInfo(userInfo.ShellProgram, "-pl")
                                {
                                    WorkingDirectory = userInfo.HomeDirectory,
                                };
                            }
                        }

                        using var proc = Process.Start(startInfo);
                        if (proc == null)
                        {
                            throw new InvalidOperationException("The shell couldn't be started.");
                        }

                        proc.WaitForExit();
                    }
                    finally
                    {
                        transaction.CloseSession();
                    }

                    // Clear after logout
                    Console.Clear();
                }
                catch (Exception exception)
                {
                    var realException = UnwindException(exception);
                    Console.Error.WriteLine(realException.Message);
                }
            }

            // ReSharper disable once FunctionNeverReturns
        }

        private static Exception UnwindException(Exception exception)
        {
            Exception oldException;

            do
            {
                oldException = exception;
                switch (exception)
                {
                    case TargetInvocationException invocationException when invocationException.InnerException != null:
                        exception = invocationException.InnerException;
                        break;
                    case AggregateException aggregateException:
                        exception = aggregateException.InnerException;
                        break;
                }
            }
            while (oldException != exception);

            return exception;
        }

        private static ServiceProvider CreateServices()
        {
            var services = new ServiceCollection()
                .AddOptions()
                .AddLogging()
                .AddSingleton<IPamService, PamService>()
                .AddSingleton<IPamMessageHandler, ConsoleMessageHandler>()
                .AddSingleton<HostNameAccessor>();

            return services.BuildServiceProvider();
        }
    }
}

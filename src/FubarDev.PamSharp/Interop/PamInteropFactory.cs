// <copyright file="PamInteropFactory.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

using FubarDev.PamSharp.Interop.UnixDl2;

using Platform.Invoke;

namespace FubarDev.PamSharp.Interop
{
    /// <summary>
    /// Factory to create an object implementing the PAM API.
    /// </summary>
    public static class PamInteropFactory
    {
        private static readonly object _lock = new object();
        private static IPamInterop? _interop;

        /// <summary>
        /// Returns a new object that implements the PAM API.
        /// </summary>
        /// <returns>a new object that implements the PAM API.</returns>
        public static IPamInterop Create()
        {
            if (_interop == null)
            {
                lock (_lock)
                {
                    if (_interop == null)
                    {
                        _interop = CreatePamInterop();
                    }

                    return _interop;
                }
            }

            return _interop;
        }

        /// <summary>
        /// The callback which loads the mapped library in place of the original.
        /// </summary>
        /// <param name="libraryName">The name of the library to search for.</param>
        /// <param name="assembly">The assembly which initiated the load request.</param>
        /// <param name="libraryLoader">The library loader.</param>
        /// <returns>The handle for the loaded library.</returns>
        private static ILibrary? MapAndLoad(string libraryName, Assembly assembly, ILibraryLoader libraryLoader)
        {
            (bool success, ILibrary? result) TryLoadLibrary(string testLibraryName)
            {
                var library = libraryLoader.Load(testLibraryName);
                return (library != null, library);
            }

            var result = NaiveDllMap.MapAndLoad(libraryName, assembly.Location + ".config", TryLoadLibrary);
            if (result.success)
            {
                return result.result;
            }

            return null;
        }

        private static IPamInterop CreatePamInterop()
        {
            var exceptions = new List<Exception>();
            foreach (var libraryLoader in GetLibraryLoaders())
            {
                try
                {
                    var library = MapAndLoad("pam", typeof(IPamInterop).Assembly, libraryLoader);
                    if (library != null)
                    {
                        return LibraryInterfaceFactory.Implement<IPamInterop>(library);
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(
                "Unsupported operating system, no viable library loader or library not found.",
                exceptions);
        }

        private static IEnumerable<ILibraryLoader> GetLibraryLoaders()
        {
            yield return LibraryLoaderFactory.Create();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                yield return new UnixDl2LibraryLoader();
            }
        }
    }
}

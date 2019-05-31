// <copyright file="PamInteropFactory.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Reflection;

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
            lock (_lock)
            {
                if (_interop == null)
                {
                    var libraryLoader = LibraryLoaderFactory.Create();
                    var library = MapAndLoad("pam", typeof(IPamInterop).Assembly, libraryLoader);
                    if (library == null)
                    {
                        throw new NotSupportedException("Unsupported operating system, no viable library loader or library not found");
                    }

                    _interop = LibraryInterfaceFactory.Implement<IPamInterop>(library);
                }

                return _interop;
            }
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
    }
}

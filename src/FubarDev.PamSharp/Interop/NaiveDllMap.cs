// <copyright file="NaiveDllMap.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;

using FubarDev.PamSharp.DllMap;

namespace FubarDev.PamSharp.Interop
{
    /// <summary>
    /// This class is a naive dllmap implementation.
    /// </summary>
    internal static class NaiveDllMap
    {
        internal const string DllMappingFilename = "dllmap.config";

        /// <summary>
        /// Try to map and load a native library.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the probe function.</typeparam>
        /// <param name="libraryName">The name of the library to map and/or load.</param>
        /// <param name="dllConfigPath">The path to the configuration file.</param>
        /// <param name="probeFunc">The probe function to load the native library.</param>
        /// <returns>A tuple that indicates success/failure and the returned value by the probe function.</returns>
        public static (bool success, T result) MapAndLoad<T>(string libraryName, string dllConfigPath, Func<string, (bool succes, T result)> probeFunc)
        {
            var additionalConfigPath = Path.GetDirectoryName(dllConfigPath) + DllMappingFilename;

            IEnumerable<DllMapItem> dllMapItems;
            if (File.Exists(dllConfigPath))
            {
                var root = XElement.Load(dllConfigPath);
                dllMapItems = DllMapItem.LoadDllMap(root.Elements("dllmap"));
            }
            else if (File.Exists(additionalConfigPath))
            {
                var root = XElement.Load(additionalConfigPath);
                dllMapItems = DllMapItem.LoadDllMap(root.Elements("dllmap"));
            }
            else
            {
                dllMapItems = GetDefaultDllMap();
            }

            var items = Filter(dllMapItems, libraryName);
            return MapAndLoad(items, probeFunc)
                ?? probeFunc(libraryName);
        }

        /// <summary>
        /// Gets default DLL mapping config.
        /// </summary>
        /// <returns>A default array of <see cref="DllMapItem"/> objects.</returns>
        internal static IEnumerable<DllMapItem> GetDefaultDllMap()
        {
            return new[]
            {
                new DllMapItem("pam", "libpam.so.0", new[] { new DllMapOsSelection(false, OSPlatform.Linux), }),
                new DllMapItem("pam", "libpam.so", new[] { new DllMapOsSelection(false, OSPlatform.Linux), }),
                new DllMapItem("pam", "libpam.so.0", new[] { new DllMapOsSelection(false, DllMapOsSelection.GetPlatform("FreeBSD")), }),
                new DllMapItem("pam", "libpam.so", new[] { new DllMapOsSelection(false, DllMapOsSelection.GetPlatform("FreeBSD")), }),
                new DllMapItem("pam", "libpam.2.dylib", new[] { new DllMapOsSelection(false, OSPlatform.OSX), }),
                new DllMapItem("pam", "libpam.1.dylib", new[] { new DllMapOsSelection(false, OSPlatform.OSX), }),
                new DllMapItem("pam", "libpam.dylib", new[] { new DllMapOsSelection(false, OSPlatform.OSX), }),
            };
        }

        /// <summary>
        /// Constructs full path to additional DLL map conifg file.
        /// </summary>
        /// <param name="dllConfigPath">The app conifg file path.</param>
        /// <returns>The full path to additional config file with DLL map.</returns>
        internal static string ConstructAdditionalConfigFile(string dllConfigPath)
        {
            return Path.GetDirectoryName(dllConfigPath) + Path.DirectorySeparatorChar + DllMappingFilename;
        }

        /// <summary>
        /// Try to map and load a native library using an enumeration of <see cref="DllMapItem"/> objects.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the probe function.</typeparam>
        /// <param name="items">The <see cref="DllMapItem"/> objects used to map the native library.</param>
        /// <param name="probeFunc">The probe function to load the native library.</param>
        /// <returns>A tuple that indicates success/failure and the returned value by the probe function.</returns>
        private static (bool succes, T result)? MapAndLoad<T>(
            IEnumerable<DllMapItem> items,
            Func<string, (bool succes, T result)> probeFunc)
        {
            foreach (var item in items)
            {
                var probeResult = probeFunc(item.Target);
                if (probeResult.succes)
                {
                    return probeResult;
                }
            }

            return null;
        }

        /// <summary>
        /// Filter by name and operating system.
        /// </summary>
        /// <param name="items">The <see cref="DllMapItem"/> objects used to map the native library.</param>
        /// <param name="libraryName">The name of the library to load.</param>
        /// <returns>A filtered list of <see cref="DllMapItem"/> objects.</returns>
        private static IEnumerable<DllMapItem> Filter(IEnumerable<DllMapItem> items, string libraryName)
        {
            return
                from item in items
                where item.Dll == libraryName
                let operatingSystems = item.OperatingSystems
                where operatingSystems.Length == 0 || operatingSystems.Any(os => os.IsMatch())
                select item;
        }
    }
}

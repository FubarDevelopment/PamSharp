// <copyright file="DllMapItem.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FubarDev.PamSharp.DllMap
{
    /// <summary>
    /// Represents a <c>dllmap</c> configuration entry.
    /// </summary>
    internal class DllMapItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DllMapItem"/> class.
        /// </summary>
        /// <param name="dll">The DLL to search for.</param>
        /// <param name="target">The DLL to redirect to.</param>
        /// <param name="operatingSystems">The selected operating system platforms to test against.</param>
        public DllMapItem(string dll, string target, DllMapOsSelection[] operatingSystems)
        {
            Dll = dll;
            Target = target;
            OperatingSystems = operatingSystems;
        }

        /// <summary>
        /// Gets the DLL to search for.
        /// </summary>
        public string Dll { get; }

        /// <summary>
        /// Gets the DLL the <see cref="Dll"/> needs to be redirected to.
        /// </summary>
        public string Target { get; }

        /// <summary>
        /// Gets the operation system selections to test against.
        /// </summary>
        public DllMapOsSelection[] OperatingSystems { get; }

        /// <summary>
        /// Load all <see cref="DllMapItem"/> entries from the found dllmap XML elements.
        /// </summary>
        /// <param name="dllMaps">The dllmap XML elements.</param>
        /// <returns>The found <see cref="DllMapItem"/> objects.</returns>
        public static IEnumerable<DllMapItem> LoadDllMap(IEnumerable<XElement> dllMaps)
        {
            return
                from el in dllMaps
                let dll = el.Attribute("dll")
                let target = el.Attribute("target")
                let os = el.Attribute("os")
                where !string.IsNullOrEmpty(dll?.Value)
                where !string.IsNullOrEmpty(target?.Value)
                select new DllMapItem(
                    dll.Value,
                    target.Value,
                    DllMapOsSelection.GetOsValues(os?.Value));
        }
    }
}

// <copyright file="DllMapOsSelection.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FubarDev.PamSharp.DllMap
{
    internal class DllMapOsSelection : IEquatable<DllMapOsSelection>
    {
        private static readonly ConcurrentDictionary<string, OSPlatform> _wellKnownOsPlatforms =
            new ConcurrentDictionary<string, OSPlatform>(
                typeof(OSPlatform)
                    .GetProperties(BindingFlags.GetProperty | BindingFlags.Static | BindingFlags.Public)
                    .Select(x => (OSPlatform)x.GetValue(null)!)
                    .ToDictionary(x => x.ToString()), StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="DllMapOsSelection"/> class.
        /// </summary>
        /// <param name="invert">Indicates whether the check result will be inverted.</param>
        /// <param name="osPlatform">The <see cref="OSPlatform"/> to test for.</param>
        public DllMapOsSelection(bool invert, OSPlatform osPlatform)
        {
            Invert = invert;
            OsPlatform = osPlatform;
        }

        /// <summary>
        /// Gets a value indicating whether the match result should be inverted.
        /// </summary>
        public bool Invert { get; }

        /// <summary>
        /// Gets the OS platform this selection should be tested against.
        /// </summary>
        public OSPlatform OsPlatform { get; }

        /// <summary>
        /// Get a list of operating systems as specified in a <c>dllmap</c> XML element.
        /// </summary>
        /// <param name="osSelection">The OS selection.</param>
        /// <returns>The list of found OS entries.</returns>
        public static DllMapOsSelection[] GetOsValues(string? osSelection)
        {
            if (string.IsNullOrEmpty(osSelection))
            {
                return Array.Empty<DllMapOsSelection>();
            }

            return
                (from osNameRaw in osSelection.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x))
                    let isInverted = osNameRaw.StartsWith('!')
                    let osName = isInverted ? osNameRaw.Substring(1).TrimStart() : osNameRaw
                    let osPlatform = GetPlatform(osName)
                    select new DllMapOsSelection(isInverted, osPlatform))
                .ToArray();
        }

        /// <summary>
        /// Determine whether this OS selection matches the current OS.
        /// </summary>
        /// <returns>true if this OS selection matches the current OS.</returns>
        public bool IsMatch()
        {
            return Invert
                ? !RuntimeInformation.IsOSPlatform(OsPlatform)
                : RuntimeInformation.IsOSPlatform(OsPlatform);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DllMapOsSelection);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Invert, OsPlatform);
        }

        public bool Equals([AllowNull] DllMapOsSelection other)
        {
            if (other is null)
            {
                return false;
            }

            return Invert.Equals(other.Invert) &&
                   OsPlatform.Equals(other.OsPlatform);
        }

        /// <summary>
        /// Gets the <see cref="OsPlatform"/> for the given name.
        /// </summary>
        /// <param name="name">The OS name.</param>
        /// <returns>The found or a new <see cref="OSPlatform"/>.</returns>
        internal static OSPlatform GetPlatform(string name)
        {
            switch (name.ToLowerInvariant())
            {
                case "bsd":
                case "openbsd":
                    name = "FreeBSD";
                    break;
            }

            return _wellKnownOsPlatforms.GetOrAdd(name, n => OSPlatform.Create(n.ToUpperInvariant()));
        }
    }
}

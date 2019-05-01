// <copyright file="DllMapTests.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using System.Xml.Linq;

using FubarDev.PamSharp.DllMap;

using Xunit;

namespace FubarDev.PamSharp.Tests
{
    public class DllMapTests
    {
        public static object[][] SimpleOsValues =
        {
            new object[] { "windows", OSPlatform.Windows },
            new object[] { "Windows", OSPlatform.Windows },
            new object[] { "linux", OSPlatform.Linux },
            new object[] { "Linux", OSPlatform.Linux },
            new object[] { "freebsd", OSPlatform.FreeBSD },
            new object[] { "FreeBSD", OSPlatform.FreeBSD },
        };

        public static object[][] CustomOsValues =
        {
            new object[] { "openbsd", OSPlatform.FreeBSD.ToString() },
            new object[] { "OpenBSD", OSPlatform.FreeBSD.ToString() },
            new object[] { "bsd", OSPlatform.FreeBSD.ToString() },
            new object[] { "BSD", OSPlatform.FreeBSD.ToString() },
            new object[] { "whatever", "whatever" },
        };

        [Fact]
        public void CanParseSimpleDllMap()
        {
            var loadedDllMaps = DllMapItem.LoadDllMap(new[]
            {
                new XElement(
                    "dllmap",
                    new XAttribute("dll", "dllToSearchFor"),
                    new XAttribute("target", "dllToReplaceWith")),
            });

            Assert.Collection(
                loadedDllMaps,
                dllMap =>
                {
                    Assert.Equal("dllToSearchFor", dllMap.Dll);
                    Assert.Equal("dllToReplaceWith", dllMap.Target);
                    Assert.Empty(dllMap.OperatingSystems);
                });
        }

        [Fact]
        public void DllMapGetsRemovedWhenDllAttributeMissingOrEmpty()
        {
            var loadedDllMaps = DllMapItem.LoadDllMap(new[]
            {
                new XElement(
                    "dllmap",
                    new XAttribute("dll", string.Empty),
                    new XAttribute("target", "dllToReplaceWith1")),
                new XElement(
                    "dllmap",
                    new XAttribute("target", "dllToReplaceWith2")),
            });

            Assert.Empty(loadedDllMaps);
        }

        [Fact]
        public void DllMapGetsRemovedWhenTargetAttributeMissingOrEmpty()
        {
            var loadedDllMaps = DllMapItem.LoadDllMap(new[]
            {
                new XElement(
                    "dllmap",
                    new XAttribute("dll", "dllToSearchFor1"),
                    new XAttribute("target", string.Empty)),
                new XElement(
                    "dllmap",
                    new XAttribute("dll", "dllToSearchFor2")),
            });

            Assert.Empty(loadedDllMaps);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AllowEmptyOsSelection(string osValue)
        {
            Assert.Empty(DllMapOsSelection.GetOsValues(osValue));
        }

        [Theory]
#pragma warning disable xUnit1019
        [MemberData(nameof(SimpleOsValues))]
#pragma warning restore xUnit1019
        public void AllowSimpleOsSelection(string osValue, OSPlatform osPlatform)
        {
            Assert.Collection(
                DllMapOsSelection.GetOsValues(osValue),
                osSelection =>
                {
                    Assert.False(osSelection.Invert);
                    Assert.Equal(osPlatform, osSelection.OsPlatform);
                });
        }

        [Theory]
#pragma warning disable xUnit1019
        [MemberData(nameof(CustomOsValues))]
#pragma warning restore xUnit1019
        public void AllowCustomOsSelection(string osValue, string expectedValue)
        {
            Assert.Collection(
                DllMapOsSelection.GetOsValues(osValue),
                osSelection =>
                {
                    Assert.False(osSelection.Invert);
                    Assert.Equal(expectedValue, osSelection.OsPlatform.ToString());
                });
        }

        [Fact]
        public void CustomOsPlatformIsRemembered()
        {
            Assert.Collection(
                DllMapOsSelection.GetOsValues("whatever"),
                osSelection =>
                {
                    Assert.False(osSelection.Invert);
                    Assert.Equal("whatever", osSelection.OsPlatform.ToString());
                });
            Assert.Collection(
                DllMapOsSelection.GetOsValues("WhAtEvEr"),
                osSelection =>
                {
                    Assert.False(osSelection.Invert);
                    Assert.Equal("whatever", osSelection.OsPlatform.ToString());
                });
        }
    }
}

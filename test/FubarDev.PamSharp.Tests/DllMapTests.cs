// <copyright file="DllMapTests.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;

using FubarDev.PamSharp.DllMap;
using FubarDev.PamSharp.Interop;
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
            new object[] { "freebsd", OSPlatform.Create("FREEBSD") },
            new object[] { "FreeBSD", OSPlatform.Create("FREEBSD") },
        };

        public static object[][] CustomOsValues =
        {
            new object[] { "openbsd", "FREEBSD" },
            new object[] { "OpenBSD", "FREEBSD" },
            new object[] { "bsd", "FREEBSD" },
            new object[] { "BSD", "FREEBSD" },
            new object[] { "whatever", "WHATEVER" },
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
                    Assert.Equal("WHATEVER", osSelection.OsPlatform.ToString());
                });
            Assert.Collection(
                DllMapOsSelection.GetOsValues("WhAtEvEr"),
                osSelection =>
                {
                    Assert.False(osSelection.Invert);
                    Assert.Equal("WHATEVER", osSelection.OsPlatform.ToString());
                });
        }

        [Fact]
        public void CompareDefaultDllMappingWithConifgFile()
        {
            var defaultDllMapItems = NaiveDllMap.GetDefaultDllMap().ToList();

            var additionalConfigPath = NaiveDllMap.ConstructAdditionalConfigFile(typeof(IPamService).Assembly.Location);
            var root = XElement.Load(additionalConfigPath);
            var configDllMapItems = DllMapItem.LoadDllMap(root.Elements("dllmap")).ToList();

            Assert.Equal(defaultDllMapItems, configDllMapItems);
        }
    }
}

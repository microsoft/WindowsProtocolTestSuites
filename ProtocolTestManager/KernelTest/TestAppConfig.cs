// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestManager.Kernel;

namespace CodeCoverage
{
    [TestClass]
    public class TestAppConfig
    {
        [TestMethod]
        public void LoadConfigFile()
        {
            AppConfig appConfig = AppConfig.LoadConfig("TestAppConfig","0.0.0.0", @".\Resources", @".\Resources");
            Assert.AreEqual(
                2,
                appConfig.PtfConfigFiles.Count,
                "2 ptfconfig files.");
            Assert.AreEqual(
                2,
                appConfig.TestSuiteAssembly.Count,
                "2 test suite dlls.");
            Assert.AreEqual(
                @"c:\Path\To\Detector.dll",
                appConfig.DetectorAssemblyPath,
                "Verify the detector path.");
            Assert.IsNotNull(
                appConfig.AdapterDefinitions,
                "Find adapters definition.");
            Assert.AreEqual(
                1,
                appConfig.PredefinedAdapters.Count,
                "1 Adapter is defined");
            Assert.AreEqual(
                "ISutControlAdapter",
                appConfig.PredefinedAdapters[0].InteractiveAdapter.Name,
                "Interactive adapter is defined");
            Assert.AreEqual(
                "ISutControlAdapter",
                appConfig.PredefinedAdapters[0].PowerShellAdapter.Name,
                "PowerShell adapter is defined");
        }
    }
}

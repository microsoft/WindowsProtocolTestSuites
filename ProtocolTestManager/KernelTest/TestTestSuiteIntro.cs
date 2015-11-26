// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestManager.Kernel;

namespace CodeCoverage
{
    [TestClass]
    public class TestTestSuiteIntro
    {
        [TestMethod]
        public void LoadFromFile()
        {
            TestSuiteFamilies families = TestSuiteFamilies.Load(@"Resources\TestSuiteIntro_Sample.xml");
            Assert.AreEqual(4, families.Count, "Load 4 groups");
        }
        [TestMethod]
        public void InstallerLocation()
        {
            TestSuiteFamilies families = TestSuiteFamilies.Load(@"Resources\TestSuiteIntro_Sample.xml");
            TestSuiteFamily fileServer = families.FirstOrDefault(i => i.Name == "File Server");
            Assert.IsNotNull(fileServer);
        }
    }
}

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocol.TestSuites.Azod.Adapter;
using Microsoft.Protocol.TestSuites.Azod.Adapter.Util;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace Microsoft.Protocol.TestSuites.Azod.TestSuite
{
    [TestClass]
    public class AzodGPTestSuite : AzodTestClassBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            AzodTestClassBase.Initialize(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            AzodTestClassBase.Cleanup();
        }

        protected override void TestCleanup()
        {
            System.Diagnostics.Process[] pro = System.Diagnostics.Process.GetProcessesByName("dllhost");
            foreach (System.Diagnostics.Process p in pro)
            {
                p.Kill();
            }
            base.TestCleanup();
        }        

        [TestCategory("Non-BVT")]      
        [Priority(0)]
        [Description("Observer")]
        [TestMethod]
        public void GroupPolicySync()
        {
            string testName = string.Empty;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize test case.");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            RunObserverTestCase(testName);   
        }

        [TestCategory("Non-BVT")]        
        [Priority(0)]
        [Description("Observer")]
        [TestMethod]
        public void ResourcePropertySync()
        {
            string testName = string.Empty;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize test case.");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            RunObserverTestCase(testName);   
        }       
    }
}

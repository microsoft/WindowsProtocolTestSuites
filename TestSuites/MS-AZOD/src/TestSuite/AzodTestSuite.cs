// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocol.TestSuites.AZOD.Adapter;
using Microsoft.Protocol.TestSuites.AZOD.Adapter.Util;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace Microsoft.Protocol.TestSuites.AZOD.TestSuite
{
    [TestClass]
    public class AZODTestSuite : AZODTestClassBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            AZODTestClassBase.Initialize(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            AZODTestClassBase.Cleanup();            
        }

        protected override void TestCleanup()
        {
            System.Diagnostics.Process[] pro = System.Diagnostics.Process.GetProcessesByName("dllhost");
            foreach(System.Diagnostics.Process p in pro)
            {
                p.Kill();
            }
            base.TestCleanup();
        }
       
       
        [TestCategory("BVT")]
        [TestCategory("Windows")]
        [Priority(0)]
        [Description("Observer")]
        [TestMethod]
        public void Smb2()
        {
            #region LocalVariables
            string testName = string.Empty;
            #endregion


            #region Initialize
            this.TestSite.Log.Add(LogEntryKind.Debug, "Initialize test case.");
            testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            this.TestSite.Log.Add(LogEntryKind.Debug, "Get test case name: {0}", testName);
            System.Environment.SetEnvironmentVariable("MS_AZOD_TESTCASENAME", testName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Load MA capture expected frames from ptfconfig file.");
            this.TestConfig.LoadCaseLevelMAConfig(testName);
            this.TestConfig.GetEndpointRoles();
            #endregion

            base.Logging();

            #region Start MA capture
            this.TestSite.Log.Add(LogEntryKind.Comment, "Start MA capture in drive machine.");
            MaCapture maCapture = new MaCapture();
            string filter = "";
            maCapture.startCapture(filter, true, false);
            #endregion

            #region Trigger
            this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger client to run powershell script remotely.");
            
            bool returnCode=this.sutController.TriggerSmb2Trace();
            this.TestSite.Assert.IsTrue(returnCode, "FileSharing scenario should complete successfully.");

            //sleep for a while to wait for MA capturing messages.
            //Please fix me
            Thread.Sleep(this.TestConfig.Smb2SleepTime);

            #endregion

            #region Stop Capture
            this.TestSite.Log.Add(LogEntryKind.Comment, "Stop MA capture in drive machine.");
            maCapture.stopCapture();
            #endregion

            #region Verify Capture
            this.TestSite.Log.Add(LogEntryKind.Comment, "Start to verify capture file.");
            maCapture.veriryMessageSequence();
            #endregion

        }
        
       
        
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class AZODTestAttribute : Attribute
    {
        public static ITestSite Site
        {
            get;
            set;
        }

        public virtual void Logging()
        {
        }
    }
}

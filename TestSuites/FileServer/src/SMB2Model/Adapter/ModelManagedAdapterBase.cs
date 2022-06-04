// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter
{
    /// <summary>
    /// This class is a base of the Model adapter.
    /// </summary>
    public class ModelManagedAdapterBase : ManagedAdapterBase
    {
        protected SMB2ModelTestConfig testConfig;
        protected List<string> testFiles = new List<string>();
        protected List<string> testDirectories = new List<string>();

        public static HashSet<string> AllTestFiles { get; } = new HashSet<string>();
        public static HashSet<string> AllTestDirectories { get; } = new HashSet<string>();

        #region Initialization

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);

            Site.DefaultProtocolDocShortName = "MS-SMB2";

            testConfig = new SMB2ModelTestConfig(Site);

            TestTools.StackSdk.Security.KerberosLib.KerberosContext.KDCComputerName = testConfig.DCServerName;
            TestTools.StackSdk.Security.KerberosLib.KerberosContext.KDCPort = testConfig.KDCPort;
        }

        public override void Reset()
        {
            foreach (var directory in testDirectories)
            {
                AllTestDirectories.Add(directory);
            }

            testDirectories.Clear();

            foreach (var fileName in testFiles)
            {
                AllTestFiles.Add(fileName);
            }

            testFiles.Clear();

            base.Reset();
        }
        #endregion

        protected string CurrentTestCaseName
        {
            get
            {
                string fullName = (string)Site.TestProperties["CurrentTestCaseName"];
                return fullName.Split('.').LastOrDefault();
            }
        }

        protected string GetTestFileName(string share)
        {
            if (string.IsNullOrEmpty(share))
            {
                return null;
            }

            string fileName = CurrentTestCaseName + "_" + Guid.NewGuid().ToString();
            testFiles.Add(string.Format(@"{0}\{1}", share, fileName));
            return fileName;
        }

        protected void AddTestFileName(string share, string fileName)
        {
            if (string.IsNullOrEmpty(share))
            {
                return;
            }

            testFiles.Add(string.Format(@"{0}\{1}", share, fileName));
        }

        protected string GetTestDirectoryName(string share)
        {
            if (string.IsNullOrEmpty(share))
            {
                return null;
            }

            string directoryName = CurrentTestCaseName + "_" + Guid.NewGuid().ToString();
            testDirectories.Add(string.Format(@"{0}\{1}", share, directoryName));
            return directoryName;
        }

        protected void AddTestDirectoryName(string share, string directoryName)
        {
            if (string.IsNullOrEmpty(share))
            {
                return;
            }

            testDirectories.Add(string.Format(@"{0}\{1}", share, directoryName));
        }

        public virtual void CheckTestOverQUIC()
        {
            List<string> testList = new List<string> {
                "ResilientHandleDurableTestCaseS2039", "ResilientHandleDurableTestCaseS3307", "ResilientHandleDurableTestCaseS2123",
                "ResilientHandleDurableTestCaseS1856", "ResilientHandleDurableTestCaseS3056", "ResilientHandleDurableTestCaseS1753",
                "ResilientHandleBasicTestCaseS2454", "DurableHandleV1PreparedWithBatchOplockReconnectTestCaseS464", "DurableHandleV1PreparedWithBatchOplockReconnectTestCaseS303",
                "ResilientHandleDurableTestCaseS3502", "ResilientHandleBasicTestCaseS536", "DurableHandleV1PreparedWithBatchOplockReconnectTestCaseS387",
                "ResilientHandleBasicTestCaseS2246", "ResilientHandleDurableTestCaseS2743", "ResilientHandleBasicTestCaseS1109", "DurableHandleV2PreparedWithLeaseV2ReconnectTestCaseS318",
                "ResilientOpenScavengerTimer_ReconnectBeforeTimeout", 
                 "AppInstanceIdTestCaseS26", "AppInstanceIdTestCaseS561", "AppInstanceIdTestCaseS566", "AppInstanceIdTestCaseS46",
                 "AppInstanceIdTestCaseS591", "AppInstanceIdTestCaseS54", "AppInstanceIdTestCaseS586"
            };
            if (testConfig.UnderlyingTransport == Smb2TransportType.Quic && testList.Contains(CurrentTestCaseName))
                Site.Assert.Inconclusive("Ignoring test {0} over QUIC", CurrentTestCaseName);

        }
    }
}

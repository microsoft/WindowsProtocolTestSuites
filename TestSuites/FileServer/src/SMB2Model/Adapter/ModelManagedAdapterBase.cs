// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

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

        private static readonly HashSet<string> incompatibleTestNamesOverQUIC = new HashSet<string>()
        {
            "AppInstanceIdTestCaseS26",
            "AppInstanceIdTestCaseS46",
            "AppInstanceIdTestCaseS54",
            "AppInstanceIdTestCaseS561",
            "AppInstanceIdTestCaseS566",
            "AppInstanceIdTestCaseS586",
            "AppInstanceIdTestCaseS591",
            "DurableHandleV1PreparedWithBatchOplockReconnectTestCaseS303",
            "DurableHandleV1PreparedWithBatchOplockReconnectTestCaseS387",
            "DurableHandleV1PreparedWithBatchOplockReconnectTestCaseS464",
            "DurableHandleV2PreparedWithLeaseV2ReconnectTestCaseS318",
            "ResilientHandleBasicTestCaseS1109",
            "ResilientHandleBasicTestCaseS2246",
            "ResilientHandleBasicTestCaseS2454",
            "ResilientHandleBasicTestCaseS536",
            "ResilientHandleDurableTestCaseS1753",
            "ResilientHandleDurableTestCaseS1856",
            "ResilientHandleDurableTestCaseS2039",
            "ResilientHandleDurableTestCaseS2123",
            "ResilientHandleDurableTestCaseS2743",
            "ResilientHandleDurableTestCaseS3056",
            "ResilientHandleDurableTestCaseS3307",
            "ResilientHandleDurableTestCaseS3502",
            "ResilientOpenScavengerTimer_ReconnectBeforeTimeout"
        };

        public virtual void CheckTestOverQUIC()
        {
            if (testConfig.UnderlyingTransport == Smb2TransportType.Quic && incompatibleTestNamesOverQUIC.Contains(CurrentTestCaseName))
            {
                Site.Assert.Inconclusive("Ignoring test {0} over QUIC", CurrentTestCaseName);
            }
        }

    }
}

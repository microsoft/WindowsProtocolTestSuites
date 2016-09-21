// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack.Enum;
using Microsoft.Protocols.TestTools.StackSdk.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.BranchCache.TestSuite.HostedCacheServer
{
    [TestClass]
    public class PccrrServerGetSegList : BranchCacheTestClassBase
    {
        #region Test Suite Initialization

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        protected override void TestInitialize()
        {
            base.TestInitialize();

            ResetHostedCacheServer();
        }

        protected override void TestCleanup()
        {
            ResetHostedCacheServer();

            base.TestCleanup();
        }

        #endregion

        #region HostedCacheServer_PccrrServer_GetSegList_SegmentIDCountNotMatch

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrServer_GetSegList_SegmentIDCountNotMatch()
        {
            CheckApplicability();

            var contentInformation = contentInformationUtility.CreateContentInformationV2();

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Send malformed GetSegList request to hosted cache server");

            PccrrClient pccrrClient = new PccrrClient(
                testConfig.HostedCacheServerComputerName,
                testConfig.HostedCacheServerHTTPListenPort);

            var pccrrGetSegListRequest = pccrrClient.CreateMsgGetSegListRequest(
                CryptoAlgoId_Values.AES_128,
                Guid.NewGuid(),
                new byte[][] { contentInformation.GetSegmentId(0, 0) });
            var buffer = pccrrGetSegListRequest.Encode();
            buffer[35] = 2;
            pccrrClient.SendBytes(
                buffer,
                testConfig.Timeout);

            bool passed = false;
            try
            {
                pccrrClient.ExpectPacket();
            }
            catch
            {
                passed = true;
            }

            BaseTestSite.Assert.IsTrue(passed, "Hosted cache server should drop GetSegList packets with invalid segment ID count");
        }

        #endregion

        #region HostedCacheServer_PccrrServer_GetSegList_SegmentIDEmpty

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrServer_GetSegList_SegmentIDEmpty()
        {
            CheckApplicability();

            var contentInformation = contentInformationUtility.CreateContentInformationV2();

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Send malformed GetSegList request to hosted cache server");

            PccrrClient pccrrClient = new PccrrClient(
                testConfig.HostedCacheServerComputerName,
                testConfig.HostedCacheServerHTTPListenPort);

            var pccrrGetSegListRequest = pccrrClient.CreateMsgGetSegListRequest(
                CryptoAlgoId_Values.AES_128,
                Guid.NewGuid(),
                new byte[][] { });
            pccrrClient.SendPacket(
                pccrrGetSegListRequest,
                testConfig.Timeout);

            bool passed = false;
            try
            {
                pccrrClient.ExpectPacket();
            }
            catch
            {
                passed = true;
            }

            BaseTestSite.Assert.IsTrue(passed, "Hosted cache server should drop GetSegList packets with no segment ID");
        }

        #endregion

        #region HostedCacheServer_PccrrServer_GetSegList_ExtensibleBlob

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrServer_GetSegList_ExtensibleBlob()
        {
            CheckApplicability();

            var contentInformation = contentInformationUtility.CreateContentInformationV2();

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Send GetSegList request with extensible blob to hosted cache server");

            PccrrClient pccrrClient = new PccrrClient(
                testConfig.HostedCacheServerComputerName,
                testConfig.HostedCacheServerHTTPListenPort);

            ExtensibleBlobVersion1 extensibleBlob = new ExtensibleBlobVersion1
            {
                ///[MS-PCCRR]Section 2.2.6.1: ExtensibleBlobVersion: Network-byte-order unsigned short integer that contains the version of the extensible blob. 
                ///It must be equal to 1.
                ExtensibleBlobVersion = 1,
                SegmentAgeUnits = 3,
                SegmentAgeCount = 1,
                SegmentAges = new EncodedSegmentAge[]
                {
                    new EncodedSegmentAge
                    {
                        SegmentIndex = 0,
                        SegmentAgeLowPart = 1
                    }
                }
            };

            var pccrrGetSegListRequest = pccrrClient.CreateMsgGetSegListRequest(
                CryptoAlgoId_Values.AES_128,
                Guid.NewGuid(),
                new byte[][] { contentInformation.GetSegmentId(0, 0) },
                extensibleBlob.ToBytes()); 
            pccrrClient.SendPacket(
                pccrrGetSegListRequest,
                testConfig.Timeout);
            pccrrClient.ExpectPacket();
        }

        #endregion
    }
}

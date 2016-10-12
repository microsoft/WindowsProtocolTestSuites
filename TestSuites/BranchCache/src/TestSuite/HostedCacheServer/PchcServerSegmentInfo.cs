// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack.Enum;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr;

namespace Microsoft.Protocols.TestSuites.BranchCache.TestSuite.HostedCacheServer
{
    [TestClass]
    public class PchcServerSegmentInfo : BranchCacheTestClassBase
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

        #region HostedCacheServer_PchcServer_SegmentInfo_VersionIncompatible

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PchcServer_SegmentInfo_VersionIncompatible()
        {
            CheckApplicability();

            var contentInformation = contentInformationUtility.CreateContentInformationV1();

            using (var pccrrServer = new PccrrServer(testConfig.ClientContentRetrievalListenPort))
            {
                pccrrServer.StartListening();

                pccrrServer.MessageArrived += new MessageArrivedEventArgs(delegate(IPEndPoint sender, PccrrPacket pccrrPacket)
                    {
                        BaseTestSite.Assert.Fail("Hosted cache server should not retrieve content information with incompatible version");
                    });

                PCHCClient pchcClient = new PCHCClient(
                    TransferProtocol.HTTPS,
                    testConfig.HostedCacheServerComputerName,
                    testConfig.HostedCacheServerHTTPSListenPort,
                    PchcConsts.HttpsUrl,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword);

                var segmentInfo = pchcClient.CreateSegmentInfoMessage(
                    testConfig.ClientContentRetrievalListenPort,
                    contentInformation,
                    0);

                segmentInfo.SegmentInfo.Version = 0xFEFE;

                var segmentInfoResponse = pchcClient.SendSegmentInfoMessage(segmentInfo);

                BaseTestSite.Assert.AreEqual(
                    RESPONSE_CODE.OK,
                    segmentInfoResponse.ResponseCode,
                    "Hosted cache server should return OK to segment info message");

                Thread.Sleep(testConfig.NegativeTestTimeout);
            }
        }

        #endregion

        #region HostedCacheServer_PchcServer_SegmentInfo_HashAlgoInvalid

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PchcServer_SegmentInfo_HashAlgoInvalid()
        {
            CheckApplicability();

            var contentInformation = contentInformationUtility.CreateContentInformationV1();

            using (var pccrrServer = new PccrrServer(testConfig.ClientContentRetrievalListenPort))
            {
                pccrrServer.StartListening();

                pccrrServer.MessageArrived += new MessageArrivedEventArgs(delegate(IPEndPoint sender, PccrrPacket pccrrPacket)
                {
                    BaseTestSite.Assert.Fail("Hosted cache server should not retrieve content information with invalid hash algorithm");
                });

                PCHCClient pchcClient = new PCHCClient(
                    TransferProtocol.HTTPS,
                    testConfig.HostedCacheServerComputerName,
                    testConfig.HostedCacheServerHTTPSListenPort,
                    PchcConsts.HttpsUrl,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword);

                var segmentInfo = pchcClient.CreateSegmentInfoMessage(
                    testConfig.ClientContentRetrievalListenPort,
                    contentInformation,
                    0);

                segmentInfo.SegmentInfo.dwHashAlgo = (dwHashAlgo_Values)0xFEFE;

                var segmentInfoResponse = pchcClient.SendSegmentInfoMessage(segmentInfo);

                BaseTestSite.Assert.AreEqual(
                    RESPONSE_CODE.OK,
                    segmentInfoResponse.ResponseCode,
                    "Hosted cache server should return OK to segment info message");

                Thread.Sleep(testConfig.NegativeTestTimeout);
            }
        }

        #endregion

        #region HostedCacheServer_PchcServer_SegmentInfo_SegmentsEmpty

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PchcServer_SegmentInfo_SegmentsEmpty()
        {
            CheckApplicability();

            var contentInformation = contentInformationUtility.CreateContentInformationV1();

            using (var pccrrServer = new PccrrServer(testConfig.ClientContentRetrievalListenPort))
            {
                pccrrServer.StartListening();

                pccrrServer.MessageArrived += new MessageArrivedEventArgs(delegate(IPEndPoint sender, PccrrPacket pccrrPacket)
                {
                    BaseTestSite.Assert.Fail("Hosted cache server should not retrieve content information with zero segment");
                });

                PCHCClient pchcClient = new PCHCClient(
                    TransferProtocol.HTTPS,
                    testConfig.HostedCacheServerComputerName,
                    testConfig.HostedCacheServerHTTPSListenPort,
                    PchcConsts.HttpsUrl,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword);

                var segmentInfo = pchcClient.CreateSegmentInfoMessage(
                    testConfig.ClientContentRetrievalListenPort,
                    contentInformation,
                    0);

                segmentInfo.SegmentInfo.segments = new SegmentDescription[0];

                var segmentInfoResponse = pchcClient.SendSegmentInfoMessage(segmentInfo);

                BaseTestSite.Assert.AreEqual(
                    RESPONSE_CODE.OK,
                    segmentInfoResponse.ResponseCode,
                    "Hosted cache server should return OK to segment info message");

                Thread.Sleep(testConfig.NegativeTestTimeout);
            }
        }

        #endregion

        #region HostedCacheServer_PchcServer_SegmentInfo_SegmentsMultiple

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PchcServer_SegmentInfo_SegmentsMultiple()
        {
            CheckApplicability();

            var contentInformation = contentInformationUtility.CreateContentInformationV1();

            using (var pccrrServer = new PccrrServer(testConfig.ClientContentRetrievalListenPort))
            {
                pccrrServer.StartListening();

                pccrrServer.MessageArrived += new MessageArrivedEventArgs(delegate(IPEndPoint sender, PccrrPacket pccrrPacket)
                {
                    BaseTestSite.Assert.Fail("Hosted cache server should not retrieve content information with multiple segments");
                });

                PCHCClient pchcClient = new PCHCClient(
                    TransferProtocol.HTTPS,
                    testConfig.HostedCacheServerComputerName,
                    testConfig.HostedCacheServerHTTPSListenPort,
                    PchcConsts.HttpsUrl,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword);

                var segmentInfo = pchcClient.CreateSegmentInfoMessage(
                    testConfig.ClientContentRetrievalListenPort,
                    contentInformation,
                    0);

                segmentInfo.SegmentInfo.cSegments = 2;
                segmentInfo.SegmentInfo.segments = segmentInfo.SegmentInfo.segments.Concat(segmentInfo.SegmentInfo.segments).ToArray();

                var segmentInfoResponse = pchcClient.SendSegmentInfoMessage(segmentInfo);

                BaseTestSite.Assert.AreEqual(
                    RESPONSE_CODE.OK,
                    segmentInfoResponse.ResponseCode,
                    "Hosted cache server should return OK to segment info message");

                Thread.Sleep(testConfig.NegativeTestTimeout);
            }
        }

        #endregion
    }
}

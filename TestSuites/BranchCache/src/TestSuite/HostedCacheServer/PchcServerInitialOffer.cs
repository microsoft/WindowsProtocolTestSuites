// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.Messages;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrd;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack.Enum;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestSuites.BranchCache.HostedCacheServer;
using Microsoft.Protocols.TestSuites.BranchCache.TestSuite;

namespace Microsoft.Protocols.TestSuites.BranchCache.TestSuite.HostedCacheServer
{
    [TestClass]
    public class PchcServerInitialOffer : BranchCacheTestClassBase
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

        #region HostedCacheServer_PchcServer_InitialOffer_ContentRetrieved

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PchcServer_InitialOffer_ContentRereieved()
        {
            CheckApplicability();

            EventQueue eventQueue = new EventQueue(BaseTestSite);
            eventQueue.Timeout = testConfig.Timeout;

            Content_Information_Data_Structure contentInformation = contentInformationUtility.CreateContentInformationV1();

            CryptoAlgoId_Values cryptoAlgoId = CryptoAlgoId_Values.AES_128;

            using (PccrrTestServerV1 pccrrTestServer = new PccrrTestServerV1())
            {
                BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Start PCCRR server to be ready to serve content to hosted cache server");

                pccrrTestServer.Start(
                    testConfig.ClientContentRetrievalListenPort,
                    cryptoAlgoId,
                    new ProtoVersion { MajorVersion = 1, MinorVersion = 0 },
                    contentInformation,
                    TestUtility.GenerateRandomArray(ContentInformationUtility.DefaultBlockSize),
                    eventQueue);

                PCHCClient pchcClient = new PCHCClient(
                    TransferProtocol.HTTPS,
                    testConfig.HostedCacheServerComputerName,
                    testConfig.HostedCacheServerHTTPSListenPort,
                    PchcConsts.HttpsUrl,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword);

                BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Send initial offer message to hosted cache server");

                SEGMENT_INFO_MESSAGE segmentInfoMessage = pchcClient.CreateSegmentInfoMessage(
                    testConfig.ClientContentRetrievalListenPort,
                    contentInformation,
                    0);
                pchcClient.SendSegmentInfoMessage(segmentInfoMessage);

                BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Make sure all blocks in segment 0 are retrieved by hosted cache server");

                int blockCount = 0;
                TestUtility.DoUntilSucceed(delegate()
                {
                    eventQueue.Expect<MessageArrivedEventArgs>(typeof(PccrrServer).GetEvent("MessageArrived"), delegate(System.Net.IPEndPoint sender, PccrrPacket pccrrPacket)
                    {
                        var pccrrGetBlksRequest = pccrrPacket as PccrrGETBLKSRequestPacket;

                        if (pccrrGetBlksRequest != null)
                        {
                            blockCount++;
                        }
                    });
                    return blockCount == contentInformation.segments[0].BlockCount;
                }, TimeSpan.MaxValue, TimeSpan.Zero);

                BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Wait until cache is available on hosted cache server");

                TestUtility.DoUntilSucceed(() => sutControlAdapter.IsLocalCacheExisted(testConfig.HostedCacheServerComputerFQDNOrNetBiosName), testConfig.Timeout, testConfig.RetryInterval);

                INITIAL_OFFER_MESSAGE initialOfferMessage = pchcClient.CreateInitialOfferMessage(
                        testConfig.ClientContentRetrievalListenPort,
                        contentInformation.GetSegmentId(0));
                Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc.RESPONSE_MESSAGE responseMessage2
                    = pchcClient.SendInitialOfferMessage(initialOfferMessage);

                TestClassBase.BaseTestSite.Assert.AreEqual<RESPONSE_CODE>(
                    RESPONSE_CODE.INTERESTED,
                    responseMessage2.ResponseCode,
                    @"The hosted cache MUST specify a response code of 0 
                        if it already has block data and block hash.");
            }
        }

        #endregion

        #region HostedCacheServer_PchcServer_InitialOffer_SegmentInfoRetrieved

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PchcServer_InitialOffer_SegmentInfoRetrieved()
        {
            CheckApplicability();

            Content_Information_Data_Structure contentInformation = contentInformationUtility.CreateContentInformationV1();

            PCHCClient pchcClient = new PCHCClient(
                    TransferProtocol.HTTPS,
                    testConfig.HostedCacheServerComputerName,
                    testConfig.HostedCacheServerHTTPSListenPort,
                    PchcConsts.HttpsUrl,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Send initial offer message to hosted cache server");

            INITIAL_OFFER_MESSAGE initialOfferMessage = pchcClient.CreateInitialOfferMessage(
                        testConfig.ClientContentRetrievalListenPort,
                        contentInformation.GetSegmentId(0));
            pchcClient.SendInitialOfferMessage(initialOfferMessage);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Supply segment info to hosted cache server");

            SEGMENT_INFO_MESSAGE segmentInfoMessage = pchcClient.CreateSegmentInfoMessage(
                testConfig.ClientContentRetrievalListenPort,
                contentInformation,
                0);
            pchcClient.SendSegmentInfoMessage(segmentInfoMessage);

            Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc.RESPONSE_MESSAGE responseMessage2 = pchcClient.SendInitialOfferMessage(initialOfferMessage);

            TestClassBase.BaseTestSite.Assert.AreEqual<RESPONSE_CODE>(
                RESPONSE_CODE.INTERESTED,
                responseMessage2.ResponseCode,
                @"The hosted cache MUST specify a response code of 0 
                        if it already has block hash.");
        }

        #endregion
    }
}


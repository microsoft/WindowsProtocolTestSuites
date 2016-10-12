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
using Microsoft.Protocols.TestSuites.BranchCache.HostedCacheServer;
using Microsoft.Protocols.TestTools.Messages;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack.Enum;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr;

namespace Microsoft.Protocols.TestSuites.BranchCache.TestSuite.HostedCacheServer
{
    [TestClass]
    public class PchcServerBatchedOffer : BranchCacheTestClassBase
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

        #region HostedCacheServer_PchcServer_BatchedOffer_SegmentDescriptorEmpty

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PchcServer_BatchedOffer_SegmentDescriptorEmpty()
        {
            CheckApplicability();

            var contentInformation = contentInformationUtility.CreateContentInformationV2();

            using (var pccrrServer = new PccrrServer(testConfig.ClientContentRetrievalListenPort))
            {
                pccrrServer.StartListening();

                pccrrServer.MessageArrived += new MessageArrivedEventArgs(delegate(IPEndPoint sender, PccrrPacket pccrrPacket)
                {
                    BaseTestSite.Assert.Fail("Hosted cache server should not retrieve content information with no segment description");
                });

                PCHCClient pchcClient = new PCHCClient(
                    TransferProtocol.HTTP,
                    testConfig.HostedCacheServerComputerName,
                    testConfig.HostedCacheServerHTTPListenPort,
                    PchcConsts.HttpUrl,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword);

                var batchedOffer = pchcClient.CreateBatchedOfferMessage(
                    testConfig.ClientContentRetrievalListenPort,
                    contentInformation);

                batchedOffer.SegmentDescriptors = new SegmentDescriptor[0];

                bool passed = false;
                try
                {
                    pchcClient.SendBatchedOfferMessage(batchedOffer);
                }
                catch
                {
                    passed = true;
                }

                BaseTestSite.Assert.IsTrue(passed, "Hosted cache server should drop invalid batched offer request");
            }
        }

        #endregion

        #region HostedCacheServer_PchcServer_BatchedOffer_SegmentDescriptorTooMany

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PchcServer_BatchedOffer_SegmentDescriptorTooMany()
        {
            CheckApplicability();

            var contentInformation = contentInformationUtility.CreateContentInformationV2();

            using (var pccrrServer = new PccrrServer(testConfig.ClientContentRetrievalListenPort))
            {
                pccrrServer.StartListening();

                pccrrServer.MessageArrived += new MessageArrivedEventArgs(delegate(IPEndPoint sender, PccrrPacket pccrrPacket)
                {
                    BaseTestSite.Assert.Fail("Hosted cache server should not retrieve content information with more than 128 segment descriptions");
                });

                PCHCClient pchcClient = new PCHCClient(
                    TransferProtocol.HTTP,
                    testConfig.HostedCacheServerComputerName,
                    testConfig.HostedCacheServerHTTPListenPort,
                    PchcConsts.HttpUrl,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword);

                var batchedOffer = pchcClient.CreateBatchedOfferMessage(
                    testConfig.ClientContentRetrievalListenPort,
                    contentInformation);

                List<SegmentDescriptor> segmentDescriptors = new List<SegmentDescriptor>();
                for (int i = 0; i < 129; i++)
                {
                    segmentDescriptors.Add(batchedOffer.SegmentDescriptors[0]);
                }
                batchedOffer.SegmentDescriptors = segmentDescriptors.ToArray();

                bool passed = false;
                try
                {
                    pchcClient.SendBatchedOfferMessage(batchedOffer);
                }
                catch
                {
                    passed = true;
                }

                BaseTestSite.Assert.IsTrue(passed, "Hosted cache server should drop invalid batched offer request");
            }
        }

        #endregion

        #region HostedCacheServer_PchcServer_BatchedOffer_ContentTagEmpty

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PchcServer_BatchedOffer_ContentTagEmpty()
        {
            CheckApplicability();

            var contentInformation = contentInformationUtility.CreateContentInformationV2();

            PCHCClient pchcClient = new PCHCClient(
                TransferProtocol.HTTP,
                testConfig.HostedCacheServerComputerName,
                testConfig.HostedCacheServerHTTPListenPort,
                PchcConsts.HttpUrl,
                testConfig.DomainName,
                testConfig.UserName,
                testConfig.UserPassword);

            var batchedOffer = pchcClient.CreateBatchedOfferMessage(
                testConfig.ClientContentRetrievalListenPort,
                contentInformation);

            batchedOffer.SegmentDescriptors[0].SizeOfContentTag = 0;
            batchedOffer.SegmentDescriptors[0].ContentTag = new byte[0];

            bool passed = false;
            try
            {
                pchcClient.SendBatchedOfferMessage(batchedOffer);
            }
            catch
            {
                passed = true;
            }

            BaseTestSite.Assert.IsTrue(passed, "Hosted cache server should drop invalid batched offer request");
        }

        #endregion

        #region HostedCacheServer_PchcServer_BatchedOffer_HashAlgoInvalid

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PchcServer_BatchedOffer_HashAlgoInvalid()
        {
            CheckApplicability();

            var contentInformation = contentInformationUtility.CreateContentInformationV2();

            using (var pccrrServer = new PccrrServer(testConfig.ClientContentRetrievalListenPort))
            {
                pccrrServer.StartListening();

                pccrrServer.MessageArrived += new MessageArrivedEventArgs(delegate(IPEndPoint sender, PccrrPacket pccrrPacket)
                {
                    BaseTestSite.Assert.Fail("Hosted cache server should not retrieve content information with more than 128 segment descriptions");
                });

                PCHCClient pchcClient = new PCHCClient(
                    TransferProtocol.HTTP,
                    testConfig.HostedCacheServerComputerName,
                    testConfig.HostedCacheServerHTTPListenPort,
                    PchcConsts.HttpUrl,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword);

                var batchedOffer = pchcClient.CreateBatchedOfferMessage(
                    testConfig.ClientContentRetrievalListenPort,
                    contentInformation);

                batchedOffer.SegmentDescriptors[0].HashAlgorithm = 0xFE;

                var batchedOfferResponse = pchcClient.SendBatchedOfferMessage(batchedOffer);

                BaseTestSite.Assert.AreEqual(
                    RESPONSE_CODE.OK,
                    batchedOfferResponse.ResponseCode,
                    "Hosted cache server should return OK to batched offer message");

                Thread.Sleep(testConfig.NegativeTestTimeout);
            }
        }

        #endregion

        #region HostedCacheServer_PchcServer_BatchedOffer_ContentRetrieved

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PchcServer_BatchedOffer_ContentRetrieved()
        {
            CheckApplicability();

            EventQueue eventQueue = new EventQueue(BaseTestSite);
            eventQueue.Timeout = testConfig.Timeout;

            byte[] content = TestUtility.GenerateRandomArray(ContentInformationUtility.DefaultBlockSize);
            Content_Information_Data_Structure_V2 contentInformationV2 = contentInformationUtility.CreateContentInformationV2();

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Start PCCRR server to be ready to serve content to hosted cache server");

            using (PccrrTestServerV2 pccrrTestServerV2 = new PccrrTestServerV2())
            {
                pccrrTestServerV2.Start(
                    testConfig.ClientContentRetrievalListenPort,
                    CryptoAlgoId_Values.AES_128,
                    contentInformationV2,
                    content,
                    eventQueue);

                PCHCClient pchcClient = new PCHCClient(
                    TransferProtocol.HTTP,
                    testConfig.HostedCacheServerComputerName,
                    testConfig.HostedCacheServerHTTPListenPort,
                    PchcConsts.HttpUrl,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword);

                BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Directly Supply segment info to hosted cache server");

                var batchedOfferMessage = pchcClient.CreateBatchedOfferMessage(
                    testConfig.ClientContentRetrievalListenPort,
                    contentInformationV2);
                pchcClient.SendBatchedOfferMessage(batchedOfferMessage);

                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Make sure all the segments in chunk 0 is retrieved by hosted cache server");

                int blockCount = 0;
                int totalBlockCount = contentInformationV2.GetBlockCount();
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
                    return blockCount == totalBlockCount;
                }, TimeSpan.MaxValue, TimeSpan.Zero);

                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Wait until cache is available on hosted cache server");

                TestUtility.DoUntilSucceed(() => sutControlAdapter.IsLocalCacheExisted(testConfig.HostedCacheServerComputerFQDNOrNetBiosName), testConfig.Timeout, testConfig.RetryInterval);

                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Re-supply segment info to hosted cache server");

                var batchedOfferResponse = pchcClient.SendBatchedOfferMessage(batchedOfferMessage);

                BaseTestSite.Assert.AreEqual(
                    RESPONSE_CODE.OK,
                    batchedOfferResponse.ResponseCode,
                    "Hosted cache server should return OK to batched offer message");
            }
        }

        #endregion
    }
}


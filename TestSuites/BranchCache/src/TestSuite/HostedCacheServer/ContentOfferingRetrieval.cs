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
using Microsoft.Protocols.TestSuites.BranchCache.TestSuite;
using Microsoft.Protocols.TestSuites.BranchCache.HostedCacheServer;

namespace Microsoft.Protocols.TestSuites.BranchCache.TestSuite.HostedCacheServer
{
    [TestClass]
    public class ContentOfferingRetrieval : BranchCacheTestClassBase
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

            ResetContentServer();
            ResetHostedCacheServer();
        }

        protected override void TestCleanup()
        {
            ResetContentServer();
            ResetHostedCacheServer();

            base.TestCleanup();
        }

        #endregion

        #region HostedCacheServer_BVT_CacheOfferingRetrieval_V1

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_BVT_CacheOfferingRetrieval_V1()
        {
            CheckApplicability();

            EventQueue eventQueue = new EventQueue(BaseTestSite);
            eventQueue.Timeout = testConfig.Timeout;

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Trigger hash generation on content server");

            byte[] content = contentInformationUtility.RetrieveContentData();

            Content_Information_Data_Structure contentInformation = 
            PccrcUtility.ParseContentInformation(contentInformationUtility.RetrieveContentInformation(BranchCacheVersion.V1));

            CryptoAlgoId_Values cryptoAlgoId = CryptoAlgoId_Values.AES_128;
            
            PccrrClient pccrrClient = new PccrrClient(testConfig.HostedCacheServerComputerName, testConfig.HostedCacheServerHTTPListenPort);

            for (int i = 0; i < contentInformation.cSegments; ++i)
            {
                var pccrrBlkListRequest = pccrrClient.CreateMsgGetBlkListRequest(
                    contentInformation.GetSegmentId(i),
                    new BLOCK_RANGE[] { new BLOCK_RANGE { Index = 0, Count = contentInformation.segments[i].BlockCount } },
                    cryptoAlgoId,
                    MsgType_Values.MSG_GETBLKLIST);
                pccrrClient.SendPacket(
                    pccrrBlkListRequest,
                    testConfig.Timeout);
                var pccrrBlkListResponse
                    = (PccrrBLKLISTResponsePacket)pccrrClient.ExpectPacket();

                BaseTestSite.Assert.AreEqual<uint>(
                    0,
                    pccrrBlkListResponse.MsgBLKLIST.BlockRangeCount,
                    "The server MUST set the BlockRangeCount field to zero if it doesn't have the requested blocks data.");
            }

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Retrieve content information from content server");

            using (PccrrTestServerV1 pccrrTestServer = new PccrrTestServerV1())
            {
                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Start PCCRR server to be ready to serve content to hosted cache server");

                pccrrTestServer.Start(
                    testConfig.ClientContentRetrievalListenPort,
                    cryptoAlgoId,
                    contentInformation,
                    content,
                    eventQueue);

                PCHCClient pchcClient = new PCHCClient(
                    TransferProtocol.HTTPS,
                    testConfig.HostedCacheServerComputerName,
                    testConfig.HostedCacheServerHTTPSListenPort,
                    PchcConsts.HttpsUrl,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword);

                for (int i = 0; i < contentInformation.cSegments; i++)
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Offer content segment {0} to hosted cache server",
                        i);

                    INITIAL_OFFER_MESSAGE initialOfferMessage = pchcClient.CreateInitialOfferMessage(
                        testConfig.ClientContentRetrievalListenPort,
                        contentInformation.GetSegmentId(i));
                    Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc.RESPONSE_MESSAGE responseMessage
                        = pchcClient.SendInitialOfferMessage(initialOfferMessage);

                    TestClassBase.BaseTestSite.Assert.AreEqual<RESPONSE_CODE>(
                        RESPONSE_CODE.INTERESTED,
                        responseMessage.ResponseCode,
                        @"The hosted cache MUST specify a response code of 1 
                        if its list of block hashes associated with the segment is incomplete.");

                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Supply segment info to hosted cache server");

                    SEGMENT_INFO_MESSAGE segmentInfoMessage = pchcClient.CreateSegmentInfoMessage(
                        testConfig.ClientContentRetrievalListenPort,
                        contentInformation,
                        i);
                    responseMessage = pchcClient.SendSegmentInfoMessage(segmentInfoMessage);

                    TestClassBase.BaseTestSite.Assert.AreEqual<RESPONSE_CODE>(
                        RESPONSE_CODE.OK,
                        responseMessage.ResponseCode,
                        @"The hosted cache MUST send a response code of 0 when SEGMENT_INFO_MESSAGE request received");

                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Make sure all blocks in segment {0} are retrieved by hosted cache server",
                        i);

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
                        return blockCount == contentInformation.segments[i].BlockCount;
                    }, TimeSpan.MaxValue, TimeSpan.Zero);
                }

                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Wait until cache is available on hosted cache server");

                TestUtility.DoUntilSucceed(() => sutControlAdapter.IsLocalCacheExisted(testConfig.HostedCacheServerComputerFQDNOrNetBiosName), testConfig.Timeout, testConfig.RetryInterval);
            }

            List<byte> retrievedContent = new List<byte>();

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Negotiate PCCRR version");

            var pccrrNegotiateRequest = pccrrClient.CreateMsgNegoRequest(
                new ProtoVersion { MajorVersion = 1, MinorVersion = 0 },
                new ProtoVersion { MajorVersion = 1, MinorVersion = ushort.MaxValue },
                cryptoAlgoId,
                MsgType_Values.MSG_NEGO_REQ);
            pccrrClient.SendPacket(
                    pccrrNegotiateRequest,
                    testConfig.Timeout);
            var pccrrNegotiateResponse
                = (PccrrNegoResponsePacket)pccrrClient.ExpectPacket();

            if (testConfig.SupportBranchCacheV1)
            {
                BaseTestSite.Assert.IsTrue(
                    pccrrNegotiateResponse.MsgNegoResp.MinSupporteProtocolVersion.MajorVersion <= 1 &&
                    pccrrNegotiateResponse.MsgNegoResp.MaxSupporteProtocolVersion.MajorVersion >= 1,
                    "SupportedProtocolVersion doesn't match configuration");
            }

            if (testConfig.SupportBranchCacheV2)
            {
                BaseTestSite.Assert.IsTrue(
                    pccrrNegotiateResponse.MsgNegoResp.MinSupporteProtocolVersion.MajorVersion <= 2 &&
                    pccrrNegotiateResponse.MsgNegoResp.MaxSupporteProtocolVersion.MajorVersion >= 2,
                    "SupportedProtocolVersion doesn't match configuration");
            }

            Aes aes = PccrrUtitlity.CreateAes(cryptoAlgoId);

            for (int i = 0; i < contentInformation.cSegments; i++)
            {
                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Retrieve block list for segment {0}",
                    i);

                var pccrrBlkListRequest = pccrrClient.CreateMsgGetBlkListRequest(
                    contentInformation.GetSegmentId(i),
                    new BLOCK_RANGE[] { new BLOCK_RANGE { Index = 0, Count = contentInformation.segments[i].BlockCount } },
                    cryptoAlgoId,
                    MsgType_Values.MSG_GETBLKLIST);
                pccrrClient.SendPacket(
                    pccrrBlkListRequest,
                    testConfig.Timeout);
                var pccrrBlkListResponse
                    = (PccrrBLKLISTResponsePacket)pccrrClient.ExpectPacket();

                BaseTestSite.Assert.AreNotEqual<uint>(
                    0,
                    pccrrBlkListResponse.MsgBLKLIST.BlockRangeCount,
                    "The server MUST set the BlockRangeCount field to a value greater than zero if it has the requested blocks data.");

                for (int j = 0; j < contentInformation.segments[i].BlockCount; j++)
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Retrieve block {0} for segment {1}",
                        j,
                        i);

                    PccrrGETBLKSRequestPacket pccrrBlkRequest = pccrrClient.CreateMsgGetBlksRequest(
                        contentInformation.GetSegmentId(i),
                        cryptoAlgoId,
                        MsgType_Values.MSG_GETBLKS,
                        (uint)j,
                        1);
                    pccrrClient.SendPacket(
                        pccrrBlkRequest,
                        testConfig.Timeout);
                    PccrrBLKResponsePacket pccrrBlkResponse
                        = (PccrrBLKResponsePacket)pccrrClient.ExpectPacket();

                    BaseTestSite.Assert.AreNotEqual<uint>(
                        0,
                        pccrrBlkResponse.MsgBLK.SizeOfBlock,
                        "The server MUST set the SizeOfBlock field to a value greater than zero if it has the requested blocks data.");

                    byte[] block = pccrrBlkResponse.MsgBLK.Block;

                    if (cryptoAlgoId != CryptoAlgoId_Values.NoEncryption)
                        block = PccrrUtitlity.Decrypt(aes, block, contentInformation.segments[i].SegmentSecret, pccrrBlkResponse.MsgBLK.IVBlock);

                    retrievedContent.AddRange(block);
                }
            }

            BaseTestSite.Assert.IsTrue(
                Enumerable.SequenceEqual(content, retrievedContent),
                "The retrieved cached data should be the same as server data.");
        }

        #endregion

        #region HostedCacheServer_BVT_CacheOfferingRetrieval_V2

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_BVT_CacheOfferingRetrieval_V2()
        {
            CheckApplicability();

            EventQueue eventQueue = new EventQueue(BaseTestSite);
            eventQueue.Timeout = testConfig.Timeout;

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Retrieve the original content data from content server");
            byte[] content = contentInformationUtility.RetrieveContentData();

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Retrieve the content information from content server");
            Content_Information_Data_Structure_V2 contentInformation = 
            PccrcUtility.ParseContentInformationV2(contentInformationUtility.RetrieveContentInformation(BranchCacheVersion.V2));

            CryptoAlgoId_Values cryptoAlgoId = CryptoAlgoId_Values.AES_128;

            PccrrClient pccrrClient = new PccrrClient(testConfig.HostedCacheServerComputerName, testConfig.HostedCacheServerHTTPListenPort);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Send PCCRR GetSegList request to HostedCacheServer to make sure that this file does not exist in HostedCacheServer.");
            for (int i = 0; i < contentInformation.chunks.Length; ++i)
            {
                var chunk = contentInformation.chunks[i];
                for (int j = 0; j < chunk.chunkData.Length; j++)
                {
                    var pccrrGetSegListRequest = pccrrClient.CreateMsgGetSegListRequest(
                        cryptoAlgoId,
                        Guid.NewGuid(),
                        new byte[][] { contentInformation.GetSegmentId(i, j) });
                    pccrrClient.SendPacket(
                        pccrrGetSegListRequest,
                        testConfig.Timeout);
                    var pccrrGetSegListResponse
                        = (PccrrSegListResponsePacket)pccrrClient.ExpectPacket();

                    BaseTestSite.Assert.AreEqual<uint>(
                        0,
                        pccrrGetSegListResponse.MsgSegList.SegmentRangeCount,
                        "The server MUST set the SegmentRangeCount field to zero if it doesn't have the requested segments data.");
                }
            }

            using (PccrrTestServerV2 pccrrTestServer = new PccrrTestServerV2())
            {
                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Start PCCRR server to be ready to serve content to hosted cache server");

                pccrrTestServer.Start(
                    testConfig.ClientContentRetrievalListenPort,
                    cryptoAlgoId,
                    contentInformation,
                    content,
                    eventQueue);

                PCHCClient pchcClient = new PCHCClient(
                    TransferProtocol.HTTP,
                    testConfig.HostedCacheServerComputerName,
                    testConfig.ContentServerHTTPListenPort,
                    PchcConsts.HttpUrl,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword);

                BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Offer all content segments to hosted cache server");

                var batchedOfferMessage = pchcClient.CreateBatchedOfferMessage(
                    testConfig.ClientContentRetrievalListenPort,
                    contentInformation);
                var responseMessage = pchcClient.SendBatchedOfferMessage(batchedOfferMessage);

                TestClassBase.BaseTestSite.Assert.AreEqual<RESPONSE_CODE>(
                    RESPONSE_CODE.OK,
                    responseMessage.ResponseCode,
                    @"The hosted cache MUST send a response code of 0 when BATCHED_OFFER_MESSAGE request received");

                BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Make sure all segments are retrieved by hosted cache server");

                int totalBlockCount = contentInformation.GetBlockCount();
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
                    return blockCount == totalBlockCount;
                }, TimeSpan.MaxValue, TimeSpan.Zero);

                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Wait until cache is available on hosted cache server");

                TestUtility.DoUntilSucceed(() => sutControlAdapter.IsLocalCacheExisted(testConfig.HostedCacheServerComputerFQDNOrNetBiosName), testConfig.Timeout, testConfig.RetryInterval);
            }

            List<byte> retrievedContent = new List<byte>();

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Start PCCRR client to retrieve all the content from hosted cache server");
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Negotiate PCCRR version");
            var pccrrNegotiateRequest = pccrrClient.CreateMsgNegoRequest(
                ///[MS-PCCRR]Section 2.2.3: ProtVer: Both the Major and Minor version number can express the version range of 0x0000 to 0xFFFF. Currently, the protocol 
                ///version number MUST be set to {major = 1 (0x0001), minor = 0 (0x0000)}.
                ///Active TDI#69240: Windows server 2012 Standard RTM sets the major version to 2
                new ProtoVersion { MajorVersion = 1, MinorVersion = 0 }, //MinSupportedProtocolVersion
                new ProtoVersion { MajorVersion = 1, MinorVersion = 0 }, //MaxSupportedProtocolVersion
                cryptoAlgoId,
                MsgType_Values.MSG_NEGO_REQ);
            pccrrClient.SendPacket(
                    pccrrNegotiateRequest,
                    testConfig.Timeout);
            var pccrrNegotiateResponse
                = (PccrrNegoResponsePacket)pccrrClient.ExpectPacket();

            if (testConfig.SupportBranchCacheV1)
            {
                BaseTestSite.Assert.IsTrue(
                    pccrrNegotiateResponse.MsgNegoResp.MinSupporteProtocolVersion.MajorVersion <= 1 &&
                    pccrrNegotiateResponse.MsgNegoResp.MaxSupporteProtocolVersion.MajorVersion >= 1,
                    "SupportedProtocolVersion doesn't match configuration");
            }

            Aes aes = PccrrUtitlity.CreateAes(cryptoAlgoId);

            for (int i = 0; i < contentInformation.chunks.Length; i++)
            {
                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Retrieve all segments in chunk {0}",
                    i);

                var chunk = contentInformation.chunks[i];
                for (int j = 0; j < chunk.chunkData.Length; j++)
                {
                    // Retrieve segment list
                    var pccrrGetSegListRequest = pccrrClient.CreateMsgGetSegListRequest(
                        cryptoAlgoId,
                        Guid.NewGuid(),
                        new byte[][] { contentInformation.GetSegmentId(i, j) });
                    pccrrClient.SendPacket(
                        pccrrGetSegListRequest,
                        testConfig.Timeout);
                    var pccrrGetSegListResponse
                        = (PccrrSegListResponsePacket)pccrrClient.ExpectPacket();

                    BaseTestSite.Assert.AreNotEqual<uint>(
                        0,
                        pccrrGetSegListResponse.MsgSegList.SegmentRangeCount,
                        "The server MUST set the SegmentRangeCount field to a value greater than zero if it has the requested segments data.");

                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Retrieve segment {0} in chunk {1}",
                        j,
                        i);

                    PccrrGETBLKSRequestPacket pccrrBlkRequest = pccrrClient.CreateMsgGetBlksRequest(
                        contentInformation.GetSegmentId(i, j),
                        cryptoAlgoId,
                        MsgType_Values.MSG_GETBLKS,
                        0,
                        1);
                    pccrrClient.SendPacket(
                        pccrrBlkRequest,
                        testConfig.Timeout);
                    PccrrBLKResponsePacket pccrrBlkResponse
                        = (PccrrBLKResponsePacket)pccrrClient.ExpectPacket();

                    BaseTestSite.Assert.AreNotEqual<uint>(
                        0,
                        pccrrBlkResponse.MsgBLK.SizeOfBlock,
                        "The server MUST set the SizeOfBlock field to a value greater than zero if it has the requested blocks data.");

                    byte[] block = pccrrBlkResponse.MsgBLK.Block;

                    if (cryptoAlgoId != CryptoAlgoId_Values.NoEncryption)
                        block = PccrrUtitlity.Decrypt(aes, block, contentInformation.chunks[i].chunkData[j].SegmentSecret, pccrrBlkResponse.MsgBLK.IVBlock);

                    retrievedContent.AddRange(block);
                }
            }

            BaseTestSite.Assert.IsTrue(
                Enumerable.SequenceEqual(content, retrievedContent),
                "The retrieved cached data should be the same as server data.");
        }

        #endregion
    }
}


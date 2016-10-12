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
    public class PccrrClientMessageHeader : BranchCacheTestClassBase
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

        #region HostedCacheServer_PccrrClient_MessageHeader_CryptoAlgoIdAES192

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrClient_MessageHeader_CryptoAlgoIdAES192()
        {
            HostedCacheServer_PccrrClient_MessageHeader_CryptoAlgoId(CryptoAlgoId_Values.AES_192);
        }

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrClient_MessageHeader_CryptoAlgoIdAES192V2()
        {
            HostedCacheServer_PccrrClient_MessageHeader_CryptoAlgoIdV2(CryptoAlgoId_Values.AES_192);
        }

        #endregion

        #region HostedCacheServer_PccrrClient_MessageHeader_CryptoAlgoIdAES256

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrClient_MessageHeader_CryptoAlgoIdAES256()
        {
            CheckApplicability();

            HostedCacheServer_PccrrClient_MessageHeader_CryptoAlgoId(CryptoAlgoId_Values.AES_256);
        }

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrClient_MessageHeader_CryptoAlgoIdAES256V2()
        {
            CheckApplicability();

            HostedCacheServer_PccrrClient_MessageHeader_CryptoAlgoIdV2(CryptoAlgoId_Values.AES_256);
        }

        #endregion

        #region HostedCacheServer_PccrrClient_MessageHeader_CryptoAlgoIdNoEncryption

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrClient_MessageHeader_CryptoAlgoIdNoEncryption()
        {
            CheckApplicability();

            HostedCacheServer_PccrrClient_MessageHeader_CryptoAlgoId(CryptoAlgoId_Values.NoEncryption);
        }

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrClient_MessageHeader_CryptoAlgoIdNoEncryptionV2()
        {
            CheckApplicability();

            HostedCacheServer_PccrrClient_MessageHeader_CryptoAlgoIdV2(CryptoAlgoId_Values.NoEncryption);
        }

        #endregion

        #region HostedCacheServer_PccrrClient_MessageHeader_TypeInvalid

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        [ExpectedException(typeof(TimeoutException), "Hosted cache server should not receive the second MSG_GETBLKS request.")]
        public void HostedCacheServer_PccrrClient_MessageHeader_TypeInvalid()
        {
            CheckApplicability();

            EventQueue eventQueue = new EventQueue(BaseTestSite);
            eventQueue.Timeout = testConfig.Timeout;

            Content_Information_Data_Structure contentInformation = contentInformationUtility.CreateContentInformationV1();

            CryptoAlgoId_Values cryptoAlgoId = CryptoAlgoId_Values.AES_128;
            ProtoVersion protoVersion = new ProtoVersion { MajorVersion = 1, MinorVersion = 0 };

            BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Start PCCRR server to be ready to serve content to hosted cache server");
            using (PccrrTestInvalidMsgTypeServer pccrrTestServer = new PccrrTestInvalidMsgTypeServer())
            {
                pccrrTestServer.Start(
                    testConfig.ClientContentRetrievalListenPort,
                    cryptoAlgoId,
                    protoVersion,
                    contentInformation,
                    new byte[0],
                    eventQueue);

                PCHCClient pchcClient = new PCHCClient(
                    TransferProtocol.HTTPS,
                    testConfig.HostedCacheServerComputerName,
                    testConfig.HostedCacheServerHTTPSListenPort,
                    PchcConsts.HttpsUrl,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword);

                SEGMENT_INFO_MESSAGE segmentInfoMessage = pchcClient.CreateSegmentInfoMessage(
                        testConfig.ClientContentRetrievalListenPort,
                        contentInformation,
                        0);
                pchcClient.SendSegmentInfoMessage(segmentInfoMessage);

                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Offer PccrrBLKSResponse with invalid message type to hosted cache server");
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
                    return blockCount == 1;
                }, TimeSpan.MaxValue, TimeSpan.Zero);

                TestUtility.DoUntilSucceed(() => sutControlAdapter.IsLocalCacheExisted(testConfig.HostedCacheServerComputerFQDNOrNetBiosName), testConfig.NegativeTestTimeout, testConfig.RetryInterval);
            }
        }

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        [ExpectedException(typeof(TimeoutException), "Hosted cache server should not receive the second MSG_GETBLKS request.")]
        public void HostedCacheServer_PccrrClient_MessageHeader_TypeInvalidV2()
        {
            CheckApplicability();

            EventQueue eventQueue = new EventQueue(BaseTestSite);
            eventQueue.Timeout = testConfig.Timeout;

            Content_Information_Data_Structure_V2 contentInformationV2 = contentInformationUtility.CreateContentInformationV2();

            CryptoAlgoId_Values cryptoAlgoId = CryptoAlgoId_Values.AES_128;

            BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Start PCCRR server to be ready to serve content to hosted cache server");
            using (PccrrTestInvalidMsgTypeServerV2 pccrrTestServerV2 = new PccrrTestInvalidMsgTypeServerV2())
            {
                pccrrTestServerV2.StartListen(
                    testConfig.ClientContentRetrievalListenPort,
                    cryptoAlgoId,
                    contentInformationV2,
                    new byte[0],
                    eventQueue);

                PCHCClient pchcClient = new PCHCClient(
                    TransferProtocol.HTTP,
                    testConfig.HostedCacheServerComputerName,
                    testConfig.ContentServerHTTPListenPort,
                    PchcConsts.HttpUrl,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword);

                var batchedOfferMessage = pchcClient.CreateBatchedOfferMessage(
                    testConfig.ClientContentRetrievalListenPort,
                    contentInformationV2);
                var responseMessage = pchcClient.SendBatchedOfferMessage(batchedOfferMessage);

                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Offer PccrrBLKSResponse with invalid message type to hosted cache server");
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
                    return blockCount == 1;
                }, TimeSpan.MaxValue, TimeSpan.Zero);

                TestUtility.DoUntilSucceed(() => sutControlAdapter.IsLocalCacheExisted(testConfig.HostedCacheServerComputerFQDNOrNetBiosName), testConfig.NegativeTestTimeout, testConfig.RetryInterval);
            }
        }

        #endregion

        #region HostedCacheServer_PccrrClient_MessageHeader_ProtoVerIncompatible

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        [ExpectedException(typeof(TimeoutException), "Hosted cache server should not receive MSG_GETBLKS request.")]
        public void HostedCacheServer_PccrrClient_MessageHeader_ProtoVerIncompatible()
        {
            CheckApplicability();

            EventQueue eventQueue = new EventQueue(BaseTestSite);
            eventQueue.Timeout = testConfig.Timeout;

            Content_Information_Data_Structure contentInformation = contentInformationUtility.CreateContentInformationV1();

            CryptoAlgoId_Values cryptoAlgoId = CryptoAlgoId_Values.AES_128;
            ProtoVersion protoVersion = new ProtoVersion { MajorVersion = 1, MinorVersion = 0 };

            BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Start PCCRR server to be ready to serve content to hosted cache server");
            using (PccrrTestIncompatibleProtoVerServer pccrrTestServer = new PccrrTestIncompatibleProtoVerServer())
            {
                pccrrTestServer.Start(
                    testConfig.ClientContentRetrievalListenPort,
                    cryptoAlgoId,
                    protoVersion,
                    contentInformation,
                    new byte[0],
                    eventQueue);

                PCHCClient pchcClient = new PCHCClient(
                    TransferProtocol.HTTPS,
                    testConfig.HostedCacheServerComputerName,
                    testConfig.HostedCacheServerHTTPSListenPort,
                    PchcConsts.HttpsUrl,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword);

                SEGMENT_INFO_MESSAGE segmentInfoMessage = pchcClient.CreateSegmentInfoMessage(
                        testConfig.ClientContentRetrievalListenPort,
                        contentInformation,
                        0);
                pchcClient.SendSegmentInfoMessage(segmentInfoMessage);


                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Offer PccrrBLKSResponse with incompatible proto version to hosted cache server");
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
                    return blockCount == 1;
                }, TimeSpan.MaxValue, TimeSpan.Zero);

                TestUtility.DoUntilSucceed(() => sutControlAdapter.IsLocalCacheExisted(testConfig.HostedCacheServerComputerFQDNOrNetBiosName), testConfig.NegativeTestTimeout, testConfig.RetryInterval);
            }
        }

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        [ExpectedException(typeof(TimeoutException), "Hosted cache server should not receive the second MSG_GETBLKS request.")]
        public void HostedCacheServer_PccrrClient_MessageHeader_ProtoVerIncompatibleV2()
        {
            CheckApplicability();

            EventQueue eventQueue = new EventQueue(BaseTestSite);
            eventQueue.Timeout = testConfig.Timeout;

            Content_Information_Data_Structure_V2 contentInformationV2 = contentInformationUtility.CreateContentInformationV2();

            CryptoAlgoId_Values cryptoAlgoId = CryptoAlgoId_Values.AES_128;

            BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Start PCCRR server to be ready to serve content to hosted cache server");
            using (PccrrTestIncompatibleProtoVerServerV2 pccrrTestServerV2 = new PccrrTestIncompatibleProtoVerServerV2())
            {
                pccrrTestServerV2.StartListen(
                    testConfig.ClientContentRetrievalListenPort,
                    cryptoAlgoId,
                    contentInformationV2,
                    new byte[0],
                    eventQueue);

                PCHCClient pchcClient = new PCHCClient(
                    TransferProtocol.HTTP,
                    testConfig.HostedCacheServerComputerName,
                    testConfig.HostedCacheServerHTTPListenPort,
                    PchcConsts.HttpUrl,
                    testConfig.DomainName,
                    testConfig.UserName,
                    testConfig.UserPassword);

                var batchedOfferMessage = pchcClient.CreateBatchedOfferMessage(
                    testConfig.ClientContentRetrievalListenPort,
                    contentInformationV2);
                pchcClient.SendBatchedOfferMessage(batchedOfferMessage);


                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Offer PccrrBLKSResponse with incompatible proto version to hosted cache server");
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
                    return blockCount == 1;
                }, TimeSpan.MaxValue, TimeSpan.Zero);

                TestUtility.DoUntilSucceed(() => sutControlAdapter.IsLocalCacheExisted(testConfig.HostedCacheServerComputerFQDNOrNetBiosName), testConfig.NegativeTestTimeout, testConfig.RetryInterval);
            }
        }

        #endregion

        #region HostedCacheServer_PccrrClient_MessageHeader_CryptoAlgoId

        public void HostedCacheServer_PccrrClient_MessageHeader_CryptoAlgoId(CryptoAlgoId_Values algoId)
        {
            CheckApplicability();

            EventQueue eventQueue = new EventQueue(BaseTestSite);
            eventQueue.Timeout = testConfig.Timeout;

            byte[] content = TestUtility.GenerateRandomArray(ContentInformationUtility.DefaultBlockSize);
            Content_Information_Data_Structure contentInformation = contentInformationUtility.CreateContentInformationV1();
             
            ProtoVersion protoVersion = new ProtoVersion { MajorVersion = 1, MinorVersion = 0 };

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Start PCCRR server to be ready to serve content to hosted cache server");

            using (PccrrTestServerV1 pccrrTestServer = new PccrrTestServerV1())
            {
                pccrrTestServer.Start(
                    testConfig.ClientContentRetrievalListenPort,
                    algoId,
                    protoVersion,
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

                SEGMENT_INFO_MESSAGE segmentInfoMessage = pchcClient.CreateSegmentInfoMessage(
                        testConfig.ClientContentRetrievalListenPort,
                        contentInformation,
                        0);
                pchcClient.SendSegmentInfoMessage(segmentInfoMessage);

                BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Offer content block 0 of segment 0 to hosted cache server to hosted cache server");

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
                    return blockCount == 1;
                }, TimeSpan.MaxValue, TimeSpan.Zero);

                BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Wait until cache is available on hosted cache server");

                TestUtility.DoUntilSucceed(() => sutControlAdapter.IsLocalCacheExisted(testConfig.HostedCacheServerComputerFQDNOrNetBiosName), testConfig.Timeout, testConfig.RetryInterval);
            }

            PccrrClient pccrrClient = new PccrrClient(testConfig.HostedCacheServerComputerName, testConfig.HostedCacheServerHTTPListenPort);
            Aes aes = PccrrUtitlity.CreateAes(algoId);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Retrieve block 0 of segment 0 from hosted cache server",
                0);
            PccrrGETBLKSRequestPacket pccrrBlkRequest = pccrrClient.CreateMsgGetBlksRequest(
                    contentInformation.GetSegmentId(0),
                    algoId,
                    MsgType_Values.MSG_GETBLKS,
                    (uint)0,
                    1);
            pccrrClient.SendPacket(
                pccrrBlkRequest,
                testConfig.Timeout);
            PccrrBLKResponsePacket pccrrBlkResponse
                = (PccrrBLKResponsePacket)pccrrClient.ExpectPacket();

            byte[] block = pccrrBlkResponse.MsgBLK.Block;

            if (algoId != CryptoAlgoId_Values.NoEncryption)
                block = PccrrUtitlity.Decrypt(aes, block, contentInformation.segments[0].SegmentSecret, pccrrBlkResponse.MsgBLK.IVBlock);

            BaseTestSite.Assert.IsTrue(
            Enumerable.SequenceEqual(content, block),
            "The retrieved cached data should be the same as server data.");
        }

        public void HostedCacheServer_PccrrClient_MessageHeader_CryptoAlgoIdV2(CryptoAlgoId_Values algoId)
        {
            CheckApplicability();

            EventQueue eventQueue = new EventQueue(BaseTestSite);
            eventQueue.Timeout = testConfig.Timeout;

            byte[] content = TestUtility.GenerateRandomArray(ContentInformationUtility.DefaultBlockSize);
            Content_Information_Data_Structure_V2 contentInformationV2 = contentInformationUtility.CreateContentInformationV2();

            ProtoVersion protoVersion = new ProtoVersion { MajorVersion = 2, MinorVersion = ushort.MaxValue };

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Start PCCRR server to be ready to serve content to hosted cache server");

            using (PccrrTestServerV2 pccrrTestServerV2 = new PccrrTestServerV2())
            {
                pccrrTestServerV2.Start(
                    testConfig.ClientContentRetrievalListenPort,
                    algoId,
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

                var batchedOfferMessage = pchcClient.CreateBatchedOfferMessage(
                    testConfig.ClientContentRetrievalListenPort,
                    contentInformationV2);
                pchcClient.SendBatchedOfferMessage(batchedOfferMessage);

                BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Offer content segment 0 of chunk 0 to hosted cache server");

                int segmentCount = 0;
                TestUtility.DoUntilSucceed(delegate()
                {
                    eventQueue.Expect<MessageArrivedEventArgs>(typeof(PccrrServer).GetEvent("MessageArrived"), delegate(System.Net.IPEndPoint sender, PccrrPacket pccrrPacket)
                    {
                        var pccrrGetBlksRequest = pccrrPacket as PccrrGETBLKSRequestPacket;

                        if (pccrrGetBlksRequest != null)
                        {
                            segmentCount++;
                        }
                    });
                    return segmentCount == 1;
                }, TimeSpan.MaxValue, TimeSpan.Zero);

                BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Wait until cache is available on hosted cache server");

                TestUtility.DoUntilSucceed(() => sutControlAdapter.IsLocalCacheExisted(testConfig.HostedCacheServerComputerFQDNOrNetBiosName), testConfig.Timeout, testConfig.RetryInterval);
            }

            PccrrClient pccrrClient = new PccrrClient(testConfig.HostedCacheServerComputerName, testConfig.HostedCacheServerHTTPListenPort);
            Aes aes = PccrrUtitlity.CreateAes(algoId);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Retrieve segment 0 of chunk 0 from hosted cache server",
                0);
            PccrrGETBLKSRequestPacket pccrrBlkRequest = pccrrClient.CreateMsgGetBlksRequest(
                    contentInformationV2.GetSegmentId(0,0),
                    algoId,
                    MsgType_Values.MSG_GETBLKS,
                    (uint)0,
                    1);
            pccrrClient.SendPacket(
                pccrrBlkRequest,
                testConfig.Timeout);
            PccrrBLKResponsePacket pccrrBlkResponse
                = (PccrrBLKResponsePacket)pccrrClient.ExpectPacket();

            byte[] data = pccrrBlkResponse.MsgBLK.Block;

            if (algoId != CryptoAlgoId_Values.NoEncryption)
                data = PccrrUtitlity.Decrypt(aes, data, contentInformationV2.chunks[0].chunkData[0].SegmentSecret, pccrrBlkResponse.MsgBLK.IVBlock);

            BaseTestSite.Assert.IsTrue(
            Enumerable.SequenceEqual(content.Take((int)contentInformationV2.chunks[0].chunkData[0].cbSegment), data),
            "The retrieved cached data should be the same as server data.");
        }

        #endregion

        #region Test Servers

        class PccrrTestInvalidMsgTypeServer : PccrrTestServerV1
        {
            private bool IsFirstMsgGetBlksArrived = false;
            private bool IsSecondMsgGetBlksArrived = false;
            private Content_Information_Data_Structure contentInformation;

            protected override void HandlePccrrGETBLKSRequestPacket(PccrrGETBLKSRequestPacket pccrrGetBlkRequest)
            {
                if (!IsFirstMsgGetBlksArrived)
                {
                    int segmentIndex = -1;
                    for (int i = 0; i < contentInformation.cSegments; i++)
                    {
                        if (Enumerable.SequenceEqual(contentInformation.GetSegmentId(i), pccrrGetBlkRequest.MsgGetBLKS.SegmentID))
                        {
                            segmentIndex = i;
                            break;
                        }
                    }

                    PccrrBLKResponsePacket pccrrBlocksResponse;
                    if (segmentIndex == -1) // Mached segement not found
                    {
                        pccrrBlocksResponse = pccrrServer.CreateMsgBlkResponse(
                            pccrrGetBlkRequest.MsgGetBLKS.SegmentID,
                            new byte[0],
                            cryptoAlgoId,
                            (MsgType_Values)0xFEFE,
                            this.iv,
                            0,
                            0);
                    }
                    else
                    {
                        ulong blockSize = PccrcConsts.V1BlockSize;
                        byte[] block = content.Skip((int)(contentInformation.segments[segmentIndex].ullOffsetInContent + pccrrGetBlkRequest.MsgGetBLKS.ReqBlockRanges[0].Index * blockSize)).Take((int)blockSize).ToArray();

                        if (cryptoAlgoId != CryptoAlgoId_Values.NoEncryption)
                            block = PccrrUtitlity.Encrypt(aes, block, contentInformation.segments[segmentIndex].SegmentSecret, iv);

                        pccrrBlocksResponse = pccrrServer.CreateMsgBlkResponse(
                            pccrrGetBlkRequest.MsgGetBLKS.SegmentID,
                            block,
                            cryptoAlgoId,
                            (MsgType_Values)0xFEFE,
                            this.iv,
                            pccrrGetBlkRequest.MsgGetBLKS.ReqBlockRanges[0].Index,
                            pccrrGetBlkRequest.MsgGetBLKS.ReqBlockRanges[0].Index == contentInformation.cSegments - 1 ? 0 : pccrrGetBlkRequest.MsgGetBLKS.ReqBlockRanges[0].Index + 1);
                    }
                    pccrrServer.SendPacket(pccrrBlocksResponse);

                    IsFirstMsgGetBlksArrived = true;
                }

                else
                {
                    IsSecondMsgGetBlksArrived = true;
                }
            }

            public bool CheckIfSecondMsgGetBlksArrived()
            {
                return IsSecondMsgGetBlksArrived;
            }
        }

        class PccrrTestInvalidMsgTypeServerV2 : PccrrTestServerV2
        {
            private Content_Information_Data_Structure_V2 contentInformationV2;

            public void StartListen(int port, CryptoAlgoId_Values cryptoAlgoId, Content_Information_Data_Structure_V2 contentInformationV2, byte[] content, EventQueue eventQueue)
            {
                this.contentInformationV2 = contentInformationV2;

                base.Start(port, cryptoAlgoId, content, eventQueue);
            }

            protected override void HandlePccrrGETBLKSRequestPacket(PccrrGETBLKSRequestPacket pccrrGetBlkRequest)
            {
                uint offset = 0;
                int chunkIndex = -1;
                int segmentIndex = -1;
                for (int i = 0; i < contentInformationV2.chunks.Length; i++)
                {
                    var chunk = contentInformationV2.chunks[i];
                    for (int j = 0; j < chunk.chunkData.Length; j++)
                    {
                        if (Enumerable.SequenceEqual(contentInformationV2.GetSegmentId(i, j), pccrrGetBlkRequest.MsgGetBLKS.SegmentID))
                        {
                            chunkIndex = i;
                            segmentIndex = j;
                            break;
                        }
                        else
                        {
                            offset += chunk.chunkData[j].cbSegment;
                        }
                    }
                }
                PccrrBLKResponsePacket pccrrBlocksResponse;
                if (segmentIndex == -1) // Mached segement not found
                {
                    pccrrBlocksResponse = pccrrServer.CreateMsgBlkResponse(
                        pccrrGetBlkRequest.MsgGetBLKS.SegmentID,
                        new byte[0],
                        cryptoAlgoId,
                        (MsgType_Values)0xFEFE,
                        iv,
                        0,
                        0);
                }
                else
                {
                    var segment = contentInformationV2.chunks[chunkIndex].chunkData[segmentIndex];

                    byte[] block = content.Skip((int)offset).Take((int)segment.cbSegment).ToArray();

                    if (cryptoAlgoId != CryptoAlgoId_Values.NoEncryption)
                        block = PccrrUtitlity.Encrypt(aes, block, contentInformationV2.chunks[chunkIndex].chunkData[segmentIndex].SegmentSecret, iv);

                    pccrrBlocksResponse = pccrrServer.CreateMsgBlkResponse(
                        pccrrGetBlkRequest.MsgGetBLKS.SegmentID,
                        block,
                        cryptoAlgoId,
                        (MsgType_Values)0xFEFE,
                        iv,
                        0,
                        0);
                }
                pccrrServer.SendPacket(pccrrBlocksResponse);
            }
        }

        class PccrrTestIncompatibleProtoVerServer : PccrrTestServerV1
        {
            private Content_Information_Data_Structure contentInformation;

            protected override void HandlePccrrGETBLKSRequestPacket(PccrrGETBLKSRequestPacket pccrrGetBlkRequest)
            {
                int segmentIndex = -1;
                for (int i = 0; i < contentInformation.cSegments; i++)
                {
                    if (Enumerable.SequenceEqual(contentInformation.GetSegmentId(i), pccrrGetBlkRequest.MsgGetBLKS.SegmentID))
                    {
                        segmentIndex = i;
                        break;
                    }
                }

                PccrrBLKResponsePacket pccrrBlocksResponse;
                if (segmentIndex == -1) // Mached segement not found
                {
                    pccrrBlocksResponse = pccrrServer.CreateMsgBlkResponse(
                        pccrrGetBlkRequest.MsgGetBLKS.SegmentID,
                        new byte[0],
                        cryptoAlgoId,
                        MsgType_Values.MSG_BLK,
                        iv,
                        0,
                        0);
                }
                else
                {
                    ulong blockSize = PccrcConsts.V1BlockSize;
                    byte[] block = content.Skip((int)(contentInformation.segments[segmentIndex].ullOffsetInContent + pccrrGetBlkRequest.MsgGetBLKS.ReqBlockRanges[0].Index * blockSize)).Take((int)blockSize).ToArray();

                    if (cryptoAlgoId != CryptoAlgoId_Values.NoEncryption)
                        block = PccrrUtitlity.Encrypt(aes, block, contentInformation.segments[segmentIndex].SegmentSecret, iv);

                    pccrrBlocksResponse = pccrrServer.CreateMsgBlkResponse(
                        pccrrGetBlkRequest.MsgGetBLKS.SegmentID,
                        new byte[0],
                        cryptoAlgoId,
                        MsgType_Values.MSG_BLK,
                        iv,
                        0,
                        0);
                }

                TestTools.StackSdk.BranchCache.Pccrr.MESSAGE_HEADER header = pccrrBlocksResponse.MessageHeader;
                header.ProtVer = new ProtoVersion { MajorVersion = 0xFEFE, MinorVersion = 0xFEFE };
                pccrrBlocksResponse.MessageHeader = header;

                pccrrServer.SendPacket(pccrrBlocksResponse);
            }
        }

        class PccrrTestIncompatibleProtoVerServerV2 : PccrrTestServerV2
        {
            private Content_Information_Data_Structure_V2 contentInformationV2;

            public void StartListen(int port, CryptoAlgoId_Values cryptoAlgoId, Content_Information_Data_Structure_V2 contentInformationV2, byte[] content, EventQueue eventQueue)
            {
                this.contentInformationV2 = contentInformationV2;

                base.Start(port, cryptoAlgoId, content, eventQueue);
            }

            protected override void HandlePccrrGETBLKSRequestPacket(PccrrGETBLKSRequestPacket pccrrGetBlkRequest)
            {
                uint offset = 0;
                int chunkIndex = -1;
                int segmentIndex = -1;
                for (int i = 0; i < contentInformationV2.chunks.Length; i++)
                {
                    var chunk = contentInformationV2.chunks[i];
                    for (int j = 0; j < chunk.chunkData.Length; j++)
                    {
                        if (Enumerable.SequenceEqual(contentInformationV2.GetSegmentId(i, j), pccrrGetBlkRequest.MsgGetBLKS.SegmentID))
                        {
                            chunkIndex = i;
                            segmentIndex = j;
                            break;
                        }
                        else
                        {
                            offset += chunk.chunkData[j].cbSegment;
                        }
                    }
                }
                PccrrBLKResponsePacket pccrrBlocksResponse;
                if (segmentIndex == -1) // Mached segement not found
                {
                    pccrrBlocksResponse = pccrrServer.CreateMsgBlkResponse(
                        pccrrGetBlkRequest.MsgGetBLKS.SegmentID,
                        new byte[0],
                        cryptoAlgoId,
                        (MsgType_Values)0xFEFE,
                        iv,
                        0,
                        0);
                }
                else
                {
                    var segment = contentInformationV2.chunks[chunkIndex].chunkData[segmentIndex];

                    byte[] block = content.Skip((int)offset).Take((int)segment.cbSegment).ToArray();

                    if (cryptoAlgoId != CryptoAlgoId_Values.NoEncryption)
                        block = PccrrUtitlity.Encrypt(aes, block, contentInformationV2.chunks[chunkIndex].chunkData[segmentIndex].SegmentSecret, iv);

                    pccrrBlocksResponse = pccrrServer.CreateMsgBlkResponse(
                        pccrrGetBlkRequest.MsgGetBLKS.SegmentID,
                        block,
                        cryptoAlgoId,
                        (MsgType_Values)0xFEFE,
                        iv,
                        0,
                        0);
                }

                TestTools.StackSdk.BranchCache.Pccrr.MESSAGE_HEADER header = pccrrBlocksResponse.MessageHeader;
                header.ProtVer = new ProtoVersion { MajorVersion = 0xFEFE, MinorVersion = 0xFEFE };
                pccrrBlocksResponse.MessageHeader = header;

                pccrrServer.SendPacket(pccrrBlocksResponse);
            }
        }

        #endregion
    }
}

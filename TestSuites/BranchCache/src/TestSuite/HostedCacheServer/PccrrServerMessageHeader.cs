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
    public class PccrrServerMessageHeaderVerification : BranchCacheTestClassBase
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

        #region HostedCacheServer_PccrrServer_MessageHeader_MsgTypeInvalid

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrServer_MessageHeader_MsgTypeInvalid()
        {
            CheckApplicability();

            ProtoVersion protoVersion = new ProtoVersion { MajorVersion = 1, MinorVersion = 0 };

            PccrrClient pccrrClient = new PccrrClient(testConfig.HostedCacheServerComputerName, testConfig.HostedCacheServerHTTPListenPort);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Send negoRequest with invalid message type to hosted cache server");

            var negoRequest = pccrrClient.CreateMsgNegoRequest(
                protoVersion,
                protoVersion,
                CryptoAlgoId_Values.AES_128,
                MsgType_Values.MSG_NEGO_REQ);

            var header = negoRequest.MessageHeader;
            header.MsgType = (MsgType_Values)0xFEFE;
            negoRequest.MessageHeader = header;

            bool passed = false;
            try
            {
                pccrrClient.ExpectPacket();
            }
            catch
            {
                passed = true;
            }

            BaseTestSite.Assert.IsTrue(passed, "The pccrr server should silently discard the message with invalid message type");
        }

        #endregion

        #region HostedCacheServer_PccrrServer_MessageHeader_VersionIncompatible

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrServer_MessageHeader_VersionIncompatible()
        {
            CheckApplicability();

            ProtoVersion protoVersion = new ProtoVersion { MajorVersion = 1, MinorVersion = 0 };

            PccrrClient pccrrClient = new PccrrClient(testConfig.HostedCacheServerComputerName, testConfig.HostedCacheServerHTTPListenPort);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Send negoRequest with incompatible proto version to hosted cache server");

            var negoRequest = pccrrClient.CreateMsgNegoRequest(
                protoVersion,
                protoVersion,
                CryptoAlgoId_Values.AES_128,
                MsgType_Values.MSG_NEGO_REQ);

            var header = negoRequest.MessageHeader;
            header.ProtVer = new ProtoVersion { MajorVersion = 0xFEFE, MinorVersion = 0xFEFE };
            negoRequest.MessageHeader = header;

            bool passed = false;
            try
            {
                pccrrClient.ExpectPacket();
            }
            catch
            {
                passed = true;
            }

            BaseTestSite.Assert.IsTrue(passed, "The pccrr server should silently discard the message with incompatible proto version");
        }

        #endregion

        #region HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoIdInvalid

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoIdInvalid()
        {
            CheckApplicability();

            ProtoVersion protoVersion = new ProtoVersion { MajorVersion = 1, MinorVersion = 0 };

            PccrrClient pccrrClient = new PccrrClient(testConfig.HostedCacheServerComputerName, testConfig.HostedCacheServerHTTPListenPort);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Send negoRequest with invalid CryptoAlgoId to hosted cache server");

            var negoRequest = pccrrClient.CreateMsgNegoRequest(
                protoVersion,
                protoVersion,
                CryptoAlgoId_Values.AES_128,
                MsgType_Values.MSG_NEGO_REQ);

            var header = negoRequest.MessageHeader;
            header.CryptoAlgoId = (CryptoAlgoId_Values)0xFEFE;
            negoRequest.MessageHeader = header;

            bool passed = false;
            try
            {
                pccrrClient.ExpectPacket();
            }
            catch
            {
                passed = true;
            }

            BaseTestSite.Assert.IsTrue(passed, "The pccrr server should silently discard the message with invalid CryptoAlgoId");
        }

        #endregion

        #region HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoIdAES192

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoIdAES192()
        {
            CheckApplicability();

            HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoId(CryptoAlgoId_Values.AES_192);
        }

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoIdAES192V2()
        {
            CheckApplicability();

            HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoIdV2(CryptoAlgoId_Values.AES_192);
        }

        #endregion

        #region HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoIdAES256

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoIdAES256()
        {
            CheckApplicability();

            HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoId(CryptoAlgoId_Values.AES_256);
        }

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoIdAES256V2()
        {
            CheckApplicability();

            HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoIdV2(CryptoAlgoId_Values.AES_256);
        }

        #endregion

        #region HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoIdNoEncryption

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoIdNoEncryption()
        {
            CheckApplicability();

            HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoId(CryptoAlgoId_Values.NoEncryption);
        }

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoIdNoEncryptionV2()
        {
            CheckApplicability();

            HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoIdV2(CryptoAlgoId_Values.NoEncryption);
        }

        #endregion

        #region HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoId

        public void HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoId(CryptoAlgoId_Values algoId)
        {
            CheckApplicability();

            EventQueue eventQueue = new EventQueue(BaseTestSite);
            eventQueue.Timeout = testConfig.Timeout;

            byte[] content = TestUtility.GenerateRandomArray(ContentInformationUtility.DefaultBlockSize);
            Content_Information_Data_Structure contentInformation = contentInformationUtility.CreateContentInformationV1();

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Start PCCRR server to be ready to serve content to hosted cache server");

            using (PccrrTestServerV1 pccrrTestServer = new PccrrTestServerV1())
            {
                pccrrTestServer.Start(
                    testConfig.ClientContentRetrievalListenPort,
                    algoId,
                    new ProtoVersion { MajorVersion = 1, MinorVersion = 0 },
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

                BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Directly Supply segment info to hosted cache server");

                SEGMENT_INFO_MESSAGE segmentInfoMessage = pchcClient.CreateSegmentInfoMessage(
                    testConfig.ClientContentRetrievalListenPort,
                    contentInformation,
                    0);
                pchcClient.SendSegmentInfoMessage(segmentInfoMessage);

                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Make sure block 0 in segment 0 is retrieved by hosted cache server");

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
                "Retrieve block 0 in segment 0 from hosted cache server");
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

        public void HostedCacheServer_PccrrServer_MessageHeader_CryptoAlgoIdV2(CryptoAlgoId_Values algoId)
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

                BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Directly Supply segment info to hosted cache server");

                var batchedOfferMessage = pchcClient.CreateBatchedOfferMessage(
                    testConfig.ClientContentRetrievalListenPort,
                    contentInformationV2);
                pchcClient.SendBatchedOfferMessage(batchedOfferMessage);

                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Make sure segment 0 in chunk 0 is retrieved by hosted cache server");

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
                "Retrieve block 0 in segment 0 from hosted cache server");
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
    }
}


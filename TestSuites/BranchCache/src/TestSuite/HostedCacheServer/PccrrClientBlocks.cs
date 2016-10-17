// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack.Enum;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr;
using System.Net;
using System.Threading;

namespace Microsoft.Protocols.TestSuites.BranchCache.TestSuite.HostedCacheServer
{
    [TestClass]
    public class PccrrClientBlocks : BranchCacheTestClassBase
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

        #region HostedCacheServer_PchcServer_GetBlocks_SizeOfBlockNotMatch

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PchcServer_GetBlocks_SizeOfBlockNotMatch()
        {
            CheckApplicability();

            var contentInformation = contentInformationUtility.CreateContentInformationV1();

            using (var pccrrServer = new PccrrServer(testConfig.ClientContentRetrievalListenPort))
            {
                bool blockRetrieved = false;
                pccrrServer.MessageArrived += new MessageArrivedEventArgs(delegate(IPEndPoint sender, PccrrPacket pccrrPacket)
                    {
                        var pccrrGetBlkRequest = (PccrrGETBLKSRequestPacket)pccrrPacket;

                        if (pccrrGetBlkRequest != null)
                        {
                            BaseTestSite.Log.Add(
                                LogEntryKind.Debug,
                                "PCCRR GetBlks request received from hosted cache server. Send malformed Blks response");

                            var pccrrBlocksResponse = pccrrServer.CreateMsgBlkResponse(
                                pccrrGetBlkRequest.MsgGetBLKS.SegmentID,
                                TestUtility.GenerateRandomArray(ContentInformationUtility.DefaultBlockSize),
                                CryptoAlgoId_Values.AES_128,
                                MsgType_Values.MSG_BLK,
                                TestUtility.GenerateRandomArray(16),
                                0,
                                0);
                            var blk = pccrrBlocksResponse.MsgBLK;
                            blk.SizeOfBlock = 0;
                            pccrrBlocksResponse.MsgBLK = blk;
                            pccrrServer.SendPacket(pccrrBlocksResponse);

                            blockRetrieved = true;
                        }
                    });
                pccrrServer.StartListening();

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
                    "Offer block to hosted cache server");

                SEGMENT_INFO_MESSAGE segmentInfoMessage = pchcClient.CreateSegmentInfoMessage(
                    testConfig.ClientContentRetrievalListenPort,
                    contentInformation,
                    0);
                pchcClient.SendSegmentInfoMessage(segmentInfoMessage);

                TestUtility.DoUntilSucceed(() =>
                {
                    return blockRetrieved;
                }, testConfig.Timeout, testConfig.RetryInterval);
            }

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Try to retrieve the block from hosted cache server");

            PccrrClient pccrrClient = new PccrrClient(
                testConfig.HostedCacheServerComputerName,
                testConfig.HostedCacheServerHTTPListenPort);

            PccrrGETBLKSRequestPacket pccrrBlkRequest = pccrrClient.CreateMsgGetBlksRequest(
                contentInformation.GetSegmentId(0),
                CryptoAlgoId_Values.AES_128,
                MsgType_Values.MSG_GETBLKS,
                0,
                1);
            pccrrClient.SendPacket(
                pccrrBlkRequest,
                testConfig.Timeout);
            PccrrBLKResponsePacket pccrrBlkResponse
                = (PccrrBLKResponsePacket)pccrrClient.ExpectPacket();

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                pccrrBlkResponse.MsgBLK.SizeOfBlock,
                "The server MUST set the SizeOfBlock field to zero if blocks data is not available.");
        }

        #endregion

        #region HostedCacheServer_PchcServer_GetBlocks_SizeOfIVBlockNotMatch

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PchcServer_GetBlocks_SizeOfIVBlockNotMatch()
        {
            CheckApplicability();

            var contentInformation = contentInformationUtility.CreateContentInformationV1();

            using (var pccrrServer = new PccrrServer(testConfig.ClientContentRetrievalListenPort))
            {
                bool blockRetrieved = false;
                pccrrServer.MessageArrived += new MessageArrivedEventArgs(delegate(IPEndPoint sender, PccrrPacket pccrrPacket)
                {
                    var pccrrGetBlkRequest = (PccrrGETBLKSRequestPacket)pccrrPacket;

                    if (pccrrGetBlkRequest != null)
                    {
                        BaseTestSite.Log.Add(
                            LogEntryKind.Debug,
                            "PCCRR GetBlks request received from hosted cache server. Send malformed Blks response");

                        var pccrrBlocksResponse = pccrrServer.CreateMsgBlkResponse(
                            pccrrGetBlkRequest.MsgGetBLKS.SegmentID,
                            TestUtility.GenerateRandomArray(ContentInformationUtility.DefaultBlockSize),
                            CryptoAlgoId_Values.AES_128,
                            MsgType_Values.MSG_BLK,
                            TestUtility.GenerateRandomArray(16),
                            0,
                            0);
                        var blk = pccrrBlocksResponse.MsgBLK;
                        blk.SizeOfIVBlock = 0;
                        pccrrBlocksResponse.MsgBLK = blk;
                        pccrrServer.SendPacket(pccrrBlocksResponse);

                        blockRetrieved = true;
                    }
                });
                pccrrServer.StartListening();

                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Offer block to hosted cache server");

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

                TestUtility.DoUntilSucceed(() =>
                {
                    return blockRetrieved;
                }, testConfig.Timeout, testConfig.RetryInterval);
            }

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Try to retrieve the block from hosted cache server");

            PccrrClient pccrrClient = new PccrrClient(
                testConfig.HostedCacheServerComputerName,
                testConfig.HostedCacheServerHTTPListenPort);

            PccrrGETBLKSRequestPacket pccrrBlkRequest = pccrrClient.CreateMsgGetBlksRequest(
                contentInformation.GetSegmentId(0),
                CryptoAlgoId_Values.AES_128,
                MsgType_Values.MSG_GETBLKS,
                0,
                1);
            pccrrClient.SendPacket(
                pccrrBlkRequest,
                testConfig.Timeout);
            PccrrBLKResponsePacket pccrrBlkResponse
                = (PccrrBLKResponsePacket)pccrrClient.ExpectPacket();

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                pccrrBlkResponse.MsgBLK.SizeOfBlock,
                "The server MUST set the SizeOfBlock field to zero if blocks data is not available.");
        }

        #endregion
    }
}

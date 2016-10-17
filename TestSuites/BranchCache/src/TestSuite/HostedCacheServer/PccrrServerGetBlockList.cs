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
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack.Enum;

namespace Microsoft.Protocols.TestSuites.BranchCache.TestSuite.HostedCacheServer
{
    [TestClass]
    public class PccrrServerGetBlockList : BranchCacheTestClassBase
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

        #region HostedCacheServer_PccrrServer_GetBlockList_BlockRangeCountNotMatch

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        [ExpectedException(typeof(WebException), "Hosted cache server should drop packets with invalid block range count")]
        public void HostedCacheServer_PccrrServer_GetBlockList_BlockRangeCountNotMatch()
        {
            CheckApplicability();

            var contentInformation = contentInformationUtility.CreateContentInformationV1();

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Send malformed GetBlockList request to hosted cache server");

            PccrrClient pccrrClient = new PccrrClient(
                testConfig.HostedCacheServerComputerName,
                testConfig.HostedCacheServerHTTPListenPort);

            var pccrrBlkListRequest = pccrrClient.CreateMsgGetBlkListRequest(
                contentInformation.GetSegmentId(0),
                new BLOCK_RANGE[] { new BLOCK_RANGE { Index = 0, Count = 1 } },
                CryptoAlgoId_Values.AES_128,
                MsgType_Values.MSG_GETBLKLIST);
            var blockList = pccrrBlkListRequest.MsgGetBLKLIST;
            blockList.NeededBlocksRangeCount = 10;
            pccrrBlkListRequest.MsgGetBLKLIST = blockList;
            pccrrClient.SendPacket(
                    pccrrBlkListRequest,
                    testConfig.Timeout);

            pccrrClient.ExpectPacket();
        }

        #endregion

        #region HostedCacheServer_PccrrServer_GetBlockList_BlockRangesEmpty

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrServer_GetBlockList_BlockRangesEmpty()
        {
            CheckApplicability();

            var contentInformation = contentInformationUtility.CreateContentInformationV1();

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Send GetBlockList request with no block range to hosted cache server");

            PccrrClient pccrrClient = new PccrrClient(
                testConfig.HostedCacheServerComputerName,
                testConfig.HostedCacheServerHTTPListenPort);

            var pccrrBlkListRequest = pccrrClient.CreateMsgGetBlkListRequest(
                contentInformation.GetSegmentId(0),
                new BLOCK_RANGE[0],
                CryptoAlgoId_Values.AES_128,
                MsgType_Values.MSG_GETBLKLIST);
            pccrrClient.SendPacket(
                    pccrrBlkListRequest,
                    testConfig.Timeout);
            var pccrrBlkListResponse = (PccrrBLKLISTResponsePacket)pccrrClient.ExpectPacket();

            BaseTestSite.Assert.AreEqual<uint>(
                0,
                pccrrBlkListResponse.MsgBLKLIST.BlockRangeCount,
                "Hosted cache server should not return any block since no block is requested");
        }

        #endregion
    }
}

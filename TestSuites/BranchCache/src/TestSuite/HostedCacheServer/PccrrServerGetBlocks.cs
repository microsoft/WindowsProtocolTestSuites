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
    public class PccrrServerGetBlocks : BranchCacheTestClassBase
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

        #region HostedCacheServer_PccrrServer_GetBlocks_BlockRangeCountNotMatch

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrServer_GetBlocks_BlockRangeCountNotMatch()
        {
            CheckApplicability();

            var contentInformation = contentInformationUtility.CreateContentInformationV1();

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Send malformed GetBlocks request to hosted cache server");

            PccrrClient pccrrClient = new PccrrClient(
                testConfig.HostedCacheServerComputerName,
                testConfig.HostedCacheServerHTTPListenPort);

            PccrrGETBLKSRequestPacket pccrrBlkRequest = pccrrClient.CreateMsgGetBlksRequest(
                        contentInformation.GetSegmentId(0),
                        CryptoAlgoId_Values.AES_128,
                        MsgType_Values.MSG_GETBLKS,
                        0,
                        1);
            var block = pccrrBlkRequest.MsgGetBLKS;
            block.ReqBlockRangeCount = 10;
            pccrrBlkRequest.MsgGetBLKS = block;
            pccrrClient.SendPacket(
                pccrrBlkRequest,
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

            BaseTestSite.Assert.IsTrue(passed, "Hosted cache server should drop GetBlocks packets with invalid block count");
        }

        #endregion

        #region HostedCacheServer_PccrrServer_GetBlocks_BlockRangesEmpty

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PccrrServer_GetBlocks_BlockRangesEmpty()
        {
            CheckApplicability();

            var contentInformation = contentInformationUtility.CreateContentInformationV1();

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Send GetBlocks request with no block range to hosted cache server");

            PccrrClient pccrrClient = new PccrrClient(
                testConfig.HostedCacheServerComputerName,
                testConfig.HostedCacheServerHTTPListenPort);

            PccrrGETBLKSRequestPacket pccrrBlkRequest = pccrrClient.CreateMsgGetBlksRequest(
                contentInformation.GetSegmentId(0),
                CryptoAlgoId_Values.AES_128,
                MsgType_Values.MSG_GETBLKS,
                0,
                0);
            pccrrClient.SendPacket(
                pccrrBlkRequest,
                testConfig.Timeout);
            PccrrBLKResponsePacket pccrrBlkResponse
                = (PccrrBLKResponsePacket)pccrrClient.ExpectPacket();

            BaseTestSite.Assert.AreEqual(
                0,
                pccrrBlkResponse.MsgBLK.Block.Length,
                "Hosted cache server should not return any block since no block is requested");
        }

        #endregion
    }
}

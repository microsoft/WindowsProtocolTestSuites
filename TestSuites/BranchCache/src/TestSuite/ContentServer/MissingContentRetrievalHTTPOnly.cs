// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.Messages;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrd;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack.Enum;
using Microsoft.Protocols.TestSuites.BranchCache.TestSuite;

namespace Microsoft.Protocols.TestSuites.BranchCache.TestSuite.ContentServer
{
    [TestClass]
    public class MissingContentRetrievalHTTPOnly : BranchCacheTestClassBase
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
        }

        protected override void TestCleanup()
        {
            ResetContentServer();

            base.TestCleanup();
        }

        #endregion

        #region ContentServer_BVT_MissingContentRetrieval_HTTPOnly_V1

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("ContentServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("PCCRTP")]
        public void ContentServer_BVT_MissingContentRetrieval_HTTPOnly_V1()
        {
            CheckApplicability();

            RetrieveMissingContent(BranchCacheVersion.V1);
        }

        #endregion

        #region ContentServer_BVT_MissingContentRetrieval_HTTPOnly_V2

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("ContentServer")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        public void ContentServer_BVT_MissingContentRetrieval_HTTPOnly_V2()
        {
            CheckApplicability();

            RetrieveMissingContent(BranchCacheVersion.V2);
        }

        #endregion

        #region Private Methods

        public void RetrieveMissingContent(BranchCacheVersion branchCacheVersion)
        {
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Trigger hash generation on content server");

            var content = contentInformationUtility.RetrieveContentData();

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Wait until the hash is generated on content server");

            TestUtility.DoUntilSucceed(() => sutControlAdapter.IsHTTPHashExisted(testConfig.ContentServerComputerFQDNOrNetBiosName), testConfig.Timeout, testConfig.RetryInterval);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Retrieve missing content");

            var pccrtpClient = new PccrtpClient();

            var pccrtpRequest = pccrtpClient.CreatePccrtpRequest(
                testConfig.ContentServerComputerName,
                testConfig.ContentServerHTTPListenPort,
                testConfig.NameOfFileWithMultipleBlocks,
                branchCacheVersion,
                true);
            var pccrtpResponse = pccrtpClient.SendHttpRequest(
                Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp.HttpVersionType.HttpVersion11,
                pccrtpRequest,
                (int)testConfig.Timeout.TotalMilliseconds);

            BaseTestSite.Assert.AreNotEqual(
                "peerdist",
                pccrtpResponse.HttpResponse.ContentEncoding,
                "The content server should reply with the content");

            byte[] retrievedContent = pccrtpResponse.PayloadData;

            BaseTestSite.Assert.IsTrue(
                Enumerable.SequenceEqual(content, retrievedContent),
                "The retrieved missing data should be the same as server data.");
        }

        #endregion
    }
}

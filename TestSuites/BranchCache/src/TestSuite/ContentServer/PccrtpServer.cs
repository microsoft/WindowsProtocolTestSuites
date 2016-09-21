// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack;

namespace Microsoft.Protocols.TestSuites.BranchCache.TestSuite.ContentServer
{
    [TestClass]
    public class PccrtpServer : BranchCacheTestClassBase
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

        #region ContentServer_PccrtpServer_ContentEncodingNotHavePeerDist

        [TestMethod]
        [TestCategory("ContentServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        public void ContentServer_PccrtpServer_ContentEncodingNotHavePeerDist()
        {
            CheckApplicability();

            PccrtpClient pccrtpClient = new PccrtpClient();
            PccrtpRequest pccrtpRequest = pccrtpClient.CreatePccrtpRequest(
                testConfig.ContentServerComputerName,
                testConfig.ContentServerHTTPListenPort,
                testConfig.NameOfFileWithMultipleBlocks);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Send PCCRTP request to trigger hash generation");

            PccrtpResponse pccrtpResponse = pccrtpClient.SendHttpRequest(
                Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp.HttpVersionType.HttpVersion11,
                pccrtpRequest,
                (int)testConfig.Timeout.TotalMilliseconds);

            var contentBeforeHashGeneration = pccrtpResponse.PayloadData;

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Wait until the hash is generated on content server");

            TestUtility.DoUntilSucceed(() => sutControlAdapter.IsHTTPHashExisted(testConfig.ContentServerComputerFQDNOrNetBiosName), testConfig.Timeout, testConfig.RetryInterval);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Send HTTP request again without peerdist Accept-Encoding");

            var contentAfterHashGeneration = TestUtility.DownloadHTTPFile(testConfig.ContentServerComputerName, testConfig.NameOfFileWithMultipleBlocks);

            BaseTestSite.Assert.IsTrue(
               contentAfterHashGeneration.SequenceEqual(contentBeforeHashGeneration),
               "The content server should return file content when peerdist is not included in Accept-Encoding");
        }

        #endregion

        #region ContentServer_PccrtpServer_VersionIncompatible

        [TestMethod]
        [TestCategory("ContentServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        public void ContentServer_PccrtpServer_VersionIncompatible()
        {
            CheckApplicability();

            PccrtpClient pccrtpClient = new PccrtpClient();
            PccrtpRequest pccrtpRequest = pccrtpClient.CreatePccrtpRequest(
                testConfig.ContentServerComputerName,
                testConfig.ContentServerHTTPListenPort,
                testConfig.NameOfFileWithMultipleBlocks);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Send PCCRTP request to trigger hash generation");

            PccrtpResponse pccrtpResponse = pccrtpClient.SendHttpRequest(
                Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp.HttpVersionType.HttpVersion11,
                pccrtpRequest,
                (int)testConfig.Timeout.TotalMilliseconds);

            var contentBeforeHashGeneration = pccrtpResponse.PayloadData;

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Wait until the hash is generated on content server");

            TestUtility.DoUntilSucceed(() => sutControlAdapter.IsHTTPHashExisted(testConfig.ContentServerComputerFQDNOrNetBiosName), testConfig.Timeout, testConfig.RetryInterval);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Modify to an incompatible version");

            pccrtpRequest.HttpHeader[PccrtpConsts.XP2PPeerDistHttpHeader] = "Version=999.999";

            pccrtpResponse = pccrtpClient.SendHttpRequest(
                Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp.HttpVersionType.HttpVersion11,
                pccrtpRequest,
                (int)testConfig.Timeout.TotalMilliseconds);

            var contentAfterHashGeneration = pccrtpResponse.PayloadData;

            BaseTestSite.Assert.AreNotEqual(
                "peerdist",
                pccrtpResponse.HttpResponse.ContentEncoding,
                "The content server should return file content when version is not compatible");

            BaseTestSite.Assert.IsTrue(
               contentAfterHashGeneration.SequenceEqual(contentBeforeHashGeneration),
               "The content server should return file content when version is not compatible");
        }

        #endregion

        #region ContentServer_PccrtpServer_MissingDataRequestFalse

        [TestMethod]
        [TestCategory("ContentServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        public void ContentServer_PccrtpServer_MissingDataRequestFalse()
        {
            CheckApplicability();

            PccrtpClient pccrtpClient = new PccrtpClient();
            PccrtpRequest pccrtpRequest = pccrtpClient.CreatePccrtpRequest(
                testConfig.ContentServerComputerName,
                testConfig.ContentServerHTTPListenPort,
                testConfig.NameOfFileWithMultipleBlocks);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Send PCCRTP request to trigger hash generation");

            PccrtpResponse pccrtpResponse = pccrtpClient.SendHttpRequest(
                Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp.HttpVersionType.HttpVersion11,
                pccrtpRequest,
                (int)testConfig.Timeout.TotalMilliseconds);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Wait until the hash is generated on content server");

            TestUtility.DoUntilSucceed(() => sutControlAdapter.IsHTTPHashExisted(testConfig.ContentServerComputerFQDNOrNetBiosName), testConfig.Timeout, testConfig.RetryInterval);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Modify to an incompatible version");

            pccrtpRequest.HttpHeader[PccrtpConsts.XP2PPeerDistHttpHeader] = "Version=1.0, MissingDataRequest=false";

            pccrtpResponse = pccrtpClient.SendHttpRequest(
                Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp.HttpVersionType.HttpVersion11,
                pccrtpRequest,
                (int)testConfig.Timeout.TotalMilliseconds);

            BaseTestSite.Assert.AreEqual(
                "peerdist",
                pccrtpResponse.HttpResponse.ContentEncoding,
                "The content server should return content information when MissingDataRequest is false");
        }

        #endregion

        #region ContentServer_PccrtpServer_MissingDataRequestInvalid

        [TestMethod]
        [TestCategory("ContentServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        public void ContentServer_PccrtpServer_MissingDataRequestInvalid()
        {
            CheckApplicability();

            PccrtpClient pccrtpClient = new PccrtpClient();
            PccrtpRequest pccrtpRequest = pccrtpClient.CreatePccrtpRequest(
                testConfig.ContentServerComputerName,
                testConfig.ContentServerHTTPListenPort,
                testConfig.NameOfFileWithMultipleBlocks);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Send PCCRTP request to trigger hash generation");

            PccrtpResponse pccrtpResponse = pccrtpClient.SendHttpRequest(
                Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp.HttpVersionType.HttpVersion11,
                pccrtpRequest,
                (int)testConfig.Timeout.TotalMilliseconds);

            var contentBeforeHashGeneration = pccrtpResponse.PayloadData;

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Wait until the hash is generated on content server");

            TestUtility.DoUntilSucceed(() => sutControlAdapter.IsHTTPHashExisted(testConfig.ContentServerComputerFQDNOrNetBiosName), testConfig.Timeout, testConfig.RetryInterval);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Modify to an incompatible version");

            pccrtpRequest.HttpHeader[PccrtpConsts.XP2PPeerDistHttpHeader] = "Version=1.0, MissingDataRequest=InvalidValue";

            pccrtpResponse = pccrtpClient.SendHttpRequest(
                Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp.HttpVersionType.HttpVersion11,
                pccrtpRequest,
                (int)testConfig.Timeout.TotalMilliseconds);

            var contentAfterHashGeneration = pccrtpResponse.PayloadData;

            BaseTestSite.Assert.AreNotEqual(
                "peerdist",
                pccrtpResponse.HttpResponse.ContentEncoding,
                "The content server should return file content when MissingDataRequest is set to invalid value");

            BaseTestSite.Assert.IsTrue(
               contentAfterHashGeneration.SequenceEqual(contentBeforeHashGeneration),
               "The content server should return file content when MissingDataRequest is set to invalid value");
        }

        #endregion

        #region ContentServer_PccrtpServer_ContentInformationVersionIncompatible

        [TestMethod]
        [TestCategory("ContentServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        public void ContentServer_PccrtpServer_ContentInformationVersionIncompatible()
        {
            CheckApplicability();

            PccrtpClient pccrtpClient = new PccrtpClient();
            PccrtpRequest pccrtpRequest = pccrtpClient.CreatePccrtpRequest(
                testConfig.ContentServerComputerName,
                testConfig.ContentServerHTTPListenPort,
                testConfig.NameOfFileWithMultipleBlocks,
                BranchCacheVersion.V2);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Send PCCRTP request to trigger hash generation");

            PccrtpResponse pccrtpResponse = pccrtpClient.SendHttpRequest(
                Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp.HttpVersionType.HttpVersion11,
                pccrtpRequest,
                (int)testConfig.Timeout.TotalMilliseconds);

            var contentBeforeHashGeneration = pccrtpResponse.PayloadData;

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Wait until the hash is generated on content server");

            TestUtility.DoUntilSucceed(() => sutControlAdapter.IsHTTPHashExisted(testConfig.ContentServerComputerFQDNOrNetBiosName), testConfig.Timeout, testConfig.RetryInterval);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Modify to an incompatible version");

            pccrtpRequest.HttpHeader[PccrtpConsts.XP2PPeerDistExHttpHeader] = "MinContentInformation=900.0, MaxContentInformation=999.0";

            pccrtpResponse = pccrtpClient.SendHttpRequest(
                Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp.HttpVersionType.HttpVersion11,
                pccrtpRequest,
                (int)testConfig.Timeout.TotalMilliseconds);

            var contentAfterHashGeneration = pccrtpResponse.PayloadData;

            BaseTestSite.Assert.AreNotEqual(
                "peerdist",
                pccrtpResponse.HttpResponse.ContentEncoding,
                "The content server should return file content when content information version is incompatible");

            BaseTestSite.Assert.IsTrue(
               contentAfterHashGeneration.SequenceEqual(contentBeforeHashGeneration),
               "The content server should return file content when content information version is incompatible");
        }

        #endregion
    }
}

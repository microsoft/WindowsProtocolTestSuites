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

namespace Microsoft.Protocols.TestSuites.BranchCache.TestSuite.HostedCacheServer
{
    [TestClass]
    public class PchcServerMessageHeaderVerification : BranchCacheTestClassBase
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

        #region HostedCacheServer_PchcServer_MessageHeader_VersionIncompatible

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PchcServer_MessageHeader_VersionIncompatible()
        {
            CheckApplicability();

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
                "Send message with incompatible version to hosted cache server");

            var initialOffer = pchcClient.CreateInitialOfferMessage(
                testConfig.ClientContentRetrievalListenPort,
                new byte[0]);

            initialOffer.MsgHeader.MajorVersion = 0xFE;
            initialOffer.MsgHeader.MinorVersion = 0xFE;

            bool passed = false;
            try
            {
                pchcClient.SendInitialOfferMessage(initialOffer);
            }
            catch
            {
                passed = true;
            }

            BaseTestSite.Assert.IsTrue(passed, "Hosted cache server should drop message with incompatible version");
        }

        #endregion

        #region HostedCacheServer_PchcServer_MessageHeader_TypeInvalid

        [TestMethod]
        [TestCategory("HostedCacheServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("BranchCacheV2")]
        [TestCategory("PCCRTP")]
        [TestCategory("SMB2")]
        public void HostedCacheServer_PchcServer_MessageHeader_TypeInvalid()
        {
            CheckApplicability();

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
                "Send message with invalid message type to hosted cache server");

            var initialOffer = pchcClient.CreateInitialOfferMessage(
                testConfig.ClientContentRetrievalListenPort,
                new byte[0]);

            initialOffer.MsgHeader.MsgType = (PCHC_MESSAGE_TYPE)0xFEFE;

            bool passed = false;
            try
            {
                pchcClient.SendInitialOfferMessage(initialOffer);
            }
            catch
            {
                passed = true;
            }

            BaseTestSite.Assert.IsTrue(passed, "Hosted cache server should drop message with invalid message type");
        }

        #endregion
    }
}

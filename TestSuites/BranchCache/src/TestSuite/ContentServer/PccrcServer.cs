// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp;
using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack.Enum;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.BranchCache.TestSuite.ContentServer
{
    [TestClass]
    public class PccrcServer : BranchCacheTestClassBase
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

        #region ContentServer_SmbServer_MultipleSegment

        [TestMethod]
        [TestCategory("ContentServer")]
        [TestCategory("BranchCacheV1")]
        [TestCategory("PCCRTP")]
        public void ContentServer_PccrcServer_MultipleSegment()
        {
            CheckApplicability();

            var content = contentInformationUtility.RetrieveContentData(testConfig.NameOfFileWithMultipleSegments);

            var contentInformation = contentInformationUtility.RetrieveContentInformation(BranchCacheVersion.V1, testConfig.NameOfFileWithMultipleSegments);

            contentInformationUtility.VerifyContentInformation(content, contentInformation, BranchCacheVersion.V1);
        }

        #endregion
    }
}

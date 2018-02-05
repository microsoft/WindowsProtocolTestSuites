// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Dfsc;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.DFSC.TestSuite
{
    [TestClass]
    public class RootReferralToDC : DFSCTestBase
    {

        #region Test Suite Initialization

        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        #endregion


        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Client sends a v4 Root referral request to DC and expects positive response.")]
        public void BVT_RootReferralV4ToDC()
        {
            ValidRootReferral(ReferralEntryType_Values.DFS_REFERRAL_V4);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends a v1 Root referral request to DC and expects positive response.")]
        public void RootReferralV1ToDC()
        {
            ValidRootReferral(ReferralEntryType_Values.DFS_REFERRAL_V1);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends a v2 Root referral request to DC and expects positive response.")]
        public void RootReferralV2EXToDC()
        {
            utility.CheckEXOverSMB();
            ValidRootReferral(ReferralEntryType_Values.DFS_REFERRAL_V2, true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends a v3 Root referral request to DC and expects positive response.")]
        public void RootReferralV3EXSiteToDC()
        {
            utility.CheckEXOverSMB();
            ValidRootReferral(ReferralEntryType_Values.DFS_REFERRAL_V3, true, true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Client sends an invalid v4 Root referral request to DC and expects negative response.")]
        public void InvalidRootReferralToDC()
        {
            uint status;
            string invalidRootPath = string.Format(@"\{0}\{1}", TestConfig.DomainFQDNName, DFSCTestUtility.Consts.InvalidComponent);
            utility.CheckEXOverSMB();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a v4 Root referral request to DC, the request path is invalid, and expects negative response.");
            utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V4, invalidRootPath, true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_NO_SUCH_FILE, status,
                "Expected Root Referral v4 to DC Response is STATUS_NO_SUCH_FILE, actual status is {0}", Smb2Status.GetStatusCode(status));
        }

        private void ValidRootReferral(ReferralEntryType_Values entryType, bool isEx = false, bool containSiteName = false)
        {
            uint status;
            string reqPath = TestConfig.ValidRootPathDomain;
            
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a v{0} Root referral request to DC and expects positive response", (ushort)entryType);
            DfscReferralResponsePacket respPacket = utility.SendAndReceiveDFSReferral(out status, client, entryType, reqPath, true, isEx, containSiteName);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Root Referral to DC Response should succeed, actual status is {0}", Smb2Status.GetStatusCode(status));
            string target = TestConfig.RootTargetDomain;
            utility.VerifyReferralResponse(ReferralResponseType.RootTarget, entryType, reqPath, target, respPacket);
        }
    }
}

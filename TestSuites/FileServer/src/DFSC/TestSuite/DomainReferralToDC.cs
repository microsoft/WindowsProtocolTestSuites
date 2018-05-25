// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Dfsc;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.DFSC.TestSuite
{
    [TestClass]
    public class DomainReferralToDC : DFSCTestBase
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
        [Description("Client sends a version 3 Domain referral request to DC and expects positive response.")]
        public void BVT_DomainReferralV3ToDC()
        {
            uint status;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a version 3 Domain referral request to DC.");
            DfscReferralResponsePacket respPacket = utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V3, "", true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Expected Domain Referral v3 to DC Response is STATUS_SUCCESS, but real status is" + Smb2Status.GetStatusCode(status));
            VerifyResponse(respPacket);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Client sends a v1 Domain referral request to DC and expects negative response.")]
        public void DomainReferralV1ToDC()
        {
            uint status;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a v1 Domain referral request to DC.");
            utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V1, "", true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_UNSUCCESSFUL, status,
                "Expected Domain Referral v1 to DC Response is STATUS_UNSUCCESSFUL, but real status is " + Smb2Status.GetStatusCode(status));
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Client send a v2 Domain referral request to DC and expects negative response.")]
        public void DomainReferralV2ToDC()
        {
            uint status;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client send a v2 Domain referral request to DC.");
            utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V2, "", true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_UNSUCCESSFUL, status,
                "Expected Domain Referral v2 to DC Response is STATUS_UNSUCCESSFUL, but real status is " + Smb2Status.GetStatusCode(status));
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends a v4 Domain referral request EX to DC and expects positive response.")]
        public void DomainReferralV4EXToDC()
        {
            utility.CheckEXOverSMB();
            uint status;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a v4 Domain referral request EX to DC.");
            DfscReferralResponsePacket respPacket = utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V4, "", true, true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Expected Domain Referral v4 ex to DC Response is STATUS_SUCCESS, but real status is " + Smb2Status.GetStatusCode(status));
            VerifyResponse(respPacket);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends a v4 Domain referral request EX to DC and expects positive response.")]
        public void DomainReferralV4EXSiteToDC()
        {
            utility.CheckEXOverSMB();
            uint status;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a v4 Domain referral request EX to DC.");
            DfscReferralResponsePacket respPacket = utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V4, "", true, true, true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Expected Domain Referral v4 ex to DC Response is STATUS_SUCCESS, but real status is" + Smb2Status.GetStatusCode(status));
            VerifyResponse(respPacket);
        }

        /// <summary>
        /// Verify response of domain referral request
        /// </summary>
        /// <param name="respPacket">Response packet of DC referral request</param>
        private void VerifyResponse(DfscReferralResponsePacket respPacket)
        {
            BaseTestSite.Assert.AreEqual(0, respPacket.ReferralResponse.PathConsumed, "PathConsumed must be set to 0 for Domain Referral");
            BaseTestSite.Assert.AreEqual(ReferralHeaderFlags.NONE, respPacket.ReferralResponse.ReferralHeaderFlags, "ReferralHeaderFlags must be set to 0 for Domain Referral");

            DFS_REFERRAL_V3V4_NameListReferral[] respV3 = client.RetrieveReferralEntries<DFS_REFERRAL_V3V4_NameListReferral>(respPacket, (ushort)ReferralEntryType_Values.DFS_REFERRAL_V3);
            uint timeToLive = respV3[0].TimeToLive;
            foreach (DFS_REFERRAL_V3V4_NameListReferral entry in respV3)
            {
                BaseTestSite.Assert.IsTrue(entry.SpecialName.IndexOf(TestConfig.DomainNetBIOSName, StringComparison.OrdinalIgnoreCase) >= 0,
                    "SpecialName must contain {0}, actual SpecialName is {1}", TestConfig.DomainNetBIOSName, entry.SpecialName);
                BaseTestSite.Assert.AreEqual(timeToLive, entry.TimeToLive, "TimeToLive must be the same");
                BaseTestSite.Assert.AreEqual((ushort)ReferralEntryType_Values.DFS_REFERRAL_V3, entry.VersionNumber, "VersionNumber must be set to " + ReferralEntryType_Values.DFS_REFERRAL_V3.ToString());
                BaseTestSite.Assert.AreEqual(0, entry.ExpandedNameOffset, "ExpandedNameOffSet must be set to 0");
                BaseTestSite.Assert.AreEqual(ReferralEntryFlags_Values.NameListReferral, entry.ReferralEntryFlags & ReferralEntryFlags_Values.NameListReferral, "NameListReferral MUST be set to 1 for Domain referral");
                BaseTestSite.Assert.AreEqual(0, entry.ServerType, "ServerType MUST be set to 0x0 for Domain referrals");
                timeToLive = entry.TimeToLive;
            }
        }
    }
}

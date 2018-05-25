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
    public class DCReferralToDC : DFSCTestBase
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
        [Description("Client sends a version 3 DC referral request with a valid domain name(FQDN) to DC and expects positive response.")]
        public void BVT_DCReferralV3ToDC()
        {
            uint status;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a version 3 DC referral request with a valid domain name(FQDN) to DC.");
            DfscReferralResponsePacket respPacket = utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V3, TestConfig.ValidFQDNPath, true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "DC Referral to DC Response has failed with status " + Smb2Status.GetStatusCode(status));
            VerifyResponse(respPacket, true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends a v4 DC referral request EX with a valid domain name(NETBIOS) to DC and expects positive response.")]
        public void DCReferralV4EXNetBiosToDC()
        {
            utility.CheckEXOverSMB();
            uint status;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a v4 DC referral request EX with a valid domain name(NETBIOS) to DC.");
            DfscReferralResponsePacket respPacket = utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V4, TestConfig.ValidNETBIOSPath,
                true, true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "DC Referral to DC Response has failed with status " + Smb2Status.GetStatusCode(status));
            VerifyResponse(respPacket, false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends a v4 DC referral request EX with a site name to DC and expects positive response.")]
        public void DCReferralV4EXSiteToDC()
        {
            utility.CheckEXOverSMB();
            uint status;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a v4 DC referral request EX with a site name to DC.");
            DfscReferralResponsePacket respPacket = utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V4, TestConfig.ValidFQDNPath,
                true, true, true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "DC Referral to DC Response has failed with status " + Smb2Status.GetStatusCode(status));
            VerifyResponse(respPacket, true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Client sends a v4 DC referral request with an invalid domain name(NETBIOS) to DC and expects negative response.")]
        public void InvalidDCReferralToDC()
        {
            uint status;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a v4 DC referral request with an invalid domain name(NETBIOS) to DC.");
            utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V4, "\\" + DFSCTestUtility.Consts.InvalidComponent, true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_INVALID_PARAMETER, status,
                "Expected Invalid DC Referral to DC Response is STATUS_INVALID_PARAMETER, but real status is " + Smb2Status.GetStatusCode(status));
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Client sends a v1 DC referral request to DC and expects negative response.")]
        public void DCReferralV1ToDC()
        {
            uint status;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a v1 DC referral request to DC.");
            utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V1, TestConfig.ValidFQDNPath, true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_UNSUCCESSFUL, status,
                "Expected DC Referral v1 to DC Response is STATUS_UNSUCCESSFUL, but real status is " + Smb2Status.GetStatusCode(status));
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Client sends a v2 DC referral request to DC and expects negative response.")]
        public void DCReferralV2ToDC()
        {
            uint status;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a v2 DC referral request to DC.");
            utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V2, TestConfig.ValidFQDNPath, true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_UNSUCCESSFUL, status,
                "Expected DC Referral v1 to DC Response is STATUS_UNSUCCESSFUL, but real status is " + Smb2Status.GetStatusCode(status));
        }

        /// <summary>
        /// Verify response of DC referral request
        /// </summary>
        /// <param name="respPacket">Response packet of DC referral request</param>
        /// <param name="fqdnOrNetbios">If the domain name is FQDN format or NetBIOS format</param>
        private void VerifyResponse(DfscReferralResponsePacket respPacket, bool fqdnOrNetbios)
        {
            BaseTestSite.Assert.AreEqual((ushort)0, respPacket.ReferralResponse.PathConsumed, "PathConsumed must be set to 0");
            BaseTestSite.Assert.AreEqual((ushort)0x0001, respPacket.ReferralResponse.NumberOfReferrals, "NumberOfReferrals must be set to 1");

            DFS_REFERRAL_V3V4_NameListReferral[] respV3 = client.RetrieveReferralEntries<DFS_REFERRAL_V3V4_NameListReferral>(respPacket, (ushort)ReferralEntryType_Values.DFS_REFERRAL_V3);
            uint timeToLive = respV3[0].TimeToLive;
            bool containValidDC = false;
            string expectedDCName;
            string expectedSpecialName;
            if (fqdnOrNetbios)
            {
                expectedDCName = string.Format(@"\{0}.{1}", TestConfig.DCServerName, TestConfig.DomainFQDNName);
                expectedSpecialName = TestConfig.ValidFQDNPath;
            }
            else
            {
                expectedDCName = @"\" + TestConfig.DCServerName;
                expectedSpecialName = TestConfig.ValidNETBIOSPath;
            }

            foreach (DFS_REFERRAL_V3V4_NameListReferral entry in respV3)
            {
                if (!containValidDC)
                {
                    foreach (string dcName in entry.DCNameArray)
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "DC name is {0}", dcName);

                        containValidDC = dcName.Equals(expectedDCName, StringComparison.OrdinalIgnoreCase);
                    }
                }
                BaseTestSite.Assert.AreEqual(expectedSpecialName, entry.SpecialName, @"SpecialName must be \" + expectedSpecialName);

                BaseTestSite.Assert.AreEqual(timeToLive, entry.TimeToLive, "TimeToLive must be the same");
                BaseTestSite.Assert.AreEqual((ushort)ReferralEntryType_Values.DFS_REFERRAL_V3, entry.VersionNumber, 
                    "VersionNumber must be set to " + ReferralEntryType_Values.DFS_REFERRAL_V3.ToString());
                BaseTestSite.Assert.AreEqual(ReferralEntryFlags_Values.NameListReferral, entry.ReferralEntryFlags, "NameListReferral MUST be set to 1 for DC referral");
                timeToLive = entry.TimeToLive;
            }

            BaseTestSite.Assert.IsTrue(containValidDC, "DCName must be " + expectedDCName);
        }
    }
}

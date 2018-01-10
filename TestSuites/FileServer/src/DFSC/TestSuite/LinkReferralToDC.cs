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
    public class LinkReferralToDC : DFSCTestBase
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

        /// Link Referral request is sent To DC
        /// If DC is not hosting DFS server, the link referral request should fail with STATUS_NOT_FOUND
        /// Otherwise DC will reply a valid link referral response.
        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Client sends a v4 Link referral request to DC and expects positive response or STATUS_NOT_FOUND depends on if the DC is hosting DFS server.")]
        public void LinkReferralV4ToDC()
        {
            ValidLinkReferral(ReferralEntryType_Values.DFS_REFERRAL_V4);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Client sends a v1 Link referral request to DC and expects positive response or STATUS_NOT_FOUND depends on if the DC is hosting DFS server.")]
        public void LinkReferralV1ToDC()
        {
            ValidLinkReferral(ReferralEntryType_Values.DFS_REFERRAL_V1);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Client sends a v2 Link referral request to DC and expects positive response or STATUS_NOT_FOUND depends on if the DC is hosting DFS server.")]
        public void LinkReferralV2EXToDC()
        {
            utility.CheckEXOverSMB();
            ValidLinkReferral(ReferralEntryType_Values.DFS_REFERRAL_V2, true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Client sends a v3 Link referral request to DC and expects positive response or STATUS_NOT_FOUND depends on if the DC is hosting DFS server.")]
        public void LinkReferralV3EXSiteToDC()
        {
            utility.CheckEXOverSMB();
            ValidLinkReferral(ReferralEntryType_Values.DFS_REFERRAL_V3, true, true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Client sends a v2 Link referral request with invalid Domain name to DC, and expects negative response.")]
        public void InvalidDomainNameLinkReferralToDC()
        {
            uint status;
            string invalidDomainLinkPath = string.Format(@"\{0}\{1}\{2}", DFSCTestUtility.Consts.InvalidComponent, TestConfig.DomainNamespace, TestConfig.DFSLink);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a v2 Link referral request with invalid Domain name to DC.");
            utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V2, invalidDomainLinkPath, true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_NOT_FOUND, status,
                "Expected Link Referral v2 to DC Response is STATUS_NOT_FOUND, actual status is {0}", Smb2Status.GetStatusCode(status));
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Client sends a v4 Link referral request to DC with invalid namespace, and expects negative response.")]
        public void InvalidNamespaceLinkReferralToDC()
        {
            uint status;
            string invalidNamespaceLinkPath = string.Format(@"\{0}\{1}\{2}", TestConfig.DomainFQDNName, DFSCTestUtility.Consts.InvalidComponent, TestConfig.DFSLink);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a v4 Link referral request to DC with invalid namespace.");
            utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V4, invalidNamespaceLinkPath, true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_NOT_FOUND, status,
                "Expected Link Referral v4 to DC Response is STATUS_NOT_FOUND, actual status is {0}", Smb2Status.GetStatusCode(status));
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Client sends a v3 Link referral request with invalid link name to DC, and expects negative response.")]
        public void InvalidLinkNameLinkReferralToDC()
        {
            uint status;
            string invalidLinkName = string.Format(@"\{0}\{1}\{2}", TestConfig.DomainFQDNName, TestConfig.DomainNamespace, DFSCTestUtility.Consts.InvalidComponent);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a v3 Link referral request with invalid link name to DC.");
            utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V3, invalidLinkName, true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_NOT_FOUND, status,
                "Expected Link Referral v3 to DC Response is STATUS_NOT_FOUND, actual status is {0}", Smb2Status.GetStatusCode(status));
        }

        /// <summary>
        /// Send valid link referral request and verify response.
        /// </summary>
        /// <param name="entryType"></param>
        /// <param name="isEx"></param>
        /// <param name="containSiteName"></param>
        private void ValidLinkReferral(ReferralEntryType_Values entryType, bool isEx = false, bool containSiteName = false)
        {
            uint status;
            string reqPath = TestConfig.ValidLinkPathDomain;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a {0} Link referral request to DC.", entryType.ToString());
            DfscReferralResponsePacket respPacket = utility.SendAndReceiveDFSReferral(out status, client, entryType, reqPath, true, isEx, containSiteName);
            bool DChostingDFSServer = TestConfig.DCServerName.Equals(TestConfig.DFSServerName, StringComparison.OrdinalIgnoreCase);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            if (DChostingDFSServer)
            {
                BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Link Referral to DC Response should succeed, actual status is {0}", Smb2Status.GetStatusCode(status));
                string target = TestConfig.LinkTarget;
                utility.VerifyReferralResponse(ReferralResponseType.LinkTarget, entryType, reqPath, target, respPacket);
            }
            else
            {
                // Section 3.3.5.5   Receiving a Root Referral Request or Link Referral Request
                // A DC MUST fail the link referral request with STATUS_NOT_FOUND, 
                // if it’s not the DFS root target for the DFS namespace specified in the link referral request.
                BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_NOT_FOUND, status, "Server should fail the referral request with STATUS_NOT_FOUND. "
                    + "Actual status is " + Smb2Status.GetStatusCode(status));
            }
        }
    }
}

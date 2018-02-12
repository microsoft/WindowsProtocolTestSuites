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
    public class SysvolReferralToDC : DFSCTestBase
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
        [Description("Client sends a v4 Sysvol referral request with SYSVOL directory to DC and expects positive response")]
        public void BVT_SysvolReferralv4ToDCSysvolPath()
        {
            ValidSysvolReferral(ReferralEntryType_Values.DFS_REFERRAL_V4, true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Client sends a version 3 sysvol referral request with NETLOGON directory to DC and expects positive response.")]
        public void BVT_SysvolReferralV3ToDCNetlogonPath()
        {
            ValidSysvolReferral(ReferralEntryType_Values.DFS_REFERRAL_V3, false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends a v1 Sysvol referral request(EX) with SYSVOL directory to DC and expects positive response.")]
        public void SysvolReferralV1EXSiteToDCSysvolPath()
        {
            utility.CheckEXOverSMB();
            ValidSysvolReferral(ReferralEntryType_Values.DFS_REFERRAL_V1, true, true, true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends a v1 Sysvol referral request with SYSVOL directory to DC and expects positive response.")]
        public void SysvolReferralV1ToDCSysvolPath()
        {
            ValidSysvolReferral(ReferralEntryType_Values.DFS_REFERRAL_V1, true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends a v2 Sysvol referral request(EX) with NETLOGON directory to DC and expects positive response.")]
        public void SysvolReferralV2EXToDCNetlogonPath()
        {
            utility.CheckEXOverSMB();
            ValidSysvolReferral(ReferralEntryType_Values.DFS_REFERRAL_V2, false, true, false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends a v2 Sysvol referral request with NETLOGON directory to DC and expects positive response.")]
        public void SysvolReferralV2ToDCNetlogonPath()
        {
            ValidSysvolReferral(ReferralEntryType_Values.DFS_REFERRAL_V2, false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Client sends a v1 Sysvol referral request with invalid Domain name to DC and expects negative response.")]
        public void InvalidSysvolReferralToDC()
        {
            uint status;
            string invalidFQDNSysvolPath = string.Format(@"\{0}\{1}", DFSCTestUtility.Consts.InvalidComponent, DfscConsts.SysvolShare);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a v1 Sysvol referral request with invalid Domain name to DC.");
            utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V1, invalidFQDNSysvolPath, true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_NOT_FOUND, status,
                "Server should fail the referral request with STATUS_NOT_FOUND. Actual status is " + Smb2Status.GetStatusCode(status));
        }

        /// <summary>
        /// Client sends sysvol referral request to DC, receives response and verifies response.
        /// </summary>
        /// <param name="entryType">Version of referral request</param>
        /// <param name="sysvolOrNetlogon">If the second component of referral request path is "SYSVOL" or "NETLOGON"</param>
        /// <param name="isEx">The request is REQ_GET_DFS_REFERRAL_EX or not</param>
        /// <param name="containSiteName">REQ_GET_DFS_REFERRAL_EX contains "SiteName" field or not</param>

        private void ValidSysvolReferral(ReferralEntryType_Values entryType, bool sysvolOrNetlogon, bool isEx = false, bool containSiteName = false)
        {
            uint status;
            string reqPath;
            string target;
            if (sysvolOrNetlogon)
            {
                reqPath = string.Format(@"\{0}\{1}", TestConfig.DomainFQDNName, DfscConsts.SysvolShare);
                target = string.Format(@"\{0}.{1}\{2}", TestConfig.DCServerName, TestConfig.DomainFQDNName, DfscConsts.SysvolShare);
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a v{0} Sysvol referral request with SYSVOL directory to DC.", (ushort)entryType);
            }
            else
            {
                reqPath = string.Format(@"\{0}\{1}", TestConfig.DomainFQDNName, DfscConsts.NetlogonShare);
                target = string.Format(@"\{0}.{1}\{2}", TestConfig.DCServerName, TestConfig.DomainFQDNName, DfscConsts.NetlogonShare);
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a v{0} Sysvol referral request with NETLOGON directory to DC.", (ushort)entryType);

            }
            DfscReferralResponsePacket respPacket = utility.SendAndReceiveDFSReferral(out status, client, entryType, reqPath, true, isEx, containSiteName);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            utility.VerifyReferralResponse(ReferralResponseType.SysvolReferralResponse, entryType, reqPath, target, respPacket);
        }
    }
}

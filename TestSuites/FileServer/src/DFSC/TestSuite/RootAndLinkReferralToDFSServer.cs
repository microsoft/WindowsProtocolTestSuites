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
    public class RootAndLinkReferralToDFSServer : DFSCTestBase
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
        [Description("Client sends a DFS root referral request v4 to DFS Server first, and then link referral request v4.")]
        public void BVT_RootAndLinkReferralStandaloneV4ToDFSServer()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a DFS root referral request v4 to DFS Server, the request path is stand-alone, and expects success.");
            ValidRootOrLinkReferralToDFSServer(ReferralEntryType_Values.DFS_REFERRAL_V4, false, true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a DFS link referral request v4 to DFS Server, the request path is stand-alone, and expects success.");
            ValidRootOrLinkReferralToDFSServer(ReferralEntryType_Values.DFS_REFERRAL_V4, false, false);
        }

        /// <summary>
        /// Client sends root referral(Domain-based namespace) to DFS server, DFS server returns root target.
        /// Client sends link referral to DFS server using the root target plus link path.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Client sends a DFS root referral request v4 to DFS Server first, and then link referral request v4.")]
        public void BVT_RootAndLinkReferralDomainV4ToDFSServer()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a DFS root referral request v4 to DFS Server, the request path is domain-based, and expects success.");
            ValidRootOrLinkReferralToDFSServer(ReferralEntryType_Values.DFS_REFERRAL_V4, true, true);

            string reqPath = string.Format(@"{0}\{1}", TestConfig.RootTargetDomain, TestConfig.DFSLink);
            uint status;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a DFS link referral request v4 to DFS Server, the request path is domain-based, and expects success.");
            DfscReferralResponsePacket respPacket = utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V4, reqPath, false);

            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Root Referral to DFS Server Response should succeed, actual status is {0}", Smb2Status.GetStatusCode(status));
            utility.VerifyReferralResponse(ReferralResponseType.LinkTarget, ReferralEntryType_Values.DFS_REFERRAL_V4,
                reqPath, TestConfig.LinkTarget, respPacket);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends a DFS root referral request v1 to DFS Server first, and then link referral request v1.")]
        public void RootAndLinkReferralEXStandaloneV1ToDFSServer()
        {
            utility.CheckEXOverSMB();
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a DFS root referral request v1 to DFS Server, the request path is stand-alone, and expects success.");
            ValidRootOrLinkReferralToDFSServer(ReferralEntryType_Values.DFS_REFERRAL_V1, false, true, true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a DFS link referral request v1 to DFS Server, the request path is stand-alone, and expects success.");
            ValidRootOrLinkReferralToDFSServer(ReferralEntryType_Values.DFS_REFERRAL_V1, false, false, true, true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends a DFS link referral request v1 to DFS Server first and expects positive response.")]
        public void LinkReferralDomainV1ToDFSServer()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a DFS link referral request v1 to DFS Server, the request path is domain-based, and expects success.");
            ValidRootOrLinkReferralToDFSServer(ReferralEntryType_Values.DFS_REFERRAL_V1, true, false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends a DFS root referral request v2 to DFS Server first, and then link referral request v2.")]
        public void RootAndLinkReferralStandaloneV2ToDFSServer()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a DFS root referral request v2 to DFS Server, the request path is stand-alone, and expects success.");            
            ValidRootOrLinkReferralToDFSServer(ReferralEntryType_Values.DFS_REFERRAL_V2, false, true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a DFS link referral request v2 to DFS Server, the request path is stand-alone, and expects success.");
            ValidRootOrLinkReferralToDFSServer(ReferralEntryType_Values.DFS_REFERRAL_V2, false, false);
        }

        /// <summary>
        /// Client sends link referral request(Stand-alone namespace) to DFS server,
        /// returned target is an interlink
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends a DFS link referral request v4 to DFS Server and expects interlink returned.")]
        public void LinkReferralStandaloneToDFSServerReturnInterlink()
        {
            uint status;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a DFS link referral request v4 to DFS Server, the request path is stand-alone, and expects interlink returned.");
            DfscReferralResponsePacket respPacket = utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V4, TestConfig.ValidInterlinkPathStandalone, false);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Root Referral to DFS Server Response should succeed, actual status is {0}", Smb2Status.GetStatusCode(status));
            utility.VerifyReferralResponse(ReferralResponseType.Interlink, ReferralEntryType_Values.DFS_REFERRAL_V4,
                TestConfig.ValidInterlinkPathStandalone, TestConfig.InterlinkTarget, respPacket);
        }

        /// <summary>
        /// Client sends link referral request(Domain-based namespace) to DFS server,
        /// returned target is an interlink
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends a DFS link referral request v4 to DFS Server and expects interlink returned.")]
        public void LinkReferralDomainToDFSServerReturnInterlink()
        {
            uint status;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a DFS link referral request v4 to DFS Server, the request path is domain-based, and expects interlink returned.");
            DfscReferralResponsePacket respPacket = utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V4, TestConfig.ValidInterlinkPathDomain, false);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Root Referral to DFS Server Response should succeed, actual status is {0}", Smb2Status.GetStatusCode(status));
            utility.VerifyReferralResponse(ReferralResponseType.Interlink, ReferralEntryType_Values.DFS_REFERRAL_V4,
                TestConfig.ValidInterlinkPathDomain, TestConfig.InterlinkTarget, respPacket);
        }

        /// <summary>
        /// Client sends link referral request(Stand-alone namespace) to DFS server,
        /// returned target is root target. Below is reason.
        /// If a DFS link that is a complete prefix of the DFS referral request path is identified, the server MUST return a DFS link referral response; 
        /// otherwise, if it has a match for the DFS root, it MUST return a root referral response.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends a DFS link referral request v3 to DFS Server and expects root target returned.")]
        public void LinkReferralStandaloneToDFSServerReturnRootTarget()
        {
            string invalidRootPathStandalone = string.Format(@"{0}\{1}", TestConfig.ValidRootPathStandalone, DFSCTestUtility.Consts.InvalidComponent);
            uint status;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a DFS link referral request v3 to DFS Server, the request path is stand-alone, and expects root target returned.");
            DfscReferralResponsePacket respPacket = utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V3, invalidRootPathStandalone, false);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Link Referral to DFS Server Response should succeed, actual status is {0}", Smb2Status.GetStatusCode(status));
            utility.VerifyReferralResponse(ReferralResponseType.RootTarget, ReferralEntryType_Values.DFS_REFERRAL_V3,
                TestConfig.ValidRootPathStandalone, TestConfig.RootTargetStandalone, respPacket);
        }

        /// <summary>
        /// Client sends invalid link referral to DFS Server
        /// DFS Server returns root target, not link target, not STATUS_NOT_FOUND either.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends a DFS link referral request v4 to DFS Server and expects root target returned.")]
        public void LinkReferralDomainToDFSServerReturnRootTarget()
        {
            string invalidRootPathStandalone = string.Format(@"{0}\{1}", TestConfig.ValidRootPathDomain, DFSCTestUtility.Consts.InvalidComponent);
            uint status;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a DFS link referral request v4 to DFS Server, the request path is domain-based, and expects root target returned.");
            DfscReferralResponsePacket respPacket = utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V4, invalidRootPathStandalone, false);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Link Referral to DFS Server Response should succeed, actual status is {0}", Smb2Status.GetStatusCode(status));
            utility.VerifyReferralResponse(ReferralResponseType.RootTarget, ReferralEntryType_Values.DFS_REFERRAL_V4, TestConfig.ValidRootPathDomain, TestConfig.RootTargetDomain, respPacket);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Client sends a v4 invalid Root referral request to DFS server and expects negative response.")]
        public void InvalidRootReferralStandaloneToDFSServer()
        {
            string invalidRootPathStandalone = string.Format(@"\{0}\{1}", TestConfig.DFSServerName, DFSCTestUtility.Consts.InvalidComponent);
            uint status;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a DFS root referral request v4 to DFS Server, the request path is stand-alone and invalid, expects negative response.");
            utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V4, invalidRootPathStandalone, false);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            if (TestConfig.IsWindowsPlatform)
            {
                BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_NOT_FOUND, status,
                    "Server SHOULD fail the referral request with STATUS_NOT_FOUND. Actual status is " + Smb2Status.GetStatusCode(status));
            }
            else
            {
                BaseTestSite.Assert.AreNotEqual(Smb2Status.STATUS_SUCCESS, status,
                    "Server SHOULD fail the referral request. Actual status is " + Smb2Status.GetStatusCode(status));
            }
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Client sends a v4 invalid Root referral request to DFS server and expects negative response.")]
        public void InvalidRootReferralDomainToDFSServer()
        {
            string invalidRootPathDomain = string.Format(@"\{0}\{1}", TestConfig.DomainFQDNName, DFSCTestUtility.Consts.InvalidComponent);
            uint status;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends a DFS root referral request v4 to DFS Server, the request path is domain-based and invalid, expects negative response.");
            utility.SendAndReceiveDFSReferral(out status, client, ReferralEntryType_Values.DFS_REFERRAL_V4, invalidRootPathDomain, false);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            // Section 6  
            // <22> Section 3.2.5.5: Windows NT Server 4.0, Windows 2000, Windows Server 2003, Windows Server 2003 R2, Windows Server 2008, and Windows Server 2008 R2 
            // fail the referral request with a STATUS_NOT_FOUND (0xC0000225) return code.
            if (TestConfig.Platform == Platform.WindowsServer2008 || TestConfig.Platform == Platform.WindowsServer2008R2)
            {
                BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_NOT_FOUND, status,
                    "Server SHOULD fail the referral request with STATUS_NOT_FOUND for 2008 and 2008R2. Actual status is: "
                    + Smb2Status.GetStatusCode(status));
            }
            else if (TestConfig.IsWindowsPlatform)
            {
                BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_DFS_UNAVAILABLE, status,
                    "Server SHOULD fail the referral request with STATUS_DFS_UNAVAILABLE. Actual status is: "
                    + Smb2Status.GetStatusCode(status));
            }
            else
            {
                BaseTestSite.Assert.AreNotEqual(Smb2Status.STATUS_SUCCESS, status,
                    "Server SHOULD fail the referral request. Actual status is " + Smb2Status.GetStatusCode(status));
            }
        }

        /// <summary>
        /// Client sends valid root or link referral request to DFS server, receives response and verifies response.
        /// </summary>
        /// <param name="entryType">Version of referral request</param>
        /// <param name="domainOrStandalone">The request path is domain-based or stand-alone</param>
        /// <param name="rootOrLink">The referral request is root referral or link referral</param>
        /// <param name="isEx">The request is REQ_GET_DFS_REFERRAL_EX or not</param>
        /// <param name="containSiteName">REQ_GET_DFS_REFERRAL_EX contains "SiteName" field or not</param>
        private void ValidRootOrLinkReferralToDFSServer(ReferralEntryType_Values entryType, bool domainOrStandalone, bool rootOrLink, bool isEx = false, bool containSiteName = false)
        {

            uint status;
            string reqPath;
            ReferralResponseType ReferralResponseType;

            string target;
            if (rootOrLink)
            {
                if (domainOrStandalone)
                {
                    reqPath = TestConfig.ValidRootPathDomain;
                    target = TestConfig.RootTargetDomain;
                }
                else
                {
                    reqPath = TestConfig.ValidRootPathStandalone;
                    target = TestConfig.RootTargetStandalone;
                }

                ReferralResponseType = ReferralResponseType.RootTarget;
            }
            else
            {
                if (domainOrStandalone)
                    reqPath = TestConfig.ValidLinkPathDomain;
                else
                    reqPath = TestConfig.ValidLinkPathStandalone;

                target = TestConfig.LinkTarget;
                ReferralResponseType = ReferralResponseType.LinkTarget;
            }

            DfscReferralResponsePacket respPacket = utility.SendAndReceiveDFSReferral(out status, client, entryType, reqPath, false, isEx, containSiteName);

            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Root Referral to DFS Server Response should succeed, actual status is {0}", Smb2Status.GetStatusCode(status));

            utility.VerifyReferralResponse(ReferralResponseType, entryType, reqPath, target, respPacket);
        }
    }
}

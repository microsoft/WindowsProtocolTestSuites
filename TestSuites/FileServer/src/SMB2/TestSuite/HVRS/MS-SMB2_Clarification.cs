// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public class MS_SMB2_Clarification : SMB2TestBase
    {

        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        protected override void TestInitialize()
        {
            base.TestInitialize();
            smb2Functionalclient = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.HvrsSmb)]
        [TestCategory(TestCategories.Smb30)]
        [Description("This test case is designed to test whether the server supports the SMB 3.0 or higher dialect.")]
        public void BVT_SMBDialect()
        {
            smb2Functionalclient.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.ShareServerName, TestConfig.ShareServerIP);
            DialectRevision[] negotiateDialects = Smb2Utility.GetDialects(DialectRevision.Smb30);

            BaseTestSite.Log.Add(TestTools.LogEntryKind.TestStep, "Send Negotiate request with maximum dialect: {0}.", DialectRevision.Smb30);

            smb2Functionalclient.Negotiate(
                Packet_Header_Flags_Values.NONE,
                negotiateDialects,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                    {
                        BaseTestSite.Log.Add(TestTools.LogEntryKind.TestStep, "Check dialect in negotiate response is equal to {0}.", DialectRevision.Smb30);
                        BaseTestSite.Assert.AreEqual(
                            Smb2Status.STATUS_SUCCESS,
                            header.Status,
                            "{0} should succeed, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                        BaseTestSite.Assert.AreEqual(DialectRevision.Smb30, response.DialectRevision, "Selected dialect should be {0}.", DialectRevision.Smb30);
                        
                    }
                );
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.HvrsSmb)]
        [TestCategory(TestCategories.Smb30)]
        [Description("This test case is designed to test whether the server supports persistent handles.")]
        public void BVT_PersistentHandles()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            #endregion 

            DialectRevision[] requestDialect = Smb2Utility.GetDialects(DialectRevision.Smb311);
            Capabilities_Values clientCapabilities = Capabilities_Values.GLOBAL_CAP_DFS
                                                    | Capabilities_Values.GLOBAL_CAP_LEASING
                                                    | Capabilities_Values.GLOBAL_CAP_LARGE_MTU
                                                    | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL
                                                    | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES
                                                    | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING
                                                    | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING
                                                    | Capabilities_Values.GLOBAL_CAP_ENCRYPTION;


            SecurityMode_Values clientSecuirtyMode = SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED;

            smb2Functionalclient.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.ShareServerName, TestConfig.ShareServerIP);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send Negotiate request with client capabilitites: {0}.", clientCapabilities.ToString());
            
            smb2Functionalclient.Negotiate(
                Packet_Header_Flags_Values.NONE,
                requestDialect,
                clientSecuirtyMode,
                clientCapabilities,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                    {

                        BaseTestSite.Log.Add(LogEntryKind.TestStep, "Check negotiate response contains {0} in Capabilities.", Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES);
                        BaseTestSite.Assert.AreEqual(
                            Smb2Status.STATUS_SUCCESS,
                            header.Status,
                            "{0} should succeed, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status));
                        BaseTestSite.Assert.IsTrue(response.DialectRevision >= DialectRevision.Smb30, "Select dialect is {0}, And it should be SMB 3.0 or higher dialect.", response.DialectRevision);
                        BaseTestSite.Assert.IsTrue(response.Capabilities.HasFlag(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES), "Server Capability should with flag {0} being set.", NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES);
                    }
                );

        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.HvrsSmb)]
        [TestCategory(TestCategories.Smb30)]
        [Description("This test case is designed to test whether the server supports the FSCTL_LMR_REQUEST_RESILIENCY.")]
        public void BVT_Resiliency()
        {
            #region Check Applicaility
            TestConfig.CheckDialect(DialectRevision.Smb30);
            #endregion 

            Guid clientGuid = Guid.NewGuid();
            FILEID fileId;
            uint treeId;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client opens a file.");
            //ConnectToShare(smb2Functionalclient, clientGuid, out treeId);

            OpenFile(smb2Functionalclient, clientGuid, out treeId, out fileId);

            Packet_Header ioCtlHeader;
            IOCTL_Response ioCtlReponse;
            byte[] inputInResponse;
            byte[] outputInResponse;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send an IOCTL FFSCTL_LMR_REQUEST_RESILLIENCY request.");
            smb2Functionalclient.ResiliencyRequest(
                treeId,
                fileId,
                TestConfig.MaxResiliencyTimeoutInSecond,
                (uint)Marshal.SizeOf(typeof(NETWORK_RESILIENCY_Request)),
                out ioCtlHeader,
                out ioCtlReponse,
                out inputInResponse,
                out outputInResponse,
                checker: (Packet_Header header, IOCTL_Response response) =>
                {
                    BaseTestSite.Log.Add(LogEntryKind.TestStep, "Check IOCTL FFSCTL_LMR_REQUEST_RESILLIENCY response");
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should succeed, actually server returns {1}", header.Command, Smb2Status.GetStatusCode(header.Status));
                });
            smb2Functionalclient.Close(treeId, fileId);
            smb2Functionalclient.TreeDisconnect(treeId);
            smb2Functionalclient.LogOff();
        }

        private void OpenFile(Smb2FunctionalClient smbClient, Guid clientGuid, out uint treeId, out FILEID fileId)
        {
            ConnectToShare(smbClient, clientGuid, out treeId);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Connect to share '{0}' on server '{1}'", TestConfig.ShareName, TestConfig.ShareServerName);

            string fileName = GetTestFileName(TestConfig.SharePath);
            Smb2CreateContextResponse[] createContextResponse;
            smbClient.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out createContextResponse);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Create Open with file name '{0}'", fileName);
        }

        private void ConnectToShare(Smb2FunctionalClient smbClient, Guid clientGuid, out uint treeId)
        {
            smbClient.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.ShareServerName, TestConfig.ShareServerIP);
            smbClient.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES,
                clientGuid: clientGuid,
                checker: (header, response) =>
                    {
                        BaseTestSite.Assert.AreEqual<NtStatus>(
                            NtStatus.STATUS_SUCCESS,
                            (NtStatus)header.Status,
                            "Negotiate should be successfully");
                        TestConfig.CheckNegotiateDialect(DialectRevision.Smb30, response);
                        TestConfig.CheckNegotiateCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES, response);
                    });

            smbClient.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.ShareServerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            smbClient.TreeConnect(
                TestConfig.SharePath,
                out treeId,
                checker: (header, response) =>
                    {
                        BaseTestSite.Assert.AreEqual<NtStatus>(
                            NtStatus.STATUS_SUCCESS,
                            (NtStatus)header.Status,
                            "TreeConnect should be successfully");
                    }

                    // TODO: Check IsCA
                );
        }
    }
}

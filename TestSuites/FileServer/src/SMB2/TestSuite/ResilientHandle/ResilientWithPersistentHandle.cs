// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite.ResilientHandle
{
    [TestClass]
    public class ResilientWithPersistentHandle : SMB2TestBase
    {
        public const LeaseStateValues LEASE_STATE = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING;

        #region Test Initialize and Cleanup
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
        #endregion

        #region Test Case Initialize and Clean up
        protected override void TestInitialize()
        {
            base.TestInitialize();
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        #region Test Case

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.CombinedFeature)]
        [TestCategory(TestCategories.Positive)]
        [Description("Verify that whether Open.IsResilient will impact persistent handle.")]
        public void ResilientWithPersistentHandle_OpenFromDiffClient()
        {
            /// 1. Open Persistent Handle with lease
            /// 2. Send Resiliency request
            /// 3. Disconnect
            /// 4. Open the same file from different client (different client guid)
            /// 5. The expected result of OPEN is STATUS_FILE_NOT_AVAILABLE according to section 3.3.5.9:
            /// If Connection.Dialect belongs to the SMB 3.x dialect family and the request does not contain SMB2_CREATE_DURABLE_HANDLE_RECONNECT 
            /// Create Context or SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 Create Context, the server MUST look up an existing open in the GlobalOpenTable 
            /// where Open.FileName matches the file name in the Buffer field of the request. 
            /// If an Open entry is found, and if all the following conditions are satisfied, the server MUST fail the request with STATUS_FILE_NOT_AVAILABLE.
            /// ¡ì   Open.IsPersistent is TRUE
            /// ¡ì	Open.Connection is NULL
            /// ¡ì	Open.OplockLevel is not equal to SMB2_OPLOCK_LEVEL_BATCH
            /// ¡ì	Open.OplockLevel is not equal to SMB2_OPLOCK_LEVEL_LEASE or Open.Lease.LeaseState does not include SMB2_LEASE_HANDLE_CACHING

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES);
            #endregion

            Smb2FunctionalClient client = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
            Guid clientGuid = Guid.NewGuid();
            Guid createGuid = Guid.NewGuid();
            Guid leaseKey = Guid.NewGuid();
            string fileName = GetTestFileName(Smb2Utility.GetUncPath(TestConfig.CAShareServerName, TestConfig.CAShareName));
            FILEID fileId;
            uint treeId;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Open Persistent Handle with lease.");
            OpenFile(
                client,
                clientGuid,
                fileName,
                true,
                out createGuid,
                out treeId,
                out fileId);

            // resiliency request
            Packet_Header ioCtlHeader;
            IOCTL_Response ioCtlResponse;
            byte[] inputInResponse;
            byte[] outputInResponse;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Send resiliency request with timeout {0} milliseconds", testConfig.MaxResiliencyTimeoutInSecond);            
            client.ResiliencyRequest(
                treeId,
                fileId,
                testConfig.MaxResiliencyTimeoutInSecond,
                (uint)Marshal.SizeOf(typeof(NETWORK_RESILIENCY_Request)),
                out ioCtlHeader,
                out ioCtlResponse,
                out inputInResponse,
                out outputInResponse
                );

            // Disconnect
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Disconnect the resilient handle");
            client.Disconnect();

            // open from another client
            Smb2FunctionalClient anotherClient = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site);
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Open the same file from different client (different client guid), the expected result of OPEN is successful");
            OpenFile(
                anotherClient,
                Guid.NewGuid(),
                fileName,
                false,
                out createGuid,
                out treeId,
                out fileId,
                responseChecker: (Packet_Header header, CREATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_FILE_NOT_AVAILABLE, header.Status, "The server MUST fail the request with STATUS_FILE_NOT_AVAILABLE.");
                });
        }

        #endregion

        #region Private Methods

        private void OpenFile(
            Smb2FunctionalClient client,
            Guid clientGuid,
            string fileName,
            bool isPersistentHandle,
            out Guid createGuid,
            out uint treeId,
            out FILEID fileId,
            ResponseChecker<CREATE_Response> responseChecker = null)
        {
            // connect to share
            ConnectToShare(
                client,
                clientGuid,
                out treeId);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Connect to share '{0}' on scaleout server '{1}'", testConfig.CAShareName, testConfig.CAShareServerName);

            #region Construct Create Context
            List<Smb2CreateContextRequest> createContextList = new List<Smb2CreateContextRequest>();
            createGuid = Guid.Empty;
            if (isPersistentHandle)
            {
                // durable handle request context
                createGuid = Guid.NewGuid();
                createContextList.Add(new Smb2CreateDurableHandleRequestV2()
                        {
                            CreateGuid = createGuid,
                            Flags = CREATE_DURABLE_HANDLE_REQUEST_V2_Flags.DHANDLE_FLAG_PERSISTENT,
                            Timeout = 0 // default
                        });
            }
            #endregion

            // open file
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Create Open with file name '{0}'", fileName);

            Smb2CreateContextResponse[] createContextResponses;
            client.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out createContextResponses,
                createContexts: createContextList.ToArray<Smb2CreateContextRequest>(),
                checker: responseChecker
            );

            #region check whether Persistent Handle is created successfully with current status
            if (isPersistentHandle)
            {
                BaseTestSite.Assert.IsTrue(
                    CheckDurableCreateContextResponse(createContextResponses),
                    "Create Response should contain Smb2CreateDurableHandleResponseV2.");
            }
            #endregion
        }

        private void ConnectToShare(
            Smb2FunctionalClient client,
            Guid clientGuid,
            out uint treeId)
        {
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.CAShareServerName, TestConfig.CAShareServerIP);

            // Negotiate
            client.Negotiate(
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
                }
                );

            // SMB2 SESSION SETUP
            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.CAShareServerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            // SMB2 Tree Connect
            client.TreeConnect(
                Smb2Utility.GetUncPath(TestConfig.CAShareServerName, TestConfig.CAShareName),
                out treeId,
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual<NtStatus>(
                        NtStatus.STATUS_SUCCESS,
                        (NtStatus)header.Status,
                        "TreeConnect should be successfully");

                    // Check IsCA
                    BaseTestSite.Assert.IsTrue(
                        response.Capabilities.HasFlag(Share_Capabilities_Values.SHARE_CAP_CONTINUOUS_AVAILABILITY),
                        "Share should support capabilities of SMB2_SHARE_CAP_CONTINUOUS_AVAILABILITY ");
                });
        }

        private bool CheckDurableCreateContextResponse(Smb2CreateContextResponse[] createContextResponses)
        {
            // check whether Persistent Handle is created successfully
            foreach (Smb2CreateContextResponse contextResponse in createContextResponses)
            {
                Smb2CreateDurableHandleResponseV2 durableHandleResponse = contextResponse as Smb2CreateDurableHandleResponseV2;
                if (durableHandleResponse != null)
                {
                    BaseTestSite.Assert.IsTrue(
                        durableHandleResponse.Flags.HasFlag(CREATE_DURABLE_HANDLE_RESPONSE_V2_Flags.DHANDLE_FLAG_PERSISTENT),
                        "Flags in Smb2CreateDurableHandleResponseV2 should be Persistent flag");
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}

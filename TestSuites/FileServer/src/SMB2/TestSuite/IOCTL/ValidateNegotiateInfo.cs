// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public class ValidateNegotiateInfo : SMB2TestBase
    {
        #region Variables
        private Smb2FunctionalClient client;
        #endregion

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
            client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
        }

        protected override void TestCleanup()
        {
            if (client != null)
            {
                try
                {
                    client.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Unexpected exception when disconnect client: {0}", ex.ToString());
                }
            }

            base.TestCleanup();
        }
        #endregion

        #region Test Case
        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.FsctlValidateNegotiateInfo)]
        [Description("Test whether server can handle IOCTL FSCTL_VALIDATE_NEGOTIATE_INFO.")]
        public void BVT_ValidateNegotiateInfo()
        {
            TestValidateNegotiateInfo(client, ValidateNegotiateInfoRequestType.None);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.FsctlValidateNegotiateInfo)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Test whether server terminates the transport connection and free the connection object " + 
            "if no dialect is matched when determine the greatest common dialect between the dialects it implements and the dialects array of VALIDATE_NEGOTIATE_INFO request.")]
        public void ValidateNegotiateInfo_Negative_InvalidDialects_NoCommonDialect()
        {
            TestValidateNegotiateInfo(client, ValidateNegotiateInfoRequestType.InvalidDialects, invalidDialects: new DialectRevision[] { DialectRevision.Smb2Unknown });
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.FsctlValidateNegotiateInfo)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Test whether the server terminates the transport connection and free the Connection object, " + 
            "if the value is not equal to Connection.Dialect when determine the greatest common dialect between the dialects it implements and the Dialects array of the VALIDATE_NEGOTIATE_INFO request.")]
        public void ValidateNegotiateInfo_Negative_InvalidDialects_CommonDialectNotExpected()
        {
            TestValidateNegotiateInfo(client, ValidateNegotiateInfoRequestType.InvalidDialects, invalidDialects: new DialectRevision[] { DialectRevision.Smb21 });
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.FsctlValidateNegotiateInfo)]
        [TestCategory(TestCategories.InvalidIdentifier)]
        [Description("Test whether the server terminates the transport connection and free the Connection object, " + 
            "if the Guid received in the VALIDATE_NEGOTIATE_INFO request structure is not equal to the Connection.ClientGuid.")]
        public void ValidateNegotiateInfo_Negative_InvalidGuid()
        {
            TestValidateNegotiateInfo(client, ValidateNegotiateInfoRequestType.InvalidGuid);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.FsctlValidateNegotiateInfo)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Test whether the server terminates the transport connection and free the Connection object, " + 
            "if the SecurityMode received in the VALIDATE_NEGOTIATE_INFO request structure is not equal to Connection.ClientSecurityMode.")]
        public void ValidateNegotiateInfo_Negative_InvalidSecurityMode()
        {
            TestValidateNegotiateInfo(client, ValidateNegotiateInfoRequestType.InvalidSecurityMode);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.FsctlValidateNegotiateInfo)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Test whether the server terminates the transport connection and free the Connection object, " + 
            "if Connection.ClientCapabilities is not equal to the Capabilities received in the VALIDATE_NEGOTIATE_INFO request structure.")]
        public void ValidateNegotiateInfo_Negative_InvalidCapabilities()
        {
            TestValidateNegotiateInfo(client, ValidateNegotiateInfoRequestType.InvalidCapabilities);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.FsctlValidateNegotiateInfo)]
        [TestCategory(TestCategories.OutOfBoundary)]
        [Description("Test whether the server terminates the transport connection and free the Connection object, " + 
            "if MaxOutputResponse in the IOCTL request is less than the size of a VALIDATE_NEGOTIATE_INFO Response.")]
        public void ValidateNegotiateInfo_Negative_InvalidMaxOutputResponse()
        {
            TestValidateNegotiateInfo(client, ValidateNegotiateInfoRequestType.InvalidMaxOutputResponse);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.FsctlValidateNegotiateInfo)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Test whether the server can terminate the transport connection when receiving a VALIDATE_NEGOTIATE_INFO request with dialect 3.1.1.")]
        public void ValidateNegotiateInfo_Negative_SMB311()
        {
            // This case is only applicable for the server that implements the SMB 3.1.1 dialect;
            TestConfig.CheckDialect(DialectRevision.Smb311);
            TestValidateNegotiateInfo(client, ValidateNegotiateInfoRequestType.InvalidSMB311Dialect);
        }
        #endregion

        #region Common Methods
        private void TestValidateNegotiateInfo(Smb2FunctionalClient client, ValidateNegotiateInfoRequestType requestType, DialectRevision[] invalidDialects = null)
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckIOCTL(CtlCode_Values.FSCTL_VALIDATE_NEGOTIATE_INFO);
            // Server will terminate connection if Validate Negotiate Info Request is not signed.
            TestConfig.CheckSigning();
            #endregion

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a client by sending the following requests: NEGOTIATE;  SESSION_SETUP; TREE_CONNECT");

            DialectRevision[] requestDialects;
            if (requestType == ValidateNegotiateInfoRequestType.InvalidSMB311Dialect)
            {
                requestDialects = Smb2Utility.GetDialects(DialectRevision.Smb311);
            }
            else
            {
                // FSCTL_VALIDATE_NEGOTIATE_INFO is only supported in SMB30 and SMB302. For SMB311 and later, we use SMB30 to test the server behavior.
                requestDialects = (TestConfig.MaxSmbVersionClientSupported > DialectRevision.Smb302) ? Smb2Utility.GetDialects(DialectRevision.Smb30) : TestConfig.RequestDialects;
            }
            
            Guid clientGuid = Guid.NewGuid();
            Capabilities_Values clientCapabilities = Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES | Capabilities_Values.GLOBAL_CAP_ENCRYPTION;
            SecurityMode_Values clientSecurityMode = SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED;
            NEGOTIATE_Response? negotiateResponse = null;
            client.Negotiate(
                requestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                clientSecurityMode,
                clientCapabilities,
                clientGuid,
                (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "Negotiation should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));

                    TestConfig.CheckNegotiateDialect(DialectRevision.Smb30, response);

                    negotiateResponse = response;
                });


            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            uint treeId;
            string ipcPath = Smb2Utility.GetIPCPath(TestConfig.SutComputerName);
            client.TreeConnect(ipcPath, out treeId);

            VALIDATE_NEGOTIATE_INFO_Request validateNegotiateInfoReq;
            switch (requestType)
            {
                case ValidateNegotiateInfoRequestType.None:
                case ValidateNegotiateInfoRequestType.InvalidSMB311Dialect:
                case ValidateNegotiateInfoRequestType.InvalidMaxOutputResponse:
                    validateNegotiateInfoReq.Guid = clientGuid;
                    validateNegotiateInfoReq.Capabilities = clientCapabilities;
                    validateNegotiateInfoReq.SecurityMode = clientSecurityMode;
                    validateNegotiateInfoReq.DialectCount = (ushort)requestDialects.Length;
                    validateNegotiateInfoReq.Dialects = requestDialects;
                    break;

                case ValidateNegotiateInfoRequestType.InvalidDialects:
                    validateNegotiateInfoReq.Guid = clientGuid;
                    validateNegotiateInfoReq.Capabilities = clientCapabilities;
                    validateNegotiateInfoReq.SecurityMode = clientSecurityMode;
                    validateNegotiateInfoReq.DialectCount = (ushort)invalidDialects.Length;
                    validateNegotiateInfoReq.Dialects = invalidDialects;
                    break;

                case ValidateNegotiateInfoRequestType.InvalidGuid:
                    validateNegotiateInfoReq.Guid = Guid.NewGuid();
                    validateNegotiateInfoReq.Capabilities = clientCapabilities;
                    validateNegotiateInfoReq.SecurityMode = clientSecurityMode;
                    validateNegotiateInfoReq.DialectCount = (ushort)requestDialects.Length;
                    validateNegotiateInfoReq.Dialects = requestDialects;
                    break;

                case ValidateNegotiateInfoRequestType.InvalidSecurityMode:
                    validateNegotiateInfoReq.Guid = clientGuid;
                    validateNegotiateInfoReq.Capabilities = clientCapabilities;
                    validateNegotiateInfoReq.SecurityMode = SecurityMode_Values.NONE;
                    validateNegotiateInfoReq.DialectCount = (ushort)requestDialects.Length;
                    validateNegotiateInfoReq.Dialects = requestDialects;
                    break;

                case ValidateNegotiateInfoRequestType.InvalidCapabilities:
                    validateNegotiateInfoReq.Guid = clientGuid;
                    validateNegotiateInfoReq.Capabilities = Capabilities_Values.NONE;
                    validateNegotiateInfoReq.SecurityMode = clientSecurityMode;
                    validateNegotiateInfoReq.DialectCount = (ushort)requestDialects.Length;
                    validateNegotiateInfoReq.Dialects = requestDialects;
                    break;

                default:
                    throw new InvalidOperationException("Unexpected ValidateNegotiateInfo request type " + requestType);
            }

            byte[] inputBuffer = TypeMarshal.ToBytes<VALIDATE_NEGOTIATE_INFO_Request>(validateNegotiateInfoReq);
            byte[] outputBuffer;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Attempt to validate negotiate info with info Guid: {0}, Capabilities: {1}, SecurityMode: {2}, DialectCount: {3}, Dialects: {4}",
                validateNegotiateInfoReq.Guid, validateNegotiateInfoReq.Capabilities, validateNegotiateInfoReq.SecurityMode, validateNegotiateInfoReq.DialectCount, Smb2Utility.GetArrayString(validateNegotiateInfoReq.Dialects));

            if (requestType == ValidateNegotiateInfoRequestType.None)
            {
                uint status = client.ValidateNegotiateInfo(treeId, inputBuffer, out outputBuffer, checker: (header, response) => { });

                BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status,
                    "ValidateNegotiateInfo should succeed ");

                VALIDATE_NEGOTIATE_INFO_Response validateNegotiateInfoResp = TypeMarshal.ToStruct<VALIDATE_NEGOTIATE_INFO_Response>(outputBuffer);
                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Capabilities returned in ValidateNegotiateInfo response: {0}", validateNegotiateInfoResp.Capabilities);
                BaseTestSite.Assert.AreEqual(
                    (Capabilities_Values)negotiateResponse.Value.Capabilities,
                    validateNegotiateInfoResp.Capabilities,
                    "Capabilities returned in ValidateNegotiateInfo response should be equal to server capabilities in original Negotiate response");

                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Guid returned in ValidateNegotiateInfo response: {0}", validateNegotiateInfoResp.Guid);
                BaseTestSite.Assert.AreEqual(
                    negotiateResponse.Value.ServerGuid,
                    validateNegotiateInfoResp.Guid,
                    "ServerGuid returned in ValidateNegotiateInfo response should be equal to server ServerGuid in original Negotiate response");

                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "SecurityMode returned in ValidateNegotiateInfo response: {0}", validateNegotiateInfoResp.SecurityMode);
                BaseTestSite.Assert.AreEqual(
                    (SecurityMode_Values)negotiateResponse.Value.SecurityMode,
                    validateNegotiateInfoResp.SecurityMode,
                    "SecurityMode returned in ValidateNegotiateInfo response should be equal to server SecurityMode in original Negotiate response");

                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Dialect returned in ValidateNegotiateInfo response: {0}", validateNegotiateInfoResp.Dialect);
                BaseTestSite.Assert.AreEqual(
                    negotiateResponse.Value.DialectRevision,
                    validateNegotiateInfoResp.Dialect,
                    "DialectRevision returned in ValidateNegotiateInfo response should be equal to server DialectRevision in original Negotiate response");
            }
            else
            {
                string errCondition;
                switch (requestType)
                {
                    case ValidateNegotiateInfoRequestType.InvalidMaxOutputResponse:
                        errCondition = "MaxOutputResponse in the request is less than the size of a VALIDATE_NEGOTIATE_INFO Response";
                        break;

                    case ValidateNegotiateInfoRequestType.InvalidSMB311Dialect:
                        errCondition = "Connection.Dialect is \"3.1.1\"";
                        break;

                    case ValidateNegotiateInfoRequestType.InvalidDialects:
                        errCondition = "there is no greatest common dialect between Dialects field in VALIDATE_NEGOTIATE_INFO Request and the server implemented dialect, or the greatest common dialect is not equal to Connection.Dialect";
                        break;

                    case ValidateNegotiateInfoRequestType.InvalidGuid:
                        errCondition = "Guid field in VALIDATE_NEGOTIATE_INFO request is not equal to the ClientGuid sent in the original SMB2 NEGOTIATE request";
                        break;

                    case ValidateNegotiateInfoRequestType.InvalidSecurityMode:
                        errCondition = "SecurityMode field in VALIDATE_NEGOTIATE_INFO request is not equal to the SecurityMode sent in the original SMB2 NEGOTIATE request";
                        break;

                    case ValidateNegotiateInfoRequestType.InvalidCapabilities:
                        errCondition = "Capabilities field in VALIDATE_NEGOTIATE_INFO request is not equal to the Capabilities sent in the original SMB2 NEGOTIATE request";
                        break;

                    default:
                        throw new InvalidOperationException("Unexpected ValidateNegotiateInfo request type " + requestType);
                }

                try
                {
                    uint maxOutputResponse = (requestType == ValidateNegotiateInfoRequestType.InvalidMaxOutputResponse) ? (uint)0 : Smb2FunctionalClient.DefaultMaxOutputResponse;
                    client.ValidateNegotiateInfo(treeId, inputBuffer, out outputBuffer, maxOutputResponse, checker: (header, response) => { });
                    client.TreeDisconnect(treeId);
                    client.LogOff();
                }
                catch
                {
                }

                BaseTestSite.Assert.IsTrue(client.Smb2Client.IsServerDisconnected, "Transport connection should be terminated when {0}", errCondition);
            }
        }
        #endregion
    }
}

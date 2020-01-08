// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Encryption
{
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class EncryptionAdapter : ModelManagedAdapterBase, IEncryptionAdapter
    {
        #region Fields

        private Smb2FunctionalClient testClient;
        private EncryptionConfig encryptionConfig;
        private uint treeId;
        private string uncSharePath;
        #endregion

        #region Initialization

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
        }
        public override void Reset()
        {
            base.Reset();

            if (testClient != null)
            {
                testClient.Disconnect();
                testClient = null;
            }
        }

        #endregion

        #region Events
        public event SessionSetupResponseEventHandler SessionSetupResponse;
        public event TreeConnectResponseEventHandler TreeConnectResponse;
        public event FileOperationVerifyEncryptionResponseEventHandler FileOperationVerifyEncryptionResponse;
        public event DisconnectionEventHandler ExpectDisconnect;
        #endregion

        #region Actions
        public void ReadConfig(out EncryptionConfig c)
        {
            if (!testConfig.IsEncryptionSupported)
            {
                Site.Assert.Inconclusive("This test case is not applicable due to Encryption is not supported");
            }

            c = new EncryptionConfig
            {
                MaxSmbVersionSupported = ModelUtility.GetModelDialectRevision(testConfig.MaxSmbVersionSupported, false),
                IsGlobalEncryptDataEnabled = testConfig.IsGlobalEncryptDataEnabled,
                IsGlobalRejectUnencryptedAccessEnabled = testConfig.IsGlobalRejectUnencryptedAccessEnabled,
                Platform = testConfig.Platform >= Platform.WindowsServer2016 ? Platform.WindowsServer2016 : testConfig.Platform
            };

            encryptionConfig = c;
            Site.Log.Add(LogEntryKind.Debug, encryptionConfig.ToString());
        }

        public void SetupConnection(ModelDialectRevision maxSmbVersionClientSupported, ClientSupportsEncryptionType clientSupportsEncryptionType)
        {
            // Set checkEncrypt to false to not check if the response from the server is actually encrypted.
            testClient = new Smb2FunctionalClient(testConfig.Timeout, testConfig, this.Site, checkEncrypt : false);
            testClient.ConnectToServer(testConfig.UnderlyingTransport, testConfig.SutComputerName, testConfig.SutIPAddress);
            testClient.Smb2Client.Disconnected += new Action(OnServerDisconnected);

            DialectRevision[] dialects = Smb2Utility.GetDialects(ModelUtility.GetDialectRevision(maxSmbVersionClientSupported));

            //Set capabilities according to isClientSupportsEncryption
            Capabilities_Values commonCapability = Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES;
            Capabilities_Values encryptionCapability = (clientSupportsEncryptionType == ClientSupportsEncryptionType.ClientSupportsEncryption) ? (commonCapability | Capabilities_Values.GLOBAL_CAP_ENCRYPTION) : commonCapability;

            uint status;
            DialectRevision selectedDialect;
            NEGOTIATE_Response? negotiateResponse = null;
            status = testClient.Negotiate(
                dialects,
                testConfig.IsSMB1NegotiateEnabled,
                capabilityValue: encryptionCapability,
                checker: (header, response) =>
                {
                    Site.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "{0} should succeed", header.Command);

                    negotiateResponse = response;
                },
                ifHandleRejectUnencryptedAccessSeparately: true,
                ifAddGLOBAL_CAP_ENCRYPTION: false,
                addDefaultEncryption: true
            );

            selectedDialect = negotiateResponse.Value.DialectRevision;

            if ((selectedDialect == DialectRevision.Smb30 || selectedDialect == DialectRevision.Smb302) && clientSupportsEncryptionType == ClientSupportsEncryptionType.ClientSupportsEncryption)
            {
                /// TD section 3.3.5.4
                /// SMB2_GLOBAL_CAP_ENCRYPTION if Connection.Dialect is "3.0" or "3.0.2", the server supports encryption, 
                /// and SMB2_GLOBAL_CAP_ENCRYPTION is set in the Capabilities field of the request
                Site.Assert.IsTrue(
                            negotiateResponse.Value.Capabilities.HasFlag(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_ENCRYPTION),
                            "SMB2_GLOBAL_CAP_ENCRYPTION should be set in the negotiate response.");
            }
            else
            {
                Site.Assert.IsFalse(
                            negotiateResponse.Value.Capabilities.HasFlag(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_ENCRYPTION),
                            "SMB2_GLOBAL_CAP_ENCRYPTION should not be set in the negotiate response.");
            }
        }

        public void SessionSetupRequest()
        {
            uint status = 0;
            SESSION_SETUP_Response? sessionSetupResponse = null;
            status = testClient.SessionSetup(
                testConfig.DefaultSecurityPackage,
                testConfig.SutComputerName,
                testConfig.AccountCredential,
                testConfig.UseServerGssToken,
                checker: (header, response) =>
                {
                    sessionSetupResponse = response;
                });

            SessionEncryptDataType sessionEncryptDataType = sessionSetupResponse.Value.SessionFlags.HasFlag(SessionFlags_Values.SESSION_FLAG_ENCRYPT_DATA) ? SessionEncryptDataType.SessionEncryptDataSet : SessionEncryptDataType.SessionEncryptDataNotSet;

            SessionSetupResponse((ModelSmb2Status)status, sessionEncryptDataType, encryptionConfig);
        }

        public void TreeConnectRequest(ConnectToShareType connectToShareType, ModelRequestType modelRequestType)
        {
            uncSharePath = (connectToShareType == ConnectToShareType.ConnectToEncryptedShare) ?
                Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.EncryptedFileShare) : Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare);

            if (modelRequestType == ModelRequestType.EncryptedRequest)
            {
                testClient.EnableSessionSigningAndEncryption(enableSigning: false, enableEncryption: true);
            }
            else
            {
                testClient.EnableSessionSigningAndEncryption(enableSigning: testConfig.SendSignedRequest, enableEncryption: false);
            }

            try
            {
                uint status = 0;
                TREE_CONNECT_Response? treeConnectResponse = null;
                status = testClient.TreeConnect(
                    uncSharePath,
                    out treeId,
                    checker: (header, response) =>
                    {
                        treeConnectResponse = response;
                    });

                ShareEncryptDataType shareEncryptDataType = treeConnectResponse.Value.ShareFlags.HasFlag(ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA) ? ShareEncryptDataType.ShareEncryptDataSet : ShareEncryptDataType.ShareEncryptDataNotSet;

                //TODO: To be implemented after TRANSFORM_HEADER added into Smb2FunctionalClient
                ModelResponseType modelResponseType = (modelRequestType == ModelRequestType.EncryptedRequest) ? ModelResponseType.EncryptedResponse : ModelResponseType.UnEncryptedResponse;
                TreeConnectResponse((ModelSmb2Status)status, shareEncryptDataType, modelResponseType, encryptionConfig);
            }
            catch
            {
            }
        }

        public void FileOperationVerifyEncryptionRequest(ModelRequestType modelRequestType)
        {
            uint status = 0;

            if (modelRequestType == ModelRequestType.UnEncryptedRequest)
            {
                testClient.EnableSessionSigningAndEncryption(enableSigning: testConfig.SendSignedRequest, enableEncryption: false);
            }

            bool isRequestEncrypted = (modelRequestType == ModelRequestType.EncryptedRequest) ? true : false;
            testClient.SetTreeEncryption(treeId, isRequestEncrypted);

            try
            {
                FILEID fileId;
                Smb2CreateContextResponse[] serverCreateContexts;
                // Skip the verification of signature when sending a non-encrypted CREATE request to an encrypted share
                testClient.Smb2Client.DisableVerifySignature = true;
                status = testClient.Create(
                    treeId,
                    GetTestFileName(uncSharePath),
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                    out fileId,
                    out serverCreateContexts,
                    checker: (header, response) => { });

                //TODO: To be implemented after TRANSFORM_HEADER added into Smb2FunctionalClient
                ModelResponseType modelResponseType = (modelRequestType == ModelRequestType.EncryptedRequest) ? ModelResponseType.EncryptedResponse : ModelResponseType.UnEncryptedResponse;
                FileOperationVerifyEncryptionResponse((ModelSmb2Status)status, modelResponseType, encryptionConfig);
            }
            catch
            {
            }
        }
        #endregion

        #region Public Methods
        public void OnServerDisconnected()
        {
            testClient = null;
            ExpectDisconnect();
        }

        #endregion
    }
}

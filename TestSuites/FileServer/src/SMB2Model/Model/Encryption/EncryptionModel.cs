// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Text;
using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Encryption;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Xrt.Runtime;

[assembly: NativeType("System.Diagnostics.Tracing.*")]
namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model.Encryption
{
    public static class EncryptionModel
    {
        #region State
        /// <summary>
        /// The dialect after negotiation
        /// </summary>
        public static DialectRevision negotiateDialect;

        /// <summary>
        /// Server model state
        /// </summary>
        private static ModelState state = ModelState.Uninitialized;

        /// <summary>
        /// Server configuration related to model
        /// </summary>
        public static EncryptionConfig config;

        /// <summary>
        /// Request that server model is handling
        /// </summary>
        public static ModelSMB2Request request;

        /// <summary>
        /// Indicates the server ADM: Connection_ClientCapabilities_SMB2_GLOBAL_CAP_ENCRYPTION
        /// This ADM is initialized as 0, and is set according to ClientCapabilities in the received negotiate request
        /// This ADM state changes will affect the session setup
        /// </summary>
        public static bool Connection_ClientCapabilities_SMB2_GLOBAL_CAP_ENCRYPTION = false;

        /// <summary>
        /// The capabilities sent by the server in the SMB2 NEGOTIATE Response on this connection
        /// </summary>
        public static bool Connection_ServerCapabilities_SMB2_GLOBAL_CAP_ENCRYPTION = false;

        /// <summary>
        /// The EncryptionTreeId is abstract to contain three value: NULL, EncryptTreeId, UnEncryptTreeId
        /// NULL: This flag indicates that the tree connect operation failed or not triggered
        /// EncryptTreeId: This flag indicates that current tree connect to encrypted share
        /// UnEncryptTreeId: This flag indicates that current tree connect to unencrypted share
        /// </summary>
        public static EncryptionTreeId Encryption_TreeId = EncryptionTreeId.NoTreeId;

        /// <summary>
        /// Indicates Session.EncryptData
        /// </summary>
        public static SessionEncryptDataType Session_EncryptData = SessionEncryptDataType.SessionEncryptDataNotSet;

        /// <summary>
        /// Indicate the session exists or not
        /// </summary>
        public static bool Session_IsExisted = false;

        #endregion

        #region Actions
        /// <summary>
        /// Call for loading server configuration
        /// </summary>
        [Rule(Action = "call ReadConfig(out _)")]
        public static void ReadConfigCall()
        {
            Condition.IsTrue(state == ModelState.Uninitialized);
        }

        /// <summary>
        /// Return for loading server configuration
        /// </summary>
        /// <param name="c">Server configuration related to model</param>
        [Rule(Action = "return ReadConfig(out c)")]
        public static void ReadConfigReturn(EncryptionConfig c)
        {
            Condition.IsTrue(state == ModelState.Uninitialized);
            Condition.IsNotNull(c);

            negotiateDialect = DialectRevision.Smb2Unknown;
            // Force SE to expand Config.MaxSmbVersionServerSupported
            Condition.IsTrue(c.MaxSmbVersionSupported == ModelDialectRevision.Smb2002 ||
                             c.MaxSmbVersionSupported == ModelDialectRevision.Smb21 ||
                             c.MaxSmbVersionSupported == ModelDialectRevision.Smb30 ||
                             c.MaxSmbVersionSupported == ModelDialectRevision.Smb302 ||
                             c.MaxSmbVersionSupported == ModelDialectRevision.Smb311);
            config = c;

            request = null;
            state = ModelState.Initialized;
        }

        /// <summary>
        /// Include setup connection and negotiation
        /// <param name="maxSmbVersionClientSupported">Indicates the max dialect revision client supports</param>
        /// <param name="clientSupportsEncryptionType">Indicates if client supports encryption or not</param>
        /// </summary>
        [Rule]
        public static void SetupConnection(ModelDialectRevision maxSmbVersionClientSupported, ClientSupportsEncryptionType clientSupportsEncryptionType)
        {
            Condition.IsTrue(state == ModelState.Initialized);
            Condition.IsNull(request);

            negotiateDialect = ModelHelper.DetermineNegotiateDialect(maxSmbVersionClientSupported, config.MaxSmbVersionSupported);

            Connection_ClientCapabilities_SMB2_GLOBAL_CAP_ENCRYPTION = (clientSupportsEncryptionType == ClientSupportsEncryptionType.ClientSupportsEncryption) ? true : false;

            if (ModelUtility.IsSmb3xFamily(negotiateDialect) && clientSupportsEncryptionType == ClientSupportsEncryptionType.ClientSupportsEncryption)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.4: The Capabilities field MUST be set to a combination of zero or more of the following bit values, as specified in section 2.2.4:");
                ModelHelper.Log(LogType.Requirement,
                    "\tSMB2_GLOBAL_CAP_ENCRYPTION if Connection.Dialect belongs to the SMB 3.x dialect family, the server supports encryption, " +
                    "and SMB2_GLOBAL_CAP_ENCRYPTION is set in the Capabilities field of the request.");

                // Encrpytion Model only applies to server that supports encryption.
                ModelHelper.Log(LogType.TestInfo,
                    "Connection.Dialect is {0}, the server supports encryption and SMB2_GLOBAL_CAP_ENCRYPTION is set. " +
                    "So SMB2_GLOBAL_CAP_ENCRYPTION bit is set in Capabilities field.", negotiateDialect);
                Connection_ServerCapabilities_SMB2_GLOBAL_CAP_ENCRYPTION = true;
            }

            state = ModelState.Connected;
        }

        /// <summary>
        /// Session setup request
        /// </summary>
        [Rule]
        public static void SessionSetupRequest()
        {
            Condition.IsTrue(state == ModelState.Connected);
            Condition.IsNull(request);
        }

        ///<summary>
        /// Response for session setup operation
        /// </summary>
        /// <param name="status">Status in response</param>
        /// <param name="sessionEncryptDataType">Indicates whether server set the SessionFlags.SMB2_SESSION_FLAG_ENCRYPT_DATA in response</param>
        /// <param name="c">Server configuration related to model</param>
        [Rule]
        public static void SessionSetupResponse(ModelSmb2Status status, SessionEncryptDataType sessionEncryptDataType, EncryptionConfig c)
        {
            Condition.IsTrue(state == ModelState.Connected);

            Condition.IsTrue(config.IsGlobalEncryptDataEnabled == c.IsGlobalEncryptDataEnabled);
            Condition.IsTrue(config.IsGlobalRejectUnencryptedAccessEnabled == c.IsGlobalRejectUnencryptedAccessEnabled);

            if (ModelUtility.IsSmb3xFamily(config.MaxSmbVersionSupported)
                && !Smb2Utility.IsSmb3xFamily(negotiateDialect)
                && config.IsGlobalEncryptDataEnabled
                && config.IsGlobalRejectUnencryptedAccessEnabled)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.5: 1. If the server implements the SMB 3.x dialect family, " +
                    "Connection.Dialect does not belong to the SMB 3.x dialect family, EncryptData is TRUE, " +
                    "and RejectUnencryptedAccess is TRUE, the server MUST fail the request with STATUS_ACCESS_DENIED.");
                ModelHelper.Log(LogType.TestInfo,
                    "The server implements {0}, Connection.Dialect is {1}, EncryptData is TRUE and RejectUnencryptedAccess is TRUE",
                    config.MaxSmbVersionSupported, negotiateDialect);

                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);

                Condition.IsTrue(status == ModelSmb2Status.STATUS_ACCESS_DENIED);
                return;
            }

            if (Smb2Utility.IsSmb3xFamily(negotiateDialect)
                && config.IsGlobalEncryptDataEnabled
                && config.IsGlobalRejectUnencryptedAccessEnabled
                && !Connection_ClientCapabilities_SMB2_GLOBAL_CAP_ENCRYPTION)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.5: 2. If Connection.Dialect belongs to the SMB 3.x dialect family, " +
                    "EncryptData is TRUE, RejectUnencryptedAccess is TRUE, " +
                    "and Connection.ClientCapabilities does not include the SMB2_GLOBAL_CAP_ENCRYPTION bit, " +
                    "the server MUST fail the request with STATUS_ACCESS_DENIED.");
                ModelHelper.Log(LogType.TestInfo,
                    "Connection.Dialect is {0}, EncryptData is TRUE, RejectUnencryptedAccess is TRUE, " +
                    "and Connection.ClientCapabilities does not include the SMB2_GLOBAL_CAP_ENCRYPTION bit.", negotiateDialect);

                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);

                Condition.IsTrue(status == ModelSmb2Status.STATUS_ACCESS_DENIED);
                return;
            }

            if (Smb2Utility.IsSmb3xFamily(negotiateDialect)
               && config.IsGlobalEncryptDataEnabled
               && (Connection_ServerCapabilities_SMB2_GLOBAL_CAP_ENCRYPTION
                || config.IsGlobalRejectUnencryptedAccessEnabled))
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.5.3: 10.	If global EncryptData is TRUE, the server MUST do the following: " +
                    "If Connection.ServerCapabilities includes SMB2_GLOBAL_CAP_ENCRYPTION or RejectUnencryptedAccess is TRUE,");
                Condition.IsTrue(sessionEncryptDataType == SessionEncryptDataType.SessionEncryptDataSet);
                Session_EncryptData = SessionEncryptDataType.SessionEncryptDataSet;
            }

            Condition.IsTrue(status == Smb2Status.STATUS_SUCCESS);
            Session_IsExisted = true;
        }

        /// <summary>
        /// Connect Share request
        /// </summary>
        /// <param name="connectToShareType">Indicates whether connect to an encrypted share</param>
        /// <param name="ModelRequestType">Indicates whether the tree connect is encrypted</param>
        [Rule]
        public static void TreeConnectRequest(ConnectToShareType connectToShareType, ModelRequestType modelRequestType)
        {
            Condition.IsTrue(state == ModelState.Connected);
            Condition.IsNull(request);
            Condition.IsTrue(Session_IsExisted);

            request = new ModelTreeConnectRequest(connectToShareType, modelRequestType);
        }

        ///<summary>
        /// Response for connect share operation
        /// </summary>
        /// <param name="status">Status in response</param>
        /// <param name="shareEncryptDataType">Indicates whether SMB2_SHAREFLAG_ENCRYPT_DATA is set in response</param>
        /// <param name="modelResponseType">Indicates whether the response is encrypted</param>
        /// <param name="c">Server configuration related to model</param>
        [Rule]
        public static void TreeConnectResponse(
            ModelSmb2Status status,
            ShareEncryptDataType shareEncryptDataType,
            ModelResponseType modelResponseType,
            EncryptionConfig c)
        {
            Condition.IsTrue(state == ModelState.Connected);
            Condition.IsTrue(config.IsGlobalRejectUnencryptedAccessEnabled == c.IsGlobalRejectUnencryptedAccessEnabled);
            Condition.IsTrue(Session_IsExisted);

            ModelTreeConnectRequest treeConnectRequest = ModelHelper.RetrieveOutstandingRequest<ModelTreeConnectRequest>(ref request);

            if (!VerifySession(status, treeConnectRequest.modelRequestType, c))
            {
                return;
            }

            if (ModelUtility.IsSmb3xFamily(config.MaxSmbVersionSupported)
                && (config.IsGlobalEncryptDataEnabled
                    || treeConnectRequest.connectToShareType == ConnectToShareType.ConnectToEncryptedShare)
                && config.IsGlobalRejectUnencryptedAccessEnabled
                && !Connection_ServerCapabilities_SMB2_GLOBAL_CAP_ENCRYPTION)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.7: If the server implements the SMB 3.x dialect family, EncryptData or Share.EncryptData is TRUE, " +
                    "RejectUnencryptedAccess is TRUE, and Connection.ServerCapabilities does not include SMB2_GLOBAL_CAP_ENCRYPTION, " +
                    "the server MUST fail the request with STATUS_ACCESS_DENIED.");
                Condition.IsTrue(config.Platform == c.Platform);
                ModelHelper.Log(LogType.TestInfo,
                    "The server implements {0}, EncryptData is {1}, Share.EncryptData is {2}, RejectUnencryptedAccess is TRUE, " +
                    "Connection.ServerCapabilities does not include SMB2_GLOBAL_CAP_ENCRYPTION.",
                    config.MaxSmbVersionSupported,
                    config.IsGlobalEncryptDataEnabled,
                    treeConnectRequest.connectToShareType == ConnectToShareType.ConnectToEncryptedShare ? "TRUE" : "FALSE");
                ModelHelper.Log(LogType.TestInfo, "The SUT platform is {0}.", config.Platform);
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);

                Condition.IsTrue(status == ModelSmb2Status.STATUS_ACCESS_DENIED);

                return;
            }

            if (Smb2Utility.IsSmb3xFamily(negotiateDialect)
                && treeConnectRequest.connectToShareType == ConnectToShareType.ConnectToEncryptedShare
                && config.IsGlobalRejectUnencryptedAccessEnabled
                && !Connection_ServerCapabilities_SMB2_GLOBAL_CAP_ENCRYPTION)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.7: If Connection.Dialect belongs to the SMB 3.x dialect family, " +
                    "Share.EncryptData is TRUE, RejectUnencryptedAccess is TRUE, " +
                    "and Connection.ServerCapabilities does not include SMB2_GLOBAL_CAP_ENCRYPTION, " +
                    "the server MUST fail the request with STATUS_ACCESS_DENIED.");
                ModelHelper.Log(LogType.Requirement,
                    "\tSet the SMB2_SHAREFLAG_ENCRYPT_DATA bit.");
                ModelHelper.Log(LogType.TestInfo, "Connection.Dialect is {0}, and Share.EncryptData is TRUE.", negotiateDialect);
                Condition.IsTrue(shareEncryptDataType == ShareEncryptDataType.ShareEncryptDataSet);
            }

            //TODO: To be implemented after TRANSFORM_HEADER added into Smb2FunctionalClient
            if (treeConnectRequest.modelRequestType == ModelRequestType.EncryptedRequest)
            {
                Condition.IsTrue(modelResponseType == ModelResponseType.EncryptedResponse);
            }
            else
            {
                Condition.IsTrue(modelResponseType == ModelResponseType.UnEncryptedResponse);
            }
            Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);

            Encryption_TreeId = (treeConnectRequest.connectToShareType == ConnectToShareType.ConnectToEncryptedShare) ? EncryptionTreeId.TreeIdToEncryptShare : EncryptionTreeId.TreeIdToUnEncryptShare;
        }

        ///<summary>
        /// File operation
        /// </summary>
        /// <param name="modelRequestType">Indicates whether the file operation request encrypted</param>
        [Rule]
        public static void FileOperationVerifyEncryptionRequest(ModelRequestType modelRequestType)
        {
            Condition.IsTrue(state == ModelState.Connected);
            Condition.IsNull(request);
            Condition.IsTrue(Session_IsExisted);

            request = new ModelFileOperationVerifyEncryptionRequest(modelRequestType);
        }

        ///<summary>
        /// Response for file operation
        /// </summary>
        /// <param name="status">Status in response</param>
        /// <param name="isResponseEncrypted">Indicates whether file operation response encrypted</param>
        /// <param name="c">Server configuration related to model</param>
        [Rule]
        public static void FileOperationVerifyEncryptionResponse(ModelSmb2Status status, ModelResponseType modelResponseType, EncryptionConfig c)
        {
            Condition.IsTrue(state == ModelState.Connected);
            Condition.IsTrue(config.IsGlobalRejectUnencryptedAccessEnabled == c.IsGlobalRejectUnencryptedAccessEnabled);
            Condition.IsTrue(Session_IsExisted);

            ModelFileOperationVerifyEncryptionRequest createFileRequest = ModelHelper.RetrieveOutstandingRequest<ModelFileOperationVerifyEncryptionRequest>(ref request);


            if (!VerifySession(status, createFileRequest.modelRequestType, c))
            {
                return;
            }

            if (!VerifyTreeConnect(status, createFileRequest.modelRequestType, c))
            {
                return;
            }
            ModelHelper.Log(LogType.TestInfo,
                        "The server implements {0}, Request.IsEncrypted is {1}, Connection.ServerCapabilities {2}include SMB2_GLOBAL_CAP_ENCRYPTION, " +
                        "TreeConnect.Share.EncryptData is {3}, " +
                        "EncryptData is {4}, RejectUnencryptedAccess is {5}",
                        config.MaxSmbVersionSupported,
                        createFileRequest.modelRequestType == ModelRequestType.UnEncryptedRequest ? "FALSE" : "TRUE",
                        Connection_ServerCapabilities_SMB2_GLOBAL_CAP_ENCRYPTION ? "" : "does not ",
                        Encryption_TreeId == EncryptionTreeId.TreeIdToEncryptShare ? "TRUE" : "FALSE",
                        config.IsGlobalEncryptDataEnabled ? "TRUE" : "FALSE",
                        config.IsGlobalRejectUnencryptedAccessEnabled ? "TRUE" : "FALSE");
            if (((createFileRequest.modelRequestType == ModelRequestType.UnEncryptedRequest
                && (config.IsGlobalEncryptDataEnabled
                    || Encryption_TreeId == EncryptionTreeId.TreeIdToEncryptShare))
                || (createFileRequest.modelRequestType == ModelRequestType.EncryptedRequest && !Connection_ServerCapabilities_SMB2_GLOBAL_CAP_ENCRYPTION))
                && config.IsGlobalRejectUnencryptedAccessEnabled)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.1.5: RejectUnencryptedAccess: A Boolean that, if set, " +
                    "indicates that the server will reject any unencrypted messages. " +
                    "This flag is applicable only if EncryptData is TRUE or if " +
                    "Share.EncryptData (as defined in section 3.3.1.6) is TRUE.");
                Condition.IsTrue(status == ModelSmb2Status.STATUS_ACCESS_DENIED);
                return;
            }

            //TODO: To be implemented after TRANSFORM_HEADER added into Smb2FunctionalClient
            if (createFileRequest.modelRequestType == ModelRequestType.EncryptedRequest)
            {
                Condition.IsTrue(modelResponseType == ModelResponseType.EncryptedResponse);
            }
            else
            {
                Condition.IsTrue(modelResponseType == ModelResponseType.UnEncryptedResponse);
            }

            Condition.IsTrue(status == Smb2Status.STATUS_SUCCESS);
        }

        /// <summary>
        /// Expect the disconnection
        /// </summary>
        /// <param name="connectionId">Connection terminated</param>
        [Rule]
        public static void ExpectDisconnect()
        {
            Condition.IsTrue(state == ModelState.Connected);
            Condition.IsTrue(Session_IsExisted);

            state = ModelState.Disconnected;
        }

        #endregion

        #region Private Methods

        private static bool VerifySession(ModelSmb2Status status, ModelRequestType modelRequestType, EncryptionConfig c)
        {
            ModelHelper.Log(LogType.Requirement, "3.3.5.2.9 Verifying the Session");
            if (Smb2Utility.IsSmb3xFamily(negotiateDialect)
                && Session_EncryptData == SessionEncryptDataType.SessionEncryptDataSet
                && (modelRequestType == ModelRequestType.UnEncryptedRequest)
                && (config.MaxSmbVersionSupported == ModelDialectRevision.Smb311 || 
                ((config.MaxSmbVersionSupported == ModelDialectRevision.Smb30 || config.MaxSmbVersionSupported == ModelDialectRevision.Smb302) && 
                config.IsGlobalRejectUnencryptedAccessEnabled)))
            {
                ModelHelper.Log(LogType.Requirement,
                    "If Connection.Dialect belongs to the SMB 3.x dialect family, and Session.EncryptData is TRUE, " +
                    "the server MUST do the following: \n" +
                    "\tIf the server supports the 3.1.1 dialect, locate the Request in Connection.RequestList for which " +
                    "Request.MessageId matches the MessageId value in the SMB2 header of the request." +
                    "\tOtherwise, if the server supports 3.0 or 3.0.2 dialect, and RejectUnencryptedAccess is TRUE, " +
                    "locate the Request in Connection.RequestList for which Request.MessageId matches the MessageId " +
                    "value in the SMB2 header of the request.\n" +
                    "If Request.IsEncrypted is FALSE, the server MUST fail the request with STATUS_ACCESS_DENIED");
                ModelHelper.Log(LogType.TestInfo, "Server supports {0}, RejectUnencryptedAccess is {1}",
                    config.MaxSmbVersionSupported, config.IsGlobalRejectUnencryptedAccessEnabled ? "TRUE" : "FALSE");
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);

                Condition.IsTrue(status == ModelSmb2Status.STATUS_ACCESS_DENIED);
                return false;
            }
            return true;
        }

        private static bool VerifyTreeConnect(ModelSmb2Status status, ModelRequestType modelRequestType, EncryptionConfig c)
        {
            ModelHelper.Log(LogType.Requirement, "3.3.5.2.11 Verifying the Tree Connect");
            if (Encryption_TreeId == EncryptionTreeId.NoTreeId)
            {
                ModelHelper.Log(LogType.Requirement,
                    "The server MUST look up the TreeConnect in Session.TreeConnectTable by using the TreeId in the SMB2 header of the request. " +
                    "If no tree connect is found, the request MUST be failed with STATUS_NETWORK_NAME_DELETED.");
                ModelHelper.Log(LogType.TestInfo, "No tree connect is found.");
                ModelHelper.Log(LogType.TestTag, TestTag.InvalidIdentifier);

                Condition.IsTrue(status == ModelSmb2Status.STATUS_NETWORK_NAME_DELETED);
                return false;
            }
            if (Smb2Utility.IsSmb3xFamily(negotiateDialect))
            {
                ModelHelper.Log(LogType.Requirement,
                    "If the Connection.Dialect belongs to the SMB 3.x dialect family, the server MUST fail the request with STATUS_ACCESS_DENIED in the following cases");
                ModelHelper.Log(LogType.TestInfo, "The Connection.Dialect is {0}.", negotiateDialect);
                // Actually the "EncryptData is true" is redundant since it would failed in verify session step
                if (Connection_ServerCapabilities_SMB2_GLOBAL_CAP_ENCRYPTION
                    && ((config.MaxSmbVersionSupported == ModelDialectRevision.Smb311 && Encryption_TreeId == EncryptionTreeId.TreeIdToEncryptShare) || 
                    ((config.MaxSmbVersionSupported == ModelDialectRevision.Smb30 || config.MaxSmbVersionSupported == ModelDialectRevision.Smb302) && 
                    (Encryption_TreeId == EncryptionTreeId.TreeIdToEncryptShare || config.IsGlobalEncryptDataEnabled) && config.IsGlobalRejectUnencryptedAccessEnabled))
                    && modelRequestType == ModelRequestType.UnEncryptedRequest)
                {
                    ModelHelper.Log(LogType.Requirement,
                       "\tServer supports the 3.1.1 dialect, TreeConnect.Share.EncryptData is TRUE, " +
                       "Connection.ServerCapabilities includes SMB2_GLOBAL_CAP_ENCRYPTION, and Request.IsEncrypted is FALSE\n" +
                       "\tServer supports the 3.0 or 3.0.2 dialect, EncryptData or TreeConnect.Share.EncryptData is TRUE, " +
                       "Connection.ServerCapabilities includes SMB2_GLOBAL_CAP_ENCRYPTION, RejectUnencryptedAccess is TRUE, " +
                       "and Request.IsEncrypted is FALSE");
                    ModelHelper.Log(LogType.TestInfo, "Server supports {0}, RejectUnencryptedAccess is {1}",
                        config.MaxSmbVersionSupported, config.IsGlobalRejectUnencryptedAccessEnabled ? "TRUE" : "FALSE");
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);

                    Condition.IsTrue(status == ModelSmb2Status.STATUS_ACCESS_DENIED);
                    return false;
                }
                if ((config.IsGlobalEncryptDataEnabled
                    || Encryption_TreeId == EncryptionTreeId.TreeIdToEncryptShare
                    || modelRequestType == ModelRequestType.EncryptedRequest)
                    && config.IsGlobalRejectUnencryptedAccessEnabled
                && !Connection_ServerCapabilities_SMB2_GLOBAL_CAP_ENCRYPTION)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "\tEncryptData or TreeConnect.Share.EncryptData or Request.IsEncrypted is TRUE, RejectUnencryptedAccess is TRUE, " +
                        "and Connection.ServerCapabilities does not include SMB2_GLOBAL_CAP_ENCRYPTION.");

                    ModelHelper.Log(LogType.TestInfo,
                        "The server implements {0}, EncryptData is {1}, TreeConnect.Share.EncryptData is {2}, " +
                        "Request.IsEncrypted is {3}, RejectUnencryptedAccess is TRUE, " +
                        "and Connection.ServerCapabilities does not include SMB2_GLOBAL_CAP_ENCRYPTION.",
                        config.MaxSmbVersionSupported,
                        config.IsGlobalEncryptDataEnabled,
                        Encryption_TreeId == EncryptionTreeId.TreeIdToEncryptShare ? "TRUE" : "FALSE",
                        modelRequestType == ModelRequestType.EncryptedRequest ? "TRUE" : "FALSE");
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is {0}.", config.Platform);

                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);

                    Condition.IsTrue(status == ModelSmb2Status.STATUS_ACCESS_DENIED);
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}

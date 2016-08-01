// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.AppInstanceId;
using Microsoft.Xrt.Runtime;

[assembly: NativeType("System.Diagnostics.Tracing.*")]
namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model.AppInstanceId
{
    public static class AppInstanceIdModel
    {
        #region state

        public static AppInstanceIdModelOpen Open = null;
        /// <summary>
        /// The state of the model.
        /// </summary>
        public static ModelState State = ModelState.Uninitialized;

        /// <summary>
        /// Max SMB dialect the server supports
        /// </summary>
        public static ModelDialectRevision MaxSmbVersionSupported;

        #endregion

        #region Rules
        /// <summary>
        /// The call for reading configuration.
        /// </summary>
        [Rule(Action = "call ReadConfig(out _)")]
        public static void ReadConfigCall()
        {
            Condition.IsTrue(State == ModelState.Uninitialized);
        }

        /// <summary>
        /// The return for reading configuration.
        /// </summary>
        /// <param name="dialectRevision">The max version of SMB supported by Server</param>
        [Rule(Action = "return ReadConfig(out dialectRevision)")]
        public static void ReadConfigReturn(ModelDialectRevision dialectRevision)
        {
            Condition.IsTrue(dialectRevision == ModelDialectRevision.Smb2002 ||
                             dialectRevision == ModelDialectRevision.Smb21 ||
                             dialectRevision == ModelDialectRevision.Smb30 ||
                             dialectRevision == ModelDialectRevision.Smb302);
            MaxSmbVersionSupported = dialectRevision;

            State = ModelState.Initialized;
        }

        /// <summary>
        /// It contains Connect, Negotiate, SessionSetup, and CreateRequest, and it will also contain disconnect according to CreateType
        /// </summary>
        /// <param name="clientDialect">The dialect version for client used in Negotiation</param>
        /// <param name="appInstanceIdType">How AppInstanceId is included in create request</param>
        /// <param name="createType">How to construct create request and if disconnect/reconnect will be done</param>
        [Rule]
        public static void PrepareOpen(
            ModelDialectRevision clientDialect,
            AppInstanceIdType appInstanceIdType,
            CreateType createType)
        {
            Condition.IsTrue(State == ModelState.Initialized);

            // appInstanceIdType should not be Invalid or None when preparing.
            Condition.IsFalse(appInstanceIdType == AppInstanceIdType.InvalidAppInstanceId || appInstanceIdType == AppInstanceIdType.NoAppInstanceId);
            Condition.IsFalse(createType == CreateType.ReconnectDurable);

            Combination.Isolated(clientDialect == ModelDialectRevision.Smb2002);
            Combination.Isolated(clientDialect == ModelDialectRevision.Smb21);

            // If the server doesn't support Dialect 3.x family, then createdurablev2 will not work and CreateDurableThenDisconnect will result in Open is null.
            Condition.IfThen(!ModelUtility.IsSmb3xFamily(MaxSmbVersionSupported), createType != CreateType.CreateDurableThenDisconnect);

            State = ModelState.Connected;
            Open = new AppInstanceIdModelOpen();
            Open.CreateTypeWhenPrepare = createType;
            Open.AppInstanceId = AppInstanceIdType.NoAppInstanceId;

            if (!ModelUtility.IsSmb3xFamily(MaxSmbVersionSupported))
            {
                ModelHelper.Log(LogType.TestInfo,
                    "Connection.Dialect does not belong to SMB 3.x dialect family. So Open.AppInstanceId is not set.");
                return;
            }

            ModelHelper.Log(LogType.Requirement,
                "3.3.5.9: If the server implements the SMB 3.x dialect family,  the server MUST initialize the following:");
            ModelHelper.Log(LogType.TestInfo, "Server supports dialect {0}.", MaxSmbVersionSupported);

            if (createType == CreateType.CreateDurable)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.9: Open.AppInstanceId MUST be set to AppInstanceId in the SMB2_CREATE_APP_INSTANCE_ID create context request " +
                    "if the create request includes the SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 and SMB2_CREATE_APP_INSTANCE_ID create contexts.");
                Open.AppInstanceId = appInstanceIdType;
                ModelHelper.Log(LogType.TestInfo,
                    "The create request includes the SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 and SMB2_CREATE_APP_INSTANCE_ID create contexts.");
            }
        }

        /// <summary>
        /// Request Open to see if it can be closed.
        /// </summary>
        /// <param name="clientGuidType">If Open.Session.Connection.ClientGuid is equal to the current Connection.ClientGuid</param>
        /// <param name="pathNameType">If Target path name is equal to Open.PathName</param>
        /// <param name="createType">How to construct create request</param>
        /// <param name="shareType">Type of the connected share</param>
        /// <param name="appInstanceIdType">How AppInstanceId is included in create request</param>
        [Rule]
        public static void OpenRequest(
            ClientGuidType clientGuidType,
            PathNameType pathNameType,
            CreateType createType,
            ShareType shareType,
            AppInstanceIdType appInstanceIdType)
        {
            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsNotNull(Open);

            // Isolate below params to limite the expanded test cases.
            Combination.Isolated(clientGuidType == ClientGuidType.SameClientGuid);
            Combination.Isolated(pathNameType == PathNameType.DifferentPathName);
            Combination.Isolated(shareType == ShareType.DifferentShareDifferentLocal);
            Combination.Isolated(shareType == ShareType.DifferentShareSameLocal);
            Combination.Isolated(appInstanceIdType == AppInstanceIdType.InvalidAppInstanceId);
            Combination.Isolated(appInstanceIdType == AppInstanceIdType.NoAppInstanceId);

            // "AppInstanceId is zero" is only applicable for the first Create Request.
            // For the second Create Request, only valid/notvalid/none make sense.
            Condition.IsFalse(appInstanceIdType == AppInstanceIdType.AppInstanceIdIsZero);

            // CreateDurableThenDisconnect is only applicable for the first Create Request.
            Condition.IsFalse(createType == CreateType.CreateDurableThenDisconnect);

            // If the client doesn't disconnect from the server after sending the first Create Request, 
            // then the second Create Request does not need to contain reconnect context.
            // And vice versa.
            Condition.IfThen(Open.CreateTypeWhenPrepare != CreateType.CreateDurableThenDisconnect, createType != CreateType.ReconnectDurable);
            Condition.IfThen(Open.CreateTypeWhenPrepare == CreateType.CreateDurableThenDisconnect, createType == CreateType.ReconnectDurable);
            
            if (createType == CreateType.ReconnectDurable)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.9.13: If the create request also includes the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context, " +
                    "the server MUST process the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context as specified in section 3.3.5.9.12, " +
                    "and this section MUST be skipped.");
                ModelHelper.Log(LogType.TestInfo, "SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context is included.");
                return;
            }

            if (!ModelUtility.IsSmb3xFamily(MaxSmbVersionSupported))
            {
                ModelHelper.Log(LogType.Requirement,
                    "2.2.13.2.13: The SMB2_CREATE_APP_INSTANCE_ID context is valid only for the SMB 3.x dialect family.");
                ModelHelper.Log(LogType.TestInfo, "The dialect version of the server is {0}.", MaxSmbVersionSupported);
                return;
            }

            if (appInstanceIdType == AppInstanceIdType.ValidAppInstanceId && Open.AppInstanceId != AppInstanceIdType.NoAppInstanceId
                && pathNameType == PathNameType.SamePathName
                && shareType == ShareType.SameShare
                && clientGuidType == ClientGuidType.DifferentClientGuid)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.9.13: The server MUST attempt to locate an Open in GlobalOpenTable where:");
                ModelHelper.Log(LogType.Requirement,
                    "\tAppInstanceId in the request is equal to Open.AppInstanceId.");
                ModelHelper.Log(LogType.Requirement,
                    "\tTarget path name is equal to Open.PathName.");
                ModelHelper.Log(LogType.Requirement,
                    "\tOpen.TreeConnect.Share is equal to TreeConnect.Share.");
                ModelHelper.Log(LogType.Requirement,
                    "\tOpen.Session.Connection.ClientGuid is not equal to the current Connection.ClientGuid.");
                ModelHelper.Log(LogType.TestInfo, "All the above conditions are met.");

                ModelHelper.Log(LogType.Requirement,
                    "If an Open is found, the server MUST calculate the maximal access that the user, " +
                    "identified by Session.SecurityContext, has on the file being opened<277>. " +
                    "If the maximal access includes GENERIC_READ access, the server MUST close the open as specified in 3.3.4.17.");
                // The user used in this model is administrator, so maximal access always includes GENERIC_READ access.
                ModelHelper.Log(LogType.TestInfo, "The maximal access includes GENERIC_READ access. So open is closed.");

                // close open
                Open = null;
            }
            else
            {
                ModelHelper.Log(LogType.TestInfo, "appInstanceIdType is {0}.", appInstanceIdType.ToString());
                ModelHelper.Log(LogType.TestInfo, "pathNameType is {0}.", pathNameType.ToString());
                ModelHelper.Log(LogType.TestInfo, "shareType is {0}.", shareType.ToString());
                ModelHelper.Log(LogType.TestInfo, "clientGuidType is {0}.", clientGuidType.ToString());
                ModelHelper.Log(LogType.TestInfo, "All the above conditions do not match the requirement, so open will not be closed.");

                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
            }
        }

        /// <summary>
        /// Verify status
        /// </summary>
        /// <param name="status"></param>
        [Rule]
        public static void OpenResponse(OpenStatus status)
        {
            ModelHelper.Log(LogType.TestInfo, "Open {0}.", Open == null ? "does not exist" : "exists");

            if (null == Open)
            {
                Condition.IsTrue(status == OpenStatus.OpenClosed);
            }
            else
            {
                Condition.IsTrue(status == OpenStatus.OpenNotClosed);
            }
        }
        #endregion
    }
}

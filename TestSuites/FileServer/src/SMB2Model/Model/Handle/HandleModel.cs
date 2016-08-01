// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Handle;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Xrt.Runtime;

[assembly: NativeType("System.Diagnostics.Tracing.*")]
namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model.Handle
{
    /// <summary>
    /// A HandleOpen class indicates an Open
    /// </summary>
    public class HandleModelOpen
    {
        /// <summary>
        /// Indicate whether the connection existed
        /// </summary>
        public bool IsConnectionExisted;

        /// <summary>
        /// Indicate whether the session existed
        /// </summary>
        public bool IsSessionExisted;

        /// <summary>
        /// Indicate whether the open has requested lease
        /// </summary>
        public bool IsLeaseExisted;

        /// <summary>
        /// Indicate the lease version
        /// </summary>
        public int LeaseVersion;

        /// <summary>
        /// Indicate whether the open has requested a batch oplock
        /// </summary>
        public bool IsBatchOplockExisted;

        /// <summary>
        /// Indicate whether the open has requested a durable handle
        /// </summary>
        public bool IsDurable;

        /// <summary>
        /// Indicate whether the open has requested a persistent handle
        /// </summary>
        public bool IsPersistent;

        public override string ToString()
        {
            return string.Format("{{IsConnectionExisted: {0}, IsSessionExisted: {1}, IsLeaseExisted: {2}, IsBatchOplockExisted: {3}, IsDurable: {4}, IsPersistent: {5}}}",
                IsConnectionExisted, IsSessionExisted, IsLeaseExisted, IsBatchOplockExisted, IsDurable, IsPersistent);
        }
    }

    /// <summary>
    /// This models behavior of Handle management for SMB2 server
    /// Assumptions/Restrictions/Notes:
    /// 1. All lease state used in this model is SMB2_LEASE_HANDLE_CACHING | SMB2_LEASE_READ_CACHING
    /// 2. The model does not cover the scenario that the file name in ReconnectOpenRequest is different from PrepareOpen.
    /// 3. Open in this model is an open to a non-directory file.
    /// 4. The model does not cover the scenario that the dialect negotiated in ReconnectOpenRequest is different from PrepareOpen.
    /// </summary>
    public static class HandleModel
    {
        #region State
        /// <summary>
        /// The dialect after negotiation
        /// </summary>
        public static DialectRevision NegotiateDialect = DialectRevision.Smb2Unknown;

        /// <summary>
        /// Server configuration related to model
        /// </summary>
        public static HandleConfig Config;

        /// <summary>
        /// Request that server model is handling
        /// </summary>
        public static ModelSMB2Request Request;

        /// <summary>
        /// Indicates the open for the handle
        /// </summary>
        public static HandleModelOpen Open;

        /// <summary>
        /// A Boolean that, if set, indicates that the share is continuously available
        /// </summary>
        public static bool Share_IsCA = false;

        public static bool ServerCapabilities_PersistentBitSet = false;

        #endregion

        #region Rules
        /// <summary>
        /// Call for loading server configuration
        /// </summary>
        [Rule(Action = "call ReadConfig(out _)")]
        public static void ReadConfigCall()
        {
            Condition.IsNull(Open);
        }

        /// <summary>
        /// Return for loading server configuration
        /// </summary>
        /// <param name="c">Server configuration related to model</param>
        [Rule(Action = "return ReadConfig(out c)")]
        public static void ReadConfigReturn(HandleConfig c)
        {
            Condition.IsNotNull(c);
            Condition.IsNull(Open);

            // workaround for SE bug. Wrong models would be generated without this workaround. 
            Condition.IsTrue(c.MaxSmbVersionSupported == ModelDialectRevision.Smb2002 
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb21 
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb30
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb302);


            Config = c;
            // workaround for SE bug. Wrong models would be generated without this workaround. 
            Condition.IsTrue(c.MaxSmbVersionSupported == Config.MaxSmbVersionSupported);
        }

        /// <summary>
        /// Prepare the open environment by performing the following
        ///      1. Negotiate
        ///      2. TreeConnect
        ///      3. SessionSetup
        ///      4. Create file with specified handle type
        /// </summary>
        /// <param name="clientMaxDialect">Indicates the max dialect revision client supports</param>
        /// <param name="persistentBit">Indicates if SMB2_GLOBAL_CAP_PERSISTENT_HANDLES bit is set in Negotiate Request</param>
        /// <param name="connectToCAShare">Indicates if connecting to CA share when Tree Connect</param>
        /// <param name="modelHandleType">Indicates type of the requested handle: DurableV1, DurableV2 or Persistent</param>
        /// <param name="oplockLeaseType">Indicates type of Lease or Oplock when sending Create Request</param>         
        [Rule]
        public static void PrepareOpen(
            ModelDialectRevision clientMaxDialect,
            PersistentBitType persistentBit,
            CAShareType connectToCAShare,
            ModelHandleType modelHandleType,
            OplockLeaseType oplockLeaseType)
        {
            Condition.IsNull(Request);
            Condition.IsNull(Open);

            // CAShare, Persistent Handle , Durable Handle v2 and Lease V2 are only applied for SMB 3.x family.
            Condition.IfThen(!ModelUtility.IsSmb3xFamily(Config.MaxSmbVersionSupported), connectToCAShare == CAShareType.NonCAShare);
            Condition.IfThen(!ModelUtility.IsSmb3xFamily(Config.MaxSmbVersionSupported), persistentBit == PersistentBitType.PersistentBitNotSet);
            Condition.IfThen(!ModelUtility.IsSmb3xFamily(Config.MaxSmbVersionSupported), modelHandleType == ModelHandleType.DurableHandleV1);

            // Case will go to non-accpeting state if clientMaxDialect > Config.MaxSmbVersionSupported.
            NegotiateDialect = ModelHelper.DetermineNegotiateDialect(clientMaxDialect, Config.MaxSmbVersionSupported);

            // Restrict the params combination
            Combination.Pairwise(clientMaxDialect, persistentBit, connectToCAShare, modelHandleType, oplockLeaseType);

            Share_IsCA = (connectToCAShare == CAShareType.CAShare);

            Open = new HandleModelOpen();
            Open.IsConnectionExisted = true;
            Open.IsSessionExisted = true;

            if (oplockLeaseType == OplockLeaseType.LeaseV1)
            {
                Open.IsLeaseExisted = true;
                Open.LeaseVersion = 1;
            }
            else if (oplockLeaseType == OplockLeaseType.LeaseV2)
            {
                Open.IsLeaseExisted = true;
                Open.LeaseVersion = 2;
            }
            else if (oplockLeaseType == OplockLeaseType.BatchOplock)
            {
                Open.IsBatchOplockExisted = true;
            }

            if (modelHandleType == ModelHandleType.DurableHandleV1)
            {
                if (!Open.IsBatchOplockExisted && !Open.IsLeaseExisted)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "3.3.5.9.6: If the RequestedOplockLevel field in the create request is not set to SMB2_OPLOCK_LEVEL_BATCH and " +
                        "the create request does not include an SMB2_CREATE_REQUEST_LEASE create context with a LeaseState field that " +
                        "includes the SMB2_LEASE_HANDLE_CACHING bit value, " +
                        "the server MUST ignore this create context and skip this section.");
                    ModelHelper.Log(LogType.TestInfo, "This section is skipped.");
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    return;
                }

                ModelHelper.Log(LogType.Requirement, "3.3.5.9.6: In the \"Successful Open Initialization\" phase, the server MUST set Open.IsDurable to TRUE. ");
                ModelHelper.Log(LogType.TestInfo, "Open.IsDurable is set to TRUE.");
                Open.IsDurable = true;
            }
            else if (modelHandleType == ModelHandleType.DurableHandleV2 || modelHandleType == ModelHandleType.PersistentHandle)
            {
                ModelHelper.Log(LogType.Requirement, "3.3.5.9.10: This section applies only to servers that implement the SMB 3.x dialect family. ");
                if (!ModelUtility.IsSmb3xFamily(Config.MaxSmbVersionSupported))
                {
                    ModelHelper.Log(LogType.TestInfo, "The server implements the dialect {0}.", Config.MaxSmbVersionSupported);
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    return;
                }

                if (modelHandleType != ModelHandleType.PersistentHandle && !Open.IsBatchOplockExisted && !Open.IsLeaseExisted)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "If the SMB2_DHANDLE_FLAG_PERSISTENT bit is not set in the Flags field of this create context, " +
                        "if RequestedOplockLevel in the create request is not set to SMB2_OPLOCK_LEVEL_BATCH, " +
                        "and if the create request does not include a SMB2_CREATE_REQUEST_LEASE or " +
                        "SMB2_CREATE_REQUEST_LEASE_V2 create context with a LeaseState field that includes SMB2_LEASE_HANDLE_CACHING, " +
                        "the server MUST ignore this create context and skip this section.");
                    ModelHelper.Log(LogType.TestInfo, "This section is skipped.");
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);

                    return;
                }

                if (ModelUtility.IsSmb3xFamily(NegotiateDialect)
                    && persistentBit == PersistentBitType.PersistentBitSet
                    && Config.IsPersistentHandleSupported)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "3.3.5.4: The Capabilities field MUST be set to a combination of zero or more of the following bit values, as specified in section 2.2.4:");
                    ModelHelper.Log(LogType.Requirement,
                        "\tSMB2_GLOBAL_CAP_PERSISTENT_HANDLES if Connection.Dialect belongs to the SMB 3.x dialect family, " +
                        "SMB2_GLOBAL_CAP_PERSISTENT_HANDLES is set in the Capabilities field of the request, and the server supports persistent handles.");
                    ModelHelper.Log(LogType.TestInfo, "All the above conditions are met. So SMB2_DHANDLE_FLAG_PERSISTENT bit is set in Connection.ServerCapabilities.");
                    ServerCapabilities_PersistentBitSet = true;
                }

                if (modelHandleType == ModelHandleType.PersistentHandle
                    && !(Share_IsCA && ServerCapabilities_PersistentBitSet))
                {
                    // If client asks for a persistent handle to a non CA share or 
                    // Connection.ServerCapabilities does not include SMB2_GLOBAL_CAP_PERSISTENT_HANDLES
                    // The persistent handle is not granted.
                    ModelHelper.Log(
                        LogType.TestInfo, 
                        "Share is not a CA share or Connection.ServerCapabilities does not include SMB2_GLOBAL_CAP_PERSISTENT_HANDLES.");
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    return;
                }

                ModelHelper.Log(LogType.Requirement, "In the \"Successful Open Initialization\" phase, the server MUST set Open.IsDurable to TRUE. ");
                ModelHelper.Log(LogType.TestInfo, "Open.IsDurable is set to TRUE.");
                Open.IsDurable = true;
                
                if (modelHandleType == ModelHandleType.PersistentHandle && Share_IsCA && ServerCapabilities_PersistentBitSet)
                {
                    ModelHelper.Log(LogType.Requirement, 
                        "3.3.5.9.10: If the SMB2_DHANDLE_FLAG_PERSISTENT bit is set in the Flags field of the request, " + 
                        "TreeConnect.Share.IsCA is TRUE, " + 
                        "and Connection.ServerCapabilities includes SMB2_GLOBAL_CAP_PERSISTENT_HANDLES, " + 
                        "the server MUST set Open.IsPersistent to TRUE.");
                    ModelHelper.Log(LogType.TestInfo, "All the above conditions are met. So Open.IsPersistent is set to TRUE.");
                    Open.IsPersistent = true;
                }
            }
        }

        /// <summary>
        /// Client sends Create Request 
        /// </summary>
        /// <param name="clientMaxDialect">Indicates the max dialect revision client supports</param>
        /// <param name="persistentBit">Indicates if SMB2_GLOBAL_CAP_PERSISTENT_HANDLES bit is set in Negotiate Request</param>
        /// <param name="connectToCAShare">Indicates if connecting to CA share when Tree Connect</param>
        /// <param name="oplockLeaseType">Indicates type of Lease or Oplock when sending Create Request</param>
        /// <param name="durableV1RequestContext">Indicates if SMB2_CREATE_DURABLE_HANDLE_REQUEST create context exists</param>
        /// <param name="durableV2RequestContext">Indicates if SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 create context exists</param>
        /// <param name="durableV1ReconnectContext">Indicates if SMB2_CREATE_DURABLE_HANDLE_RECONNECT create context exists</param>
        /// <param name="durableV2ReconnectContext">Indicates if SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context exists</param>
        [Rule]
        public static void OpenRequest(
            ModelDialectRevision clientMaxDialect,
            PersistentBitType persistentBit,
            CAShareType connectToCAShare,
            OplockLeaseType oplockLeaseType,
            DurableV1RequestContext durableV1RequestContext,
            DurableV2RequestContext durableV2RequestContext,
            DurableV1ReconnectContext durableV1ReconnectContext,
            DurableV2ReconnectContext durableV2ReconnectContext)
        {
            Condition.IsNull(Request);
            Condition.IsNull(Open);

            // CAShare, Persistent Handle and Durable Handle V2 are only applied for SMB 3.x family.
            Condition.IfThen(!ModelUtility.IsSmb3xFamily(Config.MaxSmbVersionSupported), connectToCAShare == CAShareType.NonCAShare);
            Condition.IfThen(!ModelUtility.IsSmb3xFamily(Config.MaxSmbVersionSupported), persistentBit == PersistentBitType.PersistentBitNotSet);
            Condition.IfThen(!ModelUtility.IsSmb3xFamily(Config.MaxSmbVersionSupported), durableV2RequestContext == DurableV2RequestContext.DurableV2RequestContextNotExist);
            Condition.IfThen(!ModelUtility.IsSmb3xFamily(Config.MaxSmbVersionSupported), durableV2ReconnectContext == DurableV2ReconnectContext.DurableV2ReconnectContextNotExist);
            Condition.IfThen(!ModelUtility.IsSmb3xFamily(Config.MaxSmbVersionSupported), oplockLeaseType != OplockLeaseType.LeaseV2);

            // If leasing is not supported, do not test LeaseV1 or LeaseV2.
            Condition.IfThen(!Config.IsLeasingSupported, oplockLeaseType != OplockLeaseType.LeaseV1 && oplockLeaseType != OplockLeaseType.LeaseV2);

            Combination.Pairwise(
                clientMaxDialect,
                persistentBit,
                connectToCAShare,
                oplockLeaseType,
                durableV1RequestContext,
                durableV2RequestContext,
                durableV1ReconnectContext,
                durableV2ReconnectContext);

            NegotiateDialect = ModelHelper.DetermineNegotiateDialect(clientMaxDialect, Config.MaxSmbVersionSupported);

            Share_IsCA = (connectToCAShare == CAShareType.CAShare);

            if (ModelUtility.IsSmb3xFamily(NegotiateDialect) 
                && persistentBit == PersistentBitType.PersistentBitSet 
                && Config.IsPersistentHandleSupported)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.4: The Capabilities field MUST be set to a combination of zero or more of the following bit values, as specified in section 2.2.4:");
                ModelHelper.Log(LogType.Requirement,
                    "\tSMB2_GLOBAL_CAP_PERSISTENT_HANDLES if Connection.Dialect belongs to the SMB 3.x dialect family, " +
                    "SMB2_GLOBAL_CAP_PERSISTENT_HANDLES is set in the Capabilities field of the request, and the server supports persistent handles.");
                ModelHelper.Log(LogType.TestInfo, "All the above conditions are met. So SMB2_DHANDLE_FLAG_PERSISTENT bit is set in Connection.ServerCapabilities.");
                ServerCapabilities_PersistentBitSet = true;
            }

            Request = new ModelOpenFileRequest(
                durableV1RequestContext,
                durableV2RequestContext,
                durableV1ReconnectContext,
                durableV2ReconnectContext,
                oplockLeaseType,
                false,
                false,
                false);            
        }

        /// <summary>
        /// Client sends Log off Request
        /// </summary>
        [Rule]
        public static void LogOff()
        {
            Condition.IsTrue(Open != null);

            if (!Open.IsDurable)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.6: The server MUST close every Open in Session.OpenTable of the old session, " +
                    "where Open.IsDurable is FALSE and Open.IsResilient is FALSE, as specified in section 3.3.4.17. ");
                ModelHelper.Log(LogType.TestInfo, "Open.IsDurable is FALSE and Open.IsResilient is FALSE, so the open is closed.");
                Open = null;
            }
            else
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.6: For all opens in Session.OpenTable where Open.IsDurable is TRUE or Open.IsResilient is TRUE, " +
                    "the server MUST set Open.Session, Open.Connection, and Open.TreeConnect to NULL. ");
                ModelHelper.Log(LogType.TestInfo, "Open.IsDurable is TRUE, so Open.Session, Open.Connection is set to NULL.");
                Open.IsSessionExisted = false;
                Open.IsConnectionExisted = false;
            }
        }

        /// <summary>
        /// Client disconnects the connection
        /// </summary>
        [Rule]
        public static void Disconnect()
        {
            Condition.IsTrue(Open != null);

            if (Open.IsPersistent   // Server will preserve the open for reconnect if Open.IsPersistent is true.
                || (Open.IsDurable 
                    && (Open.IsBatchOplockExisted 
                    || Open.IsLeaseExisted)))
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.7.1: The server MUST iterate over the Session.OpenTable and determine whether each Open is to be preserved for reconnect. " +
                    "If any of the following conditions is satisfied, it indicates that the Open is to be preserved for reconnect. ");

                if (Open.IsBatchOplockExisted)
                {
                    ModelHelper.Log(LogType.Requirement, "\tOpen.OplockLevel is equal to SMB2_OPLOCK_LEVEL_BATCH and Open.OplockState is equal to Held, and Open.IsDurable is TRUE.");
                    ModelHelper.Log(LogType.TestInfo, "The above condition is met.");
                }

                if (Open.IsLeaseExisted)
                {
                    ModelHelper.Log(LogType.Requirement, 
                        "\tOpen.OplockLevel is equal to SMB2_OPLOCK_LEVEL_LEASE, Lease.LeaseState contains SMB2_LEASE_HANDLE_CACHING, " + 
                        "Open.OplockState is equal to Held, and Open.IsDurable is TRUE.");
                    ModelHelper.Log(LogType.TestInfo, "The above condition is met.");
                }

                ModelHelper.Log(LogType.TestInfo, "The Open is to be preserved.");

                ModelHelper.Log(LogType.Requirement,
                    "If the Open is to be preserved for reconnect, perform the following actions: ");
                ModelHelper.Log(LogType.Requirement,
                    "\tSet Open.Connection to NULL, Open.Session to NULL, Open.TreeConnect to NULL. ");
                ModelHelper.Log(LogType.TestInfo, "Open.Connection and Open.Session are set to NULL.");

                Open.IsConnectionExisted = false;
                Open.IsSessionExisted = false;
            }
            else
            {
                ModelHelper.Log(LogType.Requirement, "3.3.7.1: If the Open is not to be preserved for reconnect, the server MUST close the Open as specified in section 3.3.4.17.");
                ModelHelper.Log(LogType.TestInfo, "The Open is closed.");
                Open = null;
            }
        }

        /// <summary>
        /// Client reconnects the previous open
        /// </summary>
        /// <param name="durableV1ReconnectContext">Indicates if SMB2_CREATE_DURABLE_HANDLE_RECONNECT create context exists</param>
        /// <param name="durableV2ReconnectContext">Indicates if SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context exists</param>
        /// <param name="oplockLeaseType">Indicates type of Lease or Oplock when sending Create Request</param>
        /// <param name="leaseKeyDifferentialType">Indicates if the LeaseKey is different from Open.Lease.LeaseKey</param>
        /// <param name="clientIdType">Indicates if ClientGuid is different from Open.ClientGuid</param>
        /// <param name="createGuidType">Indicates if CreateGuid is different from Open.CreateGuid</param>
        [Rule]
        public static void ReconnectOpenRequest(
            DurableV1ReconnectContext durableV1ReconnectContext,
            DurableV2ReconnectContext durableV2ReconnectContext,
            OplockLeaseType oplockLeaseType,
            LeaseKeyDifferentialType leaseKeyDifferentialType,
            ClientIdType clientIdType,
            CreateGuidType createGuidType)
        {
            // If lease does not exist, LeaseKeyDifferentialType and LeaseStateDifferentialType take no effect. So just fix their values.
            Condition.IfThen(oplockLeaseType != OplockLeaseType.LeaseV1 && oplockLeaseType != OplockLeaseType.LeaseV2,
                leaseKeyDifferentialType == LeaseKeyDifferentialType.SameLeaseKey);

            // If there is no durablev2 reconnect context, fix the value of CreateGuid.
            Condition.IfThen(durableV2ReconnectContext == DurableV2ReconnectContext.DurableV2ReconnectContextNotExist, createGuidType == CreateGuidType.SameCreateGuid);

            // Do not need to test the situation of both V1 and V2 reconnect contexts not existed.
            Condition.IsFalse(durableV1ReconnectContext == DurableV1ReconnectContext.DurableV1ReconnectContextNotExist
                && durableV2ReconnectContext == DurableV2ReconnectContext.DurableV2ReconnectContextNotExist);

            // Only the server implements the SMB 2.1 or SMB 3.x dialect family supports leasing
            Condition.IfThen(NegotiateDialect == DialectRevision.Smb2002, oplockLeaseType != OplockLeaseType.LeaseV1 && oplockLeaseType != OplockLeaseType.LeaseV2);

            // Lease V2 is only vaild for SMB 3.x dialect family.
            Condition.IfThen(NegotiateDialect == DialectRevision.Smb21, oplockLeaseType != OplockLeaseType.LeaseV2);
            
            // If leasing is not supported, do not test LeaseV1 or LeaseV2.
            Condition.IfThen(!Config.IsLeasingSupported,
                oplockLeaseType != OplockLeaseType.LeaseV1 && oplockLeaseType != OplockLeaseType.LeaseV2);

            // 3.3.5.9.10, 3.3.5.9.12   This section applies only to servers that implement the SMB 3.x dialect family. 
            Condition.IfThen(!ModelUtility.IsSmb3xFamily(Config.MaxSmbVersionSupported), durableV2ReconnectContext == DurableV2ReconnectContext.DurableV2ReconnectContextNotExist);

            // Add restriction to limit the generated cases of model.
            Combination.Pairwise(durableV1ReconnectContext, durableV2ReconnectContext, oplockLeaseType, leaseKeyDifferentialType, clientIdType, createGuidType);

            Condition.IsNull(Request);
            Request = new ModelOpenFileRequest(
                DurableV1RequestContext.DurableV1RequestContextNotExist,
                DurableV2RequestContext.DurableV2RequestContextNotExist,
                durableV1ReconnectContext,
                durableV2ReconnectContext,
                oplockLeaseType,
                leaseKeyDifferentialType == LeaseKeyDifferentialType.SameLeaseKey,
                clientIdType == ClientIdType.SameClient,
                createGuidType == CreateGuidType.SameCreateGuid);
        }

        ///<summary>
        /// Response for create/reconnect open 
        /// </summary>
        /// <param name="status">Indicates status in Create Response</param>
        /// <param name="modelResponseContext">Indicates type of create context in Create Response</param>
        /// <param name="c">Indicates config of server</param>        
        [Rule]
        public static void OpenResponse(
            ModelSmb2Status status,
            DurableHandleResponseContext durableHandleResponseContext, 
            LeaseResponseContext leaseResponseContext,
            HandleConfig c)
        {
            Condition.IsNotNull(Request);

            ModelOpenFileRequest modelOpenFileRequest = ModelHelper.RetrieveOutstandingRequest<ModelOpenFileRequest>(ref Request);

            if (ModelUtility.IsSmb3xFamily(NegotiateDialect)
                && modelOpenFileRequest.durableV1ReconnectContext == DurableV1ReconnectContext.DurableV1ReconnectContextNotExist
                && modelOpenFileRequest.durableV2ReconnectContext == DurableV2ReconnectContext.DurableV2ReconnectContextNotExist)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.9: If Connection.Dialect belongs to the SMB 3.x dialect family and " +
                    "the request does not contain SMB2_CREATE_DURABLE_HANDLE_RECONNECT Create Context or SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 Create Context, " +
                    "the server MUST look up an existing open in the GlobalOpenTable where Open.FileName matches the file name in the Buffer field of the request. ");
                ModelHelper.Log(LogType.TestInfo,
                    "Connection.Dialect is {0} and the request does not contain SMB2_CREATE_DURABLE_HANDLE_RECONNECT or SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 Create Context.",
                    NegotiateDialect);

                if (Open != null
                    && !modelOpenFileRequest.isSameClient
                    && Open.IsPersistent
                    && !Open.IsConnectionExisted
                    && !Open.IsBatchOplockExisted
                    && !Open.IsLeaseExisted)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "If an Open entry is found, and if all the following conditions are satisfied, " +
                        "the server MUST fail the request with STATUS_FILE_NOT_AVAILABLE.");
                    ModelHelper.Log(LogType.Requirement, "\tOpen.IsPersistent is TRUE");
                    ModelHelper.Log(LogType.Requirement, "\tOpen.Connection is NULL");
                    ModelHelper.Log(LogType.Requirement, "\tOpen.OplockLevel is not equal to SMB2_OPLOCK_LEVEL_BATCH");
                    ModelHelper.Log(LogType.Requirement, "\tOpen.OplockLevel is not equal to SMB2_OPLOCK_LEVEL_LEASE or Open.Lease.LeaseState does not include SMB2_LEASE_HANDLE_CACHING");

                    ModelHelper.Log(LogType.TestInfo, "The Open is found, and all the conditions are satisfied.");
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);

                    Condition.IsTrue(status == ModelSmb2Status.STATUS_FILE_NOT_AVAILABLE);
                    return;
                }
            }

            // TD section 3.3.5.9.6: Handling the SMB2_CREATE_DURABLE_HANDLE_REQUEST Create Context
            if (modelOpenFileRequest.durableV1RequestContext == DurableV1RequestContext.DurableV1RequestContextExist)
            {
                if (Handling_SMB2_CREATE_DURABLE_HANDLE_REQUEST_CreateContext(status, modelOpenFileRequest, durableHandleResponseContext, c))
                    return;
            }

            // TD section 3.3.5.9.7: Handling the SMB2_CREATE_DURABLE_HANDLE_RECONNECT Create Context
            if (modelOpenFileRequest.durableV1ReconnectContext == DurableV1ReconnectContext.DurableV1ReconnectContextExist)
            {
                if (Handling_SMB2_CREATE_DURABLE_HANDLE_RECONNECT_CreateContext(status, modelOpenFileRequest, leaseResponseContext, c))
                    return;
            }

            // TD section 3.3.5.9.10: Handling the SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 Create Context
            if (modelOpenFileRequest.durableV2RequestContext != DurableV2RequestContext.DurableV2RequestContextNotExist)
            {
                if (Handling_SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2_CreateContext(status, modelOpenFileRequest, durableHandleResponseContext))
                    return;
            }

            // TD section 3.3.5.9.12: Handling the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 Create Context
            if (modelOpenFileRequest.durableV2ReconnectContext != DurableV2ReconnectContext.DurableV2ReconnectContextNotExist)
            {
                if (Handling_SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2_CreateContext(status, modelOpenFileRequest, leaseResponseContext, c))
                    return;
            }

            Condition.IsTrue(status == Smb2Status.STATUS_SUCCESS);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method is used to verify the SMB2_CREATE_DURABLE_HANDLE_REQUEST Create Context
        /// The client is requesting that the open be marked for durable operation
        /// Cover TD section: 3.3.5.9.6
        /// Return true means the message is handled by this function. 
        /// Return false means the message needs further processing.
        /// </summary>
        private static bool Handling_SMB2_CREATE_DURABLE_HANDLE_REQUEST_CreateContext(
            ModelSmb2Status status,
            ModelOpenFileRequest modelOpenFileRequest,
            DurableHandleResponseContext durableHandleResponseContext,
            HandleConfig c)
        {
            ModelHelper.Log(LogType.Requirement, "3.3.5.9.6: Handling the SMB2_CREATE_DURABLE_HANDLE_REQUEST Create Context");

            if (modelOpenFileRequest.durableV1ReconnectContext == DurableV1ReconnectContext.DurableV1ReconnectContextExist)
            {
                ModelHelper.Log(LogType.Requirement,
                    "If the create request also includes an SMB2_CREATE_DURABLE_HANDLE_RECONNECT create context, " +
                    "the server MUST process the create context as specified in section 3.3.5.9.7 and skip this section.");
                ModelHelper.Log(LogType.TestInfo, "SMB2_CREATE_DURABLE_HANDLE_RECONNECT is included.");
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                return false;
            }
            
            if (modelOpenFileRequest.durableV2RequestContext != DurableV2RequestContext.DurableV2RequestContextNotExist
                || modelOpenFileRequest.durableV2ReconnectContext != DurableV2ReconnectContext.DurableV2ReconnectContextNotExist)
            {
                // Workaround to force SE explorers the platform field.
                Condition.IsTrue(Config.Platform == c.Platform);

                ModelHelper.Log(LogType.Requirement,
                    "If the create request also includes an SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 or SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context, " +
                    "the server SHOULD<266> fail the create request with STATUS_INVALID_PARAMETER.");

                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedContext);

                if (modelOpenFileRequest.durableV2RequestContext != DurableV2RequestContext.DurableV2RequestContextNotExist)
                {
                    ModelHelper.Log(LogType.TestInfo, "SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 is included.");
                }
                if (modelOpenFileRequest.durableV2ReconnectContext != DurableV2ReconnectContext.DurableV2ReconnectContextNotExist)
                {
                    ModelHelper.Log(LogType.TestInfo, "SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 is included.");
                }

                if (Config.Platform == Platform.WindowsServer2008 || Config.Platform == Platform.WindowsServer2008R2)
                {
                    ModelHelper.Log(LogType.Requirement, "<266> Section 3.3.5.9.6: Windows Vista SP1, Windows 7, Windows Server 2008, and Windows Server 2008 R2 ignore undefined create contexts.");
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is {0}", Config.Platform);
                    Condition.IsFalse(durableHandleResponseContext == DurableHandleResponseContext.SMB2_CREATE_DURABLE_HANDLE_RESPONSE);
                    return false;
                }
                else if (Config.Platform == Platform.NonWindows)
                {
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is NonWindows, so the server could return other error code.");
                    Condition.IsTrue(status != ModelSmb2Status.STATUS_SUCCESS);
                }
                else
                {
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is {0}", Config.Platform);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_PARAMETER);
                }

                return true;
            }

            if (modelOpenFileRequest.oplockLeaseType == OplockLeaseType.NoOplockOrLease)
            {
                ModelHelper.Log(LogType.Requirement,
                    "If the RequestedOplockLevel field in the create request is not set to SMB2_OPLOCK_LEVEL_BATCH and " +
                    "the create request does not include an SMB2_CREATE_REQUEST_LEASE create context with a LeaseState field that includes the SMB2_LEASE_HANDLE_CACHING bit value, " +
                    "the server MUST ignore this create context and skip this section.");
                ModelHelper.Log(LogType.TestInfo, "The create response should not contain SMB2_CREATE_DURABLE_HANDLE_RESPONSE.");
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
                Condition.IsFalse(durableHandleResponseContext == DurableHandleResponseContext.SMB2_CREATE_DURABLE_HANDLE_RESPONSE);
                return false;
            }

            ModelHelper.Log(LogType.Requirement, "In the \"Response Construction\" phase, the server MUST construct an SMB2_CREATE_DURABLE_HANDLE_RESPONSE response create context");
            ModelHelper.Log(LogType.TestInfo, "SMB2_CREATE_DURABLE_HANDLE_RESPONSE should be contained in create response");
            Condition.IsTrue(durableHandleResponseContext == DurableHandleResponseContext.SMB2_CREATE_DURABLE_HANDLE_RESPONSE);

            return false;
        }

        /// <summary>
        /// This method is used to verify the SMB2_CREATE_DURABLE_HANDLE_RECONNECT Create Context
        /// The client is requesting a reconnect to an existing durable open
        /// Cover TD section: 3.3.5.9.7
        /// Return true means the message is handled by this function. 
        /// Return false means the message needs further processing.        
        /// </summary>
        private static bool Handling_SMB2_CREATE_DURABLE_HANDLE_RECONNECT_CreateContext(
            ModelSmb2Status status,
            ModelOpenFileRequest modelOpenFileRequest,
            LeaseResponseContext leaseResponseContext,
            HandleConfig c)
        {
            Condition.IsTrue(Config.Platform == c.Platform);

            ModelHelper.Log(LogType.Requirement, "3.3.5.9.7: Handling the SMB2_CREATE_DURABLE_HANDLE_RECONNECT Create Context");

            if (modelOpenFileRequest.durableV2RequestContext != DurableV2RequestContext.DurableV2RequestContextNotExist
                || modelOpenFileRequest.durableV2ReconnectContext != DurableV2ReconnectContext.DurableV2ReconnectContextNotExist)
            {
                ModelHelper.Log(LogType.Requirement,
                    "If the create request also contains an SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 or SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context, " +
                    "the server SHOULD<267> fail the request with STATUS_INVALID_PARAMETER.");
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedContext);

                if (modelOpenFileRequest.durableV2RequestContext != DurableV2RequestContext.DurableV2RequestContextNotExist)
                {
                    ModelHelper.Log(LogType.TestInfo, "SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 is included.");
                }
                if (modelOpenFileRequest.durableV2ReconnectContext != DurableV2ReconnectContext.DurableV2ReconnectContextNotExist)
                {
                    ModelHelper.Log(LogType.TestInfo, "SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 is included.");
                }

                if (Config.Platform == Platform.WindowsServer2008 || Config.Platform == Platform.WindowsServer2008R2)
                {
                    ModelHelper.Log(LogType.Requirement, "<267> Section 3.3.5.9.7: Windows Vista SP1, Windows Server 2008, Windows 7 and Windows Server 2008 R2 ignore undefined create contexts.");
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is {0}.", Config.Platform);
                    Condition.IsTrue(leaseResponseContext == LeaseResponseContext.NONE);
                    return false;
                }
                else if (Config.Platform == Platform.NonWindows)
                {
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is NonWindows, so the server could return other error code.");
                    Condition.IsTrue(status != ModelSmb2Status.STATUS_SUCCESS);
                }
                else
                {
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is {0}", Config.Platform);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_PARAMETER);
                }

                return true;
            }

            ModelHelper.Log(LogType.Requirement,
                "The server MUST look up an existing open in the GlobalOpenTable by doing a lookup with the FileId.Persistent portion of the create context. ");

            if (Open == null)
            {
                ModelHelper.Log(LogType.Requirement, 
                    "If the lookup fails, the server SHOULD<268> fail the request with STATUS_OBJECT_NAME_NOT_FOUND and proceed as specified in \"Failed Open Handling\" in section 3.3.5.9.");
                ModelHelper.Log(LogType.TestInfo, "The open does not exist and the SUT platform is {0}.", Config.Platform);
                ModelHelper.Log(LogType.TestTag, TestTag.InvalidIdentifier);

                if (Config.Platform == Platform.NonWindows)
                    Condition.IsTrue(status != ModelSmb2Status.STATUS_SUCCESS);
                else
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                return true;
            }

            if (Open.IsLeaseExisted && !modelOpenFileRequest.isSameClient)
            {
                ModelHelper.Log(LogType.Requirement,
                    "If Open.Lease is not NULL and Open.ClientGuid is not equal to the ClientGuid of the connection that received this request, " +
                    "the server MUST fail the create request with STATUS_OBJECT_NAME_NOT_FOUND.");
                ModelHelper.Log(LogType.TestInfo, "Open.Lease is not NULL and Open.ClientGuid is not equal to the ClientGuid of the connection.");
                ModelHelper.Log(LogType.TestTag, TestTag.InvalidIdentifier);

                Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                return true;
            }

            if (NegotiateDialect != DialectRevision.Smb2002
                && !Open.IsLeaseExisted 
                && (modelOpenFileRequest.oplockLeaseType == OplockLeaseType.LeaseV1 || modelOpenFileRequest.oplockLeaseType == OplockLeaseType.LeaseV2))
            {
                ModelHelper.Log(LogType.Requirement,
                    "If Open.Lease is NULL and the SMB2_CREATE_REQUEST_LEASE_V2 or the SMB2_CREATE_REQUEST_LEASE create context is present, " +
                    "the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND.");
                ModelHelper.Log(LogType.TestInfo,
                    "Open.Lease is NULL and {0} is present.",
                    modelOpenFileRequest.oplockLeaseType == OplockLeaseType.LeaseV1 ? "SMB2_CREATE_REQUEST_LEASE" : "SMB2_CREATE_REQUEST_LEASE_V2");
                
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedContext);

                Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                return true;
            }

            if (!Open.IsDurable)
            {
                ModelHelper.Log(LogType.Requirement, 
                    "If Open.IsDurable is FALSE and Open.IsResilient is FALSE or unimplemented, " + 
                    "the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND and proceed as specified in \"Failed Open Handling\" in section 3.3.5.9");
                ModelHelper.Log(LogType.TestInfo, "Open.IsDurable is FALSE and Open.IsResilient is FALSE.");

                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedContext);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                return true;
            }

            if (Open.IsSessionExisted)
            {
                ModelHelper.Log(LogType.Requirement, "If Open.Session is not NULL, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND.");
                ModelHelper.Log(LogType.TestInfo, "Open.Session is not NULL.");
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedContext);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                return true;
            }

            // workaround for SE bug
            Condition.IsTrue(Config.IsLeasingSupported == c.IsLeasingSupported);
            Condition.IsTrue(Config.IsDirectoryLeasingSupported == c.IsDirectoryLeasingSupported);

            if (Open.IsLeaseExisted
                && modelOpenFileRequest.oplockLeaseType != OplockLeaseType.LeaseV1
                && modelOpenFileRequest.oplockLeaseType != OplockLeaseType.LeaseV2)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3. If Open.Lease is not NULL and the SMB2_CREATE_REQUEST_LEASE_V2 or the SMB2_CREATE_REQUEST_LEASE create context is not present, " +
                    "the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND.");
                ModelHelper.Log(LogType.TestInfo, "All the above conditions are met.");
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedContext);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                return true;
            }

            if (!Open.IsLeaseExisted
                && (modelOpenFileRequest.oplockLeaseType == OplockLeaseType.LeaseV1
                || modelOpenFileRequest.oplockLeaseType == OplockLeaseType.LeaseV2))
            {
                ModelHelper.Log(LogType.Requirement,
                    "4. If Open.Lease is NULL and the SMB2_CREATE_REQUEST_LEASE_V2 or the SMB2_CREATE_REQUEST_LEASE create context is present, " + 
                    "the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND.");
                ModelHelper.Log(LogType.TestInfo,
                    "Open.Lease is NULL and {0} is present.",
                    modelOpenFileRequest.oplockLeaseType == OplockLeaseType.LeaseV1 ? "SMB2_CREATE_REQUEST_LEASE" : "SMB2_CREATE_REQUEST_LEASE_V2");
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedContext);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                return true;
            }

            if (modelOpenFileRequest.oplockLeaseType == OplockLeaseType.LeaseV2
                && ModelUtility.IsSmb3xFamily(NegotiateDialect)
                && Config.IsDirectoryLeasingSupported
                && Open.IsLeaseExisted
                && !modelOpenFileRequest.isSameLeaseKey)
            {
                ModelHelper.Log(LogType.Requirement,
                    "7. If an SMB2_CREATE_REQUEST_LEASE_V2 create context is also present in the request, Connection.Dialect belongs to the SMB 3.x dialect family, " +
                    "the server supports directory leasing, Open.Lease is not NULL, and Open.Lease.LeaseKey does not match the LeaseKey provided in the SMB2_CREATE_REQUEST_LEASE_V2 create context, " +
                    "the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND and proceed as specified in \"Failed Open Handling\" in section 3.3.5.9.");
                ModelHelper.Log(LogType.TestInfo, "All the above conditions are met.");
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                return true;
            }

            if (modelOpenFileRequest.oplockLeaseType == OplockLeaseType.LeaseV1
                && (NegotiateDialect == DialectRevision.Smb21 || ModelUtility.IsSmb3xFamily(NegotiateDialect))
                && Config.IsLeasingSupported
                && Open.IsLeaseExisted
                && !modelOpenFileRequest.isSameLeaseKey)
            {
                ModelHelper.Log(LogType.Requirement, 
                    "8. If an SMB2_CREATE_REQUEST_LEASE create context is also present in the request, Connection.Dialect is \"2.100\" or belongs to the SMB 3.x dialect family, " + 
                    "the server supports leasing, Open.Lease is not NULL, and Open.Lease.LeaseKey does not match the LeaseKey provided in the SMB2_CREATE_REQUEST_LEASE create context, " + 
                    "the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND and proceed as specified in \"Failed Open Handling\" in section 3.3.5.9.");
                ModelHelper.Log(LogType.TestInfo, "All the above conditions are met.");
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                return true;
            }

            if (ModelUtility.IsSmb3xFamily(NegotiateDialect)
                && Config.IsDirectoryLeasingSupported
                && Open.IsLeaseExisted
                && Open.LeaseVersion == 2)
            {
                ModelHelper.Log(LogType.Requirement,
                    "If Connection.Dialect belongs to the SMB 3.x dialect family, the server supports directory leasing, Open.Lease is not NULL, " +
                    "and Lease.Version is 2, then the server MUST construct an SMB2_CREATE_RESPONSE_LEASE_V2 create context");
                ModelHelper.Log(LogType.TestInfo, "All the above conditions are met. So the create response should contain SMB2_CREATE_RESPONSE_LEASE_V2 create context.");
                Condition.IsTrue(leaseResponseContext == LeaseResponseContext.SMB2_CREATE_RESPONSE_LEASE_V2);
            }

            if (ModelUtility.IsSmb3xFamily(NegotiateDialect)
                && Config.IsLeasingSupported
                && Open.IsLeaseExisted
                && Open.LeaseVersion == 1)
            {
                ModelHelper.Log(LogType.Requirement,
                    "If Connection.Dialect belongs to the SMB 3.x dialect family, the server supports leasing, Open.Lease is not NULL, and Lease.Version is 1, " +
                    "then the server MUST construct an SMB2_CREATE_RESPONSE_LEASE Create Context");
                ModelHelper.Log(LogType.TestInfo, "All the above conditions are met. So the create response should contain SMB2_CREATE_RESPONSE_LEASE create context.");
                Condition.IsTrue(leaseResponseContext == LeaseResponseContext.SMB2_CREATE_RESPONSE_LEASE);
            }

            if (NegotiateDialect == DialectRevision.Smb21
                && Config.IsLeasingSupported
                && Open.IsLeaseExisted)
            {
                ModelHelper.Log(LogType.Requirement,
                    "If Connection.Dialect is \"2.100\", the server supports leasing, and Open.Lease is not NULL, " + 
                        "then the server MUST construct an SMB2_CREATE_RESPONSE_LEASE create context");
                ModelHelper.Log(LogType.TestInfo, "All the above conditions are met. So the create response should contain SMB2_CREATE_RESPONSE_LEASE create context.");
                if (Open.LeaseVersion == 1)
                    Condition.IsTrue(leaseResponseContext == LeaseResponseContext.SMB2_CREATE_RESPONSE_LEASE);
                else if (Open.LeaseVersion == 2)
                    Condition.IsTrue(leaseResponseContext == LeaseResponseContext.SMB2_CREATE_RESPONSE_LEASE_V2);
            }

            return false;
        }

        /// <summary>
        /// This method is used to verify the SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 Create Context
        /// Cover TD section: 3.3.5.9.10
        /// Return true means the message is handled by this function. 
        /// Return false means the message needs further processing.        
        /// </summary>
        private static bool Handling_SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2_CreateContext(
            ModelSmb2Status status,
            ModelOpenFileRequest modelOpenFileRequest,
            DurableHandleResponseContext durableHandleResponseContext)
        {
            ModelHelper.Log(LogType.Requirement, "3.3.5.9.10: Handling the SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 Create Context");

            if (modelOpenFileRequest.durableV1RequestContext == DurableV1RequestContext.DurableV1RequestContextExist
                || modelOpenFileRequest.durableV1ReconnectContext == DurableV1ReconnectContext.DurableV1ReconnectContextExist
                || modelOpenFileRequest.durableV2ReconnectContext != DurableV2ReconnectContext.DurableV2ReconnectContextNotExist)
            {
                ModelHelper.Log(LogType.Requirement,
                    "If the create request also includes an SMB2_CREATE_DURABLE_HANDLE_REQUEST create context, " +
                    "or an SMB2_CREATE_DURABLE_HANDLE_RECONNECT or SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context, " +
                    "the server MUST fail the create request with STATUS_INVALID_PARAMETER.");
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedContext);

                if (modelOpenFileRequest.durableV1RequestContext == DurableV1RequestContext.DurableV1RequestContextExist)
                    ModelHelper.Log(LogType.TestInfo, "SMB2_CREATE_DURABLE_HANDLE_REQUEST is included.");
                if (modelOpenFileRequest.durableV1ReconnectContext == DurableV1ReconnectContext.DurableV1ReconnectContextExist)
                    ModelHelper.Log(LogType.TestInfo, "SMB2_CREATE_DURABLE_HANDLE_RECONNECT is included.");
                if (modelOpenFileRequest.durableV2ReconnectContext != DurableV2ReconnectContext.DurableV2ReconnectContextNotExist)
                    ModelHelper.Log(LogType.TestInfo, "SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 is included.");

                Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_PARAMETER);
                return true;
            }

            if (modelOpenFileRequest.durableV2RequestContext == DurableV2RequestContext.DurableV2RequestContextExistWithoutPersistent
                && modelOpenFileRequest.oplockLeaseType == OplockLeaseType.NoOplockOrLease)
            {
                ModelHelper.Log(LogType.Requirement,
                    "If the SMB2_DHANDLE_FLAG_PERSISTENT bit is not set in the Flags field of this create context, " +
                    "if RequestedOplockLevel in the create request is not set to SMB2_OPLOCK_LEVEL_BATCH, " +
                    "and if the create request does not include a SMB2_CREATE_REQUEST_LEASE or SMB2_CREATE_REQUEST_LEASE_V2 create context  " +
                    "with a LeaseState field that includes SMB2_LEASE_HANDLE_CACHING, the server MUST ignore this create context and skip this section.");
                ModelHelper.Log(LogType.TestInfo, "All the above conditions are met. ");
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                Condition.IsTrue(durableHandleResponseContext == DurableHandleResponseContext.NONE);
                return false;
            }

            if (Open != null
                && modelOpenFileRequest.isSameClient 
                && modelOpenFileRequest.isSameCreateGuid)
            {
                ModelHelper.Log(LogType.Requirement,
                    "The server MUST locate the Open in GlobalOpenTable where Open.CreateGuid matches the CreateGuid in the SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 create context, " +
                    "and Open.ClientGuid matches the ClientGuid of the connection that received this request.");
                ModelHelper.Log(LogType.TestInfo, "The Open is found.");

                ModelHelper.Log(LogType.Requirement, 
                    "If an Open is found and the SMB2_FLAGS_REPLAY_OPERATION bit is not set in the SMB2 header, the server MUST fail the request with STATUS_DUPLICATE_OBJECTID.");
                ModelHelper.Log(LogType.TestInfo, "SMB2_FLAGS_REPLAY_OPERATION is not set.");
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_DUPLICATE_OBJECTID);
                return true;
            }

            if (Open == null)
            {
                ModelHelper.Log(LogType.Requirement,
                    "If an Open is not found, the server MUST continue the create process specified in the \"Open Execution\" Phase, and perform the following additional steps:");
                Open = new HandleModelOpen();
                ModelHelper.Log(LogType.Requirement, 
                    "In the \"Successful Open Initialization\" phase, the server MUST set Open.IsDurable to TRUE. ");
                Open.IsDurable = true;

                if (modelOpenFileRequest.durableV2RequestContext == DurableV2RequestContext.DurableV2RequestContextExistWithPersistent 
                    && Share_IsCA
                    && ServerCapabilities_PersistentBitSet)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "If the SMB2_DHANDLE_FLAG_PERSISTENT bit is set in the Flags field of the request, TreeConnect.Share.IsCA is TRUE, " + 
                        "and Connection.ServerCapabilities includes SMB2_GLOBAL_CAP_PERSISTENT_HANDLES, the server MUST set Open.IsPersistent to TRUE.");
                    ModelHelper.Log(LogType.TestInfo, "All the above conditions are met. Open.IsPersistent is set to TRUE.");
                    Open.IsPersistent = true;
                }
            }

            if (modelOpenFileRequest.durableV2RequestContext != DurableV2RequestContext.DurableV2RequestContextExistWithPersistent
                && modelOpenFileRequest.oplockLeaseType == OplockLeaseType.NoOplockOrLease)
            {
                ModelHelper.Log(LogType.Requirement,
                    "The server MUST skip the construction of the SMB2_CREATE_DURABLE_HANDLE_RESPONSE_V2 create context " +
                    "if the SMB2_DHANDLE_FLAG_PERSISTENT bit is not set in the Flags field of the request and if neither of the following conditions are met:" +
                    "\tOpen.OplockLevel is equal to SMB2_OPLOCK_LEVEL_BATCH. " + "\tOpen.Lease.LeaseState has SMB2_LEASE_HANDLE_CACHING bit set.");
                ModelHelper.Log(LogType.TestInfo, 
                    "All the above conditions are met. " + 
                    "So the server skips the construction of the SMB2_CREATE_DURABLE_HANDLE_RESPONSE_V2 create context.");
                Condition.IsTrue(durableHandleResponseContext == DurableHandleResponseContext.NONE);
            }
            else
            {
                ModelHelper.Log(LogType.Requirement,
                    "If Open.IsPersistent is TRUE, the server MUST set the SMB2_DHANDLE_FLAG_PERSISTENT bit in the Flags field. ");
                ModelHelper.Log(LogType.TestInfo, "Open.IsPersistent is {0}", Open.IsPersistent);
                if (Open.IsPersistent)
                {
                    Condition.IsTrue(durableHandleResponseContext == DurableHandleResponseContext.SMB2_CREATE_DURABLE_HANDLE_RESPONSE_V2_WITH_PERSISTENT);
                }
            }
            return false;
        }

        /// <summary>
        /// This method is used to verify the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 Create Context
        /// Cover TD section 3.3.5.9.12
        /// Return true means the message is handled by this function. 
        /// Return false means the message needs further processing.        
        /// </summary>
        private static bool Handling_SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2_CreateContext(
            ModelSmb2Status status,
            ModelOpenFileRequest modelOpenFileRequest,
            LeaseResponseContext leaseResponseContext,
            HandleConfig c)
        {
            ModelHelper.Log(LogType.Requirement, "3.3.5.9.12: Handling the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 Create Context");

            if (modelOpenFileRequest.durableV1RequestContext == DurableV1RequestContext.DurableV1RequestContextExist
                || modelOpenFileRequest.durableV1ReconnectContext == DurableV1ReconnectContext.DurableV1ReconnectContextExist
                || modelOpenFileRequest.durableV2RequestContext != DurableV2RequestContext.DurableV2RequestContextNotExist)
            {
                ModelHelper.Log(LogType.Requirement,
                    "If the create request also contains SMB2_CREATE_DURABLE_HANDLE_REQUEST context or SMB2_CREATE_DURABLE_HANDLE_RECONNECT context or " + 
                    "SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 context, the server MUST fail the request with STATUS_INVALID_PARAMETER.");
                ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedContext);

                if (modelOpenFileRequest.durableV1RequestContext == DurableV1RequestContext.DurableV1RequestContextExist)
                {
                    ModelHelper.Log(LogType.TestInfo, "SMB2_CREATE_DURABLE_HANDLE_REQUEST is included.");
                }
                if (modelOpenFileRequest.durableV1ReconnectContext == DurableV1ReconnectContext.DurableV1ReconnectContextExist)
                {
                    ModelHelper.Log(LogType.TestInfo, "SMB2_CREATE_DURABLE_HANDLE_RECONNECT is included.");
                }
                if (modelOpenFileRequest.durableV2RequestContext != DurableV2RequestContext.DurableV2RequestContextNotExist)
                {
                    ModelHelper.Log(LogType.TestInfo, "SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 is included.");
                }
                Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_PARAMETER);
                return true;
            }

            ModelHelper.Log(LogType.Requirement,
                "The server MUST look up an existing open in the GlobalOpenTable by doing a lookup with the FileId.Persistent portion of the create context.");
            ModelHelper.Log(LogType.TestInfo, "Open {0}.", Open != null ? "exists" : "does not exist");

            Condition.IsTrue(Config.Platform == c.Platform);

            if (Open == null)
            {
                ModelHelper.Log(LogType.Requirement, 
                    "If the lookup fails, the server SHOULD<276> fail the request with STATUS_OBJECT_NAME_NOT_FOUND and proceed as specified in \"Failed Open Handling\" in section 3.3.5.9.");
                ModelHelper.Log(LogType.TestTag, TestTag.InvalidIdentifier);

                if (Config.Platform == Platform.NonWindows)
                {
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is NonWindows, so the server could fail the request with other error code.");
                    Condition.IsTrue(status != ModelSmb2Status.STATUS_SUCCESS);
                }
                else if (Config.Platform == Platform.WindowsServer2012
                    && modelOpenFileRequest.durableV2ReconnectContext == DurableV2ReconnectContext.DurableV2ReconnectContextExistWithPersistent)
                {
                    // Windows 2012 fails the request, but the error code is not a fixed one.
                    Condition.IsTrue(status != ModelSmb2Status.STATUS_SUCCESS);
                }
                else
                {
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is {0}.", Config.Platform);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                }

                return true;
            }
            else
            {
                // Workaround to resolve SE bug
                Condition.IsTrue(Config.IsLeasingSupported == c.IsLeasingSupported);
                Condition.IsTrue(Config.IsDirectoryLeasingSupported == c.IsDirectoryLeasingSupported);

                if (Open.IsLeaseExisted && !modelOpenFileRequest.isSameClient)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "If Open.Lease is not NULL and Open.ClientGuid is not equal to the ClientGuid of the connection that received this request, " + 
                        "the server MUST fail the create request with STATUS_OBJECT_NAME_NOT_FOUND.");
                    ModelHelper.Log(LogType.TestInfo, "All the above conditions are met.");
                    ModelHelper.Log(LogType.TestTag, TestTag.InvalidIdentifier);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                    return true;
                }

                if (Open.IsPersistent && modelOpenFileRequest.durableV2ReconnectContext == DurableV2ReconnectContext.DurableV2ReconnectContextExistWithoutPersistent)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "If Open.IsPersistent is TRUE and the SMB2_DHANDLE_FLAG_PERSISTENT bit is not set in the Flags field of the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 Create Context, " +
                        "the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND.");
                    ModelHelper.Log(LogType.TestInfo, "All the above conditions are met.");
                    ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                    return true;
                }

                if (!modelOpenFileRequest.isSameCreateGuid)
                {
                    if (Config.Platform == Platform.WindowsServer2012
                        && modelOpenFileRequest.durableV2ReconnectContext == DurableV2ReconnectContext.DurableV2ReconnectContextExistWithPersistent)
                    {
                        // Windows 2012 fails the request, but the error code is not a fixed one.
                        Condition.IsTrue(status != ModelSmb2Status.STATUS_SUCCESS);
                    }
                    else
                    {
                        ModelHelper.Log(LogType.Requirement,
                            "If Open.CreateGuid is not equal to the CreateGuid in the request, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND.");
                        ModelHelper.Log(LogType.TestInfo, "Open.CreateGuid is not equal to the CreateGuid in the request.");
                        ModelHelper.Log(LogType.TestTag, TestTag.InvalidIdentifier);
                        Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                    }
                    return true;
                }

                if (!Open.IsDurable)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "If Open.IsDurable is FALSE and Open.IsResilient is FALSE or unimplemented, " + 
                        "the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND and proceed as specified in \"Failed Open Handling\" in section 3.3.5.9.");
                    ModelHelper.Log(LogType.TestInfo, "Open.IsDurable is FALSE and Open.IsResilient is FALSE.");
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                    return true;
                }

                // TDI, if Open.IsPersistent is true, then when reconnecting, if Open.Session is not NULL, the reconnect can still succeed.
                if (!Open.IsPersistent && Open.IsSessionExisted)
                {
                    ModelHelper.Log(LogType.Requirement, "If Open.Session is not NULL, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND. ");
                    ModelHelper.Log(LogType.TestInfo, "Open.Session is not NULL.");
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                    return true;
                }

                if ((!Open.IsLeaseExisted
                        && (modelOpenFileRequest.oplockLeaseType == OplockLeaseType.LeaseV1
                            || modelOpenFileRequest.oplockLeaseType == OplockLeaseType.LeaseV2))
                    || (Open.IsLeaseExisted
                        && modelOpenFileRequest.oplockLeaseType != OplockLeaseType.LeaseV1
                        && modelOpenFileRequest.oplockLeaseType != OplockLeaseType.LeaseV2))
                {
                    ModelHelper.Log(LogType.Requirement, 
                        "If Open.Lease is NULL and the SMB2_CREATE_REQUEST_LEASE or SMB2_CREATE_REQUEST_LEASE_V2 create context is present, " + 
                        "or if Open.Lease is NOT NULL and the SMB2_CREATE_REQUEST_LEASE or SMB2_CREATE_REQUEST_LEASE_V2 create context is not present, " + 
                        "the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND.");
                    ModelHelper.Log(LogType.TestInfo, "Open.Lease is{0} NULL.", Open.IsLeaseExisted ? " not" : "");
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    if (modelOpenFileRequest.oplockLeaseType == OplockLeaseType.LeaseV1)
                    {
                        ModelHelper.Log(LogType.TestInfo, "SMB2_CREATE_REQUEST_LEASE is present.");
                    }
                    else if (modelOpenFileRequest.oplockLeaseType == OplockLeaseType.LeaseV2)
                    {
                        ModelHelper.Log(LogType.TestInfo, "SMB2_CREATE_REQUEST_LEASE_V2 is present.");
                    }
                    else
                    {
                        ModelHelper.Log(LogType.TestInfo, "SMB2_CREATE_REQUEST_LEASE or SMB2_CREATE_REQUEST_LEASE_V2 create context is not present.");
                    }
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                    return true;
                }

                if (modelOpenFileRequest.oplockLeaseType == OplockLeaseType.LeaseV2
                    && Config.IsDirectoryLeasingSupported
                    && Open.IsLeaseExisted
                    && !modelOpenFileRequest.isSameLeaseKey)
                {
                    ModelHelper.Log(LogType.Requirement, 
                        "If an SMB2_CREATE_REQUEST_LEASE_V2 create context is also present in the request, " + 
                        "the server supports directory leasing, and Open.Lease.LeaseKey does not match the LeaseKey provided in the SMB2_CREATE_REQUEST_LEASE_V2 create context, " + 
                        "the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND and proceed as specified in \"Failed Open Handling\" in section 3.3.5.9.");
                    ModelHelper.Log(LogType.TestInfo, "All the above conditions are met.");
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                    return true;
                }

                if (modelOpenFileRequest.oplockLeaseType == OplockLeaseType.LeaseV1
                    && Config.IsLeasingSupported
                    && Open.IsLeaseExisted
                    && !modelOpenFileRequest.isSameLeaseKey)
                {
                    ModelHelper.Log(LogType.Requirement, 
                        "If an SMB2_CREATE_REQUEST_LEASE create context is also present in the request, the server supports leasing, " + 
                        "and Open.Lease.LeaseKey does not match the LeaseKey provided in the SMB2_CREATE_REQUEST_LEASE create context, " + 
                        "the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND and proceed as specified in \"Failed Open Handling\" in section 3.3.5.9.");
                    ModelHelper.Log(LogType.TestInfo, "All the above conditions are met.");
                    ModelHelper.Log(LogType.TestTag, TestTag.UnexpectedFields);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                    return true;
                }

                ModelHelper.Log(LogType.Requirement, "In the \"Response Construction\" phase:");

                if (Config.IsDirectoryLeasingSupported
                    && modelOpenFileRequest.oplockLeaseType == OplockLeaseType.LeaseV2)
                {
                    ModelHelper.Log(LogType.Requirement, "If the server supports directory leasing, and the request contains SMB2_CREATE_REQUEST_LEASE_V2 Create Context, " +
                        "then the server MUST construct an SMB2_CREATE_RESPONSE_LEASE_V2 Create Context");
                    ModelHelper.Log(LogType.TestInfo, "All the above conditions are met. So create response should contain an SMB2_CREATE_RESPONSE_LEASE_V2 Create Context.");
                    if (Open.LeaseVersion == 1)
                        Condition.IsTrue(leaseResponseContext == LeaseResponseContext.SMB2_CREATE_RESPONSE_LEASE);
                    else if (Open.LeaseVersion == 2)
                        Condition.IsTrue(leaseResponseContext == LeaseResponseContext.SMB2_CREATE_RESPONSE_LEASE_V2);
                }

                if (modelOpenFileRequest.oplockLeaseType == OplockLeaseType.LeaseV1
                    && Config.IsLeasingSupported
                    && Open.IsLeaseExisted)
                {
                    ModelHelper.Log(LogType.Requirement, 
                        "If the request contains an SMB2_CREATE_REQUEST_LEASE Create Context, " +
                        "the server supports leasing and Open.Lease is not NULL, then the server MUST construct an SMB2_CREATE_RESPONSE_LEASE create context");
                    ModelHelper.Log(LogType.TestInfo, "All the above conditions are met. So create response should contain an SMB2_CREATE_RESPONSE_LEASE Create Context.");

                    if (Open.LeaseVersion == 1)
                        Condition.IsTrue(leaseResponseContext == LeaseResponseContext.SMB2_CREATE_RESPONSE_LEASE);
                    else if (Open.LeaseVersion == 2)
                        Condition.IsTrue(leaseResponseContext == LeaseResponseContext.SMB2_CREATE_RESPONSE_LEASE_V2);
                }
            }

            return false;
        }

        #endregion
    }
}

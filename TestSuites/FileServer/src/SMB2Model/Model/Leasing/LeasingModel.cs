// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Leasing;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Xrt.Runtime;

[assembly: NativeType("System.Diagnostics.Tracing.*")]
namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model.Leasing
{
    /// <summary>
    /// This class is used to represent the lease which is abstracted from TD.
    /// </summary>
    public class Smb2Lease
    {
        /// <summary>
        /// The current state of the lease as indicated by the underlying object store.
        /// </summary>
        public uint LeaseState;

        /// <summary>
        /// The state to which the lease is breaking.
        /// </summary>
        public uint BreakToLeaseState;

        /// <summary>
        /// A Boolean that indicates if a lease break is in progress.
        /// </summary>
        public bool Breaking;

        /// <summary>
        /// A sequence number incremented by the server on every lease state change.
        /// </summary>
        public uint Epoch;

        /// <summary>
        /// A number indicating the lease version.
        /// </summary>
        public uint Version;

        public Smb2Lease(uint version = 0)
        {
            this.LeaseState = (uint)LeaseStateValues.SMB2_LEASE_NONE;
            this.BreakToLeaseState = (uint)LeaseStateValues.SMB2_LEASE_NONE;
            this.Breaking = false;
            this.Epoch = 0;
            this.Version = version;
        }

        public override string ToString()
        {
            StringBuilder outputInfo = new StringBuilder();
            outputInfo.AppendFormat("{0}: \r\n", "Smb2Lease State");
            outputInfo.AppendFormat("{0}: {1} \r\n", "LeaseState", this.LeaseState.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "BreakToLeaseState", this.BreakToLeaseState.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "Breaking", this.Breaking.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "Epoch", this.Epoch.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "Version", this.Version.ToString());

            return outputInfo.ToString();
        }
    }

    /// <summary>
    /// This models behavior of leasing management for SMB2 server
    /// Assumptions/Restrictions/Notes:
    /// 1. Does not cover directory leasing now.
    /// 2. Does not cover the underlying object store does not support leasing.
    /// 3. Only cover LeaseV1 or LeaseV2 with that RequestedOplockLevel is set to SMB2_OPLOCK_LEVEL_LEASE.
    /// 4. Does not cover the share with ShareFlags includes SHAREFLAG_FORCE_LEVELII_OPLOCK.
    /// 5. Does not cover the share with Capabilities includes SHARE_CAP_SCALEOUT.
    /// 6. Does not cover request a leasing which is breaking.
    /// </summary>
    public static class LeasingModel
    {
        /// <summary>
        /// Abstract model state.
        /// </summary>
        private static ModelState state = ModelState.Uninitialized;

        /// <summary>
        /// Configuration related to leasing.
        /// </summary>
        private static LeasingConfig config;

        /// <summary>
        /// The request which the model is handling.
        /// </summary>
        public static ModelSMB2Request request;

        /// <summary>
        /// The dialect revision after negotiation.
        /// </summary>
        public static DialectRevision negotiateDialect;

        /// <summary>
        /// The lease which the model is handling.
        /// </summary>
        public static Smb2Lease smb2Lease = null;

        #region Actions
        /// <summary>
        /// The call for reading configuration.
        /// </summary>
        [Rule(Action = "call ReadConfig(out _)")]
        public static void ReadConfigCall()
        {
            Condition.IsTrue(state == ModelState.Uninitialized);
        }

        /// <summary>
        /// The return for reading configuration.
        /// </summary>
        /// <param name="c">The configuration related to leasing.</param>
        [Rule(Action = "return ReadConfig(out c)")]
        public static void ReadConfigReturn(LeasingConfig c)
        {
            Condition.IsTrue(state == ModelState.Uninitialized);
            Condition.IsNotNull(c);

            Condition.IsTrue(c.MaxSmbVersionSupported == ModelDialectRevision.Smb2002 
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb21
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb30
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb302);
            if (c.MaxSmbVersionSupported == ModelDialectRevision.Smb2002)
            {
                Condition.IsFalse(c.IsDirectoryLeasingSupported);
                Condition.IsFalse(c.IsLeasingSupported);
            }
            else if (c.MaxSmbVersionSupported == ModelDialectRevision.Smb21)
            {
                Condition.IsFalse(c.IsDirectoryLeasingSupported);
            }

            config = c;

            state = ModelState.Initialized;
        }

        /// <summary>
        /// Setup the first client by executing these steps:
        ///     1. Negotiate 
        ///     2. SessionSetup 
        ///     3. TreeConnect
        /// </summary>
        /// <param name="maxSmbVersionClientSupported">The max smb version which the client supports.</param>
        /// <param name="clientSupportDirectoryLeasingType">This param indicates whether the client supports directory leasing.</param>
        [Rule]
        public static void SetupConnection(ModelDialectRevision maxSmbVersionClientSupported, ClientSupportDirectoryLeasingType clientSupportDirectoryLeasingType)
        {
            Condition.IsTrue(state == ModelState.Initialized);
            
            // Current leasing only supports to test leasing on the file. So limit the exploration.
            Condition.IsTrue(clientSupportDirectoryLeasingType == ClientSupportDirectoryLeasingType.ClientNotSupportDirectoryLeasing);

            request = null;
            negotiateDialect = DialectRevision.Smb2Unknown;
            smb2Lease = null;

            negotiateDialect = ModelHelper.DetermineNegotiateDialect(maxSmbVersionClientSupported, config.MaxSmbVersionSupported);

            state = ModelState.Connected;
        }

        /// <summary>
        /// Create request.
        /// </summary>
        /// <param name="connectTargetType">This flag indicates whether the target to create is a file or a directory.</param>
        /// <param name="leaseContextType">This flag indicates which type of create context for leasing to be included in create request.</param>
        /// <param name="leaseKey">Lease key type.</param>
        /// <param name="leaseState">Lease state to request.</param>
        /// <param name="leaseFlags">Lease flags to request.</param>
        /// <param name="parentLeaseKey">Parent lease key type.</param>
        [Rule]
        public static void CreateRequest(ConnectTargetType connectTargetType, LeaseContextType leaseContextType,
            LeaseKeyType leaseKey, uint leaseState, LeaseFlagsValues leaseFlags, ParentLeaseKeyType parentLeaseKey)
        {
            Condition.IsTrue(state == ModelState.Connected);
            Condition.IsNull(request);

            // Limit these three less important paramters to reduce test case numbers.
            Combination.NWise(1, connectTargetType, leaseKey, leaseState);

            // Current leasing only supports to test leasing on the file. So limit the exploration.
            Condition.In(parentLeaseKey, ParentLeaseKeyType.EmptyParentLeaseKey, ParentLeaseKeyType.ValidParentLeaseKey);
            Combination.Pairwise(leaseFlags, parentLeaseKey);

            // If Conenction.Dialect is Smb2.002 or Smb2.1, then do not test Lease V2.
            Condition.IfThen(
                negotiateDialect == DialectRevision.Smb21 || negotiateDialect == DialectRevision.Smb2002, 
                leaseContextType == LeaseContextType.LeaseV1);

            if (leaseContextType == LeaseContextType.LeaseV1)
            {
                request = new ModelCreateLeaseRequest(
                    (connectTargetType == ConnectTargetType.ConnectToDirectory) ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                    new ModelCreateRequestLease
                    {
                        LeaseKey = leaseKey,
                        LeaseState = leaseState,
                        LeaseFlags = leaseFlags
                    },
                    leaseContextType);
            }
            else
            {
                request = new ModelCreateLeaseRequest(
                    (connectTargetType == ConnectTargetType.ConnectToDirectory) ? CreateOptions_Values.FILE_DIRECTORY_FILE : CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                    new ModelCreateRequestLeaseV2
                    {
                        LeaseKey = leaseKey,
                        LeaseState = leaseState,
                        LeaseFlags = leaseFlags,
                        ParentLeaseKey = parentLeaseKey
                    },
                    leaseContextType);
            }
        }

        /// <summary>
        /// Create response.
        /// </summary>
        /// <param name="status">The status in the response.</param>
        /// <param name="isReturnLeaseContext">This flag indicates whether the create context for leasing is included in the response.</param>
        /// <param name="leaseState">The lease state included in the response.</param>
        /// <param name="leaseFlags">The lease flags included in the response.</param>
        /// <param name="c">The configuration related to leasing.</param>
        [Rule]
        public static void CreateResponse(ModelSmb2Status status, ReturnLeaseContextType returnLeaseContextType, uint leaseState, 
            LeaseFlagsValues leaseFlags, LeasingConfig c)
        {
            Condition.IsTrue(state == ModelState.Connected);

            ModelCreateLeaseRequest createRequest = ModelHelper.RetrieveOutstandingRequest<ModelCreateLeaseRequest>(ref request);

            Condition.IsTrue(config.IsDirectoryLeasingSupported == c.IsDirectoryLeasingSupported);
            Condition.IsTrue(config.IsLeasingSupported == c.IsLeasingSupported);

            if (!c.IsLeasingSupported)
            {
                ModelHelper.Log(
                    LogType.Requirement, 
                    "3.3.5.9: If the server does not support leasing and RequestedOplockLevel is set to SMB2_OPLOCK_LEVEL_LEASE, the server MUST ignore the \"RqLs\" create context.");
                ModelHelper.Log(LogType.TestInfo, "The above conditions are met.");
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                Condition.IsTrue(returnLeaseContextType == ReturnLeaseContextType.ReturnLeaseContextNotIncluded);
                return;
            }

            if ((negotiateDialect == DialectRevision.Smb21 || Smb2Utility.IsSmb3xFamily(negotiateDialect)) 
                && c.IsLeasingSupported
                && (createRequest.ContextType == LeaseContextType.LeaseV1)) // the DataLength field equals 0x20
            {
                ModelHelper.Log(
                    LogType.Requirement, 
                    "3.3.5.9: If the server supports leasing, the name of the create context is \"RqLs\" as defined in section 2.2.13.2, " + 
                    "and RequestedOplockLevel is set to SMB2_OPLOCK_LEVEL_LEASE, the server MUST do the following:");
                ModelHelper.Log(
                    LogType.Requirement,
                    "\tIf Connection.Dialect is \"2.100\" or belongs to the \"3.x\" dialect family, and the DataLength field equals 0x20, " + 
                        "the server MUST attempt to acquire a lease on the open from the underlying object store as described in section 3.3.5.9.8.");
                ModelHelper.Log(LogType.TestInfo, "All the above conditions are met.");

                // 3.3.5.9.8   Handling the SMB2_CREATE_REQUEST_LEASE Create Context
                #region Handle SMB2_CREATE_REQUEST_LEASE
                Condition.IsTrue(returnLeaseContextType == ReturnLeaseContextType.ReturnLeaseContextIncluded);
                ModelCreateRequestLease requestLease = null;

                requestLease = createRequest.LeaseContext as ModelCreateRequestLease;

                smb2Lease = new Smb2Lease();
                if (Smb2Utility.IsSmb3xFamily(negotiateDialect))
                {
                    ModelHelper.Log(LogType.Requirement, "3.3.5.9.8: If Connection.Dialect belongs to the SMB 3.x dialect family, Lease.Version is set to 1.");
                    smb2Lease.Version = 1;
                }

                if (requestLease.LeaseState != smb2Lease.LeaseState && ((~requestLease.LeaseState) & smb2Lease.LeaseState) == 0 && !smb2Lease.Breaking)
                {
                    ModelHelper.Log(
                        LogType.Requirement, 
                        "3.3.5.9.8: If the lease state requested is a superset of Lease.LeaseState and Lease.Breaking is FALSE, " + 
                        "the server MUST request promotion of the lease state from the underlying object store to the new caching state.");
                    ModelHelper.Log(LogType.TestInfo, "The above conditions are met.");
                    //If the object store succeeds this request, Lease.LeaseState MUST be set to the new caching state.
                    smb2Lease.LeaseState = requestLease.LeaseState;
                }

                // LeaseState MUST be set to Lease.LeaseState.
                Condition.IsTrue((uint)leaseState == smb2Lease.LeaseState);
                #endregion

                Condition.IsTrue(status == Smb2Status.STATUS_SUCCESS);
                return;
            }

            if (Smb2Utility.IsSmb3xFamily(negotiateDialect) 
                && c.IsLeasingSupported
                && (createRequest.ContextType == LeaseContextType.LeaseV2)) // the DataLength field equals 0x34
            {
                ModelHelper.Log(
                    LogType.Requirement, 
                    "3.3.5.9: If the server supports leasing, the name of the create context is \"RqLs\" as defined in section 2.2.13.2, " + 
                    "and RequestedOplockLevel is set to SMB2_OPLOCK_LEVEL_LEASE, the server MUST do the following:");
                ModelHelper.Log(
                    LogType.Requirement,
                    "\tIf Connection.Dialect belongs to the \"3.x\" dialect family, and the DataLength field equals 0x34, " + 
                    "the server MUST attempt to acquire a lease on the open from the underlying object store, as described in section 3.3.5.9.11.");
                ModelHelper.Log(LogType.TestInfo, "All the above conditions are met.");

                // 3.3.5.9.11   Handling the SMB2_CREATE_REQUEST_LEASE_V2 Create Context
                #region Handle SMB2_CREATE_REQUEST_LEASE_V2
                Condition.IsTrue(returnLeaseContextType == ReturnLeaseContextType.ReturnLeaseContextIncluded);
                ModelCreateRequestLeaseV2 requestLease = null;

                requestLease = createRequest.LeaseContext as ModelCreateRequestLeaseV2;

                smb2Lease = new Smb2Lease(2);

                // To reduce parameters and states, use CreateOptions instead of FileAttributes here, as we assume settings in CreateOptions and FileAtributes are consistent.
                if (createRequest.CreateOptions.HasFlag(CreateOptions_Values.FILE_DIRECTORY_FILE) 
                    && (requestLease.LeaseState & (uint)LeaseStateValues.SMB2_LEASE_WRITE_CACHING) != 0)
                {
                    ModelHelper.Log(
                        LogType.Requirement,
                        "3.3.5.9.11: If the FileAttributes field in the request indicates that this operation is on a directory and " +
                        "LeaseState includes SMB2_LEASE_WRITE_CACHING, the server MUST clear the bit SMB2_LEASE_WRITE_CACHING in the LeaseState field.");
                    ModelHelper.Log(LogType.TestInfo, "SMB2_LEASE_WRITE_CACHING is cleared.");
                    requestLease.LeaseState &= ~(uint)LeaseStateValues.SMB2_LEASE_WRITE_CACHING;
                }

                if (requestLease.LeaseState != smb2Lease.LeaseState && ((~requestLease.LeaseState) & smb2Lease.LeaseState) == 0 && !smb2Lease.Breaking)
                {
                    ModelHelper.Log(
                        LogType.Requirement,
                        "3.3.5.9.8: If the lease state requested is a superset of Lease.LeaseState and Lease.Breaking is FALSE, " +
                        "the server MUST request promotion of the lease state from the underlying object store to the new caching state.<271> " + 
                        "If the object store succeeds this request, Lease.LeaseState MUST be set to the new caching state.");
                    smb2Lease.LeaseState = requestLease.LeaseState;
                    // The server MUST increment Lease.Epoch by 1. 
                    ModelHelper.Log(LogType.TestInfo, "Lease.Epoch is incremented by 1.");
                    smb2Lease.Epoch++;
                }

                // LeaseState MUST be set to Lease.LeaseState.
                Condition.IsTrue((uint)leaseState == smb2Lease.LeaseState);

                if (requestLease.LeaseFlags.HasFlag(LeaseFlagsValues.SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET))
                {
                    ModelHelper.Log(
                        LogType.Requirement,
                        "3.3.5.9.11: If SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET bit is set in the Flags field of the request, " + 
                        "ParentLeaseKey MUST be set to the ParentLeaseKey in the request and SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET bit MUST be set in the Flags field of the response.");
                    ModelHelper.Log(LogType.TestInfo, "SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET bit is set.");
                    Condition.IsTrue(leaseFlags.HasFlag(LeaseFlagsValues.SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET));
                }
                #endregion

                Condition.IsTrue(status == Smb2Status.STATUS_SUCCESS);
                return;
            }

            Condition.IsTrue(returnLeaseContextType == ReturnLeaseContextType.ReturnLeaseContextNotIncluded);
            Condition.IsTrue(leaseState == (uint)LeaseStateValues.SMB2_LEASE_NONE);
            Condition.IsTrue(leaseFlags == LeaseFlagsValues.NONE);
            Condition.IsTrue(status == Smb2Status.STATUS_SUCCESS);
        }

        /// <summary>
        /// Lease break notification.
        /// </summary>
        /// <param name="newEpoch">NewEpoch in the notification.</param>
        /// <param name="flags">Flags in the notification.</param>
        /// <param name="currentLeaseState">Current lease state in the notification.</param>
        /// <param name="newLeaseState">New lease state in the notification.</param>
        [Rule]
        public static void OnLeaseBreakNotification(ushort newEpoch, LEASE_BREAK_Notification_Packet_Flags_Values flags,
            uint currentLeaseState, uint newLeaseState)
        {
            Condition.IsTrue(state == ModelState.Connected);
            Condition.IsNotNull(smb2Lease);

            Condition.IsTrue(newEpoch == smb2Lease.Epoch);

            Condition.IsFalse(currentLeaseState == (uint)LeaseStateValues.SMB2_LEASE_NONE);

            // 3.3.4.7   Object Store Indicates a Lease Break
            Condition.IsTrue(currentLeaseState == smb2Lease.LeaseState);
            // According to 3.3.1.4   Algorithm for Leasing in an Object Store, 
            // newLeaseState should not contain other lease state except smb2Lease.BreakToLeaseState
            Condition.IsTrue((newLeaseState | smb2Lease.BreakToLeaseState) == smb2Lease.BreakToLeaseState);

            #region Handle LeaseBreakNotification
            if (smb2Lease.LeaseState == (uint)LeaseStateValues.SMB2_LEASE_READ_CACHING)
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.4.7: If Lease.LeaseState is SMB2_LEASE_READ_CACHING, " + 
                    "the server MUST set the Flags field of the message to zero and MUST set Open.OplockState to None for all opens in Lease.LeaseOpens. " + 
                    "The server MUST set Lease.Breaking to FALSE, and the LeaseKey field MUST be set to Lease.LeaseKey. ");
                ModelHelper.Log(LogType.TestInfo, "Lease.LeaseState is SMB2_LEASE_READ_CACHING.");
                Condition.IsFalse(smb2Lease.Breaking);
                smb2Lease.LeaseState = (uint)newLeaseState;
            }
            else
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.4.7: Otherwise the server MUST set the Flags field of the message to SMB2_NOTIFY_BREAK_LEASE_FLAG_ACK_REQUIRED, " + 
                    "indicating to the client that lease acknowledgment is required.");
                ModelHelper.Log(LogType.TestInfo, "Lease.LeaseState is not SMB2_LEASE_READ_CACHING.");
                Condition.IsTrue(flags == LEASE_BREAK_Notification_Packet_Flags_Values.SMB2_NOTIFY_BREAK_LEASE_FLAG_ACK_REQUIRED);

                ModelHelper.Log(
                    LogType.Requirement,
                    "The server MUST set the CurrentLeaseState field of the message to Lease.LeaseState, set Lease.Breaking to TRUE, " + 
                    "set Lease.BreakToLeaseState to the new lease state indicated by the object store");
                Condition.IsTrue(smb2Lease.Breaking);

                // smb2Lease.LeaseState is SMB2_LEASE_WRITE_CACHING or SMB2_LEASE_HANDLE_CACHING, 
                // so when lease break, BreakToLeaseState should not contain SMB2_LEASE_WRITE_CACHING or SMB2_LEASE_HANDLE_CACHING
                Condition.IsTrue((smb2Lease.BreakToLeaseState & (uint)LeaseStateValues.SMB2_LEASE_WRITE_CACHING) == 0
                    || (smb2Lease.BreakToLeaseState & (uint)LeaseStateValues.SMB2_LEASE_HANDLE_CACHING) == 0);
            }

            #endregion
        }

        /// <summary>
        /// Lease break acknowledgment request.
        /// </summary>
        /// <param name="isValidLeaseKey">This flag indicates whether a lease key which is included in the request is valid or invalid.</param>
        /// <param name="leaseState">The lease state which is included in the request.</param>
        [Rule]
        public static void LeaseBreakAcknowledgmentRequest(ModelLeaseKeyType modelLeaseKeyType, uint leaseState)
        {
            Condition.IsTrue(state == ModelState.Connected);
            Condition.IsNull(request);

            Condition.In<uint>(leaseState, 0, 1, 3, 5, 7);
            request = new ModelLeaseBreakAckRequest(modelLeaseKeyType, leaseState);
        }

        /// <summary>
        /// Lease break response.
        /// </summary>
        /// <param name="status">The status in the response.</param>
        /// <param name="leaseState">The lease state in the response.</param>
        [Rule]
        public static void LeaseBreakResponse(ModelSmb2Status status, uint leaseState)
        {
            Condition.IsTrue(state == ModelState.Connected);
            Condition.IsTrue(request is ModelLeaseBreakAckRequest);

            ModelLeaseBreakAckRequest leaseBreakAckRequest = ModelHelper.RetrieveOutstandingRequest<ModelLeaseBreakAckRequest>(ref request);

            ModelHelper.Log(
                LogType.Requirement,
                "The server MUST locate the lease on which the client is acknowledging a lease break by performing a lookup in LeaseTable.LeaseList " +
                "using the LeaseKey of the request as the lookup key.");
            if ((leaseBreakAckRequest.modelLeaseKeyType == ModelLeaseKeyType.InvalidLeaseKey) || smb2Lease == null)
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "If no lease is found, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND.");
                ModelHelper.Log(LogType.TestInfo, "The lease is not found.");

                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_OBJECT_NAME_NOT_FOUND);
                return;
            }

            if (!smb2Lease.Breaking)
            {
                ModelHelper.Log(LogType.Requirement, "If Lease.Breaking is FALSE, the server MUST fail the request with STATUS_UNSUCCESSFUL.");
                ModelHelper.Log(LogType.TestInfo, "Lease.Breaking is FALSE");
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_UNSUCCESSFUL);
                return;
            }

            if (!(leaseBreakAckRequest.LeaseState <= smb2Lease.BreakToLeaseState))
            {
                ModelHelper.Log(LogType.Requirement, 
                    "If LeaseState is not <= Lease.BreakToLeaseState, the server MUST fail the request with STATUS_REQUEST_NOT_ACCEPTED.");
                ModelHelper.Log(LogType.TestInfo, "LeaseState is not <= Lease.BreakToLeaseState.");
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                Condition.IsTrue(status == ModelSmb2Status.STATUS_REQUEST_NOT_ACCEPTED);
                return;
            }

            ModelHelper.Log(LogType.Requirement, 
                "The server MUST set Lease.LeaseState to LeaseState received in the request, and MUST set Lease.Breaking to FALSE.");
            smb2Lease.Breaking = false;
            smb2Lease.LeaseState = leaseState;

            Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);

            ModelHelper.Log(LogType.Requirement, 
                "The server then MUST construct a lease break response using the syntax specified in section 2.2.25 with the following values:");
            ModelHelper.Log(LogType.Requirement, "\tLeaseState MUST be set to Lease.LeaseState.");
            Condition.IsTrue(leaseState == leaseBreakAckRequest.LeaseState);
        }

        /// <summary>
        /// The call for file operation request to break the specified lease state.
        /// </summary>
        /// <param name="operation">The type of file operation.</param>
        /// <param name="operatorType">The type of operator.</param>
        /// <param name="dialect">The dialect which the operator selects.</param>
        [Rule(Action = "call FileOperationToBreakLeaseRequest(operation, operatorType, dialect, out _)")]
        public static void FileOperationToBreakLeaseRequestCall(FileOperation operation, OperatorType operatorType, ModelDialectRevision dialect)
        {
            Condition.IsTrue(state == ModelState.Connected);
            Condition.IsNull(request);
            Condition.IsNotNull(smb2Lease);
            Condition.IsFalse(smb2Lease.Breaking);
            Condition.IsFalse(smb2Lease.LeaseState == (uint)LeaseStateValues.SMB2_LEASE_NONE);
            Condition.IsTrue(dialect <= config.MaxSmbVersionSupported);

            if (operatorType == OperatorType.SameClientId || operatorType == OperatorType.SameClientGuidDifferentLeaseKey)
            {
                switch (negotiateDialect)
                {
                    case DialectRevision.Smb2002:
                        Condition.IsTrue(dialect == ModelDialectRevision.Smb2002);
                        break;
                    case DialectRevision.Smb21:
                        Condition.IsTrue(dialect == ModelDialectRevision.Smb21);
                        break;
                    case DialectRevision.Smb30:
                        Condition.IsTrue(dialect == ModelDialectRevision.Smb30);
                        break;
                    case DialectRevision.Smb302:
                        Condition.IsTrue(dialect == ModelDialectRevision.Smb302);
                        break;
                    default:
                        Condition.Fail();
                        break;
                }
            }
            else
            {
                // Limit the dialect which is used by second client for se exploration.
                Condition.IsTrue(dialect == ModelDialectRevision.Smb2002);
            }

            request = new ModelFileOperationRequest(operation, operatorType, dialect);
        }

        /// <summary>
        /// The return for file operation request to break the specified lease state. If no lease state is broken, set expectedNewLeaseState to Lease.LeaseState.
        /// </summary>
        /// <param name="expectedNewLeaseState">The new lease state after the specified lease state is broken.</param>
        /// <param name="c">The configuration related to leasing.</param>
        [Rule(Action = "return FileOperationToBreakLeaseRequest(_, _, _, out c)")]
        public static void FileOperationToBreakLeaseRequestReturn(LeasingConfig c)
        {
            Condition.IsTrue(state == ModelState.Connected);

            ModelFileOperationRequest fileOperationRequest = ModelHelper.RetrieveOutstandingRequest<ModelFileOperationRequest>(ref request);

            Condition.IsFalse(negotiateDialect == DialectRevision.Smb2002);

            Condition.IsTrue(config.IsDirectoryLeasingSupported == c.IsDirectoryLeasingSupported);
            Condition.IsTrue(config.IsLeasingSupported == c.IsLeasingSupported);

            Condition.IsTrue(c.IsLeasingSupported || c.IsDirectoryLeasingSupported);
            Condition.IsTrue(config.MaxSmbVersionSupported == c.MaxSmbVersionSupported);

            // 3.3.1.4   Algorithm for Leasing in an Object Store
            smb2Lease.BreakToLeaseState = smb2Lease.LeaseState;

            if (fileOperationRequest.OptorType != OperatorType.SameClientId)
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "3.3.1.4: READ caching permits the SMB2 client to cache data read from the object. Before processing one of the following operations from a client with a different ClientId, " +
                    "the object store MUST request that the server revoke READ caching. The object store is not required to wait for acknowledgment:");

                if ((smb2Lease.LeaseState & (uint)LeaseStateValues.SMB2_LEASE_READ_CACHING) != 0
                        && ((fileOperationRequest.Operation == FileOperation.OPEN_OVERWRITE)
                            || (fileOperationRequest.Operation == FileOperation.WRITE_DATA)
                            || (fileOperationRequest.Operation == FileOperation.SIZE_CHANGED)
                            || (fileOperationRequest.Operation == FileOperation.RANGE_LOCK)))
                {
                    ModelHelper.Log(LogType.Requirement,
                        "READ caching on a file:");
                    ModelHelper.Log(LogType.Requirement,
                        "\tThe file is opened in a manner that overwrites the existing file.");
                    ModelHelper.Log(LogType.Requirement,
                        "\tData is written to the file.");
                    ModelHelper.Log(LogType.Requirement,
                        "\tThe file size is changed.");
                    ModelHelper.Log(LogType.Requirement,
                        "\tA byte range lock is requested for the file.");

                    ModelHelper.Log(LogType.TestInfo, "READ caching lease state is broken.");
                    smb2Lease.BreakToLeaseState &= ~(uint)LeaseStateValues.SMB2_LEASE_READ_CACHING;
                }
            }

            if (fileOperationRequest.OptorType != OperatorType.SameClientId)
            {
                ModelHelper.Log(
                    LogType.Requirement,
                    "WRITE caching permits the SMB2 client to cache writes and byte-range locks on an object. " + 
                    "Before processing one of the following operations, the underlying object store MUST request that the server revoke WRITE caching, " + 
                    "and the object store MUST wait for acknowledgment from the server before proceeding with the operation:");

                if ((smb2Lease.LeaseState & (uint)LeaseStateValues.SMB2_LEASE_WRITE_CACHING) != 0 
                        && fileOperationRequest.Operation != FileOperation.OPEN_SHARING_VIOLATION
                        && fileOperationRequest.Operation != FileOperation.OPEN_SHARING_VIOLATION_WITH_OVERWRITE
                        && fileOperationRequest.Operation != FileOperation.PARENT_DIR_RENAMED)
                {
                    ModelHelper.Log(
                        LogType.Requirement,
                        "The file is opened by a local application or via another protocol, " + 
                        "or opened via SMB2 without providing the same ClientId, and requested access includes any flags other than FILE_READ_ATTRIBUTES, " + 
                        "FILE_WRITE_ATTRIBUTES, and SYNCHRONIZE.");
                    ModelHelper.Log(
                        LogType.TestInfo, "WRITE caching lease state is broken.");
                    smb2Lease.BreakToLeaseState &= ~(uint)LeaseStateValues.SMB2_LEASE_WRITE_CACHING;
                }

                ModelHelper.Log(
                    LogType.Requirement,
                    "HANDLE caching permits one or more SMB2 clients to delay closing handles it holds open, " + 
                    "or to defer sending opens. Before processing one of the following operations, " + 
                    "the underlying object store MUST request that the server revoke HANDLE caching, " + 
                    "and the object store MUST wait for acknowledgment before proceeding with the operation:");
                if ((smb2Lease.LeaseState & (uint)LeaseStateValues.SMB2_LEASE_HANDLE_CACHING) != 0 
                        && (fileOperationRequest.Operation == FileOperation.OPEN_SHARING_VIOLATION
                            || fileOperationRequest.Operation == FileOperation.OPEN_SHARING_VIOLATION_WITH_OVERWRITE
                            || fileOperationRequest.Operation == FileOperation.RENAMEED || fileOperationRequest.Operation == FileOperation.DELETED
                            || fileOperationRequest.Operation == FileOperation.PARENT_DIR_RENAMED))
                {
                    ModelHelper.Log(LogType.Requirement, "HANDLE caching on a file:");
                    ModelHelper.Log(
                        LogType.Requirement,
                        "\tA file is opened with an access or share mode incompatible with opens from different ClientIds " + 
                        "or local applications as described in [MS-FSA] section 2.1.5.1.2.");
                    ModelHelper.Log(LogType.Requirement, "\tThe parent directory is being renamed.");

                    ModelHelper.Log(
                        LogType.TestInfo, "HANDLE caching lease state is broken.");
                    smb2Lease.BreakToLeaseState &= ~(uint)LeaseStateValues.SMB2_LEASE_HANDLE_CACHING;
                }
            }

            // HANDLE caching permits one or more SMB2 clients to delay closing handles it holds open, or to defer sending opens. 
            // Before processing one of the following operations, the underlying object store MUST request that the server revoke HANDLE caching, 
            // and the object store MUST wait for acknowledgment before proceeding with the operation:
            //      The parent directory is being renamed.
            if ((smb2Lease.LeaseState & (uint)LeaseStateValues.SMB2_LEASE_HANDLE_CACHING) != 0
                    && fileOperationRequest.Operation == FileOperation.PARENT_DIR_RENAMED)
            {
                smb2Lease.BreakToLeaseState &= ~(uint)LeaseStateValues.SMB2_LEASE_HANDLE_CACHING;
            }

            if (smb2Lease.BreakToLeaseState != smb2Lease.LeaseState)
            {
                if (smb2Lease.LeaseState != (uint)LeaseStateValues.SMB2_LEASE_READ_CACHING)
                {
                    // 3.3.4.7   Object Store Indicates a Lease Break
                    // The server MUST set Lease.Breaking to TRUE
                    ModelHelper.Log(LogType.TestInfo, "Lease.Breaking is set to TRUE.");
                    smb2Lease.Breaking = true;
                }

                // 3.3.4.7   Object Store Indicates a Lease Break
                if (ModelUtility.IsSmb3xFamily(c.MaxSmbVersionSupported) && smb2Lease.Version == 2)
                {
                    ModelHelper.Log(
                        LogType.Requirement,
                        "3.3.4.7: If the server implements the SMB 3.x dialect family and Lease.Version is 2, the server MUST set NewEpoch to Lease.Epoch + 1. ");
                    ModelHelper.Log(LogType.TestInfo, "The server implements the SMB 3.x dialect family and Lease.Version is 2.");
                    smb2Lease.Epoch++;
                }

                // 3.3.1.4   Algorithm for Leasing in an Object Store
                // The algorithm SHOULD support the following combinations of caching flags on a file: 
                // No caching, Read caching, Read + Write caching, Read + Handle caching, and Read + Write + Handle caching.
                if ((smb2Lease.BreakToLeaseState & (uint)LeaseStateValues.SMB2_LEASE_READ_CACHING) == 0)
                {
                    ModelHelper.Log(LogType.TestInfo, "Lease state is set to No caching.");
                    smb2Lease.BreakToLeaseState = 0;
                }
            }
        }

        /// <summary>
        /// File operation response.
        /// </summary>
        [Rule]
        public static void FileOperationToBreakLeaseResponse()
        {
            Condition.IsTrue(state == ModelState.Connected);
        }

        #endregion
    }
}

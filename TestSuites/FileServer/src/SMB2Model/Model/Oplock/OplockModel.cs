// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.Oplock;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Xrt.Runtime;

[assembly: NativeType("System.Diagnostics.Tracing.*")]
namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model.Oplock
{
    public static class OplockModel
    {
        #region State

        public static ModelState State = ModelState.Uninitialized;

        public static OplockConfig Config;

        public static ModelOplockOpen Open;

        public static bool Share_ForceLevel2Oplock;

        public static bool Share_Type_Include_STYPE_CLUSTER_SOFS;

        public static DialectRevision Connection_Dialect;

        public static ModelSMB2Request Request;

        #endregion

        #region Rules

        /// <summary>
        /// Call for loading server configuration
        /// </summary>
        [Rule(Action = "call ReadConfig(out _)")]
        public static void ReadConfigCall()
        {

            Condition.IsTrue(State == ModelState.Uninitialized);
        }

        /// <summary>
        /// Return for loading server configuration
        /// </summary>
        /// <param name="c">Server configuration related to model</param>
        [Rule(Action = "return ReadConfig(out c)")]
        public static void ReadConfigReturn(OplockConfig c)
        {

            Condition.IsTrue(State == ModelState.Uninitialized);
            Condition.IsNotNull(c);
            Condition.IsTrue(c.MaxSmbVersionSupported == ModelDialectRevision.Smb2002
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb21
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb30
                || c.MaxSmbVersionSupported == ModelDialectRevision.Smb302);
            Config = c;

            State = ModelState.Initialized;
        }

        /// <summary>
        /// Setup connection by perform following
        ///     1. Negotiate
        ///     2. SessionSetup
        ///     3. TreeConnect
        /// </summary>
        /// <param name="maxSmbVersionClientSupported">Max SMB2 dialect that client supports</param>
        /// <param name="shareFlag">Indicate if the share force level 2 oplock</param>
        /// <param name="shareType">Indicate if the share type includes STYPE_CLUSTER_SOFS</param>
        [Rule]
        public static void SetupConnection(ModelDialectRevision maxSmbVersionClientSupported, ModelShareFlag shareFlag, ModelShareType shareType)
        {

            Condition.IsTrue(State == ModelState.Initialized);

            Open = null;
            Request = null;
            Connection_Dialect = DialectRevision.Smb2Unknown;

            Connection_Dialect = ModelHelper.DetermineNegotiateDialect(maxSmbVersionClientSupported, Config.MaxSmbVersionSupported);

            Share_ForceLevel2Oplock = shareFlag == ModelShareFlag.SMB2_SHAREFLAG_FORCE_LEVELII_OPLOCK;
            Share_Type_Include_STYPE_CLUSTER_SOFS = shareType == ModelShareType.STYPE_CLUSTER_SOFS;

            State = ModelState.Connected;
        }

        /// <summary>
        /// Call to request Oplock in 1st open and operate the same file from another open
        /// </summary>
        /// <param name="requestedOplockLevel">Oplock level that requested in 1st open</param>
        /// <param name="fileOperation">File operation from another open on the same file</param>
        [Rule(Action = "call RequestOplockAndOperateFileRequest(requestedOplockLevel, fileOperation,out _, out _)")]
        public static void RequestOplockAndOperateFileCall(RequestedOplockLevel_Values requestedOplockLevel, OplockFileOperation fileOperation)
        {

            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsNull(Request);

            // <46> Section 2.2.14: Windows-based clients never use exclusive oplocks. Because there are no situations where it would
            // require an exclusive oplock where it would not also require an SMB2_OPLOCK_LEVEL_BATCH, it always requests an SMB2_OPLOCK_LEVEL_BATCH.
            Condition.IsTrue(requestedOplockLevel != RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE
                && requestedOplockLevel != RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE
                && requestedOplockLevel != RequestedOplockLevel_Values.OPLOCK_LEVEL_EXCLUSIVE);

            Request = new ModelRequestOplockAndTriggerBreakRequest(requestedOplockLevel);
        }

        /// <summary>
        /// Return to request Oplock in 1st open and operate the same file from another open
        /// </summary>
        /// <param name="grantedOplockLevel">Oplock level that granted in 1st open</param>
        /// <param name="c">Server configure</param>
        [Rule(Action = "return RequestOplockAndOperateFileRequest(_, _, out grantedOplockLevel, out c)")]
        public static void RequestOplockAndOperateFileReturn(OplockLevel_Values grantedOplockLevel, OplockConfig c)
        {

            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsNotNull(Request);

            ModelRequestOplockAndTriggerBreakRequest requestOplockAndTriggerBreakRequest = ModelHelper.RetrieveOutstandingRequest<ModelRequestOplockAndTriggerBreakRequest>(ref Request);

            Condition.IsTrue(Config.Platform == c.Platform);

            if (Share_ForceLevel2Oplock
                && (requestOplockAndTriggerBreakRequest.RequestedOplockLevel == RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH
                     || requestOplockAndTriggerBreakRequest.RequestedOplockLevel == RequestedOplockLevel_Values.OPLOCK_LEVEL_EXCLUSIVE))
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.9: If TreeConnect.Share.ForceLevel2Oplock is TRUE, " +
                    "and RequestedOplockLevel is SMB2_OPLOCK_LEVEL_BATCH or SMB2_OPLOCK_LEVEL_EXCLUSIVE, " +
                    "the server SHOULD<245> set RequestedOplockLevel to SMB2_OPLOCK_LEVEL_II.");
                if (Config.Platform != Platform.NonWindows)
                {
                    ModelHelper.Log(LogType.TestInfo, "The SUT platform is Windows.");
                    Condition.IsTrue(grantedOplockLevel == OplockLevel_Values.OPLOCK_LEVEL_II);

                    // Skip following WBN which is for pre-Win8
                    // <226> Section 3.3.5.9: Windows Vista and Windows Server 2008 do not support the SMB2_SHAREFLAG_FORCE_LEVELII_OPLOCK flag
                    // and ignore  the TreeConnect.Share.ForceLevel2Oplock value.
                }
            }

            if (ModelUtility.IsSmb3xFamily(Connection_Dialect)
                && Share_Type_Include_STYPE_CLUSTER_SOFS
                && requestOplockAndTriggerBreakRequest.RequestedOplockLevel == RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH)
            {
                ModelHelper.Log(LogType.Requirement,
                    "3.3.5.9: If Connection.Dialect belongs to the SMB 3.x dialect family TreeConnect.Share.Type includes STYPE_CLUSTER_SOFS " +
                    "and the RequestedOplockLevel is SMB2_OPLOCK_LEVEL_BATCH, the server MUST set RequestedOplockLevel to SMB2_OPLOCK_LEVEL_II.");
                // The Oplock acquisition depends on underlying object store implemention specific
                // we may not garantee level2 would be always granted
                // Only assert it's not batch
                ModelHelper.Log(LogType.TestInfo, "All the above conditions are met.");
                Condition.IsTrue(grantedOplockLevel != OplockLevel_Values.OPLOCK_LEVEL_BATCH);
            }

            Open = new ModelOplockOpen();

            ModelHelper.Log(LogType.TestInfo, "Open.OplockLevel is set to SMB2_OPLOCK_LEVEL_NONE.");
            Open.OplockLevel = OplockLevel_Values.OPLOCK_LEVEL_NONE;

            ModelHelper.Log(LogType.TestInfo, "Open.OplockState is set to None.");
            Open.OplockState = OplockState.None;

            // Acquire Oplock from underlying object

            // If the underlying object store grants the oplock,
            // then Open.OplockState is set to Held and Open.OplockLevel is set to the level of the oplock acquired.
            if (grantedOplockLevel != OplockLevel_Values.OPLOCK_LEVEL_NONE)
            {
                ModelHelper.Log(LogType.TestInfo, "Open.OplockState is set to Held.");
                Open.OplockState = OplockState.Held;
                Open.OplockLevel = grantedOplockLevel;
            }
        }

        /// <summary>
        /// Indicate oplock break
        /// </summary>
        /// <param name="acceptableAckOplockLevel">New oplock break in the notification</param>
        [Rule]
        public static void OplockBreakNotification(OPLOCK_BREAK_Notification_Packet_OplockLevel_Values acceptableAckOplockLevel)
        {
            Condition.IsTrue(State == ModelState.Connected);

            ModelHelper.Log(LogType.Requirement, "3.3.4.6: The new oplock level MUST be either SMB2_OPLOCK_LEVEL_NONE or SMB2_OPLOCK_LEVEL_II.");
            Condition.IsTrue(acceptableAckOplockLevel == OPLOCK_BREAK_Notification_Packet_OplockLevel_Values.OPLOCK_LEVEL_II
                || acceptableAckOplockLevel == OPLOCK_BREAK_Notification_Packet_OplockLevel_Values.OPLOCK_LEVEL_NONE);

            ModelHelper.Log(LogType.Requirement,
                "The server MUST locate the open by walking the GlobalOpenTable to find an entry whose Open.LocalOpen matches the one provided in the oplock break. ");
            ModelHelper.Log(LogType.Requirement,
                "If no entry is found, the break indication MUST be ignored and the server MUST complete the oplock break call with SMB2_OPLOCK_LEVEL_NONE as the new oplock level.");
            ModelHelper.Log(LogType.TestInfo, "Open should not be NULL.");
            Condition.IsNotNull(Open);

            ModelHelper.Log(LogType.TestInfo, "Open.OplockState is set to Breaking.");
            Open.OplockState = OplockState.Breaking;

        }

        /// <summary>
        /// Request of oplock break acknowledgement
        /// </summary>
        /// <param name="volatilePortion">Indicate if the open could be found by volatile portion</param>
        /// <param name="persistentPortion">Indicate if the persistent portion matches durable fileId</param>
        /// <param name="oplockLevel">Oplock level in oplock break acknowledgement request</param>
        [Rule]
        public static void OplockBreakAcknowledgementRequest(OplockVolatilePortion volatilePortion, OplockPersistentPortion persistentPortion, OplockLevel_Values oplockLevel)
        {

            Combination.Isolated(volatilePortion == OplockVolatilePortion.VolatilePortionNotFound);
            Combination.Isolated(persistentPortion == OplockPersistentPortion.PersistentNotMatchesDurableFileId);

            // Isolate the values that are not allowed
            Combination.Isolated(oplockLevel == OplockLevel_Values.OPLOCK_LEVEL_BATCH);
            Combination.Isolated(oplockLevel == OplockLevel_Values.OPLOCK_LEVEL_EXCLUSIVE);
            Combination.Isolated(oplockLevel == OplockLevel_Values.OPLOCK_LEVEL_LEASE);

            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsNull(Request);

            bool volatilePortionFound = volatilePortion == OplockVolatilePortion.VolatilePortionFound;
            bool persistentMatchesDurableFileId = persistentPortion == OplockPersistentPortion.PersistentMatchesDurableFileId;

            Request = new ModelOplockBreakAcknowledgementRequest(volatilePortionFound, persistentMatchesDurableFileId, oplockLevel);
        }

        /// <summary>
        /// Response of oplock break acknowledgement
        /// </summary>
        /// <param name="status">Status of the response</param>
        /// <param name="oplockLevel">Oplock level in oplock break acknowledgement response</param>
        /// <param name="oplockLevelOnOpen">Current oplock level on the open</param>
        [Rule]
        public static void OplockBreakResponse(ModelSmb2Status status, OPLOCK_BREAK_Response_OplockLevel_Values oplockLevel, OplockLevel_Values oplockLevelOnOpen)
        {

            Condition.IsTrue(State == ModelState.Connected);
            Condition.IsNotNull(Request);

            ModelOplockBreakAcknowledgementRequest oplockBreakAckRequest = ModelHelper.RetrieveOutstandingRequest<ModelOplockBreakAcknowledgementRequest>(ref Request);

            ModelHelper.Log(LogType.Requirement,
                "3.3.5.22.1   Processing an Oplock Acknowledgment");

            if (!oplockBreakAckRequest.VolatilePortionFound
                || !oplockBreakAckRequest.PersistentMatchesDurableFileId)
            {
                ModelHelper.Log(LogType.Requirement,
                    "Next, the server MUST locate the open on which the client is acknowledging an oplock break by performing a lookup in Session.OpenTable " + 
                    "using FileId.Volatile of the request as the lookup key. If no open is found, or if Open.DurableFileId is not equal to FileId.Persistent, " + 
                    "the server MUST fail the request with STATUS_FILE_CLOSED. ");
                ModelHelper.Log(LogType.TestTag, TestTag.InvalidIdentifier);

                if (!oplockBreakAckRequest.VolatilePortionFound)
                {
                    ModelHelper.Log(LogType.TestInfo, "Open is not found.");
                }
                if (!oplockBreakAckRequest.PersistentMatchesDurableFileId)
                {
                    ModelHelper.Log(LogType.TestInfo, "Open.DurableFileId is not equal to FileId.Persistent.");
                }
                Condition.IsTrue(status == ModelSmb2Status.STATUS_FILE_CLOSED);
                return;
            }

            // We need this to avoid Open.OplockLevel being symbolic which will result case exploration failure
            Condition.IsTrue(Open.OplockLevel == oplockLevelOnOpen);
            Condition.IsTrue(oplockLevelOnOpen == OplockLevel_Values.OPLOCK_LEVEL_NONE
                || oplockLevelOnOpen == OplockLevel_Values.OPLOCK_LEVEL_II
                || oplockLevelOnOpen == OplockLevel_Values.OPLOCK_LEVEL_BATCH);

            if (oplockBreakAckRequest.OplockLevel == OplockLevel_Values.OPLOCK_LEVEL_LEASE)
            {
                ModelHelper.Log(LogType.Requirement,
                    "If the OplockLevel in the acknowledgment is SMB2_OPLOCK_LEVEL_LEASE, the server MUST do the following:");
                ModelHelper.Log(LogType.TestInfo, "Open.OplockState is {0}.", Open.OplockState);

                if (Open.OplockState != OplockState.Breaking)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "If Open.OplockState is not Breaking, stop processing the acknowledgment, " +
                        "and send an error response with STATUS_INVALID_PARAMETER.");
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_PARAMETER);
                    return;
                }
                else
                {
                    ModelHelper.Log(LogType.Requirement,
                        "If Open.OplockState is Breaking, complete the oplock break request received from the object store as described in section 3.3.4.6, " + 
                        "with a new level SMB2_OPLOCK_LEVEL_NONE in an implementation-specific manner,<350> and set Open.OplockLevel to SMB2_OPLOCK_LEVEL_NONE, " + 
                        "and Open.OplockState to None.");
                    ModelHelper.Log(LogType.TestInfo, "Open.OplockLevel is set to SMB2_OPLOCK_LEVEL_NONE, and Open.OplockState is set to None");
                    Open.OplockLevel = OplockLevel_Values.OPLOCK_LEVEL_NONE;
                    Open.OplockState = OplockState.None;

                    // Do not assert implementation-specific manner
                    return;
                }
            }

            if ((Open.OplockLevel == OplockLevel_Values.OPLOCK_LEVEL_EXCLUSIVE
                || Open.OplockLevel == OplockLevel_Values.OPLOCK_LEVEL_BATCH)
                && (oplockBreakAckRequest.OplockLevel != OplockLevel_Values.OPLOCK_LEVEL_II && oplockBreakAckRequest.OplockLevel != OplockLevel_Values.OPLOCK_LEVEL_NONE))
            {
                ModelHelper.Log(LogType.Requirement,
                    "If Open.OplockLevel is SMB2_OPLOCK_LEVEL_EXCLUSIVE or SMB2_OPLOCK_LEVEL_BATCH, " +
                    "and if OplockLevel is not SMB2_OPLOCK_LEVEL_II or SMB2_OPLOCK_LEVEL_NONE, the server MUST do the following:");
                ModelHelper.Log(LogType.TestInfo, "Open.OplockState is {0}.", Open.OplockState);

                if (Open.OplockState != OplockState.Breaking)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "If Open.OplockState is not Breaking, stop processing the acknowledgment, " +
                        "and send an error response with STATUS_INVALID_OPLOCK_PROTOCOL.");
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_OPLOCK_PROTOCOL);
                    return;
                }
                else
                {
                    ModelHelper.Log(LogType.Requirement,
                        "If Open.OplockState is Breaking, complete the oplock break request received from the object store, " +
                        "as described in section 3.3.4.6, with a new level SMB2_OPLOCK_LEVEL_NONE in an implementation-specific manner," +
                        "<351> and set Open.OplockLevel to SMB2_OPLOCK_LEVEL_NONE and Open.OplockState to None.");
                    ModelHelper.Log(LogType.TestInfo, "Open.OplockLevel is set to SMB2_OPLOCK_LEVEL_NONE, and Open.OplockState is set to None");
                    Open.OplockLevel = OplockLevel_Values.OPLOCK_LEVEL_NONE;
                    Open.OplockState = OplockState.None;

                    // Do not assert implementation-specific manner
                    return;
                }
            }

            if (Open.OplockLevel == OplockLevel_Values.OPLOCK_LEVEL_II
                && oplockBreakAckRequest.OplockLevel != OplockLevel_Values.OPLOCK_LEVEL_NONE)
            {
                ModelHelper.Log(LogType.Requirement,
                    "If Open.OplockLevel is SMB2_OPLOCK_LEVEL_II, and if OplockLevel is not SMB2_OPLOCK_LEVEL_NONE, " + 
                    "the server MUST do the following:");
                ModelHelper.Log(LogType.TestInfo, "Open.OplockState is {0}.", Open.OplockState);

                if (Open.OplockState != OplockState.Breaking)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "If Open.OplockState is not Breaking, stop processing the acknowledgment, and send an error response with STATUS_INVALID_OPLOCK_PROTOCOL.");
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_OPLOCK_PROTOCOL);
                    return;
                }
                else
                {
                    ModelHelper.Log(LogType.Requirement,
                        "If Open.OplockState is Breaking, complete the oplock break request received from the object store, " + 
                        "as described in section 3.3.4.6, with a new level SMB2_OPLOCK_LEVEL_NONE in an implementation-specific manner," + 
                        "<352> and set Open.OplockLevel to SMB2_OPLOCK_LEVEL_NONE and Open.OplockState to None.");
                    ModelHelper.Log(LogType.TestInfo, "Open.OplockLevel is set to SMB2_OPLOCK_LEVEL_NONE, and Open.OplockState is set to None");
                    Open.OplockLevel = OplockLevel_Values.OPLOCK_LEVEL_NONE;
                    Open.OplockState = OplockState.None;

                    // Do not assert implementation-specific manner
                    return;
                }
            }

            if (oplockBreakAckRequest.OplockLevel == OplockLevel_Values.OPLOCK_LEVEL_II
                || oplockBreakAckRequest.OplockLevel == OplockLevel_Values.OPLOCK_LEVEL_NONE)
            {
                ModelHelper.Log(LogType.Requirement,
                    "If OplockLevel is SMB2_OPLOCK_LEVEL_II or SMB2_OPLOCK_LEVEL_NONE, the server MUST do the following:");
                ModelHelper.Log(LogType.TestInfo, "Open.OplockState is {0}.", Open.OplockState);

                if (Open.OplockState != OplockState.Breaking)
                {
                    ModelHelper.Log(LogType.Requirement,
                        "If Open.OplockState is not Breaking, stop processing the acknowledgment, " + 
                        "and send an error response with STATUS_INVALID_DEVICE_STATE.");
                    ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                    Condition.IsTrue(status == ModelSmb2Status.STATUS_INVALID_DEVICE_STATE);
                    return;
                }
                else
                {
                    ModelHelper.Log(LogType.Requirement,
                        "If Open.OplockState is Breaking, " +
                        "complete the oplock break request received from the object store as described in section 3.3.4.6, " +
                        "with a new level received in OplockLevel in an implementation-specific manner.<353>");

                    if (status != ModelSmb2Status.STATUS_SUCCESS)
                    {
                        ModelHelper.Log(LogType.Requirement,
                            "If the object store indicates an error, set the Open.OplockLevel to SMB2_OPLOCK_LEVEL_NONE, " +
                            "the Open.OplockState to None, and send the error response with the error code received.");
                        ModelHelper.Log(LogType.TestInfo, "Open.OplockLevel is set to SMB2_OPLOCK_LEVEL_NONE, and Open.OplockState is set to None");
                        Open.OplockLevel = OplockLevel_Values.OPLOCK_LEVEL_NONE;
                        Open.OplockState = OplockState.None;
                        return;
                    }
                    else
                    {
                        ModelHelper.Log(LogType.Requirement,
                            "If the object store indicates success, update Open.OplockLevel and Open.OplockState as follows:");
                        if (oplockBreakAckRequest.OplockLevel == OplockLevel_Values.OPLOCK_LEVEL_II)
                        {
                            ModelHelper.Log(LogType.Requirement,
                                "If OplockLevel is SMB2_OPLOCK_LEVEL_II, " +
                                "set Open.OplockLevel to SMB2_OPLOCK_LEVEL_II and Open.OplockState to Held.");
                            ModelHelper.Log(LogType.TestInfo, "Open.OplockLevel is set to OPLOCK_LEVEL_II, and Open.OplockState is set to None");
                            Open.OplockLevel = OplockLevel_Values.OPLOCK_LEVEL_II;
                            Open.OplockState = OplockState.Held;
                        }
                        if (oplockBreakAckRequest.OplockLevel == OplockLevel_Values.OPLOCK_LEVEL_NONE)
                        {
                            ModelHelper.Log(LogType.Requirement,
                                "If OplockLevel is SMB2_OPLOCK_LEVEL_NONE, " +
                                "set Open.OplockLevel to SMB2_OPLOCK_LEVEL_NONE and the Open.OplockState to None.");
                            ModelHelper.Log(LogType.TestInfo, "Open.OplockLevel is set to SMB2_OPLOCK_LEVEL_NONE, and Open.OplockState is set to None");
                            Open.OplockLevel = OplockLevel_Values.OPLOCK_LEVEL_NONE;
                            Open.OplockState = OplockState.None;
                        }
                    }
                }
            }

            ModelHelper.Log(LogType.Requirement,
                "The server then MUST construct an oplock break response using the syntax specified in section 2.2.25 with the following value:");
            ModelHelper.Log(LogType.Requirement,
                "OplockLevel MUST be set to Open.OplockLevel.");
            ModelHelper.Log(LogType.TestInfo,
                "Open.OplockLevel is {0}.", Open.OplockLevel);

            Condition.IsTrue(Open.OplockLevel == (OplockLevel_Values)oplockLevel);

            Condition.IsTrue(status == ModelSmb2Status.STATUS_SUCCESS);
        }

        #endregion

    }
}

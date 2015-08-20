// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// NT transaction model program.
    /// </summary>
    //public class ModelProgram : BaseModelProgram
    public static partial class BaseModelProgram
    {
        /// <summary>
        /// Define FID used for setting quota.
        /// </summary>
        private static int fIdUsed;

        /// <summary>
        /// Define resumeKey.
        /// </summary>
        private static string resumeKey;

        /// <summary>
        /// Set copychunkSourceFid to 0.
        /// </summary>
        private static int copyChunkSourceFid;

        /// <summary>
        /// Check Snapshots.
        /// </summary>
        /// <param name="fId">The SMB file identifier.</param>
        /// <param name="snapShots">The snapshots for the corresponding file.</param>
        /// <param name="isSucceed">It indicates whether the checking is successful or not.</param>
        [Rule]
        public static void CheckSnapshots(int fId, Set<int> snapShots, out bool isSucceed)
        {
            Condition.IsTrue((smbState != SmbState.End) && (smbState != SmbState.Closed));
            isSucceed = true;
            Condition.IsTrue(smbConnection.openedFiles.ContainsKey(fId)
                                && smbConnection.openedFiles[fId].name == Parameter.fileNames[2]);
            Condition.IsTrue(smbConnection.sentRequest.Count == 0);
            Condition.IsTrue(snapShots == new Set<int>(1, 2));
            smbConnection.openedFiles[fId].previousVersionToken = snapShots;
        }

        #region SMB_COM_NT_TRANSACTION Extensions and Clarification

        #region NT_TRANSACT_QUERY_QUOTA

        /// <summary>
        /// NT_TRANSACT_QUERY_QUOTA Request.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="sessionId">Session ID used to identify the session.</param>
        /// <param name="treeId">Tree ID used to identify the tree connection.</param>
        /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
        /// <param name="fId">The SMB file identifier of the target directory.</param>
        /// <param name="returnSingle">
        /// A bool variable, if set, indicates only a single entry is to be returned instead of filling the entire 
        /// buffer.
        /// </param>
        /// <param name="restartScan">
        /// A bool variable, if set, indicates that the scan of the quota information is to be restarted.
        /// </param>
        /// <param name="sIdListLength">
        /// Supply the length in bytes of the SidList, or 0 if there is no SidList.
        /// </param>
        /// <param name="startSidLength">
        /// Supply the length in bytes of the StartSid, or 0 if there is no StartSid.
        /// </param>
        /// <param name="startSidOffset">startSidOffset.</param>
        [Rule]
        /// Disable CA1801, because the parameter 'startSidLength' is used for interface implementation.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static void NtTransQueryQuotaRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            int fId,
            bool returnSingle,
            bool restartScan,
            int sIdListLength,
            int startSidLength,
            int startSidOffset)
        {
            Condition.IsTrue(smbConnection.openedFiles.ContainsKey(fId));
            Condition.IsTrue(fId == fIdUsed);
            Condition.IsTrue(smbConnection.openedFiles[fId].treeId == treeId);
            Condition.IsTrue(sIdListLength == 0 || startSidLength == 0);
            if (returnSingle)
            {
                Requirement.AssumeCaptured("Allowed to restart the scan of the quota information");
            }
            else
            {
                Requirement.AssumeCaptured("Don't allowed to restart the scan of the quota information");
            }
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);
            smbRequest = new NtTransactQueryQuotaRequest(
                messageId,
                sessionId,
                treeId,
                fId,
                isSigned,
                returnSingle,
                restartScan,
                sIdListLength,
                startSidLength);
            Update.UpdateRequest(smbConnection, smbRequest);
        }


        /// <summary>
        /// NT_TRANSACT_QUERY_QUOTA Response.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="sessionId">Session ID used to identify the session.</param>
        /// <param name="treeId">Tree ID used to identify the tree connection.</param>
        /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
        /// <param name="quotaUsed">The amount of quota, in bytes, used by this user.</param>
        /// <param name="messageStatus">
        /// It indicates that the status  code returned from the System Under Test (SUT) is success or failure.
        /// </param>
        [Rule]
        /// Disable CA1801, because the parameter 'quotaUsed' is used for interface implementation.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static void NtTransQueryQuotaResponse(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            int quotaUsed,
            MessageStatus messageStatus)
        {
            NtTransactQueryQuotaRequest request = null;
            Checker.CheckResponse<Microsoft.Protocol.TestSuites.Smb.NtTransactQueryQuotaRequest>(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                isSigned,
                messageStatus,
                out request,
                MessageStatus.Success);
            Condition.IsTrue(request.command == Command.NtTransactQueryQuota);

            if (messageStatus == MessageStatus.Success)
            {
                ModelHelper.CaptureRequirement(
                    9451,
                    @"[In server Response]An SMB_COM_NT_TRANSACT (section 2.2.4.8) response for an 
                    NT_TRANSACT_QUERY_QUOTA subcommand MUST be sent by a server in reply to a client 
                    NT_TRANSACT_QUERY_QUOTA subcommand request when the request is successful.");
            }

            // When Parameter initials, the SUT checks the parameter, if the SUT doesn't support quota, the Parmeter 
            // changes correspondingly.
            Condition.IsTrue(Parameter.isSupportQuota);

            Update.UpdateResponse(smbConnection, messageId);
        }


        /// <summary>
        /// NT_TRANSACT_QUERY_QUOTA Error Response.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule(Action = "ErrorResponse(messageId, messageStatus)")]
        public static void ErrorNtTransQueryQuotaResponse(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(messageStatus == MessageStatus.NetworkSessionExpired
                                || messageStatus == MessageStatus.StatusInvalidDeviceRequest
                                || messageStatus == MessageStatus.InvalidParameter);
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.NtTransactQueryQuota);
            NtTransactQueryQuotaRequest request = (NtTransactQueryQuotaRequest)smbConnection.sentRequest[messageId];

            if (messageStatus == MessageStatus.NetworkSessionExpired)
            {
                smbConnection.sessionList.Remove(request.sessionId);
                smbState = SmbState.Closed;
            }
            else if (messageStatus == MessageStatus.StatusInvalidDeviceRequest)
            {
                Condition.IsTrue(smbConnection.sessionList[request.sessionId].sessionState == SessionState.Valid);

                // When Parameter initials, the SUT checks the parameter, if the SUT doesn't support quota, the Parmeter 
                // changes correspondingly.
                Condition.IsTrue(!Parameter.isSupportQuota);
                Condition.IsTrue(Parameter.fsType == FileSystemType.Fat);

                smbState = SmbState.End;
            }
            else if (messageStatus == MessageStatus.InvalidParameter)
            {
                if (request.sidListLength == 0 && request.startSidLength == 0)
                {
                    ModelHelper.CaptureRequirement(
                        8459,
                        @"[In Application Requests Querying Quota Information,The application MUST provide:] If the 
                        application provides both an SID list and a start SID, then the client MUST fail the request 
                        with STATUS_INVALID_PARAMETER.");
                }
            }
            smbConnection.sentRequest.Remove(messageId);
        }

        #endregion NT_TRANSACT_QUERY_QUOTA

        #region NT_TRANSACT_SET_QUOTA

        /// <summary>
        /// NT_TRANSACT_SET_QUOTA Request.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="sessionId">Session ID used to identify the session.</param>
        /// <param name="treeId">Tree ID used to identify the tree connection.</param>
        /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
        /// <param name="fId">The SMB file identifier of the target directory.</param>
        /// <param name="quotaUsed">The amount of quota, in bytes, used by this user.</param>
        [Rule]
        public static void NtTransSetQuotaRequest(
            int messageId,
            int sessionId,
            int treeId,
            int fId,
            bool isSigned,
            int quotaInfo)
        {
            Condition.IsTrue(smbConnection.openedFiles.ContainsKey(fId));
            Condition.IsTrue(smbConnection.openedFiles[fId].treeId == treeId);
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);

            smbRequest = new NtTransactSetQuotaRequest(messageId, sessionId, treeId, fId, isSigned, quotaInfo);
            Update.UpdateRequest(smbConnection, smbRequest);
        }


        /// <summary>
        /// NT_TRANSACT_SET_QUOTA response.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="sessionId">Session ID used to identify the session.</param>
        /// <param name="treeId">Tree ID used to identify the tree connection.</param>
        /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        public static void NtTransSetQuotaResponse(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            MessageStatus messageStatus)
        {
            NtTransactSetQuotaRequest request = null;

            Checker.CheckResponse<Microsoft.Protocol.TestSuites.Smb.NtTransactSetQuotaRequest>(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                isSigned,
                messageStatus,
                out request,
                MessageStatus.Success);

            Condition.IsTrue(request.command == Command.NtTransactSetQuota);

            if (messageStatus == MessageStatus.Success)
            {
                ModelHelper.CaptureRequirement(
                    9463,
                    "[In sever response.]An SMB_COM_NT_TRANSACT (section 2.2.4.8) response for the " +
                    "NT_TRANSACT_SET_QUOTA subcommand MUST be sent by a server in reply to a client NT_TRANSACT_SET_QUOTA " +
                    "request when the request is successful.");
                if (request.quotaUsed < 65535 && request.quotaUsed >= 0)
                {
                    ModelHelper.CaptureRequirement(
                        5024,
                        @"[In Receiving an NT_TRANS_QUERY_QUOTA_INFO Request]Otherwise[if the entire quota information
                        fits in the response buffer], the server MUST return STATUS_SUCCESS.");
                }
            }


            // When Parameter initials, the SUT checks the parameter, if the SUT doesn't support quota, the Parmeter 
            // changes correspondingly.
            Condition.IsTrue(Parameter.isSupportQuota);
            fIdUsed = request.fId;

            Update.UpdateResponse(smbConnection, messageId);
        }


        /// <summary>
        /// NT_TRANSACT_SET_QUOTA Error Response.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule(Action = "ErrorResponse(messageId, messageStatus)")]
        public static void ErrorNtTransSetQuotaResponse(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue((messageStatus == MessageStatus.NetworkSessionExpired)
                                || messageStatus == MessageStatus.StatusInvalidDeviceRequest
                                || messageStatus == MessageStatus.StatusBufferTooSmall);
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.NtTransactSetQuota);
            NtTransactSetQuotaRequest request = (NtTransactSetQuotaRequest)smbConnection.sentRequest[messageId];

            if (messageStatus == MessageStatus.NetworkSessionExpired)
            {
                Condition.IsTrue(smbConnection.sessionList[request.sessionId].sessionState == SessionState.Valid);

                smbConnection.sessionList.Remove(request.sessionId);
                smbState = SmbState.Closed;
            }
            else if (messageStatus == MessageStatus.StatusInvalidDeviceRequest)
            {
                Condition.IsTrue(smbConnection.sessionList[request.sessionId].sessionState == SessionState.Valid);

                // When Parameter initials, the SUT checks the parameter, if the SUT doesn't support quota, the Parmeter 
                // changes correspondingly.
                Condition.IsTrue(!Parameter.isSupportQuota);
                ModelHelper.CaptureRequirement(
                    5035,
                    "[In Receiving an NT_TRANS_SET_QUOTA Request] <130> Section 3.3.5.10.3:  If the volume is capable " +
                    "of supporting disk quotas but has the feature disabled, then a STATUS_INVALID_DEVICE_REQUEST is " +
                    "returned.");
                smbState = SmbState.End;
            }
            else if (messageStatus == MessageStatus.StatusBufferTooSmall)
            {
                if (request.quotaUsed > 65535)
                {
                    ModelHelper.CaptureRequirement(
                        5032,
                        "[In Receiving an NT_TRANS_QUERY_QUOTA_INFO Request]If the entire quota information cannot fit " +
                        "in the response buffer, then the server MUST return a status of STATUS_BUFFER_TOO_SMALL.");
                }
            }
            smbConnection.sentRequest.Remove(messageId);
        }

        #endregion NT_TRANSACT_SET_QUOTA

        #region NT_TRANSACT_SET_QUOTA (Additional)

        /// <summary>
        /// NT_TRANSACT_SET_QUOTA Client Request
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="sessionId">Session ID used to identify the session.</param>
        /// <param name="treeId">Tree ID used to identify the tree connection.</param>
        /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
        /// <param name="fId">The SMB file identifier of the target directory.</param>
        /// <param name="informationLevel">
        /// It indicates that valid condition and each error condition are contained in the input conditions type.
        /// </param>
        [Rule]
        public static void NtTransSetQuotaRequestAdditional(
            int messageId,
            int sessionId,
            int treeId,
            int fId,
            bool isSigned,
            NtTransSetQuotaRequestParameter requestPara)
        {
            Condition.IsTrue(smbConnection.openedFiles.ContainsKey(fId));
            Condition.IsTrue(smbConnection.openedFiles[fId].treeId == treeId);
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);

            if (createFileAccess == 1)
            {
                // Created file is readonly.
                Condition.IsTrue(requestPara == NtTransSetQuotaRequestParameter.AccessDenied);
            }
            else
            {
                Condition.IsTrue(requestPara != NtTransSetQuotaRequestParameter.AccessDenied);
            }

            smbRequest = new NTTransactSetQuotaRequestAdditional(
                messageId,
                sessionId,
                treeId,
                fId,
                isSigned,
                requestPara);
            Update.UpdateRequest(smbConnection, smbRequest);
        }


        /// <summary>
        /// NT_TRANSACT_SET_QUOTA response.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="sessionId">Session ID used to identify the session.</param>
        /// <param name="treeId">Tree ID used to identify the tree connection.</param>
        /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        public static void NtTransSetQuotaResponseAdditional(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            MessageStatus messageStatus)
        {
            NTTransactSetQuotaRequestAdditional request = null;
            Checker.CheckResponse<Microsoft.Protocol.TestSuites.Smb.NTTransactSetQuotaRequestAdditional>(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                isSigned,
                messageStatus,
                out request,
                MessageStatus.Success);
            Condition.IsTrue(request.command == Command.NtTransactSetQuotaAdditional);
            Condition.IsTrue(request.requestPara == NtTransSetQuotaRequestParameter.Valid);
            if (messageStatus == MessageStatus.Success)
            {
                ModelHelper.CaptureRequirement(
                    9463,
                    "[In server response]An SMB_COM_NT_TRANSACT (section 2.2.4.8) response for the " +
                    "NT_TRANSACT_SET_QUOTA subcommand MUST be sent by a server in reply to a client NT_TRANSACT_SET_QUOTA " +
                    "request when the request is successful.");
                ModelHelper.CaptureRequirement(
                    2468,
                    @"The server MUST apply the quota information provided in the NT_Trans_Data block of the request
                    (see section 2.2.7.4.1). ");
            }

            // When Parameter initials, the SUT checks the parameter, if the SUT doesn't support quota, the Parmeter 
            // changes correspondingly.
            Condition.IsTrue(Parameter.isSupportQuota);
            fIdUsed = request.fId;

            Update.UpdateResponse(smbConnection, messageId);
        }


        /// <summary>
        /// NT_TRANSACT_SET_QUOTA Error Response.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        public static void ErrorNtTransSetQuotaResponseAdditional(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(messageStatus == MessageStatus.StatusInvalidHandle
                                || messageStatus == MessageStatus.AccessDenied
                                || messageStatus == MessageStatus.InvalidParameter
                                || messageStatus == MessageStatus.StatusInsuffServerResources
                                || messageStatus == MessageStatus.StatusDataError
                                || messageStatus == MessageStatus.StatusMediaWriteProtected
                                || messageStatus == MessageStatus.NetworkSessionExpired
                                || messageStatus == MessageStatus.StatusBufferTooSmall);
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.NtTransactSetQuotaAdditional);

            NTTransactSetQuotaRequestAdditional request
                = (NTTransactSetQuotaRequestAdditional)smbConnection.sentRequest[messageId];

            if (messageStatus == MessageStatus.StatusInvalidHandle)
            {
                Condition.IsTrue(request.requestPara == NtTransSetQuotaRequestParameter.FileIdErrror);
                ModelHelper.CaptureRequirement(
                    1109468,
                    @"[In server response]When SMBSTATUS code is negotiated and The Fid is invalid, the server returns 
                    STATUS_INVALID_HANDLE(0xC0000008).");
            }
            else if (messageStatus == MessageStatus.AccessDenied)
            {
                Condition.IsTrue(request.requestPara == NtTransSetQuotaRequestParameter.AccessDenied && Parameter.isSetQuotaAccessDenied);
                ModelHelper.CaptureRequirement(
                    1109470,
                    @"[In server response]When SMBSTATUS code is negotiated and Access denied, the server returns 
                    STATUS_ACCESS_DENIED(0xC0000022).");
            }
            else if (messageStatus == MessageStatus.InvalidParameter)
            {
                Condition.IsTrue(request.requestPara == NtTransSetQuotaRequestParameter.QuotaInfoError);
                ModelHelper.CaptureRequirement(
                    119473,
                    @"[In server response]When in the Xenix server implementation and A parameter is invalid, the server
                    returns STATUS_INVALID_PARAMETER(0xC000000D).");
            }
            else if (messageStatus == MessageStatus.StatusInsuffServerResources)
            {
                Condition.IsTrue(request.requestPara == NtTransSetQuotaRequestParameter.ServerOutOfResource);
                ModelHelper.CaptureRequirement(
                    1109480,
                    @"[In server response]When SMBSTATUS code is negotiated and The server is out of resources, the 
                    server returnsSTATUS_INSUFF_SERVER_RESOURCES(0xC0000205).");
            }
            else if (messageStatus == MessageStatus.StatusDataError)
            {
                Condition.IsTrue(request.requestPara == NtTransSetQuotaRequestParameter.InputOutputError);
                ModelHelper.CaptureRequirement(
                    1109488,
                    @"[In server response]When SMBSTATUS code is negotiated and Disk I/O error,the server returns 
                    STATUS_DATA_ERROR(0xC000003E).");
            }
            else if (messageStatus == MessageStatus.StatusMediaWriteProtected)
            {
                Condition.IsTrue(request.requestPara == NtTransSetQuotaRequestParameter.FsReadOnly);
                ModelHelper.CaptureRequirement(
                    1109490,
                    @"[In server response]When SMBSTATUS code is negotiated and Attempt to modify a read-only file 
                    system,the server returns STATUS_MEDIA_WRITE_PROTECTED(0xC00000A2).");
            }
            else if (messageStatus == MessageStatus.StatusInvalidDeviceRequest)
            {
                Condition.IsTrue(smbConnection.sessionList[request.sessionId].sessionState == SessionState.Valid);
                ModelHelper.CaptureRequirement(
                    5035,
                    "[In Receiving an NT_TRANS_SET_QUOTA Request] <130> Section 3.3.5.10.3: AIf the volume is capable " +
                    "of supporting disk quotas but has the feature disabled, then a STATUS_INVALID_DEVICE_REQUEST is " +
                    "returned.");
                smbState = SmbState.End;
            }
            else if (messageStatus == MessageStatus.StatusBufferTooSmall)
            {
                Condition.IsTrue(request.requestPara == NtTransSetQuotaRequestParameter.QuotaInfoError);
                ModelHelper.CaptureRequirement(
                    5023,
                    @"[In Receiving an NT_TRANS_QUERY_QUOTA_INFO Request]If the entire quota information cannot fit in 
                    the response buffer, then the server MUST return a status of STATUS_BUFFER_TOO_SMALL.");
            }
            smbConnection.sentRequest.Remove(messageId);
        }

        #endregion NT_TRANSACT_SET_QUOTA

        #region NT_TRANSACT_IOCTL

        #region FSCTL_SRV_ENUMERATE_SNAPSHOTS

        /// <summary>
        /// FSCTL_SRV_ENUMERATE_SNAPSHOTS Request.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="sessionId">Session ID used to identify the session.</param>
        /// <param name="treeId">Tree ID used to identify the tree connection.</param>
        /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
        /// <param name="fId">The SMB file identifier of the target directory.</param>
        /// <param name="maxDataCount">used to control the MaxDataCount in FSCTL_SRV_ENUMERATE_SNAPSHOTS.</param>
        [Rule]
        public static void FsctlSrvEnumSnapshotsRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            int fId,
            MaxDataCount maxDataCount)
        {
            Condition.IsTrue(smbConnection.openedFiles.ContainsKey(fId));
            Condition.IsTrue(smbConnection.openedFiles[fId].treeId == treeId);
            Condition.IsTrue(smbConnection.openedFiles[fId].previousVersionToken.Count != 0);
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);
            smbRequest = new FsctlSrvEnumSnapshotsRequest(messageId, sessionId, treeId, fId, isSigned, maxDataCount);
            Update.UpdateRequest(smbConnection, smbRequest);
        }


        /// <summary>
        /// FSCTL_SRV_ENUMERATE_SNAPSHOTS Response.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="sessionId">Session ID used to identify the session.</param>
        /// <param name="treeId">Tree ID used to identify the tree connection.</param>
        /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        /// <param name="NumberOfSnapShotsCompared">
        /// Return the comparison result of NumberOfSnapShots and NumberOfSnapShotsReturned.
        /// </param>
        [Rule]
        public static void FsctlSrvEnumSnapshotsResponse(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            MessageStatus messageStatus,
            IntegerCompare NumberOfSnapShotsCompared)
        {
            FsctlSrvEnumSnapshotsRequest request;

            Checker.CheckResponse<Microsoft.Protocol.TestSuites.Smb.FsctlSrvEnumSnapshotsRequest>(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                isSigned,
                messageStatus,
                out request,
                MessageStatus.Success);

            Condition.IsTrue(request.command == Command.FsctlSrvEnumerateSnapshots);

            if (messageStatus == MessageStatus.Success)
            {
                ModelHelper.CaptureRequirement(
                    9394,
                    "[In  Client Request Extensions]Server implementations that receive undefined FSCTL or IOCTL " +
                    "operation requests MUST either pass the operation code and data (if any) through to the underlying " +
                    "object store or fail the operation by returning STATUS_NOT_SUPPORTED.<69>");
                ModelHelper.CaptureRequirement(
                    9397,
                    "[In response. Extensions]An SMB_COM_NT_TRANSACT (section 2.2.4.8) response for an " +
                    "NT_TRANSACT_IOCTL ([MS-CIFS] section 2.2.7.2) subcommand MUST be sent by a server in reply to a " +
                    "successful NT_TRANSACT_IOCTL request.");
            }
            Condition.IsTrue(request.maxDataCount != MaxDataCount.VerySmall);

            if (request.maxDataCount == MaxDataCount.Mid)
            {
                Condition.IsTrue(NumberOfSnapShotsCompared == IntegerCompare.Smaller);
            }

            if (request.maxDataCount == MaxDataCount.VeryLarge)
            {

                Condition.IsTrue(NumberOfSnapShotsCompared == IntegerCompare.Equal);

                Requirement.AssumeCaptured(
                    @"If MaxDataCount is large enough, the adapter will return all snapshots in a 
                    FSCTLSrvEnumSnapshotsResponse");
            }

            Update.UpdateResponse(smbConnection, messageId);
        }


        /// <summary>
        /// FSCTL_SRV_ENUMERATE_SNAPSHOTS Error Response.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule(Action = "ErrorResponse(messageId, messageStatus)")]
        public static void ErrorFsctlSrvEnumSnapshotsResponse(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue((messageStatus == MessageStatus.NetworkSessionExpired)
                               || (messageStatus == MessageStatus.NotSupported)
                               || (messageStatus == MessageStatus.InvalidParameter));

            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.FsctlSrvEnumerateSnapshots);

            FsctlSrvEnumSnapshotsRequest request = (FsctlSrvEnumSnapshotsRequest)smbConnection.sentRequest[messageId];

            if (messageStatus == MessageStatus.NetworkSessionExpired)
            {
                smbConnection.sessionList.Remove(request.sessionId);
                smbState = SmbState.Closed;
            }
            else if (messageStatus == MessageStatus.NotSupported)
            {
                // Input paramater is valid.
                if (request.maxDataCount != MaxDataCount.VerySmall
                        && Parameter.isSupportNtSmb
                        && smbConnection.sessionList[request.sessionId].sessionState == SessionState.Valid)
                {
                    ModelHelper.CaptureRequirement(
                        4984,
                        @"[In Receiving an FSCTL_SRV_ENUMERATE_SNAPSHOTS Function Code]If the server does not 
                        support this operation [Receiving an FSCTL_SRV_ENUMERATE_SNAPSHOTS Function Code], then it
                        SHOULD fail the request with STATUS_NOT_SUPPORTED.");

                    if (Parameter.sutPlatform != Platform.NonWindows)
                    {
                        ModelHelper.CaptureRequirement(
                            4985,
                            @"[In Receiving an FSCTL_SRV_ENUMERATE_SNAPSHOTS Function Code]If  the server does not 
                            support this operation [Receiving an FSCTL_SRV_ENUMERATE_SNAPSHOTS Function Code], then
                            it does fail the FSCTL request with STATUS_NOT_SUPPORTED in windows.");
                    }
                }

                smbState = SmbState.End;
            }
            else if (messageStatus == MessageStatus.InvalidParameter)
            {
                Condition.IsTrue(Parameter.isSupportPreviousVersion);

                if (request.maxDataCount == MaxDataCount.VerySmall)
                {
                    ModelHelper.CaptureRequirement(
                        2439,
                        @"[In Receiving an FSCTL_SRV_ENUMERATE_SNAPSHOTS Function Code]If the MaxDataCount of the 
                        request is smaller than the size of an FSCTL_ENUMERATE_SNAPSHOTS response, then the server MUST 
                        fail the request with STATUS_INVALID_PARAMETER. ");
                    ModelHelper.CaptureRequirement(
                        9576,
                        "<72> Section 2.2.7.2.1: Windows-based Servers return a STATUS_INVALID_PARAMETER error if the " +
                        "value of MaxDataCount is less than 0x10 bytes.");
                }
                smbState = SmbState.End;
            }
            smbConnection.sentRequest.Remove(messageId);
        }

        #endregion FSCTL_SRV_ENUMERATE_SNAPSHOTS

        #region FSCTL_SRV_REQUEST_RESUME_KEY

        /// <summary>
        /// FSCTL_SRV_REQUEST_RESUME_KEY Request.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="sessionId">Session ID used to identify the session.</param>
        /// <param name="treeId">Tree ID used to identify the tree connection.</param>
        /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
        /// <param name="fId">The SMB file identifier of the target directory.</param>
        [Rule]
        public static void FsctlSrvRequestResumeKeyRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            int fId)
        {
            Condition.IsTrue(smbConnection.openedFiles.ContainsKey(fId));
            Condition.IsTrue(smbConnection.openedFiles[fId].treeId == treeId);
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);
            smbRequest = new FsctlSrvResumeKeyRequest(messageId, sessionId, treeId, fId, isSigned);
            Update.UpdateRequest(smbConnection, smbRequest);
        }


        /// <summary>
        /// FSCTL_SRV_REQUEST_RESUME_KEY Response
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="sessionId">Session ID used to identify the session.</param>
        /// <param name="treeId">Tree ID used to identify the tree connection.</param>
        /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
        /// <param name="copychunkResumeKey">The resume key generated by the SMB the SUT.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        public static void FsctlSrvRequestResumeKeyResponse(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            [Domain("ValidKeys")]string copychunkResumeKey,
            MessageStatus messageStatus)
        {
            FsctlSrvResumeKeyRequest request = null;
            Checker.CheckResponse<Microsoft.Protocol.TestSuites.Smb.FsctlSrvResumeKeyRequest>(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                isSigned,
                messageStatus,
                out request,
                MessageStatus.Success);
            Condition.IsTrue(request.command == Command.FsctlSrvRequestResumeKey);

            // When Parameter initials, the SUT checks the parameter, if the SUT doesn't support quota, the Parmeter 
            // changes correspondingly.
            Condition.IsTrue(Parameter.isSupportResumeKey);

            if (messageStatus == MessageStatus.Success)
            {
                ModelHelper.CaptureRequirement(
                    9397,
                    "[In response. Extensions]An SMB_COM_NT_TRANSACT (section 2.2.4.8) response for an " +
                    "NT_TRANSACT_IOCTL ([MS-CIFS] section 2.2.7.2) subcommand MUST be sent by a server in reply to a " +
                    "successful NT_TRANSACT_IOCTL request.");
            }

            resumeKey = copychunkResumeKey;
            ModelHelper.CaptureRequirement(
                30022,
                "[In Receiving an FSCTL_SRV_REQUEST_RESUME_KEY Function Code] If this operation [Receiving an " +
                "FSCTL_SRV_REQUEST_RESUME_KEY Function Code] is successful, then the server MUST construct an " +
                "FSCTL_SRV_CHOPYCHUNK response as specified in section 2.2.7.2.2, with the following additional " +
                "requirements: The CopychunkResumeKey field MUST be the SUT-generated value.");

            copyChunkSourceFid = request.fId;

            Update.UpdateResponse(smbConnection, messageId);
        }


        /// <summary>
        /// FSCTL_SRV_REQUEST_RESUME_KEY Error Response.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule(Action = "ErrorResponse(messageId, messageStatus)")]
        public static void ErrorFsctlrvRequestResumeKeyResponse(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue((messageStatus == MessageStatus.NetworkSessionExpired)
                                || (messageStatus == MessageStatus.NotSupported));
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.FsctlSrvRequestResumeKey);
            FsctlSrvResumeKeyRequest request = (FsctlSrvResumeKeyRequest)smbConnection.sentRequest[messageId];

            if (messageStatus == MessageStatus.NetworkSessionExpired)
            {
                smbConnection.sessionList.Remove(request.sessionId);
                smbState = SmbState.Closed;
            }
            else if (messageStatus == MessageStatus.NotSupported)
            {
                // As input paramter is valid, that the SUT still reuturn error code indicates that the SUT does not 
                // support this operation.
                if (!Parameter.isSupportResumeKey)
                {
                    ModelHelper.CaptureRequirement(
                        30023,
                        @"[In Receiving an FSCTL_SRV_REQUEST_RESUME_KEY Function Code] If this operation [Receiving an
                        FSCTL_SRV_REQUEST_RESUME_KEY Function Code] is successful, then the server MUST construct an 
                        FSCTL_SRV_CHOPYCHUNK response as specified in section 2.2.7.2.2, with the following additional 
                        requirements: If the server does not support this operation [Receiving an 
                        FSCTL_SRV_REQUEST_RESUME_KEY Function Code], then it MUST fail the request with 
                        STATUS_NOT_SUPPORTED.");
                }
                smbState = SmbState.End;
            }
            smbConnection.sentRequest.Remove(messageId);
        }

        #endregion FSCTL_SRV_REQUEST_RESUME_KEY

        #region FSCTL_SRV_COPYCHUNK

        /// <summary>
        /// FSCTL_SRV_COPYCHUNK Request.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="sessionId">Session ID used to identify the session.</param>
        /// <param name="treeId">Tree ID used to identify the tree connection.</param>
        /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
        /// <param name="fId">The target file identifier.</param>
        /// <param name="copyChunkResumeKey">The the SUT resume key for a source file.</param>
        /// <param name="sourceOffset">The offset in the source file to copy from.</param>
        /// <param name="targetOffset">The offset in the target file to copy to.</param>
        /// <param name="length">The number of bytes to copy from the source file to the target file.</param>
        [Rule]
        /// Disable CA1801, because the parameter 'sourceOffset' and 'targetOffset' is used for interface 
        /// implementation.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static void FsctlSrvCopyChunkRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            int fId,
            [Domain("ValidKeys")]string copyChunkResumeKey,
            int sourceOffset,
            int targetOffset,
            int length)
        {
            Condition.IsTrue(smbConnection.openedFiles.ContainsKey(fId));
            Condition.IsTrue(smbConnection.openedFiles.ContainsKey(copyChunkSourceFid));
            Condition.IsTrue(fId != copyChunkSourceFid);
            Condition.IsTrue(length == 0);

            Condition.IsTrue(smbConnection.openedFiles[fId].treeId == treeId);
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);

            smbRequest = new FsctlSrvCopyChunkRequest(
                messageId,
                sessionId,
                treeId,
                fId,
                isSigned,
                length,
                copyChunkResumeKey);
            if (((FsctlSrvCopyChunkRequest)smbRequest).length > Parameter.totalBytesWritten)
            {
                Condition.IsTrue((smbConnection.openedFiles[copyChunkSourceFid].accessRights & 0x01) == 1);
                Condition.IsTrue(
                    (smbConnection.openedFiles[((FsctlSrvCopyChunkRequest)smbRequest).fId].accessRights & 0x02) == 2);
            }
            Update.UpdateRequest(smbConnection, smbRequest);
        }


        /// <summary>
        /// FSCTL_SRV_COPYCHUNK Response.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="sessionId">Session ID used to identify the session.</param>
        /// <param name="treeId">Tree ID used to identify the tree connection.</param>
        /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
        /// /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        public static void FsctlSrvCopyChunkResponse(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            MessageStatus messageStatus)
        {
            FsctlSrvCopyChunkRequest request = null;

            Checker.CheckResponse<Microsoft.Protocol.TestSuites.Smb.FsctlSrvCopyChunkRequest>(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                isSigned,
                messageStatus,
                out request,
                MessageStatus.Success);
            Condition.IsTrue(request.command == Command.FsctlSrvCupychunk);

            if (messageStatus == MessageStatus.Success)
            {
                ModelHelper.CaptureRequirement(
                    9397,
                    "[In response. Extensions]An SMB_COM_NT_TRANSACT (section 2.2.4.8) response for an " +
                    "NT_TRANSACT_IOCTL ([MS-CIFS] section 2.2.7.2) subcommand MUST be sent by a server in reply to a " +
                    "successful NT_TRANSACT_IOCTL request.");
            }

            // When Parameter initials, the SUT checks the parameter, if the SUT doesn't support quota, the Parmeter 
            // changes correspondingly.
            Condition.IsTrue(Parameter.isSupportCopyChunk);

            // The Key is valid.
            Condition.IsTrue(resumeKey == request.copychunkResumeKey);
            Condition.IsTrue(smbConnection.openedFiles.ContainsKey(request.fId));
            ModelHelper.CaptureRequirement(
                2446,
                "Likewise, the target file MUST be specified by the Fid in the SMB_COM_NT_TRANSACTION request.");

            // The source file is oppend.
            Condition.IsTrue(smbConnection.openedFiles.ContainsKey(copyChunkSourceFid));

            // FILE_SHARE_READ (0x00000001) is set, which means the source file opened for read-data access.
            Condition.IsTrue((smbConnection.openedFiles[copyChunkSourceFid].accessRights & 0x01) == 1);

            // FILE_SHARE_WRITE (0x00000002) is set, which means the target file opened for write-data access.
            Condition.IsTrue((smbConnection.openedFiles[request.fId].accessRights & 0x02) == 2);

            Update.UpdateResponse(smbConnection, messageId);
        }


        /// <summary>
        /// FSCTL_SRV_COPYCHUNK Error Response.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule(Action = "ErrorResponse(messageId, messageStatus)")]
        public static void ErrorFsctlSrvCopyChunkResponse(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue((messageStatus == MessageStatus.NetworkSessionExpired)
                                || (messageStatus == MessageStatus.NotSupported)
                                || (messageStatus == MessageStatus.AccessDenied)
                                || (messageStatus == MessageStatus.ObjectNameNotFound));

            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.FsctlSrvCupychunk);
            FsctlSrvCopyChunkRequest request = (FsctlSrvCopyChunkRequest)smbConnection.sentRequest[messageId];

            if (messageStatus == MessageStatus.NetworkSessionExpired)
            {
                smbConnection.sessionList.Remove(request.sessionId);
                smbState = SmbState.Closed;
            }
            else if (messageStatus == MessageStatus.NotSupported)
            {
                if (!Parameter.isSupportCopyChunk)
                {
                    ModelHelper.CaptureRequirement(
                        30019,
                        @"[In Receiving an FSCTL_SRV_COPYCHUNK Request] If the server does not support this operation, 
                        then it MUST fail the request with STATUS_NOT_SUPPORTED.");
                }
                smbState = SmbState.End;
            }
            else if (messageStatus == MessageStatus.AccessDenied)
            {
                Condition.IsTrue(Parameter.isSupportCopyChunk);
                Condition.IsTrue(request.length <= Parameter.totalBytesWritten);
                Condition.IsTrue(resumeKey == request.copychunkResumeKey);
                if ((resumeKey == request.copychunkResumeKey)
                        && ((smbConnection.openedFiles[copyChunkSourceFid].accessRights & 0x01) != 1))
                {
                    ModelHelper.CaptureRequirement(
                        4997,
                        @"[In Receiving an FSCTL_SRV_COPYCHUNK Request]If the file represented by the resume key is not
                        opened for read-data access, then the server MUST fail the operation with 
                        STATUS_ACCESS_DENIED.");
                }

                if ((resumeKey == request.copychunkResumeKey)
                        && ((smbConnection.openedFiles[request.fId].accessRights & 0x02) != 2))
                {
                    ModelHelper.CaptureRequirement(
                        2447,
                        @"If the target file is not opened for write-data access,  then the server MUST fail the 
                        operation with STATUS_ACCESS_DENIED.");
                }

                smbState = SmbState.End;
            }
            else if (messageStatus == MessageStatus.ObjectNameNotFound)
            {
                Condition.IsTrue(smbConnection.sessionList[request.sessionId].sessionState == SessionState.Valid);
                Condition.IsTrue(Parameter.isSupportCopyChunk);
                Condition.IsTrue(request.length <= Parameter.totalBytesWritten);
                if (resumeKey != request.copychunkResumeKey)
                {
                    ModelHelper.CaptureRequirement(
                        2445,
                        @"[In Receiving an FSCTL_SRV_COPYCHUNK Request]If the copychunk resume key is not valid, or does 
                        not represent an open file, then the server MUST fail the operation with 
                        STATUS_OBJECT_NAME_NOT_FOUND. ");
                }
            }
            smbConnection.sentRequest.Remove(messageId);
        }

        #endregion FSCTL_SRV_COPYCHUNK

        #endregion

        #region NT_TRANSACT_CREATE

        /// <summary>
        /// NT_TRANSACT_CREATE request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="desiredAccess">In the model, there are 3 kinds of access right: read, write, readwrite.</param>
        /// <param name="createDisposition">The action to take if a file does or does not exist.</param>
        /// <param name="impersonationLevel">
        /// This field specifies the information given to the SUT about the client and how the SUT MUST represent,
        /// or impersonate the client.
        /// </param>
        /// <param name="fileName">This is used to represent the name of the resource.</param>
        /// <param name="shareType">The share type in the current tree connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this request.</param>
        /// <param name="isOpenByFileId">
        /// It indicates whether the FILE_OPEN_BY_FILE_ID is set in CreateOptions field of Create Request.
        /// </param>
        /// <param name="isDirectoryFile">
        /// It indicates whether the FILE_DIRECTORY_FILE and FILE_NON_DIRECTORY_FILE are set. 
        /// If true,FILE_DIRECTORY_FILE is set; else FILE_NON_DIRECTORY_FILE is set.
        /// </param>
        /// <param name="isMaximumAllowedSet">
        /// It indicates that the client set the maximum allowed in the request.
        /// </param>
        [Rule]
        public static void NtTransactCreateRequest(
            int messageId,
            int sessionId,
            int treeId,
            [Domain("ImpersonationLevel")] int impersonationLevel,
            [Domain("FileDomain")] string fileName,
            ShareType shareType,
            bool isSigned)
        {
            smbRequest = new NtTransactCreateRequest(
                messageId,
                sessionId,
                treeId,
                impersonationLevel,
                fileName,
                shareType,
                isSigned);
            Checker.CheckNtTransactCreateRequest(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                shareType,
                isSigned,
                fileName,
                smbState);
            Requirement.AssumeCaptured("Send NT_TRANSACT_CREATE request");
            Update.UpdateNTTransactCreateRequest(smbConnection, smbRequest);
        }


        /// <summary>
        /// NT_TRANSACT_CREATE response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this response.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        public static void NtTransactCreateResponse(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            MessageStatus messageStatus)
        {
            Checker.CheckNtTransactCreateResponse(smbConnection, messageId, sessionId, treeId, isSigned, messageStatus);
            Update.UpdateNTTransactCreateResponse(smbConnection, messageId);
        }


        /// <summary>
        /// NT_TRANSACT_CREATE Error Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule(Action = "ErrorResponse(messageId, messageStatus)")]
        public static void ErrorNtTransactCreate(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(messageStatus == MessageStatus.NetworkSessionExpired
                                || messageStatus == MessageStatus.BadImpersonationLevel);
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.NtTransactCreate);
            NtTransactCreateRequest request = (NtTransactCreateRequest)smbConnection.sentRequest[messageId];

            if (messageStatus == MessageStatus.NetworkSessionExpired)
            {
                smbConnection.sessionList.Remove(request.sessionId);
                smbState = SmbState.Closed;
            }

            if (messageStatus == MessageStatus.BadImpersonationLevel)
            {
                Condition.IsTrue(request.impersonationLevel > 3 || request.impersonationLevel < 0);
                ModelHelper.CaptureRequirement(
                    9560,
                    @"<65> Section 2.2.7.1: Windows-based servers fail the request with STATUS_BAD_IMPERSONATION_LEVEL 
                    if the impersonation level is not one of SECURITY_ANONYMOUS, SECURITY_IDENTIFICATION, 
                    SECURITY_IMPERSONATION, or SECURITY_DELEGATION.");
            }

            smbConnection.sentRequest.Remove(messageId);
        }
        #endregion

        #region Data Domain

        /// <summary>
        /// This is the domain of abstract valid keys.
        /// </summary>
        public static IEnumerable<string> ValidKeys()
        {
            yield return "Key1";
            yield return "Key2";
        }

        #endregion

        #endregion

        #region FSCC model

        /// <summary>
        /// FSCC_FSCTL_Name Request
        /// </summary>
        /// <param name="messageId"> Message ID used to identify the message.</param>
        /// <param name="sessionId"> Session ID used to identify the session.</param>
        /// <param name="treeId"> Tree ID used to identify the tree connection.</param>
        /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
        /// <param name="fid"> The target file identifier. </param>
        /// <param name="copychunkResumeKey">The server resume key for a source file.</param>
        /// <param name="sourceOffset"> The offset in the source file to copy from. </param>
        /// <param name="targetOffset"> The offset in the target file to copy to. </param>
        /// <param name="length"> The number of bytes to copy from the source file to the target file.</param>
        [Rule]
        public static void FSCCFSCTLNameRequest(int messageId,
                                                int sessionId,
                                                int treeId,
                                                bool isSigned,
                                                int fid,
                                                FSCCFSCTLName fsctlName)
        {
            Condition.IsTrue(smbConnection.openedFiles.ContainsKey(fid));
            Condition.IsTrue(smbConnection.openedFiles[fid].treeId == treeId);
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);

            smbRequest = new FSCTLNameRequest(messageId, sessionId, treeId, fid, isSigned, fsctlName);

            Update.UpdateRequest(smbConnection, smbRequest);
        }

        /// <summary>
        /// FSCC_FSCTL_Name Response
        /// </summary>
        /// <param name="messageId"> Message ID used to identify the message.</param>
        /// <param name="sessionId"> Session ID used to identify the session.</param>
        /// <param name="treeId"> Tree ID used to identify the tree connection.</param>
        /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
        /// /// <param name="messageStatus"> Indicates the status code returned from server, success or fail.</param>
        [Rule]
        public static void FSCCFSCTLNameResponse(int messageId,
                                                 int sessionId,
                                                 int treeId,
                                                 bool isSigned,
                                                 MessageStatus messageStatus)
        {
            FSCTLNameRequest request;

            Checker.CheckResponse<Microsoft.Protocol.TestSuites.Smb.FSCTLNameRequest>(smbConnection, messageId, sessionId, treeId, isSigned, messageStatus, out request,
                MessageStatus.Success);

            Condition.IsTrue(request.command == Command.FSCC_FSCTL_NAME);
            Condition.IsTrue(smbConnection.openedFiles.ContainsKey(request.fid));

            if (request.fsctlName == FSCCFSCTLName.FSCTL_CREATE_OR_GET_OBJECT_ID)
            {
                Requirement.AssumeCaptured("FSCTL Name is FSCTL_CREATE_OR_GET_OBJECT_ID");
            }
            else if (request.fsctlName == FSCCFSCTLName.FSCTL_DELETE_OBJECT_ID)
            {
                Requirement.AssumeCaptured("FSCTL Name is FSCTL_DELETE_OBJECT_ID");
            }
            else if (request.fsctlName == FSCCFSCTLName.FSCTL_FILESYSTEM_GET_STATISTICS)
            {
                Requirement.AssumeCaptured("FSCTL Name is FSCTL_FILESYSTEM_GET_STATISTICS");
            }
            else if (request.fsctlName == FSCCFSCTLName.FSCTL_GET_COMPRESSION)
            {
                Requirement.AssumeCaptured("FSCTL Name is FSCTL_GET_COMPRESSION");
            }
            else if (request.fsctlName == FSCCFSCTLName.FSCTL_GET_NTFS_VOLUME_DATA)
            {
                Requirement.AssumeCaptured("FSCTL Name is FSCTL_GET_NTFS_VOLUME_DATA");
            }
            else if (request.fsctlName == FSCCFSCTLName.FSCTL_IS_PATHNAME_VALID)
            {
                Requirement.AssumeCaptured("FSCTL Name is FSCTL_IS_PATHNAME_VALID");
            }
            else if (request.fsctlName == FSCCFSCTLName.FSCTL_READ_FILE_USN_DATA)
            {
                Requirement.AssumeCaptured("FSCTL Name is FSCTL_READ_FILE_USN_DATA");
            }
            else if (request.fsctlName == FSCCFSCTLName.FSCTL_SET_COMPRESSION)
            {
                Requirement.AssumeCaptured("FSCTL Name is FSCTL_SET_COMPRESSION");
            }
            else if (request.fsctlName == FSCCFSCTLName.FSCTL_SET_SPARSE)
            {
                Requirement.AssumeCaptured("FSCTL Name is FSCTL_SET_SPARSE");
            }
            else if (request.fsctlName == FSCCFSCTLName.FSCTL_SET_ZERO_DATA)
            {
                Requirement.AssumeCaptured("FSCTL Name is FSCTL_SET_ZERO_DATA");
            }
            else if (request.fsctlName == FSCCFSCTLName.FSCTL_SET_ZERO_ON_DEALLOCATION)
            {
                Requirement.AssumeCaptured("FSCTL Name is FSCTL_SET_ZERO_ON_DEALLOCATION");
            }
            else if (request.fsctlName == FSCCFSCTLName.FSCTL_WRITE_USN_CLOSE_RECORD)
            {
                Requirement.AssumeCaptured("FSCTL Name is FSCTL_WRITE_USN_CLOSE_RECORD");
            }

            Update.UpdateResponse(smbConnection, messageId);
        }

        #endregion
    }
}

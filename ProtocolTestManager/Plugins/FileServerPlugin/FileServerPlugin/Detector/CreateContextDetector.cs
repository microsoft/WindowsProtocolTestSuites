// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestManager.FileServerPlugin
{
    /// <summary>
    /// Detector to detect create context 
    /// </summary>
    public partial class FSDetector
    {
        /// <summary>
        /// To check the create contexts.
        /// </summary>
        /// <param name="sharename">The share name</param>
        /// <param name="info">The detection information</param>
        /// <returns>Returns a DetectResult instance</returns>
        public DetectResult CheckCreateContexts_AppInstanceId(string sharename, ref DetectionInfo info)
        {
            logWriter.AddLog(LogLevel.Information, "===== Detecting create context AppInstanceId =====");
            using (Smb2Client clientForInitialOpen = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)), 
                clientForReOpen = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                ulong messageIdInitial;
                ulong sessionIdInitial;
                uint treeIdInitial;
                logWriter.AddLog(LogLevel.Information, "Start first client by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT");
                ConnectToShare(sharename, info, clientForInitialOpen, out messageIdInitial, out sessionIdInitial, out treeIdInitial);

                #region client 1 connect to server
                Guid appInstanceId = Guid.NewGuid();
                FILEID fileIdInitial;
                CREATE_Response createResponse;
                Packet_Header header;
                Smb2CreateContextResponse[] serverCreateContexts;
                string fileName = "AppInstanceId_" + Guid.NewGuid() + ".txt";
                logWriter.AddLog(LogLevel.Information, "The first client opens a file with SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 and SMB2_CREATE_APP_INSTANCE_ID.");
                uint status = clientForInitialOpen.Create(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageIdInitial++,
                    sessionIdInitial,
                    treeIdInitial,
                    fileName,
                    AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                    ShareAccess_Values.NONE,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                    CreateDisposition_Values.FILE_OPEN_IF,
                    File_Attributes.NONE,
                    ImpersonationLevel_Values.Impersonation,
                    SecurityFlags_Values.NONE,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                    new Smb2CreateContextRequest[] { 
                        new Smb2CreateDurableHandleRequestV2
                        {
                             CreateGuid = Guid.NewGuid()
                        },
                        new Smb2CreateAppInstanceId
                        {
                             AppInstanceId = appInstanceId
                        }
                    },
                    out fileIdInitial,
                    out serverCreateContexts,
                    out header,
                    out createResponse,
                    0);
                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("Create", header.Status);
                    return DetectResult.UnSupported;
                }
                #endregion

                #region client 2 connect to server

                ulong messageId;
                ulong sessionId;
                uint treeId;
                logWriter.AddLog(LogLevel.Information, "Start second client by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT");
                ConnectToShare(sharename, info, clientForReOpen, out messageId, out sessionId, out treeId);

                try
                {
                    // Test if a second client can close the previous open.
                    FILEID fileId;
                    logWriter.AddLog(LogLevel.Information, "The second client opens the same file with SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 and same AppInstanceId.");
                    status = clientForReOpen.Create(
                        1,
                        1,
                        info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                        messageId++,
                        sessionId,
                        treeId,
                        fileName,
                        AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                        ShareAccess_Values.NONE,
                        CreateOptions_Values.FILE_NON_DIRECTORY_FILE| CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                        CreateDisposition_Values.FILE_OPEN_IF,
                        File_Attributes.NONE,
                        ImpersonationLevel_Values.Impersonation,
                        SecurityFlags_Values.NONE,
                        RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                        new Smb2CreateContextRequest[] { 
                            new Smb2CreateDurableHandleRequestV2
                            {
                                 CreateGuid = Guid.NewGuid()
                            },
                            new Smb2CreateAppInstanceId
                            {
                                 AppInstanceId = appInstanceId
                            }
                        },
                        out fileId,
                        out serverCreateContexts,
                        out header,
                        out createResponse,
                        0);
                }
                catch (Exception ex)
                {
                    logWriter.AddLog(LogLevel.Information, "Create failed, reason: " + ex.Message);
                    return DetectResult.UnSupported;
                }

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("Create", header.Status);
                    return DetectResult.UnSupported;
                }
                #endregion

                // Write data using the first client
                // If AppInstanceId is supported, then the open should be closed.
                WRITE_Response writeResponse;
                logWriter.AddLog(LogLevel.Information, "The first client writes to the file to check if the open is closed");
                status = clientForInitialOpen.Write(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageIdInitial++,
                    sessionIdInitial,
                    treeIdInitial,
                    0,
                    fileIdInitial,
                    Channel_Values.CHANNEL_NONE,
                    WRITE_Request_Flags_Values.None,
                    new byte[0],
                    Encoding.ASCII.GetBytes("AppInstanceId"),
                    out header,
                    out writeResponse,
                    0);

                DetectResult result = DetectResult.UnSupported;                
                if (status == Smb2Status.STATUS_FILE_CLOSED)
                {
                    result = DetectResult.Supported;
                    logWriter.AddLog(LogLevel.Information, "The open is closed. So Create context AppInstanceId is supported");
                }
                else
                {
                    logWriter.AddLog(LogLevel.Information, "The open is not closed. So Create context AppInstanceId is not supported");
                }
                return result;   
            }
        }


        public DetectResult CheckCreateContexts_HandleV1BatchOplock(string sharename, DialectRevision smb2Dialect, ref DetectionInfo info)
        {
            logWriter.AddLog(LogLevel.Information, "===== Detecting create context HandleV1 with Batch oplock =====");
            using (Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                ulong messageId;
                ulong sessionId;
                uint treeId;
                ConnectToShare(sharename, info, client, out messageId, out sessionId, out treeId);

                Packet_Header header;

                #region Create
                FILEID fileId;
                Smb2CreateContextResponse[] serverCreateContexts = null;
                string fileName = "DurableHandleV1BatchOplock_" + Guid.NewGuid() + ".txt";
                CREATE_Response createResponse;
                logWriter.AddLog(LogLevel.Information, "Client opens file with a durable handle v1 and batch oplock");
                uint status = client.Create(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    fileName,
                    AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                    ShareAccess_Values.NONE,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                    CreateDisposition_Values.FILE_OPEN_IF,
                    File_Attributes.NONE,
                    ImpersonationLevel_Values.Impersonation,
                    SecurityFlags_Values.NONE,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH,
                    new Smb2CreateContextRequest[] 
                    { 
                        new Smb2CreateDurableHandleRequest
                        {
                             DurableRequest = Guid.Empty,
                        },
                    },
                    out fileId,
                    out serverCreateContexts,
                    out header,
                    out createResponse,
                    0);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("Create durable handle v1 with batch oplock", header.Status);
                    return DetectResult.UnSupported;
                }

                #endregion

                #region Close

                CLOSE_Response closeResponse;
                client.Close(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    fileId,
                    Flags_Values.NONE,
                    out header,
                    out closeResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("CLOSE", header.Status);
                }

                #endregion

                if (serverCreateContexts != null)
                {
                    foreach (Smb2CreateContextResponse ctx in serverCreateContexts)
                    {
                        if (ctx is Smb2CreateDurableHandleResponse)
                        {
                            logWriter.AddLog(LogLevel.Information, "Create context HandleV1 with Batch oplock is supported");
                            return DetectResult.Supported;
                        }
                    }
                }



                logWriter.AddLog(LogLevel.Information,
                    "The returned Create response doesn't contain handle v1 context. So Create context HandleV1 with Batch oplock is not supported");
                return DetectResult.UnSupported;
            }
        }

        public DetectResult CheckCreateContexts_LeaseV1(string sharename, ref DetectionInfo info)
        {
            logWriter.AddLog(LogLevel.Information, "===== Detecting create context LeaseV1 =====");
            using (Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                ulong messageId;
                ulong sessionId;
                uint treeId;
                ConnectToShare(sharename, info, client, out messageId, out sessionId, out treeId);

                Packet_Header header;

                #region Create

                FILEID fileId;
                Smb2CreateContextResponse[] serverCreateContexts = null;
                string fileName = "LeaseV1_" + Guid.NewGuid() + ".txt";
                CREATE_Response createResponse;
                logWriter.AddLog(LogLevel.Information, "Client opens file with a leaseV1 context");
                client.Create(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    fileName,
                    AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                    ShareAccess_Values.NONE,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                    CreateDisposition_Values.FILE_OPEN_IF,
                    File_Attributes.NONE,
                    ImpersonationLevel_Values.Impersonation,
                    SecurityFlags_Values.NONE,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                    new Smb2CreateContextRequest[] 
                    { 
                        new Smb2CreateRequestLease
                        {
                            LeaseKey = Guid.NewGuid(),
                            LeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING,
                        }
                    },
                    out fileId,
                    out serverCreateContexts,
                    out header,
                    out createResponse,
                    0);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("Create with lease v1", header.Status);
                    return DetectResult.UnSupported;
                }

                #endregion

                #region Close

                CLOSE_Response closeResponse;
                client.Close(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    fileId,
                    Flags_Values.NONE,
                    out header,
                    out closeResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("CLOSE", header.Status);
                }

                #endregion

                if (serverCreateContexts != null)
                {
                    foreach (Smb2CreateContextResponse ctx in serverCreateContexts)
                    {
                        if (ctx is Smb2CreateResponseLease)
                        {
                            logWriter.AddLog(LogLevel.Information, "Create context LeaseV1 is supported");
                            return DetectResult.Supported;
                        }
                    }
                }

                logWriter.AddLog(LogLevel.Information, "The returned Create response doesn't contain lease v1 context. So Create context LeaseV1 is not supported");
                return DetectResult.UnSupported;
            }
        }

        public DetectResult CheckCreateContexts_LeaseV2(string sharename, ref DetectionInfo info)
        {
            logWriter.AddLog(LogLevel.Information, "===== Detecting create context LeaseV2 =====");
            using (Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                ulong messageId;
                ulong sessionId;
                uint treeId;
                ConnectToShare(sharename, info, client, out messageId, out sessionId, out treeId);

                Packet_Header header;

                #region Create

                FILEID fileId;
                Smb2CreateContextResponse[] serverCreateContexts = null;
                string fileName = "LeaseV2_" + Guid.NewGuid() + ".txt";
                CREATE_Response createResponse;
                logWriter.AddLog(LogLevel.Information, "Client opens file with a leaseV2 context");
                client.Create(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    fileName,
                    AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                    ShareAccess_Values.NONE,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                    CreateDisposition_Values.FILE_OPEN_IF,
                    File_Attributes.NONE,
                    ImpersonationLevel_Values.Impersonation,
                    SecurityFlags_Values.NONE,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                    new Smb2CreateContextRequest[] 
                    { 
                        new Smb2CreateRequestLeaseV2
                        {
                            LeaseKey = Guid.NewGuid(),
                            LeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING,
                        }
                    },
                    out fileId,
                    out serverCreateContexts,
                    out header,
                    out createResponse,
                    0);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("Create with lease v2", header.Status);
                    return DetectResult.UnSupported;
                }

                #endregion

                #region Close

                CLOSE_Response closeResponse;
                client.Close(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    fileId,
                    Flags_Values.NONE,
                    out header,
                    out closeResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("CLOSE", header.Status);
                }

                #endregion

                if (serverCreateContexts != null)
                {
                    foreach (Smb2CreateContextResponse ctx in serverCreateContexts)
                    {
                        if (ctx is Smb2CreateResponseLeaseV2)
                        {
                            logWriter.AddLog(LogLevel.Information, "Create context LeaseV2 is supported");
                            return DetectResult.Supported;
                        }
                    }
                }

                logWriter.AddLog(LogLevel.Information, "The returned Create response doesn't contain lease v2 context. So Create context LeaseV2 is not supported");
                return DetectResult.UnSupported;
            }
        }

        public DetectResult CheckCreateContexts_HandleV1LeaseV1(string sharename, ref DetectionInfo info)
        {
            logWriter.AddLog(LogLevel.Information, "===== Detecting create context HandleV1 with LeaseV1 =====");

            using (Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                ulong messageId;
                ulong sessionId;
                uint treeId;
                ConnectToShare(sharename, info, client, out messageId, out sessionId, out treeId);

                Packet_Header header;

                #region Create

                FILEID fileId;
                Smb2CreateContextResponse[] serverCreateContexts = null;
                string fileName = "DurableHandleV1LeaseV1_" + Guid.NewGuid() + ".txt";
                CREATE_Response createResponse;
                logWriter.AddLog(LogLevel.Information, "Client opens file with a durable handle v1 and a leavse v1 context");
                client.Create(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    fileName,
                    AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                    ShareAccess_Values.NONE,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                    CreateDisposition_Values.FILE_OPEN_IF,
                    File_Attributes.NONE,
                    ImpersonationLevel_Values.Impersonation,
                    SecurityFlags_Values.NONE,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                    new Smb2CreateContextRequest[] 
                    { 
                        new Smb2CreateDurableHandleRequest
                        {
                            DurableRequest = Guid.Empty,
                        },
                        new Smb2CreateRequestLease
                        {
                            LeaseKey = Guid.NewGuid(),
                            LeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING,
                        }
                    },
                    out fileId,
                    out serverCreateContexts,
                    out header,
                    out createResponse,
                    0);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("Create durable handle v1 with lease v1", header.Status);
                    return DetectResult.UnSupported;
                }

                #endregion

                #region Close

                CLOSE_Response closeResponse;
                client.Close(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    fileId,
                    Flags_Values.NONE,
                    out header,
                    out closeResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("CLOSE", header.Status);
                }

                #endregion

                if (serverCreateContexts != null)
                {
                    foreach (Smb2CreateContextResponse ctx in serverCreateContexts)
                    {
                        if (ctx is Smb2CreateDurableHandleResponse)
                        {
                            logWriter.AddLog(LogLevel.Information, "Create context HandleV1 with LeaseV1 is supported");
                            return DetectResult.Supported;
                        }
                    }
                }

                logWriter.AddLog(LogLevel.Information,
                    "The returned Create response doesn't contain durable handle response. So Create context HandleV1 with LeaseV1 is not supported");
                return DetectResult.UnSupported;
            }
        }

        public DetectResult CheckCreateContexts_HandleV2BatchOplock(string sharename, ref DetectionInfo info)
        {
            logWriter.AddLog(LogLevel.Information, "===== Detecting create context HandleV2 with Batch oplock =====");
            using (Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                ulong messageId;
                ulong sessionId;
                uint treeId;
                ConnectToShare(sharename, info, client, out messageId, out sessionId, out treeId);

                Smb2CreateContextResponse[] serverCreateContexts;
                FILEID fileId;
                Guid leaseKey = Guid.NewGuid();
                Guid createGuid = Guid.NewGuid();
                CREATE_Response createResponse;
                Packet_Header header;
                string fileName = "DurableHandleV2WithBatchOplock_" + Guid.NewGuid() + ".txt";
                logWriter.AddLog(LogLevel.Information, "Client opens file with a durable handle v2 and Batch oplock");
                client.Create(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    fileName,
                    AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                    ShareAccess_Values.NONE,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                    CreateDisposition_Values.FILE_OPEN_IF,
                    File_Attributes.NONE,
                    ImpersonationLevel_Values.Impersonation,
                    SecurityFlags_Values.NONE,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH,
                    new Smb2CreateContextRequest[] 
                    { 
                        new Smb2CreateDurableHandleRequestV2
                        {
                             CreateGuid = createGuid,
                             Timeout = 0,
                        }
                    },
                    out fileId,
                    out serverCreateContexts,
                    out header,
                    out createResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("Create durable handle v2 with Batch oplock", header.Status);
                    return DetectResult.UnSupported;
                }

                #region Close

                CLOSE_Response closeResponse;
                client.Close(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    fileId,
                    Flags_Values.NONE,
                    out header,
                    out closeResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("CLOSE", header.Status);
                }

                #endregion

                if (serverCreateContexts != null)
                {
                    foreach (Smb2CreateContextResponse ctx in serverCreateContexts)
                    {
                        if (ctx is Smb2CreateDurableHandleResponseV2)
                        {
                            logWriter.AddLog(LogLevel.Information, "Create context HandleV2 with Batch oplock is supported");
                            return DetectResult.Supported;
                        }
                    }
                }

                logWriter.AddLog(LogLevel.Information,
                    "The returned Create response doesn't contain durable handle v2 response. So Create context HandleV2 with Batch oplock is not supported");
                return DetectResult.UnSupported;
            }
        }

        public DetectResult CheckCreateContexts_HandleV2LeaseV1(string sharename, ref DetectionInfo info)
        {
            logWriter.AddLog(LogLevel.Information, "===== Detecting create context HandleV2 with LeaseV1 =====");
            using (Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                ulong messageId;
                ulong sessionId;
                uint treeId;
                ConnectToShare(sharename, info, client, out messageId, out sessionId, out treeId);

                Smb2CreateContextResponse[] serverCreateContexts;
                FILEID fileId;
                Guid leaseKey = Guid.NewGuid();
                Guid createGuid = Guid.NewGuid();
                CREATE_Response createResponse;
                Packet_Header header;
                string fileName = "DurableHandleV2LeaseV1_" + Guid.NewGuid() + ".txt";
                logWriter.AddLog(LogLevel.Information, "Client opens file with a durable handle v2 and lease v1 context");
                client.Create(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    fileName,
                    AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                    ShareAccess_Values.NONE,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                    CreateDisposition_Values.FILE_OPEN_IF,
                    File_Attributes.NONE,
                    ImpersonationLevel_Values.Impersonation,
                    SecurityFlags_Values.NONE,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                    new Smb2CreateContextRequest[] 
                    { 
                        new Smb2CreateDurableHandleRequestV2
                        {
                             CreateGuid = createGuid,
                             Timeout = 0,
                        },
                        new Smb2CreateRequestLease
                        {
                            LeaseKey = Guid.NewGuid(),
                            LeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING,
                        }
                    },
                    out fileId,
                    out serverCreateContexts,
                    out header,
                    out createResponse,
                    0);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("Create with lease v1", header.Status);
                    return DetectResult.UnSupported;
                }

                #region Close

                CLOSE_Response closeResponse;
                client.Close(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    fileId,
                    Flags_Values.NONE,
                    out header,
                    out closeResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("CLOSE", header.Status);
                }

                #endregion

                if (serverCreateContexts != null)
                {
                    foreach (Smb2CreateContextResponse ctx in serverCreateContexts)
                    {
                        if (ctx is Smb2CreateDurableHandleResponseV2)
                        {
                            logWriter.AddLog(LogLevel.Information, "Create context HandleV2 with LeaseV1 is supported");
                            return DetectResult.Supported;
                        }
                    }
                }

                logWriter.AddLog(LogLevel.Information,
                    "The returned Create response doesn't contain durable handle v2 response. So Create context HandleV2 with LeaseV1 is not supported");
                return DetectResult.UnSupported;
            }
        }

        public DetectResult CheckCreateContexts_HandleV2LeaseV2(string sharename, ref DetectionInfo info)
        {
            logWriter.AddLog(LogLevel.Information, "===== Detecting create context HandleV2 with LeaseV2 =====");
            using (Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                ulong messageId;
                ulong sessionId;
                uint treeId;
                ConnectToShare(sharename, info, client, out messageId, out sessionId, out treeId);

                Smb2CreateContextResponse[] serverCreateContexts;
                FILEID fileId;
                Guid leaseKey = Guid.NewGuid();
                Guid createGuid = Guid.NewGuid();
                CREATE_Response createResponse;
                Packet_Header header;
                string fileName = "DurableHandleV2LeaseV2_" + Guid.NewGuid() + ".txt";
                logWriter.AddLog(LogLevel.Information, "Client opens file with a durable handle v2 and lease v2 context");
                client.Create(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    fileName,
                    AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                    ShareAccess_Values.NONE,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                    CreateDisposition_Values.FILE_OPEN_IF,
                    File_Attributes.NONE,
                    ImpersonationLevel_Values.Impersonation,
                    SecurityFlags_Values.NONE,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_LEASE,
                    new Smb2CreateContextRequest[] 
                    { 
                        new Smb2CreateDurableHandleRequestV2
                        {
                             CreateGuid = createGuid,
                             Timeout = 0,
                        },
                        new Smb2CreateRequestLeaseV2
                        {
                            LeaseKey = Guid.NewGuid(),
                            LeaseState = LeaseStateValues.SMB2_LEASE_READ_CACHING | LeaseStateValues.SMB2_LEASE_HANDLE_CACHING | LeaseStateValues.SMB2_LEASE_WRITE_CACHING,
                        }
                    },
                    out fileId,
                    out serverCreateContexts,
                    out header,
                    out createResponse,
                    0);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("Create with lease v2", header.Status);
                    return DetectResult.UnSupported;
                }

                #region Close

                CLOSE_Response closeResponse;
                client.Close(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    fileId,
                    Flags_Values.NONE,
                    out header,
                    out closeResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("CLOSE", header.Status);
                }

                #endregion

                if (serverCreateContexts != null)
                {
                    foreach (Smb2CreateContextResponse ctx in serverCreateContexts)
                    {
                        if (ctx is Smb2CreateDurableHandleResponseV2)
                        {
                            logWriter.AddLog(LogLevel.Information, "Create context HandleV2 with LeaseV2 is supported");
                            return DetectResult.Supported;
                        }
                    }
                }

                logWriter.AddLog(LogLevel.Information,
                    "The returned Create response doesn't contain durable handle v2 response. So Create context HandleV2 with LeaseV2 is not supported");
                return DetectResult.UnSupported;
            }
        }
    }
}

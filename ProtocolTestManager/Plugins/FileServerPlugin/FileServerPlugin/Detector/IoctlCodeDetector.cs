// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestManager.FileServerPlugin
{
    /// <summary>
    /// Detector to detect Ioctl code 
    /// </summary>
    public partial class FSDetector
    {
        public DetectResult[] CheckIOCTL_CopyOffload(string sharename, ref DetectionInfo info)
        {
            logWriter.AddLog(LogLevel.Information, "===== Detecting IOCTL CopyOffload =====");

            #region Initialization

            int contentLength = 32;
            string content = Smb2Utility.CreateRandomString(contentLength);

            using (Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                ulong messageId;
                ulong sessionId;
                uint treeId;
                ConnectToShare(sharename, info, client, out messageId, out sessionId, out treeId);
            #endregion

                #region Create
                logWriter.AddLog(LogLevel.Information, "Client creates a file with specified length as for offload copy.");
                Smb2CreateContextResponse[] serverCreateContexts;
                FILEID fileIdSrc;
                CREATE_Response createResponse;
                Packet_Header header;
                client.Create(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    Guid.NewGuid().ToString(),
                    AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                    ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                    CreateDisposition_Values.FILE_OPEN_IF,
                    File_Attributes.NONE,
                    ImpersonationLevel_Values.Impersonation,
                    SecurityFlags_Values.NONE,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                    null,
                    out fileIdSrc,
                    out serverCreateContexts,
                    out header,
                    out createResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("CREATE", header.Status);
                    throw new Exception("CREATE failed with " + Smb2Status.GetStatusCode(header.Status));
                }

                #endregion

                #region Write

                WRITE_Response writeResponse;
                client.Write(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    0,
                    fileIdSrc,
                    Channel_Values.CHANNEL_NONE,
                    WRITE_Request_Flags_Values.None,
                    new byte[0],
                    Encoding.ASCII.GetBytes(content),
                    out header,
                    out writeResponse,
                    0);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("WRITE", header.Status);
                    throw new Exception("WRITE failed with " + Smb2Status.GetStatusCode(header.Status));
                }

                #endregion

                #region Flush
                FLUSH_Response flushResponse;
                client.Flush(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    fileIdSrc,
                    out header,
                    out flushResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("FLUSH", header.Status);
                    throw new Exception("FLUSH failed with " + Smb2Status.GetStatusCode(header.Status));
                }
                #endregion

                #region IOCTL OFFLOAD_READ
                logWriter.AddLog(LogLevel.Information,
                    "Client sends IOCTL request with FSCTL_OFFLOAD_READ to ask server to generate the token of the content for offload copy.");
                STORAGE_OFFLOAD_TOKEN token;
                ulong fileOffsetToRead = 0; //FileOffset should be aligned to logical sector boundary on the volume, e.g. 512 bytes
                ulong copyLengthToRead = (ulong)contentLength * 1024; //CopyLength should be aligned to logical sector boundary on the volume, e.g. 512 bytes
                ulong transferLength;

                // Request hardware to generate a token that represents a range of file to be copied
                FSCTL_OFFLOAD_READ_INPUT offloadReadInput = new FSCTL_OFFLOAD_READ_INPUT();
                offloadReadInput.Size = 32;
                offloadReadInput.FileOffset = fileOffsetToRead;
                offloadReadInput.CopyLength = copyLengthToRead;

                byte[] requestInputOffloadRead = TypeMarshal.ToBytes(offloadReadInput);
                byte[] responseInput;
                byte[] responseOutput;

                IOCTL_Response ioCtlResponse;
                client.IoCtl(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    CtlCode_Values.FSCTL_OFFLOAD_READ,
                    fileIdSrc,
                    0,
                    requestInputOffloadRead,
                    32000,
                    IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                    out responseInput,
                    out responseOutput,
                    out header,
                    out ioCtlResponse);

                #endregion

                DetectResult[] result = { DetectResult.UnSupported, DetectResult.UnSupported };
                if (Smb2Status.STATUS_SUCCESS != header.Status)
                {
                    LogFailedStatus("FSCTL_OFFLOAD_READ", header.Status);
                }
                else
                {
                    result[0] = DetectResult.Supported;
                    logWriter.AddLog(LogLevel.Information, "FSCTL_OFFLOAD_READ is supported");

                    #region IOCTL OFFLOAD_WRITE

                    logWriter.AddLog(LogLevel.Information, "Client creates another file as the destination of offload copy.");

                    // Create another file as the destination of offload copy.
                    FILEID fileIdDes;
                    client.Create(
                        1,
                        1,
                        info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                        messageId++,
                        sessionId,
                        treeId,
                        Guid.NewGuid().ToString(),
                        AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                        ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE,
                        CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                        CreateDisposition_Values.FILE_OPEN_IF,
                        File_Attributes.NONE,
                        ImpersonationLevel_Values.Impersonation,
                        SecurityFlags_Values.NONE,
                        RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                        null,
                        out fileIdDes,
                        out serverCreateContexts,
                        out header,
                        out createResponse);

                    if (header.Status != Smb2Status.STATUS_SUCCESS)
                    {
                        LogFailedStatus("CREATE", header.Status);
                        throw new Exception("CREATE failed with " + Smb2Status.GetStatusCode(header.Status));
                    }

                    // The destination file of CopyOffload Write should be equal to or larger than the size of original file
                    client.Write(
                        1,
                        1,
                        info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                        messageId++,
                        sessionId,
                        treeId,
                        0,
                        fileIdDes,
                        Channel_Values.CHANNEL_NONE,
                        WRITE_Request_Flags_Values.None,
                        new byte[0],
                        Smb2Utility.CreateRandomByteArray(contentLength * 1024),
                        out header,
                        out writeResponse,
                        0);

                    if (header.Status != Smb2Status.STATUS_SUCCESS)
                    {
                        LogFailedStatus("WRITE", header.Status);
                        throw new Exception("WRITE failed with " + Smb2Status.GetStatusCode(header.Status));
                    }

                    client.Flush(
                        1,
                        1,
                        info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                        messageId++,
                        sessionId,
                        treeId,
                        fileIdDes,
                        out header,
                        out flushResponse);

                    if (header.Status != Smb2Status.STATUS_SUCCESS)
                    {
                        LogFailedStatus("FLUSH", header.Status);
                        throw new Exception("FLUSH failed with " + Smb2Status.GetStatusCode(header.Status));
                    }

                    if (responseOutput != null)
                    {
                        var offloadReadOutput = TypeMarshal.ToStruct<FSCTL_OFFLOAD_READ_OUTPUT>(responseOutput);
                        transferLength = offloadReadOutput.TransferLength;
                        token = offloadReadOutput.Token;
                    }
                    else
                    {
                        transferLength = 0;
                        token = new STORAGE_OFFLOAD_TOKEN();
                    }

                    logWriter.AddLog(LogLevel.Information,
                        "Client sends IOCTL request with FSCTL_OFFLOAD_WRITE to ask server to copy the content from source to destination.");
                    ulong fileOffsetToWrite = 0; //FileOffset should be aligned to logical sector boundary on the volume, e.g. 512 bytes
                    ulong copyLengthToWrite = (ulong)contentLength * 1024; //CopyLength should be aligned to logical sector boundary on the volume, e.g. 512 bytes
                    ulong transferOffset = 0; //TransferOffset should be aligned to logical sector boundary on the volume, e.g. 512 bytes

                    FSCTL_OFFLOAD_WRITE_INPUT offloadWriteInput = new FSCTL_OFFLOAD_WRITE_INPUT();
                    offloadWriteInput.Size = 544;
                    offloadWriteInput.FileOffset = fileOffsetToWrite;
                    offloadWriteInput.CopyLength = copyLengthToWrite;
                    offloadWriteInput.TransferOffset = transferOffset;
                    offloadWriteInput.Token = token;

                    byte[] requestInputOffloadWrite = TypeMarshal.ToBytes(offloadWriteInput);

                    // Client sends IOCTL request with FSCTL_OFFLOAD_WRITE to ask server to copy the content from source to destination.
                    client.IoCtl(
                        1,
                        1,
                        info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                        messageId++,
                        sessionId,
                        treeId,
                        CtlCode_Values.FSCTL_OFFLOAD_WRITE,
                        fileIdDes,
                        0,
                        requestInputOffloadWrite,
                        32000,
                        IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                        out responseInput,
                        out responseOutput,
                        out header,
                        out ioCtlResponse);

                    if (Smb2Status.STATUS_SUCCESS == header.Status)
                    {
                        result[1] = DetectResult.Supported;
                        logWriter.AddLog(LogLevel.Information, "FSCTL_OFFLOAD_WRITE is supported");
                    }
                    else
                    {
                        LogFailedStatus("FSCTL_OFFLOAD_WRITE", header.Status);
                    }

                    CLOSE_Response closeResponseDes;
                    client.Close(
                        1,
                        1,
                        info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                        messageId++,
                        sessionId,
                        treeId,
                        fileIdDes,
                        Flags_Values.NONE,
                        out header,
                        out closeResponseDes);

                    if (header.Status != Smb2Status.STATUS_SUCCESS)
                    {
                        LogFailedStatus("CLOSE", header.Status);
                    }

                    #endregion
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
                    fileIdSrc,
                    Flags_Values.NONE,
                    out header,
                    out closeResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("CLOSE", header.Status);
                }


                #endregion

                #region Tree Disconnect

                TREE_DISCONNECT_Response treeDisconnectResponse;
                client.TreeDisconnect(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    out header,
                    out treeDisconnectResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("TREEDISCONNECT", header.Status);
                }

                #endregion

                return result;
            }
        }

        public DetectResult CheckIOCTL_FileLevelTrim(string sharename, ref DetectionInfo info)
        {
            logWriter.AddLog(LogLevel.Information, "===== Detecting FSCTL_FILE_LEVEL_TRIM =====");
            string content = Smb2Utility.CreateRandomString(32);

            using (Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                ulong messageId;
                ulong sessionId;
                uint treeId;
                ConnectToShare(sharename, info, client, out messageId, out sessionId, out treeId);

                #region Create
                logWriter.AddLog(LogLevel.Information, "Client creates a file to prepare for the trimming");
                Smb2CreateContextResponse[] serverCreateContexts;
                FILEID fileId;
                CREATE_Response createResponse;
                Packet_Header header;
                client.Create(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    Guid.NewGuid().ToString(),
                    AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                    ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                    CreateDisposition_Values.FILE_OPEN_IF,
                    File_Attributes.NONE,
                    ImpersonationLevel_Values.Impersonation,
                    SecurityFlags_Values.NONE,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                    null,
                    out fileId,
                    out serverCreateContexts,
                    out header,
                    out createResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("CREATE", header.Status);
                    throw new Exception("CREATE failed with " + Smb2Status.GetStatusCode(header.Status));
                }

                #endregion

                #region Write

                WRITE_Response writeResponse;
                client.Write(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    0,
                    fileId,
                    Channel_Values.CHANNEL_NONE,
                    WRITE_Request_Flags_Values.None,
                    new byte[0],
                    Encoding.ASCII.GetBytes(content),
                    out header,
                    out writeResponse,
                    0);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("WRITE", header.Status);
                    throw new Exception("WRITE failed with " + Smb2Status.GetStatusCode(header.Status));
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
                    throw new Exception("CLOSE failed with " + Smb2Status.GetStatusCode(header.Status));
                }

                #endregion

                #region Create

                client.Create(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    Guid.NewGuid().ToString(),
                    AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                    ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                    CreateDisposition_Values.FILE_OPEN_IF,
                    File_Attributes.NONE,
                    ImpersonationLevel_Values.Impersonation,
                    SecurityFlags_Values.NONE,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                    null,
                    out fileId,
                    out serverCreateContexts,
                    out header,
                    out createResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("CREATE", header.Status);
                    throw new Exception("Second CREATE failed with " + Smb2Status.GetStatusCode(header.Status));
                }

                #endregion

                #region IOCTL FileLevelTrim

                FSCTL_FILE_LEVEL_TRIM_RANGE fileLevelTrimRange;
                Random random = new Random();
                uint offset = (uint)random.Next(0, 32 * 1024);
                uint length = (uint)random.Next(0, (int)(32 * 1024 - offset));
                fileLevelTrimRange.Offset = offset;
                fileLevelTrimRange.Length = length;

                FSCTL_FILE_LEVEL_TRIM_INPUT fileLevelTrimInput;
                fileLevelTrimInput.Key = 0;
                fileLevelTrimInput.NumRanges = 1;
                fileLevelTrimInput.Ranges = new FSCTL_FILE_LEVEL_TRIM_RANGE[] { fileLevelTrimRange };

                byte[] buffer = TypeMarshal.ToBytes<FSCTL_FILE_LEVEL_TRIM_INPUT>(fileLevelTrimInput);
                byte[] respOutput;


                IOCTL_Response ioCtlResponse;
                byte[] respInput = new byte[1024];
                logWriter.AddLog(LogLevel.Information, "Client sends FSCTL_FILE_LEVEL_TRIM to server");
                client.IoCtl(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    CtlCode_Values.FSCTL_FILE_LEVEL_TRIM,
                    fileId,
                    0,
                    buffer,
                    64 * 1024,
                    IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                    out respInput,
                    out respOutput,
                    out header,
                    out ioCtlResponse,
                    0);

                #endregion

                DetectResult result = DetectResult.UnSupported;
                if (header.Status == Smb2Status.STATUS_SUCCESS)
                {
                    result = DetectResult.Supported;
                    logWriter.AddLog(LogLevel.Information, "FSCTL_FILE_LEVEL_TRIM is supported");
                }
                else
                {
                    LogFailedStatus("FSCTL_FILE_LEVEL_TRIM", header.Status);
                }

                #region Close

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

                #region Tree Disconnect

                TREE_DISCONNECT_Response treeDisconnectResponse;
                client.TreeDisconnect(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    out header,
                    out treeDisconnectResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("TREEDISCONNECT", header.Status);
                }

                #endregion

                return result;
            }
        }

        public DetectResult CheckIOCTL_ValidateNegotiateInfo(string sharename, ref DetectionInfo info)
        {
            logWriter.AddLog(LogLevel.Information, "===== Detecting IOCTL ValidateNegotiateInfo =====");

            using (Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                ulong messageId = 1;
                ulong sessionId = 0;
                uint treeId;
                NEGOTIATE_Response negotiateResponse;
                Guid clientGuid;
                bool encryptionRequired = false;
                DialectRevision[] preferredDialects;

                logWriter.AddLog(LogLevel.Information, "Client connects to server");
                client.ConnectOverTCP(SUTIpAddress);

                if (info.CheckHigherDialect(info.smb2Info.MaxSupportedDialectRevision, DialectRevision.Smb311))
                {
                    // VALIDATE_NEGOTIATE_INFO request is only used in 3.0 and 3.0.2
                    preferredDialects = Smb2Utility.GetDialects(DialectRevision.Smb302);
                }
                else
                {
                    preferredDialects = info.requestDialect;
                }

                #region Negotiate

                DialectRevision selectedDialect;
                byte[] gssToken;
                Packet_Header header;
                clientGuid = Guid.NewGuid();
                logWriter.AddLog(LogLevel.Information, "Client sends multi-protocol Negotiate to server");
                MultiProtocolNegotiate(
                    client,
                    1,
                    1,
                    Packet_Header_Flags_Values.NONE,
                    ref messageId,
                    preferredDialects,
                    SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                    Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU
                    | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES | Capabilities_Values.GLOBAL_CAP_ENCRYPTION,
                    clientGuid,
                    out selectedDialect,
                    out gssToken,
                    out header,
                    out negotiateResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("NEGOTIATE", header.Status);
                    throw new Exception(string.Format("NEGOTIATE failed with {0}", Smb2Status.GetStatusCode(header.Status)));
                }

                #endregion

                #region Session Setup

                SESSION_SETUP_Response sessionSetupResp;

                SspiClientSecurityContext sspiClientGss =
                    new SspiClientSecurityContext(
                        SecurityPackageType,
                        Credential,
                        Smb2Utility.GetCifsServicePrincipalName(SUTName),
                        ClientSecurityContextAttribute.None,
                        SecurityTargetDataRepresentation.SecurityNativeDrep);

                // Server GSS token is used only for Negotiate authentication when enabled
                if (SecurityPackageType == SecurityPackageType.Negotiate)
                    sspiClientGss.Initialize(gssToken);
                else
                    sspiClientGss.Initialize(null);

                do
                {
                    logWriter.AddLog(LogLevel.Information, "Client sends SessionSetup to server");
                    client.SessionSetup(
                        1,
                        64,
                        Packet_Header_Flags_Values.NONE,
                        messageId++,
                        sessionId,
                        SESSION_SETUP_Request_Flags.NONE,
                        SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                        SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
                        0,
                        sspiClientGss.Token,
                        out sessionId,
                        out gssToken,
                        out header,
                        out sessionSetupResp);

                    if ((header.Status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED || header.Status == Smb2Status.STATUS_SUCCESS) && gssToken != null && gssToken.Length > 0)
                    {
                        sspiClientGss.Initialize(gssToken);
                    }
                } while (header.Status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("SESSIONSETUP", header.Status);
                    throw new Exception(string.Format("SESSIONSETUP failed with {0}", Smb2Status.GetStatusCode(header.Status)));
                }

                byte[] sessionKey;
                sessionKey = sspiClientGss.SessionKey;
                encryptionRequired = sessionSetupResp.SessionFlags == SessionFlags_Values.SESSION_FLAG_ENCRYPT_DATA;
                client.GenerateCryptoKeys(
                    sessionId,
                    sessionKey,
                    info.smb2Info.IsRequireMessageSigning, // Enable signing according to the configuration of SUT
                    encryptionRequired,
                    null,
                    false);

                #endregion

                #region TreeConnect

                TREE_CONNECT_Response treeConnectResp;
                string uncShare = string.Format(@"\\{0}\{1}", SUTName, sharename);

                logWriter.AddLog(LogLevel.Information, "Client sends TreeConnect to server");
                client.TreeConnect(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    uncShare,
                    out treeId,
                    out header,
                    out treeConnectResp);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("TREECONNECT", header.Status);
                    throw new Exception("TREECONNECT failed with " + Smb2Status.GetStatusCode(header.Status));
                }

                #endregion

                TREE_DISCONNECT_Response treeDisconnectResponse;

                #region IOCTL FSCTL_VALIDATE_NEGOTIATE_INFO

                VALIDATE_NEGOTIATE_INFO_Request validateNegotiateInfoReq;
                validateNegotiateInfoReq.Guid = clientGuid;
                validateNegotiateInfoReq.Capabilities =
                    Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU
                    | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES
                    | Capabilities_Values.GLOBAL_CAP_ENCRYPTION;
                validateNegotiateInfoReq.SecurityMode = SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED;
                validateNegotiateInfoReq.DialectCount = (ushort)(preferredDialects.Length);
                validateNegotiateInfoReq.Dialects = preferredDialects;
                byte[] inputBuffer = TypeMarshal.ToBytes<VALIDATE_NEGOTIATE_INFO_Request>(validateNegotiateInfoReq);
                byte[] outputBuffer;
                VALIDATE_NEGOTIATE_INFO_Response validateNegotiateInfoResp;
                IOCTL_Response ioCtlResponse;

                byte[] respInput = new byte[1024];
                FILEID ioCtlFileId = new FILEID();
                ioCtlFileId.Persistent = 0xFFFFFFFFFFFFFFFF;
                ioCtlFileId.Volatile = 0xFFFFFFFFFFFFFFFF;

                logWriter.AddLog(LogLevel.Information, "Client sends FSCTL_VALIDATE_NEGOTIATE_INFO to server");

                // Validate Negotiate Info Request should be signed.
                client.EnableSessionSigningAndEncryption(sessionId, true, encryptionRequired);
                client.IoCtl(
                    1,
                    1,
                    Packet_Header_Flags_Values.FLAGS_SIGNED, // Server will terminate connection if Validate Negotiate Info Request is not signed.
                    messageId++,
                    sessionId,
                    treeId,
                    CtlCode_Values.FSCTL_VALIDATE_NEGOTIATE_INFO,
                    ioCtlFileId,
                    0,
                    inputBuffer,
                    64 * 1024,
                    IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                    out respInput,
                    out outputBuffer,
                    out header,
                    out ioCtlResponse,
                    0);

                DetectResult result = DetectResult.UnSupported;
                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("Validate Negotiate Information", header.Status);
                }
                else
                {
                    validateNegotiateInfoResp = TypeMarshal.ToStruct<VALIDATE_NEGOTIATE_INFO_Response>(outputBuffer);

                    if ((Capabilities_Values)negotiateResponse.Capabilities != validateNegotiateInfoResp.Capabilities)
                    {
                        logWriter.AddLog(LogLevel.Information, "Capabilities returned in ValidateNegotiateInfo response doesn't eaqual to server capabilities in original Negotiate response");
                    }

                    if (negotiateResponse.ServerGuid != validateNegotiateInfoResp.Guid)
                    {
                        logWriter.AddLog(LogLevel.Information, "ServerGuid returned in ValidateNegotiateInfo response doesn't eaqual to server ServerGuid in original Negotiate response");
                    }

                    if ((SecurityMode_Values)negotiateResponse.SecurityMode != validateNegotiateInfoResp.SecurityMode)
                    {
                        logWriter.AddLog(LogLevel.Information, "SecurityMode returned in ValidateNegotiateInfo response doesn't eaqual to server SecurityMode in original Negotiate response");
                    }

                    if (negotiateResponse.DialectRevision != validateNegotiateInfoResp.Dialect)
                    {
                        logWriter.AddLog(LogLevel.Information, "Validation failed for dialect supported on server");
                    }

                    result = DetectResult.Supported;
                    logWriter.AddLog(LogLevel.Information, "FSCTL_VALIDATE_NEGOTIATE_INFO is supported");
                }

                #endregion

                #region Tree Disconnect

                // Set configuration back after Validating Negotiate Info procedure.
                client.EnableSessionSigningAndEncryption(sessionId, info.smb2Info.IsRequireMessageSigning, encryptionRequired);

                client.TreeDisconnect(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    out header,
                    out treeDisconnectResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("TREEDISCONNECT", header.Status);
                }

                #endregion

                return result;
            }
        }

        public DetectResult CheckIOCTL_ResilientHandle(string sharename, ref DetectionInfo info)
        {
            logWriter.AddLog(LogLevel.Information, "===== Detecting IOCTL ResilientHandle =====");

            using (Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                ulong messageId;
                ulong sessionId;
                uint treeId;
                ConnectToShare(sharename, info, client, out messageId, out sessionId, out treeId);

                #region Create

                Smb2CreateContextResponse[] serverCreateContexts;
                FILEID fileId;
                CREATE_Response createResponse;
                Packet_Header header;
                logWriter.AddLog(LogLevel.Information, "Client opens a file");
                client.Create(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    Guid.NewGuid().ToString(),
                    AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                    ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                    CreateDisposition_Values.FILE_OPEN_IF,
                    File_Attributes.NONE,
                    ImpersonationLevel_Values.Impersonation,
                    SecurityFlags_Values.NONE,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                    null,
                    out fileId,
                    out serverCreateContexts,
                    out header,
                    out createResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("CREATE", header.Status);
                    throw new Exception("CREATE failed with " + Smb2Status.GetStatusCode(header.Status));
                }

                #endregion

                #region IOCTL ResilientHandle

                IOCTL_Response ioCtlResponse;
                byte[] inputInResponse;
                byte[] outputInResponse;
                uint inputCount = (uint)Marshal.SizeOf(typeof(NETWORK_RESILIENCY_Request));
                NETWORK_RESILIENCY_Request resilientRequest = new NETWORK_RESILIENCY_Request()
                {
                    Timeout = 120
                };

                byte[] resiliencyRequestBytes = TypeMarshal.ToBytes<NETWORK_RESILIENCY_Request>(resilientRequest);
                byte[] buffer = resiliencyRequestBytes;
                if (inputCount < resiliencyRequestBytes.Length)
                {
                    buffer = new byte[inputCount];
                    Array.Copy(resiliencyRequestBytes, buffer, buffer.Length);
                }
                else if (inputCount > resiliencyRequestBytes.Length)
                {
                    buffer = new byte[inputCount];
                    Array.Copy(resiliencyRequestBytes, buffer, resiliencyRequestBytes.Length);
                }

                logWriter.AddLog(LogLevel.Information, "Client sends an IOCTL FSCTL_LMR_REQUEST_RESILLIENCY request.");
                client.IoCtl(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    CtlCode_Values.FSCTL_LMR_REQUEST_RESILIENCY,
                    fileId,
                    0,
                    buffer,
                    64 * 1024,
                    IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                    out inputInResponse,
                    out outputInResponse,
                    out header,
                    out ioCtlResponse,
                    0);

                DetectResult result = DetectResult.UnSupported;
                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("FSCTL_LMR_REQUEST_RESILIENCY", header.Status);
                }
                else
                {
                    result = DetectResult.Supported;
                    logWriter.AddLog(LogLevel.Information, "FSCTL_LMR_REQUEST_RESILIENCY is supported");
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

                #region Tree Disconnect

                TREE_DISCONNECT_Response treeDisconnectResponse;
                client.TreeDisconnect(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    out header,
                    out treeDisconnectResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("TREEDISCONNECT", header.Status);
                }

                #endregion

                return result;
            }
        }

        public DetectResult[] CheckIOCTL_IntegrityInfo(string sharename, ref DetectionInfo info)
        {
            logWriter.AddLog(LogLevel.Information, "===== Detecting IOCTL IntegrityInfo =====");
            logWriter.AddLog(LogLevel.Information, "Share name: " + sharename);

            using (Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                ulong messageId;
                ulong sessionId;
                uint treeId;
                ConnectToShare(sharename, info, client, out messageId, out sessionId, out treeId);

                #region Create

                Smb2CreateContextResponse[] serverCreateContexts;
                FILEID fileId;
                CREATE_Response createResponse;
                Packet_Header header;
                logWriter.AddLog(LogLevel.Information, "Client opens a file");
                client.Create(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    Guid.NewGuid().ToString(),
                    AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                    ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                    CreateDisposition_Values.FILE_OPEN_IF,
                    File_Attributes.NONE,
                    ImpersonationLevel_Values.Impersonation,
                    SecurityFlags_Values.NONE,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                    null,
                    out fileId,
                    out serverCreateContexts,
                    out header,
                    out createResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("CREATE", header.Status);
                    throw new Exception("CREATE failed with with " + Smb2Status.GetStatusCode(header.Status));
                }

                #endregion

                #region IOCTL GET IntegrityInfo

                logWriter.AddLog(LogLevel.Information, "Client sends IOCTL request with FSCTL_GET_INTEGRITY_INFORMATION.");
                FSCTL_GET_INTEGRITY_INFO_OUTPUT getIntegrityInfo;
                IOCTL_Response ioCtlResponse;
                byte[] buffer = new byte[1024];
                byte[] respInput = new byte[1024];
                byte[] respOutput = new byte[1024];

                client.IoCtl(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    CtlCode_Values.FSCTL_GET_INTEGRITY_INFORMATION,
                    fileId,
                    0,
                    buffer,
                    64 * 1024,
                    IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                    out respInput,
                    out respOutput,
                    out header,
                    out ioCtlResponse,
                    0);

                DetectResult[] result = { DetectResult.UnSupported, DetectResult.UnSupported };

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    // Get_Integrity is not supported
                    // Set_Integrity cannot be tested.
                    LogFailedStatus("GET_INTEGRITY_INFORMATION", header.Status);
                    return result;
                }

                getIntegrityInfo = TypeMarshal.ToStruct<FSCTL_GET_INTEGRITY_INFO_OUTPUT>(respOutput);

                result[0] = DetectResult.Supported;
                logWriter.AddLog(LogLevel.Information, "FSCTL_GET_INTEGRITY_INFORMATION is supported");

                #endregion

                #region IOCTL SET IntegrityInfo

                logWriter.AddLog(LogLevel.Information,
                    "Client sends IOCTL request with FSCTL_SET_INTEGRITY_INFORMATION after changed the value of the following fields in FSCTL_SET_INTEGRIY_INFO_INPUT: "
                    + "ChecksumAlgorithm, Flags, Reserved.");
                FSCTL_SET_INTEGRIY_INFO_INPUT setIntegrityInfo;
                setIntegrityInfo.ChecksumAlgorithm = FSCTL_SET_INTEGRITY_INFO_INPUT_CHECKSUMALGORITHM.CHECKSUM_TYPE_CRC64;
                setIntegrityInfo.Flags = FSCTL_SET_INTEGRITY_INFO_INPUT_FLAGS.FSCTL_INTEGRITY_FLAG_CHECKSUM_ENFORCEMENT_OFF;
                setIntegrityInfo.Reserved = FSCTL_SET_INTEGRITY_INFO_INPUT_RESERVED.V1;
                buffer = TypeMarshal.ToBytes<FSCTL_SET_INTEGRIY_INFO_INPUT>(setIntegrityInfo);

                respInput = new byte[1024];
                respOutput = new byte[1024];

                client.IoCtl(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    CtlCode_Values.FSCTL_SET_INTEGRITY_INFORMATION,
                    fileId,
                    0,
                    buffer,
                    64 * 1024,
                    IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                    out respInput,
                    out respOutput,
                    out header,
                    out ioCtlResponse,
                    0);

                #endregion

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("SET_INTEGRITY_INFORMATION", header.Status);
                }
                else
                {
                    #region IOCTL GET IntegrityInfo

                    buffer = new byte[1024];
                    respInput = new byte[1024];
                    respOutput = new byte[1024];
                    logWriter.AddLog(LogLevel.Information, "Client sends second FSCTL_GET_INTEGRITY_INFORMATION.");
                    client.IoCtl(
                        1,
                        1,
                        info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                        messageId++,
                        sessionId,
                        treeId,
                        CtlCode_Values.FSCTL_GET_INTEGRITY_INFORMATION,
                        fileId,
                        0,
                        buffer,
                        64 * 1024,
                        IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                        out respInput,
                        out respOutput,
                        out header,
                        out ioCtlResponse,
                        0);

                    getIntegrityInfo = TypeMarshal.ToStruct<FSCTL_GET_INTEGRITY_INFO_OUTPUT>(respOutput);

                    if (header.Status != Smb2Status.STATUS_SUCCESS)
                    {
                        LogFailedStatus("Second GET_INTEGRITY_INFORMATION", header.Status);
                    }

                    if ((ushort)setIntegrityInfo.ChecksumAlgorithm != (ushort)getIntegrityInfo.ChecksumAlgorithm)
                    {
                        logWriter.AddLog(LogLevel.Information, "Failed to set the ChecksumAlgorithm field to value " + setIntegrityInfo.ChecksumAlgorithm);
                    }

                    result[1] = DetectResult.Supported;
                    logWriter.AddLog(LogLevel.Information, "FSCTL_SET_INTEGRITY_INFORMATION is supported");

                    #endregion
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

                #region Tree Disconnect

                TREE_DISCONNECT_Response treeDisconnectResponse;
                client.TreeDisconnect(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    out header,
                    out treeDisconnectResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("TREEDISCONNECT", header.Status);
                }

                #endregion

                return result;
            }
        }

        public DetectResult CheckIOCTL_EnumerateSnapShots(string sharename, ref DetectionInfo info)
        {
            logWriter.AddLog(LogLevel.Information, "===== Detecting IOCTL FSCTL_SRV_ENUMERATE_SNAPSHOTS =====");
            logWriter.AddLog(LogLevel.Information, "Share name: " + sharename);

            using (Smb2Client client = new Smb2Client(new TimeSpan(0, 0, defaultTimeoutInSeconds)))
            {
                ulong messageId;
                ulong sessionId;
                uint treeId;
                ConnectToShare(sharename, info, client, out messageId, out sessionId, out treeId);

                #region Create

                Smb2CreateContextResponse[] serverCreateContexts;
                FILEID fileId;
                CREATE_Response createResponse;
                Packet_Header header;
                logWriter.AddLog(LogLevel.Information, "Client opens a file");
                client.Create(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    Guid.NewGuid().ToString(),
                    AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE,
                    ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE,
                    CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                    CreateDisposition_Values.FILE_OPEN_IF,
                    File_Attributes.NONE,
                    ImpersonationLevel_Values.Impersonation,
                    SecurityFlags_Values.NONE,
                    RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                    null,
                    out fileId,
                    out serverCreateContexts,
                    out header,
                    out createResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("CREATE", header.Status);
                    throw new Exception("CREATE failed with with " + Smb2Status.GetStatusCode(header.Status));
                }

                #endregion

                #region IOCTL FSCTL_SRV_ENUMERATE_SNAPSHOTS

                logWriter.AddLog(LogLevel.Information, "Client sends IOCTL request with FSCTL_SRV_ENUMERATE_SNAPSHOTS.");
                IOCTL_Response ioCtlResponse;
                byte[] buffer = new byte[1024];
                byte[] respInput = new byte[1024];
                byte[] respOutput = new byte[1024];

                client.IoCtl(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    CtlCode_Values.FSCTL_SRV_ENUMERATE_SNAPSHOTS,
                    fileId,
                    0,
                    buffer,
                    64 * 1024,
                    IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                    out respInput,
                    out respOutput,
                    out header,
                    out ioCtlResponse,
                    0);

                DetectResult result = DetectResult.UnSupported;

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    // FSCTL_SRV_ENUMERATE_SNAPSHOTS is not supported
                    LogFailedStatus("FSCTL_SRV_ENUMERATE_SNAPSHOTS", header.Status);
                    return result;
                }

                result = DetectResult.Supported;
                logWriter.AddLog(LogLevel.Information, "FSCTL_SRV_ENUMERATE_SNAPSHOTS is supported");

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

                #region Tree Disconnect

                TREE_DISCONNECT_Response treeDisconnectResponse;
                client.TreeDisconnect(
                    1,
                    1,
                    info.smb2Info.IsRequireMessageSigning ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                    messageId++,
                    sessionId,
                    treeId,
                    out header,
                    out treeDisconnectResponse);

                if (header.Status != Smb2Status.STATUS_SUCCESS)
                {
                    LogFailedStatus("TREEDISCONNECT", header.Status);
                }

                #endregion

                return result;
            }
        }
    }
}

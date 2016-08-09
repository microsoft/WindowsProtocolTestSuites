// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rsvd
{
    /// <summary>
    /// RsvdClient is the exposed interface for testing Rsvd server.
    /// </summary>
    public sealed class RsvdClient : IDisposable
    {
        #region Fields

        private Smb2ClientTransport transport;
        private bool disposed;
        private TimeSpan timeout;

        #endregion

        #region Constructor & Dispose

        /// <summary>
        /// Constructor
        /// </summary>
        public RsvdClient(TimeSpan timeout)
        {
            this.timeout = timeout;
        }

        /// <summary>
        /// Release the managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // Take this object out of the finalization queue of the GC:
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release resources.
        /// </summary>
        /// <param name="disposing">If disposing equals true, Managed and unmanaged resources are disposed.
        /// if false, Only unmanaged resources can be disposed.</param>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources & other reference types:
                    if (this.transport != null)
                    {
                        this.transport.Dispose();
                        this.transport = null;
                    }
                }

                this.disposed = true;
            }
        }

        /// <summary>
        /// finalizer
        /// </summary>
        ~RsvdClient()
        {
            Dispose(false);
        }

        #endregion

        /// <summary>
        /// Set up connection with server.
        /// Including 4 steps: 1. Tcp connection 2. Negotiation 3. SessionSetup 4. TreeConnect 
        /// </summary>
        /// <param name="serverName">server name</param>
        /// <param name="serverIP">server IP address</param> 
        /// <param name="domain">user's domain</param>
        /// <param name="userName">user's name</param>
        /// <param name="password">user's password</param>
        /// <param name="securityPackage">The security package</param>
        /// <param name="useServerToken">Whether to use token from server</param>
        /// <param name="shareName">Name of the share when TreeConnect</param>
        public void Connect(
            string serverName,
            IPAddress serverIP,
            string domain,
            string userName,
            string password,
            SecurityPackageType securityPackage,
            bool useServerToken,
            string shareName)
        {
            transport = new Smb2ClientTransport();
            transport.ConnectShare(serverName, serverIP, domain, userName, password, shareName, securityPackage, useServerToken);
        }

        /// <summary>
        /// Disconnect from server.
        /// </summary>
        public void Disconnect()
        {
            if (transport != null)
            {
                this.transport.Disconnect(timeout);
            }
        }

        /// <summary>
        /// Open the shared virtual disk file
        /// Send smb2 create request to server
        /// </summary>
        /// <param name="vhdxName">Name of the shared virtual disk file</param>
        /// <param name="createOption">CreateOption in create request</param>
        /// <param name="contexts">Create Contexts to be sent together with Create Request</param>
        /// <param name="status">Status of create response</param>
        /// <param name="serverContexts">Create contexts returned in create response</param>
        /// <param name="response">Create Response returned by server</param>
        /// <returns>Create Response returned by server</returns>
        public uint OpenSharedVirtualDisk(
            string vhdxName,
            FsCreateOption createOption,
            Smb2CreateContextRequest[] contexts,
            out Smb2CreateContextResponse[] serverContexts,
            out CREATE_Response response)
        {
            uint status;
            transport.Create(
                vhdxName,
                // The desired access is set the same as Windows client's behavior
                FsFileDesiredAccess.FILE_READ_DATA | FsFileDesiredAccess.FILE_WRITE_DATA | FsFileDesiredAccess.FILE_READ_ATTRIBUTES | FsFileDesiredAccess.READ_CONTROL,
                ShareAccess_Values.FILE_SHARE_DELETE | ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE,
                FsImpersonationLevel.Impersonation,
                FsFileAttribute.NONE,
                FsCreateDisposition.FILE_OPEN,
                createOption,
                contexts,
                out status,
                out serverContexts,
                out response);
            return status;
        }

        /// <summary>
        /// Close the shared virtual disk file
        /// </summary>
        public void CloseSharedVirtualDisk()
        {
            transport.Close();
        }

        /// <summary>
        /// Read content from the shared virtual disk file
        /// </summary>
        /// <param name="offset">In bytes, from the beginning of the virtual disk where data should be read</param>
        /// <param name="length">The number of bytes to read</param>
        /// <param name="data">The buffer to receive the data.</param>
        /// <returns>Status of response packet</returns>
        public uint Read(ulong offset, uint length, out byte[] data)
        {
            return transport.Read(timeout, offset, length, out data);
        }

        /// <summary>
        /// Write content to the shared virtual disk file
        /// </summary>
        /// <param name="offset">In bytes, from the beginning of the virtual disk where data should be written</param>
        /// <param name="data">A buffer containing the bytes to be written.</param>
        /// <returns>Status of response packet</returns>
        public uint Write(ulong offset, byte[] data)
        {
            return transport.Write(timeout, offset, data);
        }

        /// <summary>
        /// Tunnel operations to the shared virtual disk file
        /// </summary>
        /// <typeparam name="ResponseT">Type of the operation response</typeparam>
        /// <param name="isAsyncOperation">Indicate whether the tunnel operation is async or not</param>
        /// <param name="code">Operation code</param>
        /// <param name="requestId">A value that uniquely identifies an operation request and response for all requests sent by this client</param>
        /// <param name="requestStructure">The marshalled tunnel structure of the request</param>
        /// <param name="responseHeader">Tunnel operation header of the response packet</param>
        /// <param name="response">Tunnel operation response of the smb2 response packet</param>
        /// <returns>Status of the smb2 response packet</returns>
        public uint TunnelOperation<ResponseT>(
            bool isAsyncOperation,
            RSVD_TUNNEL_OPERATION_CODE code,
            ulong requestId,
            byte[] requestStructure,
            out SVHDX_TUNNEL_OPERATION_HEADER? responseHeader,
            out ResponseT? response) where ResponseT : struct
        {
            SVHDX_TUNNEL_OPERATION_HEADER header = new SVHDX_TUNNEL_OPERATION_HEADER();
            header.OperationCode = code;
            header.RequestId = requestId;

            byte[] payload = TypeMarshal.ToBytes(header);

            if (null != requestStructure)
                payload = payload.Concat(requestStructure).ToArray();

            CtlCode_Values ctlCode = isAsyncOperation ? CtlCode_Values.FSCTL_SVHDX_ASYNC_TUNNEL_REQUEST : CtlCode_Values.FSCTL_SVHDX_SYNC_TUNNEL_REQUEST;
            transport.SendIoctlPayload(ctlCode, payload);

            uint smb2Status;
            transport.ExpectIoctlPayload(out smb2Status, out payload);

            response = null;
            responseHeader = null;
            if (smb2Status != Smb2Status.STATUS_SUCCESS)
            {
                return smb2Status;
            }

            responseHeader = TypeMarshal.ToStruct<SVHDX_TUNNEL_OPERATION_HEADER>(payload);

            // If the response does not contain Header only, parse the resonse body.
            if (responseHeader.Value.Status == 0 && typeof(ResponseT) != typeof(SVHDX_TUNNEL_OPERATION_HEADER))
            {
                response = TypeMarshal.ToStruct<ResponseT>(payload.Skip(Marshal.SizeOf(responseHeader)).ToArray());
            }
            else
            {
                // The operation failed, so no response body.
                // Or the response only contains header, no response body.
            }

            return smb2Status;
        }

        /// <summary>
        /// Create an SVHDX_TUNNEL_FILE_INFO_REQUEST structure and marshal it to a byte array
        /// </summary>
        /// <returns>The marshalled byte array</returns>
        public byte[] CreateTunnelFileInfoRequest()
        {
            // The SVHDX_TUNNEL_FILE_INFO_REQUEST packet is sent by the client to get the shared virtual disk file information. 
            // The request MUST contain only SVHDX_TUNNEL_OPERATION_HEADER, and MUST NOT contain any payload.
            return null;
        }

        /// <summary>
        /// Create an SVHDX_TUNNEL_CHECK_CONNECTION_REQUEST structure and marshal it to a byte array
        /// </summary>
        /// <returns>The marshalled byte array</returns>
        public byte[] CreateTunnelCheckConnectionStatusRequest()
        {
            ///The SVHDX_TUNNEL_CHECK_CONNECTION_REQUEST packet is sent by the client to check the connection status to the shared virtual disk. 
            ///The request MUST contain only SVHDX_TUNNEL_OPERATION_HEADER, and MUST NOT contain any payload.
            return null;
        }

        /// <summary>
        /// Create an SVHDX_TUNNEL_SRB_STATUS_REQUEST structure and marshal it to a byte array
        /// </summary>
        /// <param name="statusKey">The client MUST set this field to the least significant bytes of the status code value given by the server for the failed request.</param>
        /// <returns>The marshalled byte array</returns>
        public byte[] CreateTunnelSrbStatusRequest(byte statusKey)
        {
            SVHDX_TUNNEL_SRB_STATUS_REQUEST srbStatusRequest = new SVHDX_TUNNEL_SRB_STATUS_REQUEST();
            srbStatusRequest.StatusKey = statusKey;
            srbStatusRequest.Reserved = new byte[27];
            return TypeMarshal.ToBytes(srbStatusRequest);
        }

        /// <summary>
        /// Create an SVHDX_TUNNEL_DISK_INFO_REQUEST structure and marshal it to a byte array
        /// </summary>
        /// <param name="blockSize">The client MUST set this field to zero, and the server MUST ignore it on receipt.</param>
        /// <param name="linkageID">The client MUST set this field to zero, and the server MUST ignore it on receipt.</param>
        /// <param name="isMounted">The client MUST set this field to zero, and the server MUST ignore it on receipt.</param>
        /// <param name="is4kAligned">The client MUST set this field to zero, and the server MUST ignore it on receipt.</param>
        /// <param name="fileSize">The client MUST set this field to zero, and the server MUST ignore it on receipt.</param>
        /// <param name="virtualDiskId">This field MUST NOT be used and MUST be reserved. The client MUST set this field to zero, and the server MUST ignore it on receipt.</param>
        /// <returns>The marshalled byte array</returns>
        public byte[] CreateTunnelDiskInfoRequest(uint blockSize = 0, Guid linkageID = default(Guid), bool isMounted = false, bool is4kAligned = false, ulong fileSize = 0, Guid virtualDiskId = default(Guid))
        {
            SVHDX_TUNNEL_DISK_INFO_REQUEST diskInfoRequest = new SVHDX_TUNNEL_DISK_INFO_REQUEST();
            diskInfoRequest.BlockSize = blockSize;
            diskInfoRequest.LinkageID = linkageID;
            diskInfoRequest.IsMounted = isMounted;
            diskInfoRequest.Is4kAligned = is4kAligned;
            diskInfoRequest.FileSize = fileSize;
            diskInfoRequest.VirtualDiskId = virtualDiskId;
            return TypeMarshal.ToBytes(diskInfoRequest);
        }

        /// <summary>
        /// Create an SVHDX_TUNNEL_SCSI_REQUEST structure and marshal it to a byte array
        /// </summary>
        /// <param name="length">Specifies the size, in bytes, of the SVHDX_TUNNEL_SCSI_REQUEST structure.</param>
        /// <param name="cdbLength">The length, in bytes, of the SCSI command descriptor block. This value MUST be less than or equal to RSVD_CDB_GENERIC_LENGTH.</param>
        /// <param name="senseInfoExLength">The length, in bytes, of the request sense data buffer. This value MUST be less than or equal to RSVD_SCSI_SENSE_BUFFER_SIZE.</param>
        /// <param name="dataIn">A Boolean, indicating the SCSI command descriptor block transfer type. 
        /// The value TRUE (0x01) indicates that the operation is to store the data onto the disk. 
        /// The value FALSE (0x00) indicates that the operation is to retrieve the data from the disk.</param>
        /// <param name="srbFlags">An optional, application-provided flag to indicate the options of the SCSI request.</param>
        /// <param name="dataTransferLength">The length, in bytes, of the additional data placed in the DataBuffer field.</param>
        /// <param name="cdbBuffer">A buffer that contains the SCSI command descriptor block.</param>
        /// <param name="dataBuffer">A variable-length buffer that contains the additional buffer, as described by the DataTransferLength field.</param>
        /// <returns>The marshalled byte array</returns>
        public byte[] CreateTunnelScsiRequest(
            ushort length, 
            byte cdbLength, 
            byte senseInfoExLength, 
            bool dataIn, 
            SRB_FLAGS srbFlags, 
            uint dataTransferLength, 
            byte[] cdbBuffer, 
            byte[] dataBuffer)
        {
            SVHDX_TUNNEL_SCSI_REQUEST scsiRequest = new SVHDX_TUNNEL_SCSI_REQUEST();

            scsiRequest.Length = length;
            scsiRequest.CdbLength = cdbLength;
            scsiRequest.SenseInfoExLength = senseInfoExLength;
            scsiRequest.DataIn = dataIn;
            scsiRequest.SrbFlags = srbFlags;
            scsiRequest.DataTransferLength = dataTransferLength;
            scsiRequest.CDBBuffer = cdbBuffer;
            scsiRequest.DataBuffer = dataBuffer;
            return TypeMarshal.ToBytes(scsiRequest);
        }

        /// <summary>
        /// Create an SVHDX_TUNNEL_VALIDATE_DISK_REQUEST structure and marshal it to a byte array
        /// </summary>
        /// <returns>The marshalled byte array</returns>
        public byte[] CreateTunnelValidateDiskRequest()
        {
            ///The SVHDX_TUNNEL_VALIDATE_DISK_REQUEST packet is sent by the client to validate the shared virtual disk. 
            SVHDX_TUNNEL_VALIDATE_DISK_REQUEST validDiskRequest = new SVHDX_TUNNEL_VALIDATE_DISK_REQUEST();
            validDiskRequest.Reserved = new byte[56];
            return TypeMarshal.ToBytes(validDiskRequest);
        }

        /// <summary>
        /// Create an SVHDX_TUNNEL_VHDSET_QUERY_INFORMATION_REQUEST structure and marshal it to a byte array
        /// </summary>
        /// <param name="setInformationType">The set file information type requested</param>
        /// <param name="snapshotType">The snapshot type queried by this operation</param>
        /// <param name="snapshotId">The snapshot ID relevant to the particular request</param>
        /// <returns>The marshalled byte array</returns>
        public byte[] CreateTunnelGetVHDSetFileInfoRequest(
            VHDSet_InformationType setInformationType,
            Snapshot_Type snapshotType,
            Guid snapshotId)
        {
            SVHDX_TUNNEL_VHDSET_QUERY_INFORMATION_REQUEST getVHDSetFileInfoRequest = new SVHDX_TUNNEL_VHDSET_QUERY_INFORMATION_REQUEST();

            getVHDSetFileInfoRequest.VHDSetInformationType = setInformationType;
            getVHDSetFileInfoRequest.SnapshotType = snapshotType;
            getVHDSetFileInfoRequest.SnapshotId = snapshotId;

            return TypeMarshal.ToBytes(getVHDSetFileInfoRequest);
        }

        /// <summary>
        /// Create an SVHDX_META_OPERATION_START_CREATE_SNAPSHOT_REQUEST structure and marshal it to a byte array
        /// </summary>
        /// <param name="startRequest">Type SVHDX_META_OPERATION_START_REQUEST includes TransactionId and OperationType</param>
        /// <param name="createSnapshot">Type of SVHDX_META_OPERATION_CREATE_SNAPSHOT structure</param>
        /// <returns>The marshalled byte array</returns>
        public byte[] CreateTunnelMetaOperationStartCreateSnapshotRequest(
            SVHDX_META_OPERATION_START_REQUEST startRequest,
            SVHDX_META_OPERATION_CREATE_SNAPSHOT createSnapshot)
        {
            SVHDX_META_OPERATION_START_CREATE_SNAPSHOT_REQUEST createSnapshotRequest = new SVHDX_META_OPERATION_START_CREATE_SNAPSHOT_REQUEST();

            createSnapshotRequest.startRequest = startRequest;
            createSnapshotRequest.createSnapshot = createSnapshot;

            return TypeMarshal.ToBytes(createSnapshotRequest);
        }

        /// <summary>
        /// Create an SVHDX_META_OPERATION_START_APPLY_SNAPSHOT_REQUEST structure and marshal it to a byte array
        /// </summary>
        /// <param name="startRequest">Type SVHDX_META_OPERATION_START_REQUEST includes TransactionId and OperationType</param>
        /// <param name="applySnapshot">Type of SVHDX_APPLY_SNAPSHOT_PARAMS structure</param>
        /// <returns>The marshalled byte array</returns>
        public byte[] CreateTunnelMetaOperationStartApplySnapshotRequest(
            SVHDX_META_OPERATION_START_REQUEST startRequest,
            SVHDX_APPLY_SNAPSHOT_PARAMS applySnapshot)
        {
            SVHDX_META_OPERATION_START_APPLY_SNAPSHOT_REQUEST applySnapshotRequest = new SVHDX_META_OPERATION_START_APPLY_SNAPSHOT_REQUEST();

            applySnapshotRequest.startRequest = startRequest;
            applySnapshotRequest.applySnapshot = applySnapshot;

            return TypeMarshal.ToBytes(applySnapshotRequest);
        }

        /// <summary>
        /// Create an SVHDX_META_OPERATION_OPTIMIZE_REQUEST structure and marshal it to a byte array
        /// </summary>
        /// <param name="startRequest">Type SVHDX_META_OPERATION_START_REQUEST includes TransactionId and OperationType</param>
        /// <returns>The marshalled byte array</returns>
        public byte[] CreateTunnelMetaOperationStartOptimizeRequest(
            SVHDX_META_OPERATION_START_REQUEST startRequest)
        {
            SVHDX_META_OPERATION_START_OPTIMIZE_REQUEST optimizeRequest = new SVHDX_META_OPERATION_START_OPTIMIZE_REQUEST();

            optimizeRequest.startRequest = startRequest;

            return TypeMarshal.ToBytes(optimizeRequest);
        }

        /// <summary>
        /// Create an SVHDX_TUNNEL_DELETE_SNAPSHOT_REQUEST structure and marshal it to a byte array
        /// </summary>
        /// <param name="request">A SVHDX_TUNNEL_DELETE_SNAPSHOT_REQUEST request</param>
        /// <returns>The marshalled byte array</returns>
        public byte[] CreateTunnelMetaOperationDeleteSnapshotRequest(
            SVHDX_TUNNEL_DELETE_SNAPSHOT_REQUEST request)
        {
            return TypeMarshal.ToBytes(request);
        }

        /// <summary>
        /// Create an SVHDX_META_OPERATION_START_EXTRACT_REQUEST structure and marshal it to a byte array
        /// </summary>
        /// <param name="startRequest">Struct SVHDX_META_OPERATION_START_REQUEST includes TransactionId and OperationType</param>
        /// <param name="extract">Struct SVHDX_META_OPERATION_EXTRACT</param>
        /// <returns>The marshalled byte array</returns>
        public byte[] CreateTunnelMetaOperationStartExtractRequest(
            SVHDX_META_OPERATION_START_REQUEST startRequest,
            SVHDX_META_OPERATION_EXTRACT extract)
        {
            SVHDX_META_OPERATION_START_EXTRACT_REQUEST extractRequest = new SVHDX_META_OPERATION_START_EXTRACT_REQUEST();

            extractRequest.startRequest = startRequest;
            extractRequest.extract = extract;

            return TypeMarshal.ToBytes(extractRequest);
        }

        /// <summary>
        /// Create an SVHDX_META_OPERATION_START_CONVERT_TO_VHDSET_REQUEST structure and marshal it to a byte array
        /// </summary>
        /// <param name="startRequest">Struct SVHDX_META_OPERATION_START_REQUEST includes TransactionId and OperationType</param>
        /// <param name="convert">Struct SVHDX_META_OPERATION_CONVERT_TO_VHDSET</param>
        /// <returns>The marshalled byte array</returns>
        public byte[] CreateTunnelMetaOperationStartConvertToVHDSetRequest(
            SVHDX_META_OPERATION_START_REQUEST startRequest,
            SVHDX_META_OPERATION_CONVERT_TO_VHDSET convert)
        {
            SVHDX_META_OPERATION_START_CONVERT_TO_VHDSET_REQUEST convertRequest = new SVHDX_META_OPERATION_START_CONVERT_TO_VHDSET_REQUEST();

            convertRequest.startRequest = startRequest;
            convertRequest.convert = convert;

            return TypeMarshal.ToBytes(convertRequest);
        }

        /// <summary>
        /// Create an SVHDX_META_OPERATION_START_RESIZE_REQUEST structure and marshal it to a byte array
        /// </summary>
        /// <param name="startRequest">Struct SVHDX_META_OPERATION_START_REQUEST includes TransactionId and OperationType</param>
        /// <param name="resize">Struct SVHDX_META_OPERATION_RESIZE_VIRTUAL_DISK</param>
        /// <returns>The marshalled byte array</returns>
        public byte[] CreateTunnelMetaOperationStartResizeRequest(
            SVHDX_META_OPERATION_START_REQUEST startRequest,
            SVHDX_META_OPERATION_RESIZE_VIRTUAL_DISK resize)
        {
            SVHDX_META_OPERATION_START_RESIZE_REQUEST resizeVHDSetRequest = new SVHDX_META_OPERATION_START_RESIZE_REQUEST();

            resizeVHDSetRequest.startRequest = startRequest;
            resizeVHDSetRequest.resizeRequest = resize;

            return TypeMarshal.ToBytes(resizeVHDSetRequest);
        }

        /// <summary>
        /// Marshal an SVHDX_CHANGE_TRACKING_START_REQUEST structure
        /// </summary>
        /// <param name="request">SVHDX_CHANGE_TRACKING_START_REQUEST structure</param>
        /// <returns>The marshalled byte array</returns>
        public byte[] CreateChangeTrackingStartRequest(SVHDX_CHANGE_TRACKING_START_REQUEST request)
        {
            return TypeMarshal.ToBytes(request);
        }

        /// <summary>
        /// Marshal an SVHDX_TUNNEL_QUERY_VIRTUAL_DISK_CHANGES_REQUEST structure
        /// </summary>
        /// <param name="request">SVHDX_TUNNEL_QUERY_VIRTUAL_DISK_CHANGES_REQUEST structure</param>
        /// <returns>The marshalled byte array</returns>
        public byte[] CreateQueryVirtualDiskChangeRequest(SVHDX_TUNNEL_QUERY_VIRTUAL_DISK_CHANGES_REQUEST request)
        {
            return TypeMarshal.ToBytes(request);
        }

        /// <summary>
        /// Marshal a RSVD_BLOCK_DEVICE_TARGET_SPECIFIER structure
        /// </summary>
        /// <param name="snapshotId">A GUID that identifies the snapshot to open</param>
        /// <returns>The marshalled byte array</returns>
        public byte[] CreateTargetSpecifier(Guid snapshotId)
        {
            RSVD_BLOCK_DEVICE_TARGET_SPECIFIER targetSpecifier = new RSVD_BLOCK_DEVICE_TARGET_SPECIFIER();
            targetSpecifier.RsvdBlockDeviceTargetNamespace = TargetNamespaceType.SnapshotId;
            targetSpecifier.TargetInformationSnapshot.SnapshotID = snapshotId;
            targetSpecifier.TargetInformationSnapshot.SnapshotType = Snapshot_Type.SvhdxSnapshotTypeVM;
            return TypeMarshal.ToBytes(targetSpecifier);
        }

        /// <summary>
        /// Query shared virtual disk support
        /// </summary>
        /// <param name="response">Shared virtual disk support response to receive from RSVD server</param>
        /// <param name="requestPayload">Payload of the ioctl request, default value is null</param>
        /// <returns>Status of response packet</returns>
        public uint QuerySharedVirtualDiskSupport(out SVHDX_SHARED_VIRTUAL_DISK_SUPPORT_RESPONSE? response, byte[] requestPayload = null)
        {
            byte[] output;
            uint status;

            // FSCTL_QUERY_SHARED_VIRTUAL_DISK_SUPPORT is a fsctl code.
            // So cast the type from CtlCode_Values to FsCtlCode is to use the correct overloaded function 
            // IoControl(TimeSpan timeout, FsCtlCode controlCode, byte[] input, out byte[] output)
            status = transport.IoControl(this.timeout, (FsCtlCode)CtlCode_Values.FSCTL_QUERY_SHARED_VIRTUAL_DISK_SUPPORT, requestPayload, out output);

            if (status != Smb2Status.STATUS_SUCCESS)
            {
                response = null;
                return status;
            }

            response = TypeMarshal.ToStruct<SVHDX_SHARED_VIRTUAL_DISK_SUPPORT_RESPONSE>(output);
            return status;
        }
    }
}

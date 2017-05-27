// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rsvd
{
    #region const
    /// <summary>
    /// Consts defined in section 2.2.1
    /// </summary>
    public static class RsvdConst
    {
        /// <summary>
        /// Generic length of command descriptor block
        /// </summary>
        public const ushort RSVD_CDB_GENERIC_LENGTH = 0x10;

        /// <summary>
        /// SENSE buffer size
        /// </summary>
        public const ushort RSVD_SCSI_SENSE_BUFFER_SIZE = 0x14;

        /// <summary>
        /// Maximum length of the InitiatorHostName field in section 2.2.4.12
        /// </summary>
        public const ushort RSVD_MAXIMUM_NAME_LENGTH = 0x7E;

        /// <summary>
        /// Specifies the size, in bytes, of the SVHDX_TUNNEL_SCSI_REQUEST structure excluding the DataBuffer field
        /// </summary>
        public const ushort SVHDX_TUNNEL_SCSI_REQUEST_LENGTH = 0x24;

        /// <summary>
        /// Used to set EaName when open a shared vhd set with a target specifier
        /// </summary>
        public const string RSVD_TARGET_SPECIFIER_EA = "RSVD_TARGET_SPECIFIER_EA";

    }
    #endregion

    #region FSCTL_SVHDX_SYNC_TUNNEL_REQUEST

    /// <summary>
    /// Protocol version used in RSVD
    /// </summary>
    public enum RSVD_PROTOCOL_VERSION : uint
    {
        /// <summary>
        /// RSVD protocol version 1
        /// Windows 8.1 and Windows server 2012R2 set ClientServiceVersion to RSVD protocol version 1
        /// </summary>
        RSVD_PROTOCOL_VERSION_1 = 0x00000001,

        /// <summary>
        /// RSVD protocol version 2
        /// Windows vNext and Windows server vNext set ClientServiceVersion to RSVD protocol version 2
        /// </summary>
        RSVD_PROTOCOL_VERSION_2 = 0x00000002
    }

    /// <summary>
    /// Control codes used in shared virtual disk operations
    /// </summary>
    public enum RSVD_TUNNEL_OPERATION_CODE : uint
    {
        /// <summary>
        /// Query shared virtual disk file and server information
        /// </summary>
        RSVD_TUNNEL_GET_FILE_INFO_OPERATION = 0x02001001,

        /// <summary>
        /// Perform SCSI operation
        /// </summary>
        RSVD_TUNNEL_SCSI_OPERATION = 0x02001002,

        /// <summary>
        /// Query shared virtual disk connection status
        /// </summary>
        RSVD_TUNNEL_CHECK_CONNECTION_STATUS_OPERATION = 0x02001003,

        /// <summary>
        /// Query sense error code of previously failed request
        /// </summary>
        RSVD_TUNNEL_SRB_STATUS_OPERATION = 0x02001004,

        /// <summary>
        /// Query disk information
        /// </summary>
        RSVD_TUNNEL_GET_DISK_INFO_OPERATION = 0x02001005,

        /// <summary>
        /// Perform shared virtual disk validation
        /// </summary>
        RSVD_TUNNEL_VALIDATE_DISK_OPERATION = 0x02001006,

        /// <summary>
        /// Query the progress of an ongoing meta operation
        /// </summary>
        RSVD_TUNNEL_META_OPERATION_QUERY_PROGRESS = 0x02002002,

        /// <summary>
        /// Query the information of the VHD set file
        /// </summary>
        RSVD_TUNNEL_VHDSET_QUERY_INFORMATION = 0x02002005,

        /// <summary>
        /// Delete a previously created snapshot
        /// </summary>
        RSVD_TUNNEL_DELETE_SNAPSHOT = 0x02002006,

        /// <summary>
        /// Get change tracking parameters
        /// </summary>
        RSVD_TUNNEL_CHANGE_TRACKING_GET_PARAMETERS = 0x02002008,

        /// <summary>
        /// Start change tracking
        /// </summary>
        RSVD_TUNNEL_CHANGE_TRACKING_START = 0x02002009,

        /// <summary>
        /// Stop change tracking
        /// </summary>
        RSVD_TUNNEL_CHANGE_TRACKING_STOP = 0x0200200A,

        /// <summary>
        /// Get a list of ranges that have changed in the virtual disk since the indicated snapshot ID. 
        /// </summary>
        RSVD_TUNNEL_QUERY_VIRTUAL_DISK_CHANGES = 0x0200200C,

        /// <summary>
        /// Start a meta operation
        /// </summary>
        RSVD_TUNNEL_META_OPERATION_START = 0x02002101
    }

    /// <summary>
    /// Indicates which component has originated or issued the operation. 
    /// </summary>
    public enum OriginatorFlag : uint
    {
        /// <summary>
        /// Shared virtual disk file to be opened as a virtual SCSI disk device
        /// </summary>
        SVHDX_ORIGINATOR_PVHDPARSER = 0x00000001,

        /// <summary>
        /// Shared virtual disk file to be opened in underlying object store
        /// </summary>
        SVHDX_ORIGINATOR_VHDMP = 0x00000004
    }
    /// <summary>
    /// Indicates the type of disk type
    /// </summary>
    public enum DISK_TYPE : uint
    {
        /// <summary>
        /// Indicates that the type of the disk is fixed.
        /// </summary>
        VHD_TYPE_FIXED = 0x00000002,

        /// <summary>
        /// Indicates that the type of the disk is dynamic.
        /// </summary>
        VHD_TYPE_DYNAMIC = 0x00000003,
    }

    /// <summary>
    /// Indicates the type of the disk. 
    /// </summary>
    public enum DISK_FORMAT : uint
    {
        /// <summary>
        /// Indicates that the type of the disk is shared virtual disk.
        /// </summary>
        VIRTUAL_STORAGE_TYPE_DEVICE_VHDX = 0x00000003,

        /// <summary>
        /// Indicates that the type of the disk is shared virtual disk set.
        /// </summary>
        VIRTUAL_STORAGE_TYPE_DEVICE_VHDS = 0x00000004
    }
    /// <summary>
    /// SCSI command descriptor block flags
    /// </summary>
    public enum SRB_FLAGS : uint
    {
        
        SRB_FLAGS_NO_DATA_TRANSFER = 0,
        SRB_FLAGS_QUEUE_ACTION_ENABLE = 0x00000002,
        SRB_FLAGS_DISABLE_DISCONNECT = 0x00000004,
        SRB_FLAGS_DISABLE_SYNCH_TRANSFER = 0x00000008,
        SRB_FLAGS_BYPASS_FROZEN_QUEUE = 0x00000010,
        SRB_FLAGS_DISABLE_AUTOSENSE = 0x00000020,

        /// <summary>
        /// The application is sending data to the server
        /// </summary>
        SRB_FLAGS_DATA_IN = 0x00000040,

        /// <summary>
        /// The application is requesting data from the server
        /// </summary>
        SRB_FLAGS_DATA_OUT = 0x00000080,
        SRB_FLAGS_UNSPECIFIED_DIRECTION = SRB_FLAGS_DATA_IN | SRB_FLAGS_DATA_OUT,
        SRB_FLAGS_NO_QUEUE_FREEZE = 0x00000100,
        SRB_FLAGS_ADAPTER_CACHE_ENABLE = 0x00000200,
        SRB_FLAGS_FREE_SENSE_BUFFER = 0x00000400,
        SRB_FLAGS_D3_PROCESSING = 0x00000800,
        SRB_FLAGS_IS_ACTIVE = 0x00010000,
        SRB_FLAGS_ALLOCATED_FROM_ZONE = 0x00020000,
        SRB_FLAGS_SGLIST_FROM_POOL = 0x00040000,
        SRB_FLAGS_BYPASS_LOCKED_QUEUE = 0x00080000,
        SRB_FLAGS_NO_KEEP_AWAKE = 0x00100000,
        SRB_FLAGS_PORT_DRIVER_ALLOCSENSE = 0x00200000,
        SRB_FLAGS_PORT_DRIVER_SENSEHASPORT = 0x00400000,
        SRB_FLAGS_DONT_START_NEXT_PACKET = 0x00800000,
        SRB_FLAGS_PORT_DRIVER_RESERVED = 0x0F000000,
        SRB_FLAGS_CLASS_DRIVER_RESERVED = 0xF0000000
    }

    /// <summary>
    /// Indicates error messages from the server to the client
    /// </summary>
    public enum SRB_STATUS : byte
    {
        /// <summary>
        /// Indicates the request is in progress.
        /// </summary>
        SRB_STATUS_PENDING = 0x00,

        /// <summary>
        /// Indicates the request was completed successfully.
        /// </summary>
        SRB_STATUS_SUCCESS = 0x01,

        /// <summary>
        /// Indicates the request was aborted.
        /// </summary>
        SRB_STATUS_ABORTED = 0x02,

        /// <summary>
        /// Indicates the request was completed with an error in the SCSI bus status.
        /// </summary>
        SRB_STATUS_ERROR = 0x04,

        /// <summary>
        /// Indicates that the Shared Virtual Disk does not support the given request.
        /// </summary>
        SRB_STATUS_INVALID_REQUEST = 0x06,

        /// <summary>
        /// Indicates the Shared Virtual Disk device did not respond.
        /// </summary>
        SRB_STATUS_NO_DEVICE = 0x08,

        /// <summary>
        /// Indicates the SCSI device selection timed out.
        /// </summary>
        SRB_STATUS_SELECTION_TIMEOUT = 0x0A,

        /// <summary>
        /// Indicates that a data overrun or underrun error occurred. 
        /// </summary>
        SRB_STATUS_DATA_OVERRUN = 0x12,
    }

    /// <summary>
    /// This field indicates the type of the operation
    /// </summary>
    public enum Operation_Type : uint
    {
        /// <summary>
        /// The meta-operation requests to resize the target file
        /// </summary>
        SvhdxMetaOperationTypeResize = 0x00000000,

        /// <summary>
        /// The meta-operation is part of a snapshot creation process
        /// </summary>
        SvhdxMetaOperationTypeCreateSnapshot = 0x00000001,

        /// <summary>
        /// The meta-operation requests that the server optimizes the target file
        /// </summary>
        SvhdxMetaOperationTypeOptimize = 0x00000002,

        /// <summary>
        /// The meta-operation requests that the server extract a differencing VHD from the target file
        /// </summary>
        SvhdxMetaOperationTypeExtractVHD = 0x00000003,

        /// <summary>
        /// The meta-operation requests that the given virtual disk file be converted to a VHD set
        /// </summary>
        SvhdxMetaOperationTypeConvertToVHDSet = 0x00000004,

        /// <summary>
        /// The meta-operation requests that a specific snapshot be applied to a shared VHD Set.
        /// </summary>
        SvhdxMetaOperationTypeApplySnapshot = 0x00000005
    }

    /// <summary>
    /// This field indicates the possible snapshot types
    /// </summary>
    public enum Snapshot_Type : uint
    {
        /// <summary>
        /// Indicates invalid snapshot type
        /// </summary>
        SvhdxSnapshotTypeInvalid = 0x00000000,

        /// <summary>
        /// A snapshot created as part of routine Virtual Machine operations
        /// </summary>
        SvhdxSnapshotTypeVM = 0x00000001,

        /// <summary>
        /// A snapshot created as part of a continuous data protection (CDP) process
        /// </summary>
        SvhdxSnapshotTypeCDP = 0x00000003
    }

    /// <summary>
    /// Indicates whether the change tracking is enabled when the snapshot is taken
    /// </summary>
    public enum Snapshot_Flags : uint
    {
        /// <summary>
        /// Zero
        /// </summary>
        SVHDX_SNAPSHOT_FLAG_ZERO = 0x00000000,

        /// <summary>
        /// A request is made for change tracking to be enabled when snapshot is taken
        /// </summary>
        SVHDX_SNAPSHOT_DISK_FLAG_ENABLE_CHANGE_TRACKING = 0x00000001
    }

    /// <summary>
    /// This field indicates the values for each stage while creating snapshot
    /// </summary>
    public enum Stage_Values : uint
    {
        /// <summary>
        /// No stage present in this field
        /// </summary>
        SvhdxSnapshotStageInvalid = 0x00000000,

        /// <summary>
        /// Perform any required initialization so that the appropriate type of snapshot can be taken
        /// </summary>
        SvhdxSnapshotStageInitialize = 0x00000001,

        /// <summary>
        /// Temporarily pause all IO against the target virtual device. 
        /// </summary>
        SvhdxSnapshotStageBlockIO = 0x00000002,

        /// <summary>
        /// Switch aspects of the underlying object store so that the appropriate snapshot is taken
        /// </summary>
        SvhdxSnapshotStageSwitchObjectStore = 0x00000003,

        /// <summary>
        /// Allow further IO against the target virtual device
        /// </summary>
        SvhdxSnapshotStageUnblockIO = 0x00000004,

        /// <summary>
        /// Tear down any state associated with a snapshot of the target virtual device
        /// </summary>
        SvhdxSnapshotStageFinalize = 0x00000005
    }

    /// <summary>
    /// Indicates an error condition when attempting to perform change-tracking operations
    /// </summary>
    public enum ChangeTracking_ErrorStatus : uint
    {
        /// <summary>
        /// Change-tracking is active on the virtual disk without any error
        /// </summary>
        SVHDX_TUNNEL_CHANGE_TRACKING_STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// Change-tracking is not initialized on the virtual disk
        /// </summary>
        SVHDX_TUNNEL_CHANGE_TRACKING_NOT_INITIALIZED = 0xC03A0020,

        /// <summary>
        /// The log file size has exceeded the client specified maximum size
        /// </summary>
        SVHDX_TUNNEL_CHANGE_TRACKING_LOGSIZE_EXCEEDED_MAXSIZE = 0xC03A0021,

        /// <summary>
        /// A virtual disk write was detected to be missing from the current log file
        /// </summary>
        SVHDX_TUNNEL_CHANGE_TRACKING_VHD_CHANGED_OFFLINE = 0xC03A0022,

        /// <summary>
        /// A change-tracking operation cannot be performed on the virtual disk in its current state
        /// </summary>
        SVHDX_TUNNEL_CHANGE_TRACKING_INVALID_TRACKING_STATE = 0xC03A0023,

        /// <summary>
        /// An inconsistent log file detected
        /// </summary>
        SVHDX_TUNNEL_CHANGE_TRACKING_INCONSISTENT_TRACKING_FILE = 0xC03A0024
    }

    /// <summary>
    /// The information type requested
    /// </summary>
    public enum VHDSet_InformationType : uint
    {
        /// <summary>
        /// Returns a list of snapshots
        /// </summary>
        SvhdxVHDSetInformationTypeSnapshotList = 0x00000002,

        /// <summary>
        /// Returns the details about a specific snapshot entry
        /// </summary>
        SvhdxVHDSetInformationTypeSnapshotEntry = 0x00000005,

        /// <summary>
        /// Queries whether the target set file needs optimization
        /// </summary>
        SvhdxVHDSetInformationTypeOptimizeNeeded = 0x00000008,

        /// <summary>
        /// Returns the oldest CDP snapshot present in the target VHD set file
        /// </summary>
        SvhdxVHDSetInformationTypeCdpSnapshotRoot = 0x00000009,

        /// <summary>
        /// Returns the list of CDP snapshot IDs that are active for the VHD set file
        /// </summary>
        SvhdxVHDSetInformationTypeCdpSnapshotActiveList = 0x0000000A,

        /// <summary>
        /// Returns the list of CDP snapshot IDs that are inactive in the VHD set file
        /// </summary>
        SvhdxVHDSetInformationTypeCdpSnapshotInactiveList = 0x0000000C
    }

    /// <summary>
    /// A flag to indicate if the snapshot needs to be persisted
    /// </summary>
    public enum PersistReference_Flags : uint
    {
        /// <summary>
        /// Indicates that the snapshot will be deleted
        /// This field MUST be set to FALSE if SnapshotType is SvhdxSnapshotTypeVM
        /// </summary>
        PersistReferenceFalse = 0x00000000,

        /// <summary>
        /// Indicates that the snapshot will be stored as a reference without any data
        /// </summary>
        PersistReferenceTrue = 0x00000001
    }

    /// <summary>
    /// This field MUST be set to zero or more of the following values
    /// </summary>
    public enum ExtractSnapshot_Flags : uint
    {
        /// <summary>
        /// Indicates the flag is ZERO
        /// </summary>
        SVHDX_EXTRACT_SNAPSHOTS_FLAG_ZERO = 0x00000000,

        /// <summary>
        /// Request for VHD Set to be deleted on close after exporting the differences between specified snapshots
        /// </summary>
        SVHDX_EXTRACT_SNAPSHOTS_FLAG_DELETE_ON_CLOSE = 0x00000001
    }

    /// <summary>
    /// State of the shared virtual disk Open
    /// </summary>
    public enum HandleState : uint
    {
        /// <summary>
        /// The Open is not opened as a shared virtual disk.
        /// </summary>
        HandleStateNone = 0x00000000,

        /// <summary>
        /// The shared virtual disk file is opened as a shared virtual disk by another Open.
        /// </summary>
        HandleStateFileShared = 0x00000001,

        /// <summary>
        /// The shared virtual disk file is opened as a shared virtual disk by this Open.
        /// </summary>
        HandleStateShared = 0x00000003
    }

    /// <summary>
    /// The following is a list of possible RSVD error codes that can be returned by the server.
    /// </summary>
    public enum RsvdStatus : uint
    {
        STATUS_SVHDX_SUCCESS = 0,

        /// <summary>
        /// Sense error data was stored on server
        /// </summary>
        STATUS_SVHDX_ERROR_STORED = 0xC05C0000,

        /// <summary>
        /// Sense error data is not available
        /// </summary>
        STATUS_SVHDX_ERROR_NOT_AVAILABLE = 0xC05CFF00,

        /// <summary>
        /// Unit Attention data is available for the initiator to query
        /// </summary>
        STATUS_SVHDX_UNIT_ATTENTION_AVAILABLE = 0xC05CFF01,

        /// <summary>
        /// The data capacity of the device has changed, resulting in a Unit Attention condition
        /// </summary>
        STATUS_SVHDX_UNIT_ATTENTION_CAPACITY_DATA_CHANGED = 0xC05CFF02,

        /// <summary>
        /// A previous operation resulted in this initiator's reservations being preempted, resulting in a Unit Attention condition
        /// </summary>
        STATUS_SVHDX_UNIT_ATTENTION_RESERVATIONS_PREEMPTED = 0xC05CFF03,

        /// <summary>
        /// A previous operation resulted in this initiator's reservations being released, resulting in a Unit Attention condition
        /// </summary>
        STATUS_SVHDX_UNIT_ATTENTION_RESERVATIONS_RELEASED = 0xC05CFF04,

        /// <summary>
        /// A previous operation resulted in this initiator's registrations being preempted, resulting in a Unit Attention condition
        /// </summary>
        STATUS_SVHDX_UNIT_ATTENTION_REGISTRATIONS_PREEMPTED = 0xC05CFF05,

        /// <summary>
        /// Represents the data storage format of the device has changed, resulting in a Unit Attention condition
        /// </summary>
        STATUS_SVHDX_UNIT_ATTENTION_OPERATING_DEFINITION_CHANGED = 0xC05CFF06,

        /// <summary>
        /// The current initiator is not allowed to perform the SCSI command because of a reservation conflict
        /// </summary>
        STATUS_SVHDX_RESERVATION_CONFLICT = 0xC05CFF07,

        /// <summary>
        /// File on which open is performed is of wrong type
        /// </summary>
        STATUS_SVHDX_WRONG_FILE_TYPE = 0xC05CFF08,

        /// <summary>
        /// Protocol version in request is not equal to 1
        /// </summary>
        STATUS_SVHDX_VERSION_MISMATCH = 0xC05CFF09
    }

    /// <summary>
    /// Indicates the capabilities supported by the server.
    /// </summary>
    public enum SharedVirtualDiskSupported : uint
    {
        /// <summary>
        /// Indicates the server supports shared virtual disks.
        /// </summary>
        SharedVirtualDiskSupported = 0x00000001,

        /// <summary>
        /// Indicates the server supports shared virtual disks and version 2 operations
        /// </summary>
        SharedVirtualDiskVer2OperationsSupported = 0x00000003,

        /// <summary>
        /// The server supports shared virtual disks and all snapshot types.
        /// </summary>
        SharedVirtualDiskSnapshotsSupported = 0x00000007
    }

    /// <summary>
    /// Indicates the RSVD protocol version supported by the server.
    /// </summary>
    public enum ServerServiceVersion : uint
    {
        /// <summary>
        /// Windows Server 2012 R2 sets ServerServiceVersion to RSVD protocol version 1 (0x00000001)
        /// </summary>
        ProtocolVersion1 = 0x00000001,

        /// <summary>
        /// Windows Server vNext sets ServerServiceVersion to RSVD protocol version 2(0x00000002)
        /// </summary>
        ProtocolVersion2 = 0x00000002
    }

    /// <summary>
    /// Indicates the type of the target namespace used in RSVD_BLOCK_DEVICE_TARGET_SPECIFIER Structure
    /// </summary>
    public enum TargetNamespaceType : uint
    {
        /// <summary>
        /// The target identifier is a snapshot ID.
        /// </summary>
        SnapshotId = 0x00000000
    }

    /// <summary>
    /// The RSVD tunnel header is the header of RSVD Protocol operations.
    /// </summary>
    public struct SVHDX_TUNNEL_OPERATION_HEADER
    {
        /// <summary>
        /// The command code of this packet.
        /// </summary>
        [StaticSize(4)]
        public RSVD_TUNNEL_OPERATION_CODE OperationCode;

        /// <summary>
        /// The client SHOULD set this field to zero, and the server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public uint Status;

        /// <summary>
        /// A value that uniquely identifies an operation request and response for all requests sent by this client.
        /// </summary>
        [StaticSize(8)]
        public ulong RequestId;
    }

    /// <summary>
    /// The SVHDX_TUNNEL_FILE_INFO_RESPONSE packet is sent by the server 
    /// in response to an RSVD_TUNNEL_GET_FILE_INFO_OPERATION packet.
    /// </summary>
    public struct SVHDX_TUNNEL_FILE_INFO_RESPONSE
    {
        /// <summary>
        /// The current version of the protocol running on the server.
        /// </summary>
        [StaticSize(4)]
        public uint ServerVersion;

        /// <summary>
        /// A 32-bit unsigned integer which indicates the sector size of the shared virtual disk.
        /// </summary>
        [StaticSize(4)]
        public uint SectorSize;

        /// <summary>
        /// A 32-bit unsigned integer which indicates the physical sector size of the shared virtual disk.
        /// </summary>
        [StaticSize(4)]
        public uint PhysicalSectorSize;

        /// <summary>
        /// This field MUST be set to zero, and the client MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public uint Reserved;

        /// <summary>
        /// A 64-bit unsigned integer which indicates the virtual size of the shared virtual disk.
        /// </summary>
        [StaticSize(8)]
        public ulong VirtualSize;
    }

    /// <summary>
    /// The SVHDX_TUNNEL_SRB_STATUS_REQUEST packet is sent by the client to get the sense error code of a previously failed request.
    /// </summary>
    public struct SVHDX_TUNNEL_SRB_STATUS_REQUEST
    {
        /// <summary>
        /// The client MUST set this field to the least significant byte of the error code of a previously failed request.
        /// </summary>
        [StaticSize(1)]
        public byte StatusKey;

        /// <summary>
        /// This field MUST be set to zero, and MUST be ignored on receipt.
        /// </summary>
        [StaticSize(27)]
        public byte[] Reserved;
    }

    /// <summary>
    /// The SVHDX_TUNNEL_SRB_STATUS_RESPONSE packet is sent 
    /// by the server in a response to the RSVD_TUNNEL_SRB_STATUS_OPERATION.
    /// </summary>
    public struct SVHDX_TUNNEL_SRB_STATUS_RESPONSE
    {
        /// <summary>
        /// The server MUST set this field to the status key value received in the request.
        /// </summary>
        [StaticSize(1)]
        public byte StatusKey;

        /// <summary>
        /// An 8-bit field used to communicate error messages from the server to the client.
        /// </summary>
        [StaticSize(1)]
        public byte SrbStatus;

        /// <summary>
        /// An 8-bit field used to communicate the SCSI status that was returned by the shared virtual disk.
        /// </summary>
        [StaticSize(1)]
        public byte ScsiStatus;

        /// <summary>
        /// The length, in bytes, of the request sense data buffer.
        /// </summary>
        [StaticSize(1)]
        public byte SenseInfoExLength;

        /// <summary>
        /// A buffer of maximum size 20 bytes that contains the sense data.
        /// </summary>
        [Size("SenseInfoExLength")]
        public byte[] SenseDataEx;
    }

    /// <summary>
    /// The SVHDX_TUNNEL_DISK_INFO_REQUEST packet is sent by the client to get shared virtual disk information.
    /// </summary>
    public struct SVHDX_TUNNEL_DISK_INFO_REQUEST
    {
        /// <summary>
        /// The client MUST set this field to zero, and the server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(8)]
        public ulong Reserved1;

        /// <summary>
        /// The client MUST set this field to zero, and the server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public uint BlockSize;

        /// <summary>
        /// The client MUST set this field to zero, and the server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(16)]
        public Guid LinkageID;

        /// <summary>
        /// The client MUST set this field to zero, and the server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(1)]
        public bool IsMounted;

        /// <summary>
        /// The client MUST set this field to zero, and the server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(1)]
        public bool Is4kAligned;

        /// <summary>
        /// The client MUST set this field to zero, and the server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public ushort Reserved;

        /// <summary>
        /// The client MUST set this field to zero, and the server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(8)]
        public ulong FileSize;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. 
        /// The client MUST set this field to zero, and the server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(16)]
        public Guid VirtualDiskId;
    }

    /// <summary>
    /// The SVHDX_TUNNEL_DISK_INFO_RESPONSE packet is sent by the server in response to an RSVD_TUNNEL_VALIDATE_DISK_OPERATION.
    /// </summary>
    public struct SVHDX_TUNNEL_DISK_INFO_RESPONSE
    {
        /// <summary>
        /// Indicates the type of disk type. 
        /// </summary>
        [StaticSize(4)]
        public DISK_TYPE DiskType;
        /// <summary>
        /// Indicates the type of the disk.
        /// </summary>
        [StaticSize(4)]
        public DISK_FORMAT DiskFormat;

        /// <summary>
        /// Specifics the disk block size in bytes.
        /// </summary>
        [StaticSize(4)]
        public uint BlockSize;

        /// <summary>
        /// A GUID that specifies the linkage identification of the disk.
        /// </summary>
        [StaticSize(16)]
        public Guid LinkageID;

        /// <summary>
        /// A Boolean value. 
        /// Zero represents FALSE (0x00), indicating that the disk is not ready for read or write operations. 
        /// One represents TRUE (0x01), indicating that the disk is mounted and ready for read or write operations.
        /// </summary>
        [StaticSize(1)]
        public bool IsMounted;

        /// <summary>
        /// A Boolean value. 
        /// Zero represents FALSE, indicating disk sectors are not aligned to 4 kilobytes. 
        /// One represents TRUE, indicating disk sectors are aligned to 4 kilobytes.
        /// </summary>
        [StaticSize(1)]
        public bool Is4kAligned;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved.
        /// The client SHOULD set this field to zero, and the server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public ushort Reserved;

        /// <summary>
        /// The size of shared virtual disk, in bytes.
        /// </summary>
        [StaticSize(8)]
        public ulong FileSize;

        /// <summary>
        /// A GUID that specifies the identification of the disk.
        /// </summary>
        [StaticSize(16)]
        public Guid VirtualDiskId;
    }

    /// <summary>
    /// The SVHDX_TUNNEL_SCSI_REQUEST packet is sent by the client to process the SCSI request.
    /// </summary>
    public struct SVHDX_TUNNEL_SCSI_REQUEST
    {
        /// <summary>
        /// Specifies the size, in bytes, of the SVHDX_TUNNEL_SCSI_REQUEST structure.
        /// </summary>
        [StaticSize(2)]
        public ushort Length;

        /// <summary>
        /// The client SHOULD set this field to zero, and the server MUST ignore it on receipt. 
        /// </summary>
        [StaticSize(2)]
        public ushort Reserved1;

        /// <summary>
        /// The length, in bytes, of the SCSI command descriptor block. 
        /// This value MUST be less than or equal to RSVD_CDB_GENERIC_LENGTH.
        /// </summary>
        [StaticSize(1)]
        public byte CdbLength;

        /// <summary>
        /// The length, in bytes, of the request sense data buffer. 
        /// This value MUST be less than or equal to RSVD_SCSI_SENSE_BUFFER_SIZE.
        /// </summary>
        [StaticSize(1)]
        public byte SenseInfoExLength;

        /// <summary>
        /// A Boolean, indicating the SCSI command descriptor block transfer type. 
        /// The value TRUE (0x01) indicates that the operation is to store the data onto the disk. 
        /// The value FALSE (0x00) indicates that the operation is to retrieve the data from the disk.
        /// </summary>
        [StaticSize(1)]
        public bool DataIn;

        /// <summary>
        /// This field MUST be set to 0x00, and MUST be ignored on receipt.
        /// </summary>
        [StaticSize(1)]
        public byte Reserved2;

        /// <summary>
        /// An optional, application-provided flag to indicate the options of the SCSI request.
        /// </summary>
        [StaticSize(4)]
        public SRB_FLAGS SrbFlags;

        /// <summary>
        /// The length, in bytes, of the additional data placed in the DataBuffer field.
        /// </summary>
        [StaticSize(4)]
        public uint DataTransferLength;

        /// <summary>
        /// A buffer that contains the SCSI command descriptor block.
        /// </summary>
        [StaticSize(16)]
        public byte[] CDBBuffer;

        /// <summary>
        /// This field SHOULD be set to 0x00000000; it MUST be ignored on receipt.
        /// </summary>
        [StaticSize(4)]
        public uint Reserved3;

        /// <summary>
        /// A variable-length buffer that contains the additional buffer, as described by the DataTransferLength field.
        /// </summary>
        [Size("DataTransferLength")]
        public byte[] DataBuffer;
    }

    /// <summary>
    /// The SVHDX_TUNNEL_SCSI_RESPONSE packet is sent by the server in response to the operation RSVD_TUNNEL_SCSI_OPERATION.
    /// </summary>
    public struct SVHDX_TUNNEL_SCSI_RESPONSE
    {
        /// <summary>
        /// Specifies the size, in bytes, of the SVHDX_TUNNEL_SCSI_RESPONSE structure.
        /// </summary>
        [StaticSize(2)]
        public ushort Length;

        /// <summary>
        /// An 8-bit field used to communicate error messages from the server to the client.
        /// </summary>
        [StaticSize(1)]
        public SRB_STATUS SrbStatus;

        /// <summary>
        /// An 8-bit field used to communicate the SCSI status that was returned by the target device.
        /// </summary>
        [StaticSize(1)]
        public byte ScsiStatus;

        /// <summary>
        /// The length, in bytes, of the SCSI command descriptor block.
        /// </summary>
        [StaticSize(1)]
        public byte CdbLength;

        /// <summary>
        /// The length, in bytes, of the request sense data buffer.
        /// </summary>
        [StaticSize(1)]
        public byte SenseInfoExLength;

        /// <summary>
        /// A Boolean used to indicate the command descriptor block data-transfer type. 
        /// TRUE (0x01), indicates that the operation is to store the data onto the disk.
        /// The value FALSE (0x00) indicates that the operation is to retrieve the data from the disk.
        /// </summary>
        [StaticSize(1)]
        public bool DataIn;

        /// <summary>
        /// This field MUST be set to zero, and MUST be ignored on receipt.
        /// </summary>
        [StaticSize(1)]
        public byte Reserved;

        /// <summary>
        /// Special flags to indicate options of the SCSI response.
        /// </summary>
        [StaticSize(1)]
        public SRB_FLAGS SrbFlags;

        /// <summary>
        /// The length, in bytes, of the additional data placed in the DataBuffer field.
        /// </summary>
        [StaticSize(4)]
        public uint DataTransferLength;

        /// <summary>
        /// A buffer of maximum size 20 bytes that contains the command descriptor buffer.
        /// </summary>
        [Size("SenseInfoExLength")]
        public byte[] SenseDataEx;

        /// <summary>
        /// A variable-length buffer that contains the additional buffer, as described by the DataTransferLength field.
        /// </summary>
        [Size("DataTransferLength")]
        public byte[] DataBuffer;
    }

    /// <summary>
    /// The SVHDX_TUNNEL_VALIDATE_DISK_REQUEST packet is sent by the client to validate the shared virtual disk. 
    /// </summary>
    public struct SVHDX_TUNNEL_VALIDATE_DISK_REQUEST
    {
        /// <summary>
        /// The client MUST set all 56 bytes to zero and the server MUST ignore them on receipt.
        /// </summary>
        [StaticSize(56)]
        public byte[] Reserved;
    }

    /// <summary>
    /// The SVHDX_TUNNEL_VALIDATE_DISK_RESPONSE packet is sent by the server in a response to the operation RSVD_TUNNEL_VALIDATE_DISK_OPERATION. 
    /// </summary>
    public struct SVHDX_TUNNEL_VALIDATE_DISK_RESPONSE
    {
        /// <summary>
        /// A Boolean used to indicate whether the disk is currently valid. 
        /// Zero indicates the disk is not valid and non-zero indicates it is valid.
        /// </summary>
        [StaticSize(1)]
        public bool IsValidDisk;
    }

    #endregion

    /// <summary>
    /// The SVHDX_SHARED_VIRTUAL_DISK_SUPPORT_RESPONSE packet is sent 
    /// by the server in a response to an FSCTL_QUERY_SHARED_VIRTUAL_DISK_SUPPORT request.
    /// </summary>
    public struct SVHDX_SHARED_VIRTUAL_DISK_SUPPORT_RESPONSE
    {
        /// <summary>
        /// This field is used to indicate whether the server supports shared virtual disks. 
        /// This field MUST be 0x00000001.
        /// </summary>
        [StaticSize(4)]
        public uint SharedVirtualDiskSupport;

        /// <summary>
        /// This field is used to indicate the state of the shared virtual disk Open. 
        /// </summary>
        [StaticSize(4)]
        public HandleState SharedVirtualDiskHandleState;
    }

    /// <summary>
    /// The SVHDX_META_OPERATION_START_REQUEST packet is sent by the client to start a 
    /// meta-operation on the shared virtual disk file
    /// </summary>
    public struct SVHDX_META_OPERATION_START_REQUEST
    {
        /// <summary>
        /// This field indicates the transaction ID for the operation.
        /// This field MUST be set to a globally unique ID for each meta-operation
        /// </summary>
        [StaticSize(16)]
        public Guid TransactionId;

        /// <summary>
        /// This field indicates the type of operation.
        /// </summary>
        [StaticSize(4)]
        public Operation_Type OperationType;

        /// <summary>
        /// There should be a 4-byte padding to align 
        /// </summary>
        [StaticSize(4)]
        public uint Padding;
    }

    /// <summary>
    /// This structure is used to send the additional parameters for snapshot creation
    /// </summary>
    public struct SVHDX_META_OPERATION_CREATE_SNAPSHOT
    {
        /// <summary>
        /// The type of snapshot
        /// </summary>
        [StaticSize(4)]
        public Snapshot_Type SnapshotType;

        /// <summary>
        /// This field MUST be set to zero or a combination of the following values
        /// </summary>
        [StaticSize(4)]
        public Snapshot_Flags Flags;

        /// <summary>
        /// The first stage
        /// </summary>
        [StaticSize(4)]
        public Stage_Values Stage1;

        /// <summary>
        /// The second stage
        /// </summary>
        [StaticSize(4)]
        public Stage_Values Stage2;

        /// <summary>
        /// The third stage
        /// </summary>
        [StaticSize(4)]
        public Stage_Values Stage3;

        /// <summary>
        /// The fourth stage
        /// </summary>
        [StaticSize(4)]
        public Stage_Values Stage4;

        /// <summary>
        /// The fifth stage
        /// </summary>
        [StaticSize(4)]
        public Stage_Values Stage5;

        /// <summary>
        /// The sixth stage
        /// </summary>
        [StaticSize(4)]
        public Stage_Values Stage6;

        /// <summary>
        /// The ID to assign to the snapshot
        /// </summary>
        [StaticSize(16)]
        public Guid SnapshotId;

        /// <summary>
        /// The size of any parameters included with the create snapshot request
        /// If the SnapshotType is SvhdxSnapshotTypeVM, then this field MUST be set to 0
        /// If the SnapshotType is SvhdxSnapshotTypeCDP, then this field MUST NOT be set to 0
        /// </summary>
        [StaticSize(4)]
        public uint ParametersPayloadSize;

        [StaticSize(24)]
        public byte[] Padding;
    }

    /// <summary>
    /// This structure is used to send additional CDB parameters
    /// </summary>
    public struct SVHDX_META_OPERATION_CREATE_CDP_PARAMETER
    {
        /// <summary>
        /// The offset, in bytes, of the LogFileName from the CdpParameters field
        /// </summary>
        [StaticSize(4)]
        public uint LogFileNameOffset;

        /// <summary>
        /// A GUID associated with the log file
        /// </summary>
        [StaticSize(16)]
        public Guid LogFileId;

        /// <summary>
        /// A log file name containing a null-terminated Unicode UTF-16 string
        /// </summary>
        [String(StringEncoding.Unicode)]
        public string LogFileName;
    }

    /// <summary>
    /// This structure is used to export the differences between different VM snapshots
    /// </summary>
    public struct SVHDX_META_OPERATION_EXTRACT
    {
        /// <summary>
        /// The type of snapshot
        /// </summary>
        [StaticSize(4)]
        public Snapshot_Type snapshotType;

        /// <summary>
        /// This value MUST be set to 0 by the client and MUST be ignored by the server
        /// </summary>
        [StaticSize(4)]
        public uint Padding;

        /// <summary>
        /// This field MUST be set to zero or more of the following values
        /// </summary>
        [StaticSize(4)]
        public ExtractSnapshot_Flags flags;

        /// <summary>
        /// A GUID that indicates the Id of the last snapshot to include in the extract request
        /// </summary>
        [StaticSize(16)]
        public Guid SourceSnapshotId;

        /// <summary>
        /// A GUID that indicates the Id of the first snapshot to include in the extract request.
        /// </summary>
        [StaticSize(16)]
        public Guid SourceLimitSnapshotId;

        /// <summary>
        /// The length, in bytes, including the NULL terminating character, of the filename to extract the data differences to
        /// </summary>
        [StaticSize(4)]
        public uint DestinationFileNameLength;

        /// <summary>
        /// A buffer containing a null-terminated Unicode UTF-16 string that indicates the filename to extract the data differences to
        /// </summary>
        [Size("DestinationFileNameLength")]
        public byte[] DestinationFileName;

        /// <summary>
        /// Padding
        /// </summary>
        [StaticSize(4)]
        public uint Padding2;
    }

    /// <summary>
    /// This structure is used by server to issue a request to convert a VHD file to a snapshot-capable VHD set
    /// </summary>
    public struct SVHDX_META_OPERATION_CONVERT_TO_VHDSET
    {
        /// <summary>
        /// The length, in bytes, including the NULL terminating character, of the DestinationVhdSetName
        /// </summary>
        [StaticSize(4)]
        public uint DestinationVhdSetNameLength;

        /// <summary>
        /// A buffer containing a null-terminated Unicode UTF-16 string that indicates the name for the new VHD set that is to be created
        /// </summary>
        [Size("DestinationVhdSetNameLength")]
        public string DestinationVhdSetName;

        [StaticSize(4)]
        public uint Padding;
    }

    /// <summary>
    /// This struct is used by server to issue a request to resize the shared virtual disk
    /// </summary>
    public struct SVHDX_META_OPERATION_RESIZE_VIRTUAL_DISK
    {
        /// <summary>
        /// This specifies the new size of the shared virtual disk
        /// </summary>
        [StaticSize(8)]
        public ulong NewSize;

        /// <summary>
        /// A nonzero value indicates that the shared virtual disk size can only expand
        /// </summary>
        [StaticSize(1)]
        public bool ExpandOnly;

        /// <summary>
        /// A nonzero value indicates that the shared virtual disk size can be less than the data it currently contains
        /// </summary>
        [StaticSize(1)]
        public bool AllowUnsafeVirtualSize;

        /// <summary>
        /// A nonzero value indicates that the shared virtual disk size can be shrunk to the data it currently contains
        /// </summary>
        [StaticSize(1)]
        public bool ShrinkToMinimumSafeSize;

        /// <summary>
        /// This value MUST be set to 0 by the client and MUST be ignored by the server
        /// </summary>
        [StaticSize(1)]
        public byte Reserved;

        [StaticSize(4)]
        public uint Padding;
    }

    /// <summary>
    /// Indicates an SVHDX_META_OPERATION_START_REQUEST with operation type SvhdxMetaOperationTypeCreateSnapshot
    /// </summary>
    public struct SVHDX_META_OPERATION_START_CREATE_SNAPSHOT_REQUEST
    {
        /// <summary>
        /// Indicates an SVHDX_META_OPERATION_START_REQUEST which includes TransactionId and OperationType
        /// </summary>
        public SVHDX_META_OPERATION_START_REQUEST startRequest;

        /// <summary>
        /// Indicates an SVHDX_META_OPERATION_CREATE_SNAPSHOT structure
        /// </summary>
        public SVHDX_META_OPERATION_CREATE_SNAPSHOT createSnapshot;
    }

    /// <summary>
    /// Indicates an SVHDX_META_OPERATION_START_REQUEST with operation type SvhdxMetaOperationTypeApplySnapshot
    /// </summary>
    public struct SVHDX_META_OPERATION_START_APPLY_SNAPSHOT_REQUEST
    {
        /// <summary>
        /// Indicates an SVHDX_META_OPERATION_START_REQUEST which includes TransactionId and OperationType
        /// </summary>
        public SVHDX_META_OPERATION_START_REQUEST startRequest;

        /// <summary>
        /// Indicates an SVHDX_APPLY_SNAPSHOT_PARAMS structure
        /// </summary>
        public SVHDX_APPLY_SNAPSHOT_PARAMS applySnapshot;
    }

    /// <summary>
    /// Indicates an SVHDX_META_OPERATION_START_REQUEST with operation type SvhdxMetaOperationTypeExtractVHD
    /// </summary>
    public struct SVHDX_META_OPERATION_START_EXTRACT_REQUEST
    {
        /// <summary>
        /// Indicates an SVHDX_META_OPERATION_START_REQUEST which includes TransactionId and OperationType
        /// </summary>
        public SVHDX_META_OPERATION_START_REQUEST startRequest;

        /// <summary>
        /// Indicates an SVHDX_META_OPERATION_EXTRACT structure
        /// </summary>
        public SVHDX_META_OPERATION_EXTRACT extract;
    }

    /// <summary>
    /// Indicates an SVHDX_META_OPERATION_START_REQUEST with operation type SvhdxMetaOperationTypeConvertToVHDSet
    /// </summary>
    public struct SVHDX_META_OPERATION_START_CONVERT_TO_VHDSET_REQUEST
    {
        /// <summary>
        /// Indicates an SVHDX_META_OPERATION_START_REQUEST which includes TransactionId and OperationType
        /// </summary>
        public SVHDX_META_OPERATION_START_REQUEST startRequest;

        /// <summary>
        /// Indicates an SVHDX_META_OPERATION_CONVERT_TO_VHDSET structure
        /// </summary>
        public SVHDX_META_OPERATION_CONVERT_TO_VHDSET convert;
    }

    public struct SVHDX_META_OPERATION_START_RESIZE_REQUEST
    {
        /// <summary>
        /// Indicates an SVHDX_META_OPERATION_START_REQUEST which includes TransactionId and OperationType
        /// </summary>
        public SVHDX_META_OPERATION_START_REQUEST startRequest;

        /// <summary>
        /// Indicates an SVHDX_META_OPERATION_RESIZE_VIRTUAL_DISK structure
        /// </summary>
        public SVHDX_META_OPERATION_RESIZE_VIRTUAL_DISK resizeRequest;
    }

    /// <summary>
    /// Indicates an SVHDX_META_OPERATION_START_REQUEST with operation type SvhdxMetaOperationTypeOptimize
    /// </summary>
    public struct SVHDX_META_OPERATION_START_OPTIMIZE_REQUEST
    {
        /// <summary>
        /// Indicates an SVHDX_META_OPERATION_START_REQUEST which includes TransactionId and OperationType
        /// </summary>
        public SVHDX_META_OPERATION_START_REQUEST startRequest;
    }

    /// <summary>
    /// The SVHDX_META_OPERATION_REPLY packet is issued by a server in reply to an SVHDX_META_OPERATION_START_REQUEST operation
    /// When the meta-operation type indicates an SvhdxMetaOperationTypeOptimize, this structure is zero bytes long
    /// When the meta-operation type indicates an SvhxdMetaOperationTypeCreateSnapshot with snapshot type SvhxdSnapshotTypeCDP, this structure is returned
    /// </summary>
    public struct SVHDX_META_OPERATION_REPLY
    {
        /// <summary>
        /// Indicates an error condition when attempting to perform change-tracking operations
        /// </summary>
        [StaticSize(4)]
        public ChangeTracking_ErrorStatus ChangeTrackingErrorStatus;
    }

    /// <summary>
    /// This packet is sent by a client as part of an RSVD_TUNNEL_VHDSET_QUERY_INFORMATION request
    /// </summary>
    public struct SVHDX_TUNNEL_VHDSET_QUERY_INFORMATION_REQUEST
    {
        /// <summary>
        /// The set file information type requested
        /// </summary>
        [StaticSize(4)]
        public VHDSet_InformationType VHDSetInformationType;

        /// <summary>
        /// The snapshot type queried by this operation
        /// </summary>
        [StaticSize(4)]
        public Snapshot_Type SnapshotType;

        /// <summary>
        /// The snapshot ID relevant to the particular request
        /// </summary>
        [StaticSize(16)]
        public Guid SnapshotId;
    }

    /// <summary>
    /// This packet is sent by a server in response to an RSVD_TUNNEL_VHDSET_QUERY_INFORMATION request 
    /// where the VHDSetInformationType is SvhdxVHDSetInformationTypeSnapshotList
    /// </summary>
    public struct SVHDX_TUNNEL_VHDSET_QUERY_INFORMATION_SNAPSHOT_LIST_RESPONSE
    {
        /// <summary>
        /// The set file information type
        /// The server MUST set this to SvhdxVHDSetInformationTypeSnapshotList
        /// </summary>
        [StaticSize(4)]
        public VHDSet_InformationType VHDSetInformationType;

        // This field is set to any value by the server and MUST be ignored by the client.
        [StaticSize(4)]
        public uint Padding;

        /// <summary>
        /// Indicates if the reply contains all snapshot IDs in the SnapshotIds field. 
        /// Zero (0x00) indicates that the SnapshotIds field is not present. 
        /// One (0x01) indicates that all snapshot IDs are present in the SnapshotIds field.
        /// </summary>
        [StaticSize(1)]
        public byte ResponseComplete;

        /// <summary>
        /// This field MUST be set to 0 by the server and MUST be ignored by the client.
        /// </summary>
        [StaticSize(3)]
        public byte[] Reserved;

        /// <summary>
        /// The number of snapshots contained in the SnapshotIds field
        /// </summary>
        [StaticSize(4)]
        public uint NumberOfSnapshots;

        /// <summary>
        /// A list of IDs of snapshots of a particular type
        /// </summary>
        [Size("NumberOfSnapshots")]
        public Guid[] SnapshotIds;
    }

    /// <summary>
    /// This packet is sent by a server in response to an SVHDX_TUNNEL_OP_VHDSET_QUERY request 
    /// where the VHDSetInformationType is SvhdxVHDSetInformationTypeSnapshotEntry
    /// </summary>
    public struct SVHDX_TUNNEL_VHDSET_QUERY_INFORMATION_SNAPSHOT_ENTRY_RESPONSE
    {
        /// <summary>
        /// The set file information type
        /// The server MUST set this to SvhdxVHDSetInformationTypeSnapshotEntry
        /// </summary>
        [StaticSize(4)]
        public VHDSet_InformationType VHDSetInformationType;

        /// <summary>
        /// The time, in milliseconds since Jan 1, 1970, when the snapshot was created
        /// </summary>
        [StaticSize(8)]
        public ulong SnapshotCreationTime;

        /// <summary>
        /// The type of snapshot
        /// </summary>
        [StaticSize(4)]
        public Snapshot_Type SnapshotType;

        /// <summary>
        /// Set to 1 when the snapshot is valid, set to 0 when the snapshot is invalid
        /// </summary>
        [StaticSize(4)]
        public uint IsValidSnapshot;

        /// <summary>
        /// The globally unique ID of the snapshot
        /// </summary>
        [StaticSize(16)]
        public Guid SnapshotId;

        /// <summary>
        /// The parent snapshot ID. 
        /// This field will be set for CDP snapshots
        /// </summary>
        [StaticSize(16)]
        public Guid ParentSnapshotId;

        /// <summary>
        /// The ID of the log file associated with this snapshot. 
        /// This field will be set for CDP snapshots
        /// </summary>
        [StaticSize(16)]
        public Guid LogFileId;
    }

    /// <summary>
    /// This packet is sent by a server in response to an RSVD_TUNNEL_VHDSET_QUERY_INFORMATION request 
    /// where the VHDSetInformationType is SvhdxVHDSetInformationTypeOptimizeNeeded
    /// </summary>
    public struct SVHDX_TUNNEL_VHDSET_QUERY_INFORMATION_OPTIMIZE_RESPONSE
    {
        /// <summary>
        /// The information type.
        /// The server MUST set this to SvhdxVHDSetInformationTypeOptimizeNeeded
        /// </summary>
        [StaticSize(4)]
        public VHDSet_InformationType VHDSetInformationType;

        /// <summary>
        /// Indicates whether optimization is needed for the target file
        /// </summary>
        [StaticSize(4)]
        public uint OptimizeNeeded;
    }

    /// <summary>
    /// This packet is sent by the client to query the progress of an ongoing meta-operation
    /// </summary>
    public struct SVHDX_META_OPERATION_QUERY_PROGRESS_REQUEST
    {
        /// <summary>
        /// Indicates the transaction ID for the operation
        /// This is the transaction ID used in the RSVD_TUNNEL_META_OPERATION_START request
        /// </summary>
        [StaticSize(16)]
        public Guid TransactionId;
    }

    /// <summary>
    /// This packet is sent by the server in response to a RSVD_TUNNEL_META_OPERATION_QUERY_PROGRESS request
    /// </summary>
    public struct SVHDX_META_OPERATION_QUERY_PROGRESS_RESPONSE
    {
        /// <summary>
        /// A server-defined progress value that indicates how far along the meta-operation has proceeded
        /// </summary>
        [StaticSize(8)]
        public ulong CurrentProgressValue;

        /// <summary>
        /// The maximum progress value for the completed operation
        /// That is, when CurrentProgressValue is equal to CompleteValue, the operation is complete
        /// </summary>
        [StaticSize(8)]
        public ulong CompleteValue;
    }

    /// <summary>
    /// This packet is sent by a client to delete a snapshot from a shared virtual disk file
    /// </summary>
    public struct SVHDX_TUNNEL_DELETE_SNAPSHOT_REQUEST
    {
        /// <summary>
        /// The snapshot ID relevant to the particular request
        /// </summary>
        [StaticSize(16)]
        public Guid SnapshotId;

        /// <summary>
        /// A flag to indicate if the snapshot needs to be persisted
        /// One represents TRUE (0x00000001), indicating that the snapshot will be stored as a reference without any data
        /// Zero represents FALSE (0x00000000), indicating that the snapshot will be deleted
        /// This field MUST be set to FALSE if SnapshotType is SvhdxSnapshotTypeVM
        /// </summary>
        [StaticSize(4)]
        public PersistReference_Flags PersistReference;

        /// <summary>
        /// The type of snapshot
        /// </summary>
        [StaticSize(4)]
        public Snapshot_Type SnapshotType;
    }

    /// <summary>
    /// The SVHDX_CHANGE_TRACKING_START_REQUEST packet is sent by the client to start change tracking on the server. 
    /// </summary>
    public struct SVHDX_CHANGE_TRACKING_START_REQUEST
    {
        /// <summary>
        /// The client MUST set this to a globally unique transaction ID for each operation.
        /// </summary>
        [StaticSize(16)]
        public Guid TransactionId;

        /// <summary>
        /// The offset, in bytes, of the LogFileName field.  
        /// starts. 
        /// </summary>
        [StaticSize(4)]
        public uint LogFileNameOffset;

        /// <summary>
        /// The length, in bytes, of the name string. This length MUST include a NULL terminating character.
        /// </summary>
        [StaticSize(4)]
        public uint LogFileNameLength;

        /// <summary>
        /// A globally unique Id used to refer to this log file.
        /// </summary>
        [StaticSize(16)]
        public Guid LogFileId;

        /// <summary>
        /// The maximum allowed size, in bytes, of the underlying object store for this particular change-tracking log file.
        /// </summary>
        [StaticSize(8)]
        public ulong MaxLogFileSize;

        /// <summary>
        /// When set to TRUE, change tracking will be resumed, and further tracked data will be appended to the existing log file. 
        /// The client sets this to FALSE for the first change-tracking request issued against the target device.  
        /// </summary>
        [StaticSize(4)]
        public uint AppendData;

        /// <summary>
        /// Padding
        /// </summary>
        [StaticSize(4)]
        public uint Padding;

        /// <summary>
        /// The name of the log file containing a null-terminated Unicode UTF-16 string.
        /// </summary>
        [Size("LogFileNameLength")]
        public string LogFileName;
    }

    /// <summary>
    /// The SVHDX_CHANGE_TRACKING_GET_PARAMETERS_RESPONSE packet is sent by the server in response to a RSVD_TUNNEL_CHANGE_TRACKING_GET_PARAMETERS operation.
    /// </summary>
    public struct SVHDX_CHANGE_TRACKING_GET_PARAMETERS_RESPONSE
    {
        /// <summary>
        /// The current status of change tracking on the server. 
        /// </summary>
        [StaticSize(8)]
        public ulong ChangeTrackingStatus;

        /// <summary>
        /// The number of bytes consumed by the log file used to conduct change tracking. 
        /// </summary>
        [StaticSize(8)]
        public ulong LogFileSize;
    }

    /// <summary>
    /// The SVHDX_TUNNEL_QUERY_VIRTUAL_DISK_CHANGES_REQUEST structure requests a list of changed ranges since the designated snapshot.
    /// </summary>
    public struct SVHDX_TUNNEL_QUERY_VIRTUAL_DISK_CHANGES_REQUEST
    {
        /// <summary>
        /// A GUID that identifies the snapshot to query for the changed ranges.
        /// </summary>
        [StaticSize(16)]
        public Guid TargetSnapshotId;

        /// <summary>
        /// A GUID that identifies the snapshot to use as a baseline for getting the list of changed ranges.
        /// </summary>
        [StaticSize(16)]
        public Guid LimitSnapshotId;

        /// <summary>
        /// The type of snapshot. 
        /// </summary>
        [StaticSize(4)]
        public Snapshot_Type SnapshotType;

        /// <summary>
        /// Padding
        /// </summary>
        [StaticSize(4)]
        public uint Padding;

        /// <summary>
        /// The byte offset of the region in the virtual disk to query changes for.
        /// </summary>
        [StaticSize(8)]
        public ulong ByteOffset;

        /// <summary>
        /// The length, in bytes, of the region in the virtual disk to query changes for.
        /// </summary>
        [StaticSize(8)]
        public ulong ByteLength;
    }

    /// <summary>
    /// The SVHDX_TUNNEL_QUERY_VIRTUAL_DISK_CHANGES_REPLY structure is sent as a response to the virtual disk changes query.
    /// </summary>
    public struct SVHDX_TUNNEL_QUERY_VIRTUAL_DISK_CHANGES_REPLY
    {
        /// <summary>
        /// The total byte length that was processed. 
        /// This can be ByteLength from the SVHDX_TUNNEL_QUERY_VIRTUAL_DISK_CHANGES_REQUEST or a smaller value, 
        /// if the provided buffer was not large enough to contain all the information that was available about the requested virtual disk region.
        /// </summary>
        [StaticSize(8)]
        public ulong ProcessedByteLength;

        /// <summary>
        /// The number of changed ranges in the specified virtual disk region.
        /// </summary>
        [StaticSize(4)]
        public uint RangeCount;

        /// <summary>
        /// Reserved. The server MUST set this field to zero and the client MUST ignore it on receipt.
        /// </summary>
        [StaticSize(4)]
        public uint Reserved;

        /// <summary>
        /// The list of SVHDX_VIRTUAL_DISK_CHANGED_RANGE structures, representing the changed ranges in the specified virtual disk region.
        /// </summary>
        [Size("RangeCount")]
        public SVHDX_VIRTUAL_DISK_CHANGED_RANGE[] Ranges;
    }

    /// <summary>
    /// The SVHDX_VIRTUAL_DISK_CHANGED_RANGE structure contains the details of the changed range.
    /// </summary>
    public struct SVHDX_VIRTUAL_DISK_CHANGED_RANGE
    {
        /// <summary>
        /// The byte offset of the changed range in the virtual disk.
        /// </summary>
        [StaticSize(8)]
        public ulong ByteOffset;

        /// <summary>
        /// The length, in bytes, of the changed range in the virtual disk.
        /// </summary>
        [StaticSize(8)]
        public ulong ByteLength;

        /// <summary>
        /// Reserved. The server MUST set this field to zero and the client MUST ignore it on receipt.
        /// </summary>
        [StaticSize(8)]
        public ulong Reserved;
    }

    /// <summary>
    /// The SVHDX_APPLY_SNAPSHOT_PARAMS structure is used to specify the snapshot to apply.
    /// </summary>
    public struct SVHDX_APPLY_SNAPSHOT_PARAMS
    {
        /// <summary>
        /// The type of snapshot. 
        /// </summary>
        [StaticSize(4)]
        public Snapshot_Type SnapshotType;

        /// <summary>
        /// A GUID that identifies the snapshot to open.
        /// </summary>
        [StaticSize(16)]
        public Guid SnapshotID;
    }

    /// <summary>
    /// RSVD_BLOCK_DEVICE_TARGET_SPECIFIER structure is used to read from a particular snapshot.
    /// </summary>
    public struct RSVD_BLOCK_DEVICE_TARGET_SPECIFIER
    {
        /// <summary>
        /// This field is used to indicate the target namespace. 
        /// </summary>
        [StaticSize(4)]
        public TargetNamespaceType RsvdBlockDeviceTargetNamespace;

        /// <summary>
        /// An RSVD_BLOCK_DEVICE_TARGET_SPECIFIER_SNAPSHOT structure defined in section 2.2.4.35
        /// </summary>
        [StaticSize(20)]
        public RSVD_BLOCK_DEVICE_TARGET_SPECIFIER_SNAPSHOT TargetInformationSnapshot;
    }

    /// <summary>
    /// The RSVD_BLOCK_DEVICE_TARGET_SPECIFIER_SNAPSHOT structure is used to specify the snapshot to open when opening the virtual disk as a block device.
    /// </summary>
    public struct RSVD_BLOCK_DEVICE_TARGET_SPECIFIER_SNAPSHOT
    {
        /// <summary>
        /// The type of snapshot. 
        /// </summary>
        [StaticSize(4)]
        public Snapshot_Type SnapshotType;

        /// <summary>
        /// A GUID that identifies the snapshot to open.
        /// </summary>
        [StaticSize(16)]
        public Guid SnapshotID;
    }

    #region SCSI Primary Commands used in SVHDX_TUNNEL_SCSI_REQUEST Structure

    /// <summary>
    /// OPERATION CODE is the first byte of a SCSI CDB. It is used to identify the operation being requested by the CDB.
    /// </summary>
    public enum OPERATION_CODE: byte
    {
        /// <summary>
        /// The READ CAPACITY (10) command requests that the device server transfer 8 bytes of
        /// parameter data describing the capacity and medium format of the direct-access block device to the data-in buffer.
        /// </summary>
        READ_CAPACITY = 0x25, 

        /// <summary>
        /// The PERSISTENT RESERVE IN command is used to obtain information about persistent reservations
        /// and reservation keys (i.e., registrations) that are active within a device server.
        /// </summary>
        PERSISTENT_RESERVE_IN = 0x5E,

        /// The PERSISTENT RESERVE OUT command is used to request service actions that reserve a
        /// logical unit for the exclusive or shared use of a particular I_T nexus. 
        PERSISTENT_RESERVE_OUT = 0x5F,
    }

    /// <summary>
    /// Service Action identifies a function to be performed under the more general command function specified in the OPERATION CODE field.
    /// The followings are service actions of Persistent Reserve Out command
    /// </summary>
    public enum PERSISTENT_RESERVE_OUT_SERVICE_ACTION: byte
    {
        /// <summary>
        /// Registers a reservation key with the device server or unregisters a reservation key.
        /// </summary>
        REGISTER = 0x00,

        /// <summary>
        /// Creates a persistent reservation having a specified SCOPE and TYPE
        /// </summary>
        RESERVE = 0x01,

        /// <summary>
        /// Releases the selected persistent reservation
        /// </summary>
        RELEASE = 0x02,

        /// <summary>
        /// Clears all reservation keys (i.e., registrations) and all persistent reservations
        /// </summary>
        CLEAR = 0x03,

        /// <summary>
        /// Preempts persistent reservations and/or removes registrations
        /// </summary>
        PREEMPT = 0x04,

        /// <summary>
        /// Preempts persistent reservations and/or removes registrations and aborts all tasks for all preempted I_T nexuses
        /// </summary>
        PREEMPT_AND_ABORT = 0x05,

        /// <summary>
        /// Registers a reservation key with the device server or unregisters a reservation key
        /// </summary>
        REGISTER_AND_IGNORE_EXISTING_KEY = 0x06,

        /// <summary>
        /// Registers a reservation key for another I_T nexus with the device server and moves a persistent reservation to that I_T nexus
        /// </summary>
        REGISTER_AND_MOVE = 0x07,
    }

    /// <summary>
    /// Scope must be set to LU_SCOPE (0)
    /// So this enum is based on "Type" code.
    /// </summary>
    public enum PERSISTENT_RESERVATION_SCOPE_AND_TYPE_CODE: byte
    {
        Obsolete = 0x00,

        /// <summary>
        /// Access Restrictions: Some commands (e.g., media-access write commands) are only allowed for the persistent reservation holder.
        /// Persistent Reservation Holder: There is only one persistent reservation holder.
        /// </summary>
        WriteExclusive = 0x01,

        /// <summary>
        /// Access Restrictions: Some commands (e.g., media-access commands) are only allowed for the persistent reservation holder.
        /// Persistent Reservation Holder: There is only one persistent reservation holder.
        /// </summary>
        ExclusiveAccess = 0x03,

        /// <summary>
        /// Access Restrictions: Some commands (e.g., media-access write commands) are only allowed for registered I_T nexuses.
        /// Persistent Reservation Holder: There is only one persistent reservation holder.
        /// </summary>
        WriteExclusiveRegistrantsOnly = 0x05,

        /// <summary>
        /// Access Restrictions: Some commands (e.g., media-access commands) are only allowed for registered I_T nexuses.
        /// Persistent Reservation Holder: There is only one persistent reservation holder.
        /// </summary>
        ExclusiveAccessRegistrantsOnly = 0x06,

        /// <summary>
        /// Access Restrictions: Some commands (e.g., media-access write commands) are only allowed for registered I_T nexuses.
        /// Persistent Reservation Holder: Each registered I_T nexus is a persistent reservation holder.
        /// </summary>
        WriteExclusiveAllRegistrants = 0x07,

        /// <summary>
        /// Access Restrictions: Some commands (e.g., media-access commands) are only allowed for registered I_T nexuses.
        /// Persistent Reservation Holder: Each registered I_T nexus is a persistent reservation holder.
        /// </summary>
        ExclusiveAccessAllRegistrants = 0x08
    }

    /// <summary>
    /// Structure definition of PERSISTENT RESERVE OUT command.
    /// The PERSISTENT RESERVE OUT command is used to request service actions that reserve a logical unit for the exclusive or shared use of a particular I_T nexus. 
    /// The command uses other service actions to manage and remove such persistent reservations.
    /// </summary>
    public struct PERSISTENT_RESERVE_OUT
    {
        /// <summary>
        /// OPERATION CODE of PERSISTENT RESERVE OUT command is 0x5F
        /// </summary>
        [StaticSize(1)]
        public OPERATION_CODE OperationCode;

        /// <summary>
        /// It's used to reserve, manage and remove persistent reservations
        /// </summary>
        [StaticSize(1)]
        public PERSISTENT_RESERVE_OUT_SERVICE_ACTION ServiceAction;

        /// <summary>
        /// The SCOPE field (4 bits) shall be set to LU_SCOPE (0), specifying that the persistent reservation applies to the entire logical unit.
        /// The TYPE field (4 bits) specifies the characteristics of the persistent reservation being established for all logical blocks within the logical unit.
        /// </summary>
        [StaticSize(1)]
        public PERSISTENT_RESERVATION_SCOPE_AND_TYPE_CODE ScopeAndType;

        [StaticSize(2)]
        [ByteOrder(EndianType.BigEndian)]
        public ushort Reserved;

        /// <summary>
        /// It specifies the number of bytes of parameter data for the PERSISTENT RESERVE OUT command.
        /// </summary>
        [StaticSize(4)]
        [ByteOrder(EndianType.BigEndian)]
        public uint ParameterListLength;

        /// <summary>
        /// The CONTROL byte has the same definition for all commands.
        /// </summary>
        [StaticSize(1)]
        public byte Control;
    }

    /// <summary>
    /// The parameter list format used by the PERSISTENT RESERVE OUT command with any service action except the REGISTER AND MOVE service action.
    /// </summary>
    public struct PERSISTENT_RESERVE_OUT_PARAMETER_LIST
    {
        /// <summary>
        /// It is provided by the application client to the device server to
        /// identify the I_T nexus that is the source of the PERSISTENT RESERVE OUT command.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public ulong ReservationKey;

        /// <summary>
        /// It contains information needed for the following service actions:
        /// REGISTER, REGISTER AND IGNORE EXISTING KEY, PREEMPT, and PREEMPT AND ABORT.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]
        public ulong ServiceActionReservationKey;

        /// <summary>
        /// Serveral other fields are ignored in the test suite.
        /// </summary>
        [ByteOrder(EndianType.BigEndian)]    
        public ulong Reserved;
    }
#endregion
}

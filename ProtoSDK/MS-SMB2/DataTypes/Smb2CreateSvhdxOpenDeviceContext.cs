// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SVHDX_OPEN_DEVICE_CONTEXT packet is sent by the client to open the shared virtual disk.
    /// </summary>
    public class Smb2CreateSvhdxOpenDeviceContext : Smb2CreateContextRequest
    {
        /// <summary>
        /// The version of the create context. It MUST be set to the highest supported version of the protocol.
        /// </summary>
        public uint Version;

        /// <summary>
        /// A Boolean value, where zero represents FALSE and nonzero represents TRUE. 
        /// </summary>
        public bool HasInitiatorId;

        /// <summary>
        /// A GUID that optionally identifies the initiator of the open request.
        /// </summary>
        public Guid InitiatorId;

        /// <summary>
        /// Reserved. The client SHOULD set this field to 0x00000000, and the server MUST ignore it on receipt.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// This field is used to indicate which component has originated or issued the operation. 
        /// </summary>
        public uint OriginatorFlags;

        /// <summary>
        /// A 64-bit value assigned by the client for an outgoing request. The server MUST ignore it on receipt.
        /// </summary>
        public ulong OpenRequestId;

        /// <summary>
        /// The length, in bytes, of the InitiatorHostName, including the null termination. 
        /// </summary>
        public ushort InitiatorHostNameLength;

        /// <summary>
        /// A 126-byte buffer containing a null-terminated Unicode UTF-16 string that specifies the computer name on which the initiator resides.
        /// </summary>
        public string InitiatorHostName;
    }

    /// <summary>
    /// The SVHDX_OPEN_DEVICE_CONTEXT_V2 packet is sent by the client to open the shared virtual disk.
    /// This is valid only for the RSVD Protocol version 2
    /// </summary>
    public class Smb2CreateSvhdxOpenDeviceContextV2 : Smb2CreateContextRequest
    {
        /// <summary>
        /// The version of the create context. It MUST be set to 0x00000002.
        /// </summary>
        public uint Version;

        /// <summary>
        /// A Boolean value, where zero represents FALSE and nonzero represents TRUE. 
        /// </summary>
        public bool HasInitiatorId;

        /// <summary>
        /// A GUID that optionally identifies the initiator of the open request.
        /// </summary>
        public Guid InitiatorId;

        /// <summary>
        /// Reserved. The client SHOULD set this field to 0x00000000, and the server MUST ignore it on receipt.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// This field is used to indicate which component has originated or issued the operation. 
        /// </summary>
        public uint OriginatorFlags;

        /// <summary>
        /// A 64-bit value assigned by the client for an outgoing request. The server MUST ignore it on receipt.
        /// </summary>
        public ulong OpenRequestId;

        /// <summary>
        /// The length, in bytes, of the InitiatorHostName, including the null termination. 
        /// </summary>
        public ushort InitiatorHostNameLength;

        /// <summary>
        /// A 126-byte buffer containing a null-terminated Unicode UTF-16 string that specifies the computer name on which the initiator resides.
        /// </summary>
        public string InitiatorHostName;

        /// <summary>
        /// This field MUST be set to zero
        /// </summary>
        public uint VirtualDiskPropertiesInitialized;

        /// <summary>
        /// This field MUST be set to zero
        /// </summary>
        public uint ServerServiceVersion;

        /// <summary>
        /// This field MUST be set to zero
        /// </summary>
        public uint VirtualSectorSize;

        /// <summary>
        /// This field MUST be set to zero
        /// </summary>
        public uint PhysicalSectorSize;

        /// <summary>
        /// This field MUST be set to zero
        /// </summary>
        public ulong VirtualSize;
    }
}

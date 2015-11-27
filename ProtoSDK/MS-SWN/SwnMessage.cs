// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Swn
{
    /// <summary>
    /// MS-SWN 2.2.2.1 IPADDR_INFO
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct IPADDR_INFO
    {
        /// <summary>
        /// Indicate the IP address type of the interface
        /// UINT->unsigned int
        /// </summary>
        public uint Flags;

        /// <summary>
        /// IP V4 address of the interface
        /// ULONG->unsigned int
        /// </summary>
        public uint IPV4;

        /// <summary>
        /// IP V4 address of the interface
        /// USHORT[8]
        /// </summary>
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.U2)]
        public ushort[] IPV6;
    }

    /// <summary>
    /// MS-SWN 2.2.2.2 IPADDR_INFO_LIST
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct IPADDR_INFO_LIST
    {
        /// <summary>
        /// The size of the IPADDR_INFO_LIST structure, in bytes.
        /// UINT->unsigned int
        /// </summary>
        public uint Length;

        /// <summary>
        /// This field MUST NOT be used and MUST be reserved. The client MUST set this to 0, and the server MUST ignore it on receipt.
        /// ULONG->unsigned int
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// The number of IPADDR_INFO structures in the IPAddrList member.
        /// ULONG->unsigned int
        /// </summary>
        public uint IPAddrInstances;

        /// <summary>
        /// Indicates the IP addresses, as specified in section 2.2.2.1, of the destination Interface group
        /// IPADDR_INFO[]
        /// </summary>
        [MarshalAsAttribute(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct)]
        public IPADDR_INFO[] IPAddrList;
    }

    /// <summary>
    /// MS-SWN 2.2.2.3 RESOURCE_CHANGE
    /// </summary>
    public struct RESOURCE_CHANGE
    {
        /// <summary>
        /// Length: The size of the resource change notification, in bytes.
        /// UINT->unsigned int
        /// </summary>
        public uint Length;

        /// <summary>
        /// Specifies state change of the resource.
        /// UINT32->unsigned int
        /// </summary>
        public uint ChangeType;

        /// <summary>
        /// The resource name on which the change has been detected.
        /// WCHAR[]
        /// </summary>
        public string ResourceName;
    }

    /// <summary>
    /// MS-SWN 2.2.2.4 RESP_ASYNC_NOTIFY
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct RESP_ASYNC_NOTIFY
    {
        /// <summary>
        /// Specifies the notification type.
        /// </summary>
        public uint MessageType;

        /// <summary>
        /// Specifies the size of the RESP_ASYNC_NOTIFY structure, in bytes.
        /// </summary>
        public uint Length;

        /// <summary>
        /// Total number of notifications in the MessageBuffer field.
        /// </summary>
        public uint NumberOfMessages;

        /// <summary>
        /// Contains an array of notification information structures whose type is determined by the MessageType field.
        /// byte*
        /// </summary>
        [Messages.Marshaling.Size("Length")]
        public byte[] MessageBuffer;
    }

    /// <summary>
    /// MS-SWN 2.2.2.5 WITNESS_INTERFACE_INFO
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WITNESS_INTERFACE_INFO
    {
        /// <summary>
        /// The group name of the interface set.
        /// WCHAR[260]
        /// </summary>
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string InterfaceGroupName;

        /// <summary>
        /// The version of the interface.
        /// USHORT->unsigned short
        /// </summary>
        public uint Version;

        /// <summary>
        /// The current state of the interface.
        /// USHORT->unsigned short
        /// </summary>
        public SwnNodeState NodeState;

        /// <summary>
        /// The IPv4 addresses of the interface.
        /// ULONG->unsigned int
        /// </summary>
        public uint IPV4;

        /// <summary>
        /// The IPv6 addresses of the interface.
        /// USHORT[8]
        /// </summary>
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.U2)]
        public ushort[] IPV6;

        /// <summary>
        /// The Flags field specifies information about the interface.
        /// BOOLEAN->BYTE->unsigned char
        /// </summary>
        public uint Flags;
    }

    /// <summary>
    /// WITNESS_INTERFACE_LIST for RPC marshal
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct WITNESS_INTERFACE_LIST_RPC
    {
        /// <summary>
        /// The size of the InterfaceInfo member, in bytes.
        /// UINT->unsigned int
        /// </summary>
        public uint NumberOfInterfaces;

        /// <summary>
        /// Contains an array of WITNESS_INTERFACE_INFO structures, as specified in section 2.2.2.5.
        /// byte*
        /// </summary>
        public readonly System.IntPtr InterfaceInfo;
    }

    /// <summary>
    /// MS-SWN 2.2.2.6 WITNESS_INTERFACE_LIST
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct WITNESS_INTERFACE_LIST
    {
        /// <summary>
        /// The size of the InterfaceInfo member, in bytes.
        /// </summary>
        public uint NumberOfInterfaces;

        /// <summary>
        /// Contains an array of WITNESS_INTERFACE_INFO structures, as specified in section 2.2.2.5.
        /// </summary>
        [Messages.Marshaling.Size("NumberOfInterfaces")]
        public WITNESS_INTERFACE_INFO[] InterfaceInfo;
    }

    /// <summary>
    /// MessageType value in RESP_ASYNC_NOTIFY structure
    /// </summary>
    public enum SwnMessageType : uint
    {
        /// <summary>
        /// Resource Change notification
        /// </summary>
        RESOURCE_CHANGE_NOTIFICATION = 0x00000001,

        /// <summary>
        /// Client Move notification
        /// </summary>
        CLIENT_MOVE_NOTIFICATION = 0x00000002,

        /// <summary>
        /// Share Move notification
        /// </summary>
        SHARE_MOVE_NOTIFICATION = 0x00000003,

        /// <summary>
        /// IP Change notification
        /// </summary>
        IP_CHANGE_NOTIFICATION = 0x00000004,

    }

    /// <summary>
    /// ChangeType value in RESOURCE_CHANGE structure
    /// </summary>
    public enum SwnResourceChangeType : uint
    {
        /// <summary>
        /// Resource state unknown
        /// </summary>
        RESOURCE_STATE_UNKNOWN = 0x00000000,

        /// <summary>
        /// Resource state available
        /// </summary>
        RESOURCE_STATE_AVAILABLE = 0x00000001,

        /// <summary>
        /// Resource state unavailable
        /// </summary>
        RESOURCE_STATE_UNAVAILABLE = 0x000000FF,
    }

    /// <summary>
    /// Flags value in IPADDR_INFO structure
    /// </summary>
    public enum SwnIPAddrInfoFlags : uint
    {
        /// <summary>
        /// If set, the IPV4 field contains a valid address.
        /// </summary>
        IPADDR_V4 = 0x00000001,

        /// <summary>
        /// If set, the IPV6 field contains a valid address.
        /// </summary>
        IPADDR_V6 = 0x00000002,

        /// <summary>
        /// If set, the IPV4 or IPV6 address is available. This flag is applicable only for the servers implementing version 2.
        /// </summary>
        IPADDR_ONLINE = 0x00000008,

        /// <summary>
        /// If set, the IPV4 or IPV6 address is not available. This flag is applicable only for the server implementing version 2.
        /// </summary>
        IPADDR_OFFLINE = 0x00000010,
    }

    /// <summary>
    /// NodeState value in WITNESS_INTERFACE_INFO structure
    /// </summary>
    public enum SwnNodeState : uint
    {
        /// <summary>
        /// The state of the interface is unknown.
        /// </summary>
        UNKNOWN = 0X0000,

        /// <summary>
        /// The interface is available.
        /// </summary>
        AVAILABLE = 0X0001,

        /// <summary>
        /// The interface is unavailable.
        /// </summary>
        UNAVAILABLE = 0X00FF,
    }

    /// <summary>
    /// NodeIsCurrentNode flag value in WITNESS_INTERFACE_INFO structure
    /// </summary>
    public enum SwnNodeFlagsValue : uint
    {
        /// <summary>
        /// If set, the IPV4 field contains a valid address.
        /// </summary>
        IPv4 = 0x00000001,

        /// <summary>
        /// If set, the IPv6 field contains a valid address.
        /// </summary>
        IPv6 = 0x00000002,

        /// <summary>
        /// If set, the interface can be used for witness registration. If not set, the interface MUST NOT be used for witness registration.
        /// </summary>
        INTERFACE_WITNESS = 0x00000004,
    }

    /// <summary>
    /// Flags field value in WitnessrRegisterEx message
    /// </summary>
    public enum WitnessrRegisterExFlagsValue : uint
    {
        /// <summary>
        /// If set, the client requests notifications only for the registered IP address.
        /// </summary>
        WITNESS_REGISTER_NONE = 0x00000000,

        /// <summary>
        /// If set, the client requests notifications of any eligible server IP addresses.
        /// </summary>
        WITNESS_REGISTER_IP_NOTIFICATION = 0x00000001,
    }

    /// <summary>
    /// The protocol supports versioning negotiation, which is provided for possible future revision. 
    /// The current protocol definition is only a single version.
    /// </summary>
    public enum SwnVersion : uint
    {
        /// <summary>
        /// Witness protocol version 1
        /// </summary>
        SWN_VERSION_1 = 0x00010001,

        /// <summary>
        /// Witness protocol version 2
        /// </summary>
        SWN_VERSION_2 = 0x00020000,

        /// <summary>
        /// Unknown version
        /// </summary>
        SWN_VERSION_UNKNOWN = 0xFFFFFFFF,
    }

    /// <summary>
    /// Opnum for SWN interface
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum SWN_OPNUM : ushort
    {
        /// <summary>
        /// The opnum for WitnessrGetInterfaceList method
        /// </summary>
        WitnessrGetInterfaceList = 0,

        /// <summary>
        /// The opnum for WitnessrRegister method
        /// </summary>
        WitnessrRegister = 1,

        /// <summary>
        /// The opnum for WitnessrUnRegister method
        /// </summary>
        WitnessrUnRegister = 2,

        /// <summary>
        /// The opnum for WitnessrAsyncNotify method
        /// </summary>
        WitnessrAsyncNotify = 3,

        /// <summary>
        /// The opnum for WitnessrRegisterEx method
        /// </summary>
        WitnessrRegisterEx = 4,
    }

    /// <summary>
    /// Error codes
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum SwnErrorCode : uint
    {
        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        ERROR_SUCCESS = 0x00000000,

        /// <summary>
        /// Access is denied.
        /// </summary>
        ERROR_ACCESS_DENIED = 0x00000005,

        /// <summary>
        /// The required memory is not available.
        /// </summary>
        RPC_S_OUT_OF_MEMORY = 0x0000000E,

        /// <summary>
        /// The parameter is incorrect.
        /// </summary>
        ERROR_INVALID_PARAMETER = 0x00000057,

        /// <summary>
        /// Cannot create a file when that file already exists.
        /// </summary>
        ERROR_ALREADY_EXISTS = 0x000000B7,

        /// <summary>
        /// No more data is available.
        /// </summary>
        ERROR_NO_MORE_ITEMS = 0x00000103,

        /// <summary>
        /// The specified CONTEXT_HANDLE is not found.
        /// </summary>
        ERROR_NOT_FOUND = 0x00000490,

        /// <summary>
        /// The client request contains an invalid Witness protocol version.
        /// </summary>
        ERROR_REVISION_MISMATCH = 0x0000051A,

        /// <summary>
        /// The specified time-out.
        /// </summary>
        ERROR_TIMEOUT = 0x000005B4,

        /// <summary>
        /// Insufficient system resources exist to complete the requested service.
        /// </summary>
        ERROR_NO_SYSTEM_RESOURCES = 0x000005AA,

        /// <summary>
        /// The specified resource state is invalid.
        /// </summary>
        ERROR_INVALID_STATE = 0x0000139F,
    }
}

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpefs
{
    public enum DR_Create_RSP_InformationValue : byte
    {
        FILE_SUPERSEDED = 0x00000000,
        FILE_OPENED = 0x00000001,
        FILE_OVERWRITTEN = 0x00000003
    }

    /// <summary>
    /// Component_Values
    /// </summary>
    [Flags()]
    public enum Component_Values : ushort
    {
        /// <summary>
        /// Invalid rdpdr type
        /// </summary>
        RDPDR_CTYPE_INVALID = 0xFFFF,

        /// <summary>
        ///  Device redirector core component; most of the packets
        ///  in this protocol are sent under this component ID.
        /// </summary>
        RDPDR_CTYP_CORE = 0x4472,

        /// <summary>
        ///  Printing component. The packets that use this ID are
        ///  typically about printer cache management and identifying
        ///  XPS printers.
        /// </summary>
        RDPDR_CTYP_PRN = 0x5052,
    }

    /// <summary>
    /// PacketId_Values
    /// </summary>
    [Flags()]
    public enum PacketId_Values : ushort
    {
        /// <summary>
        /// Invalid packet id
        /// </summary>
        PAKID_INVALID = 0xFFFF,

        /// <summary>
        ///  Server Announce Request.
        /// </summary>
        PAKID_CORE_SERVER_ANNOUNCE = 0x496e,

        /// <summary>
        ///  Client Announce Reply and Server Client ID Confirm.
        /// </summary>
        PAKID_CORE_CLIENTID_CONFIRM = 0x4343,

        /// <summary>
        ///  Client Name Request.
        /// </summary>
        PAKID_CORE_CLIENT_NAME = 0x434e,

        /// <summary>
        ///  Client Device List Announce Request.
        /// </summary>
        PAKID_CORE_DEVICELIST_ANNOUNCE = 0x4441,

        /// <summary>
        ///  Server Device Announce Response.
        /// </summary>
        PAKID_CORE_DEVICE_REPLY = 0x6472,

        /// <summary>
        ///  Device I/O Request.
        /// </summary>
        PAKID_CORE_DEVICE_IOREQUEST = 0x4952,

        /// <summary>
        ///  Device I/O Response.
        /// </summary>
        PAKID_CORE_DEVICE_IOCOMPLETION = 0x4943,

        /// <summary>
        ///  Server Core Capability Request.
        /// </summary>
        PAKID_CORE_SERVER_CAPABILITY = 0x5350,

        /// <summary>
        ///  Client Core Capability Response.
        /// </summary>
        PAKID_CORE_CLIENT_CAPABILITY = 0x4350,

        /// <summary>
        ///  Client Drive Device List Remove.
        /// </summary>
        PAKID_CORE_DEVICELIST_REMOVE = 0x444d,

        /// <summary>
        ///  Add Printer Cachedata, as specified in [MS-RDPEPC] section
        ///  2.2.2.3.
        /// </summary>
        PAKID_PRN_CACHE_DATA = 0x5043,

        /// <summary>
        ///  Server User Logged On.
        /// </summary>
        PAKID_CORE_USER_LOGGEDON = 0x554c,

        /// <summary>
        ///  Server Printer Set XPS Mode, as specified in [MS-RDPEPC].
        /// </summary>
        PAKID_PRN_USING_XPS = 0x5543,
    }

    /// <summary>
    /// CAPABILITY_VERSION
    /// </summary>
    public enum CAPABILITY_VERSION : uint
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// GENERAL_CAPABILITY_VERSION_01
        /// </summary>
        V1 = 0x00000001,

        /// <summary>
        /// GENERAL_CAPABILITY_VERSION_02
        /// </summary>
        V2 = 0x00000002,
    }

    /// <summary>
    /// CapabilityType_Values
    /// </summary>
    [Flags()]
    public enum CapabilityType_Values : ushort
    {
        /// <summary>
        ///  General capability set (GENERAL_CAPS_SET)
        /// </summary>
        CAP_GENERAL_TYPE = 0x0001,

        /// <summary>
        ///  Print capability set (PRINTER_CAPS_SET)
        /// </summary>
        CAP_PRINTER_TYPE = 0x0002,

        /// <summary>
        ///  Port capability set (PORT_CAPS_SET)
        /// </summary>
        CAP_PORT_TYPE = 0x0003,

        /// <summary>
        ///  Drive capability set (DRIVE_CAPS_SET)
        /// </summary>
        CAP_DRIVE_TYPE = 0x0004,

        /// <summary>
        ///  Smart card capability set (SMARTCARD_CAPS_SET)
        /// </summary>
        CAP_SMARTCARD_TYPE = 0x0005,

        /// <summary>
        ///  Invalid capability set
        /// </summary>
        CAP_INVALID = 0xFFFF,
    }

    /// <summary>
    /// DeviceType_Values
    /// </summary>
    [Flags()]
    public enum DeviceType_Values : uint
    {
        /// <summary>
        ///  Serial port device
        /// </summary>
        RDPDR_DTYP_SERIAL = 0x00000001,

        /// <summary>
        ///  Parallel port device
        /// </summary>
        RDPDR_DTYP_PARALLEL = 0x00000002,

        /// <summary>
        ///  Printer device
        /// </summary>
        RDPDR_DTYP_PRINT = 0x00000004,

        /// <summary>
        ///  File system device
        /// </summary>
        RDPDR_DTYP_FILESYSTEM = 0x00000008,

        /// <summary>
        ///  Smart card device
        /// </summary>
        RDPDR_DTYP_SMARTCARD = 0x00000020,

        /// <summary>
        ///  Invalid device
        /// </summary>
        RDPDR_DTYP_INVALID = 0xFFFFFFFF,
    }

    /// <summary>
    /// MajorFunction_Values
    /// </summary>
    [Flags()]
    public enum MajorFunction_Values : uint
    {
        /// <summary>
        ///  Create request
        /// </summary>
        IRP_MJ_CREATE = 0x00000000,

        /// <summary>
        ///  Close request
        /// </summary>
        IRP_MJ_CLOSE = 0x00000002,

        /// <summary>
        ///  Read request
        /// </summary>
        IRP_MJ_READ = 0x00000003,

        /// <summary>
        ///  Write request
        /// </summary>
        IRP_MJ_WRITE = 0x00000004,

        /// <summary>
        ///  Device control request
        /// </summary>
        IRP_MJ_DEVICE_CONTROL = 0x0000000e,

        /// <summary>
        ///  Query volume information request
        /// </summary>
        IRP_MJ_QUERY_VOLUME_INFORMATION = 0x0000000a,

        /// <summary>
        ///  Set volume information request
        /// </summary>
        IRP_MJ_SET_VOLUME_INFORMATION = 0x0000000b,

        /// <summary>
        ///  Query information request
        /// </summary>
        IRP_MJ_QUERY_INFORMATION = 0x00000005,

        /// <summary>
        ///  Set information request
        /// </summary>
        IRP_MJ_SET_INFORMATION = 0x00000006,

        /// <summary>
        ///  Directory control request
        /// </summary>
        IRP_MJ_DIRECTORY_CONTROL = 0x0000000c,

        /// <summary>
        ///  File lock control request
        /// </summary>
        IRP_MJ_LOCK_CONTROL = 0x00000011,
    }

    /// <summary>
    /// MinorFunction_Values
    /// </summary>
    [Flags()]
    public enum MinorFunction_Values : uint
    {
        /// <summary>
        /// The MinorFunction could be ZERO when the Major Function is not IRP_MJ_DIRECTOR_CONTROL.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Query directory request
        /// </summary>
        IRP_MN_QUERY_DIRECTORY = 0x00000001,

        /// <summary>
        ///  Notify change directory request
        /// </summary>
        IRP_MN_NOTIFY_CHANGE_DIRECTORY = 0x00000002,
    }

    /// <summary>
    /// VersionMajor_Values
    /// </summary>
    public enum VersionMajor_Values : ushort
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0001,
    }

    /// <summary>
    /// VersionMinor_Values
    /// </summary>
    [Flags()]
    public enum VersionMinor_Values : ushort
    {
        /// <summary>
        ///   </summary>
        V1 = 0x000c,

        /// <summary>
        ///  and
        /// </summary>
        V2 = 0x0005,

        /// <summary>
        ///   </summary>
        V3 = 0x0002,

        /// <summary>
        /// 
        /// </summary>
        V4 = 0x000A,
        /// <summary>
        /// 
        /// </summary>
        V5 = 0x0006
    }

    /// <summary>
    /// DR_CORE_SERVER_ANNOUNCE_RSP_VersionMajor_Values
    /// </summary>
    public enum DR_CORE_SERVER_ANNOUNCE_RSP_VersionMajor_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0001,
    }

    /// <summary>
    /// DR_CORE_SERVER_ANNOUNCE_RSP_VersionMinor_Values
    /// </summary>
    [Flags()]
    public enum DR_CORE_SERVER_ANNOUNCE_RSP_VersionMinor_Values : ushort
    {
        /// <summary>
        ///  RDP Client 6.0 and 6.1
        /// </summary>
        V1 = 0x000c,

        /// <summary>
        ///  RDP Client 5.1 and 6.2
        /// </summary>
        V2 = 0x0005,

        /// <summary>
        ///  RDP Client 5.0
        /// </summary>
        V3 = 0x0002,
    }

    /// <summary>
    /// UnicodeFlag_Values
    /// </summary>
    [Flags()]
    public enum UnicodeFlag_Values : uint
    {
        /// <summary>
        ///  ComputerName is in Unicode characters.
        /// </summary>
        V1 = 0x00000001,

        /// <summary>
        ///  ComputerName is in ASCII characters.
        /// </summary>
        V2 = 0x00000000,
    }

    /// <summary>
    /// DR_CORE_SERVER_CLIENTID_CONFIRM_VersionMajor_Values
    /// </summary>
    public enum DR_CORE_SERVER_CLIENTID_CONFIRM_VersionMajor_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0001,
    }

    /// <summary>
    /// DR_CORE_SERVER_CLIENTID_CONFIRM_VersionMinor_Values
    /// </summary>
    [Flags()]
    public enum DR_CORE_SERVER_CLIENTID_CONFIRM_VersionMinor_Values : ushort
    {
        /// <summary>
        ///   </summary>
        V1 = 0x000c,

        /// <summary>
        ///  and
        /// </summary>
        V2 = 0x0005,

        /// <summary>
        ///   </summary>
        V3 = 0x0002,

        /// <summary>
        /// 
        /// </summary>
        V4 = 0x000A,

        V5 = 0x000D
    }

    /// <summary>
    /// osType_Values
    /// </summary>
    [Flags()]
    public enum osType_Values : uint
    {
        /// <summary>
        ///  Unknown operating system
        /// </summary>
        OS_TYPE_UNKNOWN = 0x00000000,

        /// <summary>
        ///  
        /// </summary>
        OS_TYPE_WIN9X = 0x00000001,

        /// <summary>
        ///  
        /// </summary>
        OS_TYPE_WINNT = 0x00000002,
    }

    /// <summary>
    /// osVersion_Values
    /// </summary>
    public enum osVersion_Values : uint
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x00000000,
    }

    /// <summary>
    /// protocolMajorVersion_Values
    /// </summary>
    public enum protocolMajorVersion_Values : ushort
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0001,
    }

    /// <summary>
    /// ioCode1_Values
    /// </summary>
    [Flags()]
    public enum ioCode1_Values : uint
    {
        None = 0,
        /// <summary>
        ///  Unused, always set.
        /// </summary>
        RDPDR_IRP_MJ_CREATE = 0x00000001,

        /// <summary>
        ///  Unused, always set.
        /// </summary>
        RDPDR_IRP_MJ_CLEANUP = 0x00000002,

        /// <summary>
        ///  Unused, always set.
        /// </summary>
        RDPDR_IRP_MJ_CLOSE = 0x00000004,

        /// <summary>
        ///  Unused, always set.
        /// </summary>
        RDPDR_IRP_MJ_READ = 0x00000008,

        /// <summary>
        ///  Unused, always set.
        /// </summary>
        RDPDR_IRP_MJ_WRITE = 0x00000010,

        /// <summary>
        ///  Unused, always set.
        /// </summary>
        RDPDR_IRP_MJ_FLUSH_BUFFERS = 0x00000020,

        /// <summary>
        ///  Unused, always set.
        /// </summary>
        RDPDR_IRP_MJ_SHUTDOWN = 0x00000040,

        /// <summary>
        ///  Unused, always set.
        /// </summary>
        RDPDR_IRP_MJ_DEVICE_CONTROL = 0x00000080,

        /// <summary>
        ///  Unused, always set.
        /// </summary>
        RDPDR_IRP_MJ_QUERY_VOLUME_INFORMATION = 0x00000100,

        /// <summary>
        ///  Unused, always set.
        /// </summary>
        RDPDR_IRP_MJ_SET_VOLUME_INFORMATION = 0x00000200,

        /// <summary>
        ///  Unused, always set.
        /// </summary>
        RDPDR_IRP_MJ_QUERY_INFORMATION = 0x00000400,

        /// <summary>
        ///  Unused, always set.
        /// </summary>
        RDPDR_IRP_MJ_SET_INFORMATION = 0x00000800,

        /// <summary>
        ///  Unused, always set.
        /// </summary>
        RDPDR_IRP_MJ_DIRECTORY_CONTROL = 0x00001000,

        /// <summary>
        ///  Unused, always set.
        /// </summary>
        RDPDR_IRP_MJ_LOCK_CONTROL = 0x00002000,

        /// <summary>
        ///  Enable Query Security requests (IRP_MJ_QUERY_SECURITY).
        /// </summary>
        RDPDR_IRP_MJ_QUERY_SECURITY = 0x00004000,

        /// <summary>
        ///  Enable Set Security requests (IRP_MJ_SET_SECURITY).
        /// </summary>
        RDPDR_IRP_MJ_SET_SECURITY = 0x00008000,
    }

    /// <summary>
    /// ioCode2_Values
    /// </summary>
    public enum ioCode2_Values : uint
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// extendedPDU_Values
    /// </summary>
    [Flags()]
    public enum extendedPDU_Values : uint
    {
        /// <summary>
        ///  Allow the client to send Client Drive Device List Remove
        ///  packets.
        /// </summary>
        RDPDR_DEVICE_REMOVE_PDUS = 0x0001,

        /// <summary>
        ///  Unused.
        /// </summary>
        RDPDR_CLIENT_DISPLAY_NAME_PDU = 0x0002,

        /// <summary>
        ///  Allow the server to send a Server User Logged On packet.
        /// </summary>
        RDPDR_USER_LOGGEDON_PDU = 0x0004,
    }

    /// <summary>
    /// extraFlags1_Values
    /// </summary>
    public enum extraFlags1_Values : uint
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x00000000,

        /// <summary>
        /// Possible value.
        /// </summary>
        ENABLE_ASYNCIO = 0x00000001,
    }

    /// <summary>
    /// extraFlags2_Values
    /// </summary>
    public enum extraFlags2_Values : uint
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x00000000,
    }
}

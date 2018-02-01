// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;


namespace Microsoft.Protocols.TestSuites.Rdpbcgr.efs
{
    /// <summary>
    /// RDPEFS state
    /// </summary>
    public enum RDPEFS_STATE
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// Receive Server Announce Request
        /// </summary>
        ServerAnnounceRequest,

        /// <summary>
        /// Send Client Announce Reply
        /// </summary>
        ClientAnnounceReply,

        /// <summary>
        /// Send Client Name Request
        /// </summary>
        ClientNameRequest,

        /// <summary>
        /// Receive Server Core Capability Request
        /// </summary>
        ServerCoreCapabilityRequest,

        /// <summary>
        /// Receive Server Client Id Confirm
        /// </summary>
        ServerClientIdConfirm,

        /// <summary>
        /// Send Client Core Capability Response
        /// </summary>
        ClientCoreCapabilityResponse,

        /// <summary>
        /// Send Client Device List Announce Request
        /// </summary>
        ClientDeviceListAnnounceRequest,
    }

    /// <summary>
    /// RDPEFS packet type
    /// </summary>
    public enum RdpefsPacketType
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// Server Announce Request
        /// </summary>
        ServerAnnounceRequest,

        /// <summary>
        /// Server Core Capability Request
        /// </summary>
        ServerCoreCapabilityRequest,

        /// <summary>
        /// Server Client ID Confirm
        /// </summary>
        ServerClientIdConfirm,

        /// <summary>
        /// Server User Logged On
        /// </summary>
        ServerUserLoggedOn,

        /// <summary>
        /// Server Device Announce Response
        /// </summary>
        ServerDeviceAnnounceResponse,

        /// <summary>
        /// Server Device Create Request
        /// </summary>
        ServerDeviceCreateRequest,

        /// <summary>
        /// Server Device Query Information Request
        /// </summary>
        ServerDeviceQueryInfoRequest,

        /// <summary>
        /// Server Device Set Information Request
        /// </summary>
        ServerDeviceSetInfoRequest,

        /// <summary>
        /// Server Device Query Volume Information Request
        /// </summary>
        ServerDeviceQueryVolumeInfoRequest,

        /// <summary>
        /// Server Device Set Volume Information Request
        /// </summary>
        ServerDeviceSetVolumeInfoRequest,

        /// <summary>
        /// Server Device Control Request
        /// </summary>
        ServerDeviceControlRequest,

        /// <summary>
        /// Server Device Query Directory Request
        /// </summary>
        ServerDeviceQueryDirectoryRequest,

        /// <summary>
        /// Server Device Notify Change Directory Request
        /// </summary>
        ServerDeviceNotifyChangeDirectoryRequest,

        /// <summary>
        /// Server Device Lock Request
        /// </summary>
        ServerDeviceLockRequest,

        /// <summary>
        /// Server Device Write Request
        /// </summary>
        ServerDeviceWriteRequest,

        /// <summary>
        /// Server Device Read Request
        /// </summary>
        ServerDeviceReadRequest,

        /// <summary>
        /// Server Device Close Request
        /// </summary>
        ServerDeviceCloseRequest,

        /// <summary>
        /// Server Disconnection
        /// </summary>
        ServerDisconnection,
    }

    /// <summary>
    /// Indicate it is a file or directory
    /// </summary>
    public enum FileType : uint
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// File
        /// </summary>
        File,

        /// <summary>
        /// Directory
        /// </summary>
        Directory,
    }

    /// <summary>
    /// FSCC type
    /// </summary>
    public enum FSCCType
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// FileFsVolumeInformation
        /// </summary>
        FileFsVolumeInformation = 1,

        /// <summary>
        /// FileFsVolumeInformation
        /// </summary>
        FileBothDirectoryInformation = 3,

        /// <summary>
        /// File basic information
        /// </summary>
        FileBaseInformation = 4,

        /// <summary>
        /// File standard information
        /// </summary>
        FileStandardInformation = 5,

        /// <summary>
        /// File names information
        /// </summary>
        FileNamesInformation = 12,

        /// <summary>
        /// File attribute tag information
        /// </summary>
        FileAttributeTagInformation = 35,
    }

    /// <summary>
    /// Invalid packet type
    /// </summary>
    public enum InvalidPacketType
    {
        /// <summary>
        /// Invalid component
        /// </summary>
        InvalidComponent,

        /// <summary>
        /// Invalid packet id
        /// </summary>
        InvalidPacketId,

        /// <summary>
        /// Invalid capability type
        /// </summary>
        InvalidCapabilityType,

        /// <summary>
        /// Invalid device type
        /// </summary>
        InvalidDeviceType,
    }

    /// <summary>
    /// ServerDisconenctionPacket
    /// </summary>
    public class ServerDisconenctionPacket
    {
    }

    /// <summary>
    ///  This header is present at the beginning of every message
    ///  in this protocol. The purpose of this header is to
    ///  describe the type of the message.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_1.xml
    ///  </remarks>
    public class RDPDR_HEADER
    {
        /// <summary>
        ///  A 16-bit unsigned integer that identifies the component
        ///  to which the packet is sent. This field MUST be set
        ///  to one of the following values.
        /// </summary>
        public Component_Values Component;

        /// <summary>
        ///  A 16-bit unsigned integer. The PacketId field is a unique
        ///  ID that identifies the packet function. This field
        ///  MUST be set to one of the following values.
        /// </summary>
        public PacketId_Values PacketId;
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
    ///  This is a header that is embedded in the Server Core
    ///  Capability Request and Client Core Capability Response.
    ///  The purpose of this header is to describe capabilities
    ///  for different device types.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_2.xml
    ///  </remarks>
    public class CAPABILITY_HEADER
    {
        /// <summary>
        ///  A 16-bit unsigned integer that identifies the type of
        ///  capability being described. It MUST be set to one of
        ///  the following values.
        /// </summary>
        public CapabilityType_Values CapabilityType;

        /// <summary>
        ///  A 16-bit unsigned integer that specifies the size,
        ///  in bytes, of the capability message, this header included.
        /// </summary>
        public ushort CapabilityLength;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the capability-specific
        ///  version.       
        /// </summary>
        public CAPABILITY_VERSION Version;
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
    /// The CAPABILITY_SET structure is used to describe the type, size, 
    /// and version of a capability set exchanged between clients and servers. 
    /// All capability set messages conform to this basic structure. 
    /// The Capability Message is embedded in the Server Core Capability Request 
    /// and Client Core Capability Response messages
    /// </summary>
    public class CAPABILITY_SET
    {
        /// <summary>
        /// A CAPABILITY_HEADER header. The CapabilityType field of the 
        /// CAPABILITY_HEADER determines the CapabilityMessage type
        /// </summary>
        public CAPABILITY_HEADER Header;

        ///// <summary>
        ///// Capability set data which conforms to the structure of the 
        ///// type given by the CapabilityType field
        ///// </summary>
        //public CapabilityType_Values capabilityData;
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
    ///  This header is embedded in the Client Device List Announce
    ///  message. Its purpose is to describe different types
    ///  of devices.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_3.xml
    ///  </remarks>
    public class DEVICE_ANNOUNCE
    {
        /// <summary>
        ///  A 32-bit unsigned integer that identifies the device
        ///  type. This field MUST be set to one of the following
        ///  values.
        /// </summary>
        public DeviceType_Values DeviceType;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies a unique ID
        ///  that identifies the announced device. This ID can be
        ///  reused only if the device is removed.
        /// </summary>
        public uint DeviceId;

        /// <summary>
        ///  A string of ASCII characters with a maximum length of
        ///  eight characters that represent the name of the device
        ///  as it appears on the client. This field might not be
        ///  null-terminated.
        /// </summary>
        //[StaticSize(8)]
        public byte[] PreferredDosName;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the number
        ///  of bytes in the DeviceData field.
        /// </summary>
        public uint DeviceDataLength;

        /// <summary>
        ///  A variable-length byte array whose size is specified
        ///  by the DeviceDataLength field. The content depends
        ///  on the DeviceType field. See [MS-RDPEPC] section 2.2.2.1
        ///  for the printer device type. See [MS-RDPESP] section
        ///  2.2.2.1 for the serial and parallel port device types.
        ///  See [MS-RDPESC] for the smart card device type.
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public byte[] DeviceData;
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
    ///  This header is embedded in all server requests on a
    ///  specific device.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_4.xml
    ///  </remarks>
    public class DR_DEVICE_IOREQUEST
    {
        /// <summary>
        ///  A common message header. The Component
        ///  field MUST be set to RDPDR_CTYP_CORE, and the PacketId
        ///  field MUST be set to PAKID_CORE_DEVICE_IOREQUEST.
        /// </summary>
        public RDPDR_HEADER Header;

        /// <summary>
        ///  A 32-bit unsigned integer that is a unique ID. The value
        ///  MUST match the DeviceId value in the Client Device
        ///  List Announce Request.
        /// </summary>
        public uint DeviceId;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies a unique ID
        ///  retrieved from the Device Create Response.
        /// </summary>
        public uint FileId;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies a unique ID
        ///  for each request.  The ID is considered valid until
        ///  a Device I/O Response is received. Subsequently, the
        ///  ID can be reused.
        /// </summary>
        public uint CompletionId;

        /// <summary>
        ///  A 32-bit unsigned integer that identifies the request
        ///  function. This field MUST have one of the following
        ///  values.
        /// </summary>
        public MajorFunction_Values MajorFunction;

        /// <summary>
        ///  A 32-bit unsigned integer. This field is valid
        ///  only when the MajorFunction field is set to IRP_MJ_DIRECTORY_CONTROL.
        ///  This field MUST have one of the following values.
        /// </summary>
        public MinorFunction_Values MinorFunction;
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
        ///  Query directory request
        /// </summary>
        IRP_MN_QUERY_DIRECTORY = 0x00000001,

        /// <summary>
        ///  Notify change directory request
        /// </summary>
        IRP_MN_NOTIFY_CHANGE_DIRECTORY = 0x00000002,
    }

    /// <summary>
    ///  This header initiates a create request. This message
    ///  can have different purposes depending on the device
    ///  for which it is issued. The device type is determined
    ///  by the DeviceId field in the DR_DEVICE_IOREQUEST header.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_4_1.xml
    ///  </remarks>
    public class DR_CREATE_REQ
    {
        /// <summary>
        ///  A DR_DEVICE_IOREQUEST  header. The MajorFunction
        ///  field in this header MUST be set to IRP_MJ_CREATE.
        /// </summary>
        public DR_DEVICE_IOREQUEST DeviceIoRequest;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the
        ///  level of access that is wanted. This field is specified
        ///  in [MS-SMB2] section 2.2.13. 
        /// </summary>
        public uint DesiredAccess;

        /// <summary>
        ///  A 64-bit unsigned integer that specifies the
        ///  initial allocation size for the file. 
        /// </summary>
        public ulong AllocationSize;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the
        ///  attributes for the file being created. This field is
        ///  specified in [MS-SMB2] section 2.2.13.
        /// </summary>
        public uint FileAttributes;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the
        ///  sharing mode for the file being opened. This field
        ///  is specified in [MS-SMB2] section 2.2.13.
        /// </summary>
        public uint SharedAccess;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the action
        ///  that the client MUST take if the file already exists.
        ///  This field is specified in [MS-SMB2] section 2.2.13.
        ///  
        /// </summary>
        public uint CreateDisposition;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the options
        ///  for creating the file. This field is specified in [MS-SMB2]
        ///  section 2.2.13. 
        /// </summary>
        public uint CreateOptions;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the
        ///  number of bytes in the Path field.     
        /// </summary>
        public uint PathLength;

        /// <summary>
        ///  A variable-length array of Unicode characters,
        ///  including null-terminator, whose size is specified
        ///  by the PathLength field.
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public byte[] Path;
    }

    /// <summary>
    ///  This header initiates a close request. This message
    ///  can have different purposes depending on the device
    ///  for which it is issued. The device type is determined
    ///  by the DeviceId field in the DR_DEVICE_IOREQUEST header.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_4_2.xml
    ///  </remarks>
    public class DR_CLOSE_REQ
    {
        /// <summary>
        ///  A DR_DEVICE_IOREQUEST header. The MajorFunction field
        ///  in this header MUST be set to IRP_MJ_CLOSE.
        /// </summary>
        public DR_DEVICE_IOREQUEST DeviceIoRequest;

        /// <summary>
        ///  An array of 32 bytes. Reserved. This field can
        ///  be set to any value, and MUST be ignored on receipt.
        /// </summary>
        //[StaticSize(32)]
        public byte[] Padding;
    }

    /// <summary>
    ///  This header initiates a read request. This message can
    ///  have different purposes depending on the device for
    ///  which it is issued. The device type is determined by
    ///  the DeviceId field in the DR_DEVICE_IOREQUEST header.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_4_3.xml
    ///  </remarks>
    public class DR_READ_REQ
    {
        /// <summary>
        ///  A DR_DEVICE_IOREQUEST header. The MajorFunction
        ///  field in this header MUST be set to IRP_MJ_READ.
        /// </summary>
        public DR_DEVICE_IOREQUEST DeviceIoRequest;

        /// <summary>
        ///  A 32-bit unsigned integer. This field specifies
        ///  the maximum number of bytes to be read from the device.
        /// </summary>
        public uint Length;

        /// <summary>
        ///  A 64-bit unsigned integer. This field specifies
        ///  the file offset where the read operation is performed.
        /// </summary>
        public ulong Offset;

        /// <summary>
        ///  An array of 20 bytes. Reserved. This field can
        ///  be set to any value, and MUST be ignored on receipt.
        /// </summary>
        //[StaticSize(20)]
        public byte[] Padding;
    }

    /// <summary>
    ///  This header initiates a write request. This message
    ///  can have different purposes depending on the device
    ///  for which it is issued. The device type is determined
    ///  by the DeviceId field in the DR_DEVICE_IOREQUEST header.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_4_4.xml
    ///  </remarks>
    public class DR_WRITE_REQ
    {
        /// <summary>
        ///  A DR_DEVICE_IOREQUEST header. The MajorFunction
        ///  field in this header MUST be set to IRP_MJ_WRITE.
        /// </summary>
        public DR_DEVICE_IOREQUEST DeviceIoRequest;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the
        ///  number of bytes in the WriteData field.     
        /// </summary>
        public uint Length;

        /// <summary>
        ///  A 64-bit unsigned integer. This field specifies
        ///  the file offset at which the data is written.
        /// </summary>
        public ulong Offset;

        /// <summary>
        ///  An array of 20 bytes. Reserved. This field can
        ///  be set to any value, and MUST be ignored on receipt.
        /// </summary>
        //[StaticSize(20)]
        public byte[] Padding;

        /// <summary>
        ///  A variable-length array of bytes, where the
        ///  length is specified by the Length field in this packet.
        ///  This array contains data to be written on the target
        ///  device.     
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public byte[] WriteData;
    }

    /// <summary>
    ///  This header initiates a device control request. This
    ///  message can have different purposes depending on the
    ///  device for which it is issued. The device type is determined
    ///  by the DeviceId field in the DR_DEVICE_IOREQUEST header.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_4_5.xml
    ///  </remarks>
    public class DR_CONTROL_REQ
    {
        /// <summary>
        ///  A DR_DEVICE_IOREQUEST header. The MajorFunction field
        ///  in this header MUST be set to IRP_MJ_DEVICE_CONTROL.
        /// </summary>
        public DR_DEVICE_IOREQUEST DeviceIoRequest;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the
        ///  maximum number of bytes expected in the OutputBuffer
        ///  field of the Device Control Response.     
        /// </summary>
        public uint OutputBufferLength;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the
        ///  number of bytes in the InputBuffer field.     
        /// </summary>
        public uint InputBufferLength;

        /// <summary>
        ///  A 32-bit unsigned integer. This field is specific
        ///  to the redirected device.     
        /// </summary>
        public uint IoControlCode;

        /// <summary>
        ///  An array of 20 bytes. Reserved. This field can be set
        ///  to any value, and MUST be ignored on receipt.
        /// </summary>
        //[StaticSize(20)]
        public byte[] Padding;

        /// <summary>
        ///  A variable-size byte array whose size is specified
        ///  by the InputBufferLength field.     
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public byte[] InputBuffer;

        /// <summary>
        /// A variable used to check the presence of RDPESC Packet
        /// in the Frame.
        /// </summary>
        public bool isRdpescPresent;
    }

    /// <summary>
    ///  A message with this header indicates that the I/O request
    ///  is complete. In a Device I/O Response message, a request
    ///  message is matched to the appropriate Device I/O Request
    ///  header. There is only one response per request.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_5.xml
    ///  </remarks>
    public class DR_DEVICE_IOCOMPLETION
    {
        /// <summary>
        ///  A common message header. The Component
        ///  field MUST be set to RDPDR_CTYP_CORE, and the PacketId
        ///  field MUST be set to PAKID_CORE_DEVICE_IOCOMPLETION.
        /// </summary>
        public RDPDR_HEADER Header;

        /// <summary>
        ///  A 32-bit unsigned integer. This field MUST match
        ///  the DeviceId field in the DR_DEVICE_IOREQUEST header
        ///  for the corresponding request.
        /// </summary>
        public uint DeviceId;

        /// <summary>
        ///  A 32-bit unsigned integer. This field MUST match
        ///  the CompletionId field in the DR_DEVICE_IOREQUEST header
        ///  for the corresponding request. After processing a response
        ///  packet with this ID, the same ID can be reused in another
        ///  request.
        /// </summary>
        public uint CompletionId;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the NTSTATUS
        ///  code that indicates success or failure for the request.
        ///  NTSTATUS codes are specified in [MS-ERREF] section
        ///  2.3.
        /// </summary>
        public int IoStatus;
    }

    /// <summary>
    ///  A message with this header describes a response
    ///  to a Device Create Request.     
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_5_1.xml
    ///  </remarks>
    public class DR_CREATE_RSP
    {
        /// <summary>
        ///  A DR_DEVICE_IOCOMPLETION header. The
        ///  CompletionId field of this header MUST match a Device
        ///  I/O Request message that had the MajorFunction field
        ///  set to IRP_MJ_CREATE.     
        /// </summary>
        public DR_DEVICE_IOCOMPLETION DeviceIoReply;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies a unique
        ///  ID for the created file object. The ID can be reused
        ///  after sending a Device Close Response.
        /// </summary>
        public uint FileId;

        /// <summary>
        ///  An unsigned 8-bit integer. This field indicates
        ///  the success of the Device Create Request. The Information
        ///  field MUST be set to zero for all responses, except
        ///  for the Client Drive Create Response.
        /// </summary>
        public byte Information;
    }

    /// <summary>
    /// This message is a reply to a Device Close Request.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_5_2.xml
    ///  </remarks>
    public class DR_CLOSE_RSP
    {
        /// <summary>
        ///  A DR_DEVICE_IOCOMPLETION header. The CompletionId
        ///  field of this header MUST match a Device I/O Request
        ///  message that had the MajorFunction field set to IRP_MJ_CLOSE.
        /// </summary>
        public DR_DEVICE_IOCOMPLETION DeviceIoReply;

        /// <summary>
        ///  An array of 5 bytes. Reserved. This field can
        ///  be set to any value, and MUST be ignored on receipt.
        /// </summary>
        //[StaticSize(5)]
        public byte[] Padding;
    }

    /// <summary>
    ///  A message with this header describes a response
    ///  to a Device Read Request.     
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_5_3.xml
    ///  </remarks>
    public class DR_READ_RSP
    {
        /// <summary>
        ///  A DR_DEVICE_IOCOMPLETION header. The CompletionId
        ///  field of this header MUST match a Device I/O Request
        ///  message that had the MajorFunction field set to IRP_MJ_READ.
        /// </summary>
        public DR_DEVICE_IOCOMPLETION DeviceIoReply;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the
        ///  number of bytes in the ReadData field.     
        /// </summary>
        public uint Length;

        /// <summary>
        ///  A variable-length array of bytes that specifies
        ///  the output data from the read request. The length of
        ///  ReadData is specified by the Length field in this packet.
        /// </summary>
        public byte[] ReadData;
    }

    /// <summary>
    ///  A message with this header describes a response
    ///  to a Device Write Request.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_5_4.xml
    ///  </remarks>
    public class DR_WRITE_RSP
    {
        /// <summary>
        ///  A DR_DEVICE_IOCOMPLETION header. The CompletionId
        ///  field of this header MUST match a Device I/O Request
        ///  message that had the MajorFunction field set to IRP_MJ_WRITE.
        /// </summary>
        public DR_DEVICE_IOCOMPLETION DeviceIoReply;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the
        ///  number of bytes written in response to the write request.
        /// </summary>
        public uint Length;

        /// <summary>
        ///  An optional, 8-bit unsigned integer intended to allow
        ///  the client minor flexibility in determining the overall
        ///  packet length. This field is unused and can be set
        ///  to any value. If present, this field MUST be ignored
        ///  on receipt.
        /// </summary>
        public byte Padding;
    }

    /// <summary>
    ///  A message with this header describes a response
    ///  to a Device Control Request.     
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_5_5.xml
    ///  </remarks>
    public class DR_CONTROL_RSP
    {
        /// <summary>
        ///  A DR_DEVICE_IOCOMPLETION header. The CompletionId
        ///  field of this header MUST match a Device I/O Request
        ///  that has the MajorFunction field set to IRP_MJ_DEVICE_CONTROL.
        /// </summary>
        public DR_DEVICE_IOCOMPLETION DeviceIoReply;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies
        ///  the number of bytes in the OutputBuffer field.
        /// </summary>
        public uint OutputBufferLength;

        /// <summary>
        ///  A variable-length array of bytes whose size is
        ///  specified by the OutputBufferLength field. The minimum
        ///  size is 1 byte; that is, if OutputBufferLength is 0,
        ///  this field will have 1 byte of extra padding.
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public byte[] OutputBuffer;
    }

    /// <summary>
    ///  The RDP_LOCK_INFO packet specifies the region of the
    ///  file to lock or unlock.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_6.xml
    ///  </remarks>
    public class RDP_LOCK_INFO
    {
        ///// <summary>
        /////  A 64-bit unsigned integer that specifies the length
        /////  of the region.
        ///// </summary>
        //public ulong Length;

        ///// <summary>
        /////  A 64-bit unsigned integer that specifies the offset
        /////  at which the region starts.
        ///// </summary>
        //public ulong Offset;

        /// <summary>
        /// 
        /// </summary>
        public uint LengthLow;

        /// <summary>
        /// 
        /// </summary>
        public uint LengthHigh;

        /// <summary>
        /// 
        /// </summary>
        public uint OffsetHigh;

        /// <summary>
        /// 
        /// </summary>
        public uint OffsetLow;
    }

    /// <summary>
    ///  The server responds to  a Client Device List Announce
    ///  Request with this message.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_1.xml
    ///  </remarks>
    public class DR_CORE_DEVICE_ANNOUNCE_RSP
    {
        /// <summary>
        ///  A common message header. The Component
        ///  field MUST be set to RDPDR_CTYP_CORE, and the PacketId
        ///  field MUST be set to PAKID_CORE_DEVICE_REPLY.     
        /// </summary>
        public RDPDR_HEADER Header;

        /// <summary>
        ///  A 32-bit unsigned integer. This ID MUST be the
        ///  same as one of the IDs specified in the Client Device
        ///  List Announce Request message. The server sends a separate
        ///  Server Device Announce Response message for each announced
        ///  device.
        /// </summary>
        public uint DeviceId;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the
        ///  NTSTATUS code that indicates the success or failure
        ///  of device initialization. NTSTATUS codes are specified
        ///  in [MS-ERREF] section 2.3.
        /// </summary>
        public uint ResultCode;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;
    }

    /// <summary>
    /// The server initiates the protocol with this message.                     
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_2.xml
    ///  </remarks>
    public class DR_CORE_SERVER_ANNOUNCE_REQ
    {
        /// <summary>
        ///  A common message header. The Component
        ///  field MUST be set to RDPDR_CTYP_CORE, and the PacketId
        ///  field MUST be set to PAKID_CORE_SERVER_ANNOUNCE.
        /// </summary>
        public RDPDR_HEADER Header;

        /// <summary>
        ///  A 16-bit unsigned integer that specifies the server
        ///  major version number. This field MUST be set to 0x0001.
        /// </summary>
        public VersionMajor_Values VersionMajor;

        /// <summary>
        ///  A 16-bit unsigned integer that specifies the server
        ///  minor version number. This field MUST be set to one
        ///  of the following values.
        /// </summary>
        public VersionMinor_Values VersionMinor;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the
        ///  unique ID generated by the server.
        /// </summary>
        public uint ClientId;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;

        /// <summary>
        /// Used to count the number of ServerAnnounceRequestFrames
        /// generated.
        /// </summary>
        public uint FrameCount;
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
    ///  The client replies to the Server Announce Request
    ///  message.                            
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_3.xml
    ///  </remarks>
    public class DR_CORE_SERVER_ANNOUNCE_RSP
    {
        /// <summary>
        ///  A common message header. The Component
        ///  field MUST be set to RDPDR_CTYP_CORE, and the PacketId
        ///  field MUST be set to PAKID_CORE_CLIENTID_CONFIRM. 
        /// </summary>
        public RDPDR_HEADER Header;

        /// <summary>
        /// A 16-bit unsigned integer that specifies the client
        ///  major version number. This field MUST be set to 0x0001.
        /// </summary>
        public DR_CORE_SERVER_ANNOUNCE_RSP_VersionMajor_Values VersionMajor;

        /// <summary>
        ///  A 16-bit unsigned integer that specifies the
        ///  client minor version number. This field MUST be set
        ///  to one of the following values.
        /// </summary>
        public DR_CORE_SERVER_ANNOUNCE_RSP_VersionMinor_Values VersionMinor;

        /// <summary>
        ///  A 32-bit unsigned integer that the client
        ///  MUST set to either the ClientID field, which is supplied
        ///  by the server in the Server Announce Request message,
        ///  or a unique ID.
        /// </summary>
        public uint ClientId;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;
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
    /// The client announces its machine name.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_4.xml
    ///  </remarks>
    public class DR_CORE_CLIENT_NAME_REQ
    {
        /// <summary>
        ///  A common message header. The Component
        ///  field MUST be set to RDPDR_CTYP_CORE, and the PacketId
        ///  field MUST be set to PAKID_CORE_CLIENT_NAME.     
        /// </summary>
        public RDPDR_HEADER Header;

        /// <summary>
        ///  A 32-bit unsigned integer that indicates the
        ///  format of the ComputerName field. This field MUST be
        ///  set to one of the following values.          
        /// </summary>
        public UnicodeFlag_Values UnicodeFlag;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the code
        ///  page of the ComputerName field; it MUST be set to 0.
        /// </summary>
        public uint CodePage;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the number
        ///  of bytes in the ComputerName field.
        /// </summary>
        public uint ComputerNameLen;

        /// <summary>
        ///  A variable-length array of ASCII or Unicode characters,
        ///  the format of which is determined by the UnicodeFlag
        ///  field. This is a string that identifies the client
        ///  computer name. The string MUST be null-terminated.
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public byte[] ComputerName;
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
    ///  The server announces that it has successfully logged
    ///  on to the session.             
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_5.xml
    ///  </remarks>
    public class DR_CORE_USER_LOGGEDON
    {
        /// <summary>
        ///  A common message header.  The Component
        ///  field MUST be set to RDPDR_CTYP_CORE, and the PacketId
        ///  field MUST be set to PAKID_CORE_USER_LOGGEDON.    
        ///  
        /// </summary>
        public RDPDR_HEADER Header;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;
    }

    /// <summary>
    ///  The server confirms the
    ///  client ID sent by the client in the Client Announce
    ///  Reply message.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_6.xml
    ///  </remarks>
    public class DR_CORE_SERVER_CLIENTID_CONFIRM
    {
        /// <summary>
        ///  A common message header. The Component
        ///  field MUST be set to RDPDR_CTYP_CORE, and the PacketId
        ///  field MUST be set to PAKID_CORE_CLIENTID_CONFIRM. 
        /// </summary>
        public RDPDR_HEADER Header;

        /// <summary>
        ///  A 16-bit unsigned integer that specifies
        ///  the server major version number. This field MUST be
        ///  set to 0x0001.
        /// </summary>
        public DR_CORE_SERVER_CLIENTID_CONFIRM_VersionMajor_Values VersionMajor;

        /// <summary>
        ///  A 16-bit unsigned integer that specifies the
        ///  server minor version number. This field MUST be set
        ///  to one of the following values.          
        /// </summary>
        public DR_CORE_SERVER_CLIENTID_CONFIRM_VersionMinor_Values VersionMinor;

        /// <summary>
        ///  A 32-bit unsigned integer that
        ///  confirms the unique ID value of the ClientId field,
        ///  which was sent by the client in the Client Announce
        ///  Reply message.     
        /// </summary>
        public uint ClientId;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;
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
        V4 = 0x000A
    }

    /// <summary>
    ///  The server announces its capabilities and requests
    ///  the same from the client.                         
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_7.xml
    ///  </remarks>
    public class DR_CORE_CAPABILITY_REQ
    {
        /// <summary>
        ///  A common message header. The Component
        ///  field MUST be set to RDPDR_CTYP_CORE, and the PacketId
        ///  field MUST be set to PAKID_CORE_SERVER_CAPABILITY.
        /// </summary>
        public RDPDR_HEADER Header;

        /// <summary>
        ///  A 16-bit integer that specifies the number of items
        ///  in the CapabilityMessage array.
        /// </summary>
        public ushort numCapabilities;

        /// <summary>
        ///  A 16-bit unsigned integer of padding. This field is
        ///  unused, and MUST be ignored on receipt. 
        /// </summary>
        public ushort Padding;

        /// <summary>
        ///  An array of capabilities. The number of capabilities
        ///  is specified by the numCapabilities field.
        /// </summary>
        public CAPABILITY_SET[] CapabilityMessage;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;
    }

    /// <summary>
    ///  The server announces its capabilities and requests
    ///  the same from the client.      
    /// </summary>
    //  <remarks>
    //   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_7.xml
    //  </remarks>
    public class DR_CORE_CAPABILITY_RSP
    {
        /// <summary>
        ///  A common message header. The Component
        ///  field MUST be set to RDPDR_CTYP_CORE, and the PacketId
        ///  field MUST be set to PAKID_CORE_SERVER_CAPABILITY.
        /// </summary>
        public RDPDR_HEADER Header;

        /// <summary>
        ///  A 16-bit integer that specifies the number of items
        ///  in the CapabilityMessage array.
        /// </summary>
        public ushort numCapabilities;

        /// <summary>
        ///  A 16-bit unsigned integer of padding. This field is
        ///  unused, and MUST be ignored on receipt. 
        /// </summary>
        public ushort Padding;

        /// <summary>
        ///  An array of capabilities. The number of capabilities
        ///  is specified by the numCapabilities field.
        /// </summary>
        public CAPABILITY_SET[] CapabilityMessage;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;
    }

    /// <summary>
    ///  This packet is embedded into Server Core Capability
    ///  Request and Client Core Capability Response messages.
    ///  It describes nondevice-specific capabilities.  
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_7_1.xml
    ///  </remarks>
    public class GENERAL_CAPS_SET : CAPABILITY_SET
    {
        ///// <summary>
        /////  A CAPABILITY_HEADER header. The CapabilityType
        /////  field of this header MUST be set to CAP_GENERAL_TYPE.
        /////  The Version field of this header MUST have one of
        /////  the following values:     
        /////  GENERAL_CAPABILITY_VERSION_01 0x0001 Version1. The SpecialTypeDeviceCap field of GENERAL_CAP_SET is not present.
        /////  GENERAL_CAPABILITY_VERSION_02 0x0002 Version2. The SpecialTypeDeviceCap field of GENERAL_CAP_SET is present.
        ///// </summary>
        //public CAPABILITY_HEADER Header;

        /// <summary>
        ///  A 32-bit unsigned integer that is the identifier
        ///  for the operating system that the capabilities are
        ///  describing. This field MUST be set to one of the following
        ///  values.          
        /// </summary>
        public osType_Values osType;

        /// <summary>
        ///  A 32-bit unsigned integer. This field is unused,
        ///  and MUST be set to 0. 
        /// </summary>
        public osVersion_Values osVersion;

        /// <summary>
        ///  A 16-bit unsigned integer. This field MUST be set
        ///  to 1.
        /// </summary>
        public protocolMajorVersion_Values protocolMajorVersion;

        /// <summary>
        ///  A 16-bit unsigned integer. This field MUST
        ///  be set to one of the values described by the VersionMinor
        ///  field of the Server Client ID Confirm packet.     
        /// </summary>
        public ushort protocolMinorVersion;

        /// <summary>
        ///  A 32-bit unsigned integer that identifies
        ///  a bitmask of the supported  I/O requests for the given
        ///  device. If the bit is set, the I/O request is allowed.
        ///  The requests are identified by the MajorFunction field
        ///  in the Device I/O Request header. This field MUST be
        ///  set to a valid combination of the following values.
        /// </summary>
        public ioCode1_Values ioCode1;

        /// <summary>
        ///  A 32-bit unsigned integer that is currently reserved
        ///  for future use, and MUST be set to 0.
        /// </summary>
        public ioCode2_Values ioCode2;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies  extended
        ///  PDU flags. This field MUST be set as a bitmask of the
        ///  following values.          
        /// </summary>
        public extendedPDU_Values extendedPDU;

        /// <summary>
        ///  A 32-bit unsigned integer that is currently reserved
        ///  for future use, and MUST be set to 0.
        /// </summary>
        public extraFlags1_Values extraFlags1;

        /// <summary>
        ///  A 32-bit unsigned integer that is currently reserved
        ///  for future use, and MUST be set to 0.
        /// </summary>
        public extraFlags2_Values extraFlags2;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the number
        ///  of special devices to be redirected before the user
        ///  is logged on. Special devices are those that are safe
        ///  and/or required to be redirected before a user logs
        ///  on (such as smart cards and serial ports).
        /// </summary>
        public uint SpecialTypeDeviceCap;
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

    /// <summary>
    ///  This packet is embedded into Server Core Capability
    ///  Request and Client Core Capability Response messages.
    ///  It indicates that printer devices are supported.                
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_7_2.xml
    ///  </remarks>
    public class PRINTER_CAPS_SET : CAPABILITY_SET
    {
        ///// <summary>
        /////  A CAPABILITY_HEADER header. The CapabilityType field
        /////  of this header MUST be set to CAP_PRINTER_TYPE, and
        /////  the Version field MUST be set to 0x00000001.
        ///// </summary>
        //public CAPABILITY_HEADER Header;
    }

    /// <summary>
    ///  This packet is embedded into Server Core Capability
    ///  Request and Client Core Capability Response messages.
    ///  It indicates that parallel and serial port devices
    ///  are supported.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_7_3.xml
    ///  </remarks>
    public class PORT_CAPS_SET : CAPABILITY_SET
    {
        ///// <summary>
        /////  A CAPABILITY_HEADER header. The CapabilityType field
        /////  of this header MUST be set to CAP_PORT_TYPE, and the
        /////  Version field MUST be set to 0x00000001.
        ///// </summary>
        //public CAPABILITY_HEADER Header;
    }

    /// <summary>
    ///  This packet is embedded into Server Core Capability
    ///  Request and Client Core Capability Response messages.
    ///  It indicates that file system devices are supported.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_7_4.xml
    ///  </remarks>
    public class DRIVE_CAPS_SET : CAPABILITY_SET
    {
        ///// <summary>
        /////  A CAPABILITY_HEADER header. The CapabilityType field
        /////  of this header MUST be set to CAP_DRIVE_TYPE, and the
        /////  Version field MUST be set to 0x00000002.
        ///// </summary>
        //public CAPABILITY_HEADER Header;
    }

    /// <summary>
    ///  This packet is embedded into Server Core Capability
    ///  Request and Client Core Capability Response messages.
    ///  It indicates that smart card devices are supported.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_7_5.xml
    ///  </remarks>
    public class SMARTCARD_CAPS_SET : CAPABILITY_SET
    {
        ///// <summary>
        /////  A CAPABILITY_HEADER header. The CapabilityType field
        /////  of this header MUST be set to CAP_SMARTCARD_TYPE, and
        /////  the Version field MUST be set to 0x00000001.
        ///// </summary>
        //public CAPABILITY_HEADER Header;
    }

    /// <summary>
    ///  The client announces the list of devices to redirect
    ///  on the server.                       
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_9.xml
    /// </remarks>
    public class DR_CORE_DEVICELIST_ANNOUNCE_REQ
    {
        /// <summary>
        ///  A common message header. The Component
        ///  field MUST be set to RDPDR_CTYP_CORE, and the PacketId
        ///  field MUST be set to PAKID_CORE_DEVICELIST_ANNOUNCE.
        /// </summary>
        public RDPDR_HEADER Header;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the number
        ///  of items in the DeviceList array.
        /// </summary>
        public uint DeviceCount;

        /// <summary>
        ///  A variable-length array of DEVICE_ANNOUNCE headers.
        ///  This field specifies a list of devices that are being
        ///  announced. The number of entries is specified by the
        ///  DeviceCount field. There is no alignment padding between
        ///  individual DEVICE_ANNOUNCE structures. They are ordered
        ///  sequentially within this packet.
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public DEVICE_ANNOUNCE[] DeviceList;
    }

    /// <summary>
    ///  	The client announces a list of new file system devices
    ///  to redirect on the server. 				 				 				 				
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_1.xml
    ///  </remarks>
    public class DR_DRIVE_DEVICELIST_ANNOUNCE
    {
        /// <summary>
        ///  A common message header. The Component
        ///  field MUST be set to RDPDR_CTYP_CORE, and the PacketId
        ///  field MUST be set to PAKID_CORE_DEVICELIST_ANNOUNCE.
        /// </summary>
        public RDPDR_HEADER Header;

        /// <summary>
        ///   				A 32-bit unsigned integer that specifies the number
        ///  of entries in the DeviceAnnounce field.
        /// </summary>
        public uint DeviceCount;

        /// <summary>
        ///   				A variable-length array of DEVICE_ANNOUNCE headers.
        ///  The number of entries is specified by the DeviceCount
        ///  field. Each entry is a DEVICE_ANNOUNCE header in which
        ///   the DeviceType field MUST be set to RDPDR_DTYP_FILESYSTEM.
        ///  The drive name MUST be specified in the PreferredDosName
        ///  field; however, if the drive name is larger than the
        ///  allocated size of the PreferredDosName field, then
        ///  the drive name will be truncated to fit. In that case,
        ///  the full name MUST also be specified in the DeviceData
        ///  field, as a null-terminated Unicode string. If the
        ///  DeviceDataLength field is nonzero, the content of the
        ///  PreferredDosName field is ignored. There is no alignment
        ///  padding between individual DEVICE_ANNOUNCE headers.
        ///  They are ordered sequentially within this packet.
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public DEVICE_ANNOUNCE[] DeviceAnnounce;
    }

    /// <summary>
    ///  The client removes a list of already-announced file
    ///  system devices from the server. 				 				 				 				
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_2.xml
    ///  </remarks>
    public class DR_DEVICELIST_REMOVE
    {
        /// <summary>
        ///  A common message header. The Component
        ///  field MUST be set to RDPDR_CTYP_CORE, and the PacketId
        ///  field MUST be set to PAKID_CORE_DEVICELIST_REMOVE.
        /// </summary>
        public RDPDR_HEADER SharedHeader;

        /// <summary>
        ///   				A 32-bit unsigned integer that specifies the number
        ///  of entries in the DeviceIds field.
        /// </summary>
        public uint DeviceCount;

        /// <summary>
        ///   				A variable-length array of 32-bit unsigned integers
        ///  that specifies device IDs. The IDs specified in this
        ///  array match the IDs specified in the Client Drive Device
        ///  List Announce packet.
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public uint[] DeviceIds;
    }

    /// <summary>
    ///  The server opens or creates a file on a redirected
    ///  file system device.                  
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_3_1.xml
    ///  </remarks>
    public class DR_DRIVE_CREATE_REQ
    {
        /// <summary>
        ///  A DR_CREATE_REQ header. The PathLength and Path fields
        ///  contain the file name of the file to be created. The
        ///  file name does not contain a drive letter, which means
        ///  that the client has to find the drive letter based
        ///  on the DeviceId field of the request.
        /// </summary>
        public DR_CREATE_REQ DeviceCreateRequest;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;
    }

    /// <summary>
    ///  The server issues a query directory request on
    ///  a redirected file system device.  This request is used
    ///  to obtain a directory enumeration.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_3_10.xml
    ///  </remarks>
    public class DR_DRIVE_QUERY_DIRECTORY_REQ
    {
        /// <summary>
        ///  A DR_DEVICE_IOREQUEST  header. The MajorFunction
        ///  field in the DR_DEVICE_IOREQUEST header MUST be set
        ///  to IRP_MJ_DIRECTORY_CONTROL, and the MinorFunction
        ///  field MUST be set to IRP_MN_QUERY_DIRECTORY.
        /// </summary>
        public DR_DEVICE_IOREQUEST DeviceIoRequest;

        /// <summary>
        ///  A 32-bit unsigned integer. The possible values
        ///  are specified in [MS-FSCC] section 2.4. This field
        ///  MUST contain one of the following values.
        /// </summary>
        public /* FsInformationClass_Values */ uint FsInformationClass;

        /// <summary>
        ///  An 8-bit unsigned integer. If the value is zero,
        ///  the Path field is not included regardless of the PathLength
        ///  value.
        /// </summary>
        public byte InitialQuery;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the number
        ///  of bytes in the Path field.
        /// </summary>
        public uint PathLength;

        /// <summary>
        ///  An array of 23 bytes. This field is unused and
        ///  can be set to any value. This field MUST be ignored
        ///  on receipt.
        /// </summary>
        //[StaticSize(23)]
        public byte[] Padding;

        /// <summary>
        ///  A variable-length array of Unicode characters that
        ///  specifies the directory on which this operation will
        ///  be performed. The Path field MUST be null-terminated.
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public byte[] Path;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;
    }

    /// <summary>
    /// FsInformationClass_Values
    /// </summary>
    public enum FileInformationClass_Values : uint
    {
        /// <summary>
        /// FileBasicInformation
        /// </summary>
        FileBasicInformation = 0x00000004,

        /// <summary>
        /// FileStandardInformation
        /// </summary>
        FileStandardInformation = 0x00000005,

        /// <summary>
        ///  Basic information about a file or directory. Basic information
        ///  is defined as the file's name, time stamp, and size,
        ///  or its attributes.
        /// </summary>
        FileDirectoryInformation = 0x00000001,

        /// <summary>
        ///  Full information about a file or directory. Full information
        ///  is defined as all the basic information, plus extended
        ///  attribute size.
        /// </summary>
        FileFullDirectoryInformation = 0x00000002,

        /// <summary>
        ///  Full information plus a volume file ID for a file or
        ///  directory. A volume file ID is defined as a unique
        ///  number assigned to a given volume.
        /// </summary>
        FileIdFullDirectoryInformation = 0x00000026,

        /// <summary>
        ///  Basic information plus extended attribute size and short
        ///  name about a file or directory.
        /// </summary>
        FileBothDirectoryInformation = 0x00000003,

        /// <summary>
        ///  FileBothDirectoryInformation plus a volume ID for a
        ///  file or directory.
        /// </summary>
        FileIdBothDirectoryInformation = 0x00000025,

        /// <summary>
        ///  Detailed information on the names of files in a directory.
        /// </summary>
        FileNamesInformation = 0x0000000c,

        /// <summary>
        ///  Detailed information on the attribute of files in a directory.
        /// </summary>
        FileAttributeTagInformation = 0x00000023,

        /// <summary>
        ///  Detailed information on the rename of files in a directory.
        /// </summary>
        FileRenameInformation = 0x0000000A,
    }

    /// <summary>
    /// FsInformationClass_Values
    /// </summary>
    //[Flags()]
    public enum FsInformationClass_Values : uint
    {
        /// <summary>
        /// FileFsLabelInformation
        /// </summary>
        FileFsLabelInformation = 0x00000002,
    }

    /// <summary>
    ///  The server issues a notify change directory request
    ///  on a redirected file system device to request directory
    ///  change notification.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_3_11.xml
    ///  </remarks>
    public class DR_DRIVE_NOTIFY_CHANGE_DIRECTORY_REQ
    {
        /// <summary>
        ///  A DR_DEVICE_IOREQUEST header. The MajorFunction field
        ///  in the DR_DEVICE_IOREQUEST header MUST be set to IRP_MJ_DIRECTORY_CONTROL,
        ///  and the MinorFunction field MUST be set to IRP_MN_NOTIFY_CHANGE_DIRECTORY.
        /// </summary>
        public DR_DEVICE_IOREQUEST DeviceIoRequest;

        /// <summary>
        ///  An 8-bit unsigned integer. If nonzero, a change anywhere
        ///  within the tree will trigger the notification response;
        ///  otherwise, only a change in the root directory will
        ///  do so.
        /// </summary>
        public byte WatchTree;

        /// <summary>
        ///  A 32-bit unsigned integer. This field has the same meaning
        ///  as the CompletionFilter field in the SMB2 CHANGE_NOTIFY
        ///  Request message specified in [MS-SMB2] section 2.2.35.
        /// </summary>
        public uint CompletionFilter;

        /// <summary>
        ///  An array of 27 bytes. This field is unused and
        ///  can be set to any value. This field MUST be ignored
        ///  on receipt.
        /// </summary>
        //[StaticSize(27)]
        public byte[] Padding;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;
    }

    /// <summary>
    ///  The server issues a request to lock or unlock portions
    ///  of a file.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_3_12.xml
    ///  </remarks>
    public class DR_DRIVE_LOCK_REQ
    {
        /// <summary>
        ///  A DR_DEVICE_IOREQUEST header. The MajorFunction field
        ///  in the DR_DEVICE_IOREQUEST header MUST be set to IRP_MJ_LOCK_CONTROL.
        /// </summary>
        public DR_DEVICE_IOREQUEST DeviceIoRequest;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the type of
        ///  the locking operation. 
        /// </summary>
        public Operation_Values Operation;

        ///// <summary>
        /////  If this bit is set, then the client MUST wait for the
        /////  locking operation to complete. If this bit is not set
        /////  and the region cannot be locked, then the request SHOULD
        /////  fail.
        ///// </summary>
        //public bool F;


        /// <summary>
        ///  20 bytes of padding. This field is unused and MUST be
        ///  ignored on receipt.
        /// </summary>
        public /* byte[] */ uint Padding;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the number
        ///  of RDP_LOCK_INFO structures in the Locks array.
        /// </summary>
        public uint NumLocks;

        /// <summary>
        /// An array of 20 bytes. Reserved. This field can be set
        /// to any value, and MUST be ignored on receipt.
        /// </summary>
        public byte[] Padding2;

        /// <summary>
        ///  A variable-length array of RDP_LOCK_INFO structures.
        ///  This field specifies one or more regions of the file
        ///  to lock or unlock.
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public RDP_LOCK_INFO[] Locks;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;

        /// <summary>
        /// Field which represents  F[1 bit] field and Padding[31 bit] field.
        /// </summary>
        public uint Flags;
    }

    /// <summary>
    /// Operation_Values
    /// </summary>
    [Flags()]
    public enum Operation_Values : uint
    {
        /// <summary>
        ///  The server is requesting a shared lock.
        /// </summary>
        RDP_LOWIO_OP_SHAREDLOCK = 0x0000002,

        /// <summary>
        ///  The server is requesting an exclusive lock.
        /// </summary>
        RDP_LOWIO_OP_EXCLUSIVELOCK = 0x0000003,

        /// <summary>
        ///  The server is requesting to unlock multiple portions
        ///  of the file.
        /// </summary>
        RDP_LOWIO_OP_UNLOCK_MULTIPLE = 0x0000005,

        /// <summary>
        /// The server is requesting to unlock a file.
        /// </summary>
        RDP_LOWIO_OP_UNLOCK = 0x0000004,
    }
    //public enum BIT
    //{
    //    UNSET = 0,
    //    SET = 1,

    //}

    /// <summary>
    ///  The server closes a file on a redirected file system
    ///  device.                  
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_3_2.xml
    ///  </remarks>
    public class DR_DRIVE_CLOSE_REQ
    {
        /// <summary>
        ///  A DR_CLOSE_REQ header. This request closes a file opened
        ///  by a Server Create Drive Request.
        /// </summary>
        public DR_CLOSE_REQ DeviceCloseRequest;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;
    }

    /// <summary>
    ///  The server reads from a file on a redirected file
    ///  system device.                  
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_3_3.xml
    ///  </remarks>
    public class DR_DRIVE_READ_REQ
    {
        /// <summary>
        ///  A DR_READ_REQ header. The Length field contains the
        ///  number of bytes to be read from the file. The Offset
        ///  field specifies the offset within the file at which
        ///  the read operation starts.
        /// </summary>
        public DR_READ_REQ DeviceReadRequest;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;
    }

    /// <summary>
    ///  The server writes to a file on a redirected file
    ///  system device.                  
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_3_4.xml
    ///  </remarks>
    public class DR_DRIVE_WRITE_REQ
    {
        /// <summary>
        ///  A DR_WRITE_REQ header. The Length field contains the
        ///  number of bytes to be written to the file. The Offset
        ///  field specifies the offset within the file at which
        ///  the write operation starts.
        /// </summary>
        public DR_WRITE_REQ DeviceWriteRequest;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;
    }

    /// <summary>
    ///  The server issues a device control request on a redirected
    ///  file system device.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_3_5.xml
    ///  </remarks>
    public class DR_DRIVE_CONTROL_REQ
    {
        /// <summary>
        ///  A DR_CONTROL_REQ header. The packet has a structure
        ///  as defined in Device Control Request. The possible
        ///  values for the IoControlCode field are specified in
        ///  [MS-FSCC] section 2.3. The content of the InputBuffer
        ///  field is defined in the request type messages that
        ///  are specified in the same section of [MS-FSCC].
        /// </summary>
        public DR_CONTROL_REQ DeviceControlRequest;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;
    }

    /// <summary>
    ///  The server issues a query volume information request
    ///  on a redirected file system device. 
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_3_6.xml
    ///  </remarks>
    public class DR_DRIVE_QUERY_VOLUME_INFORMATION_REQ
    {
        /// <summary>
        ///  A DR_DEVICE_IOREQUEST header. The MajorFunction
        ///  field in the DR_DEVICE_IOREQUEST header MUST be set
        ///  to IRP_MJ_QUERY_VOLUME_INFORMATION.
        /// </summary>
        public DR_DEVICE_IOREQUEST DeviceIoRequest;

        /// <summary>
        ///  A 32-bit unsigned integer. This field MUST contain one
        ///  of the values specified in [MS-FSCC] section 2.5. 
        /// </summary>
        public uint FsInformationClass;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the
        ///  number of bytes in the QueryVolumeBuffer field.  
        /// </summary>
        public uint Length;

        /// <summary>
        ///  An array of 24 bytes. This field is unused and
        ///  can be set to any value. This field MUST be ignored
        ///  on receipt.
        /// </summary>
        //[StaticSize(24)]
        public byte[] Padding;

        /// <summary>
        ///  A variable-length array of bytes. The size of the
        ///  array is specified by the Length field. The content
        ///  of this field is based on the value of the FsInformationClass
        ///  field, which determines the different structures that
        ///  can be contained in the QueryVolumeBuffer field. For
        ///  a complete list of these structures, refer to [MS-FSCC]
        ///  section 2.5. The "File system information class" table
        ///  defines all the possible values for the FsInformationClass
        ///  field.
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public byte[] QueryVolumeBuffer;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;
    }

    /// <summary>
    ///  The server issues a set volume information request
    ///  on a redirected file system device. 
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_3_7.xml
    ///  </remarks>
    public class DR_DRIVE_SET_VOLUME_INFORMATION_REQ
    {
        /// <summary>
        ///  A DR_DEVICE_IOREQUEST header. The MajorFunction
        ///  field in the DR_DEVICE_IOREQUEST header MUST be set
        ///  to IRP_MJ_SET_VOLUME_INFORMATION.
        /// </summary>
        public DR_DEVICE_IOREQUEST DeviceIoRequest;

        /// <summary>
        ///  A 32-bit unsigned integer. This field MUST contain
        ///  one of the values specified in [MS-FSCC] section 2.5.
        /// </summary>
        public uint FsInformationClass;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the number
        ///  of bytes in the SetVolumeBuffer field.
        /// </summary>
        public uint Length;

        /// <summary>
        ///  An array of 24 bytes. This field is unused and
        ///  can be set to any value. This field MUST be ignored
        ///  on receipt.
        /// </summary>
        //[StaticSize(24)]
        public byte[] Padding;

        /// <summary>
        ///  A variable-length array of bytes. The size of the
        ///  array is specified by the Length field.  The content
        ///  of this field is based on the value of the FsInformationClass
        ///  field, which determines the different structures that
        ///  can be contained in the SetVolumeBuffer field. For
        ///  a complete list of these structures, refer to [MS-FSCC]
        ///  section 2.5. The "File system information class" table
        ///  defines all the possible values for the FsInformationClass
        ///  field.
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public byte[] SetVolumeBuffer;

        /// <summary>
        /// FSCC type
        /// </summary>
        public object SetVolume_Buffer;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;
    }

    /// <summary>
    ///  The server issues a query information request on
    ///  a redirected file system device.  
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_3_8.xml
    ///  </remarks>
    public class DR_DRIVE_QUERY_INFORMATION_REQ
    {
        /// <summary>
        ///  A DR_DEVICE_IOREQUEST  header. The MajorFunction
        ///  field in the DR_DEVICE_IOREQUEST header MUST be set
        ///  to IRP_MJ_QUERY_INFORMATION.
        /// </summary>
        public DR_DEVICE_IOREQUEST DeviceIoRequest;

        /// <summary>
        ///  A 32-bit unsigned integer. This field MUST contain
        ///  one of the values specified in [MS-FSCC] section 2.4.
        /// </summary>
        public uint FsInformationClass;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the number
        ///  of bytes in the QueryBuffer field.
        /// </summary>
        public uint Length;

        /// <summary>
        ///  An array of 24 bytes. This field is unused and
        ///  can be set to any value. This field MUST be ignored
        ///  on receipt.
        /// </summary>
        //[StaticSize(24)]
        public byte[] Padding;

        /// <summary>
        ///  A variable-length array of bytes. The size of the
        ///  array is specified by the Length field. The content
        ///  of this field is based on the value of the FsInformationClass
        ///  field, which determines the different structures that
        ///  can be contained in the QueryBuffer field. For a complete
        ///  list of these structures, refer to [MS-FSCC] section
        ///  2.4. The "File information class" table defines all
        ///  the possible values for the FsInformationClass field.
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public byte[] QueryBuffer;

        /// <summary>
        /// FSCC type
        /// </summary>
        public object Query_Buffer;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;
    }

    /// <summary>
    ///  The server issues a set information request on
    ///  a redirected file system device.  
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_3_9.xml
    ///  </remarks>
    public class DR_DRIVE_SET_INFORMATION_REQ
    {
        /// <summary>
        ///  A DR_DEVICE_IOREQUEST header. The MajorFunction
        ///  field in the DR_DEVICE_IOREQUEST header MUST be set
        ///  to IRP_MJ_SET_INFORMATION.
        /// </summary>
        public DR_DEVICE_IOREQUEST DeviceIoRequest;

        /// <summary>
        ///  A 32-bit unsigned integer. This field MUST contain
        ///  one of the values specified in [MS-FSCC] section 2.4.
        ///  The FsInformationClass field is a 32-bit value, even
        ///  though the values described in [MS-FSCC] are single
        ///  byte only. For the purposes of conversion, the highest
        ///  24 bits are always set to zero.
        /// </summary>
        public uint FsInformationClass;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the number
        ///  of bytes in the SetBuffer field.
        /// </summary>
        public uint Length;

        /// <summary>
        ///  An array of 24 bytes. This field is unused and
        ///  can be set to any value. This field MUST be ignored
        ///  on receipt.
        /// </summary>
        //[StaticSize(24)]
        public byte[] Padding;

        /// <summary>
        ///  A variable-length array of bytes. The size of the
        ///  array is specified by the Length field. The content
        ///  of this field is based on the value of the FsInformationClass
        ///  field, which determines the different structures that
        ///  can be contained in the SetBuffer field. For a complete
        ///  list of these structures, refer to [MS-FSCC] section
        ///  2.4. The "File information class" table defines all
        ///  the possible values for the FsInformationClass field
        ///  with the exception of the following values: FileDispositionInformation
        ///  or FileRenameInformation.
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public byte[] SetBuffer;

        /// <summary>
        /// FSCC type
        /// </summary>
        public object Set_Buffer;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;
    }

    /// <summary>
    /// A structure representing FileRenameInformation as a
    /// possible value of the FsInformationClass field. All fields have
    /// the same meaning as in FILE_RENAME_INFORMATION in [MS-FSCC]
    /// section 2.4.34. The differences are only in the layout of the fields.
    /// </summary>
    public class RDP_FILE_RENAME_INFORMATION
    {
        /// <summary>
        /// MUST be an 8-bit field that is set to 1 to indicate that if a
        /// file with the given name already exists, it SHOULD be replaced
        /// with the given file. If set to 0, the rename operation MUST
        /// fail if a file with the given name already exists.
        /// </summary>
        public byte ReplaceIfExists;

        /// <summary>
        /// It contains the file handle for the root directory. For
        /// network operations, this value MUST always be zero.
        /// </summary>
        public byte RootDirectory;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the length, in bytes,
        /// of the file name contained within the FileName member.
        /// </summary>
        public uint FileNameLength;

        /// <summary>
        /// A sequence of Unicode characters containing the file name.
        /// When working with this field, use FileNameLength to determine
        /// the length of the file name rather than assuming the presence
        /// of a trailing null delimiter. If the RootDirectory member is 0,
        /// this member MUST specify a full pathname to be assigned to the
        /// file. For network operations, this pathname is relative to the
        /// root of the share. If the RootDirectory member is not 0, this
        /// member MUST specify a pathname, relative to RootDirectory, for
        /// the new name of the file.
        /// </summary>
        public byte[] FileName;
    }

    /// <summary>
    ///  This type of message is sent by the client
    ///  as a response to the Server Drive I/O Request.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_4.xml
    ///  </remarks>
    public class DR_DRIVE_CORE_DEVICE_IOCOMPLETION
    {
        /// <summary>
        ///  A DR_DEVICE_IOCOMPLETION header. This common response
        ///  header indicating the I/O response is the same as the
        ///  Device I/O Response.     
        /// </summary>
        public DR_DEVICE_IOCOMPLETION DeviceIoResponse;
    }

    /// <summary>
    ///  This message is sent by the client as a
    ///  response to the  Server Create Drive Request.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_4_1.xml
    ///  </remarks>
    public class DR_DRIVE_CREATE_RSP
    {
        /// <summary>
        ///  A DR_CREATE_RSP header. This response indicates the
        ///  success or failure of the opening of the specified
        ///  file. It matches the common Device Create Response.
        ///  If the create operation is successful, the content
        ///  of the Information field in the Device Create Response
        ///  message MUST have one of the following values, depending
        ///  on the value of the CreateDisposition field of the
        ///  Server Create Drive Request message. In case of failure,
        ///  the Information field can be set to any value, and
        ///  MUST be ignored on receipt.ValueMeaningFILE_OPENED0x00000001The
        ///  	CreateDisposition field was set to FILE_OPEN_IF.FILE_OVERWRITTEN0x00000003The
        ///  	CreateDisposition field was set to FILE_OVERWRITE_IF.FILE_SUPERSEDED0x00000000The
        ///  	CreateDisposition field was set to any other value.
        /// </summary>
        public DR_CREATE_RSP DeviceCreateResponse;
    }

    /// <summary>
    ///  This message is sent by the client as a response to
    ///  the Server Drive Set Information Request.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_4_10.xml
    ///  </remarks>
    public class DR_DRIVE_SET_INFORMATION_RSP
    {
        /// <summary>
        ///  A DR_DEVICE_IOCOMPLETION header. The CompletionId field
        ///  of the DR_DEVICE_IOCOMPLETION header MUST match a Device
        ///  I/O Request that has the MajorFunction field set to
        ///  IRP_MJ_SET_INFORMATION.     
        /// </summary>
        public DR_DEVICE_IOCOMPLETION DeviceIoReply;

        /// <summary>
        ///  A 32-bit unsigned integer. This field MUST be equal
        ///  to the Length field in the Server Drive Set Information
        ///  Request .
        /// </summary>
        public uint Length;

        /// <summary>
        ///  An optional, 8-bit unsigned integer that is intended
        ///  to allow the client minor flexibility in determining
        ///  the overall packet length. This field is unused, and
        ///  can be set to any value. If present, this field MUST
        ///  be ignored on receipt.
        /// </summary>
        public byte Padding;
    }

    /// <summary>
    ///  This message is sent by the client as a response to
    ///  the Server Drive Query Directory Request.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_4_11.xml
    ///  </remarks>
    public class DR_DRIVE_QUERY_DIRECTORY_RSP
    {
        /// <summary>
        ///  A DR_DEVICE_IOCOMPLETION  header. The CompletionId field
        ///  of the DR_DEVICE_IOCOMPLETION header MUST match a Device
        ///  I/O Request that has the MajorFunction field set to
        ///  IRP_MJ_DIRECTORY_CONTROL and the MinorFunction field
        ///  set to IRP_MN_QUERY_DIRECTORY.
        /// </summary>
        public DR_DEVICE_IOCOMPLETION DeviceIoReply;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the number
        ///  of bytes in the Buffer field.
        /// </summary>
        public uint Length;

        /// <summary>
        ///  A variable-length array of bytes, in which the number
        ///  of bytes is specified in the Length field. The content
        ///  of this field is based on the value of the FsInformationClass
        ///  field in the Server Drive Query Directory Request message,
        ///  which determines the different structures that can
        ///  be contained in the Buffer field. For a complete list
        ///  of these structures, refer to [MS-FSCC] section 2.4.
        ///  The "File information class" table defines all the
        ///  possible values for the FsInformationClass field.
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public byte[] Buffer;

        /// <summary>
        ///  An optional, 8-bit unsigned integer intended to allow
        ///  the client minor flexibility in determining the overall
        ///  packet length. This field is unused and can be set
        ///  to any value. If present, this field MUST be ignored
        ///  on receipt.
        /// </summary>
        public byte Padding;
    }

    /// <summary>
    ///  This message is sent by the client as a response to
    ///  the Server Drive NotifyChange Directory Request. 
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_4_12.xml
    ///  </remarks>
    public class DR_DRIVE_NOTIFY_CHANGE_DIRECTORY_RSP
    {
        /// <summary>
        ///  A DR_DEVICE_IOCOMPLETION header. The CompletionId
        ///  field of the DR_DEVICE_IOCOMPLETION header MUST match
        ///  a Device I/O Request that has the MajorFunction field
        ///  set to IRP_MJ_DIRECTORY_CONTROL and the MinorFunction
        ///  field set to IRP_MN_NOTIFY_CHANGE_DIRECTORY.
        /// </summary>
        public DR_DEVICE_IOCOMPLETION DeviceIoReply;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the number
        ///  of bytes in the Buffer field.
        /// </summary>
        public uint Length;

        /// <summary>
        ///  A variable-length array of bytes, in which the number
        ///  of bytes is specified in the Length field. This field
        ///  has the same meaning as the Buffer field in the SMB2
        ///  CHANGE_NOTIFY Response message specified in [MS-SMB2]
        ///  section 2.2.36. This buffer will be empty when the
        ///  Server Close Drive Request message has been issued
        ///  and no drive-specific events have occurred. 
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public byte[] Buffer;

        /// <summary>
        ///  An optional, 8-bit unsigned integer intended to allow
        ///  the client minor flexibility in determining the overall
        ///  packet length. This field is unused and can be set
        ///  to any value. If present, this field MUST be ignored
        ///  on receipt.
        /// </summary>
        public byte Padding;
    }

    /// <summary>
    ///  This message is sent by the client as a response to
    ///  the Server Drive Lock Control Request.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_4_13.xml
    ///  </remarks>
    public class DR_DRIVE_LOCK_RSP
    {
        /// <summary>
        ///  A DR_DEVICE_IOCOMPLETION header. The CompletionId
        ///  field of the DR_DEVICE_IOCOMPLETION header MUST match
        ///  a Device I/O Request that has the MajorFunction field
        ///  set to IRP_MJ_LOCK_CONTROL.
        /// </summary>
        public DR_DEVICE_IOCOMPLETION DeviceIoReply;

        /// <summary>
        ///  5 bytes of padding. This field is unused and MUST be
        ///  ignored on receipt.
        /// </summary>
        //[StaticSize(5)]
        public byte[] Padding;
    }

    /// <summary>
    ///  This message is sent by the client as a
    ///  response to the Server Close Drive Request.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_4_2.xml
    ///  </remarks>
    public class DR_DRIVE_CLOSE_RSP
    {
        /// <summary>
        ///  A DR_CLOSE_RSP packet. The DR_CLOSE_RSP packet is sent
        ///  in response to DR_DRIVE_CLOSE_REQ; it is the same as
        ///  the common Device Close Response.
        /// </summary>
        public DR_CLOSE_RSP DeviceCloseResponse;
    }

    /// <summary>
    ///  This message is sent by the client as a
    ///  response to the Server Drive Read Request.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_4_3.xml
    ///  </remarks>
    public class DR_DRIVE_READ_RSP
    {
        /// <summary>
        ///  Returns the result of the DR_DRIVE_READ_REQ; it is the
        ///  same as the common Device Read Response. If successful
        ///  (that is, if the IoStatus field is equal to STATUS_SUCCESS),
        ///  then the amount of data read is any number between
        ///  one and the number of bytes specified by the Length
        ///  field in the Server Drive Read Request message. 
        /// </summary>
        public DR_READ_RSP DeviceReadResponse;
    }

    /// <summary>
    ///  This message is sent by the client as a
    ///  response to the Server Drive Write Request.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_4_4.xml
    ///  </remarks>
    public class DR_DRIVE_WRITE_RSP
    {
        /// <summary>
        ///  Returns the result of DR_DRIVE_WRITE_REQ; it is the
        ///  same as the common Device Write Response. If successful
        ///  (that is, if the IoStatus field is equal to STATUS_SUCCESS),
        ///  then the number of bytes written is specified by the
        ///  Length field of the Server Drive Write Request message.
        /// </summary>
        public DR_WRITE_RSP DeviceWriteResponse;
    }

    /// <summary>
    ///  This message is sent by the client as a
    ///  response to the Server Drive Control Request.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_4_5.xml
    ///  </remarks>
    public class DR_DRIVE_CONTROL_RSP
    {
        /// <summary>
        ///  Returns the result of DR_DRIVE_CONROL_REQ; it
        ///  is the same as the common Device Control Response.
        ///  The content of the OutputBuffer field is described
        ///  in [MS-FSCC] section 2.3 as a reply type message.
        /// </summary>
        public DR_CONTROL_RSP DeviceIoResponse;
    }

    /// <summary>
    ///  This message is sent by the client as a response to
    ///  the Server Drive Query Volume Information Request.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_4_6.xml
    ///  </remarks>
    public class DR_DRIVE_QUERY_VOLUME_INFORMATION_RSP
    {
        /// <summary>
        ///  A DR_DEVICE_IOCOMPLETION header. The CompletionId field
        ///  of the DR_DEVICE_IOCOMPLETION header MUST match a Device
        ///  I/O Request that has the MajorFunction field set to
        ///  IRP_MJ_QUERY_VOLUME_INFORMATION.     
        /// </summary>
        public DR_DEVICE_IOCOMPLETION DeviceIoReply;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the number
        ///  of bytes in the Buffer field.
        /// </summary>
        public uint Length;

        /// <summary>
        ///  A variable-length array of bytes whose size is specified
        ///  by the  Length field. The content of this field is
        ///  based on the value of the FsInformationClass field
        ///  in the Server Drive Query Volume Information Request
        ///  message, which determines the different structures
        ///  that can be contained in the Buffer field. For a complete
        ///  list of these structures, refer to [MS-FSCC] section
        ///  2.5. The "File system information class" table defines
        ///  all the possible values for the FsInformationClass
        ///  field.
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public byte[] Buffer;

        /// <summary>
        ///  An optional, 8-bit unsigned integer that is intended
        ///  to allow the client minor flexibility in determining
        ///  the overall packet length. This field is unused, and
        ///  can be set to any value. If present, this field MUST
        ///  be ignored on receipt.
        /// </summary>
        public byte Padding;
    }

    /// <summary>
    ///  This message is sent by the client as a response to
    ///  the Server Drive Set Volume Information Request. 
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_4_8.xml
    ///  </remarks>
    public class DR_DRIVE_SET_VOLUME_INFORMATION_RSP
    {
        /// <summary>
        ///  A DR_DEVICE_IOCOMPLETION header. The CompletionId field
        ///  of the DR_DEVICE_IOCOMPLETION header MUST match a Device
        ///  I/O Request that has the MajorFunction field set to
        ///  IRP_MJ_SET_VOLUME_INFORMATION.     
        /// </summary>
        public DR_DEVICE_IOCOMPLETION DeviceIoReply;

        /// <summary>
        ///  A 32-bit unsigned integer. It MUST match the Length
        ///  field in the Server Drive Set Volume Information Request.
        /// </summary>
        public uint Length;
    }

    /// <summary>
    ///  This message is sent by the client as a response to
    ///  the Server Drive Query Information Request.     
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_3_4_9.xml
    ///  </remarks>
    public class DR_DRIVE_QUERY_INFORMATION_RSP
    {
        /// <summary>
        ///  A DR_DEVICE_IOCOMPLETION header. The CompletionId field
        ///  of the DR_DEVICE_IOCOMPLETION header MUST match a Device
        ///  I/O Request that has the MajorFunction field set to
        ///  IRP_MJ_QUERY_INFORMATION.
        /// </summary>
        public DR_DEVICE_IOCOMPLETION DeviceIoReply;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the number
        ///  of bytes in the Buffer field.
        /// </summary>
        public uint Length;

        /// <summary>
        ///  A variable-length array of bytes, in which the number
        ///  of bytes is specified in the Length field. The content
        ///  of this field is based on the value of the FsInformationClass
        ///  field in the Server Drive Query Information Request
        ///  message, which determines the different structures
        ///  that can be contained in the Buffer field. For a complete
        ///  list of these structures, refer to [MS-FSCC] section
        ///  2.4. The "File information class" table defines all
        ///  the possible values for the FsInformationClass field.
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public byte[] Buffer;
    }

    /// <summary>
    /// MsgId is a structure which is used to Parse the RDPEFS frames from the Capture.
    /// </summary>
    public struct MsgId
    {
        /// <summary>
        /// packet id
        /// </summary>
        public object packetId;

        /// <summary>
        /// major function
        /// </summary>
        public object majorFunc;

        /// <summary>
        /// minor function
        /// </summary>
        public object minorFunc;
    }

    /// <summary>
    ///  This information class is used to query or set file
    ///  information.The FILE_BASIC_INFORMATION data element
    ///  is as follows:
    /// </summary>
    public class FileBasicInformation
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the time when
        ///  the file was created. All dates and times in this message
        ///  are in absolute system-time format, which is represented
        ///  as a FILETIME structure. This field should be set to
        ///  an integer value greater than or equal to 0; alternately,
        ///  it can be set to (-1) to indicate that this time field
        ///  should not be updated by the server.
        /// </summary>
        public ulong CreationTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last time
        ///  the file was accessed in the format of a FILETIME structure.
        ///  This field should be set to an integer value greater
        ///  than or equal to 0; alternately, it can be set to (-1)
        ///  to indicate that this time field should not be updated
        ///  by the server.
        /// </summary>
        public ulong LastAccessTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last time
        ///  information was written to the file in the format of
        ///  a FILETIME structure. This field should be set to an
        ///  integer value greater than or equal to 0; alternately,
        ///  it can be set to (-1) to indicate that this time field
        ///  should not be updated by the server.
        /// </summary>
        public ulong LastWriteTime;

        /// <summary>
        ///  A 64-bit signed integer that contains the last time
        ///  the file was changed in the format of a FILETIME structure.
        ///  This field should be set to an integer value greater
        ///  than or equal to 0; alternately, it can be set to (-1)
        ///  to indicate that this time field should not be updated
        ///  by the server.
        /// </summary>
        public ulong ChangeTime;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the file attributes. The
        ///  file system updates the values of the LastAccessTime,
        ///  LastWriteTime, and ChangeTime members as appropriate
        ///  after an I/O operation is performed on a file. However,
        ///  a driver or application can request that the file system
        ///  not update one or more of these members for I/O operations
        ///  that are performed on the caller's file handle by setting
        ///  the appropriate members to -1. The caller can set one,
        ///  all, or any other combination of these three members
        ///  to -1. Only the members that are set to -1 will be
        ///  unaffected by I/O operations on the file handle; the
        ///  other members will be updated as appropriate.
        /// </summary>
        public uint FileAttributes;

        /// <summary>
        ///  A 32-bit field.  This field is reserved.  This field
        ///  MAY be set to any value, and MUST be ignored.
        /// </summary>
        public uint Reserved;
    }

    /// <summary>
    ///  This information class is used to query or set file
    ///  information.The FILE_STANDARD_INFORMATION data element is as follows:
    /// </summary>
    public class FileStandardInformation
    {

        /// <summary>
        ///  A 64-bit signed integer that contains the file
        ///  allocation size in bytes. Usually, this value is a
        ///  multiple of the sector or cluster size of the underlying
        ///  physical device. The value of this field MUST be greater
        ///  than or equal to 0.
        /// </summary>
        public long AllocationSize;

        /// <summary>
        ///  A 64-bit signed integer that contains the absolute
        ///  new end-of-file position as a byte offset from the
        ///  start of the file. EndOfFile specifies the offset to
        ///  the byte immediately following the last valid byte
        ///  in the file. Because this value is zero-based, it actually
        ///  refers to the first free byte in the file. That is,
        ///  it is the offset from the beginning of the file at
        ///  which new bytes appended to the file will be written.
        ///  The value of this field MUST be greater than or equal
        ///  to 0.
        /// </summary>
        public long EndOfFile;

        /// <summary>
        ///  A 32-bit unsigned integer that contains the number
        ///  of non-deleted links to this file.
        /// </summary>
        public uint NumberOfLinks;

        /// <summary>
        ///  An 8-bit field that MUST be set to 1 to indicate that
        ///  a file deletion has been requested; otherwise, 0.
        /// </summary>
        public byte DeletePending;

        /// <summary>
        ///  An 8-bit field that MUST be set to 1 to indicate that
        ///  the file is a directory; otherwise, 0.
        /// </summary>
        public byte Directory;

        /// <summary>
        ///  A 16-bit field.  This field is reserved.  This
        ///  field MAY be set to any value, and MUST be ignored.
        /// </summary>
        public byte Reserved;
    }

    /// <summary>
    ///  This information class is used to set the label for
    ///  a file system volume. The message contains a FILE_FS_LABEL_INFORMATION
    ///  data element.  The FILE_FS_LABEL_INFORMATION data
    ///  element is as follows:
    /// </summary>
    public class FileFsLabelInformation
    {
        /// <summary>
        ///  A 32-bit unsigned integer that contains the length,
        ///  in bytes, including the trailing NULL, if present,
        ///  of the name for the volume.
        /// </summary>
        public uint VolumeLabelLength;

        /// <summary>
        ///  A variable-length Unicode field containing the name
        ///  of the volume. The content of this field can be a NULL-terminated
        ///  string, or it can be a string padded with the space
        ///  character to be VolumeLabelLength bytes long.
        /// </summary>
        public ushort[] VolumeLabel;
    }

    /// <summary>
    ///  This information class is used to query for attribute
    ///  and reparse tag information for a file. 				The FILE_ATTRIBUTE_TAG_INFORMATION
    ///  data element is as follows:
    /// </summary>
    public class FileAttributeTagInformation
    {
        /// <summary>
        ///  A 32-bit unsigned integer that contains the file attributes.
        /// </summary>
        public uint FileAttributes;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the reparse
        ///  point tag. If the FileAttributes member includes the
        ///  FILE_ATTRIBUTE_REPARSE_POINT attribute flag, this member
        ///  specifies the reparse tag. Otherwise, this member SHOULD
        ///  be set to 0, and MUST be ignored.
        /// </summary>
        public uint ReparseTag;
    }
}

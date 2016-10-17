using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpefs
{
    #region Common Data Type
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

        public RDPDR_HEADER(){}
        public RDPDR_HEADER(Component_Values component, PacketId_Values packetId)
        {
            this.Component = component;
            this.PacketId = packetId;
        }

        public void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt16((UInt16)this.Component);
            marshaler.WriteUInt16((UInt16)this.PacketId);
        }

        public void Decode(PduMarshaler marshaler)
        {
            try
            {
                this.Component = (Component_Values)marshaler.ReadUInt16();
                this.PacketId = (PacketId_Values)marshaler.ReadUInt16();
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
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

        public CAPABILITY_HEADER() { }

        public CAPABILITY_HEADER(CapabilityType_Values type, CAPABILITY_VERSION ver)
        {
            this.CapabilityType = type;
            this.Version = ver;
        }

        public CAPABILITY_HEADER(CapabilityType_Values type)
        {
            this.CapabilityType = type;
        }

        public void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt16((UInt16)this.CapabilityType);
            marshaler.WriteUInt16(this.CapabilityLength);
            marshaler.WriteUInt32((UInt32)this.Version);
        }

        public bool Decode(PduMarshaler marshaler)
        {
            try
            {
                this.CapabilityType = (CapabilityType_Values)marshaler.ReadUInt16();
                this.CapabilityLength = marshaler.ReadUInt16();
                this.Version = (CAPABILITY_VERSION)marshaler.ReadUInt32();
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
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
        public CAPABILITY_SET() { }
        public CAPABILITY_SET(CapabilityType_Values type, CAPABILITY_VERSION ver)
        {
            this.Header = new CAPABILITY_HEADER(type, ver);
        }

        public CAPABILITY_SET(CapabilityType_Values type)
        {
            this.Header = new CAPABILITY_HEADER(type);
        }
        

        public virtual void Encode(PduMarshaler marshaler)
        {
            Header.Encode(marshaler);
        }

        public virtual void Decode(PduMarshaler marshaler)
        {
            Header.Decode(marshaler);
        }

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

        public void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt32((UInt32)this.DeviceType);
            marshaler.WriteUInt32(this.DeviceId);
            marshaler.WriteBytes(this.PreferredDosName);
            marshaler.WriteUInt32(this.DeviceDataLength);
            if (this.DeviceDataLength > 0)
                marshaler.WriteBytes(this.DeviceData);

        }

        public DEVICE_ANNOUNCE() { }

        public void Decode(PduMarshaler marshaler)
        {
            try
            {
                this.DeviceType = (DeviceType_Values)marshaler.ReadUInt32();
                this.DeviceId = marshaler.ReadUInt32();
                this.PreferredDosName = marshaler.ReadBytes(8);
                this.DeviceDataLength = marshaler.ReadUInt32();
                this.DeviceData = marshaler.ReadBytes((int)this.DeviceDataLength);
             }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
    }

    /// <summary>
    ///  This header is embedded in all server requests on a
    ///  specific device.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_4.xml
    ///  </remarks>
    public class DR_DEVICE_IOREQUEST : RdpefsPDU
    {
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

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(this.DeviceId);
            marshaler.WriteUInt32(this.FileId);
            marshaler.WriteUInt32(this.CompletionId);
            marshaler.WriteUInt32((UInt32)this.MajorFunction);
            marshaler.WriteUInt32((UInt32)this.MinorFunction);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.DeviceId = (uint)marshaler.ReadInt32();
                this.FileId = (uint)marshaler.ReadInt32();
                this.CompletionId = (uint)marshaler.ReadInt32();
                this.MajorFunction = (MajorFunction_Values)marshaler.ReadInt32();
                if (MajorFunction.HasFlag(MajorFunction_Values.IRP_MJ_DIRECTORY_CONTROL))
                {
                    this.MinorFunction = (MinorFunction_Values)marshaler.ReadInt32();
                }
                else
                {
                    marshaler.ReadInt32();
                    this.MinorFunction = MinorFunction_Values.None;
                }

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
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
    public class DR_DEVICE_IOCOMPLETION : RdpefsPDU
    {
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
        public uint IoStatus;

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(this.DeviceId);
            marshaler.WriteUInt32(this.CompletionId);
            marshaler.WriteUInt32(this.IoStatus);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.DeviceId = marshaler.ReadUInt32();
                this.CompletionId = marshaler.ReadUInt32();
                this.IoStatus = marshaler.ReadUInt32();
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
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
    public class DR_CREATE_REQ : DR_DEVICE_IOREQUEST
    {
        ///// <summary>
        /////  A DR_DEVICE_IOREQUEST  header. The MajorFunction
        /////  field in this header MUST be set to IRP_MJ_CREATE.
        ///// </summary>
        //public DR_DEVICE_IOREQUEST DeviceIoRequest;

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

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(this.DesiredAccess);
            marshaler.WriteUInt64(this.AllocationSize);
            marshaler.WriteUInt32(this.FileAttributes);
            marshaler.WriteUInt32(this.SharedAccess);
            marshaler.WriteUInt32(this.CreateDisposition);
            marshaler.WriteUInt32(this.CreateOptions);
            marshaler.WriteUInt32(this.PathLength);
            marshaler.WriteBytes(this.Path);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.DesiredAccess = marshaler.ReadUInt32();
                this.AllocationSize = marshaler.ReadUInt64();
                this.FileAttributes = marshaler.ReadUInt32();
                this.SharedAccess = marshaler.ReadUInt32();
                this.CreateDisposition = marshaler.ReadUInt32();
                this.CreateOptions = marshaler.ReadUInt32();
                this.PathLength = marshaler.ReadUInt32();
                this.Path = marshaler.ReadBytes((int)this.PathLength);
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
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
    public class DR_CLOSE_REQ : DR_DEVICE_IOREQUEST
    {
        /// <summary>
        ///  A DR_DEVICE_IOREQUEST header. The MajorFunction field
        ///  in this header MUST be set to IRP_MJ_CLOSE.
        /// </summary>

        /// <summary>
        ///  An array of 32 bytes. Reserved. This field can
        ///  be set to any value, and MUST be ignored on receipt.
        /// </summary>
        //[StaticSize(32)]
        public byte[] Padding;

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteBytes(this.Padding);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                marshaler.ReadBytes(32);
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
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
    public class DR_READ_REQ : DR_DEVICE_IOREQUEST
    {
        /// <summary>
        ///  A DR_DEVICE_IOREQUEST header. The MajorFunction
        ///  field in this header MUST be set to IRP_MJ_READ.
        /// </summary>

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

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(this.Length);
            marshaler.WriteUInt64(this.Offset);
            marshaler.WriteBytes(this.Padding);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.Length = marshaler.ReadUInt32();
                this.Offset = marshaler.ReadUInt64();
                this.Padding = marshaler.ReadBytes(20);
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
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
    public class DR_WRITE_REQ : DR_DEVICE_IOREQUEST
    {
        /// <summary>
        ///  A DR_DEVICE_IOREQUEST header. The MajorFunction
        ///  field in this header MUST be set to IRP_MJ_WRITE.
        /// </summary>

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

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.Length = marshaler.ReadUInt32();
                this.Offset = marshaler.ReadUInt64();
                this.Padding = marshaler.ReadBytes(20);
                this.WriteData = marshaler.ReadBytes((int)this.Length);
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
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
    public class DR_CONTROL_REQ : DR_DEVICE_IOREQUEST
    {
        /// <summary>
        ///  A DR_DEVICE_IOREQUEST header. The MajorFunction field
        ///  in this header MUST be set to IRP_MJ_DEVICE_CONTROL.
        /// </summary>

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

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(this.OutputBufferLength);
            marshaler.WriteUInt32(this.InputBufferLength);
            marshaler.WriteUInt32(this.IoControlCode);
            marshaler.WriteBytes(this.Padding);
            marshaler.WriteBytes(this.InputBuffer);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.OutputBufferLength = marshaler.ReadUInt32();
                this.InputBufferLength = marshaler.ReadUInt32();
                this.IoControlCode = marshaler.ReadUInt32();
                this.Padding = marshaler.ReadBytes(20);
                this.InputBuffer = marshaler.ReadBytes((int)this.InputBufferLength);
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
    }

    /// <summary>
    ///  A message with this header describes a response
    ///  to a Device Create Request.     
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_5_1.xml
    ///  </remarks>
    public class DR_CREATE_RSP : DR_DEVICE_IOCOMPLETION
    {
        /// <summary>
        ///  A DR_DEVICE_IOCOMPLETION header. The
        ///  CompletionId field of this header MUST match a Device
        ///  I/O Request message that had the MajorFunction field
        ///  set to IRP_MJ_CREATE.     
        /// </summary>

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
        public DR_Create_RSP_InformationValue Information;

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(this.FileId);
            marshaler.WriteByte((byte)this.Information);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.FileId = marshaler.ReadUInt32();
                this.Information = (DR_Create_RSP_InformationValue)marshaler.ReadByte();
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
    }

    /// <summary>
    /// This message is a reply to a Device Close Request.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_5_2.xml
    ///  </remarks>
    public class DR_CLOSE_RSP : DR_DEVICE_IOCOMPLETION
    {
        /// <summary>
        ///  A DR_DEVICE_IOCOMPLETION header. The CompletionId
        ///  field of this header MUST match a Device I/O Request
        ///  message that had the MajorFunction field set to IRP_MJ_CLOSE.
        /// </summary>

        /// <summary>
        ///  An array of 5 bytes. Reserved. This field can
        ///  be set to any value, and MUST be ignored on receipt.
        /// </summary>
        //[StaticSize(5)]
        public byte[] Padding;

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteBytes(this.Padding);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.Padding = marshaler.ReadBytes(4);
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            } 
        }
    }

    /// <summary>
    ///  A message with this header describes a response
    ///  to a Device Read Request.     
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_5_3.xml
    ///  </remarks>
    public class DR_READ_RSP : DR_DEVICE_IOCOMPLETION
    {
        /// <summary>
        ///  A DR_DEVICE_IOCOMPLETION header. The CompletionId
        ///  field of this header MUST match a Device I/O Request
        ///  message that had the MajorFunction field set to IRP_MJ_READ.
        /// </summary>

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

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(this.Length);
            marshaler.WriteBytes(this.ReadData);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.Length = marshaler.ReadUInt32();
                if (this.Length > 0)
                    this.ReadData = marshaler.ReadBytes((int)this.Length);
                return true;
               
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
            
        }
    }

    /// <summary>
    ///  A message with this header describes a response
    ///  to a Device Write Request.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_5_4.xml
    ///  </remarks>
    public class DR_WRITE_RSP : DR_DEVICE_IOCOMPLETION
    {
        /// <summary>
        ///  A DR_DEVICE_IOCOMPLETION header. The CompletionId
        ///  field of this header MUST match a Device I/O Request
        ///  message that had the MajorFunction field set to IRP_MJ_WRITE.
        /// </summary>

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

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(this.Length);
            marshaler.WriteByte(this.Padding);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.Length = marshaler.ReadUInt32();
                this.Padding = marshaler.ReadByte();
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }

    }

    /// <summary>
    ///  A message with this header describes a response
    ///  to a Device Control Request.     
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_1_5_5.xml
    ///  </remarks>
    public class DR_CONTROL_RSP : DR_DEVICE_IOCOMPLETION
    {
        /// <summary>
        ///  A DR_DEVICE_IOCOMPLETION header. The CompletionId
        ///  field of this header MUST match a Device I/O Request
        ///  that has the MajorFunction field set to IRP_MJ_DEVICE_CONTROL.
        /// </summary>

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

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(this.OutputBufferLength);
            marshaler.WriteBytes(this.OutputBuffer);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.OutputBufferLength = marshaler.ReadUInt32();
                this.OutputBuffer = marshaler.ReadBytes((int)this.OutputBufferLength);
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
    }
    #endregion

    #region base PDUs
    public class RdpefsPDU : BasePDU
    {
        public RDPDR_HEADER Header;

        public RdpefsPDU() { this.Header = new RDPDR_HEADER(); }
        public RdpefsPDU(Component_Values component, PacketId_Values packetId)
        {
            this.Header = new RDPDR_HEADER(component, packetId);
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt16((ushort)Header.Component);
            marshaler.WriteUInt16((ushort)Header.PacketId);
        }

        /// <summary>
        /// Decode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to Decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                Header.Component = (Component_Values)marshaler.ReadUInt16();
                Header.PacketId = (PacketId_Values)marshaler.ReadUInt16();
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
    }
    #endregion 

    #region embeded packets

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
        public GENERAL_CAPS_SET(CAPABILITY_HEADER header) { this.Header = header; }
        public GENERAL_CAPS_SET() : base(CapabilityType_Values.CAP_GENERAL_TYPE){ }
        public GENERAL_CAPS_SET(CAPABILITY_VERSION ver)
            : base(CapabilityType_Values.CAP_GENERAL_TYPE, ver)
        {
            // A 16-bit unsigned integer. This field MUST be set to 1.
            this.protocolMajorVersion = protocolMajorVersion_Values.V1;
            // A 32-bit unsigned integer that is currently reserved for future use, and MUST be set to 0.
            this.ioCode2 = ioCode2_Values.V1;
            // A 32-bit unsigned integer that specifies extended flags. The extraFlags1 field MUST be set as a bitmask of the following value.
            this.extraFlags1 = extraFlags1_Values.ENABLE_ASYNCIO;
            // A 32-bit unsigned integer that is currently reserved for future use, and MUST be set to 0.
            this.extraFlags2 = extraFlags2_Values.V1;
            this.Header.CapabilityLength = 44;
        }

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
        public DR_CORE_SERVER_CLIENTID_CONFIRM_VersionMinor_Values protocolMinorVersion;

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

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32((uint)this.osType);
            marshaler.WriteUInt32((uint)this.osVersion);
            marshaler.WriteUInt16((ushort)this.protocolMajorVersion);
            marshaler.WriteUInt16((ushort)this.protocolMinorVersion);
            marshaler.WriteUInt32((uint)this.ioCode1);
            marshaler.WriteUInt32((uint)this.ioCode2);
            marshaler.WriteUInt32((uint)this.extendedPDU);
            marshaler.WriteUInt32((uint)this.extraFlags1);
            marshaler.WriteUInt32((uint)this.extraFlags2);
            marshaler.WriteUInt32(this.SpecialTypeDeviceCap);

        }

        public override void Decode(PduMarshaler marshaler)
        {
            try
            {
                this.osType = (osType_Values)marshaler.ReadUInt32();
                this.osVersion = (osVersion_Values)marshaler.ReadUInt32();
                this.protocolMajorVersion = (protocolMajorVersion_Values)marshaler.ReadUInt16();
                this.protocolMinorVersion = (DR_CORE_SERVER_CLIENTID_CONFIRM_VersionMinor_Values)marshaler.ReadUInt16();
                this.ioCode1 = (ioCode1_Values)marshaler.ReadUInt32();
                this.ioCode2 = (ioCode2_Values)marshaler.ReadUInt32();
                this.extendedPDU = (extendedPDU_Values)marshaler.ReadUInt32();
                this.extraFlags1 = (extraFlags1_Values)marshaler.ReadUInt32();
                this.extraFlags2 = (extraFlags2_Values)marshaler.ReadUInt32();
                this.SpecialTypeDeviceCap = marshaler.ReadUInt32();
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
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
        public PRINTER_CAPS_SET(CAPABILITY_HEADER header) { this.Header = header; }

        public PRINTER_CAPS_SET() : 
            base(CapabilityType_Values.CAP_PRINTER_TYPE, CAPABILITY_VERSION.V1) 
        {
            this.Header.CapabilityLength = 8;
        }
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
        public PORT_CAPS_SET(CAPABILITY_HEADER header) { this.Header = header; }

        public PORT_CAPS_SET() : 
            base(CapabilityType_Values.CAP_PORT_TYPE, CAPABILITY_VERSION.V1) 
        {
            this.Header.CapabilityLength = 8;
        }
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
        public DRIVE_CAPS_SET(CAPABILITY_HEADER header) { this.Header = header; }

        public DRIVE_CAPS_SET() : 
            base(CapabilityType_Values.CAP_DRIVE_TYPE, CAPABILITY_VERSION.V2) 
        {
            this.Header.CapabilityLength = 8;
        }

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
        public SMARTCARD_CAPS_SET(CAPABILITY_HEADER header) { this.Header = header; }

        public SMARTCARD_CAPS_SET() : 
            base(CapabilityType_Values.CAP_SMARTCARD_TYPE, CAPABILITY_VERSION.V1) 
        {
            this.Header.CapabilityLength = 8;
        }
    }

    #endregion 

    #region rdpefs Packets

    /// <summary>
    /// The server initiates the protocol with this message.                     
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_2.xml
    ///  </remarks>
    public class DR_CORE_SERVER_ANNOUNCE_REQ : RdpefsPDU
    {
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

        public DR_CORE_SERVER_ANNOUNCE_REQ()
            : base(Component_Values.RDPDR_CTYP_CORE, PacketId_Values.PAKID_CORE_SERVER_ANNOUNCE) 
        {
            this.VersionMajor = VersionMajor_Values.V1;
        }

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16((ushort)this.VersionMajor);
            marshaler.WriteUInt16((ushort)this.VersionMinor);
            marshaler.WriteUInt32(this.ClientId);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.VersionMajor = (VersionMajor_Values)marshaler.ReadUInt16();
                this.VersionMinor = (VersionMinor_Values)marshaler.ReadUInt16();
                this.ClientId = marshaler.ReadUInt32();
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
    }

    /// <summary>
    ///  The client replies to the Server Announce Request
    ///  message.                            
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_3.xml
    ///  </remarks>
    public class DR_CORE_CLIENT_ANNOUNCE_RSP : RdpefsPDU
    {
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

        public DR_CORE_CLIENT_ANNOUNCE_RSP() 
            : base(Component_Values.RDPDR_CTYP_CORE, PacketId_Values.PAKID_CORE_CLIENTID_CONFIRM) 
        {
            this.VersionMajor = DR_CORE_SERVER_ANNOUNCE_RSP_VersionMajor_Values.V1;
        }

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16((ushort)this.VersionMajor);
            marshaler.WriteUInt16((ushort)this.VersionMinor);
            marshaler.WriteUInt32(this.ClientId);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.VersionMajor = (DR_CORE_SERVER_ANNOUNCE_RSP_VersionMajor_Values)marshaler.ReadUInt16();
                this.VersionMinor = (DR_CORE_SERVER_ANNOUNCE_RSP_VersionMinor_Values)marshaler.ReadUInt16();
                this.ClientId = marshaler.ReadUInt32();
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
    }

    /// <summary>
    /// The client announces its machine name.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_4.xml
    ///  </remarks>
    public class DR_CORE_CLIENT_NAME_REQ : RdpefsPDU
    {
        /// <summary>
        ///  A common message header. The Component
        ///  field MUST be set to RDPDR_CTYP_CORE, and the PacketId
        ///  field MUST be set to PAKID_CORE_CLIENT_NAME.     
        /// </summary>
        //public RDPDR_HEADER Header;

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

        public DR_CORE_CLIENT_NAME_REQ() : base(Component_Values.RDPDR_CTYP_CORE, PacketId_Values.PAKID_CORE_CLIENT_NAME)
        {
            this.CodePage = 0;
        }

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32((uint)this.UnicodeFlag);
            marshaler.WriteUInt32(this.CodePage);
            marshaler.WriteUInt32(this.ComputerNameLen);
            marshaler.WriteBytes(this.ComputerName);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.UnicodeFlag = (UnicodeFlag_Values)(marshaler.ReadUInt32() & 0x1);
                this.CodePage = marshaler.ReadUInt32();
                this.ComputerNameLen = marshaler.ReadUInt32();
                this.ComputerName = marshaler.ReadBytes((int)this.ComputerNameLen);
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
    }

    /// <summary>
    ///  The server announces that it has successfully logged
    ///  on to the session.             
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_5.xml
    ///  </remarks>
    public class DR_CORE_USER_LOGGEDON : RdpefsPDU
    {
        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;

        public DR_CORE_USER_LOGGEDON() : base(Component_Values.RDPDR_CTYP_CORE, PacketId_Values.PAKID_CORE_USER_LOGGEDON) { }

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            return base.Decode(marshaler);
        }
    }

    /// <summary>
    ///  The server confirms the
    ///  client ID sent by the client in the Client Announce
    ///  Reply message.
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_6.xml
    ///  </remarks>
    public class DR_CORE_SERVER_CLIENTID_CONFIRM : RdpefsPDU
    {
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

        public DR_CORE_SERVER_CLIENTID_CONFIRM()
            : base(Component_Values.RDPDR_CTYP_CORE, PacketId_Values.PAKID_CORE_CLIENTID_CONFIRM) 
        {
            this.VersionMajor = DR_CORE_SERVER_CLIENTID_CONFIRM_VersionMajor_Values.V1;
        }

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16((ushort)this.VersionMajor);
            marshaler.WriteUInt16((ushort)this.VersionMinor);
            marshaler.WriteUInt32(this.ClientId);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.VersionMajor = (DR_CORE_SERVER_CLIENTID_CONFIRM_VersionMajor_Values)marshaler.ReadUInt16();
                this.VersionMinor = (DR_CORE_SERVER_CLIENTID_CONFIRM_VersionMinor_Values)marshaler.ReadUInt16();
                this.ClientId = marshaler.ReadUInt32();
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }

    }

    /// <summary>
    ///  The server announces its capabilities and requests
    ///  the same from the client.                         
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_7.xml
    ///  </remarks>
    public class DR_CORE_CAPABILITY_REQ : DR_CORE_CAPABILITY
    {
        ///// <summary>
        /////  A 16-bit integer that specifies the number of items
        /////  in the CapabilityMessage array.
        ///// </summary>
        //public ushort numCapabilities;

        ///// <summary>
        /////  A 16-bit unsigned integer of padding. This field is
        /////  unused, and MUST be ignored on receipt. 
        ///// </summary>
        //public ushort Padding;

        ///// <summary>
        /////  An array of capabilities. The number of capabilities
        /////  is specified by the numCapabilities field.
        ///// </summary>
        //public CAPABILITY_SET[] CapabilityMessage;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;

        public DR_CORE_CAPABILITY_REQ()
            : base(PacketId_Values.PAKID_CORE_SERVER_CAPABILITY) { }

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
        }
    }

    public class DR_CORE_CAPABILITY : RdpefsPDU
    {
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

        public DR_CORE_CAPABILITY(PacketId_Values packetId)
            :base(Component_Values.RDPDR_CTYP_CORE, packetId)
        {
            this.Padding = 0;
        }

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(this.numCapabilities);
            marshaler.WriteUInt16(this.Padding);
            if(this.numCapabilities > 0 && this.CapabilityMessage.Length > 0)
            {
                foreach(var message in CapabilityMessage)
                {
                    message.Encode(marshaler);
                }
            }
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.numCapabilities = marshaler.ReadUInt16();
                this.Padding = marshaler.ReadUInt16();
                this.CapabilityMessage = new CAPABILITY_SET[numCapabilities];
                for (int i = 0; i < numCapabilities; i++ )
                {
                    CAPABILITY_HEADER header = new CAPABILITY_HEADER();
                    bool fsuccess = header.Decode(marshaler);
                    if(fsuccess)
                    {
                        switch (header.CapabilityType)
                        {
                            case CapabilityType_Values.CAP_GENERAL_TYPE:
                                CapabilityMessage[i] = new GENERAL_CAPS_SET(header);
                                CapabilityMessage[i].Decode(marshaler);
                                break;
                            case CapabilityType_Values.CAP_PRINTER_TYPE:
                                CapabilityMessage[i] = new PRINTER_CAPS_SET(header);
                                break;
                            case CapabilityType_Values.CAP_PORT_TYPE:
                                CapabilityMessage[i] = new PORT_CAPS_SET(header);
                                break;
                            case CapabilityType_Values.CAP_DRIVE_TYPE:
                                CapabilityMessage[i] = new DRIVE_CAPS_SET(header);
                                break;
                            case CapabilityType_Values.CAP_SMARTCARD_TYPE:
                                CapabilityMessage[i] = new SMARTCARD_CAPS_SET(header);
                                break;
                        }
                    }
                    
                }
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
            
        }
    }

    /// <summary>
    ///  The server announces its capabilities and requests
    ///  the same from the client.      
    /// </summary>
    //  <remarks>
    //   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_7.xml
    //  </remarks>
    public class DR_CORE_CAPABILITY_RSP : DR_CORE_CAPABILITY
    {
        //public RDPDR_HEADER Header;

        ///// <summary>
        /////  A 16-bit integer that specifies the number of items
        /////  in the CapabilityMessage array.
        ///// </summary>
        //public ushort numCapabilities;

        ///// <summary>
        /////  A 16-bit unsigned integer of padding. This field is
        /////  unused, and MUST be ignored on receipt. 
        ///// </summary>
        //public ushort Padding;

        ///// <summary>
        /////  An array of capabilities. The number of capabilities
        /////  is specified by the numCapabilities field.
        ///// </summary>
        //public CAPABILITY_SET[] CapabilityMessage;

        /// <summary>
        /// The MS-RDPEFS protocol runs over a static 
        /// virtual channel with the name RDPDR
        /// </summary>
        public ushort RDPDRChannelId;

        public DR_CORE_CAPABILITY_RSP() : base(PacketId_Values.PAKID_CORE_CLIENT_CAPABILITY) { }

        public override bool Decode(PduMarshaler marshaler)
        {
            return base.Decode(marshaler);
        }
    }

    /// <summary>
    ///  The client announces the list of devices to redirect
    ///  on the server.                       
    /// </summary>
    ///  <remarks>
    ///   file:///D:/programs/RFSPAC/XML-RDPEFS/_rfc_ms-rdpefs2_2_2_9.xml
    /// </remarks>
    public class DR_CORE_DEVICELIST_ANNOUNCE_REQ : RdpefsPDU
    {
        ///// <summary>
        /////  A common message header. The Component
        /////  field MUST be set to RDPDR_CTYP_CORE, and the PacketId
        /////  field MUST be set to PAKID_CORE_DEVICELIST_ANNOUNCE.
        ///// </summary>
        //public RDPDR_HEADER Header;

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

        public DR_CORE_DEVICELIST_ANNOUNCE_REQ() : base(Component_Values.RDPDR_CTYP_CORE, PacketId_Values.PAKID_CORE_DEVICELIST_ANNOUNCE) { }

        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(this.DeviceCount);
            foreach(var device in DeviceList)
            {
                device.Encode(marshaler);
            }
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.DeviceCount = marshaler.ReadUInt32();
                if(this.DeviceCount > 0)
                {
                    this.DeviceList = new DEVICE_ANNOUNCE[this.DeviceCount];
                    for(int i=0; i<this.DeviceCount; i++)
                    {
                        DeviceList[i] = new DEVICE_ANNOUNCE();
                        DeviceList[i].Decode(marshaler);
                    }
                }
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
    }

    public class RdpefsUnknownPdu : RdpefsPDU
    {
        byte[] Data;

        public override void Encode(PduMarshaler marshaler)
        {
            if (Data != null)
            {
                marshaler.WriteBytes(Data);
            };
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            Data = marshaler.ReadToEnd();
            return Data != null;
        }

    }
    #endregion
}

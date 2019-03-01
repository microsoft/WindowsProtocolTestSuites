// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx
{
    #region Common Structures

    /// <summary>
    /// This structure is meant to be a header on all other messages and MUST NOT be sent alone.
    /// </summary>
    public struct RDPGFX_HEADER
    {
        /// <summary>
        /// UINT16. The value of this integer indicates type of the graphics command PDU.
        /// </summary>
        public PacketTypeValues cmdId;

        /// <summary>
        /// UINT16 The value of this integer indicates graphics command flags, must be set to zero.
        /// </summary>
        public ushort flags;

        /// <summary>
        /// UINT32 The value of this integer indicates the length of the graphics command PDU, in bytes.
        /// </summary>
        public uint pduLength;
    }

    /// <summary>
    /// This structure specifies the layout of capability set.
    /// </summary>
    public struct RDPGFX_CAPSET
    {
        /// <summary>
        /// UINT32. The value of this integer indicates the version of the capability set.
        /// </summary>
        public CapsVersions version;
        /// <summary>
        /// UINT32. The value of this integer indicates the size, in bytes, of the capability set data 
        /// present in the capsData field.
        /// </summary>
        public uint capsDataLength;
        /// <summary>
        /// Variable-length array, indicates the data specific to the capability set.
        /// </summary>
        public byte[] capsData;
    }

    /// <summary>
    /// This structure specifies a point relative to the virtual-desktop origin.
    /// </summary>
    public struct RDPGFX_POINT16
    {
        /// <summary>
        /// A 16-bit unsigned integer, specifies  x-coordinate of the point.
        /// </summary>
        public ushort x;
        /// <summary>
        /// A 16-bit unsigned integer, specifies  y-coordinate of the point.
        /// </summary>
        public ushort y;

        public RDPGFX_POINT16(ushort xPos, ushort yPos)
        {
            x = xPos;
            y = yPos;
        }
    }

    /// <summary>
    /// This structure specifies a rectangle relative to the virtual-desktop origin.
    /// Section 2.2.4.4 Note that the width and height of the MPEG-4 AVC/H.264 codec bitstream MUST be aligned to a multiple of 16     
    /// </summary>
    public struct RDPGFX_RECT16
    {

        /// <summary>
        /// A 16-bit unsigned integer, specifies  the leftmost bound of the rectangle.
        /// </summary>
        public ushort left;
        /// <summary>
        /// A 16-bit unsigned integer, specifies  the upper  bound of the rectangle.
        /// </summary>
        public ushort top;
        /// <summary>
        /// A 16-bit unsigned integer, specifies  the rightmost  bound of the rectangle.
        /// </summary>
        public ushort right;
        /// <summary>
        /// A 16-bit unsigned integer, specifies  the lower  bound of the rectangle.
        /// </summary>
        public ushort bottom;

        public RDPGFX_RECT16(ushort l, ushort t, ushort r, ushort b)
        {
            left = l;
            top = t;
            right = r;
            bottom = b;
        }

        public ushort Width
        {
            get
            {
                return (ushort)(right - left);
            }
        }

        public ushort Height
        {
            get
            {
                return (ushort)(bottom - top);
            }
        }
    }

    /// <summary>
    /// This structure specifies a 32 bpp ARGB or XRGB color value
    /// </summary>
    public struct RDPGFX_COLOR32
    {
        /// <summary>
        /// A 8-bit unsigned integer, specifies the blue ARGB or XRGB color component.
        /// </summary>
        public byte B;
        /// <summary>
        /// A 8-bit unsigned integer, specifies the green ARGB or XRGB color component.
        /// </summary>
        public byte G;
        /// <summary>
        /// A 8-bit unsigned integer, specifies the red ARGB or XRGB color component.
        /// </summary>
        public byte R;
        /// <summary>
        /// A 8-bit unsigned integer, that in the case of ARGB specifies the alpha color component or in the case of XRGB MUST be ignored.
        /// </summary>
        public byte XA;

        public RDPGFX_COLOR32(byte blue, byte green, byte red, byte xa)
        {
            B = blue;
            G = green;
            R = red;
            XA = xa;
        }
    }

    /// <summary>
    /// This structure specifies attributes of a bitmap cache entry stored on the client
    /// </summary>
    public struct RDPGFX_CACHE_ENTRY_METADATA
    {
        /// <summary>
        /// A 64-bit unsigned integer, specifies a unique key associated with the bitmap cache entry.
        /// </summary>
        public ulong cacheKey;
        /// <summary>
        /// A 32-bit unsigned integer, specifies the size of the bitmap cache entry, in bytes.
        /// </summary>
        public uint bitmapLength;
    }

    #endregion

    #region Base PDUs

    /// <summary>
    /// The base pdu of all MS-RDPEGFX messages.
    /// </summary>
    public class RdpegfxPdu : BasePDU
    {
        #region Message Fields
        public RDPGFX_HEADER Header;    // RDPEGFX command common header
        public uint pduLen;             // RDPEGFX command actual PDU length in bytes after successfully decode

        #endregion 

        #region Methods
        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt16((ushort)Header.cmdId);
            marshaler.WriteUInt16(Header.flags);
            marshaler.WriteUInt32(Header.pduLength);
        }

        /// <summary>
        /// Decode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to Decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                Header.cmdId = (PacketTypeValues)marshaler.ReadUInt16();
                Header.flags = marshaler.ReadUInt16();
                Header.pduLength = marshaler.ReadUInt32();
                pduLen = 8;   // 8 bytes common header.
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }

        #endregion
    }

    /// <summary>
    /// The base pdu of all MS-RDPEGFX messages sent by server.
    /// </summary>
    public class RdpegfxServerPdu : RdpegfxPdu
    {
        #region Methods
        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            bool fDecoded = base.Decode(marshaler);
            if (fDecoded)
            {
                return (this.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_WIRETOSURFACE_1 ||
                        this.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_WIRETOSURFACE_2 ||
                        this.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_DELETEENCODINGCONTEXT ||
                        this.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_SOLIDFILL ||
                        this.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_SURFACETOSURFACE ||

                        this.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_SURFACETOCACHE ||
                        this.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_CACHETOSURFACE ||
                        this.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_EVICTCACHEENTRY ||
                        this.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_CREATESURFACE ||
                        this.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_DELETESURFACE ||

                        this.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_STARTFRAME ||
                        this.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_ENDFRAME ||
                        this.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_RESETGRAPHICS ||
                        this.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_MAPSURFACETOOUTPUT ||
                        this.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_CACHEIMPORTREPLY ||

                        this.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_CAPSCONFIRM
                        );
            }
            else
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion 
    }

    /// <summary>
    ///  The base pdu of all MS-RDPEGFX messages sent by the client.
    /// </summary>
    public class RdpegfxClientPdu : RdpegfxPdu
    {
        #region Methods
        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            bool fDecoded = base.Decode(marshaler);
            if (fDecoded)
            {
                return (this.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_FRAMEACKNOWLEDGE ||
                        this.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_CACHEIMPORTOFFER ||
                        this.Header.cmdId == PacketTypeValues.RDPGFX_CMDID_CAPSADVERTISE);
            }
            else
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }

    /// <summary>
    /// The unknown type.
    /// </summary>
    public class RdpegfxUnkownPdu : RdpegfxPdu
    {
        public byte[] Data;

        #region Methods
        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            if (Data != null)
            {
                marshaler.WriteBytes(Data);
            };
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            Data = marshaler.ReadToEnd();
            return Data != null;
        }
        #endregion

    }

    #endregion

    #region Server Messages

    /// <summary>
    /// The RDPGFX_START_FRAME message is sent from server to specify the start of a logical frame.
    /// </summary>
    public class RDPGFX_START_FRAME : RdpegfxServerPdu
    {
        #region Message Fields
        /// <summary>
        ///  A 32-bit unsigned integer. a time stamp assigned to the frame.
        /// </summary>
        public uint timeStamp;

        /// <summary>
        ///  A 32-bit unsigned integer. frame ID of the frame.
        /// </summary>
        public uint frameId;
        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_START_FRAME()
        {
        }

        /// <summary>
        /// Constructor, create start frame message.
        /// </summary>
        /// <param name="fid">This is used to indicate frame id.</param>
        public RDPGFX_START_FRAME(uint fid)
        {
            this.Header.cmdId = PacketTypeValues.RDPGFX_CMDID_STARTFRAME;
            this.Header.flags = 0x0;
            this.Header.pduLength = (uint)Marshal.SizeOf(Header) + 8;
            this.timeStamp = 0;
            this.frameId = fid;

        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(this.timeStamp);
            marshaler.WriteUInt32(this.frameId);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.timeStamp = marshaler.ReadUInt32();
                pduLen += 4;
                this.frameId = marshaler.ReadUInt32();
                pduLen += 4;

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }

    /// <summary>
    /// The RDPGFX_END_FRAME message is sent from server to specify the end of a logical frame.
    /// </summary>
    public class RDPGFX_END_FRAME : RdpegfxServerPdu
    {
        #region Message Fields
        /// <summary>
        ///  A 32-bit unsigned integer. frame ID of the frame
        /// </summary>
        public uint frameId;
        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_END_FRAME()
        {
        }

        /// <summary>
        /// constructor, create end frame message 
        /// </summary>
        /// <param name="fid">This is used to indicate frame id.</param>
        public RDPGFX_END_FRAME(uint fid)
        {
            this.Header.cmdId = PacketTypeValues.RDPGFX_CMDID_ENDFRAME;
            this.Header.flags = 0x0;
            this.Header.pduLength = (uint)Marshal.SizeOf(Header) + 4;
            this.frameId = fid;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(this.frameId);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.frameId = marshaler.ReadUInt32();
                pduLen += 4;

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }

    /// <summary>
    /// The RDPGFX_CAPS_CONFIRM message is sent from the server to the client to confirm capabilities for the graphics DVC.
    /// </summary>
    public class RDPGFX_CAPS_CONFIRM : RdpegfxServerPdu
    {
        #region Message Fields

        /// <summary>
        /// Variable-length. Specify the capability set selected by the server from the 
        /// RDPGFX_CAPS_ADVERTISE message sent by the client.  
        /// </summary>
        public RDPGFX_CAPSET capsSet;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_CAPS_CONFIRM()
        {
        }

        /// <summary>
        /// Constructor, create a  version 8 capability confirm message.
        /// </summary>
        /// <param name="v8flag"> this is used to specify the flag of capability.</param>
        /// <param name="version">version of the capability</param>
        public RDPGFX_CAPS_CONFIRM(CapsFlags v8flag, CapsVersions version)
        {
            Header.cmdId = PacketTypeValues.RDPGFX_CMDID_CAPSCONFIRM;
            Header.flags = 0x0;

            capsSet.version = version;
            capsSet.capsDataLength = 0x04;
            capsSet.capsData = new byte[capsSet.capsDataLength];
            // Assign flag into the capsData structure(byte[]).
            capsSet.capsData = BitConverter.GetBytes((uint)v8flag);

            Header.pduLength = (uint)Marshal.SizeOf(Header) + 8 + capsSet.capsDataLength;

        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32((uint)this.capsSet.version);
            marshaler.WriteUInt32(this.capsSet.capsDataLength);
            for (int i = 0; i < this.capsSet.capsDataLength; i++)
            {
                marshaler.WriteByte(this.capsSet.capsData[i]);
            }
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.capsSet.version = (CapsVersions)marshaler.ReadUInt32();
                pduLen += 4;
                this.capsSet.capsDataLength = marshaler.ReadUInt32();
                pduLen += 4;

                this.capsSet.capsData = marshaler.ReadBytes((int)this.capsSet.capsDataLength);
                pduLen += this.capsSet.capsDataLength;

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }

    /// <summary>
    /// The RDPGFX_RESET_GRAPHICS message is sent from the server to reset size of virtual desktop on client.
    /// </summary>
    public class RDPGFX_RESET_GRAPHICS : RdpegfxServerPdu
    {
        #region Message Fields

        /// <summary>
        /// A 32-bit, unsigned integer. specify new width of the virtual desktop.  
        /// </summary>
        public uint width;
        /// <summary>
        /// A 32-bit, unsigned integer. specify new height of the virtual desktop.
        /// </summary>
        public uint height;

        /// <summary>
        ///  A 32-bit, unsigned integer. The number of display monitor
        ///  definitions in the monitorDefArray field (the maximum allowed is 10).
        /// </summary>
        public uint monitorCount;

        /// <summary>
        ///  A variable-length array containing a series of TS_MONITOR_DEF 
        ///  structures (section 2.2.1.3.6.1) which describe the display monitor 
        ///  layout of the client. The number of TS_MONITOR_DEF structures is 
        ///  given by the monitorCount field.
        /// </summary>
        public Collection<TS_MONITOR_DEF> monitorDefArray;

        /// <summary>
        /// A variable-length byte array that is used for padding
        /// </summary>
        public byte[] pad;
        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_RESET_GRAPHICS()
        {
        }

        /// <summary>
        /// Constructor, create a default reset graphic message with one monitor only.
        /// </summary>
        /// <param name="w">This is used to indicate width of virtual desktop.</param>
        /// <param name="h">This is used to indicate height of virtual desktop.</param>
        /// <param name="monitorCount">This is used to indicate total monitor count</param>
        public RDPGFX_RESET_GRAPHICS(uint w, uint h, uint monitorCount)
        {
            Header.cmdId = PacketTypeValues.RDPGFX_CMDID_RESETGRAPHICS;
            Header.flags = 0x0;
            //according to TD, this message MUST be 340 bytes in size
            Header.pduLength = 340;
            this.width = w;
            this.height = h;
            this.monitorCount = monitorCount;

            TS_MONITOR_DEF monitorDef;
            // If monitor size is less than virtual desktop, rdpegfx test cases may fail
            // set monitor width and height to 65535 to avoid impact to any rdpegfx test cases.
            monitorDef.left = 0;
            monitorDef.top = 0;
            monitorDef.right = 0xffff;
            monitorDef.bottom = 0xffff;
            monitorDef.flags = Flags_TS_MONITOR_DEF.TS_MONITOR_PRIMARY;
            this.monitorDefArray = new Collection<TS_MONITOR_DEF>();
            for (uint i = 0; i < this.monitorCount; i++)
            {
                this.monitorDefArray.Add(monitorDef);
            }

            uint currentLength = (uint)Marshal.SizeOf(Header) + 12 + (uint)(Marshal.SizeOf(monitorDef) * this.monitorCount);
            if (currentLength < Header.pduLength)
            {
                this.pad = new byte[Header.pduLength - currentLength];
            }

        }
        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(this.width);
            marshaler.WriteUInt32(this.height);
            marshaler.WriteUInt32(this.monitorCount);
            foreach (TS_MONITOR_DEF monitorDef in monitorDefArray)
            {
                marshaler.WriteUInt32(monitorDef.left);
                marshaler.WriteUInt32(monitorDef.top);
                marshaler.WriteUInt32(monitorDef.right);
                marshaler.WriteUInt32(monitorDef.bottom);
                marshaler.WriteUInt32((uint)monitorDef.flags);
            }
            if (pad != null)
            {
                marshaler.WriteBytes(pad);
            }
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.width = marshaler.ReadUInt32();
                pduLen += 4;
                this.height = marshaler.ReadUInt32();
                pduLen += 4;
                this.monitorCount = marshaler.ReadUInt32();
                pduLen += 4;

                for (int i = 0; i < this.monitorCount; i++)
                {
                    TS_MONITOR_DEF monitorDef;
                    monitorDef.left = marshaler.ReadUInt32();
                    monitorDef.top = marshaler.ReadUInt32();
                    monitorDef.right = marshaler.ReadUInt32();
                    monitorDef.bottom = marshaler.ReadUInt32();
                    monitorDef.flags = (Flags_TS_MONITOR_DEF)marshaler.ReadUInt32();

                    monitorDefArray.Add(monitorDef);
                    pduLen += (uint)Marshal.SizeOf(monitorDef);
                }
                if (pduLen < Header.pduLength)
                {
                    this.pad = marshaler.ReadBytes((int)(Header.pduLength - pduLen));
                    pduLen = Header.pduLength;
                }
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }

    /// <summary>
    /// The RDPGFX_CREATE_SURFACE message is sent from server to instruct client to create a surface.
    /// </summary>
    public class RDPGFX_CREATE_SURFACE : RdpegfxServerPdu
    {
        #region Message Fields
        /// <summary>
        /// A 16-bit, unsigned integer. specify the ID of the surface to be created.   
        /// </summary>
        public ushort surfaceId;

        /// <summary>
        /// A 16-bit, unsigned integer. specify new width of the surface to be created.   
        /// </summary>
        public ushort width;

        /// <summary>
        /// A 16-bit, unsigned integer. specify new height of the surface to be created.   
        /// </summary>
        public ushort height;

        /// <summary>
        /// A 8-bit, unsigned integer. specify the pixel format of the surface to be created.
        /// </summary>
        public PixelFormat pixFormat;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_CREATE_SURFACE()
        {
        }

        /// <summary>
        /// constructor, create a CreatSurface message
        /// </summary>
        /// <param name="sid">This is used to indicate surface id.</param>
        /// <param name="w">This is used to indicate width of surface.</param>
        /// <param name="h">This is used to indicate height of surface.</param>
        /// <param name="pix">This is used to indicate pixel format of surface.</param>
        public RDPGFX_CREATE_SURFACE(ushort sid, ushort w, ushort h, PixelFormat pix)
        {
            this.Header.cmdId = PacketTypeValues.RDPGFX_CMDID_CREATESURFACE;
            this.Header.flags = 0x0;
            this.Header.pduLength = (uint)Marshal.SizeOf(Header) + 7;

            this.surfaceId = sid;
            this.width = w;
            this.height = h;
            this.pixFormat = pix;
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(this.surfaceId);
            marshaler.WriteUInt16(this.width);
            marshaler.WriteUInt16(this.height);
            marshaler.WriteByte((byte)this.pixFormat);
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.surfaceId = marshaler.ReadUInt16();
                pduLen += 2;
                this.width = marshaler.ReadUInt16();
                pduLen += 2;
                this.height = marshaler.ReadUInt16();
                pduLen += 2;
                this.pixFormat = (PixelFormat)(marshaler.ReadByte());
                pduLen++;
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }

    /// <summary>
    /// The RDPGFX_DELETE_SURFACE message is sent from server to instruct client to delete a surface.
    /// </summary>
    public class RDPGFX_DELETE_SURFACE : RdpegfxServerPdu
    {
        #region Message Fields
        /// <summary>
        /// A 16-bit, unsigned integer. specify the ID of the surface to be created.   
        /// </summary>
        public ushort surfaceId;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_DELETE_SURFACE()
        {
        }

        /// <summary>
        /// Constructor, create a delete surface message.
        /// </summary>
        /// <param name="sid">This is used to indicate surface id.</param>
        public RDPGFX_DELETE_SURFACE(ushort sid)
        {
            this.Header.cmdId = PacketTypeValues.RDPGFX_CMDID_DELETESURFACE;
            this.Header.flags = 0x0;
            this.Header.pduLength = (uint)Marshal.SizeOf(Header) + 2;
            this.surfaceId = sid;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(this.surfaceId);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.surfaceId = marshaler.ReadUInt16();
                pduLen += 2;

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }

        #endregion
    }

    /// <summary>
    /// The RDPGFX_MAPSURFACE_TO_OUTPUT message is to instruct client to map surface to a rectangle area of the graphics output buffer
    /// </summary>
    public class RDPGFX_MAPSURFACE_TO_OUTPUT : RdpegfxServerPdu
    {
        #region Message Fields
        /// <summary>
        /// A 16-bit, unsigned integer. specify the ID of the surface to be output.   
        /// </summary>
        public ushort surfaceId;

        /// <summary>
        /// A 16-bit, unsigned integer. must be set to zero.   
        /// </summary>
        public ushort reserved;

        /// <summary>
        ///  A 32-bit unsigned integer. the x-coordinate of the upper-left corner of the surface.
        /// </summary>
        public uint outputOriginX;

        /// <summary>
        ///  A 32-bit unsigned integer. the y-coordinate of the upper-left corner of the surface.
        /// </summary>
        public uint outputOriginY;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_MAPSURFACE_TO_OUTPUT()
        {
        }

        /// <summary>
        /// Constructor, create a map surface to output message.
        /// </summary>
        /// <param name="sid">This is used to indicate surface id.</param>
        /// <param name="x">This is used to indicate x-coordinate of the upper-left corner of the surface.</param>
        /// <param name="y">This is used to indicate y-coordinate of the upper-left corner of the surface.</param>
        public RDPGFX_MAPSURFACE_TO_OUTPUT(ushort sid, uint x, uint y)
        {
            this.Header.cmdId = PacketTypeValues.RDPGFX_CMDID_MAPSURFACETOOUTPUT;
            this.Header.flags = 0x0;
            this.Header.pduLength = (uint)Marshal.SizeOf(Header) + 12;
            this.surfaceId = sid;
            this.reserved = 0;
            this.outputOriginX = x;
            this.outputOriginY = y;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(this.surfaceId);
            marshaler.WriteUInt16(this.reserved);
            marshaler.WriteUInt32(this.outputOriginX);
            marshaler.WriteUInt32(this.outputOriginY);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.surfaceId = marshaler.ReadUInt16();
                pduLen += 2;
                this.reserved = marshaler.ReadUInt16();
                pduLen += 2;
                this.outputOriginX = marshaler.ReadUInt32();
                pduLen += 4;
                this.outputOriginY = marshaler.ReadUInt32();
                pduLen += 4;

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }

    /// <summary>
    /// The RDPGFX_MAP_SURFACE_TO_WINDOW_PDU message is sent by the server to instruct the client to map a surface to a RAIL window on the client.
    /// </summary>
    public class RDPGFX_MAPSURFACE_TO_WINDOW : RdpegfxServerPdu
    {
        #region Message Fields
        /// <summary>
        /// A 16-bit unsigned integer that specifies the ID of the surface to be associated with the surface-to-window mapping.
        /// </summary>
        public ushort surfaceId;

        /// <summary>
        ///  A 64-bit unsigned integer that specifies the ID of the RAIL window to be associated with the surface-to-window mapping
        /// </summary>
        public ulong windowId;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the width of the rectangular region on the surface to which the window is mapped.
        /// </summary>
        public uint mappedWidth;

        /// <summary>
        ///  A 32-bit unsigned integer that specifies the height of the rectangular region on the surface to which the window is mapped.
        /// </summary>
        public uint mappedHeight;
        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_MAPSURFACE_TO_WINDOW()
        {
        }

        /// <summary>
        /// Constructor, create a map surface to window message.
        /// </summary>
        /// <param name="sid">This is used to indicate surface id.</param>
        /// <param name="wid">Window ID to be mapped.</param>
        /// <param name="w">The width of the rectangular region on the surface to which the window is mapped.</param>
        /// <param name="h">The height of the rectangular region on the surface to which the window is mapped.</param>

        public RDPGFX_MAPSURFACE_TO_WINDOW(ushort sid, ulong wid, uint w, uint h)
        {
            this.Header.cmdId = PacketTypeValues.RDPGFX_CMDID_MAPSURFACETOWINDOW;
            this.Header.flags = 0x0;
            this.Header.pduLength = (uint)Marshal.SizeOf(Header) + (uint) Marshal.SizeOf(windowId) + (uint) Marshal.SizeOf(mappedWidth) + (uint) Marshal.SizeOf(mappedHeight);
            this.surfaceId = sid;
            this.windowId = wid;
            this.mappedWidth = w;
            this.mappedHeight = h;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(this.surfaceId);
            marshaler.WriteUInt64(this.windowId);
            marshaler.WriteUInt32(this.mappedWidth);
            marshaler.WriteUInt32(this.mappedHeight);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.surfaceId = marshaler.ReadUInt16();
                pduLen += 2;
                this.windowId = marshaler.ReadUInt64();
                pduLen += 8;
                this.mappedWidth = marshaler.ReadUInt32();
                pduLen += 4;
                this.mappedHeight = marshaler.ReadUInt32();
                pduLen += 4;

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }

    
    /// <summary>
    /// The RDPGFX_SOLIDFILL message is to instruct the client to fill a collection of rectangles on a destination surface with a solid color.
    /// </summary>
    public class RDPGFX_SOLIDFILL : RdpegfxServerPdu
    {
        #region Message Fields
        /// <summary>
        /// A 16-bit, unsigned integer. specify the ID of the surface to be created.   
        /// </summary>
        public ushort surfaceId;
        /// <summary>
        /// A RDPGFX_COLOR32 structure. specify the color that MUST be used to fill the destination rectangles specified in the fillRects field.  
        /// </summary>
        public RDPGFX_COLOR32 fillPixel;

        /// <summary>
        /// A 16-bit, unsigned integer. specify the number of RDPGFX_RECT16 structures in the fillRects field.   
        /// </summary>
        public ushort fillRectCount;
        /// <summary>
        /// A variable-length array. specify rectangles on the destination surface to be filled.   
        /// </summary>
        public List<RDPGFX_RECT16> fillRectList;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_SOLIDFILL()
        {
        }

        /// <summary>
        /// Constructor, create a RDPGFX_SOLIDFILL message with one rectangle to fill default.
        /// </summary>
        /// <param name="sid">This is used to indicate surface id to be filled.</param>
        /// <param name="pixel">This is used to indicate color to fill.</param>
        public RDPGFX_SOLIDFILL(ushort sid, RDPGFX_COLOR32 pixel)
        {
            this.Header.cmdId = PacketTypeValues.RDPGFX_CMDID_SOLIDFILL;
            this.Header.flags = 0x0;
            this.Header.pduLength = (uint)Marshal.SizeOf(Header) + 8;

            this.surfaceId = sid;
            this.fillPixel = pixel;
            this.fillRectCount = 0;
            this.fillRectList = new List<RDPGFX_RECT16>();
            this.fillRectList.Clear();
        }

        /// <summary>
        /// Add more rectangle to be filled in destination surface.
        /// </summary>
        /// <param name="rect">This is to specify a rectangle area of surface.</param>
        public void addFillRect(RDPGFX_RECT16 rect)
        {
            this.fillRectList.Add(rect);
            this.fillRectCount++;
            this.Header.pduLength += (uint)Marshal.SizeOf(rect);
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);

            marshaler.WriteUInt16(this.surfaceId);
            marshaler.WriteByte(this.fillPixel.B);
            marshaler.WriteByte(this.fillPixel.G);
            marshaler.WriteByte(this.fillPixel.R);
            marshaler.WriteByte(this.fillPixel.XA);
            marshaler.WriteUInt16(this.fillRectCount);

            foreach (RDPGFX_RECT16 rect in fillRectList)
            {
                marshaler.WriteUInt16(rect.left);
                marshaler.WriteUInt16(rect.top);
                marshaler.WriteUInt16(rect.right);
                marshaler.WriteUInt16(rect.bottom);
            }
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.surfaceId = marshaler.ReadUInt16();
                pduLen += 2;
                this.fillPixel.R = marshaler.ReadByte();
                this.fillPixel.G = marshaler.ReadByte();
                this.fillPixel.B = marshaler.ReadByte();
                this.fillPixel.XA = marshaler.ReadByte();
                pduLen += 4;
                this.fillRectCount = marshaler.ReadUInt16();
                pduLen += 2;

                for (int i = 0; i < this.fillRectCount; i++)
                {
                    RDPGFX_RECT16 rect;
                    rect.left = marshaler.ReadUInt16();
                    rect.top = marshaler.ReadUInt16();
                    rect.right = marshaler.ReadUInt16();
                    rect.bottom = marshaler.ReadUInt16();

                    fillRectList.Add(rect);
                    pduLen += (uint)Marshal.SizeOf(rect);
                }

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }

    /// <summary>
    /// The RDPGFX_SURFACE_TO_SURFACE message is to instruct client to copy bitmap from source surface to destination surface
    /// or same surface.
    /// </summary>
    public class RDPGFX_SURFACE_TO_SURFACE : RdpegfxServerPdu
    {
        #region Message Fields
        /// <summary>
        /// A 16-bit, unsigned integer. specify the ID of the source surface.   
        /// </summary>
        public ushort surfaceIdSrc;

        /// <summary>
        /// A 16-bit, unsigned integer. specify the ID of the destination surface.   
        /// </summary>
        public ushort surfaceIdDest;

        /// <summary>
        /// A RDPGFX_RECT16 structure. specify the rectangle that bounds the source bitmap.   
        /// </summary>
        public RDPGFX_RECT16 rectSrc;

        /// <summary>
        /// A 16-bit unsigned integer that specifies the number of RDPGFX_POINT16 structures in the destPts field.   
        /// </summary>
        public ushort destPtsCount;

        /// <summary>
        /// A RDPGFX_POINT16 array. specifies target points on the destination surface to which to copy the source bitmap.    
        /// </summary>
        public List<RDPGFX_POINT16> destPtsList;

        #endregion

        #region Methods


        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_SURFACE_TO_SURFACE()
        {
        }

        /// <summary>
        /// Constructor, create a surface to surface message, with only 1 destination copy position.
        /// </summary>
        /// <param name="srcSID">This is used to indicate source surface id.</param>
        /// <param name="destSID">This is used to indicate destination surface id.</param>
        /// <param name="srcRect">This is used to indicate source rectangle bitmap area to be copied.</param>
        public RDPGFX_SURFACE_TO_SURFACE(ushort srcSID, ushort destSID, RDPGFX_RECT16 srcRect)
        {
            this.Header.cmdId = PacketTypeValues.RDPGFX_CMDID_SURFACETOSURFACE;
            this.Header.flags = 0x0;
            this.Header.pduLength = (uint)Marshal.SizeOf(Header) + 14;

            this.surfaceIdSrc = srcSID;
            this.surfaceIdDest = destSID;
            this.rectSrc = srcRect;
            this.destPtsCount = 0;
            this.destPtsList = new List<RDPGFX_POINT16>();
            this.destPtsList.Clear();
        }

        /// <summary>
        /// Add more position in destination surface to be copied.
        /// </summary>
        /// <param name="destPosition">This is used to specify a destination position of source rectangle bitmap to be copied.</param>
        public void AddDestPosition(RDPGFX_POINT16 destPosition)
        {
            this.destPtsList.Add(destPosition);
            this.destPtsCount++;
            this.Header.pduLength += (uint)Marshal.SizeOf(destPosition);
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(this.surfaceIdSrc);
            marshaler.WriteUInt16(this.surfaceIdDest);
            marshaler.WriteUInt16(this.rectSrc.left);
            marshaler.WriteUInt16(this.rectSrc.top);
            marshaler.WriteUInt16(this.rectSrc.right);
            marshaler.WriteUInt16(this.rectSrc.bottom);
            marshaler.WriteUInt16(this.destPtsCount);

            foreach (RDPGFX_POINT16 destPoint in destPtsList)
            {
                marshaler.WriteUInt16(destPoint.x);
                marshaler.WriteUInt16(destPoint.y);
            }
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.surfaceIdSrc = marshaler.ReadUInt16();
                pduLen += 2;
                this.surfaceIdDest = marshaler.ReadUInt16();
                pduLen += 2;
                this.rectSrc.left = marshaler.ReadUInt16();
                pduLen += 2;
                this.rectSrc.top = marshaler.ReadUInt16();
                pduLen += 2;
                this.rectSrc.right = marshaler.ReadUInt16();
                pduLen += 2;
                this.rectSrc.bottom = marshaler.ReadUInt16();
                pduLen += 2;
                this.destPtsCount = marshaler.ReadUInt16();
                pduLen += 2;

                for (ushort i = 0; i < this.destPtsCount; i++)
                {
                    RDPGFX_POINT16 destPoint;
                    destPoint.x = marshaler.ReadUInt16();
                    destPoint.y = marshaler.ReadUInt16();
                    destPtsList.Add(destPoint);
                    pduLen += (uint)Marshal.SizeOf(destPoint);
                }

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }

    /// <summary>
    /// The RDPGFX_SURFACE_TO_CACHE message is to instruct client to copy bitmap data from a source surface to the bitmap cache
    /// </summary>
    public class RDPGFX_SURFACE_TO_CACHE : RdpegfxServerPdu
    {
        #region Message Fields
        /// <summary>
        /// A 16-bit, unsigned integer. specify the ID of the surface which contains source bitmap.   
        /// </summary>
        public ushort surfaceId;
        /// <summary>
        /// A 64-bit, unsigned integer. specify the key to associate with the bitmap cache entry that will store the bitmap.   
        /// </summary>
        public ulong cacheKey;
        /// <summary>
        /// A 16-bit, unsigned integer. specify the index of the bitmap cache entry in which the source bitmap data is stored.   
        /// </summary>
        public ushort cacheSlot;
        /// <summary>
        /// A RDPGFX_RECT16 structure. specify the rectangle that bounds the source bitmap.   
        /// </summary>
        public RDPGFX_RECT16 rectSrc;
        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_SURFACE_TO_CACHE()
        {
        }

        /// <summary>
        /// Constructor, create a surface to cache message.
        /// </summary>
        /// <param name="sid">This is used to indicate surface id.</param>
        /// <param name="key">This is used to indicate a key to associate with the bitmap cache entry.</param>
        /// <param name="slot">This is used to indicate the index of the bitmap cache entry in which 
        /// the source bitmap data is stored.</param>
        /// <param name="rect">This is used to indicate rectangle that bounds the source bitmap.</param>
        public RDPGFX_SURFACE_TO_CACHE(ushort sid, ulong key, ushort slot, RDPGFX_RECT16 rect)
        {
            this.Header.cmdId = PacketTypeValues.RDPGFX_CMDID_SURFACETOCACHE;
            this.Header.flags = 0x0;
            this.Header.pduLength = 28;  // header 8 bytes, body: 20 bytes

            this.surfaceId = sid;
            this.cacheKey = key;
            this.cacheSlot = slot;
            this.rectSrc = rect;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(this.surfaceId);
            marshaler.WriteUInt64(this.cacheKey);
            marshaler.WriteUInt16(this.cacheSlot);
            marshaler.WriteUInt16(this.rectSrc.left);
            marshaler.WriteUInt16(this.rectSrc.top);
            marshaler.WriteUInt16(this.rectSrc.right);
            marshaler.WriteUInt16(this.rectSrc.bottom);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.surfaceId = marshaler.ReadUInt16();
                pduLen += 2;
                this.cacheKey = marshaler.ReadUInt64();
                pduLen += 8;
                this.cacheSlot = marshaler.ReadUInt16();
                pduLen += 2;
                this.rectSrc.left = marshaler.ReadUInt16();
                pduLen += 2;
                this.rectSrc.top = marshaler.ReadUInt16();
                pduLen += 2;
                this.rectSrc.right = marshaler.ReadUInt16();
                pduLen += 2;
                this.rectSrc.bottom = marshaler.ReadUInt16();
                pduLen += 2;

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }

    /// <summary>
    /// The RDPGFX_CACHE_TO_SURFACE message instruct the client to copy bitmap data from the bitmap cache to a destination surface.
    /// </summary>
    public class RDPGFX_CACHE_TO_SURFACE : RdpegfxServerPdu
    {
        #region Message Fields
        /// <summary>
        /// A 16-bit, unsigned integer. specify the index of the bitmap cache entry in which the source bitmap data is stored.   
        /// </summary>
        public ushort cacheSlot;
        /// <summary>
        /// A 16-bit, unsigned integer. specify the ID of the destination surface.   
        /// </summary>
        public ushort surfaceId;
        /// <summary>
        /// A 16-bit unsigned integer that specifies the number of RDPGFX_POINT16 structures.   
        /// </summary>
        public ushort destPtsCount;
        /// <summary>
        /// A RDPGFX_POINT16 array. specifies target points on the destination surface to which to copy the source bitmap.    
        /// </summary>
        public List<RDPGFX_POINT16> destPtsList;
        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_CACHE_TO_SURFACE()
        {
        }

        /// <summary>
        /// Constructor, create a cache to surface message, with only 1 destination copy position.
        /// </summary>
        /// <param name="slot">This is used to indicate a cache slot.</param>
        /// <param name="sid">This is used to indicate the destination surface id.</param>
        public RDPGFX_CACHE_TO_SURFACE(ushort slot, ushort sid)
        {
            this.Header.cmdId = PacketTypeValues.RDPGFX_CMDID_CACHETOSURFACE;
            this.Header.flags = 0x0;
            this.Header.pduLength = (uint)Marshal.SizeOf(Header) + 6;

            this.cacheSlot = slot;
            this.surfaceId = sid;
            this.destPtsCount = 0;
            this.destPtsList = new List<RDPGFX_POINT16>();
            this.destPtsList.Clear();
        }

        /// <summary>
        /// Add more position in destination surface to be copied.
        /// </summary>
        /// <param name="destPosition">This is used to specify a destination position of source rectangle bitmap to be copied.</param>
        public void AddDestPosition(RDPGFX_POINT16 destPosition)
        {
            this.destPtsList.Add(destPosition);
            this.destPtsCount++;
            this.Header.pduLength += (uint)Marshal.SizeOf(destPosition);
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(this.cacheSlot);
            marshaler.WriteUInt16(this.surfaceId);
            marshaler.WriteUInt16(this.destPtsCount);

            foreach (RDPGFX_POINT16 destPoint in destPtsList)
            {
                marshaler.WriteUInt16(destPoint.x);
                marshaler.WriteUInt16(destPoint.y);
            }
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.cacheSlot = marshaler.ReadUInt16();
                pduLen += 2;
                this.surfaceId = marshaler.ReadUInt16();
                pduLen += 2;
                this.destPtsCount = marshaler.ReadUInt16();
                pduLen += 2;

                for (int i = 0; i < this.destPtsCount; i++)
                {
                    RDPGFX_POINT16 destPoint;
                    destPoint.x = marshaler.ReadUInt16();
                    destPoint.y = marshaler.ReadUInt16();
                    destPtsList.Add(destPoint);

                    pduLen += 4;
                }

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }

    /// <summary>
    /// The RDPGFX_EVICT_CACHE_ENTRY message instruct the client to delete an entry from the bitmap cache.
    /// </summary>
    public class RDPGFX_EVICT_CACHE_ENTRY : RdpegfxServerPdu
    {
        #region Message Fields
        /// <summary>
        /// A 16-bit, unsigned integer. specify the index of the bitmap cache entry to delete from the bitmap cache.
        /// </summary>
        public ushort cacheSlot;
        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_EVICT_CACHE_ENTRY()
        {
        }

        /// <summary>
        /// constructor, create an evict cache message.
        /// </summary>
        /// <param name="slot">This is used to indicate a cache slot to delete.</param>
        public RDPGFX_EVICT_CACHE_ENTRY(ushort slot)
        {
            this.Header.cmdId = PacketTypeValues.RDPGFX_CMDID_EVICTCACHEENTRY;
            this.Header.flags = 0x0;
            this.Header.pduLength = (uint)Marshal.SizeOf(Header) + 2;

            this.cacheSlot = slot;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(this.cacheSlot);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.cacheSlot = marshaler.ReadUInt16();
                pduLen += 2;

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }

    /// <summary>
    /// The RDPGFX_CACHE_IMPORT_REPLY message is to indicate that persistent bitmap cache metadata advertised 
    /// in the RDPGFX_CACHE_IMPORT_OFFER message has been transferred to the bitmap cache.
    /// </summary>
    public class RDPGFX_CACHE_IMPORT_REPLY : RdpegfxServerPdu
    {
        #region Message Fields
        /// <summary>
        /// A 16-bit, unsigned integer. specify the number of entries that were imported into the server-side Bitmap Cache Map ADM.
        /// </summary>
        public ushort importedEntriesCount;
        /// <summary>
        /// An array of 16-bit unsigned integer. specify a cache slot that each imported entry has been assigned. .    
        /// </summary>
        public List<ushort> cacheSlotsList;
        #endregion

        #region Methods

        /// <summary>
        /// Constructor, create a cache to surface message, with only 1 destination copy position.
        /// </summary>
        /// <param name="slot">This is used to indicate a cache slot.</param>
        public RDPGFX_CACHE_IMPORT_REPLY()
        {
            this.Header.cmdId = PacketTypeValues.RDPGFX_CMDID_CACHEIMPORTREPLY;
            this.Header.flags = 0x0;
            this.Header.pduLength = (uint)Marshal.SizeOf(Header) + 2;

            this.importedEntriesCount = 0;
            this.cacheSlotsList = new List<ushort>();
            this.cacheSlotsList.Clear();
        }

        /// <summary>
        /// Assign a cache slot assigned for a cache entry.
        /// </summary>
        /// <param name="slot">This is used to specify cache slot to be assigned for a cache entry.</param>
        public void AssignSlot(ushort slot)
        {
            this.importedEntriesCount++;
            this.cacheSlotsList.Add(slot);
            this.Header.pduLength += sizeof(ushort);
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(this.importedEntriesCount);

            foreach (ushort slot in cacheSlotsList)
            {
                marshaler.WriteUInt16(slot);
            }
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.importedEntriesCount = marshaler.ReadUInt16();
                pduLen += 2;

                for (int i = 0; i < this.importedEntriesCount; i++)
                {
                    ushort slot = marshaler.ReadUInt16();
                    cacheSlotsList.Add(slot);
                    pduLen += 2;
                }
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }

    /// <summary>
    /// The RDPGFX_WIRE_TO_SURFACE_PDU_1 message is to used to transfer encoded bitmap data from the server to 
    /// a client-side destination surface. it support NSCodec, RemoteFX Codec, ClearCodec, and Planar Codec.
    /// </summary>
    public class RDPGFX_WIRE_TO_SURFACE_PDU_1 : RdpegfxServerPdu
    {
        #region Message Fields
        /// <summary>
        /// A 16-bit, unsigned integer. specify the ID of the destination surface.   
        /// </summary>/// <summary>
        /// A 16-bit, unsigned integer. specify the ID of the destination surface.   
        /// </summary>
        public ushort surfaceId;
        /// <summary>
        /// A 16-bit, unsigned integer. specify the codec that was used to encode the bitmap data.
        /// encapsulated in the bitmapData field.   
        /// </summary>
        public CodecType codecId;
        /// <summary>
        /// A 8-bit, unsigned integer. specify the pixel format of the decoded bitmap data. 
        /// </summary>
        public PixelFormat pixelFormat;
        /// <summary>
        /// A RDPGFX_RECT16 structure(8 bytes). the target point on the destination surface to which to copy 
        /// the decoded bitmap and the dimensions (width and height) of the bitmap data.
        /// </summary>
        public RDPGFX_RECT16 destRect;
        /// <summary>
        /// A 32-bit, unsigned integer. Specify the length, in bytes, of the bitmapData field.   
        /// </summary>
        public uint bitmapDataLength;
        /// <summary>
        /// A variable-length array of bytes. specify bitmap data encoded using the codec identified by the ID in the codecId field.   
        /// </summary>
        public byte[] bitmapData;
        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_WIRE_TO_SURFACE_PDU_1()
        {
        }

        /// <summary>
        /// Constructor, create a wire to surface message to transfer a bitmap.
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id.</param>
        /// <param name="cId">This is used to indicate the codecId.</param>
        /// <param name="pixFormat">This is used to indicate the pixel format to fill target surface.</param>
        /// <param name="bmRect">This is used to indicate border of bitmap on target surface.</param>
        /// <param name="bmLen">This is used to indicate the length of bitmap data.</param>
        /// <param name="bmData">This is used to indicate the bitmap data encoded by cId codec.</param>
        public RDPGFX_WIRE_TO_SURFACE_PDU_1(ushort sId, CodecType cId, PixelFormat pixFormat, RDPGFX_RECT16 bmRect, byte[] bmData)
        {
            this.Header.cmdId = PacketTypeValues.RDPGFX_CMDID_WIRETOSURFACE_1;
            this.Header.flags = 0x0;
            this.Header.pduLength = (uint)Marshal.SizeOf(Header);

            this.surfaceId = sId;
            this.codecId = cId;
            this.pixelFormat = pixFormat;
            this.destRect = bmRect;
            this.Header.pduLength += 13;

            this.bitmapData = bmData;
            if (bmData != null)
            {
                this.bitmapDataLength = (uint)bmData.Length;
                this.Header.pduLength += this.bitmapDataLength;   // add length of bitmapData
            }
            this.Header.pduLength += 4;  // 4 bytes for bitmapDataLength field.


        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(this.surfaceId);
            marshaler.WriteUInt16((ushort)this.codecId);
            marshaler.WriteByte((byte)this.pixelFormat);

            // Destination rectangle.
            marshaler.WriteUInt16(this.destRect.left);
            marshaler.WriteUInt16(this.destRect.top);
            marshaler.WriteUInt16(this.destRect.right);
            marshaler.WriteUInt16(this.destRect.bottom);

            // Bitmap.
            marshaler.WriteUInt32(this.bitmapDataLength);
            marshaler.WriteBytes(this.bitmapData);

        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.surfaceId = marshaler.ReadUInt16();
                pduLen += 2;
                this.codecId = (CodecType)marshaler.ReadUInt16();
                pduLen += 2;
                this.pixelFormat = (PixelFormat)marshaler.ReadByte();
                pduLen++;

                // Destination rectangle.
                this.destRect.left = marshaler.ReadUInt16();
                this.destRect.top = marshaler.ReadUInt16();
                this.destRect.right = marshaler.ReadUInt16();
                this.destRect.bottom = marshaler.ReadUInt16();
                pduLen += 8;

                // Bitmap.
                this.bitmapDataLength = marshaler.ReadUInt32();
                pduLen += 4;
                this.bitmapData = marshaler.ReadBytes((int)this.bitmapDataLength);
                pduLen += this.bitmapDataLength;

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }

    /// <summary>
    /// The RDPGFX_WIRE_TO_SURFACE_PDU_2 message is  used to transfer encoded bitmap data progressively 
    /// from the server to a client-side destination surface. It support RFX progressive Codec only.
    /// </summary>
    public class RDPGFX_WIRE_TO_SURFACE_PDU_2 : RdpegfxServerPdu
    {
        #region Message Fields
        /// <summary>
        /// A 16-bit, unsigned integer. specify the ID of the destination surface.   
        /// </summary>
        public ushort surfaceId;
        /// <summary>
        /// A 16-bit, unsigned integer. specify the codec that was used to encode the bitmap data 
        /// encapsulated in the bitmapData field. must be RDPGFX_CODECID_CAPROGRESSIVE(0x0009)
        /// </summary>
        public CodecType codecId;
        /// <summary>
        /// A 32-bit, unsigned integer. Specify the compression context associated with the 
        /// bitmap data encapsulated in the bitmapData field.
        /// </summary>
        public uint codecContextId;
        /// <summary>
        /// A 8-bit, unsigned integer. specify the pixel format of the decoded bitmap data   
        /// </summary>
        public PixelFormat pixelFormat;
        /// <summary>
        /// A 32-bit, unsigned integer. Specify the length, in bytes, of the bitmapData field.   
        /// </summary>
        public uint bitmapDataLength;
        /// <summary>
        /// A variable-length array of bytes. specify bitmap data encoded using the codec identified by the ID in the codecId field.   
        /// </summary>
        public byte[] bitmapData;
        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_WIRE_TO_SURFACE_PDU_2()
        {
        }

        /// <summary>
        /// Constructor, create a wire to surface message to transfer a bitmap.
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id.</param>
        /// <param name="codecCtxId">This is used to indicate the codecContextId.</param>
        /// <param name="pixFormat">This is used to indicate the pixel format to fill target surface.</param>
        /// <param name="bmLen">This is used to indicate the length of bitmap data.</param>
        /// <param name="bmData">This is used to indicate the bitmap data encoded by cId codec.</param>
        public RDPGFX_WIRE_TO_SURFACE_PDU_2(ushort sId, uint codecCtxId, PixelFormat pixFormat, byte[] bmData)
        {
            this.Header.cmdId = PacketTypeValues.RDPGFX_CMDID_WIRETOSURFACE_2;
            this.Header.flags = 0x0;
            this.Header.pduLength = (uint)Marshal.SizeOf(Header);

            this.surfaceId = sId;
            this.codecId = CodecType.RDPGFX_CODECID_CAPROGRESSIVE;
            this.codecContextId = codecCtxId;
            this.pixelFormat = pixFormat;
            this.bitmapDataLength = (uint)bmData.Length;
            this.Header.pduLength += 13;

            this.bitmapData = bmData;
            this.Header.pduLength += this.bitmapDataLength;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(this.surfaceId);
            marshaler.WriteUInt16((ushort)this.codecId);
            marshaler.WriteUInt32(this.codecContextId);
            marshaler.WriteByte((byte)this.pixelFormat);

            // Bitmap.
            marshaler.WriteUInt32(this.bitmapDataLength);
            marshaler.WriteBytes(this.bitmapData);

        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.surfaceId = marshaler.ReadUInt16();
                pduLen += 2;
                this.codecId = (CodecType)marshaler.ReadUInt16();
                pduLen += 2;
                this.codecContextId = marshaler.ReadUInt32();
                pduLen += 4;
                this.pixelFormat = (PixelFormat)marshaler.ReadByte();
                pduLen++;

                // Bitmap.
                this.bitmapDataLength = marshaler.ReadUInt32();
                pduLen += 4;
                this.bitmapData = marshaler.ReadBytes((int)this.bitmapDataLength);
                pduLen += this.bitmapDataLength;

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }

        #endregion
    }

    /// <summary>
    /// The RDPGFX_DELETE_ENCODING_CONTEXT message is used to instruct the client to delete a compression context 
    /// that was used by a collection of RDPGFX_WIRE_TO_SURFACE_PDU_2 messages to progressively transfer bitmap data.
    /// </summary>
    public class RDPGFX_DELETE_ENCODING_CONTEXT : RdpegfxServerPdu
    {
        #region Message Fields
        /// <summary>
        /// A 16-bit, unsigned integer. specify the ID of the destination surface.   
        /// </summary>
        public ushort surfaceId;
        /// <summary>
        /// A 32-bit, unsigned integer. Specify the compression context associated with the 
        /// bitmap data encapsulated in the bitmapData field.
        /// </summary>
        public uint codecContextId;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_DELETE_ENCODING_CONTEXT()
        {
        }

        /// <summary>
        /// Constructor, create a delete encoding context message to client.
        /// </summary>
        /// <param name="sId">This is used to indicate the target surface id.</param>
        /// <param name="codecCtxId">This is used to indicate the codecContextId.</param>
        public RDPGFX_DELETE_ENCODING_CONTEXT(ushort sId, uint codecCtxId)
        {
            this.Header.cmdId = PacketTypeValues.RDPGFX_CMDID_DELETEENCODINGCONTEXT;
            this.Header.flags = 0x0;
            this.Header.pduLength = (uint)Marshal.SizeOf(Header) + 6;

            this.surfaceId = sId;
            this.codecContextId = codecCtxId;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(this.surfaceId);
            marshaler.WriteUInt32(this.codecContextId);

        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.surfaceId = marshaler.ReadUInt16();
                pduLen += 2;
                this.codecContextId = marshaler.ReadUInt32();
                pduLen += 4;

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }


        #endregion
    }

    /// <summary>
    /// The RDPGFX_MAP_SURFACE_TO_WINDOW_PDU message is sent by the server to instruct the client to map 
    /// a surface to a RAIL window ([MS-RDPERP] section 1.1) on the client.
    /// </summary>
    public class RDPGFX_MAP_SURFACE_TO_WINDOW : RdpegfxServerPdu
    {
        #region Message Fields
        /// <summary>
        /// A 16-bit, unsigned integer. specify the ID of the destination surface.   
        /// </summary>
        public ushort surfaceId;
        /// <summary>
        /// A 64-bit unsigned integer that specifies the ID of the RAIL window to be associated with the surface-to-window mapping.
        /// </summary>
        public ulong windowId;
        /// <summary>
        /// A 32-bit unsigned integer that specifies the width of the rectangular region on the surface to which the window is mapped.
        /// </summary>
        public uint mappedWidth;
        /// <summary>
        /// A 32-bit unsigned integer that specifies the height of the rectangular region on the surface to which the window is mapped.
        /// </summary>
        public uint mappedHeight;
        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_MAP_SURFACE_TO_WINDOW()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="surfaceId"></param>
        /// <param name="windowId"></param>
        /// <param name="mappedWidth"></param>
        /// <param name="mappedHeight"></param>
        public RDPGFX_MAP_SURFACE_TO_WINDOW(ushort surfaceId, ulong windowId, uint mappedWidth, uint mappedHeight)
        {
            this.Header.cmdId = PacketTypeValues.RDPGFX_CMDID_MAPSURFACETOWINDOW;
            this.Header.flags = 0x0;
            this.Header.pduLength = (uint)Marshal.SizeOf(Header)
                + (uint) Marshal.SizeOf(this.surfaceId)
                + (uint)Marshal.SizeOf(this.windowId)
                + (uint)Marshal.SizeOf(this.mappedWidth)
                + (uint)Marshal.SizeOf(this.mappedHeight)
                ;

            this.surfaceId = surfaceId;
            this.windowId = windowId;
            this.mappedWidth = mappedWidth;
            this.mappedHeight = mappedHeight;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(this.surfaceId);
            marshaler.WriteUInt64(this.windowId);
            marshaler.WriteUInt32(this.mappedWidth);
            marshaler.WriteUInt32(this.mappedHeight);

        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.surfaceId = marshaler.ReadUInt16();
                pduLen += 2;
                this.windowId = marshaler.ReadUInt64();
                pduLen += 8;
                this.mappedWidth = marshaler.ReadUInt32();
                pduLen += 4;
                this.mappedHeight = marshaler.ReadUInt32();
                pduLen += 4;

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }

    /// <summary>
    /// The optional RDPGFX_QOE_FRAME_ACKNOWLEDGE_PDU message is sent by the client to enable the calculation of Quality of Experience (QoE) metrics.
    /// </summary>
    public class RDPGFX_QOE_FRAME_ACKNOWLEDGE_PDU : RdpegfxServerPdu
    {
        #region Message Fields
        /// <summary>
        /// An RDPGFX_HEADER structure  
        /// </summary>
        public RDPGFX_HEADER header;
        /// <summary>
        /// A 32-bit unsigned integer that contains the ID of the frame being annotated.
        /// </summary>
        public uint frameId;
        /// <summary>
        /// A 32-bit unsigned integer that specifies the timestamp (in milliseconds) when the client started decoding the RDPGFX_START_FRAME_PDU message.
        /// </summary>
        public uint timestamp;
        /// <summary>
        /// A 16-bit unsigned integer that specifies the time, in milliseconds, that elapsed between the decoding of the RDPGFX_START_FRAME_PDU and RDPGFX_END_FRAME_PDU messages.
        /// </summary>
        public ushort timeDiffSE;
        /// <summary>
        /// A 16-bit unsigned integer that specifies the time, in milliseconds, that elapsed between the decoding of the RDPGFX_END_FRAME_PDU message and the completion of the rendering operation for the commands contained in the logical graphics frame.
        /// </summary>
        public ushort timeDiffEDR;
        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_QOE_FRAME_ACKNOWLEDGE_PDU()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="frameId"></param>
        /// <param name="timestamp"></param>
        /// <param name="timeDiffSE"></param>
        /// <param name="timeDiffEDR"></param>
        public RDPGFX_QOE_FRAME_ACKNOWLEDGE_PDU(uint frameId, uint timestamp, ushort timeDiffSE, ushort timeDiffEDR)
        {
            this.header.cmdId = PacketTypeValues.RDPGFX_CMDID_QOEFRAMEACKNOWLEDGE;
            this.header.flags = 0x0;
            this.header.pduLength = (uint)Marshal.SizeOf(header) 
                + (uint) Marshal.SizeOf(this.frameId)
                + (uint) Marshal.SizeOf(this.timestamp)
                + (uint) Marshal.SizeOf(this.timeDiffSE)
                + (uint) Marshal.SizeOf(this.timeDiffEDR);

            this.frameId = frameId;
            this.timestamp = timestamp;
            this.timeDiffSE = timeDiffSE;
            this.timeDiffEDR = timeDiffEDR;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(this.frameId);
            marshaler.WriteUInt32(this.timestamp);
            marshaler.WriteUInt16(this.timeDiffSE);
            marshaler.WriteUInt16(this.timeDiffEDR);

        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.frameId = marshaler.ReadUInt32();
                pduLen += 4;
                this.timestamp = marshaler.ReadUInt32();
                pduLen += 4;
                this.timeDiffEDR = marshaler.ReadUInt16();
                pduLen += 2;
                this.timeDiffSE = marshaler.ReadUInt16();
                pduLen += 2;

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }

    /// <summary>
    /// The RDPGFX_MAP_SURFACE_TO_SCALED_OUTPUT_PDU message is sent by the server to instruct the client to map a surface to a rectangular area of the Graphics Output Buffer ADM element, 
    /// including a target width and height to which the surface MUST be scaled.
    /// </summary>
    public class RDPGFX_MAP_SURFACE_TO_SCALED_OUTPUT_PDU : RdpegfxServerPdu
    {
        #region Message Fields
        
        /// <summary>
        /// A 16-bit unsigned integer that specifies the ID of the surface to be associated with the output-to-surface mapping.
        /// </summary>
        public ushort surfaceId;
        /// <summary>
        /// A 16-bit unsigned integer that is reserved for future use. This field MUST be set to zero. 
        /// </summary>
        public ushort reserved;
        /// <summary>
        /// A 32-bit unsigned integer that specifies the x-coordinate of the point, relative to the origin of the Graphics Output Buffer ADM element, at which to map the top-left corner of the surface.
        /// </summary>
        public uint outputOriginX;
        /// <summary>
        /// A 32-bit unsigned integer that specifies the y-coordinate of the point, relative to the origin of the Graphics Output Buffer ADM element, at which to map the upper-left corner of the surface.
        /// </summary>
        public uint outputOriginY;
        /// <summary>
        /// A 32-bit unsigned integer that specifies the width of the target Graphics Output Buffer ADM element to which the surface will be mapped.
        /// </summary>
        public uint targetWidth;
        /// <summary>
        /// A 32-bit unsigned integer that specifies the height of the target Graphics Output Buffer ADM element to which the surface will be mapped.
        /// </summary>
        public uint targetHeight;
        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_MAP_SURFACE_TO_SCALED_OUTPUT_PDU()
        {            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="surfaceId">surface id</param>
        /// <param name="x">output origin x</param>
        /// <param name="y">output origin y</param>
        /// <param name="width">target width</param>
        /// <param name="height">target height</param>
        public RDPGFX_MAP_SURFACE_TO_SCALED_OUTPUT_PDU(ushort surfaceId, uint x, uint y, uint width, uint height)
        {            
            this.Header.cmdId = PacketTypeValues.RDPGFX_CMDID_MAPSURFACETOSCALEDOUTPUT;
            this.Header.flags = 0x0;
            this.Header.pduLength = (uint)Marshal.SizeOf(Header) 
                + (uint)Marshal.SizeOf(this.surfaceId)
                + (uint)Marshal.SizeOf(this.reserved)
                + (uint)Marshal.SizeOf(this.outputOriginX)
                + (uint)Marshal.SizeOf(this.outputOriginY)
                + (uint)Marshal.SizeOf(this.targetHeight)
                + (uint)Marshal.SizeOf(this.targetWidth)
                ;
            
            this.surfaceId = surfaceId;
            this.reserved = 0;
            this.outputOriginX = x;
            this.outputOriginY = y;
            this.targetHeight = height;
            this.targetWidth = width;
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(this.surfaceId);
            marshaler.WriteUInt16(this.reserved);            
            marshaler.WriteUInt32(this.outputOriginX);
            marshaler.WriteUInt32(this.outputOriginY);
            marshaler.WriteUInt32(this.targetWidth);
            marshaler.WriteUInt32(this.targetHeight);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);              
                this.surfaceId = marshaler.ReadUInt16();
                pduLen += 2;
                this.reserved = marshaler.ReadUInt16();
                pduLen += 2;
                this.outputOriginX = marshaler.ReadUInt32();
                pduLen += 4;
                this.outputOriginY = marshaler.ReadUInt32();
                pduLen += 4;
                this.targetWidth = marshaler.ReadUInt32();
                pduLen += 4;
                this.targetHeight = marshaler.ReadUInt32();
                
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }

    /// <summary>
    /// The RDPGFX_MAP_SURFACE_TO_SCALED_WINDOW_PDU message is sent by the server to instruct the client to map a surface to a RAIL window on the client, 
    /// including a target width and height to which the surface should be scaled.
    /// </summary>
    public class RDPGFX_MAP_SURFACE_TO_SCALED_WINDOW_PDU : RdpegfxServerPdu
    {
        #region Message Fields
        /// <summary>
        /// An RDPGFX_HEADER structure  
        /// </summary>
        public RDPGFX_HEADER header;
        /// <summary>
        /// A 16-bit unsigned integer that specifies the ID of the surface to be associated with the surface-to-window mapping.
        /// </summary>
        public ushort surfaceId;
        /// <summary>
        /// A 64-bit unsigned integer that specifies the ID of the RAIL window to be associated with the surface-to-window mapping.
        /// </summary>
        public ulong windowId;
        /// <summary>
        /// A 32-bit unsigned integer that specifies the width of the rectangular region on the surface to which the window is mapped.
        /// </summary>
        public uint mappedWidth;
        /// <summary>
        /// A 32-bit unsigned integer that specifies the height of the rectangular region on the surface to which the window is mapped.
        /// </summary>
        public uint mappedHeight;
        /// <summary>
        /// A 32-bit unsigned integer that specifies the width of the target graphics output to which the surface will be mapped.
        /// </summary>
        public uint targetWidth;
        /// <summary>
        /// A 32-bit unsigned integer that specifies the height of the target graphics output to which the surface will be mapped.
        /// </summary>
        public uint targetHeight;
        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public RDPGFX_MAP_SURFACE_TO_SCALED_WINDOW_PDU()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="frameId"></param>
        /// <param name="timestamp"></param>
        /// <param name="timeDiffSE"></param>
        /// <param name="timeDiffEDR"></param>
        public RDPGFX_MAP_SURFACE_TO_SCALED_WINDOW_PDU(ushort surfaceId, ulong windowId, uint mappedWidth, uint mappedHeight, uint targetWidth, uint targetHeight)
        {
            this.header.cmdId = PacketTypeValues.RDPGFX_CMDID_MAPSURFACETOSCALEDWINDOW;
            this.header.flags = 0x0;
            this.header.pduLength = (uint)Marshal.SizeOf(header) 
                + (uint) Marshal.SizeOf(this.surfaceId)
                + (uint)Marshal.SizeOf(this.windowId)
                + (uint)Marshal.SizeOf(this.mappedWidth)
                + (uint)Marshal.SizeOf(this.mappedHeight)
                + (uint)Marshal.SizeOf(this.targetWidth)
                + (uint)Marshal.SizeOf(this.targetHeight)
                ;

            this.surfaceId = surfaceId;
            this.windowId = windowId;
            this.mappedWidth = mappedWidth;
            this.mappedHeight = mappedHeight;            
            this.targetWidth = targetWidth;
            this.targetHeight = targetHeight;            
        }

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(this.surfaceId);
            marshaler.WriteUInt64(this.windowId);
            marshaler.WriteUInt32(this.mappedWidth);
            marshaler.WriteUInt32(this.mappedHeight);
            marshaler.WriteUInt32(this.targetWidth);
            marshaler.WriteUInt32(this.targetHeight);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.surfaceId = marshaler.ReadUInt16();
                pduLen += 2;
                this.windowId = marshaler.ReadUInt64();
                pduLen += 8;
                this.mappedWidth = marshaler.ReadUInt32();
                pduLen += 4;
                this.mappedHeight = marshaler.ReadUInt32();
                pduLen += 4;
                this.targetWidth = marshaler.ReadUInt32();
                pduLen += 4;
                this.targetHeight = marshaler.ReadUInt32();
                pduLen += 4;

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }
    #endregion


    #region Client Messages
    /// <summary>
    /// This message is sent by the client to server to advertise supported capabilities. 
    /// </summary>
    public class RDPGFX_CAPS_ADVERTISE : RdpegfxClientPdu
    {
        #region Message Fields

        /// <summary>
        /// UINT16. Specify the number of capability sets supported on the clients.
        /// </summary>
        public ushort capsSetCount;

        /// <summary>
        /// variable-length. Specify the capability sets supported on the clients.
        /// </summary>
        public RDPGFX_CAPSET[] capsSets;

        #endregion

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(this.capsSetCount);

            for (int i = 0; i < this.capsSetCount; i++)
            {
                marshaler.WriteUInt32((uint)capsSets[i].version);
                marshaler.WriteUInt32(capsSets[i].capsDataLength);
                marshaler.WriteBytes(capsSets[i].capsData);
            }
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                capsSetCount = marshaler.ReadUInt16();
                pduLen += 2;
                capsSets = new RDPGFX_CAPSET[capsSetCount];

                for (ushort i = 0; i < capsSetCount; i++)
                {
                    CapsVersions version = (CapsVersions)marshaler.ReadUInt32();
                    capsSets[i].version = version;
                    pduLen += 4;
                    capsSets[i].capsDataLength = marshaler.ReadUInt32();
                    pduLen += 4;
                    capsSets[i].capsData = new byte[capsSets[i].capsDataLength];
                    capsSets[i].capsData = marshaler.ReadBytes((int)capsSets[i].capsDataLength);
                    pduLen += capsSets[i].capsDataLength;
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
    /// The RDPGFX_FRAME_ACK message is sent from client to indicate to server that a logical frame is successfully decoded on client.
    /// </summary>
    public class RDPGFX_FRAME_ACK : RdpegfxClientPdu
    {
        public const uint SUSPEND_FRAME_ACKNOWLEDGE = 0xFFFFFFFF;

        #region Message Fields
        /// <summary>
        ///  A 32-bit unsigned integer. number of unprocessed bytes buffered at the client.
        /// </summary>
        public uint queueDepth;
        /// <summary>
        ///  A 32-bit unsigned integer. frame ID of the frame.
        /// </summary>
        public uint frameId;

        /// <summary>
        ///  A 32-bit unsigned integer. The number of frames decoded by the client since connection was initiated.
        /// </summary>
        public uint totalFramesDecoded;

        #endregion

        #region Methods

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt32(this.queueDepth);
            marshaler.WriteUInt32(this.frameId);
            marshaler.WriteUInt32(this.totalFramesDecoded);

        }
        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                this.queueDepth = marshaler.ReadUInt32();
                pduLen += 4;
                this.frameId = marshaler.ReadUInt32();
                pduLen += 4;
                this.totalFramesDecoded = marshaler.ReadUInt32();
                pduLen += 4;
                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }

        #endregion
    }

    /// <summary>
    /// The RDPGFX_FRAME_ACK message is sent from client to  inform the server of bitmap data 
    /// that is present in the client-side persistent bitmap cache.
    /// </summary>
    public class RDPGFX_CACHE_IMPORT_OFFER : RdpegfxClientPdu
    {
        #region Message Fields
        /// <summary>
        /// UINT16. Specify the number of RDPGFX_CACHE_ENTRY_METADATA structures.
        /// </summary>
        public ushort cacheEntriesCount;

        /// <summary>
        /// Variable-length. Specify a collection of bitmap cache entries present on the client.
        /// </summary>
        public RDPGFX_CACHE_ENTRY_METADATA[] cacheEntries;

        /// <summary>
        /// UINT32. Specify the bitmap cache total size of cache entries indicated in the cache import offer request.
        /// </summary>
        public uint totalCacheSize;

        #endregion

        #region Methods

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler);
            marshaler.WriteUInt16(this.cacheEntriesCount);
            for (ushort i = 0; i < cacheEntriesCount; i++)
            {
                marshaler.WriteUInt64(cacheEntries[i].cacheKey);
                marshaler.WriteUInt64(cacheEntries[i].bitmapLength);
            }
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                base.Decode(marshaler);
                cacheEntriesCount = marshaler.ReadUInt16();
                pduLen += 2;
                cacheEntries = new RDPGFX_CACHE_ENTRY_METADATA[cacheEntriesCount];
                totalCacheSize = 0;

                for (ushort i = 0; i < cacheEntriesCount; i++)
                {
                    cacheEntries[i].cacheKey = marshaler.ReadUInt64();
                    pduLen += 8;
                    cacheEntries[i].bitmapLength = marshaler.ReadUInt32();
                    pduLen += 4;
                    totalCacheSize += cacheEntries[i].bitmapLength;  // Calculate the summary of cache size in the request.
                }

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
        #endregion
    }
    #endregion

}

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegt
{
    /// <summary>
    /// It's the only message of MS-RDPEGT. 
    /// It consists of a command, geometry (rectangles), and an identifier which allows correlation of the geometry 
    /// in the current message to any previous geometry the server may have sent.
    /// </summary>
    public class MAPPED_GEOMETRY_PACKET : BasePDU
    {
        #region Packet Fields

        /// <summary>
        /// UINT32. Length, in bytes, of this message.
        /// </summary>
        public uint cbGeometryData;

        /// <summary>
        /// UINT32. The current version of the Remote Desktop Protocol: Geometry Tracking Virtual Channel Extension. In RDP8, this MUST be set to 0x01.
        /// </summary>
        public RdpegtVersionValues Version;

        /// <summary>
        /// UINT64. A number that uniquely identifies this geometry mapping on the server.
        /// </summary>
        public UInt64 MappingId;

        /// <summary>
        /// UINT32. A number that identifies which operation the client is to perform.
        /// </summary>
        public UpdateTypeValues UpdateType;

        /// <summary>
        /// UINT32. This field is reserved and MUST be set to 0x0.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// UINT64. If in window tracking mode (see section 3.1.1), this field MUST be set to the window handle of 
        /// the top-level parent of the window being tracked, or to the window handle of the window itself, if it is a top-level window. 
        /// If not in window tracking mode (see section 3.1.2), this field MUST be set to 0x0.
        /// </summary>
        public UInt64 TopLevelId;

        /// <summary>
        /// UINT32. Position of the left edge of the tracked rectangle, relative to the top-level parent rectangle.
        /// </summary>
        public uint Left;

        /// <summary>
        /// UINT32. Position of the top edge of the tracked rectangle, relative to the top-level parent rectangle.
        /// </summary>
        public uint Top;

        /// <summary>
        /// UINT32. Position of the right edge of the tracked rectangle, relative to the top-level parent rectangle.
        /// </summary>
        public uint Right;

        /// <summary>
        /// UINT32. Position of the bottom edge of the tracked rectangle, relative to the top-level parent rectangle.
        /// </summary>
        public uint Bottom;

        /// <summary>
        /// UINT32. Position of the left edge of the top-level rectangle in virtual desktop coordinates.
        /// </summary>
        public uint TopLevelLeft;

        /// <summary>
        /// UINT32. Position of the top edge of the top-level rectangle in virtual desktop coordinates
        /// </summary>
        public uint TopLevelTop;

        /// <summary>
        /// UINT32. Position of the right edge of the top-level rectangle in virtual desktop coordinates
        /// </summary>
        public uint TopLevelRight;

        /// <summary>
        /// UINT32. Position of the bottom edge of the top-level rectangle in virtual desktop coordinates
        /// </summary>
        public uint TopLevelBottom;

        /// <summary>
        /// UINT32. This MUST be set to 0x02 in RDP8.
        /// </summary>
        public GeometryTypeValues GeometryType;

        /// <summary>
        /// UINT32. Length of the pGeometryBuffer appended to this message.
        /// </summary>
        public uint cbGeometryBuffer;

        /// <summary>
        /// Array of UINT8. This field contains a RGNDATA structure. 
        /// The rectangles in this structure are relative to the tracked rectangle, and represent which parts of the tracked rectangle are visible.
        /// </summary>
        public RGNDATA pGeometryBuffer;


        /// <summary>
        /// Reserved, should be 0.
        /// </summary>
        public byte Reserved2;

        #endregion

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt32(this.cbGeometryData);
            marshaler.WriteUInt32((uint)this.Version);
            marshaler.WriteUInt64(this.MappingId);
            marshaler.WriteUInt32((uint)this.UpdateType);
            marshaler.WriteUInt32(this.Flags);
            marshaler.WriteUInt64(this.TopLevelId);
            marshaler.WriteUInt32(this.Left);
            marshaler.WriteUInt32(this.Top);
            marshaler.WriteUInt32(this.Right);
            marshaler.WriteUInt32(this.Bottom);
            marshaler.WriteUInt32(this.TopLevelLeft);
            marshaler.WriteUInt32(this.TopLevelTop);
            marshaler.WriteUInt32(this.TopLevelRight);
            marshaler.WriteUInt32(this.TopLevelBottom);
            marshaler.WriteUInt32((uint)this.GeometryType);
            marshaler.WriteUInt32(this.cbGeometryBuffer);

            //Encode RGNDATA
            if (this.cbGeometryBuffer != 0)
            {
                marshaler.WriteUInt32(this.pGeometryBuffer.rdh.dwSize);
                marshaler.WriteUInt32(this.pGeometryBuffer.rdh.iType);
                marshaler.WriteUInt32(this.pGeometryBuffer.rdh.nCount);
                marshaler.WriteUInt32(this.pGeometryBuffer.rdh.nRgnSize);
                EncodeRect(this.pGeometryBuffer.rdh.rcBound, marshaler);

                if (this.pGeometryBuffer.Buffer != null)
                {
                    foreach (RECT rct in this.pGeometryBuffer.Buffer)
                    {
                        EncodeRect(rct, marshaler);
                    }
                }
            }
            marshaler.WriteByte(this.Reserved2);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                this.cbGeometryData = marshaler.ReadUInt32();
                this.Version = (RdpegtVersionValues)marshaler.ReadUInt32();
                this.MappingId = marshaler.ReadUInt64();
                this.UpdateType = (UpdateTypeValues)marshaler.ReadUInt32();
                this.Flags = marshaler.ReadUInt32();
                this.TopLevelId = marshaler.ReadUInt64();
                this.Left = marshaler.ReadUInt32();
                this.Top = marshaler.ReadUInt32();
                this.Right = marshaler.ReadUInt32();
                this.Bottom = marshaler.ReadUInt32();
                this.TopLevelLeft = marshaler.ReadUInt32();
                this.TopLevelTop = marshaler.ReadUInt32();
                this.TopLevelRight = marshaler.ReadUInt32();
                this.TopLevelBottom = marshaler.ReadUInt32();
                this.GeometryType = (GeometryTypeValues)marshaler.ReadUInt32();
                this.cbGeometryBuffer = marshaler.ReadUInt32();

                //Decode RGNDATA
                this.pGeometryBuffer.rdh.dwSize = marshaler.ReadUInt32();
                this.pGeometryBuffer.rdh.iType = marshaler.ReadUInt32();
                this.pGeometryBuffer.rdh.nCount = marshaler.ReadUInt32();
                this.pGeometryBuffer.rdh.nRgnSize = marshaler.ReadUInt32();
                DecodeRect(marshaler, out this.pGeometryBuffer.rdh.rcBound);

                this.pGeometryBuffer.Buffer = new RECT[this.pGeometryBuffer.rdh.nCount];
                for (int i = 0; i < this.pGeometryBuffer.Buffer.Length; i++)
                {
                    DecodeRect(marshaler, out this.pGeometryBuffer.Buffer[i]);
                }

                this.Reserved2 = marshaler.ReadByte();

                return true;
            }
            catch
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }

        /// <summary>
        /// Encode a RECT.
        /// </summary>
        private void EncodeRect(RECT rct, PduMarshaler marshaler)
        {
            marshaler.WriteInt32(rct.left);
            marshaler.WriteInt32(rct.top);
            marshaler.WriteInt32(rct.right);
            marshaler.WriteInt32(rct.bottom);
        }

        /// <summary>
        /// Decode to a RECT.
        /// </summary>
        private void DecodeRect(PduMarshaler marshaler, out RECT rct)
        {
            rct = new RECT();
            rct.left = marshaler.ReadInt32();
            rct.top = marshaler.ReadInt32();
            rct.right = marshaler.ReadInt32();
            rct.bottom = marshaler.ReadInt32();
        }
    }

    #region Refers to http://msdn.microsoft.com/en-us/library/dd162940(v=vs.85).aspx
    
    /// <summary>
    /// The RGNDATA structure contains a header and an array of rectangles that compose a region. 
    /// The rectangles are sorted top to bottom, left to right. They do not overlap.
    /// </summary>
    public struct RGNDATA
    {
        /// <summary>
        /// The members of this structure specify the type of region 
        /// (whether it is rectangular or trapezoidal), 
        /// the number of rectangles that make up the region, the size of the buffer that contains the rectangle structures, and so on.
        /// </summary>
        public RGNDATAHEADER rdh;

        /// <summary>
        /// Specifies an arbitrary-size buffer that contains the RECT structures that make up the region.
        /// </summary>
        public RECT[] Buffer;
    }

    /// <summary>
    /// The RGNDATAHEADER structure describes the data returned by the GetRegionData function.
    /// </summary>
    public struct RGNDATAHEADER
    {
        /// <summary>
        /// The size, in bytes, of the header.
        /// </summary>
        public uint dwSize;

        /// <summary>
        /// The type of region. This value must be RDH_RECTANGLES (1).
        /// </summary>
        public uint iType;

        /// <summary>
        /// The number of rectangles that make up the region.
        /// </summary>
        public uint nCount;

        /// <summary>
        /// The size of the RGNDATA buffer required to receive the RECT structures that make up the region. 
        /// If the size is not known, this member can be zero.
        /// </summary>
        public uint nRgnSize;

        /// <summary>
        /// A bounding rectangle for the region in logical units.
        /// </summary>
        public RECT rcBound;
    }

    /// <summary>
    /// The RECT structure defines the coordinates of the upper-left and lower-right corners of a rectangle.
    /// </summary>
    public struct RECT
    {
        /// <summary>
        /// The x-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int left;

        /// <summary>
        /// The y-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int top;

        /// <summary>
        /// The x-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int right;

        /// <summary>
        /// The y-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int bottom;
    }
    
    #endregion
    
}

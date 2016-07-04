// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpemt
{
    /// <summary>
    /// Action (4 bits): A 4-bit unsigned integer that indicates the type of PDU being transmitted.
    /// </summary>
    public enum RDP_TUNNEL_ACTION_Values : byte
    {
        /// <summary>
        /// RDP_TUNNEL_CREATEREQUEST (section 2.2.2.1)
        /// </summary>
        RDPTUNNEL_ACTION_CREATEREQUEST = 0x0,

        /// <summary>
        /// RDP_TUNNEL_CREATERESPONSE (section 2.2.2.2)
        /// </summary>
        RDPTUNNEL_ACTION_CREATERESPONSE = 0x1,

        /// <summary>
        /// RDP_TUNNEL_DATA (section 2.2.2.3)
        /// </summary>
        RDPTUNNEL_ACTION_DATA = 0x2

    }

    /// <summary>
    /// The RDP_TUNNEL_HEADER structure is a common header included in every multitransport PDU specified in section 2.2.2.
    /// </summary>
    public class RDP_TUNNEL_HEADER : BasePDU
    {
        /// <summary>
        /// A 4-bit unsigned integer that indicates the type of PDU being transmitted.
        /// </summary>
        public RDP_TUNNEL_ACTION_Values Action;

        /// <summary>
        /// A 4-bit unsigned integer that specifies tunnel flags. This field MUST be set to zero.
        /// </summary>
        public byte Flags;

        /// <summary>
        /// A 16-bit unsigned integer that specifies the length, in bytes, of the payload following the header. 
        /// This length MUST NOT include the length of the RDP_TUNNEL_HEADER structure.
        /// </summary>
        public ushort PayloadLength;

        /// <summary>
        /// An 8-bit unsigned integer that specifies the combined length, in bytes, of the Action, Flags, PayloadLength, HeaderLength, and SubHeaders fields. 
        /// If the value in this field is larger than 4 bytes, then the SubHeaders field MUST be present.
        /// </summary>
        public byte HeaderLength;

        /// <summary>
        /// An optional, variable-length array of RDP_TUNNEL_SUBHEADER structures (section 2.2.1.1.1). 
        /// This field MUST be present if the value specified in the HeaderLength field is larger than 4 bytes.
        /// </summary>
        public RDP_TUNNEL_SUBHEADER[] SubHeaders;


        #region Encoding/Decoding

        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteByte((byte)((byte)(Flags << 4) | (byte)Action));
            marshaler.WriteUInt16(this.PayloadLength);
            marshaler.WriteByte(this.HeaderLength);
            if (SubHeaders != null)
            {
                foreach (RDP_TUNNEL_SUBHEADER subHeader in SubHeaders)
                {
                    marshaler.WriteBytes(PduMarshaler.Marshal(subHeader));
                }
            }
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                byte actionFlags = marshaler.ReadByte();
                this.Action = (RDP_TUNNEL_ACTION_Values)(byte)(actionFlags & 0xF);
                this.Flags = (byte)(actionFlags & 0xF0);
                this.PayloadLength = marshaler.ReadUInt16();
                this.HeaderLength = marshaler.ReadByte();
                if (this.HeaderLength > 4)
                {
                    int subHdTotalLen = this.HeaderLength - 4;
                    int curDecodedLen = 0;
                    List<RDP_TUNNEL_SUBHEADER> subHds = new List<RDP_TUNNEL_SUBHEADER>();
                    while (subHdTotalLen > curDecodedLen)
                    {

                        byte subHdLen = marshaler.ReadByte();
                        byte subHdType = marshaler.ReadByte();
                        byte[] subHdData = null;
                        if (subHdLen > 2)
                        {
                            subHdData = marshaler.ReadBytes(subHdLen - 2);
                        }
                        RDP_TUNNEL_SUBHEADER subHd = new RDP_TUNNEL_SUBHEADER();
                        subHd.SubHeaderLength = subHdLen;
                        subHd.SubHeaderType = (RDP_TUNNEL_SUBHEADER_TYPE_Values)subHdType;
                        subHd.SubHeaderData = subHdData;
                        subHds.Add(subHd);

                        curDecodedLen += subHdLen;
                    }
                    this.SubHeaders = subHds.ToArray();
                }
                return true;
            }
            catch
            {
                return false; ;
            }
        }

        #endregion
    }

    /// <summary>
    /// An 8-bit unsigned integer that specifies the high-level type of the subheader.
    /// </summary>
    public enum RDP_TUNNEL_SUBHEADER_TYPE_Values : byte
    {
        /// <summary>
        /// The subheader conforms to one of the following auto-detect request structures:
        ///     Bandwidth Measure Start ([MS-RDPBCGR] section 2.2.14.1.2)
        ///     Bandwidth Measure Stop ([MS-RDPBCGR] section 2.2.14.1.4)
        ///     Network Characteristics Result ([MS-RDPBCGR] section 2.2.14.1.5)
        /// The specific auto-detect request structure type MUST be determined by examining the common requestType field.
        /// </summary>
        TYPE_ID_AUTODETECT_REQUEST = 0x00,

        /// <summary>
        /// The subheader conforms to the Bandwidth Measure Results ([MS-RDPBCGR] section 2.2.14.2.2) auto-detect response structure.
        /// </summary>
        TYPE_ID_AUTODETECT_RESPONSE = 0x01

    }

    /// <summary>
    /// A 16-bit unsigned integer that specifies a request type code of the AUTODETECT_REQUEST messages.
    /// </summary>
    [Flags]
    public enum AUTODETECT_REQUEST_TYPE_Values : ushort
    {
        /// <summary>
        /// Not carry any AUTODETECT_REQUEST messages.
        /// </summary>
        None = 0,

        /// <summary>
        /// The Bandwidth Measure Start message is encapsulated in the SubHeaderData field of 
        /// an RDP_TUNNEL_SUBHEADER ([MS-RDPEMT] section 2.2.1.1.1) structure.
        /// </summary>
        // RDP_BW_START = 0x01,

        /// <summary>
        /// The Bandwidth Measure Start message is encapsulated in the SubHeaderData field of
        /// an RDP_TUNNEL_SUBHEADER ([MS-RDPEMT] section 2.2.1.1.1) structure that is being
        /// tunneled over a reliable UDP multitransport connection ([MS-RDPEMT] sections 1.3 and 2.1).
        /// </summary>
        RDP_BW_START_RELIABLE = 0x0014,

        /// <summary>
        /// The Bandwidth Measure Start message is encapsulated in the SubHeaderData field of
        /// an RDP_TUNNEL_SUBHEADER ([MS-RDPEMT] section 2.2.1.1.1) structure that is being
        /// tunneled over a lossy UDP multitransport connection ([MS-RDPEMT] sections 1.3 and 2.1).
        /// </summary>
        RDP_BW_START_LOSSY = 0x0114,

        /// <summary>
        /// A 16-bit unsigned integer that specifies a request type code. This field MUST be set to 0x0002.
        /// </summary>
        RDP_BW_PAYLOAD = 0x0002,

        /// <summary>
        /// The Bandwidth Measure Stop message is encapsulated in the SubHeaderData field of
        /// an RDP_TUNNEL_SUBHEADER ([MS-RDPEMT] section 2.2.1.1.1) structure that is being
        /// tunneled over a reliable UDP multitransport connection ([MS-RDPEMT] sections 1.3 and 2.1).
        /// </summary>
        RDP_BW_STOP_RELIABLE = 0x0429,

        /// <summary>
        /// The Bandwidth Measure Stop message is encapsulated in the SubHeaderData field of
        /// an RDP_TUNNEL_SUBHEADER ([MS-RDPEMT] section 2.2.1.1.1) structure that is being
        /// tunneled over a lossy UDP multitransport connection ([MS-RDPEMT] sections 1.3 and 2.1).
        /// </summary>
        RDP_BW_STOP_LOSSY = 0x0629,

        /// <summary>
        /// The Bandwidth Measure Stop message is encapsulated in the SubHeaderData field of 
        /// an RDP_TUNNEL_SUBHEADER ([MS-RDPEMT] section 2.2.1.1.1) structure.
        /// </summary>
        // RDP_BW_STOP = 0x02,


        /// <summary>
        /// The Network Characteristics Result message is encapsulated in the SubHeaderData field of 
        /// an RDP_TUNNEL_SUBHEADER ([MS-RDPEMT] section 2.2.1.1.1) structure.
        /// </summary>
        RDP_NETCHAR_RESULT = 0x04
    }


    /// <summary>
    /// The RDP_TUNNEL_SUBHEADER structure defines a variable-length generic subheader 
    /// that is embedded within the RDP_TUNNEL_HEADER structure (section 2.2.1.1).
    /// </summary>
    public class RDP_TUNNEL_SUBHEADER : BasePDU
    {
        /// <summary>
        /// An 8-bit unsigned integer that specifies the length, in bytes, of the header fields. 
        /// This length MUST be a minimum of 0x2 bytes since the SubHeaderLength and SubHeaderType fields are an implicit part of the header. The remaining header fields (specific to the subheader type) are embedded in the SubHeaderData field.
        /// </summary>
        public byte SubHeaderLength;

        /// <summary>
        /// An 8-bit unsigned integer that specifies the high-level type of the subheader.
        /// </summary>
        public RDP_TUNNEL_SUBHEADER_TYPE_Values SubHeaderType;

        /// <summary>
        /// A variable-length field that contains data specific to the high-level subheader type.
        /// </summary>
        public byte[] SubHeaderData;

        #region Encoding/Decoding

        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteByte(this.SubHeaderLength);
            marshaler.WriteByte((byte)this.SubHeaderType);
            if (this.SubHeaderData != null)
            {
                marshaler.WriteBytes(this.SubHeaderData);
            }
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                this.SubHeaderLength = marshaler.ReadByte();
                this.SubHeaderType = (RDP_TUNNEL_SUBHEADER_TYPE_Values)marshaler.ReadByte();
                if (this.SubHeaderLength > 2)
                {
                    this.SubHeaderData = marshaler.ReadBytes(this.SubHeaderLength - 2);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }

    /// <summary>
    /// Base class for RDPEMT packets
    /// </summary>
    public abstract class RdpemtBasePDU : BasePDU
    {
        /// <summary>
        /// An RDP_TUNNEL_HEADER structure (section 2.2.1.1). The Action field MUST be set to RDPTUNNEL_ACTION_CREATEREQUEST (0x0).
        /// </summary>
        public RDP_TUNNEL_HEADER TunnelHeader;
        
        /// <summary>
        /// Convert the fields of this PDU to a string.
        /// </summary>
        /// <returns>A string contains the fields of this PDU.</returns>
        public override string ToString()
        {
            return "RDPEMT BasePDU";
        }
    }

    /// <summary>
    /// The RDP_TUNNEL_CREATEREQUEST PDU is sent by the client to request the creation of a multitransport tunnel.
    /// </summary>
    public class RDP_TUNNEL_CREATEREQUEST : RdpemtBasePDU
    {
        /// <summary>
        /// A 32-bit unsigned integer that contains the request ID included in the Initiate Multitransport Request PDU ([MS-RDPBCGR] section 2.2.15.1) 
        /// that was sent over the main RDP connection.
        /// </summary>
        public uint RequestID;

        /// <summary>
        /// A 32-bit unsigned integer that is unused and reserved for future use. This field MUST be set to zero.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// A 16-byte element array of 8-bit unsigned integers that contains the security cookie included 
        /// in the Initiate Multitransport Request PDU that was sent over the main RDP connection.
        /// </summary>
        public byte[] SecurityCookie;

        #region Encoding / Decoding
        public override void Encode(PduMarshaler marshaler)
        {
            byte[] hdData = PduMarshaler.Marshal(this.TunnelHeader);
            marshaler.WriteBytes(hdData);
            marshaler.WriteUInt32(this.RequestID);
            marshaler.WriteUInt32(this.Reserved);
            if (this.SecurityCookie != null)
            {
                marshaler.WriteBytes(this.SecurityCookie);
            }
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                byte[] data = new byte[marshaler.RawData.Length];
                Array.Copy(marshaler.RawData, data, data.Length);

                this.TunnelHeader = new RDP_TUNNEL_HEADER();
                bool bSuccessful = PduMarshaler.Unmarshal(data, this.TunnelHeader);
                if (bSuccessful)
                {
                    marshaler.ReadBytes(this.TunnelHeader.HeaderLength); // Move forward to payload data
                    this.RequestID = marshaler.ReadUInt32();
                    this.Reserved = marshaler.ReadUInt32();
                    this.SecurityCookie = marshaler.ReadBytes(16);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {

                return false;
            }

        }
        #endregion
    }


    /// <summary>
    /// The RDP_TUNNEL_CREATERESPONSE PDU is sent by the server to confirm the creation of a multitransport tunnel.
    /// </summary>
    public class RDP_TUNNEL_CREATERESPONSE : RdpemtBasePDU
    {
        /// <summary>
        /// An HRESULT code ([MS-ERREF] section 2.1) that indicates whether the server accepted the request to create a multitransport connection.
        /// </summary>
        public uint HrResponse;

        #region Encoding / Decoding

        public override void Encode(PduMarshaler marshaler)
        {
            byte[] hdData = PduMarshaler.Marshal(this.TunnelHeader);
            marshaler.WriteBytes(hdData);
            marshaler.WriteUInt32(this.HrResponse);
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                byte[] data = new byte[marshaler.RawData.Length];
                Array.Copy(marshaler.RawData, data, data.Length);

                this.TunnelHeader = new RDP_TUNNEL_HEADER();
                bool bSuccessful = PduMarshaler.Unmarshal(data, this.TunnelHeader);
                if (bSuccessful)
                {
                    marshaler.ReadBytes(this.TunnelHeader.HeaderLength); // Move forward to payload data
                    this.HrResponse = marshaler.ReadUInt32();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {

                return false;
            }
        }
        #endregion
    }

    /// <summary>
    /// The RDP_TUNNEL_DATA PDU is used by the client and server to transport higher-layer data between RDP end-points.
    /// </summary>
    public class RDP_TUNNEL_DATA : RdpemtBasePDU
    {
        /// <summary>
        /// A variable-length array of 8-bit unsigned integers that contains the data that is being transported from one RDP endpoint to another.
        /// </summary>
        public byte[] HigherLayerData;

        #region Encoding / Decoding

        public override void Encode(PduMarshaler marshaler)
        {
            byte[] hdData = PduMarshaler.Marshal(this.TunnelHeader);
            marshaler.WriteBytes(hdData);
            if (this.HigherLayerData != null)
            {
                marshaler.WriteBytes(this.HigherLayerData);
            }
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                byte[] data = new byte[marshaler.RawData.Length];
                Array.Copy(marshaler.RawData, data, data.Length);

                this.TunnelHeader = new RDP_TUNNEL_HEADER();
                bool bSuccessful = PduMarshaler.Unmarshal(data, this.TunnelHeader);
                if (bSuccessful)
                {
                    marshaler.ReadBytes(this.TunnelHeader.HeaderLength); // Move forward to payload data
                    if (this.TunnelHeader.PayloadLength > 0)
                    {
                        this.HigherLayerData = marshaler.ReadBytes(this.TunnelHeader.PayloadLength);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {

                return false;
            }
        }
        #endregion
    }
        
}

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp
{
    /// <summary>
    /// The VECTOR_ELEMENT_STATE enumeration is sent along with every ACK vector (section 2.2.3.1) 
    /// that acknowledges the receipt of a continuous array of datagrams.
    /// </summary>
    public enum VECTOR_ELEMENT_STATE : byte
    {
        /// <summary>
        /// A datagram was received.
        /// </summary>
        DATAGRAM_RECEIVED = 0,

        /// <summary>
        /// Not used.
        /// </summary>
        DATAGRAM_RESERVED_1 = 1,

        /// <summary>
        /// Not used.
        /// </summary>
        DATAGRAM_RESERVED_2 = 2,

        /// <summary>
        /// A datagram has not been received yet.
        /// </summary>
        DATAGRAM_NOT_YET_RECEIVED = 3
    }

    /// <summary>
    /// A 16-bit unsigned integer that indicates supported options, or additional headers.
    /// </summary>
    [Flags]
    public enum RDPUDP_FLAG : ushort
    {
        ///
        /// Corresponds to the SYN flag, for initializing connection.	
        ///
        RDPUDP_FLAG_SYN = 0x0001,

        ///
        /// Corresponds to the FIN flag. Currently unused.
        ///	
        RDPUDP_FLAG_FIN = 0x0002,

        ///
        /// Specifies that the RDPUDP_ACK_VECTOR_HEADER Structure (section 2.2.2.7) is present.	
        ///
        RDPUDP_FLAG_ACK = 0x0004,

        ///
        /// Specifies that the RDPUDP_SOURCE_PAYLOAD_HEADER Structure (section 2.2.2.4) or the RDPUDP_FEC_PAYLOAD_HEADER Structure (section 2.2.2.2) is present. 
        /// This flag specifies that the datagram has additional data beyond the UDP ACK headers.	
        ///
        RDPUDP_FLAG_DATA = 0x0008,

        ///
        /// Specifies that the RDPUDP_FEC_PAYLOAD_HEADER Structure (section 2.2.2.2) is present.	
        ///
        RDPUDP_FLAG_FEC = 0x0010,

        ///
        /// Congestion Notification flag (section 3.1.1), the receiver reports missing datagrams.	
        ///
        RDPUDP_FLAG_CN = 0x0020,

        ///
        /// Congestion Window Reset flag (section 3.1.1), the sender has reduced the congestion window, and informs the receiver to stop adding the RDPUDP_FLAG_CN.	
        ///
        RDPUDP_FLAG_CWR = 0x0040,

        ///
        /// Not used.	
        ///
        RDPUDP_FLAG_SACK_OPTION = 0x0080,

        ///
        /// Specifies that the RDPUDP_ACK_OF_ACKVECTOR_HEADER Structure (section 2.2.2.6) is present.	
        ///
        RDPUDP_FLAG_ACK_OF_ACKS = 0x0100,

        ///
        /// Specifies that the connection does not require persistent retransmits.	
        ///
        RDPUDP_FLAG_SYNLOSSY = 0x0200,

        /// <summary>
        /// Specifies that the receiver delayed generating the ACK for the source sequence numbers received. 
        /// The sender should not use this ACK for estimating the network RTT.
        /// </summary>
        RDPUDP_FLAG_ACKDELAYED = 0x0400,
        
        /// <summary>
        /// Specifies that the RDPUDP_CORRELATION_ID_PAYLOAD Structure (section 2.2.2.8) is present.
        /// </summary>
        RDPUDP_FLAG_CORRELATION_ID = 0x0800,

        /// <summary>
        /// Specifies that the optional RDPUDP_SYNDATAEX_PAYLOAD Structure (section 2.2.2.9) is present.
        /// </summary>
        RDPUDP_FLAG_SYNEX = 0x1000
    }

    /// <summary>
    /// 2.2.2.1   RDPUDP_FEC_HEADER Structure
    /// The RDPUDP_FEC_HEADER structure forms the basic header for every datagram sent or received by the endpoint. 
    /// </summary>
    public struct RDPUDP_FEC_HEADER
    {
        /// <summary>
        /// A 32-bit unsigned value that specifies the highest sequence number for a Source Packet detected by the remote endpoint. 
        /// This value wraps around; for more information about the sequence numbers range, see [RFC793] section 3.3.
        /// </summary>
        public uint snSourceAck;

        /// <summary>
        /// A 16-bit unsigned value that specifies the size of the receiver's buffer.
        /// </summary>
        public ushort uReceiveWindowSize;

        /// <summary>
        /// A 16-bit unsigned integer that indicates supported options, or additional headers.
        /// </summary>
        public RDPUDP_FLAG uFlags;
    }

    /// <summary>
    /// The RDPUDP_FEC_PAYLOAD_HEADER structure accompanies every datagram that contains an FEC payload.
    /// </summary>
    public struct RDPUDP_FEC_PAYLOAD_HEADER
    {
        /// <summary>
        /// A 32-bit unsigned value that contains the sequence number for a Coded Packet. 
        /// </summary>
        public uint snCoded;

        /// <summary>
        /// A 32-bit unsigned value that specifies the first sequence number of a Source Packet that is contained in the FEC payload.
        /// </summary>
        public uint snSourceStart;

        /// <summary>
        /// An unsigned 8-bit value that specifies the number of packets, from snSourceStart, that are contained in the FEC payload. 
        /// </summary>
        public byte uRange;

        /// <summary>
        /// A 8-bit unsigned value. This value is generated by the FEC engine.
        /// </summary>
        public byte uFecIndex;

        /// <summary>
        /// An array of UINT8 ([MS-DTYP] section 2.2.46). 
        /// </summary>
        public ushort uPadding;
    }

    /// <summary>
    /// The RDPUDP_PAYLOAD_PREFIX structure specifies the length of a data payload. 
    /// This header is used for generating an FEC Packet or for decoding an FEC Packet. 
    /// Once a datagram is decoded by using FEC, this field specifies the size of the recovered datagram.
    /// </summary>
    public struct RDPUDP_PAYLOAD_PREFIX
    {
        /// <summary>
        /// An unsigned 16-bit value that specifies the size of the data payload. 
        /// </summary>
        public ushort cbPayloadSize;
    }

    /// <summary>
    /// The RDPUDP_SOURCE_PAYLOAD_HEADER structure specifies the metadata of a data payload.
    /// </summary>
    public struct RDPUDP_SOURCE_PAYLOAD_HEADER
    {
        /// <summary>
        /// An unsigned 32-bit value that specifies the sequence number for the current  Coded Packet.
        /// </summary>
        public uint snCoded;

        /// <summary>
        /// An unsigned 32-bit value that specifies the sequence number for the current Source Packet. 
        /// </summary>
        public uint snSourceStart;
    }

    /// <summary>
    /// The RDPUDP_SYNDATA_PAYLOAD structure specifies the parameters that are used to initialize the UDP connection.
    /// </summary>
    public struct RDPUDP_SYNDATA_PAYLOAD
    {
        /// <summary>
        /// A 32-bit unsigned value that specifies the starting value for sequence numbers for Source Packets and Coded Packets. 
        /// </summary>
        public uint snInitialSequenceNumber;

        /// <summary>
        ///  A 16-bit unsigned value that specifies the maximum size for a datagram that can be generated by the endpoint. 
        ///  This value MUST be greater than or equal to 1132 and less than or equal to 1232.
        /// </summary>
        public ushort uUpStreamMtu;

        /// <summary>
        /// A 16-bit unsigned value that specifies the maximum size of the maximum transmission unit (MTU) that the endpoint can accept. 
        /// This value MUST be greater than or equal to 1132 and less than or equal to 1232.
        /// </summary>
        public ushort uDownStreamMtu;
    }

    /// <summary>
    /// The RDPUDP_ACK_OF_ACKVECTOR_HEADER structure resets the start position of an ACK vector (section 2.2.3.1).
    /// </summary>
    public struct RDPUDP_ACK_OF_ACKVECTOR_HEADER
    {
        /// <summary>
        /// This value specifies the new sequence number from which the ACK vector starts encoding the state of the receiver queue. 
        /// </summary>
        public uint snAckOfAcksSeqNum;
    }

    /// <summary>
    /// The RDPUDP_ACK_VECTOR_HEADER structure contains the ACK vector (section 2.2.3.1) that specifies the states of the datagram in the receiver’s queue. 
    /// This vector is a variable-size array. 
    /// The states are encoded by using run-length encoding (RLE) and are stored in this array.
    /// </summary>
    public struct RDPUDP_ACK_VECTOR_HEADER
    {
        /// <summary>
        /// A 16-bit unsigned value that contains the size of the AckVectorElement array. 
        /// </summary>
        public ushort uAckVectorSize;

        /// <summary>
        ///  An array of ACK Vector elements. Each element is composed of a state, and the number of contiguous datagrams that share the same state.
        /// </summary>
        public AckVector[] AckVectorElement;

        ///
        /// A variable-sized array, of length zero or more, such that this structure ends on a DWORD ([MS-DTYP] section 2.2.9) boundary.
        /// 
        public byte[] Padding;

    }

    /// <summary>
    /// The ACK vector captures the state of the queue of Source Packets at the receiver endpoint. 
    /// </summary>
    public struct AckVector
    {
        /// <summary>
        /// The two most significant bits of each element compose the VECTOR_ELEMENT_STATE enumeration (section 2.2.1.1).
        /// </summary>
        public VECTOR_ELEMENT_STATE State;

        /// <summary>
        /// The next 6 bits are the length of a continuous sequence of datagrams that share the same state.
        /// </summary>
        public byte Length;
    }

    /// <summary>
    /// The RDPUDP_CORRELATION_ID_PAYLOAD structure allows a terminal client to specify the correlation identifier for the connection, 
    /// which may appear in some of the terminal server’s event logs. Otherwise, the terminal server may generate a random identifier.
    /// </summary>
    public struct RDPUDP_CORRELATION_ID_PAYLOAD
    {
        /// <summary>
        /// DTYP.GUID. An array of 16 8-bit, unsigned integers that specifies a unique identifier to associate with the connection. 
        /// </summary>
        public byte[] uCorrelationId;

        /// <summary>
        /// 16 8-bit values, all set to 0x00
        /// </summary>
        public byte[] uReserved; 
    }

    public struct RDPUDP_SYNDATAEX_PAYLOAD
    {
        /// <summary>
        /// A 16-bit unsigned integer that indicates supported options.
        /// </summary>
        public uSynExFlags_Values uSynExFlags;

        /// <summary>
        /// A 16-bit unsigned value. When the RDPUDP_VERSION_INFO_VALID flag is present, 
        /// this specifies a supported version of the UDP Transport Extension, used to negotiate with the other endpoint.
        /// </summary>
        public uUdpVer_Values uUdpVer;
    }

    /// <summary>
    /// A 16-bit unsigned integer that indicates supported options. 
    /// </summary>
    [Flags]
    public enum uSynExFlags_Values : ushort
    {
        None = 0x0000,

        /// <summary>
        /// The uUdpVer field indicates a supported version of the RDP-UDP protocol.
        /// </summary>
        RDPUDP_VERSION_INFO_VALID = 0x0001
    }

    /// <summary>
    /// Supported Version flags value.
    /// </summary>
    public enum uUdpVer_Values : ushort
    {
        None = 0x0000,

        /// <summary>
        /// The minimum retransmit time-out is 500 ms (section 3.1.6.1), and the minimum delayed ACK time-out is 200 ms (section 3.1.6.3).<1>
        /// </summary>
        RDPUDP_PROTOCOL_VERSION_1 = 0x0001,

        /// <summary>
        /// The minimum retransmit time-out is 300 ms (section 3.1.6.1), and the minimum delayed ACK time-out is 50 ms (section 3.1.6.3).<2>
        /// </summary>
        RDPUDP_PROTOCOL_VERSION_2 = 0x0002
    }

    /// <summary>
    /// The base packet type for Rdpeudp.
    /// </summary>
    public class RdpeudpPacket : BasePDU
    {
        /// <summary>
        /// FEC Header
        /// </summary>
        public RDPUDP_FEC_HEADER fecHeader;

        /// <summary>
        /// SYN data
        /// </summary>
        public RDPUDP_SYNDATA_PAYLOAD? SynData;

        /// <summary>
        /// CorrelationId
        /// </summary>
        public RDPUDP_CORRELATION_ID_PAYLOAD? CorrelationId;

        /// <summary>
        /// SYNEX data
        /// </summary>
        public RDPUDP_SYNDATAEX_PAYLOAD? SynDataEx;

        /// <summary>
        /// RDPUDP_ACK_VECTOR_HEADER
        /// </summary>
        public RDPUDP_ACK_VECTOR_HEADER? ackVectorHeader;

        /// <summary>
        /// RDPUDP_ACK_OF_ACKVECTOR_HEADER
        /// </summary>
        public RDPUDP_ACK_OF_ACKVECTOR_HEADER? ackOfAckVector;

        /// <summary>
        /// RDPUDP_SOURCE_PAYLOAD_HEADER
        /// </summary>
        public RDPUDP_SOURCE_PAYLOAD_HEADER? sourceHeader;

        /// <summary>
        /// RDPUDP_FEC_PAYLOAD_HEADER
        /// </summary>
        public RDPUDP_FEC_PAYLOAD_HEADER? fecPayloadHeader;

        /// <summary>
        /// The source data payload or FEC payload.
        /// </summary>
        public byte[] payload;

        /// <summary>
        /// The pad bytes of SYN packet.
        /// </summary>
        public byte[] padding;

        #region constructor

        public RdpeudpPacket()
        {
            fecHeader = new RDPUDP_FEC_HEADER();
            SynData = null;
            ackVectorHeader = null;
            ackOfAckVector = null;
            sourceHeader = null;
            fecPayloadHeader = null;
            payload = null;
            padding = null;
        }
           
        #endregion constructor


        #region Encoding/Decoding members

        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt32(fecHeader.snSourceAck);
            marshaler.WriteUInt16(fecHeader.uReceiveWindowSize);
            marshaler.WriteUInt16((ushort)fecHeader.uFlags);

            if (SynData.HasValue)
            {
                marshaler.WriteUInt32(SynData.Value.snInitialSequenceNumber);
                marshaler.WriteUInt16(SynData.Value.uUpStreamMtu);
                marshaler.WriteUInt16(SynData.Value.uDownStreamMtu);
                // This datagram MUST be zero-padded to increase the size of this datagram to 1232 bytes.
                int length = 16;
                if (CorrelationId.HasValue)
                {
                    marshaler.WriteBytes(CorrelationId.Value.uCorrelationId);
                    length += 16;
                    marshaler.WriteBytes(CorrelationId.Value.uReserved);
                    length += 16;
                }

                if(SynDataEx.HasValue)
                {
                    marshaler.WriteUInt16((ushort)SynDataEx.Value.uSynExFlags);
                    marshaler.WriteUInt16((ushort)SynDataEx.Value.uUdpVer);
                    length += 4;
                }

                byte[] padBytes = new byte[1232 - length];
                marshaler.WriteBytes(padBytes);
                return;
            }

            if (ackVectorHeader.HasValue)
            {
                // ACK
                marshaler.WriteUInt16(ackVectorHeader.Value.uAckVectorSize);
                if (ackVectorHeader.Value.AckVectorElement != null)
                {
                    List<byte> ackVecElementList = new List<byte>();
                    foreach (AckVector vec in ackVectorHeader.Value.AckVectorElement)
                    {
                        byte vecByte = 0;
                        vecByte |= vec.Length;
                        byte state = (byte)vec.State;
                        vecByte |= (byte)(state << 6);
                        ackVecElementList.Add(vecByte);                        
                    }
                    ackVecElementList.Reverse();
                    marshaler.WriteBytes(ackVecElementList.ToArray());
                }

                // Padding (variable): A variable-sized array, of length zero or more, 
                // such that this structure ends on a DWORD ([MS-DTYP] section 2.2.9) boundary.
                int padLen = 4 - (2 + ackVectorHeader.Value.uAckVectorSize) % 4;

                if (padLen > 0 && padLen != 4)
                {
                    byte[] padding = new byte[padLen];
                    marshaler.WriteBytes(padding);
                }

                // Ack of Acks.
                if (ackOfAckVector.HasValue)
                {
                    marshaler.WriteUInt32(ackOfAckVector.Value.snAckOfAcksSeqNum);
                }
            }

            if (sourceHeader.HasValue || fecPayloadHeader.HasValue)
            {
                if (sourceHeader.HasValue)
                {
                    // Source Data.
                    marshaler.WriteUInt32(sourceHeader.Value.snCoded);
                    marshaler.WriteUInt32(sourceHeader.Value.snSourceStart);
                }
                else
                {
                    // FEC Data.
                    marshaler.WriteUInt32(fecPayloadHeader.Value.snCoded);
                    marshaler.WriteUInt32(fecPayloadHeader.Value.snSourceStart);
                    marshaler.WriteByte(fecPayloadHeader.Value.uRange);
                    marshaler.WriteByte(fecPayloadHeader.Value.uFecIndex);
                    marshaler.WriteUInt16(fecPayloadHeader.Value.uPadding);
                }

                if (payload != null)
                {
                    marshaler.WriteBytes(payload);
                }
            }
        }

        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                fecHeader.snSourceAck = marshaler.ReadUInt32();
                fecHeader.uReceiveWindowSize = marshaler.ReadUInt16();
                fecHeader.uFlags = (RDPUDP_FLAG)marshaler.ReadUInt16();

                if (fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_SYN))
                {
                    RDPUDP_SYNDATA_PAYLOAD synData = new RDPUDP_SYNDATA_PAYLOAD();
                    synData.snInitialSequenceNumber = marshaler.ReadUInt32();
                    synData.uUpStreamMtu = marshaler.ReadUInt16();
                    synData.uDownStreamMtu = marshaler.ReadUInt16();
                    // This datagram MUST be zero-padded to increase the size of this datagram to 1232 bytes.

                    if (fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_CORRELATION_ID))
                    {
                        RDPUDP_CORRELATION_ID_PAYLOAD correlationId = new RDPUDP_CORRELATION_ID_PAYLOAD();
                        correlationId.uCorrelationId = marshaler.ReadBytes(16);
                        this.CorrelationId = correlationId;
                        correlationId.uReserved = marshaler.ReadBytes(16);
                    }

                    if (fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_SYNEX))
                    {
                        RDPUDP_SYNDATAEX_PAYLOAD synDataEx = new RDPUDP_SYNDATAEX_PAYLOAD();
                        synDataEx.uSynExFlags = (uSynExFlags_Values)marshaler.ReadUInt16();
                        synDataEx.uUdpVer = (uUdpVer_Values)marshaler.ReadUInt16();
                        this.SynDataEx = synDataEx;
                    }

                    this.padding = marshaler.ReadToEnd();
                    this.SynData = synData;
                    return true;
                }

                
                if (fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_ACK))
                {
                    // ACK.
                    RDPUDP_ACK_VECTOR_HEADER ackVector = new RDPUDP_ACK_VECTOR_HEADER();
                    ackVector.uAckVectorSize = marshaler.ReadUInt16();
                    ackVector.AckVectorElement = new AckVector[ackVector.uAckVectorSize];

                    List<byte> ackVecElementList = new List<byte>(marshaler.ReadBytes(ackVector.AckVectorElement.Length));
                    ackVecElementList.Reverse();

                    for (int i = 0; i < ackVector.AckVectorElement.Length; i++)
                    {
                        byte vecByte = ackVecElementList[i];
                        ackVector.AckVectorElement[i].Length = (byte)(0x3F & vecByte);
                        ackVector.AckVectorElement[i].State = (VECTOR_ELEMENT_STATE)(vecByte >> 6);
                    }
                    
                    this.ackVectorHeader = ackVector;

                    // Padding (variable): A variable-sized array, of length zero or more, 
                    // such that this structure ends on a DWORD ([MS-DTYP] section 2.2.9) boundary.
                    int padLen = 4 - (2 + ackVector.AckVectorElement.Length) % 4;
                    if (padLen > 0 && padLen != 4)
                    {
                        this.padding = marshaler.ReadBytes(padLen);
                    }


                    // Ack of Acks.
                    if (fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_ACK_OF_ACKS))
                    {
                        RDPUDP_ACK_OF_ACKVECTOR_HEADER aoaHeader = new RDPUDP_ACK_OF_ACKVECTOR_HEADER();
                        aoaHeader.snAckOfAcksSeqNum = marshaler.ReadUInt32();
                        this.ackOfAckVector = aoaHeader;
                    }
                }

                if (fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_DATA))
                {
                    if (!fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_FEC))
                    {
                        // Source Data.
                        RDPUDP_SOURCE_PAYLOAD_HEADER srcHeader = new RDPUDP_SOURCE_PAYLOAD_HEADER();
                        srcHeader.snCoded = marshaler.ReadUInt32();
                        srcHeader.snSourceStart = marshaler.ReadUInt32();
                        this.sourceHeader = srcHeader;
                    }
                    else
                    {
                        // FEC Data.
                        RDPUDP_FEC_PAYLOAD_HEADER fecDataHeader = new RDPUDP_FEC_PAYLOAD_HEADER();
                        fecDataHeader.snCoded =  marshaler.ReadUInt32();
                        fecDataHeader.snSourceStart = marshaler.ReadUInt32();
                        fecDataHeader.uRange = marshaler.ReadByte();
                        fecDataHeader.uFecIndex = marshaler.ReadByte();
                        fecDataHeader.uPadding = marshaler.ReadUInt16();
                        this.fecPayloadHeader = fecDataHeader;
                    }

                    this.payload = marshaler.ReadToEnd();
                }

                return true;
            }
            catch
            {
                return false;
            }
        } 

        #endregion

        /// <summary>
        /// The short name of the RDPEUDP packet.
        /// </summary>
        /// <returns>A string contains the fields of this PDU.</returns>
        public override string ToString()
        {

            if (fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_SYN))
            {
                if (fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_ACK))
                {
                    return "SYN-ACK";
                }
                else
                {
                    return "SYN";
                }
            }
            else
            {
                string shortName = string.Empty;
                if (fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_ACK))
                {
                    // ACK.
                    shortName = "ACK";
                }

                if (fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_DATA))
                {
                    if (!fecHeader.uFlags.HasFlag(RDPUDP_FLAG.RDPUDP_FLAG_FEC))
                    {
                        // Source Data.
                        if (shortName == string.Empty)
                        {
                            shortName = "SOURCE";
                        }
                        else
                        {
                            shortName += "-SOURCE";
                        }
                    }
                    else
                    {
                        // FEC Data.
                        if (shortName == string.Empty)
                        {
                            shortName = "FEC";
                        }
                        else
                        {
                            shortName += "-FEC";
                        }
                    }
                }

                if (shortName != string.Empty) return shortName;
                return "UNKOWN";
            }
        }
    }

    /// <summary>
    /// The Base Packet which contains the raw data.
    /// </summary>
    public class RdpeudpBasePacket : StackPacket
    {

        public RdpeudpBasePacket(byte[] data)
            : base(data)
        {
        }

        public override StackPacket Clone()
        {
            if (this.PacketBytes != null)
            {
                byte[] data = new byte[this.PacketBytes.Length];
                Array.Copy(this.PacketBytes, data, this.PacketBytes.Length);
                return new RdpeudpBasePacket(data);
            }
            return null;
        }

        public override byte[] ToBytes()
        {
            if (this.PacketBytes != null)
            {
                byte[] data = new byte[this.PacketBytes.Length];
                Array.Copy(this.PacketBytes, data, this.PacketBytes.Length);
                return data;
            }
            return null;
        }

        /// <summary>
        /// Callback function used for TransportStack
        /// UDP TransportStack will call this function when received bytes from UDP transport
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="messageBytes"></param>
        /// <param name="consumedLength"></param>
        /// <param name="expectedLength"></param>
        /// <returns></returns>
        public static StackPacket[] DecodePacketCallback(object endPoint, byte[] messageBytes, out int consumedLength, out int expectedLength)
        {
            RdpeudpBasePacket[] packets = new RdpeudpBasePacket[1];
            RdpeudpBasePacket basePacket = new RdpeudpBasePacket(messageBytes);
            consumedLength = messageBytes.Length;
            expectedLength = messageBytes.Length;
            packets[0] = basePacket;
            return packets;
        }
    }
}

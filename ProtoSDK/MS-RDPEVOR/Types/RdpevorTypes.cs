// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpevor
{
    #region Base PDUs

    /// <summary>
    /// The base pdu of all MS-RDPEVOR messages.
    /// </summary>
    public class RdpevorPdu : BasePDU
    {
        public TSMM_VIDEO_PACKET_HEADER Header;

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt32(Header.cbSize);
            marshaler.WriteUInt32((uint)Header.PacketType);
        }

        /// <summary>
        /// Decode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to Decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                Header.cbSize = marshaler.ReadUInt32();
                Header.PacketType = (PacketTypeValues)marshaler.ReadUInt32();
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
    /// The base pdu of all MS-RDPEVOR messages sent by server.
    /// </summary>
    public class RdpevorServerPdu : RdpevorPdu
    {
        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            base.Encode(marshaler) ;
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
                return (this.Header.PacketType == PacketTypeValues.TSMM_PACKET_TYPE_PRESENTATION_REQUEST || this.Header.PacketType == PacketTypeValues.TSMM_PACKET_TYPE_VIDEO_DATA);
            }
            else
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
    }

    /// <summary>
    ///  The base pdu of all MS-RDPEVOR messages sent by client.
    /// </summary>
    public class RdpevorClientPdu : RdpevorPdu
    {
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
                return (this.Header.PacketType == PacketTypeValues.TSMM_PACKET_TYPE_CLIENT_NOTIFICATION || this.Header.PacketType == PacketTypeValues.TSMM_PACKET_TYPE_PRESENTATION_RESPONSE);
            }
            else
            {
                marshaler.Reset();
                throw new PDUDecodeException(this.GetType(), marshaler.ReadToEnd());
            }
        }
    }

    /// <summary>
    /// The unknown type.
    /// </summary>
    public class RdpevorUnkownPdu : RdpevorPdu
    {
        public byte[] Data;

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

    }

    #endregion

    #region Common Structures

    /// <summary>
    /// This message is meant to be a header on all other messages and MUST NOT be sent alone.
    /// </summary>
    public struct TSMM_VIDEO_PACKET_HEADER
    {
        /// <summary>
        /// UINT32. Length, in bytes, of the entire message following and including this header.
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// UINT32. The value of this integer indicates the type of message following this header. The following table defines valid values.
        /// </summary>
        public PacketTypeValues PacketType;
    }

    /// <summary>
    /// This structure is appended to a TSMM_CLIENT_NOTIFICATION in the pData field.
    /// </summary>
    public struct TSMM_CLIENT_NOTIFICATION_FRAMERATE_OVERRIDE
    {
        /// <summary>
        /// UINT32. A number that identifies which operation to execute on the server.  This number is a bitmask.
        /// </summary>
        public FrameRateOverride_FlagsValues Flags;

        /// <summary>
        /// UINT32. If Flags contains 0x1 – Override frame rate, this value MUST be set to the desired rate at which the server will deliver samples.  
        /// This value MUST be in the range of 1 to 30.
        /// </summary>
        public uint DesiredFrameRate;
    } 

    #endregion

    #region Server Messages

    /// <summary>
    /// The TSMM_PRESENTATION_REQUEST message is sent from the server to the client to indicate that a video stream is either starting or stopping.
    /// </summary>
    public class TSMM_PRESENTATION_REQUEST : RdpevorServerPdu
    {
        #region Message Fields
        /// <summary>
        /// UINT8. A number that uniquely identifies the video stream on the server.  
        /// The server MUST ensure that presentation IDs are unique across all active presentations.
        /// </summary>
        public byte PresentatioinId;

        /// <summary>
        /// UINT8. The current version of the Remote Desktop Protocol: Video Optimized Remoting Virtual Channel Extension. 
        /// </summary>
        public RdpevorVersionValues Version;

        /// <summary>
        /// A number that identifies which operation the client is to perform. 
        /// </summary>
        public CommandValues Command;

        /// <summary>
        /// UINT8. This field is reserved and MUST be ignored.
        /// </summary>
        public byte FrameRate;

        /// <summary>
        /// UINT16. This field is reserved and MUST be ignored.
        /// </summary>
        public ushort AverageBitrateKbps;

        /// <summary>
        /// UINT16. This field is reserved and MUST be ignored.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// UINT32. This is the width of the video stream after scaling back to the original resolution.s
        /// </summary>
        public uint SourceWidth;

        /// <summary>
        /// UINT32. This is the height of the video stream after scaling back to the original resolution.
        /// </summary>
        public uint SourceHeight;

        /// <summary>
        /// UINT32. This is the width of the video stream.
        /// </summary>
        public uint ScaledWidth;

        /// <summary>
        /// UINT32. This is the height of the video stream.
        /// </summary>
        public uint ScaledHeight;

        /// <summary>
        /// UINT64. The time on the server (in 100-ns intervals since the system was started) when the video presentation was started.
        /// </summary>
        public UInt64 hnsTimestampOffset;

        /// <summary>
        /// UINT64. This field is used to correlate this video data with its geometry, which is sent on another channel.  See [MS-RDPEGT] for more details.
        /// </summary>
        public UInt64 GeometryMappingId;

        /// <summary>
        /// Guid Type. This field identifies the Media Foundation video subtype of the video stream.
        /// </summary>
        public byte[] VideoSubtypeId;

        /// <summary>
        /// Length of extra data (in bytes) appended to this structure, starting at pExtraData.
        /// </summary>
        public uint cbExtra;

        /// <summary>
        /// Array of UINT8. The data in this field depends on the format of the video indicated in the VideoSubtypeId field. 
        /// For the case when the video subtype is MFVideoFormat_H264, this field should be set to the MPEG-1 
        /// or MPEG-2 sequence header data, which, for the Microsoft implementation of the H.264 encoder, 
        /// can be found by querying the MF_MT_MPEG_SEQUENCE_HEADER attribute of the video media type after setting it as the encoder output.
        /// </summary>
        public byte[] pExtraData;

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
            marshaler.WriteUInt32(this.Header.cbSize);
            marshaler.WriteUInt32((uint)this.Header.PacketType);
            marshaler.WriteByte(this.PresentatioinId);
            marshaler.WriteByte((byte)this.Version);
            marshaler.WriteByte((byte)this.Command);
            marshaler.WriteByte(this.FrameRate);
            marshaler.WriteUInt16(this.AverageBitrateKbps);
            marshaler.WriteUInt16(this.Reserved);
            marshaler.WriteUInt32(this.SourceWidth);
            marshaler.WriteUInt32(this.SourceHeight);
            marshaler.WriteUInt32(this.ScaledWidth);
            marshaler.WriteUInt32(this.ScaledHeight);
            marshaler.WriteUInt64(this.hnsTimestampOffset);
            marshaler.WriteUInt64(this.GeometryMappingId);
            marshaler.WriteBytes(this.VideoSubtypeId, 0, 16);
            marshaler.WriteUInt32(this.cbExtra);
            if (this.pExtraData != null)
            {
                marshaler.WriteBytes(this.pExtraData);
            }
            marshaler.WriteByte(this.Reserved2);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                this.Header.cbSize = marshaler.ReadUInt32();
                this.Header.PacketType = (PacketTypeValues)marshaler.ReadUInt32();
                this.PresentatioinId = marshaler.ReadByte();
                this.Version = (RdpevorVersionValues)marshaler.ReadByte();
                this.Command = (CommandValues)marshaler.ReadByte();
                this.FrameRate = marshaler.ReadByte();
                this.AverageBitrateKbps = marshaler.ReadUInt16();
                this.Reserved = marshaler.ReadUInt16();
                this.SourceWidth = marshaler.ReadUInt32();
                this.SourceHeight = marshaler.ReadUInt32();
                this.ScaledWidth = marshaler.ReadUInt32();
                this.ScaledHeight = marshaler.ReadUInt32();
                this.hnsTimestampOffset = marshaler.ReadUInt64();
                this.GeometryMappingId = marshaler.ReadUInt64();
                this.VideoSubtypeId = marshaler.ReadBytes(16);
                this.cbExtra = marshaler.ReadUInt32();
                this.pExtraData = marshaler.ReadBytes((int)this.cbExtra);
                this.Reserved2 = marshaler.ReadByte();
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
    /// This message contains a potentially fragmented video sample. 
    /// </summary>
    public class TSMM_VIDEO_DATA : RdpevorServerPdu
    {
        #region Message Fields
        /// <summary>
        /// UINT8. This corresponds to a PresentationId of an earlier TSMM_PRESENTATION_REQUEST message.
        /// </summary>
        public byte PresentatioinId;

        /// <summary>
        /// UINT8. The current version of the Remote Desktop Protocol: Video Optimized Remoting Virtual Channel Extension. 
        /// </summary>
        public RdpevorVersionValues Version;

        /// <summary>
        /// UINT8. The bits of this integer indicate attributes of this message. 
        /// </summary>
        public TsmmVideoData_FlagsValues Flags;

        /// <summary>
        /// UINT8. This field is reserved and MUST be ignored.
        /// </summary>
        public byte Reserved;

        /// <summary>
        /// UINT64. Timestamp of the current packet, in 100-ns intervals since the video presentation was started.
        /// </summary>
        public UInt64 HnsTimestamp;

        /// <summary>
        /// UINT64. Duration of the current packet, in 100-ns intervals.  This is the length of time between the last sample and the current sample.
        /// </summary>
        public UInt64 HnsDuration;

        /// <summary>
        /// UINT16. Each sample (logically one contiguous frame) is divided into packets for network transmission as atomic units.  
        /// This field contains the index of the current packet within the larger sample.  
        /// This field is indexed starting with 1 and increases until it is equal to the value in the PacketsInSample field.
        /// </summary>
        public ushort CurrentPacketIndex;

        /// <summary>
        /// UINT16. This field contains the number of packets that make up the current sample.
        /// </summary>
        public ushort PacketsInSample;

        /// <summary>
        /// UINT32. This field contains the current sample number.  The first sample will have this field set to 1.
        /// </summary>
        public uint SampleNumber;

        /// <summary>
        /// UINT32. Length (in bytes) of the pSample field.
        /// </summary>
        public uint cbSample;

        /// <summary>
        /// Array of UINT8. Encoded sample data.
        /// </summary>
        public byte[] pSample;

        /// <summary>
        /// UINT8. This field is reserved and should be 0.
        /// </summary>
        public byte Reserved2;
        #endregion

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt32(this.Header.cbSize);
            marshaler.WriteUInt32((uint)this.Header.PacketType);
            marshaler.WriteByte(this.PresentatioinId);
            marshaler.WriteByte((byte)this.Version);
            marshaler.WriteByte((byte)this.Flags);
            marshaler.WriteByte(this.Reserved);
            marshaler.WriteUInt64(this.HnsTimestamp);
            marshaler.WriteUInt64(this.HnsDuration);
            marshaler.WriteUInt16(this.CurrentPacketIndex);
            marshaler.WriteUInt16(this.PacketsInSample);
            marshaler.WriteUInt32(this.SampleNumber);
            marshaler.WriteUInt32(this.cbSample);
            if (this.pSample != null)
            {
                marshaler.WriteBytes(this.pSample);
            }
            marshaler.WriteByte(this.Reserved2);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                this.Header.cbSize = marshaler.ReadUInt32();
                this.Header.PacketType = (PacketTypeValues)marshaler.ReadUInt32();
                this.PresentatioinId = marshaler.ReadByte();
                this.Version = (RdpevorVersionValues)marshaler.ReadByte();
                this.Flags = (TsmmVideoData_FlagsValues)marshaler.ReadByte();
                this.Reserved = marshaler.ReadByte();
                this.HnsTimestamp = marshaler.ReadUInt64();
                this.HnsDuration = marshaler.ReadUInt64();
                this.CurrentPacketIndex = marshaler.ReadUInt16();
                this.PacketsInSample = marshaler.ReadUInt16();
                this.SampleNumber = marshaler.ReadUInt32();
                this.cbSample = marshaler.ReadUInt32();
                this.pSample = marshaler.ReadBytes((int)this.cbSample);
                this.Reserved2 = marshaler.ReadByte();
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

    #region Client Messages

    /// <summary>
    /// This message is sent from the client to the server in response to a TSMM_PRESENTATION_REQUEST message with the Command field set to 0x01 (Start Presentation). 
    /// </summary>
    public class TSMM_PRESENTATION_RESPONSE : RdpevorClientPdu
    {
        /// <summary>
        /// UINT8. This corresponds to a PresentationId of an earlier TSMM_PRESENTATION_REQUEST message.
        /// </summary>
        public byte PresentatioinId;

        /// <summary>
        /// UINT8. This field is reserved and MUST be set to 0.
        /// </summary>
        public byte ResponseFlags;

        /// <summary>
        /// UINT16. This field is reserved and MUST be set to 0.
        /// </summary>
        public ushort ResultFlags;

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt32(this.Header.cbSize);
            marshaler.WriteUInt32((uint)this.Header.PacketType);
            marshaler.WriteByte(this.PresentatioinId);
            marshaler.WriteByte(this.ResponseFlags);
            marshaler.WriteUInt16(this.ResultFlags);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                this.Header.cbSize = marshaler.ReadUInt32();
                this.Header.PacketType = (PacketTypeValues)marshaler.ReadUInt32();
                this.PresentatioinId = marshaler.ReadByte();
                this.ResponseFlags = marshaler.ReadByte();
                this.ResultFlags = marshaler.ReadUInt16();
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
    /// This message is sent from the client to the server to notify of certain events happening on the client.
    /// </summary>
    public class TSMM_CLIENT_NOTIFICATION : RdpevorClientPdu
    {
        /// <summary>
        /// UINT8. This corresponds to a PresentationId of an earlier TSMM_PRESENTATION_REQUEST message.
        /// </summary>
        public byte PresentatioinId;

        /// <summary>
        /// UINT8. A number that identifies which notification type the client is sending.
        /// </summary>
        public NotificationTypeValues NotificationType;

        /// <summary>
        /// UINT16. This field is reserved and MUST be ignored.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// UINT32. Length of extra data (in bytes) appended to this structure, starting at pData.
        /// </summary>
        public uint cbData;

        /// <summary>
        /// Array of UINT8. The data in the field is dependent on the value of the NotificationType field.
        /// </summary>
        public byte[] pData;

        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override void Encode(PduMarshaler marshaler)
        {
            marshaler.WriteUInt32(this.Header.cbSize);
            marshaler.WriteUInt32((uint)this.Header.PacketType);
            marshaler.WriteByte(this.PresentatioinId);
            marshaler.WriteByte((byte)this.NotificationType);
            marshaler.WriteUInt16(this.Reserved);
            marshaler.WriteUInt32(this.cbData);
            marshaler.WriteBytes(this.pData);
        }

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public override bool Decode(PduMarshaler marshaler)
        {
            try
            {
                this.Header.cbSize = marshaler.ReadUInt32();
                this.Header.PacketType = (PacketTypeValues)marshaler.ReadUInt32();
                this.PresentatioinId = marshaler.ReadByte();
                this.NotificationType = (NotificationTypeValues)marshaler.ReadByte();
                this.Reserved = marshaler.ReadUInt16();
                this.cbData = marshaler.ReadUInt32();
                this.pData = marshaler.ReadToEnd();
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
}

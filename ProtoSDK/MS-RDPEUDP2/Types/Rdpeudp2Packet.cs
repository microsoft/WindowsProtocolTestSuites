// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2.Types
{
    /// <summary>
    /// The base packet type for RDPEUDP2.
    /// </summary>
    public class Rdpeudp2Packet
    {
        public Rdpeudp2PacketHeader Header;

        public AcknowledgementPayload? ACK;

        public OverheadSizePayload? OverheadSize;

        public DelayAckInfoPayload? DelayAckInfo;

        public AckOfAcksPayload? AckOfAcks;

        public DataHeaderPayload? DataHeader;

        public AcknowledgementVectorPayload? ACKVEC;

        public DataBodyPayload? DataBody;

        private bool isDummyPacket = false;

        public bool IsDummyPacket => isDummyPacket;

        /// <summary>
        /// Initialize a new instance of Rdpeudp2Packet.
        /// </summary>
        /// <param name="isDummyPacket">Whether the packet is a dummy packet.</param>
        public Rdpeudp2Packet(bool isDummyPacket = false)
        {
            Header = new Rdpeudp2PacketHeader();
            ACK = null;
            OverheadSize = null;
            DelayAckInfo = null;
            AckOfAcks = null;
            DataHeader = null;
            ACKVEC = null;
            DataBody = null;

            this.isDummyPacket = isDummyPacket;
        }

        /// <summary>
        /// Initialize a new instance of Rdpeudp2Packet from the on-wire version RDPEUDP2 packet bytes.
        /// </summary>
        /// <param name="data">The on-wire version RDPEUDP2 packet bytes.</param>
        /// <param name="consumedLength">The consumed bytes length to construct the Rdpeudp2Packet instance.</param>
        public Rdpeudp2Packet(byte[] data, out int consumedLength)
        {
            var prefixByteOnWire = data[7];
            var prefixByte = new PacketPrefixByte(new byte[] { prefixByteOnWire }, out consumedLength);
            if (prefixByte.Packet_Type_Index == 8)
            {
                isDummyPacket = true;
            }
            data[7] = data[0];

            var header = new Rdpeudp2PacketHeader(data[consumedLength..], out var payloadConsumedLength);
            consumedLength += payloadConsumedLength;
            Header = header;

            if (Header.Flags.HasFlag(Rdpeudp2PacketHeaderFlags.ACK))
            {
                var ack = new AcknowledgementPayload(data[consumedLength..], out payloadConsumedLength);
                consumedLength += payloadConsumedLength;
                ACK = ack;
            }

            if (Header.Flags.HasFlag(Rdpeudp2PacketHeaderFlags.OVERHEADSIZE))
            {
                var overheadSize = new OverheadSizePayload(data[consumedLength..], out payloadConsumedLength);
                consumedLength += payloadConsumedLength;
                OverheadSize = overheadSize;
            }

            if (Header.Flags.HasFlag(Rdpeudp2PacketHeaderFlags.DELAYACKINFO))
            {
                var delayAckInfo = new DelayAckInfoPayload(data[consumedLength..], out payloadConsumedLength);
                consumedLength += payloadConsumedLength;
                DelayAckInfo = delayAckInfo;
            }

            if (Header.Flags.HasFlag(Rdpeudp2PacketHeaderFlags.AOA))
            {
                var aoa = new AckOfAcksPayload(data[consumedLength..], out payloadConsumedLength);
                consumedLength += payloadConsumedLength;
                AckOfAcks = aoa;
            }

            if (Header.Flags.HasFlag(Rdpeudp2PacketHeaderFlags.DATA))
            {
                var dataHeader = new DataHeaderPayload(data[consumedLength..], out payloadConsumedLength);
                consumedLength += payloadConsumedLength;
                DataHeader = dataHeader;
            }

            if (Header.Flags.HasFlag(Rdpeudp2PacketHeaderFlags.ACKVEC))
            {
                var ackVec = new AcknowledgementVectorPayload(data[consumedLength..], out payloadConsumedLength);
                consumedLength += payloadConsumedLength;
                ACKVEC = ackVec;
            }

            if (Header.Flags.HasFlag(Rdpeudp2PacketHeaderFlags.DATA))
            {
                var dataBody = new DataBodyPayload(data[consumedLength..], out payloadConsumedLength);
                consumedLength += payloadConsumedLength;
                DataBody = dataBody;
            }
        }

        /// <summary>
        /// Get the on-wire version packet bytes of this RDPEUDP2 packet. 
        /// </summary>
        /// <returns>On-wire version packet bytes.</returns>
        public byte[] ToBytes()
        {
            var bytes = new List<byte>(new byte[] { 0 }); // Reserve the first position to insert the prefix byte.

            bytes.AddRange(Header.ToBytes());

            if (ACK.HasValue)
            {
                bytes.AddRange(ACK.Value.ToBytes());
            }

            if (OverheadSize.HasValue)
            {
                bytes.AddRange(OverheadSize.Value.ToBytes());
            }

            if (DelayAckInfo.HasValue)
            {
                bytes.AddRange(DelayAckInfo.Value.ToBytes());
            }

            if (AckOfAcks.HasValue)
            {
                bytes.AddRange(AckOfAcks.Value.ToBytes());
            }

            if (DataHeader.HasValue)
            {
                bytes.AddRange(DataHeader.Value.ToBytes());
            }

            if (ACKVEC.HasValue)
            {
                bytes.AddRange(ACKVEC.Value.ToBytes());
            }

            if (DataBody.HasValue)
            {
                bytes.AddRange(DataBody.Value.ToBytes());
            }

            var prefixByte = new PacketPrefixByte();
            prefixByte.Reserved = 0;
            prefixByte.Packet_Type_Index = (byte)(isDummyPacket ? 8 : 0);
            prefixByte.Short_Packet_Length = (byte)(bytes.Count - 1 < 7 ? bytes.Count - 1 : 7);

            // If the size of the RDP-UDP2 Packet Layout is less than 7 bytes, then it MUST be padded to be of length of 7 bytes
            if (prefixByte.Short_Packet_Length < 7)
            {
                bytes.AddRange(new byte[7 - prefixByte.Short_Packet_Length]);
            }

            var prefixByteOnWire = prefixByte.ToBytes().First();
            bytes[0] = bytes[7];
            bytes[7] = prefixByteOnWire;

            return bytes.ToArray();
        }

        /// <summary>
        /// The short name of the RDPEUDP2 packet.
        /// </summary>
        /// <returns>A string contains the fields of this PDU.</returns>
        public override string ToString()
        {
            var summary = new StringBuilder();
            summary.Append((isDummyPacket ? "Dummy Packet, " : "Packet, ") + Enum.Format(typeof(Rdpeudp2PacketHeaderFlags), Header.Flags, "F").Replace(", ", "|"));

            if (DataHeader.HasValue)
            {
                summary.Append($", DataSeqNum: {DataHeader.Value.DataSeqNum}");
            }

            if (!isDummyPacket && DataBody.HasValue)
            {
                summary.Append($", ChannelSeqNum: {DataBody.Value.ChannelSeqNum}");
            }

            if (AckOfAcks.HasValue)
            {
                summary.Append($", {AckOfAcks.Value}");
            }

            if (ACK.HasValue)
            {
                summary.Append($", {ACK.Value}");
            }

            if (ACKVEC.HasValue)
            {
                summary.Append($", {ACKVEC.Value}");
            }

            return summary.ToString();
        }
    }
}
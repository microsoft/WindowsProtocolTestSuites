// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// CompoundPacket is composed by several single packet
    /// </summary>
    public class Smb2CompoundPacket : Smb2CompressiblePacket
    {
        private List<Smb2SinglePacket> packets;
        internal Smb2Decoder decoder;

        /// <summary>
        /// The packets the compoundPacket contains
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<Smb2SinglePacket> Packets
        {
            get
            {
                return packets;
            }
            set
            {
                packets = value;
            }
        }

        /// <summary>
        /// Convert the object to a byte array
        /// </summary>
        /// <returns>The converted byte array</returns>
        public override byte[] ToBytes()
        {
            if (Packets == null)
            {
                return null;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                for (int i = 0; i < Packets.Count; i++)
                {
                    byte[] temp;
                    temp = Packets[i].ToBytes();
                    ms.Write(temp, 0, temp.Length);
                }
                return ms.ToArray();
            }
        }


        /// <summary>
        /// Build a Smb2Packet from a byte array
        /// </summary>
        /// <param name="data">The byte array</param>
        /// <param name="consumedLen">The consumed data length</param>
        /// <param name="expectedLen">The expected data length</param>
        /// <returns>The Smb2Packet</returns>
        internal override void FromBytes(byte[] data, out int consumedLen, out int expectedLen)
        {
            this.Packets = new List<Smb2SinglePacket>();

            Packet_Header header = TypeMarshal.ToStruct<Packet_Header>(data);

            int innerConsumedLen = 0;
            int innerExpectedLen = 0;

            consumedLen = 0;

            byte[] temp = null;
            Smb2SinglePacket singlePacket = null;

            while (header.NextCommand != 0)
            {
                temp = new byte[header.NextCommand];

                Array.Copy(data, consumedLen, temp, 0, temp.Length);
                singlePacket = decoder.DecodeSinglePacket(
                    temp,
                    decoder.DecodeRole,
                    GetRealSessionId(header),
                    GetRealTreeId(header),
                    out innerConsumedLen,
                    out innerExpectedLen
                    ) as Smb2SinglePacket;
                singlePacket.OuterCompoundPacket = this;
                singlePacket.IsInCompoundPacket = true;
                singlePacket.IsLast = false;

                Packets.Add(singlePacket);

                //If a packet is in compound packet, there may be some padding at the end,
                //here we do not rely on the innerConsumedLen but header.NextCommand
                consumedLen += temp.Length;

                header = TypeMarshal.ToStruct<Packet_Header>(ArrayUtility.SubArray(data, consumedLen));
            }

            temp = new byte[data.Length - consumedLen];
            Array.Copy(data, consumedLen, temp, 0, temp.Length);

            singlePacket = decoder.DecodeSinglePacket(
                temp,
                decoder.DecodeRole,
                GetRealSessionId(header),
                GetRealTreeId(header),
                out innerConsumedLen,
                out innerExpectedLen
                ) as Smb2SinglePacket;

            singlePacket.OuterCompoundPacket = this;
            singlePacket.IsInCompoundPacket = true;
            singlePacket.IsLast = true;

            Packets.Add(singlePacket);

            consumedLen += innerConsumedLen;
            expectedLen = 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("COMPOUND packet");

            foreach (var packet in Packets)
            {
                sb.AppendLine();
                sb.Append(packet.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Get the real sessionId granted by server.
        /// </summary>
        /// <param name="header">The header of packet</param>
        /// <returns>The real sessionId</returns>
        private ulong GetRealSessionId(Packet_Header header)
        {
            ulong sessionId = header.SessionId;

            if ((header.Flags & Packet_Header_Flags_Values.FLAGS_RELATED_OPERATIONS)
                == Packet_Header_Flags_Values.FLAGS_RELATED_OPERATIONS)
            {
                if (header.SessionId == ulong.MaxValue)
                {
                    for (int i = packets.Count - 1; i >= 0; i--)
                    {
                        if (packets[i].Header.SessionId != ulong.MaxValue)
                        {
                            sessionId = packets[i].Header.SessionId;
                            break;
                        }
                    }
                }
            }

            return sessionId;
        }


        /// <summary>
        /// Get the real treeId granted by server.
        /// </summary>
        /// <param name="header">The header of packet</param>
        /// <returns>The real treeId</returns>
        private uint GetRealTreeId(Packet_Header header)
        {
            uint treeId = header.TreeId;

            if ((header.Flags & Packet_Header_Flags_Values.FLAGS_ASYNC_COMMAND)
                == Packet_Header_Flags_Values.FLAGS_ASYNC_COMMAND)
            {
                treeId = GetTreeIdFromAsyncPacket(header);
            }

            if ((header.Flags & Packet_Header_Flags_Values.FLAGS_RELATED_OPERATIONS)
                == Packet_Header_Flags_Values.FLAGS_RELATED_OPERATIONS)
            {
                if (treeId == uint.MaxValue)
                {
                    for (int i = packets.Count - 1; i >= 0; i--)
                    {
                        if ((packets[i].Header.Flags & Packet_Header_Flags_Values.FLAGS_ASYNC_COMMAND)
                            == Packet_Header_Flags_Values.FLAGS_ASYNC_COMMAND)
                        {
                            treeId = GetTreeIdFromAsyncPacket(packets[i].Header);
                        }
                        else
                        {
                            treeId = packets[i].Header.TreeId;
                        }

                        //0xffffffff is not a real treeId
                        //keep searching the real treeId if it 
                        //is oxffffffff
                        if (treeId != uint.MaxValue)
                        {
                            break;
                        }
                    }
                }
            }

            return treeId;
        }


        /// <summary>
        /// Get treeId from the async packet
        /// </summary>
        /// <param name="header">The packet header</param>
        /// <returns>The treeId</returns>
        private uint GetTreeIdFromAsyncPacket(Packet_Header header)
        {
            throw new NotImplementedException();
        }

    }
}

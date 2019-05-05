// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Linq;
using System.Runtime.InteropServices;
namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 TREE_DISCONNECT Request packet is sent by the client to request that the tree connect
    /// that is specified in the TreeId within the SMB2 header be disconnected
    /// </summary>
    public class Smb2TreeConnectRequestPacket : Smb2StandardPacket<TREE_CONNECT_Request>, IPacketBuffer
    {
        public Smb2TreeConnectRequestPacket()
        {
            // The fixed length fields are 8 bytes in total, plus 1 representing the variable length buffer
            PayLoad.StructureSize = 9;
        }

        /// <summary>
        /// The offset of buffer starting from the begining of the packet.
        /// </summary>
        public ushort BufferOffset
        {
            // 64 bytes of packet header and 8 bytes of fixed length fields
            get { return 64 + 8; }
        }

        public uint BufferLength
        {
            get { return PayLoad.PathLength; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string path;
            if (this.PayLoad.PathOffset - this.BufferOffset != 0)   // Path is in TreeConnectContext
            {
                var buffer = this.Buffer.Skip(this.PayLoad.PathOffset - this.BufferOffset).ToArray();
                path = Encoding.Unicode.GetString(buffer.Take(this.PayLoad.PathLength).ToArray());
            }
            else
            {
                // Path is in Buffer
                path = Encoding.Unicode.GetString(this.Buffer);
            }

            sb.Append("C TREE CONNECT");
            sb.Append(", Path=" + path);
            return sb.ToString();
        }

        /// <summary>
        /// Covert Smb2TreeConnectRequestPacket to a byte array 
        /// </summary>
        /// <returns>The byte array</returns>
        public override byte[] ToBytes()
        {
            return ArrayUtility.ConcatenateArrays(
                TypeMarshal.ToBytes(this.Header), 
                Smb2Utility.MarshalStructure(this.PayLoad),//based on .net marshal
                Buffer != null ? Buffer : new byte[0]);
        }

        /// <summary>
        /// Build a Smb2TreeConnectRequestPacket from a byte array
        /// </summary>
        /// <param name="data">The byte array</param>
        /// <param name="consumedLen">The consumed data length</param>
        /// <param name="expectedLen">The expected data length</param>
        internal override void FromBytes(byte[] data, out int consumedLen, out int expectedLen)
        {
            consumedLen = 0;
            this.Header = TypeMarshal.ToStruct<Packet_Header>(data, ref consumedLen);
            this.PayLoad = Smb2Utility.UnmarshalStructure<TREE_CONNECT_Request>(data.Skip(consumedLen).ToArray()); //based on .net unmarshal
            consumedLen += Marshal.SizeOf(typeof(TREE_CONNECT_Request));
            
            var packetBuffer = this as IPacketBuffer;            
            int bufferLength = (int)packetBuffer.BufferLength;
            this.Buffer = data.Skip(consumedLen).Take(bufferLength).ToArray();
            consumedLen += bufferLength;
            
            expectedLen = 0;
        }
    }
    
}

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// This is a genetic class which every smb2 packet will inherit
    /// </summary>
    /// <typeparam name="T">The payload type</typeparam>
    public abstract class Smb2StandardPacket<T> : Smb2SinglePacket where T : struct
    {
        /// <summary>
        /// The payload of the packet
        /// </summary>
        public T PayLoad;

        public byte[] Buffer = new byte[0];

        public Smb2StandardPacket()
        {
        }

        /// <summary>
        /// Covert to a byte array
        /// </summary>
        /// <returns>The byte array</returns>
        public override byte[] ToBytes()
        {
            return ArrayUtility.ConcatenateArrays(
                TypeMarshal.ToBytes(this.Header),
                TypeMarshal.ToBytes(this.PayLoad),
                Buffer != null ? Buffer : new byte[0],
                Padding);
        }


        /// <summary>
        /// Build a Smb2Packet from a byte array
        /// </summary>
        /// <param name="data">The byte array</param>
        /// <param name="consumedLen">The consumed data length</param>
        /// <param name="expectedLen">The expected data length</param>
        internal override void FromBytes(byte[] data, out int consumedLen, out int expectedLen)
        {
            consumedLen = 0;
            this.Header = TypeMarshal.ToStruct<Packet_Header>(data, ref consumedLen);
            this.PayLoad = TypeMarshal.ToStruct<T>(data, ref consumedLen);

            var packetBuffer = this as IPacketBuffer;
            if (packetBuffer != null)
            {
                int bufferLength = (int)packetBuffer.BufferLength;
                this.Buffer = data.Skip(consumedLen).Take(bufferLength).ToArray();
                consumedLen += bufferLength;
            }
            // The remained bytes are padding.
            this.Padding = new byte[data.Length - consumedLen];
            expectedLen = 0;
        }
    }
}

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The packet is sent by client to handle dialect and capability negotiation
    /// </summary>
    public class SmbNegotiateRequestPacket : Smb2Packet
    {
        public const byte DialectFormatCharactor = 0x02;

        /// <summary>
        /// The header of this packet
        /// </summary>
        public SmbHeader Header;

        /// <summary>
        /// The payload of this packet
        /// </summary>
        public SmbNegotiateRequest PayLoad;

        public SmbNegotiateRequestPacket()
        {
            // Smb packet signature: 0XFF, 'S', 'M', 'B'
            Header.Protocol = 0x424d53ff;
            Header.Command = (byte)SmbCommand.Negotiate;
        }

        /// <summary>
        /// Convert to a byte array
        /// </summary>
        /// <returns>The converted byte array</returns>
        public override byte[] ToBytes()
        {
            return ArrayUtility.ConcatenateArrays(
                TypeMarshal.ToBytes(this.Header),
                TypeMarshal.ToBytes(this.PayLoad));
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
            this.Header = TypeMarshal.ToStruct<SmbHeader>(data, ref consumedLen);
            this.PayLoad = TypeMarshal.ToStruct<SmbNegotiateRequest>(data, ref consumedLen);
            expectedLen = 0;
        }
    }
}

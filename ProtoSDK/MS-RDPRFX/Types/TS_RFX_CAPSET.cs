// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// The TS_RFX_CAPSET structure contains the capability information specific to the RemoteFX codec.
    /// </summary>
    public struct TS_RFX_CAPSET
    {
        /// <summary>
        /// A 16-bit, unsigned integer. Specifies the data block type.
        /// This field MUST be set to CBY_CAPSET (0xCBC1).
        /// </summary>
        public blockType_Value blockType;

        /// <summary>
        /// Specifies the combined size, in bytes, of the blockType, blockLen,
        /// codecId, capsetType, numIcaps, icapLen, and icapsData fields.
        /// </summary>
        public uint blockLen;

        /// <summary>
        /// Specifies the codec ID. This field MUST be set to 0x01.
        /// </summary>
        public byte codecId;

        /// <summary>
        /// This field MUST be set to CLY_CAPSET (0xCFC0).
        /// </summary>
        public ushort capsetType;

        /// <summary>
        /// The number of TS_RFX_ICAP structures contained in the icapsData field.
        /// </summary>
        public ushort numIcaps;

        /// <summary>
        /// Specifies the size, in bytes, of each TS_RFX_ICAP structure contained
        /// in the icapsData field.
        /// </summary>
        public ushort icapLen;

        /// <summary>
        /// A variable-length array of TS_RFX_ICAP (section 2.2.1.1.1.1.1) structures.
        /// Each structure MUST be packed on byte boundaries.
        /// The size of each TS_RFX_ICAP structure within the array is specified in the icapLen field.
        /// </summary>
        [Size("numIcaps")]
        public TS_RFX_ICAP[] icapsData;

        /// <summary>
        /// Marshals this struct to managed byte array.
        /// </summary>
        /// <returns>Marshalled managed byte array.</returns>
        public byte[] ToBytes()
        {
            // This field MUST be set to CBY_CAPSET (0xCBC1).
            blockType = blockType_Value.CBY_CAPSET;
            // This field MUST be set to 0x01.
            codecId = 1;
            // This field MUST be set to CLY_CAPSET (0xCFC0).
            capsetType = 0xCFC0;
            numIcaps = (ushort)(icapsData.Length);
            icapLen = (ushort)(icapsData[0].ToBytes().Length);
            // blockType, blockLen, codecId, capsetType, numIcaps and icapLen
            // fields take 13 bytes, the icapsData field takes icapLen*numIcaps bytes.
            blockLen = (uint)(13 + icapLen * numIcaps);

            byte[] buf = new byte[blockLen];
            using (BinaryWriter writer = new BinaryWriter(new MemoryStream(buf)))
            {
                writer.Write(TypeMarshal.ToBytes<ushort>((ushort)blockType));
                writer.Write(TypeMarshal.ToBytes<uint>(blockLen));
                writer.Write(TypeMarshal.ToBytes<byte>(codecId));
                writer.Write(TypeMarshal.ToBytes<ushort>(capsetType));
                writer.Write(TypeMarshal.ToBytes<ushort>(numIcaps));
                writer.Write(TypeMarshal.ToBytes<ushort>(icapLen));
                foreach (TS_RFX_ICAP icap in icapsData)
                {
                    writer.Write(icap.ToBytes());
                }
            }

            return buf;
        }
    }
}

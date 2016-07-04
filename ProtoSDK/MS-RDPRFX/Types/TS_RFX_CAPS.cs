// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// The TS_RFX_CAPS structure contains information about the encoder and decoder capabilities.
    /// </summary>
    public struct TS_RFX_CAPS
    {
        /// <summary>
        /// A 16-bit, unsigned integer. Specifies the data block type.
        /// </summary>
        public blockType_Value blockType;

        /// <summary>
        /// Specifies the combined size, in bytes, of the blockType,
        /// blockLen, and numCapsets fields.
        /// </summary>
        public uint blockLen;

        /// <summary>
        /// Specifies the number of TS_RFX_CAPSET (section 2.2.1.1.1.1)
        /// structures contained in the capsetsData field.
        /// </summary>
        public ushort numCapsets;

        /// <summary>
        /// A variable-sized array of TS_RFX_CAPSET (section 2.2.1.1.1.1) structures.
        /// </summary>
        [Size("numCapsets")]
        public TS_RFX_CAPSET[] capsetsData;

        /// <summary>
        /// Marshals this struct to managed byte array.
        /// </summary>
        /// <returns>Marshalled managed byte array.</returns>
        public byte[] ToBytes()
        {
            // This field MUST be set to CBY_CAPS (0xCBC0).
            blockType = blockType_Value.CBY_CAPS;
            // This field MUST be set to 0x0008.
            blockLen = 0x0008;
            numCapsets = (ushort)capsetsData.Length;

            // Convert each TS_RFX_CAPSET in capsetsData field to a byte array
            // and calculate total block length.
            byte[][] capsetsBytes = new byte[numCapsets][];
            uint totalBlockLen = blockLen;
            for (int i = 0; i < numCapsets; i++)
            {
                capsetsBytes[i] = capsetsData[i].ToBytes();
                totalBlockLen += (uint)capsetsBytes[i].Length;
            }

            // Marshal all bytes into a single buffer.
            byte[] buf = new byte[totalBlockLen];
            using (BinaryWriter writer = new BinaryWriter(new MemoryStream(buf)))
            {
                writer.Write(TypeMarshal.ToBytes<ushort>((ushort)blockType));
                writer.Write(TypeMarshal.ToBytes<uint>(blockLen));
                writer.Write(TypeMarshal.ToBytes<ushort>(numCapsets));
                foreach (byte[] capset in capsetsBytes)
                {
                    writer.Write(capset);
                }
            }

            return buf;
        }
    }
}

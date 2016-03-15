// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.IO;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx
{
    /// <summary>
    /// Represents the structure defined in section 2.2.1.1.
    /// This structure is the top-level capability container
    /// that wraps a TS_RFX_CAPS (section 2.2.1.1.1) structure.
    /// </summary>
    public struct TS_RFX_CLNT_CAPS_CONTAINER
    {
        /// <summary>
        /// Specifies the combined size, in bytes, of the length,
        /// captureFlags, capsLength, and capsData fields.
        /// </summary>
        public uint length;

        /// <summary>
        /// A collection of flags that allow a client to control how data is 
        /// captured and transmitted by the server.
        /// </summary>
        public uint captureFlags;

        /// <summary>
        /// Specifies the size, in bytes, of the capsData field.
        /// </summary>
        public uint capsLength;

        /// <summary>
        /// A nullable field that contains a TS_RFX_CAPS structure
        /// if it's not null.
        /// </summary>
        public TS_RFX_CAPS capsData;

        /// <summary>
        /// Marshals this struct to managed byte array.
        /// </summary>
        /// <returns>Marshalled managed byte array.</returns>
        public byte[] ToBytes()
        {
            byte[] capsDataBytes = capsData.ToBytes();
            capsLength = (uint)(capsDataBytes.Length);
            // The length, captureFlags and capsLength fields take 12 bytes,
            // the capsData field takes capsLength bytes.
            length = capsLength + 12;

            byte[] buf = new byte[length];
            using (BinaryWriter writer = new BinaryWriter(new MemoryStream(buf)))
            {
                writer.Write(TypeMarshal.ToBytes<uint>(length));
                writer.Write(TypeMarshal.ToBytes<uint>(captureFlags));
                writer.Write(TypeMarshal.ToBytes<uint>(capsLength));
                writer.Write(capsDataBytes);
            }

            return buf;
        }
    }
}

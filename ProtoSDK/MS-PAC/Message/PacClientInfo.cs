// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Pac
{
    /// <summary>
    ///  The PAC_CLIENT_INFO structure is a variable length header
    ///  of the PAC that contains the client's name and authentication
    ///  time. It is used to verify that the PAC corresponds
    ///  to the client of the ticket. The PAC_CLIENT_INFO structure
    ///  is placed directly after the Buffers array of the topmost
    ///  PACTYPE structure, at the offset specified in the Offset
    ///  field of the corresponding PAC_INFO_BUFFER structure
    ///  in the Buffers array. The UlType field of the corresponding
    ///  PAC_INFO_BUFFER is set to 0x0000000A.  
    ///  </summary>
    public class PacClientInfo : PacInfoBuffer
    {
        /// <summary>
        /// The native PAC_CLIENT_INFO object.
        /// </summary>
        public PAC_CLIENT_INFO NativePacClientInfo;


        /// <summary>
        /// Decode specified buffer from specified index, with specified count
        /// of bytes, into the instance of current class.
        /// </summary>
        /// <param name="buffer">The specified buffer.</param>
        /// <param name="index">The specified index from beginning of buffer.</param>
        /// <param name="count">The specified count of bytes to be decoded.</param>
        internal override void DecodeBuffer(byte[] buffer, int index, int count)
        {
            NativePacClientInfo =
                PacUtility.MemoryToObject<PAC_CLIENT_INFO>(buffer, index, count);
        }


        /// <summary>
        /// Encode the instance of current class into byte array,
        /// according to TD specification.
        /// </summary>
        /// <returns>The encoded byte array</returns>
        internal override byte[] EncodeBuffer()
        {
            return PacUtility.ObjectToMemory(NativePacClientInfo);
        }


        /// <summary>
        /// Calculate size of current instance's encoded buffer, in bytes.
        /// </summary>
        /// <returns>The size of current instance's encoded buffer, in bytes.</returns>
        internal override int CalculateSize()
        {
            int clientIdSize = Marshal.SizeOf(NativePacClientInfo.ClientId);
            ushort nameLength = NativePacClientInfo.NameLength;
            // The structure contains following part:
            // ClientId (8 bytes)
            // NameLength (2 bytes)
            // Name (Namelength bytes)
            return clientIdSize + Marshal.SizeOf(nameLength) + nameLength;
        }


        /// <summary>
        /// Get the ulType of current instance's PAC_INFO_BUFFER.
        /// </summary>
        /// <returns>The ulType of current instance's PAC_INFO_BUFFER.</returns>
        internal override PAC_INFO_BUFFER_Type_Values GetBufferInfoType()
        {
            return PAC_INFO_BUFFER_Type_Values.ClientNameAndTicketInformation;
        }
    }
}

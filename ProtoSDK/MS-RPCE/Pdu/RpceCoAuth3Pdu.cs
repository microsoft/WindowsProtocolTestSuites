// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// If a client has prior knowledge of how many legs different security 
    /// providers use, and it knows that a given security provider has an 
    /// odd number of legs, the client SHOULD use an rpc_auth_3 PDU instead 
    /// of an alter_context PDU for the last leg.
    /// </summary>
    public class RpceCoAuth3Pdu : RpceCoPdu
    {
        /// <summary>
        /// Initialize an instance of RpceCoAuth3Pdu class.
        /// </summary>
        /// <param name="context">context</param>
        public RpceCoAuth3Pdu(RpceContext context)
            : base(context)
        {
        }


        /// <summary>
        /// Initialize an instance of RpceCoAuth3Pdu class, and 
        /// unmarshal a byte array to PDU struct.
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="pduBytes">A byte array contains PDU data.</param>
        public RpceCoAuth3Pdu(RpceContext context, byte[] pduBytes)
            : base(context, pduBytes)
        {
        }


        /// <summary>
        /// Can be set to any arbitrary value when set and MUST be ignored on receipt. 
        /// The pad field MUST be immediately followed by a sec_trailer structure 
        /// whose layout, location, and alignment are as specified in section 2.2.2.11.
        /// </summary>
        public uint pad;

        /// <summary>
        /// optional authentication verifier<para/>
        /// following fields present if auth_length != 0
        /// </summary>
        public auth_verifier_co_t? auth_verifier;


        /// <summary>
        /// Get the size of structure. 
        /// Length of stub, auth_verifier and auth_value is not included.
        /// </summary>
        /// <returns>The size</returns>
        internal override int GetSize()
        {
            int size = base.GetSize();
            
            size += Marshal.SizeOf(pad); 
            return size;
        }


        /// <summary>
        /// Marshal the PDU struct to a byte array.
        /// </summary>
        /// <param name="binaryWriter">BinaryWriter</param>
        internal override void ToBytes(BinaryWriter binaryWriter)
        {
            base.ToBytes(binaryWriter);
            binaryWriter.Write(pad);
            RpceUtility.AuthVerifierToBytes(binaryWriter, auth_verifier);
        }


        /// <summary>
        /// Un-marshal a byte array to PDU struct.
        /// </summary>
        /// <param name="binaryReader">BinaryReader</param>
        internal override void FromBytes(BinaryReader binaryReader)
        {
            base.FromBytes(binaryReader);
            auth_verifier = RpceUtility.AuthVerifierFromBytes(
                binaryReader, 
                auth_length);
            pad = binaryReader.ReadUInt32();

            if (packed_drep.dataRepFormat != RpceDataRepresentationFormat.IEEE_LittleEndian_ASCII)
            {
                pad = EndianUtility.ReverseByteOrder(pad);
            }
        }
    }
}

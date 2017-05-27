// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// The fault PDU is used to indicate either an RPC run-time, RPC stub, 
    /// or RPC-specific exception to the client. The p_cont_id field holds 
    /// a context identifier that identifies the data representation.
    /// </summary>
    public class RpceCoFaultPdu : RpceCoPdu
    {
        /// <summary>
        /// Initialize an instance of RpceCoFaultPdu class.
        /// </summary>
        /// <param name="context">context</param>
        public RpceCoFaultPdu(RpceContext context)
            : base(context)
        {
        }


        /// <summary>
        /// Initialize an instance of RpceCoFaultPdu class, and 
        /// unmarshal a byte array to PDU struct.
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="pduBytes">A byte array contains PDU data.</param>
        public RpceCoFaultPdu(RpceContext context, byte[] pduBytes)
            : base(context, pduBytes)
        {
        }


        /// <summary>
        /// allocation hint
        /// </summary>
        public uint alloc_hint;

        /// <summary>
        /// pres context, i.e. data rep
        /// </summary>
        public ushort p_cont_id;

        /// <summary>
        /// received cancel count
        /// </summary>
        public byte cancel_count;

        /// <summary>
        /// reserved, m.b.z.
        /// </summary>
        public byte reserved;

        /// <summary>
        /// run-time fault code or zero
        /// </summary>
        public uint status;

        /// <summary>
        /// always pad to next 8-octet boundary<para/>
        /// reserved padding, m.b.z.
        /// </summary>
        public uint reserved2;

        /// <summary>
        /// stub data here, 8-octet aligned
        /// </summary>
        public byte[] stub;

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

            size += Marshal.SizeOf(alloc_hint);
            size += Marshal.SizeOf(p_cont_id);
            size += Marshal.SizeOf(cancel_count);
            size += Marshal.SizeOf(reserved);
            size += Marshal.SizeOf(status);
            size += Marshal.SizeOf(reserved2);
            return size;
        }


        /// <summary>
        /// Marshal the PDU struct to a byte array.
        /// </summary>
        /// <param name="binaryWriter">BinaryWriter</param>
        internal override void ToBytes(BinaryWriter binaryWriter)
        {
            base.ToBytes(binaryWriter);

            binaryWriter.Write(alloc_hint);
            binaryWriter.Write(p_cont_id);
            binaryWriter.Write(cancel_count);
            binaryWriter.Write(reserved);
            binaryWriter.Write(status);
            binaryWriter.Write(reserved2);

            if (stub != null)
            {
                binaryWriter.Write(stub);
            }

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

            alloc_hint = binaryReader.ReadUInt32();
            p_cont_id = binaryReader.ReadUInt16();
            cancel_count = binaryReader.ReadByte();
            reserved = binaryReader.ReadByte();
            status = binaryReader.ReadUInt32();
            reserved2 = binaryReader.ReadUInt32();

            if (packed_drep.dataRepFormat != RpceDataRepresentationFormat.IEEE_LittleEndian_ASCII)
            {
                alloc_hint = EndianUtility.ReverseByteOrder(alloc_hint);
                p_cont_id = EndianUtility.ReverseByteOrder(p_cont_id);
                status = EndianUtility.ReverseByteOrder(status);
            }

            // read stub
            int stubLength = frag_length;
            stubLength -= GetSize();
            if (auth_verifier != null)
            {
                stubLength -= auth_verifier.Value.auth_pad_length;
                stubLength -= RpceUtility.AUTH_VERIFIER_SIZE;
                stubLength -= auth_length;
            }
            stub = binaryReader.ReadBytes(stubLength);
        }
    }
}

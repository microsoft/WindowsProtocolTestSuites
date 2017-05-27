// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// The bind_ack PDU is returned by the server when it accepts 
    /// a bind request initiated by the client's bind PDU. 
    /// It contains the results of presentation context and 
    /// fragment size negotiations. It may also contain a new 
    /// association group identifier if one was requested by the client.
    /// </summary>
    public class RpceCoBindAckPdu : RpceCoPdu
    {
        /// <summary>
        /// Initialize an instance of RpceCoBindAckPdu class.
        /// </summary>
        /// <param name="context">context</param>
        public RpceCoBindAckPdu(RpceContext context)
            : base(context)
        {
        }


        /// <summary>
        /// Initialize an instance of RpceCoBindAckPdu class, and 
        /// unmarshal a byte array to PDU struct.
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="pduBytes">A byte array contains PDU data.</param>
        public RpceCoBindAckPdu(RpceContext context, byte[] pduBytes)
            : base(context, pduBytes)
        {
        }


        /// <summary>
        /// max transmit frag size, bytes
        /// </summary>
        public ushort max_xmit_frag;

        /// <summary>
        /// max receive frag size, bytes
        /// </summary>
        public ushort max_recv_frag;

        /// <summary>
        /// incarnation of client-server assoc group
        /// </summary>
        public uint assoc_group_id;

        /// <summary>
        /// optional secondary address<para/>
        /// for process incarnation; local port part of address only
        /// </summary>
        public port_any_t sec_addr;

        /// <summary>
        /// restore 4-octet alignment
        /// size_is(align(4))
        /// </summary>
        public byte[] pad2;

        /// <summary>
        /// presentation context result list, including hints
        /// </summary>
        public p_result_list_t p_result_list;

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

            size += Marshal.SizeOf(max_xmit_frag);
            size += Marshal.SizeOf(max_recv_frag);
            size += Marshal.SizeOf(assoc_group_id);

            size += RpceUtility.PORT_ANY_T_HEADER_SIZE;
            if (sec_addr.port_spec != null)
            {
                size += sec_addr.port_spec.Length;
            }
            if (pad2 != null)
            {
                size += pad2.Length;
            }
            size += RpceUtility.P_RESULT_LIST_T_HEADER_SIZE;
            if (p_result_list.p_results != null)
            {
                size += p_result_list.p_results.Length * RpceUtility.P_RESULT_T_SIZE;
            }
            return size;
        }


        /// <summary>
        /// Marshal the PDU struct to a byte array.
        /// </summary>
        /// <param name="binaryWriter">BinaryWriter</param>
        internal override void ToBytes(BinaryWriter binaryWriter)
        {
            base.ToBytes(binaryWriter);

            binaryWriter.Write(max_xmit_frag);
            binaryWriter.Write(max_recv_frag);
            binaryWriter.Write(assoc_group_id);

            binaryWriter.Write(sec_addr.length);
            binaryWriter.Write(sec_addr.port_spec);

            if (pad2 != null)
            {
                binaryWriter.Write(pad2);
            }

            binaryWriter.Write(p_result_list.n_results);
            binaryWriter.Write(p_result_list.reserved);
            binaryWriter.Write(p_result_list.reserved2);

            if (p_result_list.p_results != null)
            {
                for (int i = 0; i < p_result_list.p_results.Length; i++)
                {
                    binaryWriter.Write((ushort)p_result_list.p_results[i].result);
                    binaryWriter.Write((ushort)p_result_list.p_results[i].reason);

                    binaryWriter.Write(p_result_list.p_results[i].transfer_syntax.if_uuid.ToByteArray());
                    binaryWriter.Write(p_result_list.p_results[i].transfer_syntax.if_version);
                }
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

            max_xmit_frag = binaryReader.ReadUInt16();
            max_recv_frag = binaryReader.ReadUInt16();
            assoc_group_id = binaryReader.ReadUInt32();

            if (packed_drep.dataRepFormat != RpceDataRepresentationFormat.IEEE_LittleEndian_ASCII)
            {
                max_xmit_frag = EndianUtility.ReverseByteOrder(max_xmit_frag);
                max_recv_frag = EndianUtility.ReverseByteOrder(max_recv_frag);
                assoc_group_id = EndianUtility.ReverseByteOrder(assoc_group_id);
            }

            sec_addr = new port_any_t();
            sec_addr.length = binaryReader.ReadUInt16();
            if (packed_drep.dataRepFormat != RpceDataRepresentationFormat.IEEE_LittleEndian_ASCII)
            {
                sec_addr.length = EndianUtility.ReverseByteOrder(sec_addr.length);
            }
            sec_addr.port_spec = binaryReader.ReadBytes(sec_addr.length);

            // restore 4-octet alignment
            int pad2Length = RpceUtility.Align((int)binaryReader.BaseStream.Position, 4)
                                - (int)binaryReader.BaseStream.Position;
            pad2 = binaryReader.ReadBytes(pad2Length);

            p_result_list = new p_result_list_t();
            p_result_list.n_results = binaryReader.ReadByte();
            p_result_list.reserved = binaryReader.ReadByte();
            p_result_list.reserved2 = binaryReader.ReadUInt16();

            p_result_list.p_results = new p_result_t[p_result_list.n_results];
            for (int i = 0; i < p_result_list.n_results; i++)
            {
                if (packed_drep.dataRepFormat == RpceDataRepresentationFormat.IEEE_LittleEndian_ASCII)
                {
                    p_result_list.p_results[i].result = (p_cont_def_result_t)binaryReader.ReadUInt16();
                    p_result_list.p_results[i].reason = (p_provider_reason_t)binaryReader.ReadUInt16();
                }
                else
                {

                    p_result_list.p_results[i].result = (p_cont_def_result_t)EndianUtility.ReverseByteOrder(binaryReader.ReadUInt16());
                    p_result_list.p_results[i].reason = (p_provider_reason_t)EndianUtility.ReverseByteOrder(binaryReader.ReadUInt16());
                }

                p_result_list.p_results[i].transfer_syntax = new p_syntax_id_t();
                p_result_list.p_results[i].transfer_syntax.if_uuid
                    = new Guid(binaryReader.ReadBytes(RpceUtility.GUID_SIZE));
                p_result_list.p_results[i].transfer_syntax.if_version
                    = binaryReader.ReadUInt32();

                if (packed_drep.dataRepFormat != RpceDataRepresentationFormat.IEEE_LittleEndian_ASCII)
                {
                    p_result_list.p_results[i].transfer_syntax.if_uuid = EndianUtility.ReverseByteOrder(p_result_list.p_results[i].transfer_syntax.if_uuid);
                    p_result_list.p_results[i].transfer_syntax.if_version = EndianUtility.ReverseByteOrder(p_result_list.p_results[i].transfer_syntax.if_version);
                }
            }
        }
    }
}

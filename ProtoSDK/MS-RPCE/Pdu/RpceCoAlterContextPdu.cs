// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// The alter_context PDU is used to request additional 
    /// presentation negotiation for another interface and/or version, 
    /// or to negotiate a new security context, or both. 
    /// The format is identical to the bind PDU, except that 
    /// the value of the PTYPE field is set to alter_context. 
    /// The max_xmit_frag, max_recv_frag and assoc_group_id fields are be ignored.
    /// </summary>
    public class RpceCoAlterContextPdu : RpceCoPdu
    {
        /// <summary>
        /// Initialize an instance of RpceCoAlterContextPdu class.
        /// </summary>
        /// <param name="context">context</param>
        public RpceCoAlterContextPdu(RpceContext context)
            : base(context)
        {
        }


        /// <summary>
        /// Initialize an instance of RpceCoAlterContextPdu class, and 
        /// unmarshal a byte array to PDU struct.
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="pduBytes">A byte array contains PDU data.</param>
        public RpceCoAlterContextPdu(RpceContext context, byte[] pduBytes)
            : base(context, pduBytes)
        {
        }


        /// <summary>
        /// ignored
        /// </summary>
        public ushort max_xmit_frag;

        /// <summary>
        /// ignored
        /// </summary>
        public ushort max_recv_frag;

        /// <summary>
        /// ignored
        /// </summary>
        public uint assoc_group_id;

        /// <summary>
        /// presentation context list
        /// </summary>
        public p_cont_list_t p_context_elem;

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
            size += RpceUtility.P_CONT_LIST_T_HEADER_SIZE;
            if (p_context_elem.p_cont_elem != null)
            {
                for (int i = 0; i < p_context_elem.p_cont_elem.Length; i++)
                {
                    size += Marshal.SizeOf(p_context_elem.p_cont_elem[i].p_cont_id);
                    size += Marshal.SizeOf(p_context_elem.p_cont_elem[i].n_transfer_syn);
                    size += Marshal.SizeOf(p_context_elem.p_cont_elem[i].reserved);
                    size += RpceUtility.P_SYNTAX_ID_T_SIZE;
                    if (p_context_elem.p_cont_elem[i].transfer_syntaxes != null)
                    {
                        size += p_context_elem.p_cont_elem[i].transfer_syntaxes.Length * RpceUtility.P_SYNTAXID_T_SIZE;
                    }
                }
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

            binaryWriter.Write(p_context_elem.n_context_elem);
            binaryWriter.Write(p_context_elem.reserved);
            binaryWriter.Write(p_context_elem.reserved2);

            if (p_context_elem.p_cont_elem != null)
            {
                for (int i = 0; i < p_context_elem.p_cont_elem.Length; i++)
                {
                    binaryWriter.Write(p_context_elem.p_cont_elem[i].p_cont_id);
                    binaryWriter.Write(p_context_elem.p_cont_elem[i].n_transfer_syn);
                    binaryWriter.Write(p_context_elem.p_cont_elem[i].reserved);
                    binaryWriter.Write(p_context_elem.p_cont_elem[i].abstract_syntax.if_uuid.ToByteArray());
                    binaryWriter.Write(p_context_elem.p_cont_elem[i].abstract_syntax.if_version);
                    if (p_context_elem.p_cont_elem[i].transfer_syntaxes != null)
                    {
                        for (int j = 0; j < p_context_elem.p_cont_elem[i].transfer_syntaxes.Length; j++)
                        {
                            binaryWriter.Write(
                                p_context_elem.p_cont_elem[i].transfer_syntaxes[j].if_uuid.ToByteArray());
                            binaryWriter.Write(p_context_elem.p_cont_elem[i].transfer_syntaxes[j].if_version);
                        }
                    }
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

            p_context_elem = new p_cont_list_t();

            p_context_elem.n_context_elem = binaryReader.ReadByte();
            p_context_elem.reserved = binaryReader.ReadByte();
            p_context_elem.reserved2 = binaryReader.ReadUInt16();

            p_context_elem.p_cont_elem = new p_cont_elem_t[p_context_elem.n_context_elem];
            for (int i = 0; i < p_context_elem.n_context_elem; i++)
            {
                p_context_elem.p_cont_elem[i].p_cont_id = binaryReader.ReadUInt16();
                p_context_elem.p_cont_elem[i].n_transfer_syn = binaryReader.ReadByte();
                p_context_elem.p_cont_elem[i].reserved = binaryReader.ReadByte();

                p_context_elem.p_cont_elem[i].abstract_syntax = new p_syntax_id_t();
                p_context_elem.p_cont_elem[i].abstract_syntax.if_uuid
                    = new Guid(binaryReader.ReadBytes(RpceUtility.GUID_SIZE)); 
                p_context_elem.p_cont_elem[i].abstract_syntax.if_version
                    = binaryReader.ReadUInt32();

                if (packed_drep.dataRepFormat != RpceDataRepresentationFormat.IEEE_LittleEndian_ASCII)
                {
                    p_context_elem.p_cont_elem[i].p_cont_id = EndianUtility.ReverseByteOrder(p_context_elem.p_cont_elem[i].p_cont_id);
                    p_context_elem.p_cont_elem[i].abstract_syntax.if_uuid = EndianUtility.ReverseByteOrder(p_context_elem.p_cont_elem[i].abstract_syntax.if_uuid);
                    p_context_elem.p_cont_elem[i].abstract_syntax.if_version = EndianUtility.ReverseByteOrder(p_context_elem.p_cont_elem[i].abstract_syntax.if_version);
                }

                p_context_elem.p_cont_elem[i].transfer_syntaxes
                    = new p_syntax_id_t[p_context_elem.p_cont_elem[i].n_transfer_syn];
                for (int j = 0; j < p_context_elem.p_cont_elem[i].transfer_syntaxes.Length; j++)
                {
                    p_context_elem.p_cont_elem[i].transfer_syntaxes[j].if_uuid
                        = new Guid(binaryReader.ReadBytes(RpceUtility.GUID_SIZE));
                    p_context_elem.p_cont_elem[i].transfer_syntaxes[j].if_version
                        = binaryReader.ReadUInt32();

                    if (packed_drep.dataRepFormat != RpceDataRepresentationFormat.IEEE_LittleEndian_ASCII)
                    {
                        p_context_elem.p_cont_elem[i].transfer_syntaxes[j].if_uuid = EndianUtility.ReverseByteOrder(p_context_elem.p_cont_elem[i].transfer_syntaxes[j].if_uuid);
                        p_context_elem.p_cont_elem[i].transfer_syntaxes[j].if_version = EndianUtility.ReverseByteOrder(p_context_elem.p_cont_elem[i].transfer_syntaxes[j].if_version);
                    }
                }
            }
        }
    }
}

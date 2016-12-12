// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// The bind_nak PDU is returned by the server when it rejects 
    /// an association request initiated by the client's bind PDU. 
    /// The provider_reject_reason field holds the rejection reason code. 
    /// When the reject reason is protocol_version_not_supported, 
    /// the versions field contains a list of runtime protocol 
    /// versions supported by the server.<para/>
    /// The bind_nak PDU never contains an authentication verifier.
    /// </summary>
    public class RpceCoBindNakPdu : RpceCoPdu
    {
        /// <summary>
        /// Initialize an instance of RpceCoBindNakPdu class.
        /// </summary>
        /// <param name="context">context</param>
        public RpceCoBindNakPdu(RpceContext context)
            : base(context)
        {
        }


        /// <summary>
        /// Initialize an instance of RpceCoBindNakPdu class, and 
        /// unmarshal a byte array to PDU struct.
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="pduBytes">A byte array contains PDU data.</param>
        public RpceCoBindNakPdu(RpceContext context, byte[] pduBytes)
            : base(context, pduBytes)
        {
        }


        /// <summary>
        /// presentation context reject
        /// </summary>
        public p_reject_reason_t provider_reject_reason;

        /// <summary>
        /// array of protocol versions supported
        /// </summary>
        public p_rt_versions_supported_t versions;


        /// <summary>
        /// pad after versions. 
        /// align(4)
        /// </summary>
        public byte[] pad;

        /// <summary>
        /// The value for the Signature field that specifies the presence 
        /// of extended error information in a bind_nak PDU MUST be 
        /// 90740320-fad0-11d3-82d7-009027b130ab.
        /// MS-RPCE add the Signature field at the end as an optional field.
        /// </summary>
        public Guid? signature;


        /// <summary>
        /// If the Signature field is equal to the extended error information 
        /// signature value, as specified in section 2.2.1.1.2, the client 
        /// MUST assume that the bind_nak PDU contains RPC extended error 
        /// information appended as a BLOB, as specified in [MS-EERR], 
        /// immediately following the Signature field that continues until 
        /// the end of the PDU. If RPC extended error information is present, 
        /// the length of the BLOB containing it MUST be calculated as frag_length - 0x28.
        /// </summary>
        public byte[] extended_error_info;


        /// <summary>
        /// Get the size of structure.
        /// Length of stub, auth_verifier and auth_value is not included.
        /// </summary>
        /// <returns>The size</returns>
        internal override int GetSize()
        {
            int size = base.GetSize();

            // p_reject_reason_t 
            size += sizeof(ushort);
            size += RpceUtility.P_RT_VERSIONS_SUPPORTED_T_HEADER_SIZE;
            if (versions.p_protocols != null)
            {
                size += versions.p_protocols.Length * RpceUtility.VERSION_T_SIZE; 
            }
            if (pad != null)
            {
                size += pad.Length;
            }
            if (signature != null)
            {
                size += RpceUtility.GUID_SIZE;
            }
            if (extended_error_info != null)
            {
                size += extended_error_info.Length;
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

            binaryWriter.Write((ushort)provider_reject_reason);
            binaryWriter.Write(versions.n_protocols);
            if (versions.p_protocols != null)
            {
                for (int i = 0; i < versions.p_protocols.Length; i++)
                {
                    binaryWriter.Write(versions.p_protocols[i].major);
                    binaryWriter.Write(versions.p_protocols[i].minor);
                }
            }
            if (pad != null)
            {
                binaryWriter.Write(pad);
            }
            if (signature != null)
            {
                binaryWriter.Write(signature.Value.ToByteArray());
            }
            if (extended_error_info != null)
            {
                binaryWriter.Write(extended_error_info);
            }
        }


        /// <summary>
        /// Un-marshal a byte array to PDU struct.
        /// </summary>
        /// <param name="binaryReader">BinaryReader</param>
        internal override void FromBytes(BinaryReader binaryReader)
        {
            base.FromBytes(binaryReader);

            if (packed_drep.dataRepFormat == RpceDataRepresentationFormat.IEEE_LittleEndian_ASCII)
            {
                provider_reject_reason = (p_reject_reason_t)binaryReader.ReadUInt16();
            }
            else
            {
                provider_reject_reason = (p_reject_reason_t)EndianUtility.ReverseByteOrder(binaryReader.ReadUInt16());
            }

            versions = new p_rt_versions_supported_t();
            versions.n_protocols = binaryReader.ReadByte();
            versions.p_protocols = new version_t[versions.n_protocols];
            for (int i = 0; i < versions.n_protocols; i++)
            {
                versions.p_protocols[i].major = binaryReader.ReadByte();
                versions.p_protocols[i].minor = binaryReader.ReadByte();
            }

            //Assume that the client calculates the length of the PDU 
            //until the Signature field as L.
            //If the frag_length field is greater than or equal to L 
            //plus the size of the Signature field, 
            //the client SHOULD assume that the Signature field is present.
            //Otherwise, the client SHOULD assume that the Signature field is not present.
            //TD shows the signature is aligned at 4.
            int L = RpceUtility.Align((int)binaryReader.BaseStream.Position, 4);
            if (frag_length >= (L + RpceUtility.GUID_SIZE))
            {
                pad = binaryReader.ReadBytes(L - (int)binaryReader.BaseStream.Position);
                signature = new Guid(binaryReader.ReadBytes(RpceUtility.GUID_SIZE));
                extended_error_info = binaryReader.ReadBytes(frag_length - L - RpceUtility.GUID_SIZE);

                if (packed_drep.dataRepFormat != RpceDataRepresentationFormat.IEEE_LittleEndian_ASCII)
                {
                    signature = EndianUtility.ReverseByteOrder((Guid)signature);
                }
            }
        }
    }
}

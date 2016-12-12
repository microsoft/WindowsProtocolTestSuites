// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// The request PDU is used for an initial call request. 
    /// The p_cont_id field holds a presentation context 
    /// identifier that identifies the data representation. 
    /// The opnum field identifies the operation being invoked within the interface.
    /// </summary>
    public class RpceCoRequestPdu : RpceCoPdu
    {
        /// <summary>
        /// Initialize an instance of RpceCoRequestPdu class.
        /// </summary>
        /// <param name="context">context</param>
        public RpceCoRequestPdu(RpceContext context)
            : base(context)
        {
        }


        /// <summary>
        /// Initialize an instance of RpceCoRequestPdu class, and 
        /// unmarshal a byte array to PDU struct.
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="pduBytes">A byte array contains PDU data.</param>
        public RpceCoRequestPdu(RpceContext context, byte[] pduBytes)
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
        /// operation # within the interface
        /// </summary>
        public ushort opnum;

        /// <summary>
        /// object UID<para/>
        /// optional field for request, only present if the PFC_OBJECT_UUID
        /// field is non-zero
        /// </summary>
        public Guid? @object;

        /// <summary>
        /// stub data, 8-octet aligned
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
            size += Marshal.SizeOf(opnum);
            if (@object != null)
            {
                size += RpceUtility.GUID_SIZE;
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

            binaryWriter.Write(alloc_hint);
            binaryWriter.Write(p_cont_id);
            binaryWriter.Write(opnum);

            if (@object != null)
            {
                binaryWriter.Write(@object.Value.ToByteArray());
            }

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
            opnum = binaryReader.ReadUInt16();

            if (packed_drep.dataRepFormat != RpceDataRepresentationFormat.IEEE_LittleEndian_ASCII)
            {
                alloc_hint = EndianUtility.ReverseByteOrder(alloc_hint);
                p_cont_id = EndianUtility.ReverseByteOrder(p_cont_id);
                opnum = EndianUtility.ReverseByteOrder(opnum);
            }

            if ((pfc_flags & RpceCoPfcFlags.PFC_OBJECT_UUID) != 0)
            {
                @object = new Guid(binaryReader.ReadBytes(RpceUtility.GUID_SIZE)); 
            }

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


        /// <summary>
        /// Create an verification trailer with commands parameter passed in to the method.
        /// </summary>
        /// <param name="commands">
        /// A list of commands to be contained in the verification trailer.<para/>
        /// Flag SEC_VT_COMMAND_END is ignored by input, and will be added to 
        /// the last command automatically.
        /// </param>
        /// <returns>
        /// Created verification trailer. Pad is not added until appending to stub.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when commands is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a SEC_VT_COMMAND is invalid, or NDR/NDR64 was not negotiated.
        /// </exception>
        public verification_trailer_t CreateVerificationTrailer(
            params SEC_VT_COMMAND[] commands)
        {
            if (commands == null)
            {
                throw new ArgumentNullException("commands");
            }
            
            verification_trailer_t verificationTrailer = new verification_trailer_t();
            verificationTrailer.header = new rpc_sec_verification_trailer();
            verificationTrailer.header.signature = BitConverter.GetBytes(verification_trailer_t.SIGNATURE);
            List<SEC_VT> secVtList = new List<SEC_VT>();
            for (int i = 0; i < commands.Length; i++)
            {
                switch (commands[i]
                    & ~(SEC_VT_COMMAND.SEC_VT_COMMAND_END | SEC_VT_COMMAND.SEC_VT_MUST_PROCESS_COMMAND))
                {
                    case SEC_VT_COMMAND.SEC_VT_COMMAND_BITMASK_1:
                        rpc_sec_vt_bitmask secVtBitmask = new rpc_sec_vt_bitmask();
                        secVtBitmask.command = commands[i];
                        //length: MUST be 0x0004.
                        secVtBitmask.length = verification_trailer_t.SEC_VT_BITMASK_LENGTH;
                        //bits: The bits field is a bitmask. 
                        //A server MUST ignore bits it does not understand. 
                        //Currently, there is only one bit defined: CLIENT_SUPPORT_HEADER_SIGNING 
                        //(bitmask of 0x00000001). If this bit is set, the PFC_SUPPORT_HEADER_SIGN 
                        //bit, as specified in section 2.2.2.3, MUST be present in the PDU header 
                        //for the bind PDU on this connection.
                        secVtBitmask.bits = context.SupportsHeaderSign ? (uint)0x00000001 : 0;
                        secVtList.Add(secVtBitmask);
                        break;

                    case SEC_VT_COMMAND.SEC_VT_COMMAND_HEADER2:
                        rpc_sec_vt_header2 secVtHeader2 = new rpc_sec_vt_header2();
                        secVtHeader2.command = commands[i];
                        //length: MUST be 0x0010.
                        secVtHeader2.length = verification_trailer_t.SEC_VT_HEADER2_LENGTH;
                        //PTYPE: MUST be the same as the PTYPE field in the request PDU header.
                        secVtHeader2.PTYPE = PTYPE;
                        secVtHeader2.reserved1 = 0;
                        secVtHeader2.reserved2 = 0;
                        secVtHeader2.drep = packed_drep;
                        secVtHeader2.call_id = call_id;
                        secVtHeader2.p_cont_id = p_cont_id;
                        secVtHeader2.opnum = opnum;
                        secVtList.Add(secVtHeader2);
                        break;

                    case SEC_VT_COMMAND.SEC_VT_COMMAND_PCONTEXT:
                        rpc_sec_vt_pcontext secVtPcontext = new rpc_sec_vt_pcontext();
                        secVtPcontext.command = commands[i];
                        //length: MUST be set to 0x28.
                        secVtPcontext.length = verification_trailer_t.SEC_VT_PCONTEXT_LENGTH;
                        secVtPcontext.interfaceId = new p_syntax_id_t();
                        secVtPcontext.interfaceId.if_uuid = context.InterfaceId;
                        secVtPcontext.interfaceId.if_vers_major = context.InterfaceMajorVersion;
                        secVtPcontext.interfaceId.if_vers_minor = context.InterfaceMinorVersion;
                        secVtPcontext.transferSyntax = new p_syntax_id_t();
                        if (context.NdrVersion == RpceNdrVersion.NDR)
                        {
                            secVtPcontext.transferSyntax.if_uuid = RpceUtility.NDR_INTERFACE_UUID;
                            secVtPcontext.transferSyntax.if_vers_major = RpceUtility.NDR_INTERFACE_MAJOR_VERSION;
                            secVtPcontext.transferSyntax.if_vers_minor = RpceUtility.NDR_INTERFACE_MINOR_VERSION;
                        }
                        else if (context.NdrVersion == RpceNdrVersion.NDR64)
                        {
                            secVtPcontext.transferSyntax.if_uuid = RpceUtility.NDR64_INTERFACE_UUID;
                            secVtPcontext.transferSyntax.if_vers_major = RpceUtility.NDR64_INTERFACE_MAJOR_VERSION;
                            secVtPcontext.transferSyntax.if_vers_minor = RpceUtility.NDR64_INTERFACE_MINOR_VERSION;
                        }
                        else
                        {
                            throw new InvalidOperationException("Neither NDR nore NDR64 was negotiated.");
                        }
                        secVtList.Add(secVtPcontext);
                        break;

                    default:
                        throw new InvalidOperationException("Invalid SEC_VT_COMMAND.");
                }
            }
            verificationTrailer.commands = secVtList.ToArray();

            return verificationTrailer;
        }


        /// <summary>
        /// Append verification_trailer to stub.
        /// </summary>
        /// <param name="verificationTrailer">verification_trailer</param>
        /// <exception cref="ArgumentNullException">Thrown when verificationTrailer is null.</exception>
        public void AppendVerificationTrailerToStub(verification_trailer_t verificationTrailer)
        {
            if (verificationTrailer == null)
            {
                throw new ArgumentNullException("verificationTrailer");
            }

            if (stub == null)
            {
                stub = new byte[0];
            }

            stub = RpceUtility.AppendVerificationTrailerToStub(stub, verificationTrailer);

            if (auth_verifier != null)
            {
                auth_verifier_co_t authVerifier = auth_verifier.Value;
                authVerifier.auth_pad_length = (byte)(
                    RpceUtility.Align(stub.Length, RpceUtility.STUB_PAD_LENGTH) - stub.Length);
                authVerifier.auth_pad = new byte[authVerifier.auth_pad_length];
                auth_verifier = authVerifier;
            }

            SetLength();
        }


        /// <summary>
        /// Extract verification_trailer from stub.
        /// </summary>
        /// <param name="startIndex">
        /// Input: The position of the end of stub (the beginning position to search for verification_trailer).
        /// Output: The start position of verification_trailer.
        /// </param>
        /// <returns>Returns verification_trailer if found; otherwise, returns null.</returns>
        public verification_trailer_t ExtractVerificationTrailerFromStub(ref int startIndex)
        {
            if (stub == null)
            {
                //No stub, no verification_trailer.
                return null;
            }

            return RpceUtility.ExtractVerificationTrailerFromStub(stub, ref startIndex);
        }
    }
}

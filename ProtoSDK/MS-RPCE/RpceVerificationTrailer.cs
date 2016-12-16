// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// Whenever the verification trailer is present, 
    /// the signature field MUST contain the following 
    /// series of octets {0x8a, 0xe3, 0x13, 0x71, 0x02, 
    /// 0xf4, 0x36, 0x71}. These values have no special 
    /// protocol significance and only serve as a signature 
    /// for this structure.
    /// </summary>
    public struct rpc_sec_verification_trailer
    {
        /// <summary>
        /// Signature field MUST contain the following 
        /// series of octets {0x8a, 0xe3, 0x13, 0x71, 0x02, 
        /// 0xf4, 0x36, 0x71}.
        /// </summary>
        public byte[] signature;
    }


    /// <summary>
    /// The verification trailer commands may come in any order after the header. 
    /// If more than one command is present, the next command MUST be placed 
    /// immediately after the previous one. Each command MUST start with a common 
    /// command header.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum SEC_VT_COMMAND : ushort
    {
        /// <summary>
        /// This is an rpc_sec_vt_bitmask command, as specified in section 2.2.2.13.2.
        /// </summary>
        SEC_VT_COMMAND_BITMASK_1 = 0x0001,

        /// <summary>
        /// This is an rpc_sec_vt_pcontext command, as specified in section 2.2.2.13.4.
        /// </summary>
        SEC_VT_COMMAND_PCONTEXT = 0x0002,

        /// <summary>
        /// This is an rpc_sec_vt_header2 command, as specified in section 2.2.2.13.3.
        /// </summary>
        SEC_VT_COMMAND_HEADER2 = 0x0003,

        /// <summary>
        /// This flag MUST be present in the last command in the verification trailer body.
        /// </summary>
        SEC_VT_COMMAND_END = 0x4000,

        /// <summary>
        /// Indicates that the server MUST process this command. 
        /// If the server does not support the command, it MUST reject the request.
        /// </summary>
        SEC_VT_MUST_PROCESS_COMMAND = 0x8000
    }


    /// <summary>
    /// SEC_VT class is the common header for all verification trailer commands.
    /// </summary>
    public class SEC_VT
    {
        /// <summary>
        /// The commands MUST be encoded by using little-endian encoding for all fields.<para/>
        /// Least significant bits 0 through 13 (including 0 and 13) are used to hold 
        /// the command type and MUST be considered a single field. 
        /// Bits 14 and 15 are used to indicate command processing rules. 
        /// If a server does not understand a command, it MUST ignore it unless the 
        /// SEC_VT_MUST_PROCESS_COMMAND bit is set. 
        /// If the server does not understand the command and the 
        /// SEC_VT_MUST_PROCESS_COMMAND bit is set, it MUST treat the request as invalid, 
        /// as if unmarshaling failure occurred, as specified in section 3.1.3.5.2, 
        /// except that a status code of 5 SHOULD be used instead of the status code 
        /// specified in section 3.1.3.5.2. 
        /// Any combination of a value for the command type (bits 0 through 13) and 
        /// command processing rules (bits 14 and 15) is valid.
        /// </summary>
        public SEC_VT_COMMAND command;

        /// <summary>
        /// The length field is in octets, MUST be a multiple of 4, 
        /// and MUST NOT include the length of the command header. 
        /// For fixed-size commands, the length field MUST be equal 
        /// to the length of the fixed-size command.
        /// </summary>
        public ushort length;
    }


    /// <summary>
    /// rpc_sec_vt_bitmask verification trailer.
    /// </summary>
    public class rpc_sec_vt_bitmask : SEC_VT
    {
        /// <summary>
        /// The bits field is a bitmask. 
        /// A server MUST ignore bits it does not understand. 
        /// Currently, there is only one bit defined: 
        /// CLIENT_SUPPORT_HEADER_SIGNING (bitmask of 0x00000001). 
        /// If this bit is set, the PFC_SUPPORT_HEADER_SIGN bit, 
        /// as specified in section 2.2.2.3, MUST be present in 
        /// the PDU header for the bind PDU on this connection. 
        /// For information on how PFC_SUPPORT_HEADER_SIGN is used, 
        /// see section 3.3.1.5.2.2.
        /// </summary>
        public uint bits;
    }


    /// <summary>
    /// rpc_sec_vt_header2 verification trailer.
    /// </summary>
    public class rpc_sec_vt_header2 : SEC_VT
    {
        /// <summary>
        /// MUST be the same as the PTYPE field in the request PDU header.
        /// </summary>
        public RpcePacketType PTYPE;

        /// <summary>
        /// MUST be set to 0 when sent and MUST be ignored on receipt.
        /// </summary>
        public byte reserved1;

        /// <summary>
        /// MUST be set to 0 when sent and MUST be ignored on receipt.
        /// </summary>
        public ushort reserved2;

        /// <summary>
        /// MUST be the same as the drep field in the request PDU header.
        /// </summary>
        public DataRepresentationFormatLabel drep;

        /// <summary>
        /// MUST be the same as the call_id field in the request PDU header.
        /// </summary>
        public uint call_id;

        /// <summary>
        /// MUST be the same as the p_cont_id field in the request PDU header.
        /// </summary>
        public ushort p_cont_id;

        /// <summary>
        /// MUST be the same as the opnum field in the request PDU header.
        /// </summary>
        public ushort opnum;
    }


    /// <summary>
    /// rpc_sec_vt_pcontext verification trailer.
    /// </summary>
    public class rpc_sec_vt_pcontext : SEC_VT
    {
        /// <summary>
        /// The interface identifier for the presentation context of 
        /// the request PDU in which this verification trailer appears. 
        /// This MUST match the chosen abstract_syntax field from the bind 
        /// or alter_context PDU where the presentation context was negotiated. 
        /// For information on how a presentation context is negotiated, 
        /// see section 3.3.1.5.7.
        /// </summary>
        public p_syntax_id_t interfaceId;

        /// <summary>
        /// The transfer syntax identifier for the presentation context 
        /// of the request PDU in which this verification trailer appears. 
        /// This MUST match the chosen transfer_syntax from the bind or 
        /// alter_context PDU where the presentation context was negotiated. 
        /// For information on how a presentation context is negotiated, 
        /// see section 3.3.1.5.7.
        /// </summary>
        public p_syntax_id_t transferSyntax;
    }


    /// <summary>
    /// Verification trailer.
    /// </summary>
    public class verification_trailer_t
    {
        /// <summary>
        /// The signature of verification_trailer, it MUST contain the following 
        /// series of octets {0x8a, 0xe3, 0x13, 0x71, 0x02, 0xf4, 0x36, 0x71}.
        /// </summary>
        public const ulong SIGNATURE = 0x7136f4027113e38a;

        // length of SEC_VT common header.
        internal const ushort SEC_VT_HEADER_LENGTH = 0x04;

        // length of SEC_VT_BITMASK
        internal const ushort SEC_VT_BITMASK_LENGTH = 0x04;

        // length of SEC_VT_HEADER2
        internal const ushort SEC_VT_HEADER2_LENGTH = 0x10;

        // length of SEC_VT_PCONTEXT
        internal const ushort SEC_VT_PCONTEXT_LENGTH = 0x28;


        /// <summary>
        /// The beginning of the header MUST be 4-byte aligned with 
        /// respect to the beginning of the PDU.
        /// </summary>
        public byte[] pad;

        /// <summary>
        /// Verification trailer header.
        /// </summary>
        public rpc_sec_verification_trailer header;

        /// <summary>
        /// Verification trailer commands.
        /// </summary>
        public SEC_VT[] commands;


        /// <summary>
        /// The  size of verification trailer, in bytes.
        /// </summary>
        /// <returns>The size of verification trailer, in bytes.</returns>
        // We don't use property, use GetSize() method.
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public int GetSize()
        {
            int size = 0;
            if (pad != null)
            {
                size += pad.Length;
            }
            if (header.signature != null)
            {
                size += Marshal.SizeOf(verification_trailer_t.SIGNATURE);
            }
            if (commands != null)
            {
                for (int i = 0; i < commands.Length; i++)
                {
                    if (commands[i] != null)
                    {
                        Type secVtType = commands[i].GetType();
                        size += SEC_VT_HEADER_LENGTH;
                        if (secVtType == typeof(rpc_sec_vt_bitmask))
                        {
                            size += SEC_VT_BITMASK_LENGTH; // length of rpc_sec_vt_bitmask
                        }
                        else if (secVtType == typeof(rpc_sec_vt_header2))
                        {
                            size += SEC_VT_HEADER2_LENGTH; // length of rpc_sec_vt_header2
                        }
                        else if (secVtType == typeof(rpc_sec_vt_pcontext))
                        {
                            size += SEC_VT_PCONTEXT_LENGTH; // length of rpc_sec_vt_pcontext
                        }
                        else
                        {
                            throw new InvalidOperationException("Invalid SEC_VT type.");
                        }
                    }
                }
            }
            return size;
        }


        /// <summary>
        /// Marshal the verification trailer to a byte array.
        /// </summary>
        /// <returns>
        /// A byte array.
        /// </returns>
        public byte[] ToBytes()
        {
            byte[] buf = new byte[GetSize()];

            using (BinaryWriter writer = new BinaryWriter(new MemoryStream(buf)))
            {
                if (pad != null)
                {
                    writer.Write(pad);
                }
                if (header.signature != null)
                {
                    writer.Write(header.signature);
                }
                if (commands != null)
                {
                    for (int i = 0; i < commands.Length; i++)
                    {
                        if (commands[i] != null)
                        {
                            Type type = commands[i].GetType();
                            if (type == typeof(rpc_sec_vt_bitmask))
                            {
                                rpc_sec_vt_bitmask secVtBitmask = (rpc_sec_vt_bitmask)commands[i];
                                writer.Write((ushort)secVtBitmask.command);
                                writer.Write(secVtBitmask.length);
                                writer.Write(secVtBitmask.bits);
                            }
                            else if (type == typeof(rpc_sec_vt_header2))
                            {
                                rpc_sec_vt_header2 secVtHeader2 = (rpc_sec_vt_header2)commands[i];
                                writer.Write((ushort)secVtHeader2.command);
                                writer.Write(secVtHeader2.length);
                                writer.Write((byte)secVtHeader2.PTYPE);
                                writer.Write(secVtHeader2.reserved1);
                                writer.Write(secVtHeader2.reserved2);
                                writer.Write((ushort)secVtHeader2.drep.dataRepFormat);
                                writer.Write(secVtHeader2.drep.reserved);
                                writer.Write(secVtHeader2.call_id);
                                writer.Write(secVtHeader2.p_cont_id);
                                writer.Write(secVtHeader2.opnum);
                            }
                            else if (type == typeof(rpc_sec_vt_pcontext))
                            {
                                rpc_sec_vt_pcontext secVtPcontext = (rpc_sec_vt_pcontext)commands[i];
                                writer.Write((ushort)secVtPcontext.command);
                                writer.Write(secVtPcontext.length);
                                writer.Write(secVtPcontext.interfaceId.if_uuid.ToByteArray());
                                writer.Write(secVtPcontext.interfaceId.if_version);
                                writer.Write(secVtPcontext.transferSyntax.if_uuid.ToByteArray());
                                writer.Write(secVtPcontext.transferSyntax.if_version);
                            }
                            else
                            {
                                // we called GetSize() before, this won't happen.
                                throw new InvalidOperationException("Invalid SEC_VT type.");
                            }
                        }
                    }
                }
            }

            return buf;
        }


        /// <summary>
        /// Unmarshal a verification trailer from byte array.
        /// </summary>
        /// <param name="verificationTrailerBuffer">byte array.</param>
        public void FromBytes(byte[] verificationTrailerBuffer)
        {
            int padLength = -1;
            for (int i = 0; i < verificationTrailerBuffer.Length; i++)
            {
                if ((verificationTrailerBuffer.Length - i) >= Marshal.SizeOf(verification_trailer_t.SIGNATURE)
                    && BitConverter.ToUInt64(verificationTrailerBuffer, i) == verification_trailer_t.SIGNATURE)
                {
                    padLength = i;
                }
            }
            if (padLength < 0)
            {
                throw new InvalidOperationException("verification_trailer not found.");
            }

            using (BinaryReader reader = new BinaryReader(new MemoryStream(verificationTrailerBuffer)))
            {
                pad = reader.ReadBytes(padLength);
                header.signature = reader.ReadBytes(sizeof(ulong));

                List<SEC_VT> secVtList = new List<SEC_VT>();
                while (reader.BaseStream.Position < verificationTrailerBuffer.Length)
                {
                    if ((verificationTrailerBuffer.Length - reader.BaseStream.Position) < SEC_VT_HEADER_LENGTH)
                    {
                        break;
                    }

                    SEC_VT_COMMAND command = (SEC_VT_COMMAND)reader.ReadUInt16();
                    ushort length = reader.ReadUInt16();

                    if ((verificationTrailerBuffer.Length - reader.BaseStream.Position) < length)
                    {
                        //length of remain bytes is less than expected.
                        break;
                    }

                    switch (command
                        & ~(SEC_VT_COMMAND.SEC_VT_MUST_PROCESS_COMMAND | SEC_VT_COMMAND.SEC_VT_COMMAND_END))
                    {
                        case SEC_VT_COMMAND.SEC_VT_COMMAND_BITMASK_1:
                            rpc_sec_vt_bitmask secVtBitmask = new rpc_sec_vt_bitmask();
                            secVtBitmask.command = command;
                            secVtBitmask.length = length;
                            secVtBitmask.bits = reader.ReadUInt32();

                            secVtList.Add(secVtBitmask);
                            break;

                        case SEC_VT_COMMAND.SEC_VT_COMMAND_PCONTEXT:
                            rpc_sec_vt_pcontext secVtPcontext = new rpc_sec_vt_pcontext();
                            secVtPcontext.command = command;
                            secVtPcontext.length = length;
                            secVtPcontext.interfaceId = new p_syntax_id_t();
                            secVtPcontext.interfaceId.if_uuid = new Guid(reader.ReadBytes(RpceUtility.GUID_SIZE));
                            secVtPcontext.interfaceId.if_version = reader.ReadUInt32();
                            secVtPcontext.transferSyntax = new p_syntax_id_t();
                            secVtPcontext.transferSyntax.if_uuid = new Guid(reader.ReadBytes(RpceUtility.GUID_SIZE));
                            secVtPcontext.transferSyntax.if_version = reader.ReadUInt32();

                            secVtList.Add(secVtPcontext);
                            break;

                        case SEC_VT_COMMAND.SEC_VT_COMMAND_HEADER2:
                            rpc_sec_vt_header2 secVtHeader2 = new rpc_sec_vt_header2();
                            secVtHeader2.command = command;
                            secVtHeader2.length = length;
                            secVtHeader2.PTYPE = (RpcePacketType)reader.ReadByte();
                            secVtHeader2.reserved1 = reader.ReadByte();
                            secVtHeader2.reserved2 = reader.ReadUInt16();
                            secVtHeader2.drep = new DataRepresentationFormatLabel();
                            secVtHeader2.drep.dataRepFormat = (RpceDataRepresentationFormat)reader.ReadUInt16();
                            secVtHeader2.drep.reserved = reader.ReadUInt16();
                            secVtHeader2.call_id = reader.ReadUInt32();
                            secVtHeader2.p_cont_id = reader.ReadUInt16();
                            secVtHeader2.opnum = reader.ReadUInt16();

                            secVtList.Add(secVtHeader2);
                            break;

                        default:
                            //do nothing
                            break;
                    }
                }

                commands = secVtList.ToArray();
            }
        }
    }
}

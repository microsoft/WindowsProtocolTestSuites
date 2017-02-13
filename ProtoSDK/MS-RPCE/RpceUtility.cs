// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;

using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// RPCE utility class.
    /// </summary>
    public static class RpceUtility
    {
        /// <summary>
        /// Protocol Sequence of RPC over TCP/IP.
        /// </summary>
        public const string RPC_OVER_TCPIP_PROTOCOL_SEQUENCE = "ncacn_ip_tcp";

        /// <summary>
        /// Protocol Sequence of RPC over named pipe.
        /// </summary>
        public const string RPC_OVER_NAMED_PIPE_PROTOCOL_SEQUENCE = "ncacn_np";

        /// <summary>
        /// if_uuid of NDR transfer syntax
        /// </summary>
        public static readonly Guid NDR_INTERFACE_UUID
            = new Guid(0x8A885D04, 0x1CEB, 0x11C9, 0x9F, 0xE8, 0x08, 0x00, 0x2B, 0x10, 0x48, 0x60);

        /// <summary>
        /// if_vers_major of NDR transfer syntax
        /// </summary>
        public const ushort NDR_INTERFACE_MAJOR_VERSION = 2;

        /// <summary>
        /// if_vers_minor of NDR transfer syntax
        /// </summary>
        public const ushort NDR_INTERFACE_MINOR_VERSION = 0;

        /// <summary>
        /// if_uuid of NDR64 transfer syntax
        /// </summary>
        public static readonly Guid NDR64_INTERFACE_UUID
            = new Guid(0x71710533, 0xBEBA, 0x4937, 0x83, 0x19, 0xB5, 0xDB, 0xEF, 0x9C, 0xCC, 0x36);

        /// <summary>
        /// if_vers_major of NDR64 transfer syntax
        /// </summary>
        public const ushort NDR64_INTERFACE_MAJOR_VERSION = 1;

        /// <summary>
        /// if_vers_minor of NDR64 transfer syntax
        /// </summary>
        public const ushort NDR64_INTERFACE_MINOR_VERSION = 0;


        /// <summary>
        /// Default rpc_ver_major
        /// </summary>
        public const byte DEFAULT_RPC_VERSION_MAJOR = 5;

        /// <summary>
        /// Default rpc_ver_minor
        /// </summary>
        public const byte DEFAULT_RPC_VERSION_MINOR = 0;

        /// <summary>
        /// Default NDR version
        /// </summary>
        public const RpceNdrVersion DEFAULT_NDR_VERSION = RpceNdrVersion.NDR;

        /// <summary>
        /// Default max transmit fragment size.
        /// 5840 is default value for Windows.
        /// </summary>
        public const ushort DEFAULT_MAX_TRANSMIT_FRAGMENT_SIZE = 5840;

        /// <summary>
        /// Default max receive fragment size.
        /// 5840 is default value for Windows.
        /// </summary>
        public const ushort DEFAULT_MAX_RECEIVE_FRAGMENT_SIZE = 5840;

        /// <summary>
        /// The value for the Signature field that specifies the presence 
        /// of extended error information in a bind_nak PDU MUST be 
        /// 90740320-fad0-11d3-82d7-009027b130ab.
        /// </summary>
        public static readonly Guid BINDNAK_SIGNATURE = new Guid("90740320-fad0-11d3-82d7-009027b130ab");


        // Length of p_rt_versions_supported_t header.
        internal const int P_RT_VERSIONS_SUPPORTED_T_HEADER_SIZE = 1;

        // Length of version_t.
        internal const int VERSION_T_SIZE = 2;

        // Length of port_any_t header.
        internal const int PORT_ANY_T_HEADER_SIZE = 2;

        // Length of p_cont_list_t header.
        internal const int P_CONT_LIST_T_HEADER_SIZE = 4;

        // Length of p_result_list_t header.
        internal const int P_RESULT_LIST_T_HEADER_SIZE = 4;

        // Length of auth_verifier.
        internal const int AUTH_VERIFIER_SIZE = 8;

        // Length of p_syntaxid_t.
        internal const int P_SYNTAXID_T_SIZE = 20;

        // Length of p_syntax_id_t.
        internal const int P_SYNTAX_ID_T_SIZE = 20;

        // Length of GUID.
        internal const int GUID_SIZE = 16;

        // Length of p_result_t.
        internal const int P_RESULT_T_SIZE = 24;

        // Length of RPCE C/O common header.
        internal const int CO_PDU_HEADER_SIZE = 16;

        // Offset of data_representation field
        internal const int DREP_FIELD_OFFSET = 4;

        // Offset of frag_length field.
        internal const int FRAG_LENGTH_FIELD_OFFSET = 8;

        // Pad stub to be 4 bytes blocks.
        internal const int STUB_PAD_LENGTH = 4;

        // Pad stub to be 16 bytes blocks.
        internal const int AUTH_PAD_LENGTH = 16;

        // Prefix length of BindTimeFeatureNegotiationBitmask
        internal const int BIND_TIME_FEATURE_NEGOTIATION_BITMASK_PREFIX_LENGTH = 8;

        // BindTimeFeatureNegotiationBitmask bytes
        internal readonly static byte[] BIND_TIME_FEATURE_NEGOTIATION_BITMASK_GUID_BYTES
            = new byte[] { 0x2C, 0x1C, 0xB7, 0x6C, 0x12, 0x98, 0x40, 0x45, 
                           0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        // Endpoint prefix for RPC over named pipe 
        internal const string NAMED_PIPE_ENDPOINT_PREFIX = @"\PIPE\";

        // TCP port for named pipe
        internal const int NAMED_PIPE_PORT = 445;

        // SMB/SMB2 share name for named pipe
        internal const string NAMED_PIPE_SHARENAME = "IPC$";

        // indicates whether to disable smb2
        private static bool disableSmb2;

        /// <summary>
        /// Do not allow using SMB2 as RPCE underlying transport.
        /// DisableSmb2 is false by default, which means RPCE client will try to connect using SMB2 first, 
        /// if failed, try SMB next.
        /// RPCE server will choose SMB2 as its transport if possible.
        /// </summary>
        public static bool DisableSmb2
        {
            get
            {
                return disableSmb2;
            }
            set
            {
                disableSmb2 = value;
            }
        }


        /// <summary>
        /// ALIGN<para/>
        /// Example: align(33, 4) == 36
        /// </summary>
        /// <param name="value">value to be aligned.</param>
        /// <param name="align">alignment.</param>
        /// <returns>Aligned value.</returns>
        public static int Align(int value, int align)
        {
            checked { align--; }
            return (value + align) & ~align;
        }


        /// <summary>
        /// Create an instance of auth_verifier_co_t.
        /// </summary>
        /// <param name="packetType">PTYPE</param>
        /// <param name="stubLength">stub length</param>
        /// <param name="securityContext">security context</param>
        /// <param name="type">auth_type</param>
        /// <param name="level">auth_level</param>
        /// <param name="contextId">auth_context_id</param>
        /// <returns>an auth_verifier_co_t instance.</returns>
        internal static auth_verifier_co_t? AuthVerifierCreateInstance(
            RpcePacketType packetType,
            int stubLength,
            SecurityContext securityContext,
            RpceAuthenticationType type,
            RpceAuthenticationLevel level,
            uint contextId)
        {
            if (type == RpceAuthenticationType.RPC_C_AUTHN_NONE
                || level == RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_NONE)
            {
                return null;
            }

            if (level <= RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_CONNECT)
            {
                if (packetType != RpcePacketType.Bind
                    && packetType != RpcePacketType.BindAck
                    && packetType != RpcePacketType.BindNak
                    && packetType != RpcePacketType.AlterContext
                    && packetType != RpcePacketType.AlterContextResp
                    && packetType != RpcePacketType.Auth3)
                {
                    return null;
                }
            }

            //The authentication verifier is never present in bind_nak and shutdown PDUs
            if (packetType == RpcePacketType.BindNak
                || packetType == RpcePacketType.Shutdown)
            {
                return null;
            }

            auth_verifier_co_t authVerifier = new auth_verifier_co_t();

            //The sec_trailer structure MUST be 4-byte aligned with respect 
            //to the beginning of the PDU. Padding octets MUST be used to 
            //align the sec_trailer structure if its natural beginning is not 
            //already 4-byte aligned.
            authVerifier.auth_pad_length = (byte)(Align(stubLength, STUB_PAD_LENGTH) - stubLength);
            authVerifier.auth_pad = new byte[authVerifier.auth_pad_length];

            authVerifier.auth_type = (byte)type;
            authVerifier.auth_level = (byte)level;
            authVerifier.auth_reserved = 0;
            authVerifier.auth_context_id = contextId;

            if (securityContext != null)
            {
                if (packetType == RpcePacketType.Bind
                    || packetType == RpcePacketType.BindAck
                    || packetType == RpcePacketType.AlterContext
                    || packetType == RpcePacketType.AlterContextResp
                    || packetType == RpcePacketType.Auth3)
                {
                    authVerifier.auth_value = securityContext.Token;
                }
                else if (level == RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_PRIVACY)
                {
                    int authValueSize = Align(
                        (int)securityContext.ContextSizes.SecurityTrailerSize,
                        STUB_PAD_LENGTH);
                    authVerifier.auth_value = new byte[authValueSize];
                }
                else 
                {
                    // level == RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_INTEGRITY ||
                    // level == RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT ||
                    // level == RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_CALL
                    int authValueSize = Align(
                        (int)securityContext.ContextSizes.MaxSignatureSize,
                        STUB_PAD_LENGTH);
                    authVerifier.auth_value = new byte[authValueSize];
                }
            }

            return authVerifier;
        }


        /// <summary>
        /// Get size of auth_verifier.
        /// </summary>
        /// <param name="authVerifier">auth_verifier</param>
        /// <returns>size</returns>
        internal static ushort AuthVerifierGetSize(
            auth_verifier_co_t? authVerifier)
        {
            ushort size = 0;
            if (authVerifier != null)
            {
                size += AUTH_VERIFIER_SIZE;
                if (authVerifier.Value.auth_pad != null)
                {
                    size += (ushort)authVerifier.Value.auth_pad.Length;
                }
                if (authVerifier.Value.auth_value != null)
                {
                    size += (ushort)authVerifier.Value.auth_value.Length;
                }
            }
            return size;
        }


        /// <summary>
        /// Write auth_verifier into binary writer.
        /// </summary>
        /// <param name="binaryWriter">binary writer</param>
        /// <param name="authVerifier">auth_verifier</param>
        internal static void AuthVerifierToBytes(
            BinaryWriter binaryWriter,
            auth_verifier_co_t? authVerifier)
        {
            if (authVerifier != null)
            {
                if (authVerifier.Value.auth_pad != null)
                {
                    binaryWriter.Write(authVerifier.Value.auth_pad);
                }
                binaryWriter.Write(authVerifier.Value.auth_type);
                binaryWriter.Write(authVerifier.Value.auth_level);
                binaryWriter.Write(authVerifier.Value.auth_pad_length);
                binaryWriter.Write(authVerifier.Value.auth_reserved);
                binaryWriter.Write(authVerifier.Value.auth_context_id);
                if (authVerifier.Value.auth_value != null)
                {
                    binaryWriter.Write(authVerifier.Value.auth_value);
                }
            }
        }


        /// <summary>
        /// Read auth_verifier from binary reader.
        /// </summary>
        /// <param name="binaryReader">binary reader</param>
        /// <param name="authLength">auth_length</param>
        /// <returns>auth_varifier</returns>
        internal static auth_verifier_co_t? AuthVerifierFromBytes(
            BinaryReader binaryReader,
            ushort authLength)
        {
            if (authLength == 0)
            {
                return null;
            }

            long currentPosition = binaryReader.BaseStream.Position;

            int fragLength = (int)binaryReader.BaseStream.Length;
            int authVerifierStartPos = fragLength - authLength - AUTH_VERIFIER_SIZE;
            binaryReader.BaseStream.Position = authVerifierStartPos;

            auth_verifier_co_t authVerifier = new auth_verifier_co_t();
            authVerifier.auth_type = binaryReader.ReadByte();
            authVerifier.auth_level = binaryReader.ReadByte();
            authVerifier.auth_pad_length = binaryReader.ReadByte();
            authVerifier.auth_reserved = binaryReader.ReadByte();
            authVerifier.auth_context_id = binaryReader.ReadUInt32();
            authVerifier.auth_value = binaryReader.ReadBytes(authLength);

            binaryReader.BaseStream.Position
                = authVerifierStartPos - authVerifier.auth_pad_length;
            authVerifier.auth_pad = binaryReader.ReadBytes(authVerifier.auth_pad_length);

            binaryReader.BaseStream.Position = currentPosition;

            return authVerifier;
        }


        #region FragmentPdu and ReassemblePdu

        /// <summary>
        /// Fragment a PDU into several PDUs by max_xmit_frag field.<para/>
        /// Only bind, bind_ack, alter_context, alter_context_response,
        /// request and response PDUs will be fragmented.<para/>
        /// Must call before sign/encrypt.
        /// </summary>
        /// <param name="context">RpceContext to fragment PDU</param>
        /// <param name="pdu">
        /// A PDU to be fragmented. 
        /// Only bind, bind_ack, alter_context, alter_context_response,
        /// request and response PDUs will be fragmented.
        /// </param>
        /// <returns>Fragmented PDUs.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when pdu or context is null.
        /// </exception>
        /// <exception cref="NotSupportedException">Thrown when PDU PacketType isn't supported.</exception>
        public static RpceCoPdu[] FragmentPdu(RpceContext context, RpceCoPdu pdu)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (pdu == null)
            {
                throw new ArgumentNullException("pdu");
            }

            if (pdu.frag_length > context.MaxTransmitFragmentSize)
            {
                int headerAndTrailerSize;
                byte[] stub;

                switch (pdu.PTYPE)
                {
                    case RpcePacketType.Request:

                        //Get stub of request PDU
                        RpceCoRequestPdu requestPdu = pdu as RpceCoRequestPdu;

                        if (requestPdu == null)
                        {
                            throw new ArgumentException("The pdu is not a valid RpceCoRequestPdu.");
                        }

                        headerAndTrailerSize = requestPdu.GetSize();

                        if (requestPdu.auth_verifier != null)
                        {
                            //length of auth_verifier
                            headerAndTrailerSize += RpceUtility.AUTH_VERIFIER_SIZE;
                            headerAndTrailerSize += requestPdu.auth_verifier.Value.auth_value.Length;

                            //To keep stub always be padded to 16 bytes, and pdu doesnot exceed max transmit frag size.
                            int stubLength = context.MaxTransmitFragmentSize - headerAndTrailerSize;
                            headerAndTrailerSize +=
                                RpceUtility.Align(stubLength, RpceUtility.AUTH_PAD_LENGTH) - stubLength;

                            //The beginning of the verification_trailer header MUST be 4-byte aligned 
                            //with respect to the beginning of the PDU.
                            headerAndTrailerSize = RpceUtility.Align(headerAndTrailerSize, RpceUtility.STUB_PAD_LENGTH);
                        }

                        stub = requestPdu.stub ?? new byte[0];

                        //Fragment
                        RpceCoRequestPdu[] requestFragmentPduList = FragmentPdu<RpceCoRequestPdu>(
                            context,
                            pdu,
                            stub.Length,
                            headerAndTrailerSize);

                        for (int i = 0; i < requestFragmentPduList.Length; i++)
                        {
                            //SHOULD set the alloc_hint field in every PDU to 
                            //be the combined stub data length of all remaining fragment PDUs.
                            requestFragmentPduList[i].alloc_hint = (uint)stub.Length;
                            requestFragmentPduList[i].p_cont_id = requestPdu.p_cont_id;
                            requestFragmentPduList[i].opnum = requestPdu.opnum;
                            requestFragmentPduList[i].@object = requestPdu.@object;
                            requestFragmentPduList[i].stub = ArrayUtility.SubArray(
                                stub,
                                0,
                                Math.Min(stub.Length, context.MaxTransmitFragmentSize - headerAndTrailerSize));

                            //For request and response PDUs, where the request and response PDUs are 
                            //part of a fragmented request or response and authentication is requested, 
                            //the sec_trailer structure MUST be present in every fragment of the request 
                            //or response.
                            requestFragmentPduList[i].AppendAuthenticationVerifier();
                            requestFragmentPduList[i].SetLength();
                            stub = ArrayUtility.SubArray(stub, requestFragmentPduList[i].stub.Length);
                        }

                        return requestFragmentPduList;

                    case RpcePacketType.Response:

                        //Get stub of response PDU
                        RpceCoResponsePdu responsePdu = pdu as RpceCoResponsePdu;

                        if (responsePdu == null)
                        {
                            throw new ArgumentException("The PDU is not a valid RpceCoResponsePdu");
                        }

                        headerAndTrailerSize = responsePdu.GetSize();

                        if (responsePdu.auth_verifier != null)
                        {
                            //length of auth_verifier
                            headerAndTrailerSize += RpceUtility.AUTH_VERIFIER_SIZE;
                            headerAndTrailerSize += responsePdu.auth_verifier.Value.auth_value.Length;

                            //To keep stub always be padded to 16 bytes, and pdu doesnot exceed max transmit frag size.
                            int stubLength = context.MaxTransmitFragmentSize - headerAndTrailerSize;
                            headerAndTrailerSize +=
                                RpceUtility.Align(stubLength, RpceUtility.AUTH_PAD_LENGTH) - stubLength;

                            //The beginning of the verification_trailer header MUST be 4-byte aligned 
                            //with respect to the beginning of the PDU.
                            headerAndTrailerSize = RpceUtility.Align(headerAndTrailerSize, RpceUtility.STUB_PAD_LENGTH);
                        }

                        stub = responsePdu.stub ?? new byte[0];

                        //Fragment
                        RpceCoResponsePdu[] responseFragmentPduList = FragmentPdu<RpceCoResponsePdu>(
                            context,
                            pdu,
                            stub.Length,
                            headerAndTrailerSize);

                        for (int i = 0; i < responseFragmentPduList.Length; i++)
                        {
                            //SHOULD set the alloc_hint field in every PDU to 
                            //be the combined stub data length of all remaining fragment PDUs.
                            responseFragmentPduList[i].alloc_hint = (uint)stub.Length;
                            responseFragmentPduList[i].p_cont_id = responsePdu.p_cont_id;
                            responseFragmentPduList[i].cancel_count = responsePdu.cancel_count;
                            responseFragmentPduList[i].reserved = responsePdu.reserved;
                            responseFragmentPduList[i].stub = ArrayUtility.SubArray(
                                stub,
                                0,
                                Math.Min(stub.Length, context.MaxTransmitFragmentSize - headerAndTrailerSize));

                            //For request and response PDUs, where the request and response PDUs are 
                            //part of a fragmented request or response and authentication is requested, 
                            //the sec_trailer structure MUST be present in every fragment of the request 
                            //or response.
                            responseFragmentPduList[i].AppendAuthenticationVerifier();
                            responseFragmentPduList[i].SetLength();
                            stub = ArrayUtility.SubArray(stub, responseFragmentPduList[i].stub.Length);
                        }

                        return responseFragmentPduList;

                    case RpcePacketType.Bind:
                    case RpcePacketType.BindAck:
                    case RpcePacketType.AlterContext:
                    case RpcePacketType.AlterContextResp:
                    case RpcePacketType.Auth3:
                        //Windows RPC support version 5.0 only.
                        //Bind fragment requires RPC ver 5.1.
                        //We don't support it.
                        throw new NotSupportedException("bind/bind_ack/alt_context/alt_context_resp/auth3 PDU fragment are not supported.");

                    default:
                        throw new InvalidOperationException("PDU PacketType isn't supported.");
                }
            }

            //If we cannot fragment the PDU
            return new RpceCoPdu[] { pdu };
        }


        /// <summary>
        /// Fragment a PDU
        /// </summary>
        /// <param name="context">RpceContext to fragment PDU</param>
        /// <typeparam name="T">Type of PDU to fragment</typeparam>
        /// <param name="pdu">PDU to fragment</param>
        /// <param name="stubLength">length of data to be fragmented</param>
        /// <param name="extraLength">length of header and trailer</param>
        /// <returns>Fragmented PDUs</returns>
        private static T[] FragmentPdu<T>(
            RpceContext context,
            RpceCoPdu pdu,
            int stubLength,
            int extraLength) where T : RpceCoPdu
        {
            List<T> fragmentPduList = new List<T>();
            bool isFirstFragment = true;
            while (stubLength > 0)
            {
                T fragmentPdu = (T)Activator.CreateInstance(typeof(T), context);
                fragmentPdu.rpc_vers = pdu.rpc_vers;
                fragmentPdu.rpc_vers_minor = pdu.rpc_vers_minor;
                fragmentPdu.PTYPE = pdu.PTYPE;
                fragmentPdu.pfc_flags = pdu.pfc_flags
                    & ~(RpceCoPfcFlags.PFC_FIRST_FRAG | RpceCoPfcFlags.PFC_LAST_FRAG);
                fragmentPdu.packed_drep = pdu.packed_drep;
                fragmentPdu.frag_length = (ushort)Math.Min(context.MaxTransmitFragmentSize, stubLength + extraLength);
                fragmentPdu.auth_length = pdu.auth_length;
                fragmentPdu.call_id = pdu.call_id;

                if (isFirstFragment)
                {
                    fragmentPdu.pfc_flags |= RpceCoPfcFlags.PFC_FIRST_FRAG;
                    isFirstFragment = false;
                }

                if (stubLength <= (context.MaxTransmitFragmentSize - extraLength))
                {
                    fragmentPdu.pfc_flags |= RpceCoPfcFlags.PFC_LAST_FRAG;
                }

                stubLength -= (context.MaxTransmitFragmentSize - extraLength);
                fragmentPduList.Add(fragmentPdu);
            }

            return fragmentPduList.ToArray();
        }


        /// <summary>
        /// Reassemble several fragment PDUs to one PDU.<para/>
        /// Must call after decrypt/verify.
        /// </summary>
        /// <param name="context">RpceContext to Reassemble PDU.</param>
        /// <param name="pdus">Fragment PDUs to be reassembled.</param>
        /// <returns>A ressembled PDU.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when pdus is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when pdus is invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">Thrown when Speicified PDU doesn't 
        /// support fragment and reassemble.</exception>
        public static RpceCoPdu ReassemblePdu(RpceContext context, params RpceCoPdu[] pdus)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (pdus == null)
            {
                throw new ArgumentNullException("pdus");
            }

            if (pdus.Length == 0)
            {
                throw new ArgumentException("There's no PDU to reassemble.", "pdus");
            }

            //verify if the fisrt pdu has the valid PFC_FIRST_FRAG. 
            if ((pdus[0].pfc_flags & RpceCoPfcFlags.PFC_FIRST_FRAG) == 0)
            {
                throw new ArgumentException("First PDU doesn't have PFC_FIRST_FRAG flag.", "pdus");
            }

            //verify if fragments end expected.
            for (int i = 0; i < pdus.Length - 1; i++)
            {
                if ((pdus[i].pfc_flags & RpceCoPfcFlags.PFC_LAST_FRAG) != 0)
                {
                    throw new ArgumentException("Fragments ended unexpected.", "pdus");
                }
            }

            if ((pdus[pdus.Length - 1].pfc_flags & RpceCoPfcFlags.PFC_LAST_FRAG) == 0)
            {
                throw new ArgumentException("Fragments is not ended.", "pdus");
            }

            //All PTYPEs should be the same.
            for (int i = 1; i < pdus.Length; i++)
            {
                if (pdus[i].PTYPE != pdus[0].PTYPE)
                {
                    throw new ArgumentException("PDUs' PTYPE are different.", "pdus");
                }
            }

            if (pdus.Length == 1)
            {
                return pdus[0];
            }

            RpceCoPdu reassembledPdu;
            switch (pdus[0].PTYPE)
            {
                case RpcePacketType.Request:
                    RpceCoRequestPdu requestPdu = pdus[0] as RpceCoRequestPdu;

                    if (requestPdu == null)
                    {
                        throw new ArgumentException("The PDU is not a valid RpceCoRequestPdu");
                    }

                    for (int i = 1; i < pdus.Length; i++)
                    {
                        requestPdu.stub = ArrayUtility.ConcatenateArrays(
                            requestPdu.stub,
                            ((RpceCoRequestPdu)pdus[i]).stub);
                    }

                    requestPdu.pfc_flags |= RpceCoPfcFlags.PFC_LAST_FRAG;
                    requestPdu.frag_length = (ushort)(requestPdu.GetSize() + requestPdu.stub.Length);
                    requestPdu.auth_length = 0;
                    requestPdu.auth_verifier = null;
                    reassembledPdu = requestPdu;
                    break;

                case RpcePacketType.Response:
                    RpceCoResponsePdu responsePdu = pdus[0] as RpceCoResponsePdu;

                    if (responsePdu == null)
                    {
                        throw new ArgumentException("The PDU is not a valid RpceCoResponsePdu");
                    }

                    for (int i = 1; i < pdus.Length; i++)
                    {
                        responsePdu.stub = ArrayUtility.ConcatenateArrays(
                            responsePdu.stub,
                            ((RpceCoResponsePdu)pdus[i]).stub);
                    }

                    responsePdu.pfc_flags |= RpceCoPfcFlags.PFC_LAST_FRAG;
                    responsePdu.frag_length = (ushort)(responsePdu.GetSize() + responsePdu.stub.Length);
                    responsePdu.auth_length = 0;
                    responsePdu.auth_verifier = null;
                    reassembledPdu = responsePdu;
                    break;

                case RpcePacketType.Bind:
                case RpcePacketType.BindAck:
                case RpcePacketType.AlterContext:
                case RpcePacketType.AlterContextResp:
                case RpcePacketType.Auth3:
                    //Windows RPC support version 5.0 only.
                    //Bind fragment requires RPC ver 5.1.
                    //We don't support it.
                    throw new NotSupportedException("bind/bind_ack/alt_context/alt_context_resp/auth3 PDU fragment are not supported.");

                default:
                    throw new InvalidOperationException("Speicified PDU doesn't support fragment and reassemble.");
            }

            return reassembledPdu;
        }


        #endregion


        #region Decoding

        /// <summary>
        /// Decode CO PDU.
        /// </summary>
        /// <param name="context">The context that received data.</param>
        /// <param name="messageBytes">bytes received</param>
        /// <param name="consumedLength">num of bytes consumed in processing</param>
        /// <param name="expectedLength">num of bytes expected if the bytes is not enough</param>
        /// <returns>pdus</returns>
        internal static RpceCoPdu[] DecodeCoPdu(
            RpceContext context,
            byte[] messageBytes,
            out int consumedLength,
            out int expectedLength)
        {
            List<RpceCoPdu> pduList = new List<RpceCoPdu>();

            consumedLength = 0;
            expectedLength = 0;

            while (consumedLength < messageBytes.Length)
            {
                if ((messageBytes.Length - consumedLength) < RpceUtility.CO_PDU_HEADER_SIZE)
                {
                    expectedLength = RpceUtility.CO_PDU_HEADER_SIZE;
                    break;
                }

                //#4 byte is drep
                uint dataRepresentation = BitConverter.ToUInt32(
                    messageBytes,
                    consumedLength + RpceUtility.DREP_FIELD_OFFSET);
                //#8 byte is frag_length
                ushort fragmentLength = BitConverter.ToUInt16(
                    messageBytes,
                    consumedLength + RpceUtility.FRAG_LENGTH_FIELD_OFFSET);
                if ((dataRepresentation & 0x0000FFFFU) != NativeMethods.NDR_LOCAL_DATA_REPRESENTATION)
                {
                    fragmentLength = EndianUtility.ReverseByteOrder(fragmentLength);
                }

                if ((messageBytes.Length - consumedLength) < fragmentLength)
                {
                    expectedLength = fragmentLength;
                    break;
                }

                byte[] pduBytes = new byte[fragmentLength];
                Buffer.BlockCopy(messageBytes, consumedLength, pduBytes, 0, fragmentLength);

                RpceCoPdu pdu = RpceUtility.DecodeCoPdu(context, pduBytes);

                pduList.Add(pdu);
                consumedLength += fragmentLength;
            }

            return pduList.ToArray();
        }


        /// <summary>
        /// Decode CO PDU.
        /// </summary>
        /// <param name="context">The context that received data.</param>
        /// <param name="pduBytes">Received bytes</param>
        /// <returns>pdus</returns>
        /// <exception cref="ArgumentNullException">Thrown when context is null.</exception>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public static RpceCoPdu DecodeCoPdu(
            RpceContext context,
            byte[] pduBytes)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (pduBytes == null)
            {
                throw new ArgumentNullException("pduBytes");
            }

            RpceCoPdu pdu;

            //#2 byte is PTYPE
            RpcePacketType packageType = (RpcePacketType)pduBytes[2];
            //#3 byte is PFC_*** flags
            RpceCoPfcFlags pfcFlags = (RpceCoPfcFlags)pduBytes[3];

            RpceCoPfcFlags pfcFlagsNoFragment = RpceCoPfcFlags.PFC_FIRST_FRAG | RpceCoPfcFlags.PFC_LAST_FRAG;
            if (((pfcFlags & pfcFlagsNoFragment) != pfcFlagsNoFragment)
                && (packageType == RpcePacketType.Bind
                    || packageType == RpcePacketType.BindAck
                    || packageType == RpcePacketType.AlterContext
                    || packageType == RpcePacketType.AlterContextResp
                    || packageType == RpcePacketType.Auth3))
            {
                //If it's a fragment and PTYPE is bind/bind_ack/alter_context/alter_context_resp
                //Windows RPC support version 5.0 only.
                //Bind fragment requires RPC ver 5.1.
                //We don't support it.
                throw new NotSupportedException("bind/bind_ack/alt_context/alt_context_resp/auth3 PDU fragment are not supported.");
            }
            else
            {
                switch (packageType)
                {
                    case RpcePacketType.Bind:
                        pdu = new RpceCoBindPdu(context, pduBytes);
                        break;

                    case RpcePacketType.BindAck:
                        pdu = new RpceCoBindAckPdu(context, pduBytes);
                        break;

                    case RpcePacketType.BindNak:
                        pdu = new RpceCoBindNakPdu(context, pduBytes);
                        break;

                    case RpcePacketType.AlterContext:
                        pdu = new RpceCoAlterContextPdu(context, pduBytes);
                        break;

                    case RpcePacketType.AlterContextResp:
                        pdu = new RpceCoAlterContextRespPdu(context, pduBytes);
                        break;

                    case RpcePacketType.Auth3:
                        pdu = new RpceCoAuth3Pdu(context, pduBytes);
                        break;

                    case RpcePacketType.Request:
                        pdu = new RpceCoRequestPdu(context, pduBytes);
                        break;

                    case RpcePacketType.Response:
                        pdu = new RpceCoResponsePdu(context, pduBytes);
                        break;

                    case RpcePacketType.Fault:
                        pdu = new RpceCoFaultPdu(context, pduBytes);
                        break;

                    case RpcePacketType.CoCancel:
                        pdu = new RpceCoCancelPdu(context, pduBytes);
                        break;

                    case RpcePacketType.Orphaned:
                        pdu = new RpceCoOrphanedPdu(context, pduBytes);
                        break;

                    case RpcePacketType.Shutdown:
                        pdu = new RpceCoShutdownPdu(context, pduBytes);
                        break;

                    default:
                        throw new InvalidOperationException(
                            string.Format("Receive invalid packet - {0}.", packageType));
                }
            }

            return pdu;
        }

        #endregion


        #region GeneratePfcFlags

        /// <summary>
        /// Generate PFC_*** based on context and package type.
        /// </summary>
        /// <param name="context">Context of the session.</param>
        /// <param name="packageType">package type.</param>
        /// <returns>RFC_*** flag.</returns>
        internal static RpceCoPfcFlags GeneratePfcFlags(RpceContext context, RpcePacketType packageType)
        {
            RpceServerSessionContext sessionContext = context as RpceServerSessionContext;

            RpceCoPfcFlags flags = RpceCoPfcFlags.PFC_FIRST_FRAG | RpceCoPfcFlags.PFC_LAST_FRAG;
            
            if (sessionContext == null) //client-side
            {
                if (context.SupportsConcurrentMultiplexing)
                {
                    flags |= RpceCoPfcFlags.PFC_CONC_MPX;
                }
            }
            else if ( //server-side
                    (packageType == RpcePacketType.BindAck 
                    && sessionContext.ServerContext.SupportsConcurrentMultiplexing) // first response, read server context
                || sessionContext.SupportsConcurrentMultiplexing) // if it's not first response, we can read session context
            {
                flags |= RpceCoPfcFlags.PFC_CONC_MPX;
            }

            if ((packageType == RpcePacketType.Bind ||
                packageType == RpcePacketType.BindAck ||
                packageType == RpcePacketType.AlterContext ||
                packageType == RpcePacketType.AlterContextResp)
                &&
                context.SupportsHeaderSign)
            {
                if (sessionContext == null //client-side
                    || sessionContext.ServerContext.SupportsHeaderSign) //server-side
                {
                    flags |= RpceCoPfcFlags.PFC_SUPPORT_HEADER_SIGN;
                }
            }

            return flags;
        }

        #endregion


        /// <summary>
        /// Append verification_trailer to stub.
        /// </summary>
        /// <param name="stub">The stub.</param>
        /// <param name="verificationTrailer">verification_trailer</param>
        /// <returns>The stub with verification_trailer.</returns>
        /// <exception cref="ArgumentNullException">Thrown when stub or verificationTrailer is null.</exception>
        public static byte[] AppendVerificationTrailerToStub(byte[] stub, verification_trailer_t verificationTrailer)
        {
            if (stub == null)
            {
                throw new ArgumentNullException("stub");
            }

            if (verificationTrailer == null)
            {
                throw new ArgumentNullException("verificationTrailer");
            }

            //The beginning of the header MUST be 4-byte aligned with respect to the beginning of the PDU.
            int padLength = RpceUtility.Align(stub.Length, 4) - stub.Length;
            verificationTrailer.pad = new byte[padLength];
            byte[] verificationTrailerBytes = verificationTrailer.ToBytes();
            return ArrayUtility.ConcatenateArrays(stub, verificationTrailerBytes);
        }
        
        
        /// <summary>
        /// Extract verification_trailer from stub.
        /// </summary>
        /// <param name="stub">The stub.</param>
        /// <param name="startIndex">
        /// Input: The position of the end of stub (the beginning position to search for verification_trailer).
        /// Output: The start position of verification_trailer.
        /// </param>
        /// <returns>Returns verification_trailer if found; otherwise, returns null.</returns>
        /// <exception cref="ArgumentNullException">Thrown when stub is null.</exception>
        public static verification_trailer_t ExtractVerificationTrailerFromStub(byte[] stub, ref int startIndex)
        {
            if (stub == null)
            {
                throw new ArgumentNullException("stub");
            }

            //The beginning of the header MUST be 4-byte aligned with respect to the beginning of the PDU.
            for (int i = RpceUtility.Align(startIndex, 4); i < stub.Length; i += 4)
            {
                if ((stub.Length - i) >= Marshal.SizeOf(verification_trailer_t.SIGNATURE)
                    && BitConverter.ToUInt64(stub, i) == verification_trailer_t.SIGNATURE)
                {
                    //found verification_trailer
                    byte[] verificationTrailerBuffer = ArrayUtility.SubArray(stub, i);
                    verification_trailer_t verificationTrailer = new verification_trailer_t();
                    verificationTrailer.FromBytes(verificationTrailerBuffer);
                    startIndex = i;
                    return verificationTrailer;
                }
            }

            return null;
        }
    }
}

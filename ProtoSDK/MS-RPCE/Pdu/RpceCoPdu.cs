// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// RpceCoPdu is a base class of all connection-oriented PDU.
    /// </summary>
    public abstract class RpceCoPdu : RpcePdu
    {

        /// <summary>
        /// Initialize an instance of RpceCoPdu class.
        /// </summary>
        /// <param name="context">context</param>
        protected RpceCoPdu(RpceContext context)
            : base(context)
        {
        }


        /// <summary>
        /// Initialize an instance of RpceCoPdu class, and 
        /// unmarshal a byte array to PDU struct.
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="pduBytes">A byte array contains PDU data.</param>
        // Suppress CA2214 because calling virtual FromBytes is what we want.
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        protected RpceCoPdu(RpceContext context, byte[] pduBytes)
            : base(context, pduBytes)
        {
            using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(pduBytes)))
            {
                FromBytes(binaryReader);
            }
        }


        /// <summary>
        /// RPC version
        /// </summary>
        public byte rpc_vers;

        /// <summary>
        /// minor version
        /// </summary>
        public byte rpc_vers_minor;

        /// <summary>
        /// packet type
        /// </summary>
        public RpcePacketType PTYPE;

        /// <summary>
        /// flags (see PFC_... )
        /// </summary>
        public RpceCoPfcFlags pfc_flags;

        /// <summary>
        /// NDR data representation format label
        /// </summary>
        public DataRepresentationFormatLabel packed_drep;

        /// <summary>
        /// total length of fragment
        /// </summary>
        public ushort frag_length;

        /// <summary>
        /// length of auth_value
        /// </summary>
        public ushort auth_length;

        /// <summary>
        /// call identifier
        /// </summary>
        public uint call_id;

        /// <summary>
        /// Everytime initialized or accepted a security token, 
        /// this will indicates whether need continue processing or not.
        /// It is usually equals to SecurityContext.NeedContinueProcessing, 
        /// but in some situations (like server-side), this value is more 
        /// reliable because it will not be overridden by multi-thread.
        /// </summary>
        internal bool securityContextNeedContinueProcessing;


        /// <summary>
        /// Marshal the PDU struct to a byte array.
        /// </summary>
        /// <returns>A byte array contains PDU data.</returns>
        public override byte[] ToBytes()
        {
            int size = GetSize();

            FieldInfo fieldInfo = GetType().GetField("stub");
            if (fieldInfo != null)
            {
                byte[] stub = (byte[])fieldInfo.GetValue(this);
                if (stub != null)
                {
                    size += stub.Length;
                }
            }

            fieldInfo = GetType().GetField("auth_verifier");
            if (fieldInfo != null)
            {
                auth_verifier_co_t? authVerifier = (auth_verifier_co_t?)fieldInfo.GetValue(this);
                size += RpceUtility.AuthVerifierGetSize(authVerifier);
            }

            byte[] pduBytes = new byte[size];
            using (BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(pduBytes)))
            {
                ToBytes(binaryWriter);
            }

            return pduBytes;
        }


        /// <summary>
        /// Get the size of structure. 
        /// Length of stub, auth_verifier and auth_value is not included.
        /// </summary>
        /// <returns>The size</returns>
        internal override int GetSize()
        {
            return RpceUtility.CO_PDU_HEADER_SIZE;
        }


        /// <summary>
        /// Marshal the PDU struct to a byte array.
        /// </summary>
        /// <param name="binaryWriter">BinaryWriter</param>
        internal override void ToBytes(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(rpc_vers);
            binaryWriter.Write(rpc_vers_minor);
            binaryWriter.Write((byte)PTYPE);
            binaryWriter.Write((byte)pfc_flags);
            binaryWriter.Write((ushort)packed_drep.dataRepFormat);
            binaryWriter.Write((ushort)packed_drep.reserved);
            binaryWriter.Write(frag_length);
            binaryWriter.Write(auth_length);
            binaryWriter.Write(call_id);
        }


        /// <summary>
        /// Un-marshal a byte array to PDU struct.
        /// </summary>
        /// <param name="binaryReader">A binary reader.</param>
        internal override void FromBytes(BinaryReader binaryReader)
        {
            rpc_vers = binaryReader.ReadByte();
            rpc_vers_minor = binaryReader.ReadByte();
            PTYPE = (RpcePacketType)binaryReader.ReadByte();
            pfc_flags = (RpceCoPfcFlags)binaryReader.ReadByte();
            packed_drep = new DataRepresentationFormatLabel();
            packed_drep.dataRepFormat = (RpceDataRepresentationFormat)binaryReader.ReadUInt16();
            packed_drep.reserved = binaryReader.ReadUInt16();
            frag_length = binaryReader.ReadUInt16();
            auth_length = binaryReader.ReadUInt16();
            call_id = binaryReader.ReadUInt32();

            if (packed_drep.dataRepFormat != RpceDataRepresentationFormat.IEEE_LittleEndian_ASCII)
            {
                frag_length = EndianUtility.ReverseByteOrder(frag_length);
                auth_length = EndianUtility.ReverseByteOrder(auth_length);
                call_id = EndianUtility.ReverseByteOrder(call_id);
            }
        }


        /// <summary>
        /// Append auth_verifier to the end of PDU.
        /// </summary>
        public override void AppendAuthenticationVerifier()
        {
            FieldInfo fieldInfo = this.GetType().GetField("auth_verifier");
            if (fieldInfo == null)
            {
                return;
            }

            byte[] stub = GetStub();

            auth_verifier_co_t? authVerifier = RpceUtility.AuthVerifierCreateInstance(
                PTYPE,
                stub != null ? stub.Length : 0,
                context.SecurityContext,
                context.AuthenticationType,
                context.AuthenticationLevel,
                context.AuthenticationContextId);

            fieldInfo.SetValue(this, authVerifier);
        }


        /// <summary>
        /// Set length field of PDU.
        /// </summary>
        public override void SetLength()
        {
            auth_verifier_co_t? authVerifier = null;
            FieldInfo fieldInfo = this.GetType().GetField("auth_verifier");
            if (fieldInfo != null)
            {
                authVerifier = (auth_verifier_co_t?)fieldInfo.GetValue(this);
            }

            byte[] stub = GetStub();
            int stubLength = (stub == null) ? 0 : stub.Length;

            // stub was already padded in AppendAuthenticationVerifier()
            auth_length = (authVerifier == null || authVerifier.Value.auth_value == null)
                            ? (ushort)0
                            : (ushort)authVerifier.Value.auth_value.Length;
            frag_length = (ushort)(GetSize() + stubLength + RpceUtility.AuthVerifierGetSize(authVerifier));
        }


        /// <summary>
        /// Encrypt stub and initial auth_token.
        /// </summary>
        public override void InitializeAuthenticationToken()
        {
            if (context.AuthenticationType == RpceAuthenticationType.RPC_C_AUTHN_NONE
                || context.AuthenticationLevel == RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_NONE)
            {
                // AUTHN_NONE and AUTHN_LEVEL_NONE, do nothing.
                return;
            }

            FieldInfo fieldInfo = this.GetType().GetField("auth_verifier");
            if (fieldInfo == null)
            {
                // PDU has no auth_verifier field, do nothing.
                return;
            }

            auth_verifier_co_t? authVerifier = (auth_verifier_co_t?)fieldInfo.GetValue(this);
            if (authVerifier == null)
            {
                // PDU has no auth_verifier, do nothing.
                return;
            }

            if (PTYPE == RpcePacketType.Bind
                || PTYPE == RpcePacketType.AlterContext
                || PTYPE == RpcePacketType.Auth3)
            {
                //Bind PDU, First call to GSS_Init_sec_context, as specified in [RFC2743] section 2.2.1. 
                //alter_context, rpc_auth_3, Second and subsequent calls to GSS_Init_sec_context, 
                //as specified in [RFC2743] section 2.2.1.
                auth_verifier_co_t newAuthVerifier = authVerifier.Value;
                newAuthVerifier.auth_value = context.SecurityContext.Token;
                authVerifier = newAuthVerifier;
            }
            else if (PTYPE == RpcePacketType.BindAck
                || PTYPE == RpcePacketType.AlterContextResp)
            {
                auth_verifier_co_t newAuthVerifier = authVerifier.Value;
                newAuthVerifier.auth_value = context.SecurityContext.Token;
                authVerifier = newAuthVerifier;
            }
            else if (context.AuthenticationLevel != RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_DEFAULT
                && context.AuthenticationLevel != RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_CONNECT)
            {
                auth_verifier_co_t newAuthVerifier = authVerifier.Value;
                EncryptAndSign(ref newAuthVerifier);
                authVerifier = newAuthVerifier;
            }

            // set it back to PDU.
            fieldInfo.SetValue(this, authVerifier);
        }


        /// <summary>
        /// Decrypt stub and validate auth_token.
        /// </summary>
        public override bool ValidateAuthenticationToken()
        {
            FieldInfo fieldInfo = this.GetType().GetField("auth_verifier");
            if (fieldInfo == null)
            {
                // PDU has no auth_verifier field, do nothing.
                return true;
            }

            auth_verifier_co_t? authVerifier = (auth_verifier_co_t?)fieldInfo.GetValue(this);
            if (authVerifier == null)
            {
                // PDU has no auth_verifier, do nothing.
                return true;
            }

            if (authVerifier.Value.auth_type != (byte)context.AuthenticationType
                || authVerifier.Value.auth_level != (byte)context.AuthenticationLevel
                || authVerifier.Value.auth_context_id != context.AuthenticationContextId)
            {
                //SecurityProvider in context is not the right SSPI to decrypt and validate the PDU.
                return false;
            }

            bool result = true;
            if (PTYPE == RpcePacketType.Bind
                || PTYPE == RpcePacketType.AlterContext
                || PTYPE == RpcePacketType.Auth3)
            {
                ServerSecurityContext serverSspi = context.SecurityContext as ServerSecurityContext;
                if (serverSspi != null)
                {
                    // Accept or Initialize should throw exception when token is incorrect.
                    serverSspi.Accept(authVerifier.Value.auth_value);
                    securityContextNeedContinueProcessing = serverSspi.NeedContinueProcessing;
                }
            }
            else if (PTYPE == RpcePacketType.BindAck
                || PTYPE == RpcePacketType.AlterContextResp)
            {
                ClientSecurityContext clientSspi = context.SecurityContext as ClientSecurityContext;
                if (clientSspi != null)
                {
                    //BindAck only received on client.
                    clientSspi.Initialize(authVerifier.Value.auth_value);
                    securityContextNeedContinueProcessing = clientSspi.NeedContinueProcessing;
                }
            }
            else if (context.AuthenticationLevel != RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_DEFAULT
                && context.AuthenticationLevel != RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_CONNECT)
            {
                auth_verifier_co_t newAuthVerifier = authVerifier.Value;
                result = DecryptAndVerify(ref newAuthVerifier);
                authVerifier = newAuthVerifier;
            }

            return result;
        }


        /// <summary>
        /// Decrypt and verify auth_token.
        /// </summary>
        /// <param name="authVerifier">auth_verifier, not null.</param>
        /// <returns>true if verify success; otherwise, false.</returns>
        private bool DecryptAndVerify(ref auth_verifier_co_t authVerifier)
        {
            bool result;

            //Just get the stub, no padding.
            byte[] stub = GetStub();
            if (stub == null)
            {
                stub = new byte[0];
                SetStub(stub);
            }

            //Get SecurityBuffers.
            int stubSecBufIndex;
            SecurityBufferType readonlyFlag;
            List<SecurityBuffer> securityBufferList = new List<SecurityBuffer>();
            if (context.SupportsHeaderSign)
            {
                readonlyFlag = SecurityBufferType.ReadOnlyWithChecksum;
            }
            else
            {
                readonlyFlag = SecurityBufferType.ReadOnly;
            }
            int headerSize = GetSize();
            securityBufferList.Add(
                new SecurityBuffer(
                    SecurityBufferType.Data | readonlyFlag,
                    ArrayUtility.SubArray(PacketBytes, 0, headerSize)));
            stubSecBufIndex = securityBufferList.Count;
            securityBufferList.Add(
                new SecurityBuffer(
                    SecurityBufferType.Data, 
                    ArrayUtility.SubArray(
                        PacketBytes, 
                        headerSize, 
                        stub.Length + authVerifier.auth_pad_length)));
            securityBufferList.Add(
                new SecurityBuffer(
                    SecurityBufferType.Data | readonlyFlag,
                    ArrayUtility.SubArray(
                        PacketBytes, 
                        headerSize + stub.Length + authVerifier.auth_pad_length, 
                        RpceUtility.AUTH_VERIFIER_SIZE)));
            securityBufferList.Add(
                new SecurityBuffer(SecurityBufferType.Token, authVerifier.auth_value));
            SecurityBuffer[] securityBuffers = securityBufferList.ToArray();

            //decrypt and validate
            switch (context.AuthenticationLevel)
            {
                case RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_CALL: //Same as RPC_C_AUTHN_LEVEL_PKT
                case RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT:
                    //the field "checksum" is the checksum value returned 
                    //by the underlying security service in response to 
                    //an integrity protection call
                    result = context.SecurityContext.Verify(securityBuffers);
                    break;

                case RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_INTEGRITY:
                    //the field "checksum" is the checksum value returned 
                    //by the underlying security service in response to 
                    //an integrity protection call
                    result = context.SecurityContext.Verify(securityBuffers);
                    break;

                case RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_PRIVACY:
                    //This level of service provides strong integrity protection for the
                    //entire PDU, plus privacy protection for the body data only. 
                    //Therefore, only the bodies of the request, response and fault PDUs 
                    //are encrypted.
                    result = context.SecurityContext.Decrypt(securityBuffers);
                    authVerifier.auth_pad = ArrayUtility.SubArray(
                        securityBuffers[stubSecBufIndex].Buffer, 
                        stub.Length);
                    stub = ArrayUtility.SubArray(
                        securityBuffers[stubSecBufIndex].Buffer, 
                        0, 
                        stub.Length);
                    SetStub(stub);
                    break;

                default:
                    //do nothing.
                    result = false;
                    break;
            }

            return result;
        }


        /// <summary>
        /// Encrypt and sign to get auth_token.
        /// </summary>
        /// <param name="authVerifier">auth_verifier, not null.</param>
        private void EncryptAndSign(ref auth_verifier_co_t authVerifier)
        {
            //Request, Response, Fault
            //Get stub, pad the length to a multiple of 4 bytes.
            byte[] stub = GetStub();
            if (stub == null)
            {
                stub = new byte[0];
                SetStub(stub);
            }

            //Get SecurityBuffers.
            int stubSecBufIndex;
            int tokenSecBufIndex;
            SecurityBufferType readonlyFlag;
            List<SecurityBuffer> securityBufferList = new List<SecurityBuffer>();
            if (context.SupportsHeaderSign)
            {
                readonlyFlag = SecurityBufferType.ReadOnlyWithChecksum;
            }
            else
            {
                readonlyFlag = SecurityBufferType.ReadOnly;
            }
            int headerSize = GetSize();
            byte[] buf = new byte[
                headerSize
                + stub.Length
                + RpceUtility.AuthVerifierGetSize(authVerifier)];
            using (BinaryWriter binaryWriter = new BinaryWriter(new MemoryStream(buf)))
            {
                ToBytes(binaryWriter);
            }

            securityBufferList.Add(
                new SecurityBuffer(
                    SecurityBufferType.Data | readonlyFlag,
                    ArrayUtility.SubArray(buf, 0, headerSize)));
            stubSecBufIndex = securityBufferList.Count;
            securityBufferList.Add(
                new SecurityBuffer(
                    SecurityBufferType.Data, 
                    ArrayUtility.ConcatenateArrays(stub, authVerifier.auth_pad)));
            securityBufferList.Add(
                new SecurityBuffer(
                    SecurityBufferType.Data | readonlyFlag,
                    ArrayUtility.SubArray(
                        buf, 
                        headerSize + stub.Length + authVerifier.auth_pad_length, 
                        RpceUtility.AUTH_VERIFIER_SIZE))); //8 == length of auth_verifier
            tokenSecBufIndex = securityBufferList.Count;
            securityBufferList.Add(
                new SecurityBuffer(
                    SecurityBufferType.Token,
                    authVerifier.auth_value));
            SecurityBuffer[] securityBuffers = securityBufferList.ToArray();

            //encrypt and sign
            switch (context.AuthenticationLevel)
            {
                case RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_CALL: //Same as RPC_C_AUTHN_LEVEL_PKT
                case RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT:
                    //the field "checksum" is the checksum value returned 
                    //by the underlying security service in response to 
                    //an integrity protection call
                    context.SecurityContext.Sign(securityBuffers);
                    authVerifier.auth_value = securityBuffers[tokenSecBufIndex].Buffer;
                    break;

                case RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_INTEGRITY:
                    //the field "checksum" is the checksum value returned 
                    //by the underlying security service in response to 
                    //an integrity protection call
                    context.SecurityContext.Sign(securityBuffers);
                    authVerifier.auth_value = securityBuffers[tokenSecBufIndex].Buffer;
                    break;

                case RpceAuthenticationLevel.RPC_C_AUTHN_LEVEL_PKT_PRIVACY:
                    //This level of service provides strong integrity protection for the
                    //entire PDU, plus privacy protection for the body data only. 
                    //Therefore, only the bodies of the request, response and fault PDUs 
                    //are encrypted.
                    context.SecurityContext.Encrypt(securityBuffers);
                    authVerifier.auth_pad = ArrayUtility.SubArray(securityBuffers[stubSecBufIndex].Buffer, stub.Length);
                    authVerifier.auth_value = securityBuffers[tokenSecBufIndex].Buffer;
                    stub = ArrayUtility.SubArray(securityBuffers[stubSecBufIndex].Buffer, 0, stub.Length);
                    SetStub(stub);
                    break;

                //default do nothing.
            }

            //A client or a server that (during composing of a PDU) has allocated more space for 
            //the authentication token than the security provider fills in SHOULD<36> fill in 
            //the rest of the allocated space with zero octets. These zero octets are still 
            //considered to belong to the authentication token part of the PDU.
            int padLength = (int)auth_length - authVerifier.auth_value.Length;
            if (padLength < 0)
            {
                throw new InvalidOperationException("Length of calculated auth_value is incorrect.");
            }
            else if (padLength > 0)
            {
                authVerifier.auth_value = ArrayUtility.ConcatenateArrays(authVerifier.auth_value, new byte[padLength]);
            }
        }


        /// <summary>
        /// Get the stub of a PDU. Only Request, Response and Fault PDU have stub.
        /// </summary>
        /// <returns>stub</returns>
        private byte[] GetStub()
        {
            byte[] stub;
            Type pduType = this.GetType();
            if (pduType == typeof(RpceCoRequestPdu))
            {
                stub = ((RpceCoRequestPdu)this).stub;
            }
            else if (pduType == typeof(RpceCoResponsePdu))
            {
                stub = ((RpceCoResponsePdu)this).stub;
            }
            else if (pduType == typeof(RpceCoFaultPdu))
            {
                stub = ((RpceCoFaultPdu)this).stub;
            }
            else
            {
                stub = null;
            }
            return stub;
        }


        /// <summary>
        /// Set the stub of a PDU. Only Request, Response and Fault PDU have stub.
        /// </summary>
        private void SetStub(byte[] stub)
        {
            Type pduType = this.GetType();
            if (pduType == typeof(RpceCoRequestPdu))
            {
                ((RpceCoRequestPdu)this).stub = stub;
            }
            else if (pduType == typeof(RpceCoResponsePdu))
            {
                ((RpceCoResponsePdu)this).stub = stub;
            }
            else if (pduType == typeof(RpceCoFaultPdu))
            {
                ((RpceCoFaultPdu)this).stub = stub;
            }
        }
    }
}

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 NEGOTIATE Response packet is sent by the server to notify the client of the preferred common dialect
    /// </summary>
    public class Smb2NegotiateResponsePacket : Smb2StandardPacket<NEGOTIATE_Response>, IPacketBuffer
    {
        /// <summary>
        /// The offset of buffer starting from the begining of the packet.
        /// </summary>
        public ushort BufferOffset
        {
            // 64 bytes of packet header and 64 bytes of fixed length fields
            get { return 64 + 64; }
        }

        public uint BufferLength
        {
            get { return PayLoad.SecurityBufferLength; }
        }

        /// <summary>
        /// Indicates which preauthentication integrity hash algorithms the client supports and 
        /// to optionally supply a preauthentication integrity hash salt value.
        /// </summary>
        public SMB2_PREAUTH_INTEGRITY_CAPABILITIES? NegotiateContext_PREAUTH;

        /// <summary>
        /// Indicates which encryption algorithms the client supports.
        /// </summary>
        public SMB2_ENCRYPTION_CAPABILITIES? NegotiateContext_ENCRYPTION;

        /// <summary>
        /// Indicates which compression algorithms the server supports.
        /// </summary>
        public SMB2_COMPRESSION_CAPABILITIES? NegotiateContext_COMPRESSION;

        /// <summary>
        /// Covert to a byte array
        /// </summary>
        /// <returns>The byte array</returns>
        public override byte[] ToBytes()
        {
            byte[] messageData = TypeMarshal.ToBytes(this.Header);
            messageData = messageData.Concat(Smb2Utility.MarshalStructure(this.PayLoad)).ToArray();
            if (this.Buffer != null) messageData = messageData.Concat(this.Buffer).ToArray();

            if (NegotiateContext_PREAUTH != null)
            {
                // 8-byte align
                Smb2Utility.Align8(ref messageData);
                messageData = messageData.Concat(TypeMarshal.ToBytes<SMB2_PREAUTH_INTEGRITY_CAPABILITIES>(NegotiateContext_PREAUTH.Value)).ToArray();
            }

            if (NegotiateContext_ENCRYPTION != null)
            {
                // 8-byte align
                Smb2Utility.Align8(ref messageData);
                messageData = messageData.Concat(TypeMarshal.ToBytes<SMB2_ENCRYPTION_CAPABILITIES>(NegotiateContext_ENCRYPTION.Value)).ToArray();
            }

            if (NegotiateContext_COMPRESSION != null)
            {
                // 8-byte align
                Smb2Utility.Align8(ref messageData);
                messageData = messageData.Concat(TypeMarshal.ToBytes<SMB2_COMPRESSION_CAPABILITIES>(NegotiateContext_COMPRESSION.Value)).ToArray();
            }

            return messageData;
        }


        /// <summary>
        /// Build a Smb2Packet from a byte array
        /// </summary>
        /// <param name="data">The byte array</param>
        /// <param name="consumedLen">The consumed data length</param>
        /// <param name="expectedLen">The expected data length</param>
        internal override void FromBytes(byte[] data, out int consumedLen, out int expectedLen)
        {
            consumedLen = 0;
            this.NegotiateContext_ENCRYPTION = null;
            this.NegotiateContext_PREAUTH = null;
            this.Header = TypeMarshal.ToStruct<Packet_Header>(data, ref consumedLen);

            byte[] tempData = data.Skip(consumedLen).ToArray();
            this.PayLoad = Smb2Utility.UnmarshalStructure<NEGOTIATE_Response>(tempData);
            consumedLen += Marshal.SizeOf(this.PayLoad);

            if (this.PayLoad.SecurityBufferLength > 0)
            {
                this.Buffer = data.Skip(this.PayLoad.SecurityBufferOffset).Take(this.PayLoad.SecurityBufferLength).ToArray();
                consumedLen += this.Buffer.Length;
            }

            while (data.Length > consumedLen)
            {
                // Skip padding
                int paddingLen = 8 - (consumedLen) % 8;
                if (paddingLen != 8)
                {
                    if (data.Length - consumedLen <= paddingLen) break;
                    consumedLen += paddingLen;
                }

                if (data.Length - consumedLen < 8) break;
                SMB2_NEGOTIATE_CONTEXT_Type_Values contextType = (SMB2_NEGOTIATE_CONTEXT_Type_Values)BitConverter.ToUInt16(data, consumedLen);
                if (contextType == SMB2_NEGOTIATE_CONTEXT_Type_Values.SMB2_PREAUTH_INTEGRITY_CAPABILITIES)
                {
                    if (this.NegotiateContext_PREAUTH != null) throw new Exception("More than one SMB2_PREAUTH_INTEGRITY_CAPABILITIES are present.");
                    this.NegotiateContext_PREAUTH = TypeMarshal.ToStruct<SMB2_PREAUTH_INTEGRITY_CAPABILITIES>(data, ref consumedLen);
                }
                else if (contextType == SMB2_NEGOTIATE_CONTEXT_Type_Values.SMB2_ENCRYPTION_CAPABILITIES)
                {
                    if (this.NegotiateContext_ENCRYPTION != null) throw new Exception("More than one SMB2_ENCRYPTION_CAPABILITIES are present.");
                    this.NegotiateContext_ENCRYPTION = TypeMarshal.ToStruct<SMB2_ENCRYPTION_CAPABILITIES>(data, ref consumedLen);
                }
                else if (contextType == SMB2_NEGOTIATE_CONTEXT_Type_Values.SMB2_COMPRESSION_CAPABILITIES)
                {
                    if (this.NegotiateContext_COMPRESSION != null) throw new Exception("More than one SMB2_COMPRESSION_CAPABILITIES are present.");
                    this.NegotiateContext_COMPRESSION = TypeMarshal.ToStruct<SMB2_COMPRESSION_CAPABILITIES>(data, ref consumedLen);
                }
                else
                {
                    throw new Exception(string.Format("Unknow Negotiate Context: {0}.", (ushort)contextType));
                }
            }
            expectedLen = 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("R NEGOTIATE");

            if (Header.Status != 0) // Append error code if fail.
            {
                sb.Append(", ErrorCode=" + Smb2Status.GetStatusCode(Header.Status));
            }
            else
            {
                sb.Append(", Dialect=" + PayLoad.DialectRevision);
                if (NegotiateContext_PREAUTH != null)
                {
                    sb.Append(", HashAlgorithms={");
                    foreach (var hashId in NegotiateContext_PREAUTH.Value.HashAlgorithms)
                    {
                        sb.Append(hashId.ToString() + ",");
                    }
                    sb.Length--;
                    sb.Append("}");
                }
                if (NegotiateContext_ENCRYPTION != null)
                {
                    sb.Append(", Ciphers={");
                    foreach (var alg in NegotiateContext_ENCRYPTION.Value.Ciphers)
                    {
                        sb.Append(alg.ToString() + ",");
                    }
                    sb.Length--;
                    sb.Append("}");
                }
                sb.Append(", Capabilities=" + PayLoad.Capabilities);
            }
            return sb.ToString();
        }
    }
}

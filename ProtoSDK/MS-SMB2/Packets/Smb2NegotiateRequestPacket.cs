// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The SMB2 NEGOTIATE Request packet is used by the client 
    /// to notify the server what dialects of the SMB 2 Protocol the client understands
    /// </summary>
    public class Smb2NegotiateRequestPacket : Smb2StandardPacket<NEGOTIATE_Request>, IPacketBuffer
    {
        public Smb2NegotiateRequestPacket()
        {
            // The fixed length fields are 36 bytes in total
            PayLoad.StructureSize = 36;
        }

        /// <summary>
        /// The offset of buffer starting from the begining of the packet.
        /// </summary>
        public ushort BufferOffset
        {
            // 64 bytes of packet header and 36 bytes of fixed length fields
            get { return 64 + 36; }
        }

        public uint BufferLength
        {
            get
            {
                return (uint)(this.Buffer == null ? 0 : this.Buffer.Length);
            }
        }

        /// <summary>
        ///  An array of one or more supported dialect revision numbers.
        ///   The array MUST contain at least one element of value 0.
        /// </summary>
        public DialectRevision[] Dialects;

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
        /// Indicates which compression algorithms the client supports.
        /// </summary>
        public SMB2_COMPRESSION_CAPABILITIES? NegotiateContext_COMPRESSION;

        /// <summary>
        /// Contains the server name specified by client.
        /// </summary>
        public SMB2_NETNAME_NEGOTIATE_CONTEXT_ID NegotiateContext_NETNAME;


        /// <summary>
        /// Covert to a byte array
        /// </summary>
        /// <returns>The byte array</returns>
        public override byte[] ToBytes()
        {
            byte[] messageData = TypeMarshal.ToBytes(this.Header);
            messageData = messageData.Concat(Smb2Utility.MarshalStructure(this.PayLoad)).ToArray();

            if (this.Dialects != null & this.Dialects.Length > 0)
            {
                messageData = messageData.Concat(Smb2Utility.MarshalStructArray(Dialects)).ToArray();
            }

            if (NegotiateContext_PREAUTH != null)
            {
                Smb2Utility.Align8(ref messageData);
                messageData = messageData.Concat(TypeMarshal.ToBytes<SMB2_PREAUTH_INTEGRITY_CAPABILITIES>(NegotiateContext_PREAUTH.Value)).ToArray();
            }

            if (NegotiateContext_ENCRYPTION != null)
            {
                Smb2Utility.Align8(ref messageData);
                messageData = messageData.Concat(TypeMarshal.ToBytes<SMB2_ENCRYPTION_CAPABILITIES>(NegotiateContext_ENCRYPTION.Value)).ToArray();
            }

            if (NegotiateContext_COMPRESSION != null)
            {
                Smb2Utility.Align8(ref messageData);
                messageData = messageData.Concat(TypeMarshal.ToBytes<SMB2_COMPRESSION_CAPABILITIES>(NegotiateContext_COMPRESSION.Value)).ToArray();
            }

            if (NegotiateContext_NETNAME != null)
            {
                Smb2Utility.Align8(ref messageData);
                messageData = messageData.Concat(NegotiateContext_NETNAME.Marshal()).ToArray();
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
            this.Header = TypeMarshal.ToStruct<Packet_Header>(data, ref consumedLen);

            byte[] tempData = data.Skip(consumedLen).ToArray();
            this.PayLoad = Smb2Utility.UnmarshalStructure<NEGOTIATE_Request>(tempData);
            consumedLen += Marshal.SizeOf(this.PayLoad);

            this.Buffer = data.Skip(consumedLen).ToArray(); //Dialects + Padding + NegotiateContextList 

            if (PayLoad.DialectCount > 0)
            {
                this.Dialects = Smb2Utility.UnmarshalStructArray<DialectRevision>(Buffer, PayLoad.DialectCount);
                consumedLen += PayLoad.DialectCount * sizeof(DialectRevision);
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
                    this.NegotiateContext_PREAUTH = TypeMarshal.ToStruct<SMB2_PREAUTH_INTEGRITY_CAPABILITIES>(data, ref consumedLen);
                }
                else if (contextType == SMB2_NEGOTIATE_CONTEXT_Type_Values.SMB2_ENCRYPTION_CAPABILITIES)
                {
                    this.NegotiateContext_ENCRYPTION = TypeMarshal.ToStruct<SMB2_ENCRYPTION_CAPABILITIES>(data, ref consumedLen);
                }
                else if (contextType == SMB2_NEGOTIATE_CONTEXT_Type_Values.SMB2_COMPRESSION_CAPABILITIES)
                {
                    this.NegotiateContext_COMPRESSION = TypeMarshal.ToStruct<SMB2_COMPRESSION_CAPABILITIES>(data, ref consumedLen);
                }
                else if (contextType == SMB2_NEGOTIATE_CONTEXT_Type_Values.SMB2_NETNAME_NEGOTIATE_CONTEXT_ID)
                {
                    this.NegotiateContext_NETNAME = SMB2_NETNAME_NEGOTIATE_CONTEXT_ID.Unmarshal(data, ref consumedLen);
                }
            }

            expectedLen = 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("C NEGOTIATE");

            sb.Append(", Dialects={");
            foreach (var dialect in Dialects)
            {
                sb.Append(dialect.ToString() + ",");
            }
            sb.Length--;
            sb.Append("}");
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
            return sb.ToString();
        }
    }
}

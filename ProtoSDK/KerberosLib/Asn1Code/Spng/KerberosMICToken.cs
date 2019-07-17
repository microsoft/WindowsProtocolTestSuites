// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
     * Constants, data structures and algorithms are defined in [RFC4121](https://tools.ietf.org/html/rfc4121).
     */
    public enum KerberosMICToken_TOK_ID_Values : UInt16
    {
        GSS_GetMIC = 0x0404,
    }

    [Flags]
    public enum KerberosMICToken_Flags_Values : Byte
    {
        None = 0x0,
        SentByAcceptor = 0x01,
        Sealed = 0x02,
        AcceptorSubkey = 0x04,
    }

    public struct KerberosMICTokenHeader
    {
        [ByteOrder(EndianType.BigEndian)]
        public KerberosMICToken_TOK_ID_Values TOK_ID;

        public KerberosMICToken_Flags_Values Flags;

        [StaticSize(5)]
        public byte[] Filler;

        [ByteOrder(EndianType.BigEndian)]
        public long SND_SEQ;
    }

    public struct KerberosMICToken
    {
        public KerberosMICTokenHeader Header;

        public byte[] SGN_CKSUM;

        public static KerberosMICToken GSS_GetMIC(KerberosMICToken_Flags_Values flags, long sequenceNumber, ChecksumType type, byte[] key, byte[] data)
        {
            var applicableChecksumType = new List<ChecksumType>()
            {
                ChecksumType.hmac_sha1_96_aes128,
                ChecksumType.hmac_sha1_96_aes256
            };
            if (!applicableChecksumType.Any(checksumType => checksumType == type))
            {
                throw new InvalidOperationException("The checksum type is not applicable!");
            }

            var obj = new KerberosMICToken();
            obj.Header.TOK_ID = KerberosMICToken_TOK_ID_Values.GSS_GetMIC;
            obj.Header.Flags = flags;
            obj.Header.Filler = new byte[5] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            obj.Header.SND_SEQ = sequenceNumber;

            var headerBytes = TypeMarshal.ToBytes(obj.Header);

            var checksumData = data.Concat(headerBytes).ToArray();

            int usage;

            if (flags.HasFlag(KerberosMICToken_Flags_Values.SentByAcceptor))
            {
                usage = (int)TokenKeyUsage.KG_USAGE_ACCEPTOR_SIGN;
            }
            else
            {
                usage = (int)TokenKeyUsage.KG_USAGE_INITIATOR_SIGN;
            }

            obj.SGN_CKSUM = KerberosUtility.GetChecksum(key, checksumData, usage, type);

            return obj;
        }

        public byte[] Encode()
        {
            var header = TypeMarshal.ToBytes(Header);
            var result = header.Concat(SGN_CKSUM).ToArray();
            return result;
        }

        public static KerberosMICToken Decode(byte[] data)
        {
            var result = new KerberosMICToken();
            int offset = 0;
            result.Header = TypeMarshal.ToStruct<KerberosMICTokenHeader>(data, ref offset);
            result.SGN_CKSUM = data.Skip(offset).ToArray();
            return result;
        }
    }
}

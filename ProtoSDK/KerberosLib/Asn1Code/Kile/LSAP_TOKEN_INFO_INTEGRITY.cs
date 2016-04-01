// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    LSAP_TOKEN_INFO_INTEGRITY ::= SEQUENCE {
    flags UInt32,
    tokenIL UInt32,
    machineID OCTET STRING
    }
    */
    public class LSAP_TOKEN_INFO_INTEGRITY : Asn1Sequence
    {
        [Asn1Field(0)]
        public KerbUInt32 flags { get; set; }

        [Asn1Field(1)]
        public KerbUInt32 tokenIL { get; set; }

        [Asn1Field(2)]
        public Asn1OctetString machineID { get; set; }

        public LSAP_TOKEN_INFO_INTEGRITY()
        {
            this.flags = null;
            this.tokenIL = null;
            this.machineID = null;
        }

        public LSAP_TOKEN_INFO_INTEGRITY(
         KerbUInt32 param0,
         KerbUInt32 param1,
         Asn1OctetString param2)
        {
            this.flags = param0;
            this.tokenIL = param1;
            this.machineID = param2;
        }


        //Added manually

        public void GetElements(Asn1OctetString asn1MsgToDecode)
        {
            if (asn1MsgToDecode == null)
            {
                throw new Exception();
            }

            this.flags = null;
            this.tokenIL = null;
            this.machineID = null;

            byte[] flagsToDecode = new byte[sizeof(uint)];
            byte[] tokenILToDecode = new byte[sizeof(uint)];
            byte[] machineIDToDecode = new byte[32];

            Buffer.BlockCopy(asn1MsgToDecode.ByteArrayValue, 0, flagsToDecode, 0, sizeof(uint));
            Buffer.BlockCopy(asn1MsgToDecode.ByteArrayValue, sizeof(uint), tokenILToDecode, 0, sizeof(uint));
            Buffer.BlockCopy(asn1MsgToDecode.ByteArrayValue, sizeof(uint) * 2, machineIDToDecode, 0, 32);

            flags = new KerbUInt32(BitConverter.ToUInt32(flagsToDecode, 0));
            tokenIL = new KerbUInt32(BitConverter.ToUInt32(tokenILToDecode, 0));
            machineID = new Asn1OctetString(machineIDToDecode);
        }

        public Asn1OctetString SetElements()
        {
            var machineIdToEncode = machineID.ByteArrayValue;
            var tokenIlToEncode = BitConverter.GetBytes((uint)tokenIL.Value);
            var flagsToEncode = BitConverter.GetBytes((uint)flags.Value);

            byte[] msgToEncode = new byte[flagsToEncode.Length + tokenIlToEncode.Length + machineIdToEncode.Length];
            flagsToEncode.CopyTo(msgToEncode, 0);
            tokenIlToEncode.CopyTo(msgToEncode, flagsToEncode.Length);
            machineIdToEncode.CopyTo(msgToEncode, flagsToEncode.Length + tokenIlToEncode.Length);

            Asn1OctetString asn1MsgToEncode = new Asn1OctetString(msgToEncode);
            return asn1MsgToEncode;
        }

        public override int BerEncode(IAsn1BerEncodingBuffer buffer, bool explicitTag)
        {
            return 0;
        }

        public override int BerDecode(IAsn1DecodingBuffer buffer, bool explicitTag = true)
        {
            return 0;
        }
    }
}


// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    EncryptedData   ::= SEQUENCE {
        etype   [0] Int32 -- EncryptionType --,
        kvno    [1] UInt32 OPTIONAL,
        cipher  [2] OCTET STRING -- ciphertext
    }
    */
    public class EncryptedData : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public KerbInt32 etype { get; set; }

        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public KerbInt32 kvno { get; set; }
        
        [Asn1Field(2), Asn1Tag(Asn1TagType.Context, 2)]
        public Asn1OctetString cipher { get; set; }
        
        public EncryptedData()
        {
            this.etype = null;
            this.kvno = null;
            this.cipher = null;
        }
        
        public EncryptedData(
         KerbInt32 param0,
         KerbInt32 param1,
         Asn1OctetString param2)
        {
            this.etype = param0;
            this.kvno = param1;
            this.cipher = param2;
        }
    }
}


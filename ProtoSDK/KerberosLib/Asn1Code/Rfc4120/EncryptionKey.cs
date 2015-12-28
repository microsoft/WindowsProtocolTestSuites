// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    EncryptionKey   ::= SEQUENCE {
        keytype         [0] Int32 -- actually encryption type --,
        keyvalue        [1] OCTET STRING
    }
    */
    public class EncryptionKey : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public KerbInt32 keytype { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1OctetString keyvalue { get; set; }
        
        public EncryptionKey()
        {
            this.keytype = null;
            this.keyvalue = null;
        }
        
        public EncryptionKey(
         KerbInt32 param0,
         Asn1OctetString param1)
        {
            this.keytype = param0;
            this.keyvalue = param1;
        }
    }
}


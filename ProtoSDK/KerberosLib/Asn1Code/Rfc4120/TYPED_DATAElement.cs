// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    TYPED-DATA      ::= SEQUENCE SIZE (1..MAX) OF SEQUENCE {
        data-type       [0] Int32,
        data-value      [1] OCTET STRING OPTIONAL
    }
    */
    public class TYPED_DATAElement : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public KerbInt32 data_type { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1OctetString data_value { get; set; }
        
        public TYPED_DATAElement()
        {
            this.data_type = null;
            this.data_value = null;
        }
        
        public TYPED_DATAElement(
         KerbInt32 param0,
         Asn1OctetString param1)
        {
            this.data_type = param0;
            this.data_value = param1;
        }
    }
}


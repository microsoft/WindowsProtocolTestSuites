// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    ExtendedRequest ::= [APPLICATION 23] SEQUENCE {
                requestName      [0] LDAPOID,
                requestValue     [1] OCTET STRING OPTIONAL }
    */
    [Asn1Tag(Asn1TagType.Application, 23)]
    public class ExtendedRequest : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public LDAPOID requestName { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1OctetString requestValue { get; set; }
        
        public ExtendedRequest()
        {
            this.requestName = null;
            this.requestValue = null;
        }
        
        public ExtendedRequest(
         LDAPOID requestName,
         Asn1OctetString requestValue)
        {
            this.requestName = requestName;
            this.requestValue = requestValue;
        }
    }
}


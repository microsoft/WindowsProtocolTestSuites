// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    CompareRequest ::=
    [APPLICATION 14] SEQUENCE {
         entry          LDAPDN,
         ava            AttributeValueAssertion
    }
    */
    [Asn1Tag(Asn1TagType.Application, 14)]
    public class CompareRequest : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPDN entry { get; set; }
        
        [Asn1Field(1)]
        public AttributeValueAssertion ava { get; set; }
        
        public CompareRequest()
        {
            this.entry = null;
            this.ava = null;
        }
        
        public CompareRequest(
         LDAPDN entry,
         AttributeValueAssertion ava)
        {
            this.entry = entry;
            this.ava = ava;
        }
    }
}


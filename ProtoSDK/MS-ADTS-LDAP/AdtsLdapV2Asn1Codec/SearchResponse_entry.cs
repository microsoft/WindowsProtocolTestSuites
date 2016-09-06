// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    SearchResponse_entry ::=         [APPLICATION 4] SEQUENCE {
                             objectName     LDAPDN,
                             attributes     SEQUENCE OF SearchResponse_entry_attributes_element
                        }
    */
    [Asn1Tag(Asn1TagType.Application, 4)]
    public class SearchResponse_entry : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPDN objectName { get; set; }
        
        [Asn1Field(1)]
        public Asn1SequenceOf<SearchResponse_entry_attributes_element> attributes { get; set; }
        
        public SearchResponse_entry()
        {
            this.objectName = null;
            this.attributes = null;
        }
        
        public SearchResponse_entry(
         LDAPDN objectName,
         Asn1SequenceOf<SearchResponse_entry_attributes_element> attributes)
        {
            this.objectName = objectName;
            this.attributes = attributes;
        }
    }
}


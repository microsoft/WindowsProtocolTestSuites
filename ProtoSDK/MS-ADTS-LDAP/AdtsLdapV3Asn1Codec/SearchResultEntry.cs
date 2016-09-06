// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    SearchResultEntry ::= [APPLICATION 4] SEQUENCE {
                objectName      LDAPDN,
                attributes      PartialAttributeList }
    */
    [Asn1Tag(Asn1TagType.Application, 4)]
    public class SearchResultEntry : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPDN objectName { get; set; }
        
        [Asn1Field(1)]
        public PartialAttributeList attributes { get; set; }
        
        public SearchResultEntry()
        {
            this.objectName = null;
            this.attributes = null;
        }
        
        public SearchResultEntry(
         LDAPDN objectName,
         PartialAttributeList attributes)
        {
            this.objectName = objectName;
            this.attributes = attributes;
        }
    }
}


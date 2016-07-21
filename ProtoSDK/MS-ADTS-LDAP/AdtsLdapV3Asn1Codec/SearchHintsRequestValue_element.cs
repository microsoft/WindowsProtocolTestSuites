// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
     SearchHintsRequestValue_element ::= SEQUENCE{
	    hintId    LDAPOID
	    hintValue OCTET STRING
	}

    */
    public class SearchHintsRequestValue_element : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPOID hintId { get; set; }
        
        [Asn1Field(1)]
        public Asn1OctetString hintValue { get; set; }
        
        public SearchHintsRequestValue_element()
        {
            this.hintId = null;
            this.hintValue = null;
        }
        
        public SearchHintsRequestValue_element(
         LDAPOID hintId,
         Asn1OctetString hintValue)
        {
            this.hintId = hintId;
            this.hintValue = hintValue;
        }
    }
}


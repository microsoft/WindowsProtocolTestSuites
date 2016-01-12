// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	SearchHintsRequestValue ::= SEQUENCE OF SearchHintsRequestValue_element

    */
    public class SearchHintsRequestValue : Asn1SequenceOf<SearchHintsRequestValue_element>
    {
        public SearchHintsRequestValue()
            : base()
        {
            this.Elements = null;
        }
        
        public SearchHintsRequestValue(SearchHintsRequestValue_element[] val)
            : base(val)
        {
        }
    }
}


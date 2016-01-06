// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	DNInputRequestValue ::= SEQUENCE {
	    InputDN    OCTET STRING
	}

    */
    public class DNInputRequestValue : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1OctetString InputDN { get; set; }
        
        public DNInputRequestValue()
        {
            this.InputDN = null;
        }
        
        public DNInputRequestValue(
         Asn1OctetString InputDN)
        {
            this.InputDN = InputDN;
        }
    }
}


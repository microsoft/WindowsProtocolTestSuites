// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	QuotaRequestValue ::= SEQUENCE {
	    querySID    OCTET STRING
	}

    */
    public class QuotaRequestValue : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1OctetString querySID { get; set; }
        
        public QuotaRequestValue()
        {
            this.querySID = null;
        }
        
        public QuotaRequestValue(
         Asn1OctetString querySID)
        {
            this.querySID = querySID;
        }
    }
}


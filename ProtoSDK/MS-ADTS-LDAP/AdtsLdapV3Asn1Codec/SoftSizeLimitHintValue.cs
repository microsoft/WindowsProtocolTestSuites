// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	SoftSizeLimitHintValue ::= SEQUENCE {
	    limitValue    INTEGER
	}

    */
    public class SoftSizeLimitHintValue : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1Integer limitValue { get; set; }
        
        public SoftSizeLimitHintValue()
        {
            this.limitValue = null;
        }
        
        public SoftSizeLimitHintValue(
         Asn1Integer limitValue)
        {
            this.limitValue = limitValue;
        }
    }
}


// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	PolicyHintsRequestValue ::= SEQUENCE {
	    Flags    INTEGER
	}

    */
    public class PolicyHintsRequestValue : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1Integer Flags { get; set; }
        
        public PolicyHintsRequestValue()
        {
            this.Flags = null;
        }
        
        public PolicyHintsRequestValue(
         Asn1Integer Flags)
        {
            this.Flags = Flags;
        }
    }
}


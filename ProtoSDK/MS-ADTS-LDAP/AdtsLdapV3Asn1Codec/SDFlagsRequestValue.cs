// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	SDFlagsRequestValue ::= SEQUENCE {
	    Flags    INTEGER
	}

    */
    public class SDFlagsRequestValue : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1Integer Flags { get; set; }
        
        public SDFlagsRequestValue()
        {
            this.Flags = null;
        }
        
        public SDFlagsRequestValue(
         Asn1Integer Flags)
        {
            this.Flags = Flags;
        }
    }
}


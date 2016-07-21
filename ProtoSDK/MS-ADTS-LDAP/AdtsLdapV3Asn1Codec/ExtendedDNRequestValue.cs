// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	ExtendedDNRequestValue ::= SEQUENCE {
	    Flag    INTEGER
	}

    */
    public class ExtendedDNRequestValue : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1Integer Flag { get; set; }
        
        public ExtendedDNRequestValue()
        {
            this.Flag = null;
        }
        
        public ExtendedDNRequestValue(
         Asn1Integer Flag)
        {
            this.Flag = Flag;
        }
    }
}


// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	RequireSortIndexHintValue ::= SEQUENCE {
	    IndexOnly    BOOLEAN
	}

    */
    public class RequireSortIndexHintValue : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1Boolean IndexOnly { get; set; }
        
        public RequireSortIndexHintValue()
        {
            this.IndexOnly = null;
        }
        
        public RequireSortIndexHintValue(
         Asn1Boolean IndexOnly)
        {
            this.IndexOnly = IndexOnly;
        }
    }
}


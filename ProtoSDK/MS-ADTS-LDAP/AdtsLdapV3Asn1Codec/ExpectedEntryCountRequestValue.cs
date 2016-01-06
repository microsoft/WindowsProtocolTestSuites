// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	ExpectedEntryCountRequestValue ::= SEQUENCE {
	    searchEntriesMin    INTEGER
	    searchEntriesMax    INTEGER
	}

    */
    public class ExpectedEntryCountRequestValue : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1Integer searchEntriesMin { get; set; }
        
        [Asn1Field(1)]
        public Asn1Integer searchEntriesMax { get; set; }
        
        public ExpectedEntryCountRequestValue()
        {
            this.searchEntriesMin = null;
            this.searchEntriesMax = null;
        }
        
        public ExpectedEntryCountRequestValue(
         Asn1Integer searchEntriesMin,
         Asn1Integer searchEntriesMax)
        {
            this.searchEntriesMin = searchEntriesMin;
            this.searchEntriesMax = searchEntriesMax;
        }
    }
}


// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
     UpdateStatsResponseValue_element ::= SEQUENCE {
	    statID       LDAPOID
	    statValue    OCTET STRING
	}

    */
    public class UpdateStatsResponseValue_element : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPOID statID { get; set; }
        
        [Asn1Field(1)]
        public Asn1OctetString statValue { get; set; }
        
        public UpdateStatsResponseValue_element()
        {
            this.statID = null;
            this.statValue = null;
        }
        
        public UpdateStatsResponseValue_element(
         LDAPOID statID,
         Asn1OctetString statValue)
        {
            this.statID = statID;
            this.statValue = statValue;
        }
    }
}


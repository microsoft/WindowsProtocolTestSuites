// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	StatsResponseValueV4_element ::= SEQUENCE {
	    statisticName         OCTET STRING
	    statsResponseValueV4_element_choice StatsResponseValueV4_element_choice
	}

    */
    public class StatsResponseValueV4_element : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1OctetString statisticName { get; set; }
        
        [Asn1Field(1)]
        public StatsResponseValueV4_element_choice statsResponseValueV4_element_choice { get; set; }
        
        public StatsResponseValueV4_element()
        {
            this.statisticName = null;
            this.statsResponseValueV4_element_choice = null;
        }
        
        public StatsResponseValueV4_element(
         Asn1OctetString statisticName,
         StatsResponseValueV4_element_choice statsResponseValueV4_element_choice)
        {
            this.statisticName = statisticName;
            this.statsResponseValueV4_element_choice = statsResponseValueV4_element_choice;
        }
    }
}


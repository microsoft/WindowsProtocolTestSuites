// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	StatsResponseValueV4 ::= SEQUENCE OF StatsResponseValueV4_element
    */
    public class StatsResponseValueV4 : Asn1SequenceOf<StatsResponseValueV4_element>
    {
        public StatsResponseValueV4()
            : base()
        {
            this.Elements = null;
        }
        
        public StatsResponseValueV4(StatsResponseValueV4_element[] val)
            : base(val)
        {
        }
    }
}


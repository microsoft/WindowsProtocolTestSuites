// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	UpdateStatsResponseValue ::= SEQUENCE OF UpdateStatsResponseValue_element

    */
    public class UpdateStatsResponseValue : Asn1SequenceOf<UpdateStatsResponseValue_element>
    {
        public UpdateStatsResponseValue()
            : base()
        {
            this.Elements = null;
        }
        
        public UpdateStatsResponseValue(UpdateStatsResponseValue_element[] val)
            : base(val)
        {
        }
    }
}


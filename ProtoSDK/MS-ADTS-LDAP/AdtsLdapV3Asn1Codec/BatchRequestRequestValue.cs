// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	BatchRequestRequestValue ::= SEQUENCE of OCTET STRING
    */
    public class BatchRequestRequestValue : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1OctetString of { get; set; }
        
        public BatchRequestRequestValue()
        {
            this.of = null;
        }
        
        public BatchRequestRequestValue(
         Asn1OctetString of)
        {
            this.of = of;
        }
    }
}


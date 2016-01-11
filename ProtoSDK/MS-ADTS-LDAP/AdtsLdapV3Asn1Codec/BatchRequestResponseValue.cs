// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	BatchRequestResponseValue ::= SEQUENCE of LDAPMessage
    */
    public class BatchRequestResponseValue : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPMessage of { get; set; }
        
        public BatchRequestResponseValue()
        {
            this.of = null;
        }
        
        public BatchRequestResponseValue(
         LDAPMessage of)
        {
            this.of = of;
        }
    }
}


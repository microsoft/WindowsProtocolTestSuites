// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	VerifyNameRequestValue ::= SEQUENCE {
	    Flags         INTEGER
	    ServerName    OCTET STRING
	}

    */
    public class VerifyNameRequestValue : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1Integer Flags { get; set; }
        
        [Asn1Field(1)]
        public Asn1OctetString ServerName { get; set; }
        
        public VerifyNameRequestValue()
        {
            this.Flags = null;
            this.ServerName = null;
        }
        
        public VerifyNameRequestValue(
         Asn1Integer Flags,
         Asn1OctetString ServerName)
        {
            this.Flags = Flags;
            this.ServerName = ServerName;
        }
    }
}


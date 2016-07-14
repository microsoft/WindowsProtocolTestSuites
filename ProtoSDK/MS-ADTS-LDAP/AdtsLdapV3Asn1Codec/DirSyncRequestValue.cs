// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	DirSyncRequestValue ::= SEQUENCE {
	    Flags       INTEGER
	    MaxBytes    INTEGER
	    Cookie      OCTET STRING
	}

    */
    public class DirSyncRequestValue : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1Integer Flags { get; set; }
        
        [Asn1Field(1)]
        public Asn1Integer MaxBytes { get; set; }
        
        [Asn1Field(2)]
        public Asn1OctetString Cookie { get; set; }
        
        public DirSyncRequestValue()
        {
            this.Flags = null;
            this.MaxBytes = null;
            this.Cookie = null;
        }
        
        public DirSyncRequestValue(
         Asn1Integer Flags,
         Asn1Integer MaxBytes,
         Asn1OctetString Cookie)
        {
            this.Flags = Flags;
            this.MaxBytes = MaxBytes;
            this.Cookie = Cookie;
        }
    }
}


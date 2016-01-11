// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	DirSyncResponseValue ::= SEQUENCE {
	    MoreResults     INTEGER
	    unused          INTEGER
	    CookieServer    OCTET STRING
	}

    */
    public class DirSyncResponseValue : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1Integer MoreResults { get; set; }
        
        [Asn1Field(1)]
        public Asn1Integer unused { get; set; }
        
        [Asn1Field(2)]
        public Asn1OctetString CookieServer { get; set; }
        
        public DirSyncResponseValue()
        {
            this.MoreResults = null;
            this.unused = null;
            this.CookieServer = null;
        }
        
        public DirSyncResponseValue(
         Asn1Integer MoreResults,
         Asn1Integer unused,
         Asn1OctetString CookieServer)
        {
            this.MoreResults = MoreResults;
            this.unused = unused;
            this.CookieServer = CookieServer;
        }
    }
}


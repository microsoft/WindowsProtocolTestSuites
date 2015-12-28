// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    PA-ENC-TS-ENC           ::= SEQUENCE {
        patimestamp     [0] KerberosTime -- client's time --,
        pausec          [1] Microseconds OPTIONAL
    }
    */
    public class PA_ENC_TS_ENC : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public KerberosTime patimestamp { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public Microseconds pausec { get; set; }
        
        public PA_ENC_TS_ENC()
        {
            this.patimestamp = null;
            this.pausec = null;
        }
        
        public PA_ENC_TS_ENC(
         KerberosTime param0,
         Microseconds param1)
        {
            this.patimestamp = param0;
            this.pausec = param1;
        }
    }
}


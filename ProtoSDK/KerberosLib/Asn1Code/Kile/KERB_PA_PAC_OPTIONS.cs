// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    KERB_PA_PAC_OPTIONS ::= SEQUENCE {
	pac_flags [0] PA_PAC_OPTIONS
    }
    */
    public class KERB_PA_PAC_OPTIONS : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public PA_PAC_OPTIONS pac_flags { get; set; }
        
        public KERB_PA_PAC_OPTIONS()
        {
            this.pac_flags = null;
        }
        
        public KERB_PA_PAC_OPTIONS(
         PA_PAC_OPTIONS param0)
        {
            this.pac_flags = param0;
        }
    }
}


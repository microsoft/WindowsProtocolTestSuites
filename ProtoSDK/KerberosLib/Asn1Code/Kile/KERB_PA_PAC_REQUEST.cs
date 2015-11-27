// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    KERB-PA-PAC-REQUEST ::= SEQUENCE {
    include-pac[0] BOOLEAN --If TRUE, and no pac present, include PAC.
     --If FALSE, and PAC present, remove PAC
    } 
    */
    public class KERB_PA_PAC_REQUEST : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public Asn1Boolean include_pac { get; set; }
        
        public KERB_PA_PAC_REQUEST()
        {
            this.include_pac = null;
        }
        
        public KERB_PA_PAC_REQUEST(
         Asn1Boolean param0)
        {
            this.include_pac = param0;
        }
    }
}


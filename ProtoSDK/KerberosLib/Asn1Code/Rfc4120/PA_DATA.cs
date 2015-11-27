// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    PA-DATA         ::= SEQUENCE {
        -- NOTE: first tag is [1], not [0]
        padata-type     [1] Int32,
        padata-value    [2] OCTET STRING -- might be encoded AP-REQ
    }
    */
    public class PA_DATA : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 1)]
        public KerbInt32 padata_type { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 2)]
        public Asn1OctetString padata_value { get; set; }
        
        public PA_DATA()
        {
            this.padata_type = null;
            this.padata_value = null;
        }
        
        public PA_DATA(
         KerbInt32 param0,
         Asn1OctetString param1)
        {
            this.padata_type = param0;

            if(param1 == null)
            {
                param1 = new Asn1OctetString(new byte[0]);
            }
            this.padata_value = param1;
        }
    }
}


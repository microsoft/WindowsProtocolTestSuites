// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    KERB-ERROR-DATA ::= SEQUENCE {
     data-type [1] INTEGER,
     data-value [2] OCTET STRING OPTIONAL
    }
    */
    public class KERB_ERROR_DATA : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1Integer data_type { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 2)]
        public Asn1OctetString data_value { get; set; }
        
        public KERB_ERROR_DATA()
        {
            this.data_type = null;
            this.data_value = null;
        }
        
        public KERB_ERROR_DATA(
         Asn1Integer param0,
         Asn1OctetString param1)
        {
            this.data_type = param0;
            this.data_value = param1;
        }
    }
}


// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    -- NOTE: AuthorizationData is always used as an OPTIONAL field and
    -- should not be empty.
    AuthorizationData       ::= SEQUENCE OF SEQUENCE {
            ad-type         [0] Int32,
            ad-data         [1] OCTET STRING
    }
    */
    public class AuthorizationDataElement : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public KerbInt32 ad_type { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1OctetString ad_data { get; set; }
        
        public AuthorizationDataElement()
        {
            this.ad_type = null;
            this.ad_data = null;
        }
        
        public AuthorizationDataElement(
         KerbInt32 param0,
         Asn1OctetString param1)
        {
            this.ad_type = param0;
            this.ad_data = param1;
        }
    }
}


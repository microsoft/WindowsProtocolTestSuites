// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    AD-KDCIssued            ::= SEQUENCE {
        ad-checksum     [0] Checksum,
        i-realm         [1] Realm OPTIONAL,
        i-sname         [2] PrincipalName OPTIONAL,
        elements        [3] AuthorizationData
    }
    */
    public class AD_KDCIssued : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public Checksum ad_checksum { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public Realm i_realm { get; set; }
        
        [Asn1Field(2, Optional = true), Asn1Tag(Asn1TagType.Context, 2)]
        public PrincipalName i_sname { get; set; }
        
        [Asn1Field(3), Asn1Tag(Asn1TagType.Context, 3)]
        public AuthorizationData elements { get; set; }
        
        public AD_KDCIssued()
        {
            this.ad_checksum = null;
            this.i_realm = null;
            this.i_sname = null;
            this.elements = null;
        }
        
        public AD_KDCIssued(
         Checksum param0,
         Realm param1,
         PrincipalName param2,
         AuthorizationData param3)
        {
            this.ad_checksum = param0;
            this.i_realm = param1;
            this.i_sname = param2;
            this.elements = param3;
        }
    }
}


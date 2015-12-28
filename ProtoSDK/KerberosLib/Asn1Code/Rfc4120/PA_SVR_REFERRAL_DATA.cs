// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    PA-SVR-REFERRAL-DATA ::= SEQUENCE {
                  referred-name   [1] PrincipalName OPTIONAL,
                  referred-realm  [0] Realm
           }
    */
    public class PA_SVR_REFERRAL_DATA : Asn1Sequence
    {
        [Asn1Field(0, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public PrincipalName referred_name { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 0)]
        public Realm referred_realm { get; set; }
        
        public PA_SVR_REFERRAL_DATA()
        {
            this.referred_name = null;
            this.referred_realm = null;
        }
        
        public PA_SVR_REFERRAL_DATA(
         PrincipalName param0,
         Realm param1)
        {
            this.referred_name = param0;
            this.referred_realm = param1;
        }
    }
}


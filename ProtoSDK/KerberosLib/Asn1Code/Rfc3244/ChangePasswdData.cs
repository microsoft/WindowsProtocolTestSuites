// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    ChangePasswdData ::=  SEQUENCE {
                          newpasswd[0]   OCTET STRING,
                          targname[1]    PrincipalName OPTIONAL,
                          targrealm[2]   Realm OPTIONAL
                          }
    */
    public class ChangePasswdData : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public Asn1OctetString newpasswd { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public PrincipalName targname { get; set; }
        
        [Asn1Field(2, Optional = true), Asn1Tag(Asn1TagType.Context, 2)]
        public Realm targrealm { get; set; }
        
        public ChangePasswdData()
        {
            this.newpasswd = null;
            this.targname = null;
            this.targrealm = null;
        }
        
        public ChangePasswdData(
         Asn1OctetString param0,
         PrincipalName param1,
         Realm param2)
        {
            this.newpasswd = param0;
            this.targname = param1;
            this.targrealm = param2;
        }
    }
}


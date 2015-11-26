// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    PrincipalName   ::= SEQUENCE {
        name-type       [0] Int32,
        name-string     [1] SEQUENCE OF KerberosString
    }

    */
    public class PrincipalName : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public KerbInt32 name_type { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1SequenceOf<KerberosString> name_string { get; set; }
        
        public PrincipalName()
        {
            this.name_type = null;
            this.name_string = null;
        }
        
        public PrincipalName(
         KerbInt32 param0,
         Asn1SequenceOf<KerberosString> param1)
        {
            this.name_type = param0;
            this.name_string = param1;
        }
    }
}


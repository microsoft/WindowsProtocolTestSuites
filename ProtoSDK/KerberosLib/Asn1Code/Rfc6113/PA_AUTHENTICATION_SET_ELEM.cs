// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth 
{
    /*
    PA-AUTHENTICATION-SET-ELEM ::= SEQUENCE {
          pa-type      [0] Int32,
              -- same as padata-type.
          pa-hint      [1] OCTET STRING OPTIONAL,
          pa-value     [2] OCTET STRING OPTIONAL,
          ...
      }
    */
    public class PA_AUTHENTICATION_SET_ELEM : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public KerbInt32 pa_type { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1OctetString pa_hint { get; set; }
        
        [Asn1Field(2, Optional = true), Asn1Tag(Asn1TagType.Context, 2)]
        public Asn1OctetString pa_value { get; set; }
        
        public PA_AUTHENTICATION_SET_ELEM()
        {
            this.pa_type = null;
            this.pa_hint = null;
            this.pa_value = null;
        }
        
        public PA_AUTHENTICATION_SET_ELEM(
         KerbInt32 param0,
         Asn1OctetString param1,
         Asn1OctetString param2)
        {
            this.pa_type = param0;
            this.pa_hint = param1;
            this.pa_value = param2;
        }
    }
}


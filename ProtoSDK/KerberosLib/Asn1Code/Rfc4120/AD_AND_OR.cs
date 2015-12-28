// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    AD-AND-OR               ::= SEQUENCE {
        condition-count [0] Int32,
        elements        [1] AuthorizationData
    }
    */
    public class AD_AND_OR : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public KerbInt32 condition_count { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public AuthorizationData elements { get; set; }
        
        public AD_AND_OR()
        {
            this.condition_count = null;
            this.elements = null;
        }
        
        public AD_AND_OR(
         KerbInt32 param0,
         AuthorizationData param1)
        {
            this.condition_count = param0;
            this.elements = param1;
        }
    }
}


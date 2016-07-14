// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
     SaslCredentials ::= SEQUENCE {
                mechanism               LDAPString,
                credentials             OCTET STRING OPTIONAL }
    */
    public class SaslCredentials : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPString mechanism { get; set; }
        
        [Asn1Field(1, Optional = true)]
        public Asn1OctetString credentials { get; set; }
        
        public SaslCredentials()
        {
            this.mechanism = null;
            this.credentials = null;
        }
        
        public SaslCredentials(
         LDAPString mechanism,
         Asn1OctetString credentials)
        {
            this.mechanism = mechanism;
            this.credentials = credentials;
        }
    }
}


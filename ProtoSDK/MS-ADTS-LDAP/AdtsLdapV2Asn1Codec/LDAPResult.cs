// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    LDAPResult ::=
    SEQUENCE {
        resultCode  LDAPResult_resultCode,
        matchedDN     LDAPDN,
        errorMessage  LDAPString
    }
    */
    public class LDAPResult : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPResult_resultCode resultCode { get; set; }
        
        [Asn1Field(1)]
        public LDAPDN matchedDN { get; set; }
        
        [Asn1Field(2)]
        public LDAPString errorMessage { get; set; }
        
        public LDAPResult()
        {
            this.resultCode = null;
            this.matchedDN = null;
            this.errorMessage = null;
        }
        
        public LDAPResult(
         LDAPResult_resultCode resultCode,
         LDAPDN matchedDN,
         LDAPString errorMessage)
        {
            this.resultCode = resultCode;
            this.matchedDN = matchedDN;
            this.errorMessage = errorMessage;
        }
    }
}


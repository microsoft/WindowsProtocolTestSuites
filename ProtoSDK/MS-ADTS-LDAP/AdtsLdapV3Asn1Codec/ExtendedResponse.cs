// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    ExtendedResponse ::= [APPLICATION 24] SEQUENCE {
                COMPONENTS OF LDAPResult,
                responseName     [10] LDAPOID OPTIONAL,
                response         [11] OCTET STRING OPTIONAL }
    */
    [Asn1Tag(Asn1TagType.Application, 24)]
    public class ExtendedResponse : Asn1Sequence
    {
        [Asn1Field(0)]
        public LDAPResult_resultCode resultCode { get; set; }
        
        [Asn1Field(1)]
        public LDAPDN matchedDN { get; set; }
        
        [Asn1Field(2)]
        public LDAPString errorMessage { get; set; }
        
        [Asn1Field(3, Optional = true), Asn1Tag(Asn1TagType.Context, 3)]
        public Referral referral { get; set; }
        
        [Asn1Field(4, Optional = true), Asn1Tag(Asn1TagType.Context, 10)]
        public LDAPOID responseName { get; set; }
        
        [Asn1Field(5, Optional = true), Asn1Tag(Asn1TagType.Context, 11)]
        public Asn1OctetString response { get; set; }
        
        public ExtendedResponse()
        {
            this.resultCode = null;
            this.matchedDN = null;
            this.errorMessage = null;
            this.referral = null;
            this.responseName = null;
            this.response = null;
        }

        public ExtendedResponse(
         LDAPResult_resultCode resultCode,
         LDAPDN matchedDN,
         LDAPString errorMessage,
         Referral referral)
        {
            this.resultCode = resultCode;
            this.matchedDN = matchedDN;
            this.errorMessage = errorMessage;
            this.referral = referral;
            this.responseName = null;
            this.response = null;
        }

        public ExtendedResponse(
         LDAPResult_resultCode resultCode,
         LDAPDN matchedDN,
         LDAPString errorMessage,
         Referral referral,
         LDAPOID responseName,
         Asn1OctetString response)
        {
            this.resultCode = resultCode;
            this.matchedDN = matchedDN;
            this.errorMessage = errorMessage;
            this.referral = referral;
            this.responseName = responseName;
            this.response = response;
        }
    }
}


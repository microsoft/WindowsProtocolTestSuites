// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    SicilyBindResponse ::= [APPLICATION 1] SEQUENCE {
  
     resultCode   SicilyBindResponse_resultCode,
     serverCreds  OCTET STRING,
     errorMessage LDAPString }

    */
    [Asn1Tag(Asn1TagType.Application, 1)]
    public class SicilyBindResponse : Asn1Sequence
    {
        [Asn1Field(0)]
        public SicilyBindResponse_resultCode resultCode { get; set; }
        
        [Asn1Field(1)]
        public Asn1OctetString serverCreds { get; set; }
        
        [Asn1Field(2)]
        public LDAPString errorMessage { get; set; }
        
        public SicilyBindResponse()
        {
            this.resultCode = null;
            this.serverCreds = null;
            this.errorMessage = null;
        }
        
        public SicilyBindResponse(
         SicilyBindResponse_resultCode resultCode,
         Asn1OctetString serverCreds,
         LDAPString errorMessage)
        {
            this.resultCode = resultCode;
            this.serverCreds = serverCreds;
            this.errorMessage = errorMessage;
        }
    }
}


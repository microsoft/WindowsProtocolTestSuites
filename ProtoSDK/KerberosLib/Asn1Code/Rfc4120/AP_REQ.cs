// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    AP-REQ          ::= [APPLICATION 14] SEQUENCE {
        pvno            [0] INTEGER (5),
        msg-type        [1] INTEGER (14),
        ap-options      [2] APOptions,
        ticket          [3] Ticket,
        authenticator   [4] EncryptedData -- Authenticator
    }
    */
    [Asn1Tag(Asn1TagType.Application, 14)]
    public class AP_REQ : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public Asn1Integer pvno { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1Integer msg_type { get; set; }
        
        [Asn1Field(2), Asn1Tag(Asn1TagType.Context, 2)]
        public APOptions ap_options { get; set; }
        
        [Asn1Field(3), Asn1Tag(Asn1TagType.Context, 3)]
        public Ticket ticket { get; set; }
        
        [Asn1Field(4), Asn1Tag(Asn1TagType.Context, 4)]
        public EncryptedData authenticator { get; set; }
        
        public AP_REQ()
        {
            this.pvno = null;
            this.msg_type = null;
            this.ap_options = null;
            this.ticket = null;
            this.authenticator = null;
        }
        
        public AP_REQ(
         Asn1Integer param0,
         Asn1Integer param1,
         APOptions param2,
         Ticket param3,
         EncryptedData param4)
        {
            this.pvno = param0;
            this.msg_type = param1;
            this.ap_options = param2;
            this.ticket = param3;
            this.authenticator = param4;
        }

        protected override bool VerifyConstraints()
        {
            return pvno != null && pvno.Value == 5 && msg_type != null && msg_type.Value == 14;
        }
    }
}


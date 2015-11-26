// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    AP-REP          ::= [APPLICATION 15] SEQUENCE {
        pvno            [0] INTEGER (5),
        msg-type        [1] INTEGER (15),
        enc-part        [2] EncryptedData -- EncAPRepPart
    }
    */
    [Asn1Tag(Asn1TagType.Application, 15)]
    public class AP_REP : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public Asn1Integer pvno { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1Integer msg_type { get; set; }
        
        [Asn1Field(2), Asn1Tag(Asn1TagType.Context, 2)]
        public EncryptedData enc_part { get; set; }
        
        public AP_REP()
        {
            this.pvno = null;
            this.msg_type = null;
            this.enc_part = null;
        }
        
        public AP_REP(
         Asn1Integer param0,
         Asn1Integer param1,
         EncryptedData param2)
        {
            this.pvno = param0;
            this.msg_type = param1;
            this.enc_part = param2;
        }

        protected override bool VerifyConstraints()
        {
            return pvno != null && pvno.Value == 5 
                && msg_type != null && msg_type.Value == 15;
        }
    }
}


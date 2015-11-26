// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    TGS-REP         ::= [APPLICATION 13] KDC-REP
    */
    [Asn1Tag(Asn1TagType.Application, 13)]
    public class TGS_REP : KDC_REP
    {
        
        public TGS_REP()
            : base()
        {
        }
        
        public TGS_REP(
         Asn1Integer param0,
         Asn1Integer param1,
         Asn1SequenceOf<PA_DATA> param2,
         Realm param3,
         PrincipalName param4,
         Ticket param5,
         EncryptedData param6)
        {
            this.pvno = param0;
            this.msg_type = param1;
            this.padata = param2;
            this.crealm = param3;
            this.cname = param4;
            this.ticket = param5;
            this.enc_part = param6;
        }
    }
}


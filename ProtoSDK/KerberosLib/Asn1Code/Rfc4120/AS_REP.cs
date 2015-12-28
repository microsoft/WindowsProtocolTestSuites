// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    AS-REP          ::= [APPLICATION 11] KDC-REP
    */
    [Asn1Tag(Asn1TagType.Application, 11)]
    public class AS_REP : KDC_REP
    {
        
        public AS_REP()
            : base()
        {
        }
        
        public AS_REP(
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


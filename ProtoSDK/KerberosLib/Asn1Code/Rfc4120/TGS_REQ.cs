// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    TGS-REQ         ::= [APPLICATION 12] KDC-REQ
    */
    [Asn1Tag(Asn1TagType.Application, 12)]
    public class TGS_REQ : KDC_REQ
    {
        
        public TGS_REQ()
            : base()
        {
        }
        
        public TGS_REQ(
         Asn1Integer param0,
         Asn1Integer param1,
         Asn1SequenceOf<PA_DATA> param2,
         KDC_REQ_BODY param3)
        {
            this.pvno = param0;
            this.msg_type = param1;
            this.padata = param2;
            this.req_body = param3;
        }
    }
}


// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    EncTGSRepPart   ::= [APPLICATION 26] EncKDCRepPart
    */
    [Asn1Tag(Asn1TagType.Application, 26)]
    public class EncTGSRepPart : EncKDCRepPart
    {
        
        public EncTGSRepPart()
            : base()
        {
        }

        public EncTGSRepPart(
         EncryptionKey param0,
         LastReq param1,
         KerbUInt32 param2,
         KerberosTime param3,
         TicketFlags param4,
         KerberosTime param5,
         KerberosTime param6,
         KerberosTime param7,
         KerberosTime param8,
         Realm param9,
         PrincipalName param10,
         HostAddresses param11,
         Asn1SequenceOf<PA_DATA> param12)
        {
            this.key = param0;
            this.last_req = param1;
            this.nonce = param2;
            this.key_expiration = param3;
            this.flags = param4;
            this.authtime = param5;
            this.starttime = param6;
            this.endtime = param7;
            this.renew_till = param8;
            this.srealm = param9;
            this.sname = param10;
            this.caddr = param11;
            this.pa_datas = param12;
        }
    }
}


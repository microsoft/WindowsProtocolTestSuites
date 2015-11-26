// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    KRB-ERROR       ::= [APPLICATION 30] SEQUENCE {
        pvno            [0] INTEGER (5),
        msg-type        [1] INTEGER (30),
        ctime           [2] KerberosTime OPTIONAL,
        cusec           [3] Microseconds OPTIONAL,
        stime           [4] KerberosTime,

        susec           [5] Microseconds,
        error-code      [6] Int32,
        crealm          [7] Realm OPTIONAL,
        cname           [8] PrincipalName OPTIONAL,
        realm           [9] Realm -- service realm --,
        sname           [10] PrincipalName -- service name --,
        e-text          [11] KerberosString OPTIONAL,
        e-data          [12] OCTET STRING OPTIONAL
    }
    */
    [Asn1Tag(Asn1TagType.Application, 30)]
    public class KRB_ERROR : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public Asn1Integer pvno { get; set; }

        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Asn1Integer msg_type { get; set; }

        [Asn1Field(2, Optional = true), Asn1Tag(Asn1TagType.Context, 2)]
        public KerberosTime ctime { get; set; }

        [Asn1Field(3, Optional = true), Asn1Tag(Asn1TagType.Context, 3)]
        public Microseconds cusec { get; set; }

        [Asn1Field(4), Asn1Tag(Asn1TagType.Context, 4)]
        public KerberosTime stime { get; set; }

        [Asn1Field(5), Asn1Tag(Asn1TagType.Context, 5)]
        public Microseconds susec { get; set; }

        [Asn1Field(6), Asn1Tag(Asn1TagType.Context, 6)]
        public KerbInt32 error_code { get; set; }

        [Asn1Field(7, Optional = true), Asn1Tag(Asn1TagType.Context, 7)]
        public Realm crealm { get; set; }

        [Asn1Field(8, Optional = true), Asn1Tag(Asn1TagType.Context, 8)]
        public PrincipalName cname { get; set; }

        [Asn1Field(9), Asn1Tag(Asn1TagType.Context, 9)]
        public Realm realm { get; set; }

        [Asn1Field(10), Asn1Tag(Asn1TagType.Context, 10)]
        public PrincipalName sname { get; set; }

        [Asn1Field(11, Optional = true), Asn1Tag(Asn1TagType.Context, 11)]
        public KerberosString e_text { get; set; }

        [Asn1Field(12, Optional = true), Asn1Tag(Asn1TagType.Context, 12)]
        public Asn1OctetString e_data { get; set; }

        public KRB_ERROR()
        {
            this.pvno = null;
            this.msg_type = null;
            this.ctime = null;
            this.cusec = null;
            this.stime = null;
            this.susec = null;
            this.error_code = null;
            this.crealm = null;
            this.cname = null;
            this.realm = null;
            this.sname = null;
            this.e_text = null;
            this.e_data = null;
        }

        public KRB_ERROR(
         Asn1Integer param0,
         Asn1Integer param1,
         KerberosTime param2,
         Microseconds param3,
         KerberosTime param4,
         Microseconds param5,
         KerbInt32 param6,
         Realm param7,
         PrincipalName param8,
         Realm param9,
         PrincipalName param10,
         KerberosString param11,
         Asn1OctetString param12)
        {
            this.pvno = param0;
            this.msg_type = param1;
            this.ctime = param2;
            this.cusec = param3;
            this.stime = param4;
            this.susec = param5;
            this.error_code = param6;
            this.crealm = param7;
            this.cname = param8;
            this.realm = param9;
            this.sname = param10;
            this.e_text = param11;
            this.e_data = param12;
        }

        protected override bool VerifyConstraints()
        {
            return pvno != null && pvno.Value == 5 &&
                msg_type != null && msg_type.Value == 30;
        }
    }
}


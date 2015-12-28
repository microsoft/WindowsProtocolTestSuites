// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    -- Unencrypted authenticator
    Authenticator   ::= [APPLICATION 2] SEQUENCE  {
        authenticator-vno       [0] INTEGER (5),
        crealm                  [1] Realm,
        cname                   [2] PrincipalName,
        cksum                   [3] Checksum OPTIONAL,
        cusec                   [4] Microseconds,
        ctime                   [5] KerberosTime,
        subkey                  [6] EncryptionKey OPTIONAL,
        seq-number              [7] UInt32 OPTIONAL,
        authorization-data      [8] AuthorizationData OPTIONAL
    }
    */
    [Asn1Tag(Asn1TagType.Application, 2)]
    public class Authenticator : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public Asn1Integer authenticator_vno { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Realm crealm { get; set; }
        
        [Asn1Field(2), Asn1Tag(Asn1TagType.Context, 2)]
        public PrincipalName cname { get; set; }
        
        [Asn1Field(3, Optional = true), Asn1Tag(Asn1TagType.Context, 3)]
        public Checksum cksum { get; set; }
        
        [Asn1Field(4), Asn1Tag(Asn1TagType.Context, 4)]
        public Microseconds cusec { get; set; }
        
        [Asn1Field(5), Asn1Tag(Asn1TagType.Context, 5)]
        public KerberosTime ctime { get; set; }
        
        [Asn1Field(6, Optional = true), Asn1Tag(Asn1TagType.Context, 6)]
        public EncryptionKey subkey { get; set; }
        
        [Asn1Field(7, Optional = true), Asn1Tag(Asn1TagType.Context, 7)]
        public KerbUInt32 seq_number { get; set; }
        
        [Asn1Field(8, Optional = true), Asn1Tag(Asn1TagType.Context, 8)]
        public AuthorizationData authorization_data { get; set; }
        
        public Authenticator()
        {
            this.authenticator_vno = null;
            this.crealm = null;
            this.cname = null;
            this.cksum = null;
            this.cusec = null;
            this.ctime = null;
            this.subkey = null;
            this.seq_number = null;
            this.authorization_data = null;
        }
        
        public Authenticator(
         Asn1Integer param0,
         Realm param1,
         PrincipalName param2,
         Checksum param3,
         Microseconds param4,
         KerberosTime param5,
         EncryptionKey param6,
         KerbUInt32 param7,
         AuthorizationData param8)
        {
            this.authenticator_vno = param0;
            this.crealm = param1;
            this.cname = param2;
            this.cksum = param3;
            this.cusec = param4;
            this.ctime = param5;
            this.subkey = param6;
            this.seq_number = param7;
            this.authorization_data = param8;
        }

        protected override bool VerifyConstraints()
        {
            return authenticator_vno != null && authenticator_vno.Value == 5;
        }
    }
}


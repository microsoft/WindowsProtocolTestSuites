// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    EncAPRepPart    ::= [APPLICATION 27] SEQUENCE {
        ctime           [0] KerberosTime,
        cusec           [1] Microseconds,
        subkey          [2] EncryptionKey OPTIONAL,
        seq-number      [3] UInt32 OPTIONAL
    }
    */
    [Asn1Tag(Asn1TagType.Application, 27)]
    public class EncAPRepPart : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public KerberosTime ctime { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Microseconds cusec { get; set; }
        
        [Asn1Field(2, Optional = true), Asn1Tag(Asn1TagType.Context, 2)]
        public EncryptionKey subkey { get; set; }
        
        [Asn1Field(3, Optional = true), Asn1Tag(Asn1TagType.Context, 3)]
        public KerbUInt32 seq_number { get; set; }
        
        public EncAPRepPart()
        {
            this.ctime = null;
            this.cusec = null;
            this.subkey = null;
            this.seq_number = null;
        }
        
        public EncAPRepPart(
         KerberosTime param0,
         Microseconds param1,
         EncryptionKey param2,
         KerbUInt32 param3)
        {
            this.ctime = param0;
            this.cusec = param1;
            this.subkey = param2;
            this.seq_number = param3;
        }
    }
}


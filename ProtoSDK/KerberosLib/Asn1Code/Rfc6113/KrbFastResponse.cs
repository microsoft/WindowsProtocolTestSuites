// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth 
{
    /*
    KrbFastResponse ::= SEQUENCE {
          padata         [0] SEQUENCE OF PA-DATA,
              -- padata typed holes.
          strengthen-key [1] EncryptionKey OPTIONAL,
              -- This, if present, strengthens the reply key for AS and
              -- TGS.  MUST be present for TGS
              -- MUST be absent in KRB-ERROR.
          finished       [2] KrbFastFinished OPTIONAL,
              -- Present in AS or TGS reply; absent otherwise.
          nonce          [3] UInt32,
              -- Nonce from the client request.
          ...
      }
    */
    public class KrbFastResponse : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public Asn1SequenceOf<PA_DATA> padata { get; set; }
        
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public EncryptionKey strengthen_key { get; set; }
        
        [Asn1Field(2, Optional = true), Asn1Tag(Asn1TagType.Context, 2)]
        public KrbFastFinished finished { get; set; }
        
        [Asn1Field(3), Asn1Tag(Asn1TagType.Context, 3)]
        public KerbUInt32 nonce { get; set; }
        
        public KrbFastResponse()
        {
            this.padata = null;
            this.strengthen_key = null;
            this.finished = null;
            this.nonce = null;
        }
        
        public KrbFastResponse(
         Asn1SequenceOf<PA_DATA> param0,
         EncryptionKey param1,
         KrbFastFinished param2,
         KerbUInt32 param3)
        {
            this.padata = param0;
            this.strengthen_key = param1;
            this.finished = param2;
            this.nonce = param3;
        }
    }
}


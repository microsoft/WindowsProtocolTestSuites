// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth 
{
    /*
    KrbFastArmoredReq ::= SEQUENCE {
          armor        [0] KrbFastArmor OPTIONAL,
              -- Contains the armor that identifies the armor key.
              -- MUST be present in AS-REQ.
          req-checksum [1] Checksum,
              -- For AS, contains the checksum performed over the type
              -- KDC-REQ-BODY for the req-body field of the KDC-REQ
              -- structure;
              -- For TGS, contains the checksum performed over the type
              -- AP-REQ in the PA-TGS-REQ padata.
              -- The checksum key is the armor key, the checksum
              -- type is the required checksum type for the enctype of
              -- the armor key, and the key usage number is
              -- KEY_USAGE_FAST_REQ_CHKSUM.
          enc-fast-req [2] EncryptedData, -- KrbFastReq --
              -- The encryption key is the armor key, and the key usage
              -- number is KEY_USAGE_FAST_ENC.
          ...
      }
    */
    public class KrbFastArmoredReq : Asn1Sequence
    {
        [Asn1Field(0, Optional = true), Asn1Tag(Asn1TagType.Context, 0)]
        public KrbFastArmor armor { get; set; }
        
        [Asn1Field(1), Asn1Tag(Asn1TagType.Context, 1)]
        public Checksum req_checksum { get; set; }
        
        [Asn1Field(2), Asn1Tag(Asn1TagType.Context, 2)]
        public EncryptedData enc_fast_req { get; set; }
        
        public KrbFastArmoredReq()
        {
            this.armor = null;
            this.req_checksum = null;
            this.enc_fast_req = null;
        }
        
        public KrbFastArmoredReq(
         KrbFastArmor param0,
         Checksum param1,
         EncryptedData param2)
        {
            this.armor = param0;
            this.req_checksum = param1;
            this.enc_fast_req = param2;
        }
    }
}


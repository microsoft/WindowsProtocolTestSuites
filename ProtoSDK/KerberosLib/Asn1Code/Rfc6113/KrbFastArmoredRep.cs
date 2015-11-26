// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth 
{
    /*
    KrbFastArmoredRep ::= SEQUENCE {
          enc-fast-rep      [0] EncryptedData, -- KrbFastResponse --
              -- The encryption key is the armor key in the request, and
              -- the key usage number is KEY_USAGE_FAST_REP.
          ...
      }
    */
    public class KrbFastArmoredRep : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public EncryptedData enc_fast_rep { get; set; }
        
        public KrbFastArmoredRep()
        {
            this.enc_fast_rep = null;
        }
        
        public KrbFastArmoredRep(
         EncryptedData param0)
        {
            this.enc_fast_rep = param0;
        }
    }
}


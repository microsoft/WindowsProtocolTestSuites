// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosV5.Preauth 
{
    /*
    EncryptedChallenge ::= EncryptedData
              -- Encrypted PA-ENC-TS-ENC, encrypted in the challenge key
              -- using key usage KEY_USAGE_ENC_CHALLENGE_CLIENT for the
              -- client and KEY_USAGE_ENC_CHALLENGE_KDC for the KDC.
    */
    public class EncryptedChallenge : EncryptedData
    {
        
        public EncryptedChallenge()
            : base()
        {
        }
        
        public EncryptedChallenge(
         KerbInt32 param0,
         KerbInt32 param1,
         Asn1OctetString param2)
        {
            this.etype = param0;
            this.kvno = param1;
            this.cipher = param2;
        }
    }
}


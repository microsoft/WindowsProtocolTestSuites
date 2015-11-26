// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    PA-ENC-TIMESTAMP        ::= EncryptedData -- PA-ENC-TS-ENC
    */
    public class PA_ENC_TIMESTAMP : EncryptedData
    {
        
        public PA_ENC_TIMESTAMP()
            : base()
        {
        }

        public PA_ENC_TIMESTAMP(
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

